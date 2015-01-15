//----------------------------------------------------------------------------
//  Copyright (C) 2004-2015 by EMGU Corporation. All rights reserved.       
//----------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using Emgu.CV;
using Emgu.CV.Structure;
using Emgu.Util;

namespace Emgu.CV.Cuda
{
   /// <summary>
   /// Base CornernessCriteria class
   /// </summary>
   public abstract class CudaCornernessCriteria : UnmanagedObject
   {
      /// <summary>
      /// Release all the unmanaged memory associated with this gpu filter
      /// </summary>
      protected override void DisposeObject()
      {
         if (_ptr != IntPtr.Zero)
            CudaInvoke.cudaCornernessCriteriaRelease(ref _ptr);
      }

      /// <summary>
      /// Apply the cuda filter
      /// </summary>
      /// <param name="image">The source CudaImage where the filter will be applied to</param>
      /// <param name="dst">The destination CudaImage</param>
      /// <param name="stream">Use a Stream to call the function asynchronously (non-blocking) or null to call the function synchronously (blocking).</param>
      public void Apply(IInputArray image, IOutputArray dst, Stream stream = null)
      {
         using (InputArray iaImage = image.GetInputArray())
         using (OutputArray oaDst = dst.GetOutputArray())
            CudaInvoke.cudaCornernessCriteriaCompute(_ptr, iaImage, oaDst, stream);
      }
   }

   public static partial class CudaInvoke
   {
      [DllImport(CvInvoke.ExternCudaLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      internal static extern void cudaCornernessCriteriaCompute(IntPtr detector, IntPtr src, IntPtr dst, IntPtr stream);

      [DllImport(CvInvoke.ExternCudaLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      internal static extern void cudaCornernessCriteriaRelease(ref IntPtr detector);
   }

}
