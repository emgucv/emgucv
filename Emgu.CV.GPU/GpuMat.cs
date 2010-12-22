using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using Emgu.CV.Structure;
using Emgu.Util;

namespace Emgu.CV.GPU
{
   public class GpuMat<TDepth> : UnmanagedObject
      where TDepth : new()
   {
      public GpuMat(int rows, int cols, int channels)
      {
         _ptr = GpuInvoke.gpuMatCreate( rows, cols, CvInvoke.CV_MAKETYPE((int)Util.GetMatrixDepth(typeof(TDepth)), channels));
      }

      protected override void DisposeObject()
      {
         GpuInvoke.gpuMatRelease(ref _ptr);
      }
   }

   public static class GpuInvoke
   {
      static GpuInvoke()
      {
         //Dummy code to make sure the static constructore of CvInvoke has been called and the error handler has been registered.
         using (Image<Gray, Byte> img = new Image<Gray, byte>(12, 8))
         {
            img.Not();
         }
      }

      [DllImport(CvInvoke.EXTERN_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention, EntryPoint = "gpuGetCudaEnabledDeviceCount")]
      public static extern int GetCudaEnabledDeviceCount();

      [DllImport(CvInvoke.EXTERN_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      public static extern IntPtr gpuMatCreate(int rows, int cols, int type);

      [DllImport(CvInvoke.EXTERN_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      public static extern void gpuMatRelease(ref IntPtr mat);
   }
}
