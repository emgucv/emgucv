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
    /// Class for computing the optical flow vectors between two images using NVIDIA Optical Flow hardware and Optical Flow SDK 2.0.
    /// </summary>
    public class NvidiaOpticalFlow_2_0 : SharedPtrObject, INvidiaOpticalFlow
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
        }

        /// <summary>
        /// Output vector grid size
        /// </summary>
        public enum OutputVectorGridSize
        {
            /// <summary>
            /// Undefined
            /// </summary>
            Undefined,
            /// <summary>
            /// Output buffer grid size is 1x1
            /// </summary>
            Size1 = 1,
            /// <summary>
            /// Output buffer grid size is 2x2
            /// </summary>
            Size2 = 2,
            /// <summary>
            /// Output buffer grid size is 4x4
            /// </summary>
            Size4 = 4,
            /// <summary>
            /// Max size
            /// </summary>
            Max
        }

        /// <summary>
        /// Hint vector grid size
        /// </summary>
        public enum HintVectorGridSize
        {
            /// <summary>
            /// Undefined
            /// </summary>
            Undefined,
            /// <summary>
            /// Hint buffer grid size is 1x1.
            /// </summary>
            Size1 = 1,
            /// <summary>
            /// Hint buffer grid size is 2x2.
            /// </summary>
            Size2 = 2,
            /// <summary>
            /// Hint buffer grid size is 4x4.
            /// </summary>
            Size4 = 4,
            /// <summary>
            /// Hint buffer grid size is 8x8.
            /// </summary>
            Size8 = 8,
            /// <summary>
            /// Max size
            /// </summary>
            Max
        };

        private IntPtr _nvidiaHWOpticalFlow;
        private IntPtr _algorithm;

        /// <summary>
        /// Class for computing the optical flow vectors between two images using NVIDIA Optical Flow hardware and Optical Flow SDK 2.0.
        /// </summary>
        /// <param name="imageSize">Size of input image in pixels.</param>
        /// <param name="perfPreset">Optional parameter. Refer https://developer.nvidia.com/opticalflow-sdk for details about presets. Defaults to Slow.</param>
        /// <param name="outputGridSize">Optional parameter. Refer https://developer.nvidia.com/opticalflow-sdk for details about presets.</param>
        /// <param name="hintGridSize">Optional parameter. Refer https://developer.nvidia.com/opticalflow-sdk for details about presets.</param>
        /// <param name="enableTemporalHints">Optional Parameter. Flag to enable passing external hints buffer to calc(). Defaults to false.</param>
        /// <param name="enableExternalHints">Optional Parameter. Flag to enable passing external hints buffer to calc(). Defaults to false.</param>
        /// <param name="enableCostBuffer">Optional Parameter. Flag to enable cost buffer output from calc(). Defaults to false.</param>
        /// <param name="gpuId">Optional parameter to select the GPU ID on which the optical flow should be computed. Useful in multi-GPU systems. Defaults to 0.</param>
        /// <param name="inputStream">Optical flow algorithm may optionally involve cuda preprocessing on the input buffers. The input cuda stream can be used to pipeline and synchronize the cuda preprocessing tasks with OF HW engine. If input stream is not set, the execute function will use default stream which is NULL stream</param>
        /// <param name="outputStream">Optical flow algorithm may optionally involve cuda post processing on the output flow vectors. The output cuda stream can be used to pipeline and synchronize the cuda post processing tasks with OF HW engine. If output stream is not set, the execute function will use default stream which is NULL stream</param>
        public NvidiaOpticalFlow_2_0(
            Size imageSize,
            NvidiaOpticalFlow_2_0.PerfLevel perfPreset = PerfLevel.Slow,
            NvidiaOpticalFlow_2_0.OutputVectorGridSize outputGridSize = OutputVectorGridSize.Size1,
            NvidiaOpticalFlow_2_0.HintVectorGridSize hintGridSize = HintVectorGridSize.Size1,
            bool enableTemporalHints = false,
            bool enableExternalHints = false,
            bool enableCostBuffer = false,
            int gpuId = 0,
            Stream inputStream = null,
            Stream outputStream = null)
        {
            _ptr = CudaInvoke.cudaNvidiaOpticalFlow_2_0_Create(
                ref imageSize,
                perfPreset,
                outputGridSize,
                hintGridSize,
                enableTemporalHints,
                enableExternalHints,
                enableCostBuffer,
                gpuId,
                inputStream,
                outputStream,
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
                CudaInvoke.cudaNvidiaOpticalFlow_2_0_Release(ref _sharedPtr);
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

        /// <summary>
        /// Converts the hardware-generated flow vectors to floating point representation
        /// </summary>
        /// <param name="flow">Buffer of type CV_16FC2 containing flow vectors generated by Calc().</param>
        /// <param name="floatFlow">Buffer of type CV_32FC2, containing flow vectors in floating point representation, each flow vector for 1 pixel per gridSize, in the pitch-linear layout.</param>
        public void ConvertToFloat(IInputArray flow, IInputOutputArray floatFlow)
        {
            using (InputArray iaFlow = flow.GetInputArray())
            using (InputOutputArray ioaFloatFlow = floatFlow.GetInputOutputArray())
            {
                CudaInvoke.cudaNvidiaOpticalFlow_2_0_ConvertToFloat(_ptr, iaFlow, ioaFloatFlow);
            }
        }
    }

    public static partial class CudaInvoke
    {
        [DllImport(CvInvoke.ExternCudaLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern IntPtr cudaNvidiaOpticalFlow_2_0_Create(
            ref Size imageSize,
            NvidiaOpticalFlow_2_0.PerfLevel perfPreset,
            NvidiaOpticalFlow_2_0.OutputVectorGridSize outputVectorGridSize,
            NvidiaOpticalFlow_2_0.HintVectorGridSize hintVectorGridSize,
            [MarshalAs(CvInvoke.BoolMarshalType)]
            bool enableTemporalHints,
            [MarshalAs(CvInvoke.BoolMarshalType)]
            bool enableExternalHints,
            [MarshalAs(CvInvoke.BoolMarshalType)]
            bool enableCostBuffer,
            int gpuId,
            IntPtr inputStream,
            IntPtr outputStream,
            ref IntPtr nHWOpticalFlow,
            ref IntPtr algorithm,
            ref IntPtr sharedPtr);

        [DllImport(CvInvoke.ExternCudaLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern void cudaNvidiaOpticalFlow_2_0_ConvertToFloat(
            IntPtr novf,
            IntPtr flow, 
            IntPtr floatFlow);

        [DllImport(CvInvoke.ExternCudaLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern void cudaNvidiaOpticalFlow_2_0_Release(ref IntPtr flow);

    }
}
