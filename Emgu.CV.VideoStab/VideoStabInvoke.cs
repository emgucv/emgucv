//----------------------------------------------------------------------------
//  Copyright (C) 2004-2015 by EMGU Corporation. All rights reserved.       
//----------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Emgu.CV;
#if !(IOS || UNITY_IPHONE || NETFX_CORE)
using Emgu.CV.Cuda;
#endif
using Emgu.CV.Structure;

namespace Emgu.CV.VideoStab
{
   internal static partial class VideoStabInvoke
   {
#if !(IOS || UNITY_IPHONE || NETFX_CORE)
      static VideoStabInvoke()
      {
         //Dummy code to make sure the static constructor of GpuInvoke has been called
         bool hasCuda = CudaInvoke.HasCuda;
      }
#endif

      [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      internal static extern IntPtr VideostabCaptureFrameSourceCreate(IntPtr capture, ref IntPtr frameSourcePtr);

      [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      internal static extern void VideostabCaptureFrameSourceRelease(ref IntPtr captureFrameSource);

      [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      [return: MarshalAs(CvInvoke.BoolMarshalType)]
      internal static extern bool VideostabFrameSourceGetNextFrame(IntPtr frameSource, IntPtr nextFrame);

      [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      internal static extern IntPtr OnePassStabilizerCreate(IntPtr capture, ref IntPtr stabilizerBase, ref IntPtr frameSource);

      [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      internal static extern void OnePassStabilizerSetMotionFilter(IntPtr stabalizer, IntPtr motionFilter);

      [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      internal static extern void OnePassStabilizerRelease(ref IntPtr stabilizer);

      [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      internal static extern IntPtr TwoPassStabilizerCreate(IntPtr capture, ref IntPtr stabilizerBase, ref IntPtr frameSource);

      [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      internal static extern void TwoPassStabilizerRelease(ref IntPtr stabilizer);

      [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      internal static extern IntPtr GaussianMotionFilterCreate(int radius, float stdev);

      [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      internal static extern void GaussianMotionFilterRelease(ref IntPtr filter);

      [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      internal static extern void StabilizerBaseSetMotionEstimator(IntPtr stabilizer, IntPtr motionEstimator);
   }
}
