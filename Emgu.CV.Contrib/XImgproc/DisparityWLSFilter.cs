//----------------------------------------------------------------------------
//  Copyright (C) 2004-2018 by EMGU Corporation. All rights reserved.       
//----------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
using System.Text;
using System.Runtime.InteropServices;
using Emgu.CV.CvEnum;
using Emgu.CV.Structure;
using Emgu.CV.Text;
using Emgu.CV.Util;
using Emgu.Util;

namespace Emgu.CV.XImgproc
{

    public class DisparityWLSFilter : SharedPtrObject, IAlgorithm, IDisparityFilter
    {
        private IntPtr _algorithm;
        private IntPtr _disparityFilterPtr;

        public IntPtr AlgorithmPtr
        {
            get { return _algorithm; }
        }

        public IntPtr DisparityFilterPtr
        {
            get { return _disparityFilterPtr; }
        }

        public DisparityWLSFilter(IStereoMatcher matcherLeft)
        {
            _ptr = XImgprocInvoke.cveCreateDisparityWLSFilter(
                matcherLeft.StereoMatcherPtr, 
                ref _disparityFilterPtr,
                ref _algorithm, 
                ref _sharedPtr);
        }

        public DisparityWLSFilter(bool useConfidence)
        {
            _ptr = XImgprocInvoke.cveCreateDisparityWLSFilterGeneric(
                useConfidence,
                ref _disparityFilterPtr,
                ref _algorithm,
                ref _sharedPtr);
        }

        protected override void DisposeObject()
        {
            if (_sharedPtr != IntPtr.Zero)
            {
                XImgprocInvoke.cveDisparityWLSFilterRelease(ref _sharedPtr);
                _ptr = IntPtr.Zero;
            }
        }
    }

    public class RightMatcher : SharedPtrObject, IStereoMatcher
    {
        public IntPtr StereoMatcherPtr
        {
            get { return _ptr; }
        }

        public RightMatcher(IStereoMatcher matcherLeft)
        {
            _ptr = XImgprocInvoke.cveCreateRightMatcher(matcherLeft.StereoMatcherPtr, ref _sharedPtr);
        }

        protected override void DisposeObject()
        {
            if (_sharedPtr != IntPtr.Zero)
            {
                Calib3dInvoke.cveStereoMatcherRelease(ref _sharedPtr);
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
        internal static extern IntPtr cveCreateDisparityWLSFilterGeneric(bool useConfidence, ref IntPtr disparityFilter, ref IntPtr algorithm, ref IntPtr sharedPtr);

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern void cveDisparityWLSFilterRelease(ref IntPtr sharedPtr);
    }
}
