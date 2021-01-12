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
    /// Sparse PyrLK optical flow
    /// </summary>
    public class CudaSparsePyrLKOpticalFlow : SharedPtrObject, ICudaSparseOpticalFlow
    {
        private IntPtr _sparseFlow;
        private IntPtr _algorithm;

        /// <summary>
        /// Create the PyrLK optical flow solver
        /// </summary>
        /// <param name="winSize">Windows size. Use 21x21 for default</param>
        /// <param name="maxLevel">The maximum number of pyramid levels.</param>
        /// <param name="iters">The number of iterations.</param>
        /// <param name="useInitialFlow">Weather or not use the initial flow in the input matrix.</param>
        public CudaSparsePyrLKOpticalFlow(Size winSize, int maxLevel = 3, int iters = 30, bool useInitialFlow = false)
        {
            _ptr = CudaInvoke.cudaSparsePyrLKOpticalFlowCreate(
                ref winSize, maxLevel, iters, useInitialFlow,
                ref _sparseFlow, ref _algorithm, ref _sharedPtr);
        }

        /// <summary>
        /// Release all the unmanaged memory associated with this optical flow solver.
        /// </summary>
        protected override void DisposeObject()
        {
            if (_sharedPtr != IntPtr.Zero)
            {
                CudaInvoke.cudaSparsePyrLKOpticalFlowRelease(ref _sharedPtr);
                _ptr = IntPtr.Zero;
                _sparseFlow = IntPtr.Zero;
                _algorithm = IntPtr.Zero;
            }
        }

        /// <summary>
        /// Pointer to the unmanaged SparseOpticalFlow object
        /// </summary>
        public IntPtr SparseOpticalFlowPtr
        {
            get { return _sparseFlow; }
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
        internal extern static IntPtr cudaSparsePyrLKOpticalFlowCreate(
            ref Size winSize, 
            int maxLevel, 
            int iters,
            [MarshalAs(CvInvoke.BoolMarshalType)]
            bool useInitialFlow,
            ref IntPtr sparseFlow,
            ref IntPtr algorithm, 
            ref IntPtr sharedPtr);

        [DllImport(CvInvoke.ExternCudaLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal extern static void cudaSparsePyrLKOpticalFlowRelease(ref IntPtr flow);
    }

}