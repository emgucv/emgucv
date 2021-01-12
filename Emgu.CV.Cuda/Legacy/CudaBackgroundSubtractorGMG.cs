//----------------------------------------------------------------------------
//  Copyright (C) 2004-2021 by EMGU Corporation. All rights reserved.       
//----------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;
using Emgu.CV.Structure;
using Emgu.CV.Util;
using Emgu.Util;

namespace Emgu.CV.Cuda
{
    /// <summary>
    /// Background/Foreground Segmentation Algorithm.
    /// </summary>
    public class CudaBackgroundSubtractorGMG : SharedPtrObject
    {
        /// <summary>
        /// Create a Background/Foreground Segmentation model
        /// </summary>
        /// <param name="initializationFrames">The number of frames used for initialization</param>
        /// <param name="decisionThreshold">The decision threshold</param>
        public CudaBackgroundSubtractorGMG(int initializationFrames = 120, double decisionThreshold = 0.8)
        {
            _ptr = CudaInvoke.cudaBackgroundSubtractorGMGCreate(initializationFrames, decisionThreshold, ref _sharedPtr);
        }

        /// <summary>
        /// Updates the background model
        /// </summary>
        /// <param name="frame">Next video frame.</param>
        /// <param name="learningRate">The learning rate, use -1.0f for default value.</param>
        /// <param name="foregroundMask">The output foreground mask as an 8-bit binary image.</param>
        /// <param name="stream">Use a Stream to call the function asynchronously (non-blocking) or null to call the function synchronously (blocking).</param>
        public void Apply(IInputArray frame, IOutputArray foregroundMask, double learningRate = -1, Stream stream = null)
        {
            using (InputArray iaFrame = frame.GetInputArray())
            using (OutputArray oaForegroundMask = foregroundMask.GetOutputArray())
                CudaInvoke.cudaBackgroundSubtractorGMGApply(_ptr, iaFrame, oaForegroundMask, learningRate, stream);
        }

        /// <summary>
        /// Release all the unmanaged resource associated with this object
        /// </summary>
        protected override void DisposeObject()
        {
            if (IntPtr.Zero != _sharedPtr)
            {
                CudaInvoke.cudaBackgroundSubtractorGMGRelease(ref _sharedPtr);
                _ptr = IntPtr.Zero;
            }
        }
    }

    public static partial class CudaInvoke
    {
        [DllImport(CvInvoke.ExternCudaLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern IntPtr cudaBackgroundSubtractorGMGCreate(int initializationFrames, double decisionThreshold, ref IntPtr sharedPtr);

        [DllImport(CvInvoke.ExternCudaLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern void cudaBackgroundSubtractorGMGApply(IntPtr gmg, IntPtr frame, IntPtr fgMask, double learningRate, IntPtr stream);

        [DllImport(CvInvoke.ExternCudaLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern void cudaBackgroundSubtractorGMGRelease(ref IntPtr gmg);
    }
}
