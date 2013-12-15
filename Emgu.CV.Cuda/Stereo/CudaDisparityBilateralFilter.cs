//----------------------------------------------------------------------------
//  Copyright (C) 2004-2013 by EMGU. All rights reserved.       
//----------------------------------------------------------------------------

﻿using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;
using Emgu.CV.Structure;
using Emgu.Util;

namespace Emgu.CV.Cuda
{
   /// <summary>
   /// Disparity map refinement using joint bilateral filtering given a single color image.
   /// Qingxiong Yang, Liang Wang†, Narendra Ahuja
   /// http://vision.ai.uiuc.edu/~qyang6/
   /// </summary>
   public class CudaDisparityBilateralFilter : UnmanagedObject
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
      public CudaDisparityBilateralFilter(int ndisp, int radius, int iters)
      {
         _ptr = CudaInvoke.cudaDisparityBilateralFilterCreate(ndisp, radius, iters);
      }

      /// <summary>
      /// Apply the filter to the disparity image
      /// </summary>
      /// <param name="disparity">The input disparity map</param>
      /// <param name="image">The image</param>
      /// <param name="dst">The output disparity map, should have the same size as the input disparity map</param>
      /// <param name="stream">Use a Stream to call the function asynchronously (non-blocking) or null to call the function synchronously (blocking).</param>
      public void Apply(CudaImage<Gray, Byte> disparity, CudaImage<Gray, Byte> image, CudaImage<Gray, byte> dst, Stream stream)
      {
         CudaInvoke.cudaDisparityBilateralFilterApply(this, disparity.InputArrayPtr, image.InputArrayPtr, dst.OutputArrayPtr, stream);
      }

      /// <summary>
      /// Release the unmanaged resources associated with the filter.
      /// </summary>
      protected override void DisposeObject()
      {
         CudaInvoke.cudaDisparityBilateralFilterRelease(ref _ptr);
      }
   }

   public static partial class CudaInvoke
   {
      [DllImport(CvInvoke.EXTERN_CUDA_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      internal extern static IntPtr cudaDisparityBilateralFilterCreate(int ndisp, int radius, int iters);

      [DllImport(CvInvoke.EXTERN_CUDA_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      internal extern static void cudaDisparityBilateralFilterApply(IntPtr filter, IntPtr disparity, IntPtr image, IntPtr dst, IntPtr stream);

      [DllImport(CvInvoke.EXTERN_CUDA_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      internal extern static void cudaDisparityBilateralFilterRelease(ref IntPtr filter);
   }
}
