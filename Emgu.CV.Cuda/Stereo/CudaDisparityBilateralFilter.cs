//----------------------------------------------------------------------------
//  Copyright (C) 2004-2021 by EMGU Corporation. All rights reserved.       
//----------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;
using Emgu.CV.Structure;
using Emgu.CV.Util;
using Emgu.Util;

namespace Emgu.CV.Cuda
{
   /// <summary>
   /// Disparity map refinement using joint bilateral filtering given a single color image.
   /// Qingxiong Yang, Liang Wang†, Narendra Ahuja
   /// http://vision.ai.uiuc.edu/~qyang6/
   /// </summary>
   public class CudaDisparityBilateralFilter : SharedPtrObject
   {
      /// <summary>
      /// Create a GpuDisparityBilateralFilter
      /// </summary>
      /// <param name="ndisp">Number of disparities. Use 64 as default</param>
      /// <param name="radius">Filter radius, use 3 as default</param>
      /// <param name="iters">Number of iterations, use 1 as default</param>
      public CudaDisparityBilateralFilter(int ndisp = 64, int radius = 3, int iters = 1)
      {
         _ptr = CudaInvoke.cudaDisparityBilateralFilterCreate(ndisp, radius, iters, ref _sharedPtr);
      }

      /// <summary>
      /// Apply the filter to the disparity image
      /// </summary>
      /// <param name="disparity">The input disparity map</param>
      /// <param name="image">The image</param>
      /// <param name="dst">The output disparity map, should have the same size as the input disparity map</param>
      /// <param name="stream">Use a Stream to call the function asynchronously (non-blocking) or null to call the function synchronously (blocking).</param>
      public void Apply(IInputArray disparity, IInputArray image, IOutputArray dst, Stream stream = null)
      {
         using (InputArray iaDisparity = disparity.GetInputArray())
         using (InputArray iaImage = image.GetInputArray())
         using (OutputArray oaDst = dst.GetOutputArray())
            CudaInvoke.cudaDisparityBilateralFilterApply(this, iaDisparity, iaImage, oaDst, stream);
      }

      /// <summary>
      /// Release the unmanaged resources associated with the filter.
      /// </summary>
      protected override void DisposeObject()
      {
          if (IntPtr.Zero != _sharedPtr)
          {
              CudaInvoke.cudaDisparityBilateralFilterRelease(ref _sharedPtr);
              _ptr = IntPtr.Zero;
          }
      }
   }

   public static partial class CudaInvoke
   {
      [DllImport(CvInvoke.ExternCudaLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      internal extern static IntPtr cudaDisparityBilateralFilterCreate(int ndisp, int radius, int iters, ref IntPtr sharedPtr);

      [DllImport(CvInvoke.ExternCudaLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      internal extern static void cudaDisparityBilateralFilterApply(IntPtr filter, IntPtr disparity, IntPtr image, IntPtr dst, IntPtr stream);

      [DllImport(CvInvoke.ExternCudaLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      internal extern static void cudaDisparityBilateralFilterRelease(ref IntPtr filter);
   }
}
