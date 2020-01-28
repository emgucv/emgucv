//----------------------------------------------------------------------------
//  Copyright (C) 2004-2020 by EMGU Corporation. All rights reserved.       
//----------------------------------------------------------------------------

using System;
using System.Drawing;
using System.Runtime.InteropServices;
using Emgu.CV;
using Emgu.CV.Structure;
using Emgu.Util;

namespace Emgu.CV
{
    /// <summary>
    /// K-nearest neighbors - based Background/Foreground Segmentation Algorithm.
    /// </summary>
    public partial class BackgroundSubtractorKNN : UnmanagedObject, IBackgroundSubtractor
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
        /// Create a K-nearest neighbors - based Background/Foreground Segmentation Algorithm.
        /// </summary>
        /// <param name="history">Length of the history.</param>
        /// <param name="dist2Threshold">Threshold on the squared distance between the pixel and the sample to decide whether a pixel is close to that sample. This parameter does not affect the background update.</param>
        /// <param name="detectShadows">If true, the algorithm will detect shadows and mark them. It decreases the speed a bit, so if you do not need this feature, set the parameter to false.</param>
        public BackgroundSubtractorKNN(int history, double dist2Threshold, bool detectShadows)
        {
            _ptr = CvInvoke.cveBackgroundSubtractorKNNCreate(history, dist2Threshold, detectShadows, ref _backgroundSubtractorPtr, ref _algorithmPtr, ref _sharedPtr);
        }

        /// <summary>
        /// Release all the unmanaged memory associated with this background model.
        /// </summary>
        protected override void DisposeObject()
        {
            if (_ptr != IntPtr.Zero)
            {
                CvInvoke.cveBackgroundSubtractorKNNRelease(ref _ptr, ref _sharedPtr);
                _backgroundSubtractorPtr = IntPtr.Zero;
                _algorithmPtr = IntPtr.Zero;
            }
        }
    }


    public static partial class CvInvoke
    {
        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern IntPtr cveBackgroundSubtractorKNNCreate(
                int history,
                double dist2Threshold,
                [MarshalAs(CvInvoke.BoolMarshalType)]
                bool detectShadows,
                ref IntPtr bgSubtractor,
                ref IntPtr algorithm,
                ref IntPtr sharedPtr);

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern void cveBackgroundSubtractorKNNRelease(ref IntPtr bgSubstractor, ref IntPtr sharedPtr);
    }

}
