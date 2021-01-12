//----------------------------------------------------------------------------
//  Copyright (C) 2004-2021 by EMGU Corporation. All rights reserved.       
//----------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Text;
using Emgu.CV.CvEnum;
using Emgu.CV.Structure;
using Emgu.CV.Util;
using Emgu.Util;

namespace Emgu.CV.Cuda
{
    /// <summary>
    /// Gaussian Mixture-based Background/Foreground Segmentation Algorithm.
    /// </summary>
    public class CudaBackgroundSubtractorMOG : SharedPtrObject, IBackgroundSubtractor
    {

        private IntPtr _algorithmPtr;
        private IntPtr _backgroundSubtractorPtr;

        /// <summary>
        /// Pointer to the unmanaged Algorithm object
        /// </summary>
        public IntPtr AlgorithmPtr { get { return _algorithmPtr; } }

        /// <summary>
        /// Pointer to the unmanaged BackgroundSubtractor object
        /// </summary>
        public IntPtr BackgroundSubtractorPtr { get { return _backgroundSubtractorPtr; } }

        /// <summary>
        /// Create a Gaussian Mixture-based Background/Foreground Segmentation model
        /// </summary>
        /// <param name="history">Length of the history.</param>
        /// <param name="nMixtures">Number of Gaussian mixtures.</param>
        /// <param name="backgroundRatio">Background ratio.</param>
        /// <param name="noiseSigma">Noise strength (standard deviation of the brightness or each color channel). 0 means some automatic value.</param>
        public CudaBackgroundSubtractorMOG(int history = 200, int nMixtures = 4, double backgroundRatio = 0.7, double noiseSigma = 0)
        {
            _ptr = CudaInvoke.cudaBackgroundSubtractorMOGCreate(history, nMixtures, backgroundRatio, noiseSigma, ref _backgroundSubtractorPtr, ref _algorithmPtr, ref _sharedPtr);
        }

        /// <summary>
        /// Updates the background model
        /// </summary>
        /// <param name="frame">Next video frame.</param>
        /// <param name="learningRate">The learning rate, use -1.0f for default value.</param>
        /// <param name="stream">Use a Stream to call the function asynchronously (non-blocking) or null to call the function synchronously (blocking).</param>
        /// <param name="foregroundMask">The foregroundMask</param>
        public void Update(IInputArray frame, IOutputArray foregroundMask, double learningRate, Stream stream = null)
        {
            using (InputArray iaFrame = frame.GetInputArray())
            using (OutputArray oaForegroundMask = foregroundMask.GetOutputArray())
                CudaInvoke.cudaBackgroundSubtractorMOGApply(_ptr, iaFrame, oaForegroundMask, learningRate, stream);
        }

        /// <summary>
        /// Release all the unmanaged resource associated with this object
        /// </summary>
        protected override void DisposeObject()
        {
            if (IntPtr.Zero != _sharedPtr)
            {
                CudaInvoke.cudaBackgroundSubtractorMOGRelease(ref _sharedPtr);
                _ptr = IntPtr.Zero;
            }
        }

    }

    public static partial class CudaInvoke
    {
        [DllImport(CvInvoke.ExternCudaLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern IntPtr cudaBackgroundSubtractorMOGCreate(
            int history, int nMixtures, double backgroundRatio, double noiseSigma,
            ref IntPtr bgSubtractor,
            ref IntPtr algorithm,
            ref IntPtr sharedPtr);

        [DllImport(CvInvoke.ExternCudaLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern void cudaBackgroundSubtractorMOGApply(IntPtr mog, IntPtr frame, IntPtr fgMask, double learningRate, IntPtr stream);

        [DllImport(CvInvoke.ExternCudaLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern void cudaBackgroundSubtractorMOGRelease(ref IntPtr mog);
    }
}
