using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;
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
      internal static extern IntPtr CaptureFrameSourceCreate(IntPtr capture);

      [DllImport(CvInvoke.EXTERN_GPU_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      internal static extern void CaptureFrameSourceRelease(ref IntPtr captureFrameSource);

      [DllImport(CvInvoke.EXTERN_GPU_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      [return: MarshalAs(CvInvoke.BoolMarshalType)]
      internal static extern bool CaptureFrameSourceGetNextFrame(IntPtr captureFrameSource, ref IntPtr nextFrame);

      [DllImport(CvInvoke.EXTERN_GPU_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      internal static extern IntPtr OnePassStabilizerCreate(IntPtr capture);

      [DllImport(CvInvoke.EXTERN_GPU_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      internal static extern void OnePassStabilizerRelease(ref IntPtr stabilizer);
   }
}
