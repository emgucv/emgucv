//----------------------------------------------------------------------------
//  Copyright (C) 2004-2017 by EMGU Corporation. All rights reserved.       
//----------------------------------------------------------------------------

/*
#if !(__IOS__ || UNITY_IPHONE || NETFX_CORE || NETSTANDARD1_4)
using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using Emgu.CV.Structure;
using Emgu.CV.Text;
using Emgu.CV.Util;
using Emgu.Util;
using System.Diagnostics;

namespace Emgu.CV.Dnn
{
    /// <summary>
    /// This class provides methods for continuous n-dimensional CPU and GPU array processing.
    /// </summary>
    public class Blob : UnmanagedObject
    {
        internal Blob(IntPtr blobPtr)
        {
            _ptr = blobPtr;
        }

        /// <summary>
        /// Constructs 4-dimensional blob (so-called batch) from image or array of images.
        /// </summary>
        /// <param name="image">2-dimensional multi-channel or 3-dimensional single-channel image (or array of images)</param>
        public Blob(IInputArray image)
        {
            using (InputArray iaImage = image.GetInputArray())
                _ptr = DnnInvoke.cveDnnBlobCreateFromInputArray(iaImage);
        }

        /// <summary>
        /// Create an empty Blob
        /// </summary>
        public Blob()
        {
            _ptr = DnnInvoke.cveDnnBlobCreate();
        }

        /// <summary>
        /// Returns reference to Mat, containing blob data.
        /// </summary>
        /// <returns>Reference to Mat, containing blob data.</returns>
        public Mat MatRef()
        {
            Mat m = new Mat();
            DnnInvoke.cveDnnBlobMatRef(_ptr, m);
            return m;
        }

        /// <summary>
        /// Release the unmanaged memory associated with this Blob
        /// </summary>
        protected override void DisposeObject()
        {
            if (_ptr != IntPtr.Zero)
            {
                DnnInvoke.cveDnnBlobRelease(ref _ptr);
            }
        }

        /// <summary>
        /// Returns number of blob dimensions.
        /// </summary>
        public int Dims
        {
            get { return DnnInvoke.cveDnnBlobDims(_ptr); }
        }

        /// <summary>
        /// Returns size of the second axis blob.
        /// </summary>
        public int Channels
        {
            get { return DnnInvoke.cveDnnBlobChannels(_ptr); }
        }

        /// <summary>
        /// Returns size of the fourth axis blob.
        /// </summary>
        public int Cols
        {
            get { return DnnInvoke.cveDnnBlobCols(_ptr); }
        }

        /// <summary>
        /// Returns size of the first axis blob.
        /// </summary>
        public int Num
        {
            get { return DnnInvoke.cveDnnBlobNum(_ptr); }
        }

        /// <summary>
        /// Returns size of the third axis blob. 
        /// </summary>
        public int Rows
        {
            get { return DnnInvoke.cveDnnBlobRows(_ptr); }
        }

        /// <summary>
        /// Returns size of single element in bytes.
        /// </summary>
        public int ElemSize
        {
            get { return DnnInvoke.cveDnnBlobElemSize(_ptr); }
        }

        /// <summary>
        /// Returns type of the blob.
        /// </summary>
        public CvEnum.DepthType Type
        {
            get { return DnnInvoke.cveDnnBlobType(_ptr); }
        }

        /// <summary>
        ///  Constructs 4-dimensional blob (so-called batch) from image or array of images.
        /// </summary>
        /// <param name="image">2-dimensional multi-channel or 3-dimensional single-channel image (or array of such images)</param>
        /// <param name="dstCn">specifies size of second axis of output blob</param>
        public void BatchFromImages(IInputArray image, int dstCn = -1)
        {
            using (InputArray iaImage = image.GetInputArray())
            DnnInvoke.cveDnnBlobBatchFromImages(_ptr, iaImage, dstCn);
        }

        /// <summary>
        /// Returns pointer to the blob element with the specified position, stored in CPU memory.
        /// </summary>
        /// <param name="n">correspond to the first axis</param>
        /// <param name="cn">correspond to the second axis</param>
        /// <param name="row">correspond to the 3rd axis</param>
        /// <param name="col">correspond to the 4th axis</param>
        /// <returns></returns>
        public IntPtr GetPtr(int n = 0, int cn = 0, int row = 0, int col = 0)
        {
            return DnnInvoke.cveDnnBlobGetPtr(_ptr, n, cn, row, col);
        }
    }

    /// <summary>
    /// Provide interfaces to the Open CV DNN functions
    /// </summary>
    public static partial class DnnInvoke
    {
        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern IntPtr cveDnnBlobCreate();
        
        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern IntPtr cveDnnBlobCreateFromInputArray(IntPtr image);
        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern void cveDnnBlobMatRef(IntPtr blob, IntPtr outMat);
        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern void cveDnnBlobRelease(ref IntPtr blob);

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern void cveDnnBlobBatchFromImages(IntPtr blob, IntPtr image, int dstCn);

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern int cveDnnBlobDims(IntPtr blob);
        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern int cveDnnBlobChannels(IntPtr blob);
        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern int cveDnnBlobCols(IntPtr blob);
        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern int cveDnnBlobNum(IntPtr blob);
        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern int cveDnnBlobRows(IntPtr blob);

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern CvEnum.DepthType cveDnnBlobType(IntPtr blob);
        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern int cveDnnBlobElemSize(IntPtr blob);
        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern IntPtr cveDnnBlobGetPtr(IntPtr blob, int n, int cn, int row, int col);
    }
}

#endif

*/