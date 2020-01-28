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
    /// Implementation of bio-inspired features (BIF) from the paper: Guo, Guodong, et al. "Human age estimation using bio-inspired features." Computer Vision and Pattern Recognition, 2009. CVPR 2009.
    /// </summary>
    public class BIF : SharedPtrObject
    {
        /// <summary>
        /// Create an instance of bio-inspired features
        /// </summary>
        /// <param name="numBands">The number of filter bands used for computing BIF.</param>
        /// <param name="numRotations">The number of image rotations.</param>
        public BIF(int numBands, int numRotations)
        {
            _ptr = FaceInvoke.cveBIFCreate(numBands, numRotations, ref _sharedPtr);
        }

        /// <summary>
        /// Computes features by input image.
        /// </summary>
        /// <param name="image">Input image (CV_32FC1)</param>
        /// <param name="features">Feature vector (CV_32FC1)</param>
        public void Compute(IInputArray image, IOutputArray features)
        {
            using (InputArray iaImage = image.GetInputArray())
            using (OutputArray oaFeatures = features.GetOutputArray())
                FaceInvoke.cveBIFCompute(_ptr, iaImage, oaFeatures);
        }

        /// <summary>
        /// Release the unmanaged memory associated with this BIF
        /// </summary>
        protected override void DisposeObject()
        {
            if (_sharedPtr != IntPtr.Zero)
            {
                FaceInvoke.cveBIFRelease(ref _sharedPtr);
                _ptr = IntPtr.Zero;
            }
        }
    }

    public static partial class FaceInvoke
    {
        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal extern static IntPtr cveBIFCreate(int numBands, int numRotations, ref IntPtr sharedPtr);

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal extern static void cveBIFCompute(IntPtr bif, IntPtr image, IntPtr features);

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal extern static void cveBIFRelease(ref IntPtr sharedPtr);
    }
}