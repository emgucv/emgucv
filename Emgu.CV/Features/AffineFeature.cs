//----------------------------------------------------------------------------
//  Copyright (C) 2004-2026 by EMGU Corporation. All rights reserved.
//----------------------------------------------------------------------------

using System;
using System.Runtime.InteropServices;
using Emgu.CV;
using Emgu.Util;

namespace Emgu.CV.Features
{
    /// <summary>
    /// Wrapper that makes detectors and extractors affine invariant, described as ASIFT.
    /// </summary>
    public class AffineFeature : Feature2D
    {
        private Feature2D _backend;

        /// <summary>
        /// Create an affine invariant wrapper around the given detector/extractor.
        /// </summary>
        /// <param name="backend">The detector/extractor to use as backend. The backend must not be disposed before this AffineFeature.</param>
        /// <param name="maxTilt">The highest power index of tilt factor. 5 is used in the paper as tilt sampling range n.</param>
        /// <param name="minTilt">The lowest power index of tilt factor. 0 is used in the paper.</param>
        /// <param name="tiltStep">Tilt sampling step in Algorithm 1 in the paper.</param>
        /// <param name="rotateStepBase">Rotation sampling step factor b in Algorithm 1 in the paper.</param>
        public AffineFeature(
            Feature2D backend,
            int maxTilt = 5,
            int minTilt = 0,
            float tiltStep = 1.4142135623730951f,
            float rotateStepBase = 72)
        {
            _backend = backend;
            _ptr = FeaturesInvoke.cveAffineFeatureCreate(
                backend.Feature2DPtr,
                maxTilt,
                minTilt,
                tiltStep,
                rotateStepBase,
                ref _feature2D,
                ref _sharedPtr);
        }

        /// <summary>
        /// Release the unmanaged resources associated with this object
        /// </summary>
        protected override void DisposeObject()
        {
            if (_sharedPtr != IntPtr.Zero)
                FeaturesInvoke.cveAffineFeatureRelease(ref _sharedPtr);
            _backend = null;
            base.DisposeObject();
        }
    }

    public static partial class FeaturesInvoke
    {
        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern IntPtr cveAffineFeatureCreate(
            IntPtr backend,
            int maxTilt,
            int minTilt,
            float tiltStep,
            float rotateStepBase,
            ref IntPtr feature2D,
            ref IntPtr sharedPtr);

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern void cveAffineFeatureRelease(ref IntPtr sharedPtr);
    }
}
