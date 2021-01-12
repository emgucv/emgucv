//----------------------------------------------------------------------------
//  Copyright (C) 2004-2021 by EMGU Corporation. All rights reserved.       
//----------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Emgu.CV.Structure;
using Emgu.CV.Util;
using Emgu.Util;

namespace Emgu.CV.Cuda
{
    /// <summary>
    /// Farneback optical flow
    /// </summary>
    public class CudaFarnebackOpticalFlow : SharedPtrObject, ICudaDenseOpticalFlow
    {
        private IntPtr _denseFlow;
        private IntPtr _algorithm;

        /// <summary>
        /// Create a CudaFarnebackOpticalFlow object
        /// </summary>
        /// <param name="pyrScale">Specifies the image scale (!1) to build the pyramids for each image. pyrScale=0.5 means the classical pyramid, where each next layer is twice smaller than the previous</param>
        /// <param name="numLevels">The number of pyramid layers, including the initial image. levels=1 means that no extra layers are created and only the original images are used</param>
        /// <param name="winSize">The averaging window size; The larger values increase the algorithm robustness to image noise and give more chances for fast motion detection, but yield more blurred motion field</param>
        /// <param name="numIters">The number of iterations the algorithm does at each pyramid level</param>
        /// <param name="polyN">Size of the pixel neighborhood used to find polynomial expansion in each pixel. The larger values mean that the image will be approximated with smoother surfaces, yielding more robust algorithm and more blurred motion field. Typically, poly n=5 or 7</param>
        /// <param name="polySigma">Standard deviation of the Gaussian that is used to smooth derivatives that are used as a basis for the polynomial expansion. For poly n=5 you can set poly sigma=1.1, for poly n=7 a good value would be poly sigma=1.5</param>
        /// <param name="flags">The operation flags</param>
        /// <param name="fastPyramids">Fast Pyramids</param>
        public CudaFarnebackOpticalFlow(
           int numLevels = 5,
           double pyrScale = 0.5,
           bool fastPyramids = false,
           int winSize = 13,
           int numIters = 10,
           int polyN = 5,
           double polySigma = 1.1,
           CvEnum.OpticalflowFarnebackFlag flags = 0)
        {
            _ptr = CudaInvoke.cudaFarnebackOpticalFlowCreate(
                numLevels, 
                pyrScale, 
                fastPyramids, 
                winSize, 
                numIters, 
                polyN, 
                polySigma, 
                flags, 
                ref _denseFlow, 
                ref _algorithm,
                ref _sharedPtr);
        }

        /// <summary>
        /// Release all the unmanaged memory associated with this optical flow solver.
        /// </summary>
        protected override void DisposeObject()
        {
            if (_sharedPtr != IntPtr.Zero)
            {
                CudaInvoke.cudaFarnebackOpticalFlowRelease(ref _sharedPtr);
                _ptr = IntPtr.Zero;
                _denseFlow = IntPtr.Zero;
                _algorithm = IntPtr.Zero;
            }
        }

        /// <summary>
        /// Pointer to the unmanaged DenseOpticalFlow object
        /// </summary>
        public IntPtr DenseOpticalFlowPtr
        {
            get { return _denseFlow; }
        }

        /// <summary>
        /// Pointer to the unamanged Algorithm object
        /// </summary>
        public IntPtr AlgorithmPtr
        {
            get { return _algorithm; } 
        }
    }

    public static partial class CudaInvoke
    {
        [DllImport(CvInvoke.ExternCudaLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal extern static IntPtr cudaFarnebackOpticalFlowCreate(
           int numLevels,
           double pyrScale,
           [MarshalAs(CvInvoke.BoolMarshalType)]
           bool fastPyramids,
           int winSize,
           int numIters,
           int polyN,
           double polySigma,
           CvEnum.OpticalflowFarnebackFlag flags,
           ref IntPtr denseFlow,
           ref IntPtr algorithm,
           ref IntPtr sharedPtr);

        [DllImport(CvInvoke.ExternCudaLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal extern static void cudaFarnebackOpticalFlowRelease(ref IntPtr flow);

    }
}
