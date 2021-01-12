//----------------------------------------------------------------------------
//  Copyright (C) 2004-2021 by EMGU Corporation. All rights reserved.       
//----------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using System.Drawing;
using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Structure;
using System.IO;
using Emgu.Util;

namespace Emgu.CV.Cuda
{ 
   /// <summary>
   /// Bayer Demosaicing (Malvar, He, and Cutler)
   /// </summary>
   public enum DemosaicTypes
   {
      /// <summary>
      /// BayerBG2BGR_MHT
      /// </summary>
      BayerBG2BGR_MHT = 256,
      /// <summary>
      /// BayerGB2BGR_MHT
      /// </summary>
      BayerGB2BGR_MHT = 257,
      /// <summary>
      /// BayerRG2BGR_MHT
      /// </summary>
      BayerRG2BGR_MHT = 258,
      /// <summary>
      /// BayerGR2BGR_MHT
      /// </summary>
      BayerGR2BGR_MHT = 259,

      /// <summary>
      /// BayerBG2RGB_MHT
      /// </summary>
      BayerBG2RGB_MHT = BayerRG2BGR_MHT,
      /// <summary>
      /// BayerGB2RGB_MHT
      /// </summary>
      BayerGB2RGB_MHT = BayerGR2BGR_MHT,
      /// <summary>
      /// BayerRG2RGB_MHT
      /// </summary>
      BayerRG2RGB_MHT = BayerBG2BGR_MHT,
      /// <summary>
      /// BayerGR2RGB_MHT
      /// </summary>
      BayerGR2RGB_MHT = BayerGB2BGR_MHT,

      /// <summary>
      /// BayerBG2GRAY_MHT
      /// </summary>
      BayerBG2GRAY_MHT = 260,
      /// <summary>
      /// BayerGB2GRAY_MHT
      /// </summary>
      BayerGB2GRAY_MHT = 261,
      /// <summary>
      /// BayerRG2GRAY_MHT
      /// </summary>
      BayerRG2GRAY_MHT = 262,
      /// <summary>
      /// BayerGR2GRAY_MHT
      /// </summary>
      BayerGR2GRAY_MHT = 263
   }

   /// <summary>
   /// Alpha composite types
   /// </summary>
   public enum AlphaCompTypes
   {
      /// <summary>
      /// Over
      /// </summary>
      Over,
      /// <summary>
      /// In
      /// </summary>
      In,
      /// <summary>
      /// Out
      /// </summary>
      Out,
      /// <summary>
      /// Atop
      /// </summary>
      Atop,
      /// <summary>
      /// Xor
      /// </summary>
      Xor,
      /// <summary>
      /// Plus
      /// </summary>
      Plus,
      /// <summary>
      /// Over Premul
      /// </summary>
      OverPremul,
      /// <summary>
      /// In Premul
      /// </summary>
      InPremul,
      /// <summary>
      /// Out Premul
      /// </summary>
      OutPremul,
      /// <summary>
      /// Atop Premul
      /// </summary>
      AtopPremul,
      /// <summary>
      /// Xor Premul
      /// </summary>
      XorPremul,
      /// <summary>
      /// Plus Premul
      /// </summary>
      PlusPremul,
      /// <summary>
      /// Premul
      /// </summary>
      Premul
   };

   public static partial class CudaInvoke
   {
      /// <summary>
      /// Converts image from one color space to another
      /// </summary>
      /// <param name="src">The source GpuMat</param>
      /// <param name="dst">The destination GpuMat</param>
      /// <param name="code">The color conversion code</param>
      /// <param name="dcn">Number of channels in the destination image. If the parameter is 0, the number of the channels is derived automatically from src and the code .</param>
      /// <param name="stream">Use a Stream to call the function asynchronously (non-blocking) or null to call the function synchronously (blocking).</param>
      public static void CvtColor(IInputArray src, IOutputArray dst, CvEnum.ColorConversion code, int dcn = 0, Stream stream = null)
      {
         using (InputArray iaSrc = src.GetInputArray())
         using (OutputArray oaDst = dst.GetOutputArray())
            cudaCvtColor(iaSrc, oaDst, code, dcn, stream);
      }
      [DllImport(CvInvoke.ExternCudaLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      private static extern void cudaCvtColor(IntPtr src, IntPtr dst, CvEnum.ColorConversion code, int dcn, IntPtr stream);

      /// <summary>
      /// Converts an image from Bayer pattern to RGB or grayscale.
      /// </summary>
      /// <param name="src">Source image (8-bit or 16-bit single channel).</param>
      /// <param name="dst">Destination image.</param>
      /// <param name="code">Color space conversion code (see the description below).</param>
      /// <param name="dcn">Number of channels in the destination image. If the parameter is 0, the number of the channels is derived automatically from src and the code .</param>
      /// <param name="stream">Stream for the asynchronous version.</param>
      public static void Demosaicing(IInputArray src, IOutputArray dst, DemosaicTypes code, int dcn = -1, Stream stream = null)
      {
         using (InputArray iaSrc = src.GetInputArray())
         using (OutputArray oaDst = dst.GetOutputArray())
            cudaDemosaicing(iaSrc, oaDst, code, dcn, stream);
      }
      [DllImport(CvInvoke.ExternCudaLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      private static extern void cudaDemosaicing(IntPtr src, IntPtr dst, DemosaicTypes code, int dcn, IntPtr stream);

      /// <summary>
      /// Swap channels.
      /// </summary>
      /// <param name="src">The image where the channels will be swapped</param>
      /// <param name="dstOrder">
      /// Integer array describing how channel values are permutated. The n-th entry
      /// of the array contains the number of the channel that is stored in the n-th channel of
      /// the output image. E.g. Given an RGBA image, aDstOrder = [3,2,1,0] converts this to ABGR
      /// channel order.
      /// </param>
      /// <param name="stream">Use a Stream to call the function asynchronously (non-blocking) or null to call the function synchronously (blocking).</param>
      public static void SwapChannels(IInputOutputArray src, int[] dstOrder, Stream stream)
      {
         if (dstOrder == null || dstOrder.Length < 4)
            throw new ArgumentException("dstOrder must be an int array of size 4");
         GCHandle handle = GCHandle.Alloc(dstOrder, GCHandleType.Pinned);
         using (InputOutputArray ioaSrc = src.GetInputOutputArray())
            cudaSwapChannels(ioaSrc, handle.AddrOfPinnedObject(), stream);

         handle.Free();
      }
      [DllImport(CvInvoke.ExternCudaLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      private static extern void cudaSwapChannels(IntPtr src, IntPtr dstOrder, IntPtr stream);

      /// <summary>
      /// Routines for correcting image color gamma
      /// </summary>
      /// <param name="src">Source image (3- or 4-channel 8 bit).</param>
      /// <param name="dst">Destination image.</param>
      /// <param name="forward">True for forward gamma correction or false for inverse gamma correction.</param>
      /// <param name="stream">Stream for the asynchronous version.</param>
      public static void GammaCorrection(IInputArray src, IOutputArray dst, bool forward = true, Stream stream = null)
      {
         using (InputArray iaSrc = src.GetInputArray())
         using (OutputArray oaDst = dst.GetOutputArray())
            cudaGammaCorrection(iaSrc, oaDst, forward, stream);
      }
      [DllImport(CvInvoke.ExternCudaLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      private static extern void cudaGammaCorrection(
         IntPtr src,
         IntPtr dst,
         [MarshalAs(CvInvoke.BoolMarshalType)]
         bool forward,
         IntPtr stream);

      /// <summary>
      /// Composites two images using alpha opacity values contained in each image.
      /// </summary>
      /// <param name="img1">First image. Supports CV_8UC4 , CV_16UC4 , CV_32SC4 and CV_32FC4 types.</param>
      /// <param name="img2">Second image. Must have the same size and the same type as img1 .</param>
      /// <param name="dst">Destination image</param>
      /// <param name="alphaOp">Flag specifying the alpha-blending operation</param>
      /// <param name="stream">Stream for the asynchronous version</param>
      public static void AlphaComp(IInputArray img1, IInputArray img2, IOutputArray dst, AlphaCompTypes alphaOp,
         Stream stream = null)
      {
         using (InputArray iaImg1 = img1.GetInputArray())
         using (InputArray iaImg2 = img2.GetInputArray())
            using (OutputArray oaDst = dst.GetOutputArray())
            {
               cudaAlphaComp(iaImg1, iaImg2, oaDst, alphaOp, stream);
            }
      }
      [DllImport(CvInvoke.ExternCudaLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      private static extern void cudaAlphaComp(IntPtr img1, IntPtr img2, IntPtr dst, AlphaCompTypes alphaOp, IntPtr stream);

      /// <summary>
      /// Calculates histogram for one channel 8-bit image.
      /// </summary>
      /// <param name="src">Source image with CV_8UC1 type.</param>
      /// <param name="hist">Destination histogram with one row, 256 columns, and the CV_32SC1 type.</param>
      /// <param name="stream">tream for the asynchronous version.</param>
      public static void CalcHist(IInputArray src, IOutputArray hist, Stream stream = null)
      {
         using (InputArray iaSrc = src.GetInputArray())
         using (OutputArray oaHist = hist.GetOutputArray())
         {
            cudaCalcHist(iaSrc, oaHist, stream);
         }
      }
      [DllImport(CvInvoke.ExternCudaLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      private static extern void cudaCalcHist(IntPtr src, IntPtr hist, IntPtr stream);

      /// <summary>
      /// Equalizes the histogram of a grayscale image.
      /// </summary>
      /// <param name="src">Source image with CV_8UC1 type.</param>
      /// <param name="dst">Destination image.</param>
      /// <param name="stream">Stream for the asynchronous version.</param>
      public static void EqualizeHist(IInputArray src, IOutputArray dst, Stream stream = null)
      {
         using (InputArray iaSrc = src.GetInputArray())
         using (OutputArray oaDst = dst.GetOutputArray())
            cudaEqualizeHist(iaSrc, oaDst, stream);
      }
      [DllImport(CvInvoke.ExternCudaLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      private static extern void cudaEqualizeHist(IntPtr src, IntPtr dst, IntPtr stream);

      /// <summary>
      /// Calculates histogram with evenly distributed bins for single channel source.
      /// </summary>
      /// <param name="src">The source GpuMat. Supports CV_8UC1, CV_16UC1 and CV_16SC1 types.</param>
      /// <param name="hist">Histogram with evenly distributed bins. A GpuMat&lt;int&gt; type.</param>
      /// <param name="histSize">The size of histogram (number of levels)</param>                                                                                                                                                                                                                                                             
      /// <param name="lowerLevel">The lower level</param>
      /// <param name="upperLevel">The upper level</param>
      /// <param name="stream">Use a Stream to call the function asynchronously (non-blocking) or null to call the function synchronously (blocking).</param>  
      /// <returns>Histogram with evenly distributed bins</returns>
      public static void HistEven(IInputArray src, IOutputArray hist, int histSize, int lowerLevel, int upperLevel, Stream stream = null)
      {
         using (InputArray iaSrc = src.GetInputArray())
         using (OutputArray oaHist = hist.GetOutputArray())
            cudaHistEven(iaSrc, oaHist, histSize, lowerLevel, upperLevel, stream);
      }
      [DllImport(CvInvoke.ExternCudaLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      private static extern void cudaHistEven(IntPtr src, IntPtr hist, int histSize, int lowerLevel, int upperLevel, IntPtr stream);

      /// <summary>
      /// Calculates a histogram with bins determined by the levels array
      /// </summary>
      /// <param name="src">Source image. CV_8U , CV_16U , or CV_16S depth and 1 or 4 channels are supported. For a four-channel image, all channels are processed separately.</param>
      /// <param name="hist">Destination histogram with one row, (levels.cols-1) columns, and the CV_32SC1 type.</param>
      /// <param name="levels">Number of levels in the histogram.</param>
      /// <param name="stream">Stream for the asynchronous version.</param>
      public static void HistRange(IInputArray src, IOutputArray hist, IInputArray levels, Stream stream = null)
      {
         using (InputArray iaSrc = src.GetInputArray())
         using (OutputArray oaHist = hist.GetOutputArray())
         using (InputArray iaLevels = levels.GetInputArray())
            cudaHistRange(iaSrc, oaHist, iaLevels, stream);
      }
      [DllImport(CvInvoke.ExternCudaLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      private static extern void cudaHistRange(IntPtr src, IntPtr hist, IntPtr levels, IntPtr stream);

      /// <summary>
      /// Performs linear blending of two images.
      /// </summary>
      /// <param name="img1">First image. Supports only CV_8U and CV_32F depth.</param>
      /// <param name="img2">Second image. Must have the same size and the same type as img1 .</param>
      /// <param name="weights1">Weights for first image. Must have tha same size as img1. Supports only CV_32F type.</param>
      /// <param name="weights2">Weights for second image. Must have tha same size as img2. Supports only CV_32F type.</param>
      /// <param name="result">Destination image.</param>
      /// <param name="stream">Use a Stream to call the function asynchronously (non-blocking) or null to call the function synchronously (blocking).</param>  
      public static void BlendLinear(IInputArray img1, IInputArray img2, IInputArray weights1, IInputArray weights2, IOutputArray result,
         Stream stream = null)
      {
         using (InputArray iaImg1 = img1.GetInputArray())
         using (InputArray iaImg2 = img2.GetInputArray())
         using (InputArray iaWeights1 = weights1.GetInputArray())
         using (InputArray iaWeights2 = weights2.GetInputArray())
         using (OutputArray oaResult = result.GetOutputArray())
            cudaBlendLinear(iaImg1, iaImg2, iaWeights1, iaWeights2, oaResult, stream);
      }
      [DllImport(CvInvoke.ExternCudaLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      private static extern void cudaBlendLinear(IntPtr img1, IntPtr img2, IntPtr weights1, IntPtr weights2, IntPtr result, IntPtr stream);

      /// <summary>
      /// Applies bilateral filter to the image.
      /// </summary>
      /// <param name="src">The source image</param>
      /// <param name="dst">The destination image; should have the same size and the same type as src</param>
      /// <param name="kernelSize">The diameter of each pixel neighborhood, that is used during filtering.</param>
      /// <param name="sigmaColor">Filter sigma in the color space. Larger value of the parameter means that farther colors within the pixel neighborhood (see sigmaSpace) will be mixed together, resulting in larger areas of semi-equal color</param>
      /// <param name="sigmaSpatial">Filter sigma in the coordinate space. Larger value of the parameter means that farther pixels will influence each other (as long as their colors are close enough; see sigmaColor). Then d&gt;0, it specifies the neighborhood size regardless of sigmaSpace, otherwise d is proportional to sigmaSpace.</param>
      /// <param name="borderType">Pixel extrapolation method, use DEFAULT for default</param>
      /// <param name="stream">Use a Stream to call the function asynchronously (non-blocking) or null to call the function synchronously (blocking).</param>  
      public static void BilateralFilter(IInputArray src, IOutputArray dst, int kernelSize, float sigmaColor, float sigmaSpatial, CvEnum.BorderType borderType = BorderType.Default, Stream stream = null)
      {
         using (InputArray iaSrc = src.GetInputArray())
         using (OutputArray oaDst = dst.GetOutputArray())
            cudaBilateralFilter(iaSrc, oaDst, kernelSize, sigmaColor, sigmaSpatial, borderType, stream);
      }
      [DllImport(CvInvoke.ExternCudaLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      private static extern void cudaBilateralFilter(IntPtr src, IntPtr dst, int kernelSize, float sigmaColor, float sigmaSpatial, CvEnum.BorderType borderType, IntPtr stream);

      /// <summary>
      /// Performs mean-shift filtering for each point of the source image. It maps each point of the source
      /// image into another point, and as the result we have new color and new position of each point.
      /// </summary>
      /// <param name="src">Source CudaImage. Only CV 8UC4 images are supported for now.</param>
      /// <param name="dst">Destination CudaImage, containing color of mapped points. Will have the same size and type as src.</param>
      /// <param name="sp">Spatial window radius.</param>
      /// <param name="sr">Color window radius.</param>
      /// <param name="criteria">Termination criteria.</param>
      /// <param name="stream">Use a Stream to call the function asynchronously (non-blocking) or null to call the function synchronously (blocking).</param>  
      public static void MeanShiftFiltering(IInputArray src, IOutputArray dst, int sp, int sr, MCvTermCriteria criteria,
         Stream stream = null)
      {
         using (InputArray iaSrc = src.GetInputArray())
         using (OutputArray oaDst = dst.GetOutputArray())
            cudaMeanShiftFiltering(iaSrc, oaDst, sp, sr, ref criteria, stream);
      }
      [DllImport(CvInvoke.ExternCudaLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      private static extern void cudaMeanShiftFiltering(IntPtr src, IntPtr dst, int sp, int sr, ref MCvTermCriteria criteria, IntPtr stream);

      /// <summary>
      /// Performs mean-shift procedure and stores information about processed points (i.e. their colors
      /// and positions) into two images.
      /// </summary>
      /// <param name="src">Source CudaImage. Only CV 8UC4 images are supported for now.</param>
      /// <param name="dstr">Destination CudaImage, containing color of mapped points. Will have the same size and type as src.</param>
      /// <param name="dstsp">Destination CudaImage, containing position of mapped points. Will have the same size as src and CV 16SC2 type.</param>
      /// <param name="sp">Spatial window radius.</param>
      /// <param name="sr">Color window radius.</param>
      /// <param name="criteria">Termination criteria.</param>
      /// <param name="stream">Use a Stream to call the function asynchronously (non-blocking) or null to call the function synchronously (blocking).</param>  
      public static void MeanShiftProc(IInputArray src, IOutputArray dstr, IOutputArray dstsp, int sp, int sr,
         MCvTermCriteria criteria, Stream stream = null)
      {
         using (InputArray iaSrc = src.GetInputArray())
         using (OutputArray oaDstr = dstr.GetOutputArray())
         using (OutputArray oaDstsp = dstsp.GetOutputArray())
            cudaMeanShiftProc(iaSrc, oaDstr, oaDstsp, sp, sr, ref criteria, stream);
      }
      [DllImport(CvInvoke.ExternCudaLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      private static extern void cudaMeanShiftProc(IntPtr src, IntPtr dstr, IntPtr dstsp, int sp, int sr, ref MCvTermCriteria criteria, IntPtr stream);

      /// <summary>
      /// Performs mean-shift segmentation of the source image and eleminates small segments.
      /// </summary>
      /// <param name="src">Source CudaImage. Only CV 8UC4 images are supported for now.</param>
      /// <param name="dst">Segmented Image. Will have the same size and type as src. Note that this is an Image type and not CudaImage type</param>
      /// <param name="sp">Spatial window radius.</param>
      /// <param name="sr">Color window radius.</param>
      /// <param name="minSize">Minimum segment size. Smaller segements will be merged.</param>
      /// <param name="criteria">Termination criteria.</param>
      /// <param name="stream">Use a Stream to call the function asynchronously (non-blocking) or null to call the function synchronously (blocking).</param>  
      public static void MeanShiftSegmentation(
         IInputArray src, IOutputArray dst, int sp, int sr, int minSize,
         MCvTermCriteria criteria, Stream stream)
      {
         using (InputArray iaSrc = src.GetInputArray())
         using (OutputArray oaDst = dst.GetOutputArray())
            cudaMeanShiftSegmentation(iaSrc, oaDst, sp, sr, minSize, ref criteria, stream);
      }
      [DllImport(CvInvoke.ExternCudaLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      private static extern void cudaMeanShiftSegmentation(IntPtr src, IntPtr dst, int sp, int sr, int minsize, ref MCvTermCriteria criteria, IntPtr stream);

   }
}
