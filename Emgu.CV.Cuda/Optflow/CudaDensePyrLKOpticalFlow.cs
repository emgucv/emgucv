//----------------------------------------------------------------------------
//  Copyright (C) 2004-2021 by EMGU Corporation. All rights reserved.       
//----------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Runtime.InteropServices;
using Emgu.CV.Structure;
using Emgu.CV.Util;
using Emgu.Util;

namespace Emgu.CV.Cuda
{
    /// <summary>
    /// PyrLK optical flow
    /// </summary>
    public class CudaDensePyrLKOpticalFlow : SharedPtrObject, ICudaDenseOpticalFlow
    {
        private IntPtr _denseFlow;
        private IntPtr _algorithm;

        /// <summary>
        /// Create the PyrLK optical flow solver
        /// </summary>
        /// <param name="winSize">Windows size. Use 21x21 for default</param>
        /// <param name="maxLevel">The maximum number of pyramid levels.</param>
        /// <param name="iters">The number of iterations.</param>
        /// <param name="useInitialFlow">Weather or not use the initial flow in the input matrix.</param>
        public CudaDensePyrLKOpticalFlow(Size winSize, int maxLevel = 3, int iters = 30, bool useInitialFlow = false)
        {
            _ptr = CudaInvoke.cudaDensePyrLKOpticalFlowCreate(ref winSize, maxLevel, iters, useInitialFlow, ref _denseFlow, ref _algorithm, ref _sharedPtr);
        }

        /// <summary>
        /// Release all the unmanaged memory associated with this optical flow solver.
        /// </summary>
        protected override void DisposeObject()
        {
            if (_sharedPtr != IntPtr.Zero)
            {
                CudaInvoke.cudaDensePyrLKOpticalFlowRelease(ref _sharedPtr);
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
        internal extern static IntPtr cudaDensePyrLKOpticalFlowCreate(
           ref Size winSize, int maxLevel, int iters,
           [MarshalAs(CvInvoke.BoolMarshalType)]
           bool useInitialFlow,
           ref IntPtr denseFlow,
           ref IntPtr algorithm, 
           ref IntPtr sharedPtr);

        [DllImport(CvInvoke.ExternCudaLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal extern static void cudaDensePyrLKOpticalFlowRelease(ref IntPtr flow);
    }
}
