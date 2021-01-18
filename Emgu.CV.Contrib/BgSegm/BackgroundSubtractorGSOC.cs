//----------------------------------------------------------------------------
//  Copyright (C) 2004-2021 by EMGU Corporation. All rights reserved.       
//----------------------------------------------------------------------------

using System;
using System.Runtime.InteropServices;
using Emgu.CV;
using Emgu.CV.Structure;
using Emgu.Util;

namespace Emgu.CV.BgSegm
{

    /// <summary>
    /// Implementation of the different yet better algorithm which is called GSOC, as it was implemented during GSOC and was not originated from any paper.
    /// </summary>
    public class BackgroundSubtractorGSOC : UnmanagedObject, IBackgroundSubtractor
    {
        private IntPtr _sharedPtr;

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
        /// Creates an instance of BackgroundSubtractorGSOC algorithm.
        /// </summary>
        /// <param name="mc">Whether to use camera motion compensation.</param>
        /// <param name="nSamples">Number of samples to maintain at each point of the frame.</param>
        /// <param name="replaceRate">Probability of replacing the old sample - how fast the model will update itself.</param>
        /// <param name="propagationRate">Probability of propagating to neighbors.</param>
        /// <param name="hitsThreshold">How many positives the sample must get before it will be considered as a possible replacement.</param>
        /// <param name="alpha">Scale coefficient for threshold.</param>
        /// <param name="beta">Bias coefficient for threshold.</param>
        /// <param name="blinkingSupressionDecay">Blinking supression decay factor.</param>
        /// <param name="blinkingSupressionMultiplier">Blinking supression multiplier.</param>
        /// <param name="noiseRemovalThresholdFacBG">Strength of the noise removal for background points.</param>
        /// <param name="noiseRemovalThresholdFacFG">Strength of the noise removal for foreground points.</param>
        public BackgroundSubtractorGSOC(
            BackgroundSubtractorLSBP.CameraMotionCompensation mc = BackgroundSubtractorLSBP.CameraMotionCompensation.None, 
            int nSamples = 20, 
            float replaceRate = 0.003f, 
            float propagationRate = 0.01f, 
            int hitsThreshold = 32, 
            float alpha = 0.01f, 
            float beta = 0.0022f, 
            float blinkingSupressionDecay = 0.1f, 
            float blinkingSupressionMultiplier = 0.1f, 
            float noiseRemovalThresholdFacBG = 0.0004f, 
            float noiseRemovalThresholdFacFG = 0.0008f)
        {
            _ptr = BgSegmInvoke.cveBackgroundSubtractorGSOCCreate(mc, nSamples, replaceRate, propagationRate, hitsThreshold, alpha, beta, blinkingSupressionDecay, blinkingSupressionMultiplier, noiseRemovalThresholdFacBG, noiseRemovalThresholdFacFG, ref _backgroundSubtractorPtr, ref _algorithmPtr, ref _sharedPtr);
        }

        /// <summary>
        /// Release all the unmanaged memory associated with this background model.
        /// </summary>
        protected override void DisposeObject()
        {
            if (IntPtr.Zero != _ptr)
            {
                BgSegmInvoke.cveBackgroundSubtractorGSOCRelease(ref _ptr, ref _sharedPtr);
                _backgroundSubtractorPtr = IntPtr.Zero;
                _algorithmPtr = IntPtr.Zero;
            }
        }
    }

    public static partial class BgSegmInvoke
    {
        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern IntPtr cveBackgroundSubtractorGSOCCreate(
            Emgu.CV.BgSegm.BackgroundSubtractorLSBP.CameraMotionCompensation mc, int nSamples, float replaceRate, float propagationRate, int hitsThreshold, float alpha, float beta, float blinkingSupressionDecay, float blinkingSupressionMultiplier, float noiseRemovalThresholdFacBG, float noiseRemovalThresholdFacFG,
            ref IntPtr bgSubtractor, ref IntPtr algorithm, ref IntPtr sharedPtr);

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern void cveBackgroundSubtractorGSOCRelease(ref IntPtr bgSubtractor, ref IntPtr sharedPtr);
    }
}
