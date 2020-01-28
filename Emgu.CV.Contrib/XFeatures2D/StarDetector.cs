//----------------------------------------------------------------------------
//  Copyright (C) 2004-2020 by EMGU Corporation. All rights reserved.       
//----------------------------------------------------------------------------

using System;
using System.Runtime.InteropServices;
using Emgu.CV.Structure;
using Emgu.CV.Util;
using Emgu.Util;
using Emgu.CV.Features2D;

namespace Emgu.CV.XFeatures2D
{
    /// <summary>
    /// StarDetector
    /// </summary>
    public class StarDetector : Feature2D
    {

        /// <summary>
        /// Create a star detector with the specific parameters
        /// </summary>
        /// <param name="maxSize">
        /// Maximum size of the features. The following
        /// values of the parameter are supported:
        /// 4, 6, 8, 11, 12, 16, 22, 23, 32, 45, 46, 64, 90, 128</param>
        /// <param name="responseThreshold">
        /// Threshold for the approximated laplacian,
        /// used to eliminate weak features. The larger it is,
        /// the less features will be retrieved
        /// </param>
        /// <param name="lineThresholdProjected">
        /// Another threshold for the laplacian to eliminate edges.
        /// The larger the threshold, the more points you get.
        /// </param>
        /// <param name="lineThresholdBinarized">
        /// Another threshold for the feature size to eliminate edges. 
        /// The larger the threshold, the more points you get.</param>
        /// <param name="suppressNonmaxSize">
        /// Suppress Nonmax Size
        /// </param>
        public StarDetector(
            int maxSize = 45, int responseThreshold = 30, int lineThresholdProjected = 10,
            int lineThresholdBinarized = 8, int suppressNonmaxSize = 5)
        {
            _ptr = XFeatures2DInvoke.cveStarDetectorCreate(
                maxSize, responseThreshold, lineThresholdProjected,
                lineThresholdBinarized, suppressNonmaxSize, ref _feature2D,
                ref _sharedPtr);
        }

        /// <summary>
        /// Release the unmanaged memory associated with this detector.
        /// </summary>
        protected override void DisposeObject()
        {
            if (_sharedPtr != IntPtr.Zero)
                XFeatures2DInvoke.cveStarDetectorRelease(ref _sharedPtr);
            base.DisposeObject();
        }
    }

    public static partial class XFeatures2DInvoke
    {

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal extern static IntPtr cveStarDetectorCreate(int maxSize, int responseThreshold, int lineThresholdProjected, int lineThresholdBinarized, int suppressNonmaxSize, ref IntPtr feature2D, ref IntPtr sharedPtr);

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal extern static void cveStarDetectorRelease(ref IntPtr sharedPtr);
    }
}
