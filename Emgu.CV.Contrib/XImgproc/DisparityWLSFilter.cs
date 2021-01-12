//----------------------------------------------------------------------------
//  Copyright (C) 2004-2021 by EMGU Corporation. All rights reserved.       
//----------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using Emgu.CV.CvEnum;
using Emgu.CV.Structure;
using Emgu.CV.Text;
using Emgu.CV.Util;
using Emgu.Util;

namespace Emgu.CV.XImgproc
{
    /// <summary>
    /// Disparity map filter based on Weighted Least Squares filter (in form of Fast Global Smoother that is a lot faster than traditional Weighted Least Squares filter implementations) and optional use of left-right-consistency-based confidence to refine the results in half-occlusions and uniform areas.
    /// </summary>
    public class DisparityWLSFilter : SharedPtrObject, IAlgorithm, IDisparityFilter
    {
        private IntPtr _algorithm;
        private IntPtr _disparityFilterPtr;

        /// <summary>
        /// Pointer to cv::Algorithm
        /// </summary>
        public IntPtr AlgorithmPtr
        {
            get { return _algorithm; }
        }

        /// <summary>
        /// Pointer to the native DisparityFilter
        /// </summary>
        public IntPtr DisparityFilterPtr
        {
            get { return _disparityFilterPtr; }
        }

        /// <summary>
        /// Creates an instance of DisparityWLSFilter and sets up all the relevant filter parameters automatically based on the matcher instance. Currently supports only StereoBM and StereoSGBM.
        /// </summary>
        /// <param name="matcherLeft">stereo matcher instance that will be used with the filter</param>
        public DisparityWLSFilter(IStereoMatcher matcherLeft)
        {
            _ptr = XImgprocInvoke.cveCreateDisparityWLSFilter(
                matcherLeft.StereoMatcherPtr, 
                ref _disparityFilterPtr,
                ref _algorithm, 
                ref _sharedPtr);
        }

        /// <summary>
        /// Create instance of DisparityWLSFilter and execute basic initialization routines. When using this method you will need to set-up the ROI, matchers and other parameters by yourself.
        /// </summary>
        /// <param name="useConfidence">Filtering with confidence requires two disparity maps (for the left and right views) and is approximately two times slower. However, quality is typically significantly better.</param>
        public DisparityWLSFilter(bool useConfidence)
        {
            _ptr = XImgprocInvoke.cveCreateDisparityWLSFilterGeneric(
                useConfidence,
                ref _disparityFilterPtr,
                ref _algorithm,
                ref _sharedPtr);
        }

        /// <summary>
        /// Release the unmanaged memory associated with this DisparityWLSFilter
        /// </summary>
        protected override void DisposeObject()
        {
            if (_sharedPtr != IntPtr.Zero)
            {
                XImgprocInvoke.cveDisparityWLSFilterRelease(ref _sharedPtr);
                _ptr = IntPtr.Zero;
            }
        }
    }

    /// <summary>
    /// The matcher for computing the right-view disparity map that is required in case of filtering with confidence.
    /// </summary>
    public class RightMatcher : SharedPtrObject, IStereoMatcher
    {
        /// <summary>
        /// Pointer to the stereo matcher
        /// </summary>
        public IntPtr StereoMatcherPtr
        {
            get { return _ptr; }
        }

        /// <summary>
        /// Set up the matcher for computing the right-view disparity map that is required in case of filtering with confidence.
        /// </summary>
        /// <param name="matcherLeft">Main stereo matcher instance that will be used with the filter</param>
        public RightMatcher(IStereoMatcher matcherLeft)
        {
            _ptr = XImgprocInvoke.cveCreateRightMatcher(matcherLeft.StereoMatcherPtr, ref _sharedPtr);
        }

        /// <summary>
        /// Release the unmanaged memory associated with the RightMatcher
        /// </summary>
        protected override void DisposeObject()
        {
            if (_sharedPtr != IntPtr.Zero)
            {
                CvInvoke.cveStereoMatcherRelease(ref _sharedPtr);
                _ptr = IntPtr.Zero;
            }
        }
    }

    public static partial class XImgprocInvoke
    {
        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern IntPtr cveCreateDisparityWLSFilter(IntPtr matcherLeft, ref IntPtr disparityFilter, ref IntPtr algorithm, ref IntPtr sharedPtr);

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern IntPtr cveCreateRightMatcher(IntPtr matcherLeft, ref IntPtr sharedPtr);

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern IntPtr cveCreateDisparityWLSFilterGeneric(
            [MarshalAs(CvInvoke.BoolMarshalType)]
            bool useConfidence, 
            ref IntPtr disparityFilter, 
            ref IntPtr algorithm, 
            ref IntPtr sharedPtr);

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern void cveDisparityWLSFilterRelease(ref IntPtr sharedPtr);
    }
}
