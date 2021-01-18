//----------------------------------------------------------------------------
//  Copyright (C) 2004-2021 by EMGU Corporation. All rights reserved.       
//----------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Emgu.CV;
using Emgu.CV.Structure;

namespace Emgu.CV.VideoStab
{
    internal static partial class VideoStabInvoke
    {
        static VideoStabInvoke()
        {
            CvInvoke.Init();
        }

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern IntPtr cveVideostabCaptureFrameSourceCreate(IntPtr capture, ref IntPtr frameSourcePtr);

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern void cveVideostabCaptureFrameSourceRelease(ref IntPtr captureFrameSource);

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        [return: MarshalAs(CvInvoke.BoolMarshalType)]
        internal static extern bool cveVideostabFrameSourceGetNextFrame(IntPtr frameSource, IntPtr nextFrame);

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern IntPtr cveOnePassStabilizerCreate(IntPtr capture, ref IntPtr stabilizerBase, ref IntPtr frameSource);

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern void cveOnePassStabilizerSetMotionFilter(IntPtr stabalizer, IntPtr motionFilter);

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern void cveOnePassStabilizerRelease(ref IntPtr stabilizer);

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern IntPtr cveTwoPassStabilizerCreate(IntPtr capture, ref IntPtr stabilizerBase, ref IntPtr frameSource);

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern void cveTwoPassStabilizerRelease(ref IntPtr stabilizer);

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern IntPtr cveGaussianMotionFilterCreate(int radius, float stdev);

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern void cveGaussianMotionFilterRelease(ref IntPtr filter);

        /*
        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern void StabilizerBaseSetMotionEstimator(IntPtr stabilizer, IntPtr motionEstimator);
        */
    }
}
