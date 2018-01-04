//----------------------------------------------------------------------------
//  Copyright (C) 2004-2018 by EMGU Corporation. All rights reserved.       
//----------------------------------------------------------------------------

using System;
using System.Runtime.InteropServices;
using Emgu.CV;
using Emgu.CV.Structure;
using Emgu.Util;

namespace Emgu.CV.BgSegm
{

    public class BackgroundSubtractorLSBP : UnmanagedObject, IBackgroundSubtractor
    {
        public enum CameraMotionCompensation
        {
            None = 0,
            LK = 1
        }

        private IntPtr _algorithmPtr;
        private IntPtr _backgroundSubtractorPtr;
        public IntPtr AlgorithmPtr { get { return _algorithmPtr; } }
        public IntPtr BackgroundSubtractorPtr { get { return _backgroundSubtractorPtr; } }
        
        public BackgroundSubtractorLSBP(CameraMotionCompensation mc = CameraMotionCompensation.None, int nSamples = 20, int LSBPRadius = 16, float tlower = 2.0f, float tupper = 32.0f, float tinc = 1.0f, float tdec = 0.05f, float rscale = 10.0f, float rincdec = 0.005f, float noiseRemovalThresholdFacBG = 0.0004f, float noiseRemovalThresholdFacFG = 0.0008f, int LSBPthreshold = 8, int minCount = 2)
        {
            _ptr = Emgu.CV.ContribInvoke.cveBackgroundSubtractorLSBPCreate(
                mc, nSamples, LSBPRadius, tlower, tupper, tinc, tdec, rscale, rincdec, noiseRemovalThresholdFacBG, noiseRemovalThresholdFacFG, LSBPthreshold, minCount, ref _backgroundSubtractorPtr, ref _algorithmPtr);
        }

        /// <summary>
        /// Release all the unmanaged memory associated with this background model.
        /// </summary>
        protected override void DisposeObject()
        {
            if (IntPtr.Zero != _ptr)
            {
                Emgu.CV.ContribInvoke.cveBackgroundSubtractorLSBPRelease(ref _ptr);
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
            Emgu.CV.BgSegm.BackgroundSubtractorLSBP.CameraMotionCompensation mc, int nSamples, int LSBPRadius, float tlower, float tupper, float tinc, float tdec, float rscale, float rincdec, float noiseRemovalThresholdFacBG, float noiseRemovalThresholdFacFG, int LSBPthreshold, int minCount, ref IntPtr bgSubtractor, ref IntPtr algorithm);

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern void cveBackgroundSubtractorLSBPRelease(ref IntPtr bgSubstractor);
    }
}
