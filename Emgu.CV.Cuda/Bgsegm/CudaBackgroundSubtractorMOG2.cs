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
    /// Gaussian Mixture-based Background/Foreground Segmentation Algorithm.
    /// </summary>
    public class CudaBackgroundSubtractorMOG2 : SharedPtrObject, IBackgroundSubtractor
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
        /// <param name="varThreshold">Threshold on the squared Mahalanobis distance between the pixel and the model to decide whether a pixel is well described by the background model. This parameter does not affect the background update.</param>
        /// <param name="detectShadows">If true, the algorithm will detect shadows and mark them. It decreases the speed a bit, so if you do not need this feature, set the parameter to false.</param>
        public CudaBackgroundSubtractorMOG2(
            int history = 500, 
            double varThreshold = 16, 
            bool detectShadows = true)
        {
            _ptr = CudaInvoke.cudaBackgroundSubtractorMOG2Create(history, varThreshold, detectShadows, ref _backgroundSubtractorPtr, ref _algorithmPtr, ref _sharedPtr);
        }

        /// <summary>
        /// Updates the background model
        /// </summary>
        /// <param name="frame">Next video frame.</param>
        /// <param name="fgmask">The output forground mask</param>
        /// <param name="learningRate">The learning rate, use -1.0f for default value.</param>
        /// <param name="stream">Use a Stream to call the function asynchronously (non-blocking) or null to call the function synchronously (blocking).</param>
        public void Update(IInputArray frame, IOutputArray fgmask, double learningRate, Stream stream = null)
        {
            using (InputArray iaFrame = frame.GetInputArray())
            using (OutputArray oaForgroundMask = fgmask.GetOutputArray())
                CudaInvoke.cudaBackgroundSubtractorMOG2Apply(_ptr, iaFrame, oaForgroundMask, learningRate, stream);
        }

        /// <summary>
        /// Release all the unmanaged resource associated with this object
        /// </summary>
        protected override void DisposeObject()
        {
            if (IntPtr.Zero != _sharedPtr)
            {
                CudaInvoke.cudaBackgroundSubtractorMOG2Release(ref _sharedPtr);
                _ptr = IntPtr.Zero;
            }
        }

    }

    public static partial class CudaInvoke
    {
        [DllImport(CvInvoke.ExternCudaLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern IntPtr cudaBackgroundSubtractorMOG2Create(
           int history,
           double varThreshold,
           [MarshalAs(CvInvoke.BoolMarshalType)]
           bool detectShadows,
           ref IntPtr bgSubtractor,
           ref IntPtr algorithm,
           ref IntPtr sharedPtr);

        [DllImport(CvInvoke.ExternCudaLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern void cudaBackgroundSubtractorMOG2Apply(IntPtr mog, IntPtr frame, IntPtr fgMask, double learningRate, IntPtr stream);

        [DllImport(CvInvoke.ExternCudaLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern void cudaBackgroundSubtractorMOG2Release(ref IntPtr mog);
    }
}
