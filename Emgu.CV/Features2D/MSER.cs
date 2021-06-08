//----------------------------------------------------------------------------
//  Copyright (C) 2004-2021 by EMGU Corporation. All rights reserved.       
//----------------------------------------------------------------------------

using System;
using System.Runtime.InteropServices;
using System.Drawing;
using Emgu.CV.Structure;
using Emgu.CV.Util;
using Emgu.Util;

namespace Emgu.CV.Features2D
{
    /// <summary>
    /// MSER detector
    /// </summary>
    public class MSERDetector : Feature2D
    {
        /// <summary>
        /// Create a MSER detector using the specific parameters
        /// </summary>
        /// <param name="delta">In the code, it compares (size_{i}-size_{i-delta})/size_{i-delta}</param>
        /// <param name="maxArea">Prune the area which bigger than max_area</param>
        /// <param name="minArea">Prune the area which smaller than min_area</param>
        /// <param name="maxVariation">Prune the area have similar size to its children</param>
        /// <param name="minDiversity">Trace back to cut off mser with diversity &lt; min_diversity</param>
        /// <param name="maxEvolution">For color image, the evolution steps</param>
        /// <param name="areaThreshold">The area threshold to cause re-initialize</param>
        /// <param name="minMargin">Ignore too small margin</param>
        /// <param name="edgeBlurSize">The aperture size for edge blur</param>
        public MSERDetector(
           int delta = 5, int minArea = 60, int maxArea = 14400, double maxVariation = 0.25, double minDiversity = 0.2,
           int maxEvolution = 200, double areaThreshold = 1.01, double minMargin = 0.003, int edgeBlurSize = 5)
        {
            _ptr = Features2DInvoke.cveMserGetFeatureDetector(
               delta,
               minArea,
               maxArea,
               maxVariation,
               minDiversity,
               maxEvolution,
               areaThreshold,
               minMargin,
               edgeBlurSize,
               ref _feature2D, 
               ref _sharedPtr);
        }

        /// <summary>
        /// Release the unmanaged memory associated with this detector.
        /// </summary>
        protected override void DisposeObject()
        {
            if (_sharedPtr != IntPtr.Zero)
                Features2DInvoke.cveMserFeatureDetectorRelease(ref _sharedPtr);
            base.DisposeObject();
        }

        /// <summary>
        /// Detect MSER regions
        /// </summary>
        /// <param name="image">input image (8UC1, 8UC3 or 8UC4, must be greater or equal than 3x3)</param>
        /// <param name="msers">resulting list of point sets</param>
        /// <param name="bboxes">resulting bounding boxes</param>
        public void DetectRegions(IInputArray image, VectorOfVectorOfPoint msers, VectorOfRect bboxes)
        {
            using (InputArray iaImage = image.GetInputArray())
                Features2DInvoke.cveMserDetectRegions(_ptr, iaImage, msers, bboxes);
        }
    }

    public static partial class Features2DInvoke
    {
        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal extern static IntPtr cveMserGetFeatureDetector(
           int delta,
           int minArea,
           int maxArea,
           double maxVariation,
           double minDiversity,
           int maxEvolution,
           double areaThreshold,
           double minMargin,
           int edgeBlurSize,
           ref IntPtr feature2D,
           ref IntPtr sharedPtr);

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal extern static void cveMserFeatureDetectorRelease(ref IntPtr sharedPtr);

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal extern static void cveMserDetectRegions(
            IntPtr mser,
            IntPtr image,
            IntPtr msers,
            IntPtr bboxes);
    }
}