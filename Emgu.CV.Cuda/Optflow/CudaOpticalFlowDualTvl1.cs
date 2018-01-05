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
    /// DualTvl1 optical flow
    /// </summary>
    public class CudaOpticalFlowDualTvl1 : UnmanagedObject, ICudaDenseOpticalFlow
    {
        private IntPtr _denseFlow;
        private IntPtr _algorithm;

        /// <summary>
        /// Initializes a new instance of the <see cref="CudaOpticalFlowDualTvl1"/> class.
        /// </summary>
        public CudaOpticalFlowDualTvl1(
           double tau, double lambda, double theta, int nscales, int warps,
           double epsilon, int iterations, double scaleStep, double gamma,
           bool useInitialFlow)
        {
            _ptr = CudaInvoke.cudaOpticalFlowDualTvl1Create(tau, lambda, theta, nscales, warps, epsilon, iterations, scaleStep, gamma, useInitialFlow, ref _denseFlow, ref _algorithm);
        }

        /// <summary>
        /// Release all the unmanaged memory associated with this optical flow solver.
        /// </summary>
        protected override void DisposeObject()
        {
            if (_ptr != IntPtr.Zero)
            {
                CudaInvoke.cudaOpticalFlowDualTvl1Release(ref _ptr);
                _denseFlow = IntPtr.Zero;
                _algorithm = IntPtr.Zero;
            }
        }

        /// <summary>
        /// Pointer to the DenseOpticalFlow object
        /// </summary>
        public IntPtr DenseOpticalFlowPtr
        {
            get { return _denseFlow; }
        }

        /// <summary>
        /// Pointer to the algorithm object
        /// </summary>
        public IntPtr AlgorithmPtr { get { return _algorithm; } }
    }

    public static partial class CudaInvoke
    {
        [DllImport(CvInvoke.ExternCudaLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern IntPtr cudaOpticalFlowDualTvl1Create(
            double tau, double lambda, double theta, int nscales, int warps,
            double epsilon, int iterations, double scaleStep, double gamma,
            [MarshalAs(CvInvoke.BoolMarshalType)]
            bool useInitialFlow,
            ref IntPtr denseFlow,
            ref IntPtr algorithm);

        [DllImport(CvInvoke.ExternCudaLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal extern static void cudaOpticalFlowDualTvl1Release(ref IntPtr flow);

    }
}
