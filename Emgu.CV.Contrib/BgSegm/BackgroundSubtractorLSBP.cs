//----------------------------------------------------------------------------
//  Copyright (C) 2004-2020 by EMGU Corporation. All rights reserved.       
//----------------------------------------------------------------------------

using System;
using System.Runtime.InteropServices;
using Emgu.CV;
using Emgu.CV.Structure;
using Emgu.Util;

namespace Emgu.CV.BgSegm
{
    /// <summary>
    /// Background Subtraction using Local SVD Binary Pattern.
    /// </summary>
    /// <remarks>More details about the algorithm can be found at: L. Guo, D. Xu, and Z. Qiang. Background subtraction using local svd binary pattern. In 2016 IEEE Conference on Computer Vision and Pattern Recognition Workshops (CVPRW), pages 1159–1167, June 2016.</remarks>
    public class BackgroundSubtractorLSBP : UnmanagedObject, IBackgroundSubtractor
    {
        private IntPtr _sharedPtr;

        /// <summary>
        /// Camera Motion compensation mode
        /// </summary>
        public enum CameraMotionCompensation
        {
            /// <summary>
            /// None
            /// </summary>
            None = 0,
            /// <summary>
            /// Use LK camera compensation
            /// </summary>
            LK = 1
        }

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
        /// Creates an instance of BackgroundSubtractorLSBP algorithm.
        /// </summary>
        /// <param name="mc">Whether to use camera motion compensation.</param>
        /// <param name="nSamples">Number of samples to maintain at each point of the frame.</param>
        /// <param name="LSBPRadius">LSBP descriptor radius.</param>
        /// <param name="tlower">Lower bound for T-values.</param>
        /// <param name="tupper">Upper bound for T-values.</param>
        /// <param name="tinc">Increase step for T-values.</param>
        /// <param name="tdec">Decrease step for T-values.</param>
        /// <param name="rscale">Scale coefficient for threshold values.</param>
        /// <param name="rincdec">Increase/Decrease step for threshold values.</param>
        /// <param name="noiseRemovalThresholdFacBG">Strength of the noise removal for background points.</param>
        /// <param name="noiseRemovalThresholdFacFG">Strength of the noise removal for foreground points.</param>
        /// <param name="LSBPthreshold">Threshold for LSBP binary string.</param>
        /// <param name="minCount">Minimal number of matches for sample to be considered as foreground.</param>
        public BackgroundSubtractorLSBP(
            CameraMotionCompensation mc = CameraMotionCompensation.None, 
            int nSamples = 20, 
            int LSBPRadius = 16, 
            float tlower = 2.0f, 
            float tupper = 32.0f, 
            float tinc = 1.0f, 
            float tdec = 0.05f, 
            float rscale = 10.0f, 
            float rincdec = 0.005f, 
            float noiseRemovalThresholdFacBG = 0.0004f, 
            float noiseRemovalThresholdFacFG = 0.0008f, 
            int LSBPthreshold = 8, 
            int minCount = 2)
        {
            _ptr = Emgu.CV.ContribInvoke.cveBackgroundSubtractorLSBPCreate(
                mc, nSamples, LSBPRadius, tlower, tupper, tinc, tdec, rscale, rincdec, noiseRemovalThresholdFacBG, noiseRemovalThresholdFacFG, LSBPthreshold, minCount, ref _backgroundSubtractorPtr, ref _algorithmPtr, ref _sharedPtr);
        }

        /// <summary>
        /// Release all the unmanaged memory associated with this background model.
        /// </summary>
        protected override void DisposeObject()
        {
            if (IntPtr.Zero != _ptr)
            {
                Emgu.CV.ContribInvoke.cveBackgroundSubtractorLSBPRelease(ref _ptr, ref _sharedPtr);
                _backgroundSubtractorPtr = IntPtr.Zero;
                _algorithmPtr = IntPtr.Zero;
            }
        }

    }
}

namespace Emgu.CV
{
    public static partial class ContribInvoke
    {
        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern IntPtr cveBackgroundSubtractorLSBPCreate(
            Emgu.CV.BgSegm.BackgroundSubtractorLSBP.CameraMotionCompensation mc, int nSamples, int LSBPRadius, float tlower, float tupper, float tinc, float tdec, float rscale, float rincdec, float noiseRemovalThresholdFacBG, float noiseRemovalThresholdFacFG, int LSBPthreshold, int minCount, ref IntPtr bgSubtractor, ref IntPtr algorithm, ref IntPtr sharedPtr);

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern void cveBackgroundSubtractorLSBPRelease(ref IntPtr bgSubstractor, ref IntPtr sharedPtr);
    }
}
