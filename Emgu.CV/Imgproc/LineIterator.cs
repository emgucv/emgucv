//----------------------------------------------------------------------------
//  Copyright (C) 2004-2021 by EMGU Corporation. All rights reserved.       
//----------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Runtime.InteropServices;
using Emgu.CV.Structure;
using Emgu.CV.Util;
using Emgu.Util;

namespace Emgu.CV
{
    /// <summary>
    /// The class is used to iterate over all the pixels on the raster line segment connecting two specified points.
    /// </summary>
    public partial class LineIterator : UnmanagedObject
    {
        private Mat _mat = null;
        private Type _type = null;
        private int _elementLength = 0;
        private int _byteSize = 0;

        #region constructor

        /// <summary>
        /// Create a LineIterator that can be used to get each pixel of a raster line.  The line will be clipped on the image boundaries
        /// </summary>
        /// <param name="img">The image</param>
        /// <param name="p1">The first point</param>
        /// <param name="p2">The second point</param>
        /// <param name="connectivity">8-connected or 4-connected</param>
        /// <param name="leftToRight">If true, then the iteration is always done from the left-most point to the right most, not to depend on the ordering of pt1 and pt2 parameters</param>
        public LineIterator(Mat img, Point p1, Point p2, int connectivity = 8, bool leftToRight = false)
        {
            _mat = img;
            _ptr = CvInvoke.cveLineIteratorCreate(img, ref p1, ref p2, connectivity, leftToRight);
        }

        #endregion

        /// <summary>
        /// Native pointer to the pixel data at the current position
        /// </summary>
        public IntPtr DataPtr
        {
            get { return CvInvoke.cveLineIteratorGetDataPointer(_ptr); }
        }

        /// <summary>
        /// Get or set the pixel data in the current position
        /// </summary>
        public Array Data
        {
            get
            {
                if (_mat.IsEmpty)
                    return null;

                if (_type == null)
                {
                    _type = CvInvoke.GetDepthType(_mat.Depth);
                    if (_type == null)
                        return null;
                }

                if (_byteSize <= 0)
                {
                    _byteSize = _mat.ElementSize;
                }

                if (_elementLength <= 0)
                {
                    _elementLength = _byteSize / Marshal.SizeOf(_type);
                }

                Array array = Array.CreateInstance(_type, _elementLength);

                GCHandle handle = GCHandle.Alloc(array, GCHandleType.Pinned);
                CvInvoke.cveMemcpy(handle.AddrOfPinnedObject(), DataPtr, _byteSize);
                handle.Free();
                return array;
            }
            set
            {
                if (_type == null)
                {
                    _type = CvInvoke.GetDepthType(_mat.Depth);
                    if (_type == null)
                        return;
                }

                if (_byteSize <= 0)
                {
                    _byteSize = _mat.ElementSize;
                }

                if (_elementLength <= 0)
                {
                    _elementLength = _byteSize / Marshal.SizeOf(_type);
                }

                if (value.Length != _elementLength)
                    throw new ArgumentException("Input data size do not match Mat element size");
                
                if (_type != value.GetValue(0).GetType())
                    throw new ArgumentException("Input data type do not match Mat element type");

                GCHandle handle = GCHandle.Alloc(value, GCHandleType.Pinned);
                CvInvoke.cveMemcpy( DataPtr, handle.AddrOfPinnedObject(), _byteSize);
                handle.Free();
            }

        }

        /// <summary>
        ///  returns the current position in the image
        /// </summary>
        public Point Pos
        {
            get
            {
                Point p = new Point();
                CvInvoke.cveLineIteratorPos(_ptr, ref p);
                return p;
            }
        }

        /// <summary>
        /// Move to the next point
        /// </summary>
        public void MoveNext()
        {
            CvInvoke.cveLineIteratorMoveNext(_ptr);
        }

        /// <summary>
        /// Release unmanaged resources
        /// </summary>
        protected override void DisposeObject()
        {
            if (_ptr != IntPtr.Zero)
                CvInvoke.cveLineIteratorRelease(ref _ptr);

            _mat = null;
            _type = null;
            _byteSize = 0;
            _elementLength = 0;
        }

        /// <summary>
        /// Copy the pixel data in the line to a new Mat that is of the same type. 
        /// </summary>
        /// <param name="img">The image to sample from</param>
        /// <param name="p1">The first point</param>
        /// <param name="p2">The second point</param>
        /// <param name="connectivity">8-connected or 4-connected</param>
        /// <param name="leftToRight">If true, then the iteration is always done from the left-most point to the right most, not to depend on the ordering of pt1 and pt2 parameters</param>
        /// <returns>A new single column Mat of the same data type. The number of rows equals to the number of points on the sample line.</returns>
        public static Mat SampleLine(Mat img, Point p1, Point p2, int connectivity = 8, bool leftToRight = false)
        {
            Mat m = new Mat();
            CvInvoke.cveLineIteratorSampleLine(img, ref p1, ref p2, connectivity, leftToRight, m);
            return m;
        }
}

    public static partial class CvInvoke
    {
        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern IntPtr cveLineIteratorCreate(
            IntPtr img,
            ref Point pt1,
            ref Point pt2,
            int connectivity,
            [MarshalAs(CvInvoke.BoolMarshalType)]
            bool leftToRight);

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern void cveLineIteratorRelease(ref IntPtr iterator);

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern IntPtr cveLineIteratorGetDataPointer(IntPtr iterator);
        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern void cveLineIteratorPos(IntPtr iterator, ref Point pos);
        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern void cveLineIteratorMoveNext(IntPtr iterator);

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern void cveLineIteratorSampleLine(
            IntPtr img,
            ref Point pt1,
            ref Point pt2,
            int connectivity,
            [MarshalAs(CvInvoke.BoolMarshalType)]
            bool leftToRight, 
            IntPtr line);

    }
}
