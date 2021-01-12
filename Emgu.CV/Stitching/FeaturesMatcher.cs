//----------------------------------------------------------------------------
//  Copyright (C) 2004-2021 by EMGU Corporation. All rights reserved.       
//----------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;
using System.Drawing;
using Emgu.CV.Structure;
using Emgu.CV.Util;
using Emgu.Util;

namespace Emgu.CV.Stitching
{
    /// <summary>
    /// Features Matcher for Image Stitching
    /// </summary>
    public abstract class FeaturesMatcher : UnmanagedObject
    {
        /// <summary>
        /// Pointer to the native FeaturesMatcher object.
        /// </summary>
        protected IntPtr _featuresMatcherPtr;

        /// <summary>
        /// Pointer to the native FeaturesMatcher object.
        /// </summary>
        public IntPtr FeaturesMatcherPtr
        {
            get { return _featuresMatcherPtr; }
        }

        /// <summary>
        /// Reset the unmanaged pointer associated to this object
        /// </summary>
        protected override void DisposeObject()
        {
            if (_featuresMatcherPtr != IntPtr.Zero)
                _featuresMatcherPtr = IntPtr.Zero;
        }
    }

    /// <summary>
    /// Features matcher which finds two best matches for each feature and leaves the best one only if the ratio between descriptor distances is greater than the threshold match_conf.
    /// </summary>
    public class BestOf2NearestMatcher : FeaturesMatcher
    {
        /// <summary>
        /// Create a new features matcher
        /// </summary>
        /// <param name="tryUseGpu">If true, will try to use gpu.</param>
        /// <param name="matchConf">Match confident</param>
        /// <param name="numMatchesThresh1">Number of matches threshold</param>
        /// <param name="numMatchesThresh2">Number of matches threshold</param>
        public BestOf2NearestMatcher(
			bool tryUseGpu = false, 
			float matchConf = 0.3f, 
			int numMatchesThresh1 = 6,
            int numMatchesThresh2 = 6)
        {
            _ptr = StitchingInvoke.cveBestOf2NearestMatcherCreate(
                tryUseGpu, 
                matchConf, 
                numMatchesThresh1, 
                numMatchesThresh2, 
                ref _featuresMatcherPtr);
        }

        /// <summary>
        /// Release all the unmanaged memory associated with this blender
        /// </summary>
        protected override void DisposeObject()
        {
            base.DisposeObject();
            if (_ptr != IntPtr.Zero)
            {
                StitchingInvoke.cveBestOf2NearestMatcherRelease(ref _ptr);
            }
        }
    }

    /// <summary>
    /// Best of 2 nearest range matcher
    /// </summary>
    public class BestOf2NearestRangeMatcher : FeaturesMatcher
    {
        /// <summary>
        /// Create a new instance of BestOf2NearestRangeMatcher
        /// </summary>
        /// <param name="rangeWidth">Range width</param>
        /// <param name="tryUseGpu">If true, will try to use GPU</param>
        /// <param name="matchConf">Match confident</param>
        /// <param name="numMatchesThresh1">Number of matches threshold</param>
        /// <param name="numMatchesThresh2">Number of matches threshold</param>
        public BestOf2NearestRangeMatcher(
            int rangeWidth = 5,
            bool tryUseGpu = false,
            float matchConf = 0.3f,
            int numMatchesThresh1 = 6,
            int numMatchesThresh2 = 6)
        {
            _ptr = StitchingInvoke.cveBestOf2NearestRangeMatcherCreate(
                rangeWidth,
                tryUseGpu, 
                matchConf, 
                numMatchesThresh1, 
                numMatchesThresh2, 
                ref _featuresMatcherPtr);
        }

        /// <summary>
        /// Release all the unmanaged memory associated with this blender
        /// </summary>
        protected override void DisposeObject()
        {
            base.DisposeObject();
            if (_ptr != IntPtr.Zero)
            {
                StitchingInvoke.cveBestOf2NearestRangeMatcherRelease(ref _ptr);
            }
        }
    }

    /// <summary>
    /// Features matcher similar to BestOf2NearestMatcher which finds two best matches for each feature and leaves the best one only if the ratio between descriptor distances is greater than the threshold match_conf.
    /// </summary>
    public class AffineBestOf2NearestMatcher : FeaturesMatcher
    {
        /// <summary>
        /// Create a new features matcher
        /// </summary>
        /// <param name="fullAffine">Full Affine</param>
        /// <param name="tryUseGpu">If true, will try to use gpu</param>
        /// <param name="matchConf">Match confident</param>
        /// <param name="numMatchesThresh1">Number of matches threshold</param>
        public AffineBestOf2NearestMatcher(
            bool fullAffine = false,
            bool tryUseGpu = false,
            float matchConf = 0.3f,
            int numMatchesThresh1 = 6)
        {
            _ptr = StitchingInvoke.cveAffineBestOf2NearestMatcherCreate(
                fullAffine,
                tryUseGpu,
                matchConf,
                numMatchesThresh1,
                ref _featuresMatcherPtr);
        }

        /// <summary>
        /// Release all the unmanaged memory associated with this blender
        /// </summary>
        protected override void DisposeObject()
        {
            base.DisposeObject();
            if (_ptr != IntPtr.Zero)
            {
                StitchingInvoke.cveAffineBestOf2NearestMatcherRelease(ref _ptr);
            }
        }
    }


    public static partial class StitchingInvoke
    {

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern IntPtr cveBestOf2NearestMatcherCreate(
            [MarshalAs(CvInvoke.BoolMarshalType)]
            bool tryUseGpu,
            float matchConf,
            int numMatchesThresh1,
            int numMatchesThresh2, 
            ref IntPtr featuresMatcher);
        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern void cveBestOf2NearestMatcherRelease(ref IntPtr blender);

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern IntPtr cveBestOf2NearestRangeMatcherCreate(
            int rangeWidth,
            [MarshalAs(CvInvoke.BoolMarshalType)]
            bool tryUseGpu,
            float matchConf,
            int numMatchesThresh1,
            int numMatchesThresh2,
            ref IntPtr featuresMatcher);
        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern void cveBestOf2NearestRangeMatcherRelease(ref IntPtr featuresMatcher);

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern IntPtr cveAffineBestOf2NearestMatcherCreate(
            [MarshalAs(CvInvoke.BoolMarshalType)]
            bool fullAffine,
            [MarshalAs(CvInvoke.BoolMarshalType)]
            bool tryUseGpu,
            float matchConf,
            int numMatchesThresh1,
            ref IntPtr featuresMatcher);
        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern void cveAffineBestOf2NearestMatcherRelease(ref IntPtr featuresMatcher);
    }
}
