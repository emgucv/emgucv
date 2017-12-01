//----------------------------------------------------------------------------
//  Copyright (C) 2004-2017 by EMGU Corporation. All rights reserved.       
//----------------------------------------------------------------------------

using System;
using System.Runtime.InteropServices;
using Emgu.CV;
using Emgu.CV.Structure;
using Emgu.Util;

namespace Emgu.CV.BgSegm
{

    public class BackgroundSubtractorCNT : UnmanagedObject, IBackgroundSubtractor
    {
        private IntPtr _algorithmPtr;
        private IntPtr _backgroundSubtractorPtr;
        public IntPtr AlgorithmPtr { get { return _algorithmPtr; } }
        public IntPtr BackgroundSubtractorPtr { get { return _backgroundSubtractorPtr; } }

        public BackgroundSubtractorCNT(
            int minPixelStability = 15,
            bool useHistory = true,
            int maxPixelStability = 15 * 60,
            bool isParallel = true)
        {
            _ptr = ContribInvoke.cveBackgroundSubtractorCNTCreate(
                minPixelStability,
                useHistory,
                maxPixelStability,
                isParallel,
                ref _backgroundSubtractorPtr,
                ref _algorithmPtr);
        }

        /// <summary>
        /// Release all the unmanaged memory associated with this background model.
        /// </summary>
        protected override void DisposeObject()
        {
            if (IntPtr.Zero != _ptr)
            {
                ContribInvoke.cveBackgroundSubtractorCNTRelease(ref _ptr);
                _backgroundSubtractorPtr = IntPtr.Zero;
                _algorithmPtr = IntPtr.Zero;
            }
        }
    }
}

namespace Emgu.CV
{
    public static partial class ContribInvoke
    {
        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern IntPtr cveBackgroundSubtractorCNTCreate(
            int minPixelStability,
            [MarshalAs(CvInvoke.BoolMarshalType)]
            bool useHistory,
            int maxPixelStability,
            [MarshalAs(CvInvoke.BoolMarshalType)]
            bool isParallel,
            ref IntPtr bgSubtractor,
            ref IntPtr algorithm);

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern void cveBackgroundSubtractorCNTRelease(ref IntPtr bgSubstractor);
    }
}
