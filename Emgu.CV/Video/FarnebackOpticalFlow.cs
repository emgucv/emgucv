//----------------------------------------------------------------------------
//  Copyright (C) 2004-2021 by EMGU Corporation. All rights reserved.       
//----------------------------------------------------------------------------

using System;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Runtime.InteropServices;
using Emgu.CV.Structure;
using Emgu.Util;

namespace Emgu.CV
{
    /// <summary>
    /// Class computing a dense optical flow using the Gunnar Farneback's algorithm.
    /// </summary>
    public partial class FarnebackOpticalFlow : UnmanagedObject, IDenseOpticalFlow
    {
        private IntPtr _sharedPtr;

        private IntPtr _algorithm;
        private IntPtr _denseOpticalFlow;

        /// <summary>
        /// Create a FarnebackOpticalFlow object
        /// </summary>
        /// <param name="pyrScale">Specifies the image scale (!1) to build the pyramids for each image. pyrScale=0.5 means the classical pyramid, where each next layer is twice smaller than the previous</param>
        /// <param name="numLevels">The number of pyramid layers, including the initial image. levels=1 means that no extra layers are created and only the original images are used</param>
        /// <param name="winSize">The averaging window size; The larger values increase the algorithm robustness to image noise and give more chances for fast motion detection, but yield more blurred motion field</param>
        /// <param name="numIters">The number of iterations the algorithm does at each pyramid level</param>
        /// <param name="polyN">Size of the pixel neighborhood used to find polynomial expansion in each pixel. The larger values mean that the image will be approximated with smoother surfaces, yielding more robust algorithm and more blurred motion field. Typically, poly n=5 or 7</param>
        /// <param name="polySigma">Standard deviation of the Gaussian that is used to smooth derivatives that are used as a basis for the polynomial expansion. For poly n=5 you can set poly sigma=1.1, for poly n=7 a good value would be poly sigma=1.5</param>
        /// <param name="flags">The operation flags</param>
        /// <param name="fastPyramids">Fast Pyramids</param>
        public FarnebackOpticalFlow(
            int numLevels = 5,
            double pyrScale = 0.5,
            bool fastPyramids = false,
            int winSize = 13,
            int numIters = 10,
            int polyN = 5,
            double polySigma = 1.1,
            CvEnum.OpticalflowFarnebackFlag flags = CvEnum.OpticalflowFarnebackFlag.Default)
        {
            _ptr = CvInvoke.cveFarnebackOpticalFlowCreate(
                numLevels,
                pyrScale, fastPyramids, winSize, numIters, polyN, polySigma,
                flags,
                ref _denseOpticalFlow, ref _algorithm,
                ref _sharedPtr);
        }

        /// <summary>
        /// Release the unmanaged resources
        /// </summary>
        protected override void DisposeObject()
        {
            if (_ptr != IntPtr.Zero)
            {
                CvInvoke.cveFarnebackOpticalFlowRelease(ref _ptr, ref _sharedPtr);
                _algorithm = IntPtr.Zero;
                _denseOpticalFlow = IntPtr.Zero;
            }
        }

        /// <summary>
        /// Gets the dense optical flow pointer.
        /// </summary>
        /// <value>
        /// The pointer to the dense optical flow object.
        /// </value>
        public IntPtr DenseOpticalFlowPtr
        {
            get { return _denseOpticalFlow; }
        }

        /// <summary>
        /// Return the pointer to the algorithm object
        /// </summary>
        public IntPtr AlgorithmPtr
        {
            get { return _algorithm; }
        }
    }

    public static partial class CvInvoke
    {

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern void cveFarnebackOpticalFlowRelease(ref IntPtr flow, ref IntPtr sharedPtr);

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern IntPtr cveFarnebackOpticalFlowCreate(
            int numLevels,
            double pyrScale,
            [MarshalAs(CvInvoke.BoolMarshalType)]
            bool fastPyramids,
            int winSize,
            int numIters,
            int polyN,
            double polySigma,
            CvEnum.OpticalflowFarnebackFlag flags,
            ref IntPtr denseOpticalFlow,
            ref IntPtr algorithm,
            ref IntPtr sharedPtr);
    }
}
