//----------------------------------------------------------------------------
//  Copyright (C) 2004-2020 by EMGU Corporation. All rights reserved.       
//----------------------------------------------------------------------------

using System;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using Emgu.CV;
using Emgu.CV.Structure;
using Emgu.CV.Util;
using Emgu.Util;

namespace Emgu.CV.Face
{
    /// <summary>
    /// Minimum Average Correlation Energy Filter useful for authentication with (cancellable) biometrical features. (does not need many positives to train (10-50), and no negatives at all, also robust to noise/salting)
    /// </summary>
    public class MACE : SharedPtrObject
    {
        /// <summary>
        /// Create a new MACE object
        /// </summary>
        /// <param name="imgSize">images will get resized to this (should be an even number)</param>
        public MACE(int imgSize)
        {
            _ptr = FaceInvoke.cveMaceCreate(imgSize, ref _sharedPtr);
        }

        /// <summary>
        /// optionally encrypt images with random convolution
        /// </summary>
        /// <param name="passphrase">a crc64 random seed will get generated from this</param>
        public void Salt(String passphrase)
        {
            using (CvString csPassphrase = new CvString(passphrase))
            {
                FaceInvoke.cveMaceSalt(_ptr, csPassphrase);
            }
        }

        /// <summary>
        /// train it on positive features compute the mace filter: h = D(-1) * X * (X(+) * D(-1) * X)(-1) * C also calculate a minimal threshold for this class, the smallest self-similarity from the train images
        /// </summary>
        /// <param name="images">A VectorOfMat with the train images</param>
        public void Train(IInputArrayOfArrays images)
        {
            using (InputArray iaImages = images.GetInputArray())
            {
                FaceInvoke.cveMaceTrain(_ptr, iaImages);
            }
        }

        /// <summary>
        /// correlate query img and threshold to min class value
        /// </summary>
        /// <param name="query">a Mat with query image</param>
        /// <returns>True if the query is the same</returns>
        public bool Same(IInputArray query)
        {
            using (InputArray iaQuery = query.GetInputArray())
            {
                return FaceInvoke.cveMaceSame(_ptr, iaQuery);
            }
        }
        /// <summary>
        /// Release the unmanaged memory associated with this BIF
        /// </summary>
        protected override void DisposeObject()
        {
            if (_sharedPtr != IntPtr.Zero)
            {
                FaceInvoke.cveMaceRelease(ref _sharedPtr);
                _ptr = IntPtr.Zero;
            }
        }
    }

    public static partial class FaceInvoke
    {
        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal extern static IntPtr  cveMaceCreate(int imgSize, ref IntPtr sharedPtr);

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal extern static void cveMaceSalt(IntPtr mace, IntPtr passphrase);
        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal extern static void cveMaceTrain(IntPtr mace, IntPtr images);
        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        [return: MarshalAs(CvInvoke.BoolMarshalType)]
        internal extern static bool cveMaceSame(IntPtr mace, IntPtr query);

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal extern static void cveMaceRelease(ref IntPtr sharedPtr);
    }
}