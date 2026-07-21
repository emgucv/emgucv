//----------------------------------------------------------------------------
//  Copyright (C) 2004-2026 by EMGU Corporation. All rights reserved.
//----------------------------------------------------------------------------

using System;
using System.Runtime.InteropServices;
using Emgu.Util;

namespace Emgu.CV.VideoStab
{
    /// <summary>
    /// Describes a robust RANSAC-based global 2D motion estimation method which minimizes L2 error.
    /// </summary>
    public class MotionEstimatorRansacL2 : UnmanagedObject
    {
        internal IntPtr MotionEstimatorBasePtr;

        /// <summary>
        /// Creates a new RANSAC-based L2 motion estimator.
        /// </summary>
        /// <param name="model">The motion model to use.</param>
        public MotionEstimatorRansacL2(MotionModel model = MotionModel.Affine)
        {
            _ptr = VideoStabInvoke.cveMotionEstimatorRansacL2Create((int)model, ref MotionEstimatorBasePtr);
        }

        /// <inheritdoc/>
        protected override void DisposeObject()
        {
            if (_ptr != IntPtr.Zero)
            {
                VideoStabInvoke.cveMotionEstimatorRansacL2Release(ref _ptr);
                MotionEstimatorBasePtr = IntPtr.Zero;
            }
        }
    }
}
