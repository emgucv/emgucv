//----------------------------------------------------------------------------
//  Copyright (C) 2004-2021 by EMGU Corporation. All rights reserved.       
//----------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;
using Emgu.CV.Features2D;
using Emgu.CV.Structure;
using Emgu.CV.Util;
using Emgu.Util;

namespace Emgu.CV.Cuda
{
    /// <summary>
    /// A FAST detector using Cuda
    /// </summary>
    public class CudaFastFeatureDetector : FastFeatureDetector, IFeature2DAsync
    {
        private IntPtr _feature2DAsyncPtr;

        /// <summary>
        /// Create a fast detector with the specific parameters
        /// </summary>
        /// <param name="threshold">Threshold on difference between intensity of center pixel and pixels on circle around
        /// this pixel. Use 10 for default.</param>
        /// <param name="nonmaxSupression">Specify if non-maximum supression should be used.</param>
        /// <param name="maxNKeypoints">The maximum number of keypoints to be extracted.</param>
        /// <param name="type">The detector type</param>
        public CudaFastFeatureDetector(
            int threshold = 10, 
            bool nonmaxSupression = true, 
            FastFeatureDetector.DetectorType type = DetectorType.Type9_16, 
            int maxNKeypoints = 5000)
        {
            _ptr = CudaInvoke.cveCudaFastFeatureDetectorCreate(threshold, nonmaxSupression, type, maxNKeypoints, ref _feature2D, ref _feature2DAsyncPtr, ref _sharedPtr);
        }


        /// <summary>
        /// Release the unmanaged resource associate to the Detector
        /// </summary>
        protected override void DisposeObject()
        {
            if (_ptr != IntPtr.Zero)
            {
                CudaInvoke.cveCudaFastFeatureDetectorRelease(ref _sharedPtr);
            }
            _feature2DAsyncPtr = IntPtr.Zero;
            base.DisposeObject();
        }

        IntPtr IFeature2DAsync.Feature2DAsyncPtr
        {
            get { return _feature2DAsyncPtr; }
        }
    }

    public static partial class CudaInvoke
    {
        [DllImport(CvInvoke.ExternCudaLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern IntPtr cveCudaFastFeatureDetectorCreate(
           int threshold,
           [MarshalAs(CvInvoke.BoolMarshalType)]
           bool nonmaxSupression,
           FastFeatureDetector.DetectorType type,
           int maxPoints,
           ref IntPtr feature2D,
           ref IntPtr feature2DAsync, 
           ref IntPtr sharedPtr);

        [DllImport(CvInvoke.ExternCudaLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern void cveCudaFastFeatureDetectorRelease(ref IntPtr sharedPtr);

    }
}
