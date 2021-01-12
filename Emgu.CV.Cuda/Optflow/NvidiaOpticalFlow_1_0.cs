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
    /// Class for computing the optical flow vectors between two images using NVIDIA Optical Flow hardware and Optical Flow SDK 1.0.
    /// </summary>
    public class NvidiaOpticalFlow_1_0 : SharedPtrObject, INvidiaOpticalFlow
    {
        /// <summary>
        /// Supported optical flow performance levels.
        /// </summary>
        public enum PerfLevel
        {
            /// <summary>
            /// Undefined
            /// </summary>
            Undefined,
            /// <summary>
            /// Slow perf level results in lowest performance and best quality
            /// </summary>
            Slow = 5,
            /// <summary>
            /// Medium perf level results in low performance and medium quality
            /// </summary>
            Medium = 10,
            /// <summary>
            /// Fast perf level results in high performance and low quality
            /// </summary>
            Fast = 20,
            /// <summary>
            /// Max
            /// </summary>
            Max
        };

        private IntPtr _nvidiaHWOpticalFlow;
        private IntPtr _algorithm;

        /// <summary>
        /// Instantiate NVIDIA Optical Flow
        /// </summary>
        /// <param name="width">Width of input image in pixels.</param>
        /// <param name="height">Height of input image in pixels.</param>
        /// <param name="perfPreset">Optional parameter. Refer https://developer.nvidia.com/opticalflow-sdk for details about presets. Defaults to Slow.</param>
        /// <param name="enableTemporalHints">Optional Parameter. Flag to enable passing external hints buffer to calc(). Defaults to false.</param>
        /// <param name="enableExternalHints">Optional Parameter. Flag to enable passing external hints buffer to calc(). Defaults to false.</param>
        /// <param name="enableCostBuffer">Optional Parameter. Flag to enable cost buffer output from calc(). Defaults to false.</param>
        /// <param name="gpuId">Optional parameter to select the GPU ID on which the optical flow should be computed. Useful in multi-GPU systems. Defaults to 0.</param>
        public NvidiaOpticalFlow_1_0(
            int width,
            int height,
            NvidiaOpticalFlow_1_0.PerfLevel perfPreset = PerfLevel.Slow,
            bool enableTemporalHints = false,
            bool enableExternalHints = false,
            bool enableCostBuffer = false,
            int gpuId = 0)
        {
            _ptr = CudaInvoke.cudaNvidiaOpticalFlow_1_0_Create(
                width,
                height,
                perfPreset,
                enableTemporalHints,
                enableExternalHints,
                enableCostBuffer,
                gpuId,
                ref _nvidiaHWOpticalFlow,
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
                CudaInvoke.cudaNvidiaOpticalFlow_1_0_Release(ref _sharedPtr);
                _nvidiaHWOpticalFlow = IntPtr.Zero;
                _algorithm = IntPtr.Zero;
                _ptr = IntPtr.Zero;
            }
        }

        /// <summary>
        /// Pointer to the NvidiaOpticalFlow object
        /// </summary>
        public IntPtr NvidiaOpticalFlowPtr
        {
            get { return _nvidiaHWOpticalFlow; }
        }

        /// <summary>
        /// Pointer to the algorithm object
        /// </summary>
        public IntPtr AlgorithmPtr { get { return _algorithm; } }
    }

    public static partial class CudaInvoke
    {
        [DllImport(CvInvoke.ExternCudaLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern IntPtr cudaNvidiaOpticalFlow_1_0_Create(
            int width,
            int height,
            NvidiaOpticalFlow_1_0.PerfLevel perfPreset,
            [MarshalAs(CvInvoke.BoolMarshalType)]
            bool enableTemporalHints,
            [MarshalAs(CvInvoke.BoolMarshalType)]
            bool enableExternalHints,
            [MarshalAs(CvInvoke.BoolMarshalType)]
            bool enableCostBuffer,
            int gpuId,
            ref IntPtr nHWOpticalFlow,
            ref IntPtr algorithm,
            ref IntPtr sharedPtr);

        [DllImport(CvInvoke.ExternCudaLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal extern static void cudaNvidiaOpticalFlow_1_0_Release(ref IntPtr flow);

    }
}
