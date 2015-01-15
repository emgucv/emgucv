//----------------------------------------------------------------------------
//  Copyright (C) 2004-2015 by EMGU Corporation. All rights reserved.       
//----------------------------------------------------------------------------

﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Text;
﻿using Emgu.CV.CvEnum;
﻿using Emgu.CV.Structure;
using Emgu.CV.Util;
using Emgu.Util;

namespace Emgu.CV.Cuda
{
   /// <summary>
   /// Cuda template matching filter.
   /// </summary>
   public class CudaTemplateMatching : UnmanagedObject
   {
      /// <summary>
      /// Create a Cuda template matching filter
      /// </summary>
      /// <param name="method">Specifies the way the template must be compared with image regions </param>
      /// <param name="blockSize">The block size</param>
      public CudaTemplateMatching(DepthType depthType, int channels, CvEnum.TemplateMatchingType method, Size blockSize = new Size())
      {
         _ptr = CudaInvoke.cudaTemplateMatchingCreate(CvInvoke.MakeType(depthType, channels), method, ref blockSize);
      }

      /// <summary>
      ///  This function is similiar to cvCalcBackProjectPatch. It slids through image, compares overlapped patches of size wxh with templ using the specified method and stores the comparison results to result
      /// </summary>
      /// <param name="image">Image where the search is running. It should be 8-bit or 32-bit floating-point</param>
      /// <param name="templ">Searched template; must be not greater than the source image and the same data type as the image</param>
      /// <param name="result">A map of comparison results; single-channel 32-bit floating-point. If image is WxH and templ is wxh then result must be W-w+1xH-h+1.</param>
      /// <param name="stream">Use a Stream to call the function asynchronously (non-blocking) or null to call the function synchronously (blocking).</param>  
      public void Match(IInputArray image, IInputArray templ, IOutputArray result, Stream stream = null)
      {
         using (InputArray iaImage = image.GetInputArray())
         using (InputArray iaTempl = templ.GetInputArray())
         using (OutputArray oaResult = result.GetOutputArray())
            CudaInvoke.cudaTemplateMatchingMatch(_ptr, iaImage, iaTempl, oaResult, stream);
      }

      /// <summary>
      /// Release the buffer
      /// </summary>
      protected override void DisposeObject()
      {
         if (_ptr != IntPtr.Zero)
            CudaInvoke.cudaTemplateMatchingRelease(ref _ptr);
      }
   }

   public static partial class CudaInvoke
   {
      [DllImport(CvInvoke.ExternCudaLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      internal static extern IntPtr cudaTemplateMatchingCreate(int srcType, CvEnum.TemplateMatchingType method, ref Size blockSize);

      [DllImport(CvInvoke.ExternCudaLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      internal static extern void cudaTemplateMatchingRelease(ref IntPtr buf);

      /// <summary>
      /// This function is similiar to cvCalcBackProjectPatch. It slids through image, compares overlapped patches of size wxh with templ using the specified method and stores the comparison results to result
      /// </summary>
      /// <param name="image">Image where the search is running. It should be 8-bit or 32-bit floating-point</param>
      /// <param name="templ">Searched template; must be not greater than the source image and the same data type as the image</param>
      /// <param name="result">A map of comparison results; single-channel 32-bit floating-point. If image is WxH and templ is wxh then result must be W-w+1xH-h+1.</param>
      /// <param name="tm">Pointer to cv::gpu::TemplateMatching</param>
      /// <param name="stream">Use a Stream to call the function asynchronously (non-blocking) or IntPtr.Zero to call the function synchronously (blocking).</param>  
      [DllImport(CvInvoke.ExternCudaLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      internal static extern void cudaTemplateMatchingMatch(IntPtr tm, IntPtr image, IntPtr templ, IntPtr result, IntPtr stream);
   }
}
