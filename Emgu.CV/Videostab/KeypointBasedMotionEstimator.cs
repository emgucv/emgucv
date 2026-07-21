//----------------------------------------------------------------------------
//  Copyright (C) 2004-2026 by EMGU Corporation. All rights reserved.
//----------------------------------------------------------------------------

using System;
using System.Runtime.InteropServices;
using Emgu.Util;

namespace Emgu.CV.VideoStab
{
    /// <summary>
    /// Describes a global 2D motion estimation method which uses keypoints detection and optical flow for matching.
    /// </summary>
    public class KeypointBasedMotionEstimator : UnmanagedObject
    {
        internal IntPtr ImageMotionEstimatorBasePtr;

        private MotionEstimatorRansacL2 _motionEstimator;

        /// <summary>
        /// Creates a keypoint-based motion estimator backed by the given low-level estimator.
        /// </summary>
        /// <param name="estimator">The underlying motion estimator (e.g. <see cref="MotionEstimatorRansacL2"/>).</param>
        public KeypointBasedMotionEstimator(MotionEstimatorRansacL2 estimator)
        {
            _motionEstimator = estimator;
            _ptr = VideoStabInvoke.cveKeypointBasedMotionEstimatorCreate(
                estimator.MotionEstimatorBasePtr,
                ref ImageMotionEstimatorBasePtr);
        }

        /// <inheritdoc/>
        protected override void DisposeObject()
        {
            if (_ptr != IntPtr.Zero)
            {
                VideoStabInvoke.cveKeypointBasedMotionEstimatorRelease(ref _ptr);
                ImageMotionEstimatorBasePtr = IntPtr.Zero;
            }
        }
    }
}
