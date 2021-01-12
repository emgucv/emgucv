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
    /// Brox optical flow
    /// </summary>
    public class CudaBroxOpticalFlow : SharedPtrObject, ICudaDenseOpticalFlow
    {
        private IntPtr _denseFlow;
        private IntPtr _algorithm;

        /// <summary>
        /// Create the Brox optical flow solver
        /// </summary>
        /// <param name="alpha">Flow smoothness</param>
        /// <param name="gamma">Gradient constancy importance</param>
        /// <param name="scaleFactor">Pyramid scale factor</param>
        /// <param name="innerIterations">Number of lagged non-linearity iterations (inner loop)</param>
        /// <param name="outerIterations">Number of warping iterations (number of pyramid levels)</param>
        /// <param name="solverIterations">Number of linear system solver iterations</param>
        public CudaBroxOpticalFlow(double alpha = 0.197, double gamma = 50, double scaleFactor = 0.8, int innerIterations = 5, int outerIterations = 150, int solverIterations = 10)
        {
            _ptr = CudaInvoke.cudaBroxOpticalFlowCreate(alpha, gamma, scaleFactor, innerIterations, outerIterations, solverIterations, ref _denseFlow, ref _algorithm, ref _sharedPtr);
        }

        /// <summary>
        /// Release all the unmanaged memory associated with this optical flow solver.
        /// </summary>
        protected override void DisposeObject()
        {
            if (_sharedPtr != IntPtr.Zero)
            {
                CudaInvoke.cudaBroxOpticalFlowRelease(ref _sharedPtr);
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
        /// Pointer to the unmanaged Algorithm object
        /// </summary>
        public IntPtr AlgorithmPtr
        {
            get { return _algorithm; }
        }
    }

    public static partial class CudaInvoke
    {
        [DllImport(CvInvoke.ExternCudaLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal extern static IntPtr cudaBroxOpticalFlowCreate(double alpha, double gamma, double scaleFactor, int innerIterations, int outerIterations, int solverIterations, ref IntPtr denseFlow, ref IntPtr algorithm, ref IntPtr sharedPtr);

        [DllImport(CvInvoke.ExternCudaLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal extern static void cudaBroxOpticalFlowRelease(ref IntPtr flow);
    }
}
