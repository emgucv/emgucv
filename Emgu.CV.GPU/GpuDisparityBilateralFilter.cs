//----------------------------------------------------------------------------
//  Copyright (C) 2004-2013 by EMGU. All rights reserved.       
//----------------------------------------------------------------------------

﻿using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;
using Emgu.CV.Structure;
using Emgu.Util;

namespace Emgu.CV.GPU
{
   /// <summary>
   /// Disparity map refinement using joint bilateral filtering given a single color image.
   /// Qingxiong Yang, Liang Wang†, Narendra Ahuja
   /// http://vision.ai.uiuc.edu/~qyang6/
   /// </summary>
   public class GpuDisparityBilateralFilter : UnmanagedObject
   {
      /// <summary>
      /// Create a GpuDisparityBilateralFilter
      /// </summary>
      /// <param name="ndisp">Number of disparities. Use 64 as default</param>
      /// <param name="radius">Filter radius, use 3 as default</param>
      /// <param name="iters">Number of iterations, use 1 as default</param>
      /*
      /// <param name="edgeThreshold">Truncation of data continuity, use 0.1 as default</param>
      /// <param name="maxDiscThreshold">Truncation of disparity continuity, use 0.2 as default</param>
      /// <param name="sigmaRange">Filter range sigma, use 10.0 as default</param>*/
      public GpuDisparityBilateralFilter(int ndisp, int radius, int iters)
      {
         _ptr = GpuInvoke.GpuDisparityBilateralFilterCreate(ndisp, radius, iters);
      }

      /// <summary>
      /// Apply the filter to the disparity image
      /// </summary>
      /// <param name="disparity">The input disparity map</param>
      /// <param name="image">The image</param>
      /// <param name="dst">The output disparity map, should have the same size as the input disparity map</param>
      /// <param name="stream">Use a Stream to call the function asynchronously (non-blocking) or null to call the function synchronously (blocking).</param>
      public void Apply(GpuImage<Gray, Byte> disparity, GpuImage<Gray, Byte> image, GpuImage<Gray, byte> dst, Stream stream)
      {
         GpuInvoke.GpuDisparityBilateralFilterApply(_ptr, disparity, image, dst, stream);
      }

      /// <summary>
      /// Release the unmanaged resources associated with the filter.
      /// </summary>
      protected override void DisposeObject()
      {
         GpuInvoke.GpuDisparityBilateralFilterRelease(ref _ptr);
      }
   }

   public static partial class GpuInvoke
   {
      [DllImport(CvInvoke.EXTERN_GPU_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      internal extern static IntPtr GpuDisparityBilateralFilterCreate(int ndisp, int radius, int iters);

      [DllImport(CvInvoke.EXTERN_GPU_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      internal extern static void GpuDisparityBilateralFilterApply(IntPtr filter, IntPtr disparity, IntPtr image, IntPtr dst, IntPtr stream);

      [DllImport(CvInvoke.EXTERN_GPU_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      internal extern static void GpuDisparityBilateralFilterRelease(ref IntPtr filter);
   }
}
