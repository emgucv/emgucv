//----------------------------------------------------------------------------
//  Copyright (C) 2004-2021 by EMGU Corporation. All rights reserved.       
//----------------------------------------------------------------------------

using System;
using Emgu.Util;
using Emgu.CV;
using Emgu.CV.Structure;
using System.Drawing;
using System.Runtime.InteropServices;

namespace Emgu.CV
{
    /// <summary>
    /// The class implements the following algorithm:
    /// "Improved adaptive Gaussian mixture model for background subtraction"
    /// Z.Zivkovic
    /// International Conference Pattern Recognition, UK, August, 2004.
    /// http://www.zoranz.net/Publications/zivkovic2004ICPR.pdf
    /// </summary>
    public partial class BackgroundSubtractorMOG2 : UnmanagedObject, IBackgroundSubtractor
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
        /// Create an "Improved adaptive Gaussian mixture model for background subtraction".
        /// </summary>
        /// <param name="history">The length of the history.</param>
        /// <param name="varThreshold">The maximum allowed number of mixture components. Actual number is determined dynamically per pixel.</param>
        /// <param name="shadowDetection">If true, the algorithm will detect shadows and mark them. It decreases the speed a bit, so if you do not need this feature, set the parameter to false.</param>
        public BackgroundSubtractorMOG2(int history = 500, float varThreshold = 16, bool shadowDetection = true)
        {
            _ptr = CvInvoke.cveBackgroundSubtractorMOG2Create(history, varThreshold, shadowDetection, ref _backgroundSubtractorPtr, ref _algorithmPtr, ref _sharedPtr);
        }

        /// <summary>
        /// Release all the unmanaged memory associated with this background model.
        /// </summary>
        protected override void DisposeObject()
        {
            if (_ptr != IntPtr.Zero)
            {
                CvInvoke.cveBackgroundSubtractorMOG2Release(ref _ptr, ref _sharedPtr);
                _backgroundSubtractorPtr = IntPtr.Zero;
                _algorithmPtr = IntPtr.Zero;
            }
        }


    }

    public static partial class CvInvoke
    {
        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern IntPtr cveBackgroundSubtractorMOG2Create(
            int history,
            float varThreshold,
            [MarshalAs(CvInvoke.BoolMarshalType)]
            bool bShadowDetection,
            ref IntPtr bgSubtractor,
            ref IntPtr algorithm, 
            ref IntPtr sharedPtr);

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern void cveBackgroundSubtractorMOG2Release(ref IntPtr bgSubstractor, ref IntPtr sharedPtr);
    }
}