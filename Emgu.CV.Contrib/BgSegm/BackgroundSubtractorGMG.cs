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
    /// Background Subtractor module based on the algorithm given in:
    /// Andrew B. Godbehere, Akihiro Matsukawa, Ken Goldberg, 
    /// "Visual Tracking of Human Visitors under Variable-Lighting Conditions for a Responsive Audio Art Installation", 
    /// American Control Conference, Montreal, June 2012.
    /// </summary>
    public class BackgroundSubtractorGMG : UnmanagedObject, IBackgroundSubtractor
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
        /// Create a background subtractor module based on GMG
        /// </summary>
        /// <param name="initializationFrames">Number of frames used to initialize the background models.</param>
        /// <param name="decisionThreshold">Threshold value, above which it is marked foreground, else background.</param>
        public BackgroundSubtractorGMG(int initializationFrames, double decisionThreshold)
        {
            _ptr = BgSegmInvoke.cveBackgroundSubtractorGMGCreate(initializationFrames, decisionThreshold, ref _backgroundSubtractorPtr, ref _algorithmPtr, ref _sharedPtr);
        }

        /// <summary>
        /// Release all the unmanaged memory associated with this background model.
        /// </summary>
        protected override void DisposeObject()
        {
            if (IntPtr.Zero != _ptr)
            {
                BgSegmInvoke.cveBackgroundSubtractorGMGRelease(ref _ptr, ref _sharedPtr);
                _backgroundSubtractorPtr = IntPtr.Zero;
                _algorithmPtr = IntPtr.Zero;
            }
        }

    }

    public static partial class BgSegmInvoke
    {
        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern IntPtr cveBackgroundSubtractorGMGCreate(int initializationFrames, double decisionThreshold, ref IntPtr bgSubtractor, ref IntPtr algorithm, ref IntPtr sharedPtr);

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern void cveBackgroundSubtractorGMGRelease(ref IntPtr bgSubstractor, ref IntPtr sharedPtr);
    }
}
