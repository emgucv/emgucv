//----------------------------------------------------------------------------
//  Copyright (C) 2004-2013 by EMGU. All rights reserved.       
//----------------------------------------------------------------------------

﻿using System;
using System.Runtime.InteropServices;
using System.Drawing;

namespace Emgu.CV
{
   public static partial class CvInvoke
   {
      /// <summary>
      /// Reconstructs the selected image area from the pixel near the area boundary. The function may be used to remove dust and scratches from a scanned photo, or to remove undesirable objects from still images or video.
      /// </summary>
      /// <param name="src">The input 8-bit 1-channel or 3-channel image</param>
      /// <param name="mask">The inpainting mask, 8-bit 1-channel image. Non-zero pixels indicate the area that needs to be inpainted</param>
      /// <param name="dst">The output image of the same format and the same size as input</param>
      /// <param name="flags">The inpainting method</param>
      /// <param name="inpaintRadius">The radius of circlular neighborhood of each point inpainted that is considered by the algorithm</param>
      public static void Inpaint(IInputArray src, IInputArray mask, IOutputArray dst, double inpaintRadius, CvEnum.INPAINT_TYPE flags)
      {
         cveInpaint(src.InputArrayPtr, mask.InputArrayPtr, dst.OutputArrayPtr, inpaintRadius, flags);
      }
      [DllImport(EXTERN_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      private static extern void cveInpaint(IntPtr src, IntPtr mask, IntPtr dst, double inpaintRadius, CvEnum.INPAINT_TYPE flags);

      /// <summary>
      /// Perform image denoising using Non-local Means Denoising algorithm: 
      /// http://www.ipol.im/pub/algo/bcm_non_local_means_denoising/ 
      /// with several computational optimizations. Noise expected to be a gaussian white noise.
      /// </summary>
      /// <param name="src">Input 8-bit 1-channel, 2-channel or 3-channel image.</param>
      /// <param name="dst">Output image with the same size and type as src.</param>
      /// <param name="h">Parameter regulating filter strength. Big h value perfectly removes noise but also removes image details, smaller h value preserves details but also preserves some noise. Recommended value 3</param>
      /// <param name="templateWindowSize">Size in pixels of the template patch that is used to compute weights. Should be odd. Recommended value 7 pixels</param>
      /// <param name="searchWindowSize">Size in pixels of the window that is used to compute weighted average for given pixel. Should be odd. Affect performance linearly: greater searchWindowsSize - greater denoising time. Recommended value 21 pixels</param>
      public static void FastNlMeansDenoising(IInputArray src, IOutputArray dst, float h, int templateWindowSize, int searchWindowSize)
      {
         cveFastNlMeansDenoising(src.InputArrayPtr, dst.OutputArrayPtr, h, templateWindowSize, searchWindowSize);
      }
      [DllImport(EXTERN_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      private static extern void cveFastNlMeansDenoising(IntPtr src, IntPtr dst, float h, int templateWindowSize, int searchWindowSize);

      /// <summary>
      /// Perform image denoising using Non-local Means Denoising algorithm (modified for color image): 
      /// http://www.ipol.im/pub/algo/bcm_non_local_means_denoising/ 
      /// with several computational optimizations. Noise expected to be a gaussian white noise.
      /// The function converts image to CIELAB colorspace and then separately denoise L and AB components with given h parameters using fastNlMeansDenoising function.
      /// </summary>
      /// <param name="src">Input 8-bit 1-channel, 2-channel or 3-channel image.</param>
      /// <param name="dst">Output image with the same size and type as src.</param>
      /// <param name="h">Parameter regulating filter strength. Big h value perfectly removes noise but also removes image details, smaller h value preserves details but also preserves some noise. Recommended value 3</param>
      /// <param name="hColor">The same as h but for color components. For most images value equals 10 will be enought to remove colored noise and do not distort colors.</param>
      /// <param name="templateWindowSize">Size in pixels of the template patch that is used to compute weights. Should be odd. Recommended value 7 pixels</param>
      /// <param name="searchWindowSize">Size in pixels of the window that is used to compute weighted average for given pixel. Should be odd. Affect performance linearly: greater searchWindowsSize - greater denoising time. Recommended value 21 pixels</param>
      public static void FastNlMeansDenoisingColored(IInputArray src, IOutputArray dst, float h, float hColor, int templateWindowSize, int searchWindowSize)
      {
         cveFastNlMeansDenoisingColored(src.InputArrayPtr, dst.OutputArrayPtr, h, hColor, templateWindowSize, searchWindowSize);
      }
      [DllImport(EXTERN_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      private static extern void cveFastNlMeansDenoisingColored(IntPtr src, IntPtr dst, float h, float hColor, int templateWindowSize, int searchWindowSize);

   }
}
