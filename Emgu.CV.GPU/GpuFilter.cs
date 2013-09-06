//----------------------------------------------------------------------------
//  Copyright (C) 2004-2013 by EMGU. All rights reserved.       
//----------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;
using Emgu.CV.Features2D;
using Emgu.CV.Structure;
using Emgu.CV.Util;
using Emgu.Util;

namespace Emgu.CV.GPU
{
   public abstract class GpuFilter : UnmanagedObject
   {
      protected override void DisposeObject()
      {
         if (_ptr != null)
            GpuInvoke.gpuFilterRelease(ref _ptr);
      }  
   }

   /// <summary>
   /// Applies arbitrary linear filter to the image. In-place operation is supported. When the aperture is partially outside the image, the function interpolates outlier pixel values from the nearest pixels that is inside the image
   /// </summary>
   public class GpuLinearFilter : GpuFilter
   {
      /// <summary>
      /// Create a Gpu LinearFilter
      /// </summary>
      /// <param name="kernel">Convolution kernel, single-channel floating point matrix (e.g. Emgu.CV.Matrix). If you want to apply different kernels to different channels, split the gpu image into separate color planes and process them individually</param>
      /// <param name="anchor">The anchor of the kernel that indicates the relative position of a filtered point within the kernel. The anchor shoud lie within the kernel. The special default value (-1,-1) means that it is at the kernel center</param>
      /// <param name="borderType">Border type. Use REFLECT101 as default.</param>
      /// <param name="borderValue">The border value</param>
      public GpuLinearFilter(int srcType, int dstType, IntPtr kernel, System.Drawing.Point anchor, CvEnum.BORDER_TYPE borderType, MCvScalar borderValue)
      {
         _ptr = GpuInvoke.gpuCreateLinearFilter(srcType, dstType, kernel, ref anchor, borderType, ref borderValue);

      }
      /*
      /// <summary>
      /// 
      /// </summary>
      /// <param name="src">The source GpuMat</param>
      /// <param name="dst">The destination GpuMmage</param>
      /// <param name="kernel">Convolution kernel, single-channel floating point matrix (e.g. Emgu.CV.Matrix). If you want to apply different kernels to different channels, split the gpu image into separate color planes and process them individually</param>
      /// <param name="anchor">The anchor of the kernel that indicates the relative position of a filtered point within the kernel. The anchor shoud lie within the kernel. The special default value (-1,-1) means that it is at the kernel center</param>
      /// <param name="borderType">Border type. Use REFLECT101 as default.</param>
      /// <param name="stream">Use a Stream to call the function asynchronously (non-blocking) or IntPtr.Zero to call the function synchronously (blocking).</param>
      [DllImport(CvInvoke.EXTERN_GPU_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention, EntryPoint = "gpuMatFilter2D")]
      public static extern void Filter2D(IntPtr src, IntPtr dst, IntPtr kernel, Point anchor, CvEnum.BORDER_TYPE borderType, IntPtr stream);
      */
   }

   public static partial class GpuInvoke
   {
      [DllImport(CvInvoke.EXTERN_GPU_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      internal static extern void gpuFilterApply(IntPtr filter, IntPtr image, IntPtr dst, IntPtr stream);

      [DllImport(CvInvoke.EXTERN_GPU_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      internal static extern void gpuFilterRelease(ref IntPtr filter);

            [DllImport(CvInvoke.EXTERN_GPU_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention, EntryPoint = "gpuMatFilter2D")]
      internal static extern IntPtr gpuCreateLinearFilter(int srcType, int dstType, IntPtr kernel, ref System.Drawing.Point anchor, CvEnum.BORDER_TYPE borderMode, ref MCvScalar borderValue);
   }

}
