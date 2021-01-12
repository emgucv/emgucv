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
    /// DualTvl1 optical flow
    /// </summary>
    public class CudaOpticalFlowDualTvl1 : SharedPtrObject, ICudaDenseOpticalFlow
    {
        private IntPtr _denseFlow;
        private IntPtr _algorithm;

        /// <summary>
        /// Initializes a new instance of the CudaOpticalFlowDualTvl1 class.
        /// </summary>
        /// <param name="tau">Time step of the numerical scheme.</param>
        /// <param name="lambda">Weight parameter for the data term, attachment parameter. This is the most relevant parameter, which determines the smoothness of the output. The smaller this parameter is, the smoother the solutions we obtain. It depends on the range of motions of the images, so its value should be adapted to each image sequence.</param>
        /// <param name="theta">Parameter used for motion estimation. It adds a variable allowing for illumination variations Set this parameter to 1. if you have varying illumination.</param>
        /// <param name="nscales">Number of scales used to create the pyramid of images.</param>
        /// <param name="warps">Number of warpings per scale. Represents the number of times that I1(x+u0) and grad( I1(x+u0) ) are computed per scale. This is a parameter that assures the stability of the method. It also affects the running time, so it is a compromise between speed and accuracy.</param>
        /// <param name="epsilon">Stopping criterion threshold used in the numerical scheme, which is a trade-off between precision and running time. A small value will yield more accurate solutions at the expense of a slower convergence.</param>
        /// <param name="iterations">Stopping criterion iterations number used in the numerical scheme.</param>
        /// <param name="scaleStep">Scale step</param>
        /// <param name="gamma">Weight parameter for (u - v)^2, tightness parameter. It serves as a link between the attachment and the regularization terms. In theory, it should have a small value in order to maintain both parts in correspondence. The method is stable for a large range of values of this parameter.</param>
        /// <param name="useInitialFlow">If true, use initial flow.</param>
        public CudaOpticalFlowDualTvl1(
           double tau = 0.25, 
           double lambda = 0.15, 
           double theta = 0.3, 
           int nscales = 5, 
           int warps = 5,
           double epsilon = 0.01, 
           int iterations = 300, 
           double scaleStep = 0.8, 
           double gamma = 0.0,
           bool useInitialFlow = false)
        {
            _ptr = CudaInvoke.cudaOpticalFlowDualTvl1Create(
                tau, lambda, theta, nscales, warps, epsilon, iterations, scaleStep, gamma, useInitialFlow, ref _denseFlow, ref _algorithm, ref _sharedPtr);
        }

        /// <summary>
        /// Release all the unmanaged memory associated with this optical flow solver.
        /// </summary>
        protected override void DisposeObject()
        {
            if (_sharedPtr != IntPtr.Zero)
            {
                CudaInvoke.cudaOpticalFlowDualTvl1Release(ref _sharedPtr);
                _denseFlow = IntPtr.Zero;
                _algorithm = IntPtr.Zero;
                _ptr = IntPtr.Zero;
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
            ref IntPtr algorithm, 
            ref IntPtr sharedPtr);

        [DllImport(CvInvoke.ExternCudaLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern void cudaOpticalFlowDualTvl1Release(ref IntPtr flow);

    }
}
