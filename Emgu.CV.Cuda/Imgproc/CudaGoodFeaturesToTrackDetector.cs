//----------------------------------------------------------------------------
//  Copyright (C) 2004-2021 by EMGU Corporation. All rights reserved.       
//----------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Runtime.InteropServices;
using Emgu.CV.CvEnum;
using Emgu.CV.Util;
using Emgu.Util;

namespace Emgu.CV.Cuda
{
    /// <summary>
    /// Cuda implementation of GoodFeaturesToTrackDetector
    /// </summary>
    public class CudaGoodFeaturesToTrackDetector : SharedPtrObject
    {

        /// <summary>
        /// Create the Cuda implementation of GoodFeaturesToTrackDetector
        /// </summary>
        /// <param name="srcDepth">The depth of the src image</param>
        /// <param name="srcChannels">The number of channels in the src image</param>
        /// <param name="maxCorners">The maximum number of channels</param>
        /// <param name="qualityLevel">The quality level</param>
        /// <param name="minDistance">The minimum distance</param>
        /// <param name="blockSize">The block size</param>
        /// <param name="useHarrisDetector">If true, use Harris detector</param>
        /// <param name="harrisK">Harris K</param>
        public CudaGoodFeaturesToTrackDetector(
            DepthType srcDepth, 
            int srcChannels, 
            int maxCorners = 1000, 
            double qualityLevel = 0.01, 
            double minDistance = 0, 
            int blockSize = 3, 
            bool useHarrisDetector = false, 
            double harrisK = 0.04)
        {
            _ptr = CudaInvoke.cudaGoodFeaturesToTrackDetectorCreate(CvInvoke.MakeType(srcDepth, srcChannels), maxCorners, qualityLevel, minDistance, blockSize, useHarrisDetector, harrisK, ref _sharedPtr);
        }

        /// <summary>
        /// Find the good features to track
        /// </summary>
        /// <param name="image">The input image</param>
        /// <param name="corners">The output corners</param>
        /// <param name="mask">Optional mask</param>
        /// <param name="stream">Use a Stream to call the function asynchronously (non-blocking) or null to call the function synchronously (blocking).</param>
        public void Detect(IInputArray image, IOutputArray corners, IInputArray mask = null, Stream stream = null)
        {
            using (InputArray iaImage = image.GetInputArray())
            using (OutputArray oaCorners = corners.GetOutputArray())
            using (InputArray iaMask = (mask != null ? mask.GetInputArray() : InputArray.GetEmpty()))
                CudaInvoke.cudaCornersDetectorDetect(_ptr, iaImage, oaCorners, iaMask, stream);
        }

        /// <summary>
        /// Release all the unmanaged memory associated with this detector
        /// </summary>
        protected override void DisposeObject()
        {
            if (IntPtr.Zero != _sharedPtr)
            {
                CudaInvoke.cudaCornersDetectorRelease(ref _sharedPtr);
                _ptr = IntPtr.Zero;
            }
        }
    }

    public static partial class CudaInvoke
    {
        [DllImport(CvInvoke.ExternCudaLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal extern static IntPtr cudaGoodFeaturesToTrackDetectorCreate(
            int srcType, 
            int maxCorners, 
            double qualityLevel, 
            double minDistance, 
            int blockSize,
            [MarshalAs(CvInvoke.BoolMarshalType)]
            bool useHarrisDetector,
            double harrisK,
            ref IntPtr _sharedPtr);

        [DllImport(CvInvoke.ExternCudaLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal extern static void cudaCornersDetectorDetect(IntPtr detector, IntPtr image, IntPtr corners, IntPtr mask, IntPtr stream);

        [DllImport(CvInvoke.ExternCudaLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal extern static void cudaCornersDetectorRelease(ref IntPtr detector);
    }
}
