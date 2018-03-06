//----------------------------------------------------------------------------
//  Copyright (C) 2004-2018 by EMGU Corporation. All rights reserved.       
//----------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Emgu.CV.Structure;
using Emgu.Util;

namespace Emgu.CV.Cuda
{
    /// <summary>
    /// Farneback optical flow
    /// </summary>
    public class CudaFarnebackOpticalFlow : UnmanagedObject, ICudaDenseOpticalFlow
    {
        private IntPtr _denseFlow;
        private IntPtr _algorithm;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="numLevels"></param>
        /// <param name="pyrScale"></param>
        /// <param name="fastPyramids"></param>
        /// <param name="winSize"></param>
        /// <param name="numIters"></param>
        /// <param name="polyN"></param>
        /// <param name="polySigma"></param>
        /// <param name="flags"></param>
        public CudaFarnebackOpticalFlow(
           int numLevels = 5,
           double pyrScale = 0.5,
           bool fastPyramids = false,
           int winSize = 13,
           int numIters = 10,
           int polyN = 5,
           double polySigma = 1.1,
           int flags = 0)
        {
            _ptr = CudaInvoke.cudaFarnebackOpticalFlowCreate(numLevels, pyrScale, fastPyramids, winSize, numIters, polyN, polySigma, flags, ref _denseFlow, ref _algorithm);
        }

        /// <summary>
        /// Release all the unmanaged memory associated with this optical flow solver.
        /// </summary>
        protected override void DisposeObject()
        {
            if (_ptr != IntPtr.Zero)
            {
                CudaInvoke.cudaFarnebackOpticalFlowRelease(ref _ptr);
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
           int flags,
           ref IntPtr denseFlow,
           ref IntPtr algorithm);

        [DllImport(CvInvoke.ExternCudaLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal extern static void cudaFarnebackOpticalFlowRelease(ref IntPtr flow);

    }
}
