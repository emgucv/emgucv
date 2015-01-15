//----------------------------------------------------------------------------
//  Copyright (C) 2004-2015 by EMGU Corporation. All rights reserved.       
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
      /// <param name="inpaintRadius">The radius of circular neighborhood of each point inpainted that is considered by the algorithm</param>
      public static void Inpaint(IInputArray src, IInputArray mask, IOutputArray dst, double inpaintRadius, CvEnum.InpaintType flags)
      {
         using (InputArray iaSrc = src.GetInputArray())
         using (InputArray iaMask = mask.GetInputArray())
         using (OutputArray oaDst = dst.GetOutputArray())
            cveInpaint(iaSrc, iaMask, oaDst, inpaintRadius, flags);
      }
      [DllImport(ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      private static extern void cveInpaint(IntPtr src, IntPtr mask, IntPtr dst, double inpaintRadius, CvEnum.InpaintType flags);

      /// <summary>
      /// Perform image denoising using Non-local Means Denoising algorithm: 
      /// http://www.ipol.im/pub/algo/bcm_non_local_means_denoising/ 
      /// with several computational optimizations. Noise expected to be a gaussian white noise.
      /// </summary>
      /// <param name="src">Input 8-bit 1-channel, 2-channel or 3-channel image.</param>
      /// <param name="dst">Output image with the same size and type as src.</param>
      /// <param name="h">Parameter regulating filter strength. Big h value perfectly removes noise but also removes image details, smaller h value preserves details but also preserves some noise.</param>
      /// <param name="templateWindowSize">Size in pixels of the template patch that is used to compute weights. Should be odd.</param>
      /// <param name="searchWindowSize">Size in pixels of the window that is used to compute weighted average for given pixel. Should be odd. Affect performance linearly: greater searchWindowsSize - greater denoising time.</param>
      public static void FastNlMeansDenoising(IInputArray src, IOutputArray dst, float h = 3, int templateWindowSize = 7, int searchWindowSize = 21)
      {
         using (InputArray iaSrc = src.GetInputArray())
         using (OutputArray oaDst = dst.GetOutputArray())
            cveFastNlMeansDenoising(iaSrc, oaDst, h, templateWindowSize, searchWindowSize);
      }
      [DllImport(ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      private static extern void cveFastNlMeansDenoising(IntPtr src, IntPtr dst, float h, int templateWindowSize, int searchWindowSize);

      /// <summary>
      /// Perform image denoising using Non-local Means Denoising algorithm (modified for color image): 
      /// http://www.ipol.im/pub/algo/bcm_non_local_means_denoising/ 
      /// with several computational optimizations. Noise expected to be a gaussian white noise.
      /// The function converts image to CIELAB colorspace and then separately denoise L and AB components with given h parameters using fastNlMeansDenoising function.
      /// </summary>
      /// <param name="src">Input 8-bit 1-channel, 2-channel or 3-channel image.</param>
      /// <param name="dst">Output image with the same size and type as src.</param>
      /// <param name="h">Parameter regulating filter strength. Big h value perfectly removes noise but also removes image details, smaller h value preserves details but also preserves some noise.</param>
      /// <param name="hColor">The same as h but for color components. For most images value equals 10 will be enought to remove colored noise and do not distort colors.</param>
      /// <param name="templateWindowSize">Size in pixels of the template patch that is used to compute weights. Should be odd.</param>
      /// <param name="searchWindowSize">Size in pixels of the window that is used to compute weighted average for given pixel. Should be odd. Affect performance linearly: greater searchWindowsSize - greater denoising time.</param>
      public static void FastNlMeansDenoisingColored(IInputArray src, IOutputArray dst, float h = 3, float hColor = 3, int templateWindowSize = 7, int searchWindowSize = 21)
      {
         using (InputArray iaSrc = src.GetInputArray())
         using (OutputArray oaDst = dst.GetOutputArray())
            cveFastNlMeansDenoisingColored(iaSrc, oaDst, h, hColor, templateWindowSize, searchWindowSize);
      }
      [DllImport(ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      private static extern void cveFastNlMeansDenoisingColored(IntPtr src, IntPtr dst, float h, float hColor, int templateWindowSize, int searchWindowSize);

      /// <summary>
      /// Filtering is the fundamental operation in image and video processing. Edge-preserving smoothing filters are used in many different applications.
      /// </summary>
      /// <param name="src">Input 8-bit 3-channel image</param>
      /// <param name="dst">Output 8-bit 3-channel image</param>
      /// <param name="flags">Edge preserving filters</param>
      /// <param name="sigmaS">Range between 0 to 200</param>
      /// <param name="sigmaR">Range between 0 to 1</param>
      public static void EdgePreservingFilter(
         IInputArray src, IOutputArray dst,
         CvEnum.EdgePreservingFilterFlag flags = CvEnum.EdgePreservingFilterFlag.RecursFilter,
         float sigmaS = 60.0f,
         float sigmaR = 0.4f)
      {
         using (InputArray iaSrc = src.GetInputArray())
         using (OutputArray oaDst = dst.GetOutputArray())
            cveEdgePreservingFilter(iaSrc, oaDst, flags, sigmaS, sigmaR);
      }
      [DllImport(ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      private static extern void cveEdgePreservingFilter(IntPtr src, IntPtr dst, CvEnum.EdgePreservingFilterFlag flags, float sigmaS, float sigmaR);

      /// <summary>
      /// This filter enhances the details of a particular image.
      /// </summary>
      /// <param name="src">Input 8-bit 3-channel image</param>
      /// <param name="dst">Output image with the same size and type as src</param>
      /// <param name="sigmaS">Range between 0 to 200</param>
      /// <param name="sigmaR">Range between 0 to 1</param>
      public static void DetailEnhance(IInputArray src, IOutputArray dst, float sigmaS = 10.0f, float sigmaR = 0.15f)
      {
         using (InputArray iaSrc = src.GetInputArray())
         using (OutputArray oaDst = dst.GetOutputArray())
            cveDetailEnhance(iaSrc, oaDst, sigmaS, sigmaR);
      }
      [DllImport(ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      private static extern void cveDetailEnhance(IntPtr src, IntPtr dst, float sigmaS, float sigmaR);

      /// <summary>
      /// Pencil-like non-photorealistic line drawing
      /// </summary>
      /// <param name="src">Input 8-bit 3-channel image</param>
      /// <param name="dst1">Output 8-bit 1-channel image</param>
      /// <param name="dst2">Output image with the same size and type as src</param>
      /// <param name="sigmaS">Range between 0 to 200</param>
      /// <param name="sigmaR">Range between 0 to 1</param>
      /// <param name="shadeFactor">Range between 0 to 0.1</param>
      public static void PencilSketch(IInputArray src, IOutputArray dst1, IOutputArray dst2, float sigmaS = 60.0f, float sigmaR = 0.07f, float shadeFactor = 0.02f)
      {
         using (InputArray iaSrc = src.GetInputArray())
         using (OutputArray oaDst1 = dst1.GetOutputArray())
         using (OutputArray oaDst2 = dst2.GetOutputArray())
            cvePencilSketch(iaSrc, oaDst1, oaDst2, sigmaS, sigmaR, shadeFactor);
      }
      [DllImport(ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      private static extern void cvePencilSketch(IntPtr src, IntPtr dst1, IntPtr dst2, float sigmaS, float sigmaR, float shadeFactor);

      /// <summary>
      /// Stylization aims to produce digital imagery with a wide variety of effects not focused on photorealism. Edge-aware filters are ideal for stylization, as they can abstract regions of low contrast while preserving, or enhancing, high-contrast features.
      /// </summary>
      /// <param name="src">Input 8-bit 3-channel image.</param>
      /// <param name="dst">Output image with the same size and type as src.</param>
      /// <param name="sigmaS">Range between 0 to 200.</param>
      /// <param name="sigmaR"> Range between 0 to 1.</param>
      public static void Stylization(IInputArray src, IOutputArray dst, float sigmaS = 60, float sigmaR = 0.45f)
      {
         using (InputArray iaSrc = src.GetInputArray())
         using (OutputArray oaDst = dst.GetOutputArray())
            cveStylization(iaSrc, oaDst, sigmaS, sigmaR);
      }
      [DllImport(ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      private static extern void cveStylization(IntPtr src, IntPtr dst, float sigmaS, float sigmaR);

      /// <summary>
      /// Given an original color image, two differently colored versions of this image can be mixed seamlessly.
      /// </summary>
      /// <param name="src">Input 8-bit 3-channel image.</param>
      /// <param name="mask">Input 8-bit 1 or 3-channel image.</param>
      /// <param name="dst">Output image with the same size and type as src .</param>
      /// <param name="redMul">R-channel multiply factor. Multiplication factor is between .5 to 2.5.</param>
      /// <param name="greenMul">G-channel multiply factor. Multiplication factor is between .5 to 2.5.</param>
      /// <param name="blueMul">B-channel multiply factor. Multiplication factor is between .5 to 2.5.</param>
      public static void ColorChange(IInputArray src, IInputArray mask, IOutputArray dst, float redMul = 1.0f, float greenMul = 1.0f, float blueMul = 1.0f)
      {
         using (InputArray iaSrc = src.GetInputArray())
         using (OutputArray oaDst = dst.GetOutputArray())
         using (InputArray iaMask = mask == null ? InputArray.GetEmpty() : mask.GetInputArray())
            cveColorChange(iaSrc, iaMask, oaDst, redMul, greenMul, blueMul);
      }
      [DllImport(ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      private static extern void cveColorChange(IntPtr src, IntPtr mask, IntPtr dst, float redMul, float greenMul, float blueMul);

      /// <summary>
      /// Applying an appropriate non-linear transformation to the gradient field inside the selection and then integrating back with a Poisson solver, modifies locally the apparent illumination of an image.
      /// </summary>
      /// <param name="src">Input 8-bit 3-channel image.</param>
      /// <param name="mask">Input 8-bit 1 or 3-channel image.</param>
      /// <param name="dst">Output image with the same size and type as src.</param>
      /// <param name="alpha">Value ranges between 0-2.</param>
      /// <param name="beta">Value ranges between 0-2.</param>
      public static void IlluminationChange(IInputArray src, IInputArray mask, IOutputArray dst, float alpha = 0.2f, float beta = 0.4f)
      {
         using (InputArray iaSrc = src.GetInputArray())
         using (OutputArray oaDst = dst.GetOutputArray())
         using (InputArray iaMask = mask == null ? InputArray.GetEmpty() : mask.GetInputArray())
            cveIlluminationChange(iaSrc, iaMask, oaDst, alpha, beta);
      }
      [DllImport(ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      private static extern void cveIlluminationChange(IntPtr src, IntPtr mask, IntPtr dst, float alpha, float beta);

      /// <summary>
      /// By retaining only the gradients at edge locations, before integrating with the Poisson solver, one washes out the texture of the selected region, giving its contents a flat aspect. Here Canny Edge Detector is used.
      /// </summary>
      /// <param name="src">Input 8-bit 3-channel image.</param>
      /// <param name="mask">Input 8-bit 1 or 3-channel image.</param>
      /// <param name="dst">Output image with the same size and type as src.</param>
      /// <param name="lowThreshold">Range from 0 to 100.</param>
      /// <param name="highThreshold">Value &gt; 100</param>
      /// <param name="kernelSize">The size of the Sobel kernel to be used.</param>
      public static void TextureFlattening(IInputArray src, IInputArray mask, IOutputArray dst, float lowThreshold = 30, float highThreshold = 45, int kernelSize = 3)
      {
         using (InputArray iaSrc = src.GetInputArray())
         using (OutputArray oaDst = dst.GetOutputArray())
         using (InputArray iaMask = mask == null ? InputArray.GetEmpty() : mask.GetInputArray())
            cveTextureFlattening(iaSrc, iaMask, oaDst, lowThreshold, highThreshold, kernelSize);
      }
      [DllImport(ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      private static extern void cveTextureFlattening(IntPtr src, IntPtr mask, IntPtr dst, float lowThreshold, float highThreshold, int kernelSize);

   }
}
