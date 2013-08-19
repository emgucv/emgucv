//----------------------------------------------------------------------------
//  Copyright (C) 2004-2013 by EMGU. All rights reserved.       
//----------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Emgu.CV;
using Emgu.CV.GPU;
using Emgu.CV.Structure;

namespace Emgu.CV.VideoStab
{
   internal static partial class VideoStabInvoke
   {
      static VideoStabInvoke()
      {
         //Dummy code to make sure the static constructor of GpuInvoke has been called
         bool hasCuda = GpuInvoke.HasCuda;
      }

      [DllImport(CvInvoke.EXTERN_GPU_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      internal static extern IntPtr CaptureFrameSourceCreate(IntPtr capture, ref IntPtr frameSourcePtr);

      [DllImport(CvInvoke.EXTERN_GPU_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      internal static extern void CaptureFrameSourceRelease(ref IntPtr captureFrameSource);

      [DllImport(CvInvoke.EXTERN_GPU_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      [return: MarshalAs(CvInvoke.BoolMarshalType)]
      internal static extern bool FrameSourceGetNextFrame(IntPtr frameSource, ref IntPtr nextFrame);

      [DllImport(CvInvoke.EXTERN_GPU_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      internal static extern IntPtr OnePassStabilizerCreate(IntPtr capture, ref IntPtr stabilizerBase, ref IntPtr frameSource);

      [DllImport(CvInvoke.EXTERN_GPU_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      internal static extern void OnePassStabilizerSetMotionFilter(IntPtr stabalizer, IntPtr motionFilter);

      [DllImport(CvInvoke.EXTERN_GPU_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      internal static extern void OnePassStabilizerRelease(ref IntPtr stabilizer);

      [DllImport(CvInvoke.EXTERN_GPU_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      internal static extern IntPtr TwoPassStabilizerCreate(IntPtr capture, ref IntPtr stabilizerBase, ref IntPtr frameSource);

      [DllImport(CvInvoke.EXTERN_GPU_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      internal static extern void TwoPassStabilizerRelease(ref IntPtr stabilizer);

      [DllImport(CvInvoke.EXTERN_GPU_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      internal static extern IntPtr GaussianMotionFilterCreate(int radius, float stdev);

      [DllImport(CvInvoke.EXTERN_GPU_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      internal static extern void GaussianMotionFilterRelease(ref IntPtr filter);

      [DllImport(CvInvoke.EXTERN_GPU_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      internal static extern void StabilizerBaseSetMotionEstimator(IntPtr stabilizer, IntPtr motionEstimator);
   }
}
