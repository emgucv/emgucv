//----------------------------------------------------------------------------
//  Copyright (C) 2004-2013 by EMGU. All rights reserved.       
//----------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;
using System.Drawing;
using Emgu.CV.Features2D;
using Emgu.CV.Structure;
using Emgu.CV.Util;
using Emgu.Util;

namespace Emgu.CV.GPU
{
   public abstract class GpuFilter<TColor, TDepth> : UnmanagedObject
      where TColor : struct, IColor
      where TDepth : new()
   {
      protected static int _matType;
      static GpuFilter()
      {
         using (GpuImage<TColor, TDepth> tmp = new GpuImage<TColor, TDepth>(4, 4))
         {
            _matType = tmp.Type;
         }
      }

      protected override void DisposeObject()
      {
         if (_ptr != null)
            GpuInvoke.gpuFilterRelease(ref _ptr);
      }

      public void Apply(GpuImage<TColor, TDepth> image, GpuImage<TColor, TDepth> dst, Stream stream)
      {
         GpuInvoke.gpuFilterApply(_ptr, image, dst, stream);
      }
   }

   /// <summary>
   /// Applies arbitrary linear filter to the image. In-place operation is supported. When the aperture is partially outside the image, the function interpolates outlier pixel values from the nearest pixels that is inside the image
   /// </summary>
   public class GpuLinearFilter<TColor, TDepth> : GpuFilter<TColor, TDepth>
      where TColor : struct, IColor
      where TDepth : new()
   {
      /// <summary>
      /// Create a Gpu LinearFilter
      /// </summary>
      /// <param name="kernel">Convolution kernel, single-channel floating point matrix (e.g. Emgu.CV.Matrix). If you want to apply different kernels to different channels, split the gpu image into separate color planes and process them individually</param>
      /// <param name="anchor">The anchor of the kernel that indicates the relative position of a filtered point within the kernel. The anchor shoud lie within the kernel. The special default value (-1,-1) means that it is at the kernel center</param>
      /// <param name="borderType">Border type. Use REFLECT101 as default.</param>
      /// <param name="borderValue">The border value</param>
      public GpuLinearFilter(Matrix<float> kernel, System.Drawing.Point anchor, CvEnum.BORDER_TYPE borderType, MCvScalar borderValue)
      {
         _ptr = GpuInvoke.gpuCreateLinearFilter(_matType, _matType, kernel, ref anchor, borderType, ref borderValue);
      }
   }

   /// <summary>
   /// Laplacian filter
   /// </summary>
   public class GpuLaplacianFilter<TColor, TDepth> : GpuFilter<TColor, TDepth>
      where TColor : struct, IColor
      where TDepth : new()
   {
      /// <summary>
      /// Create a Laplacian filter.
      /// </summary>
      /// <param name="ksize">Either 1 or 3</param>
      /// <param name="scale">Optional scale. Use 1.0 for default</param>
      /// <param name="borderType">The border type.</param>
      /// <param name="borderValue">The border value.</param>
      public GpuLaplacianFilter(int ksize, double scale, CvEnum.BORDER_TYPE borderType, MCvScalar borderValue)
      {
         _ptr = GpuInvoke.gpuCreateLaplacianFilter(_matType, _matType, ksize, scale, (int) borderType, ref borderValue);
      }
   }

   /// <summary>
   /// Gaussian filter
   /// </summary>
   public class GpuGaussianFilter<TColor, TDepth> : GpuFilter<TColor, TDepth>
      where TColor : struct, IColor
      where TDepth : new()
   {
      /// <summary>
      /// Create a Gaussian filter.
      /// </summary>
      /// <param name="ksize">The size of the kernel</param>
      /// <param name="sigma1">This parameter may specify Gaussian sigma (standard deviation). If it is zero, it is calculated from the kernel size.</param>
      /// <param name="sigma2">In case of non-square Gaussian kernel the parameter may be used to specify a different (from param3) sigma in the vertical direction. Use 0 for default</param>
      /// <param name="rowBorderType">The row border type.</param>
      /// <param name="columnBorderType">The column border type.</param>
      public GpuGaussianFilter(Size ksize, double sigma1, double sigma2, CvEnum.BORDER_TYPE rowBorderType, CvEnum.BORDER_TYPE columnBorderType)
      {
         _ptr = GpuInvoke.gpuCreateGaussianFilter(_matType, _matType, ref ksize, sigma1, sigma2, (int) rowBorderType, (int) columnBorderType);
      }
   }


   /// <summary>
   /// Sobel filter
   /// </summary>
   public class GpuSobelFilter<TColor, TDepth> : GpuFilter<TColor, TDepth>
      where TColor : struct, IColor
      where TDepth : new()
   {
      /// <summary>
      /// Create a Sobel filter.
      /// </summary>
      /// <param name="dx">Order of the derivative x</param>
      /// <param name="dy">Order of the derivative y</param>
      /// <param name="ksize">Size of the extended Sobel kernel</param>
      /// <param name="scale">Optional scale, use 1 for default.</param>
      /// <param name="rowBorderType">The row border type.</param>
      /// <param name="columnBorderType">The column border type.</param>
      public GpuSobelFilter(int dx, int dy, int ksize, double scale, CvEnum.BORDER_TYPE rowBorderType, CvEnum.BORDER_TYPE columnBorderType)
      {
         _ptr = GpuInvoke.gpuCreateSobelFilter(_matType, _matType, dx, dy, ksize, scale, (int)rowBorderType, (int)columnBorderType);
      }
   }

   /*
         /// <summary>
      /// Erodes the image (applies the local minimum operator).
      /// Supports CV_8UC1, CV_8UC4 type.
      /// </summary>
      /// <param name="src">The source GpuMat</param>
      /// <param name="dst">The destination GpuMat</param>
      /// <param name="kernel">The morphology kernel, pointer to an CvArr. If it is IntPtr.Zero, a 3x3 rectangular structuring element is used.</param>
      /// <param name="buffer">Temperary buffer. Should be the same size and type as the <paramref name="src"/> GpuMat. </param>
      /// <param name="anchor">The center of the kernel. User (-1, -1) for the default kernel center.</param>
      /// <param name="iterations">The number of iterations morphology is applied</param>
      /// <param name="stream">Use a Stream to call the function asynchronously (non-blocking) or IntPtr.Zero to call the function synchronously (blocking).</param>
      [DllImport(CvInvoke.EXTERN_GPU_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention, EntryPoint = "gpuMatErode")]
      public static extern void Erode(IntPtr src, IntPtr dst, IntPtr kernel, IntPtr buffer, Point anchor, int iterations, IntPtr stream);

      /// <summary>
      /// Dilate the image (applies the local maximum operator).
      /// Supports CV_8UC1, CV_8UC4 type.
      /// </summary>
      /// <param name="src">The source GpuMat</param>
      /// <param name="dst">The destination GpuMat</param>
      /// <param name="kernel">The morphology kernel, pointer to an CvArr. If it is IntPtr.Zero, a 3x3 rectangular structuring element is used.</param>
      /// <param name="buffer">Temperary buffer. Should be the same size and type as the <paramref name="src"/> GpuMat. </param>
      /// <param name="anchor">The center of the kernel. User (-1, -1) for the default kernel center.</param>
      /// <param name="iterations">The number of iterations morphology is applied</param>
      /// <param name="stream">Use a Stream to call the function asynchronously (non-blocking) or IntPtr.Zero to call the function synchronously (blocking).</param>
      [DllImport(CvInvoke.EXTERN_GPU_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention, EntryPoint = "gpuMatDilate")]
      public static extern void Dilate(IntPtr src, IntPtr dst, IntPtr kernel, IntPtr buffer, Point anchor, int iterations, IntPtr stream);
   */

   /// <summary>
   /// BoxMax filter
   /// </summary>
   public class GpuBoxMaxFilter<TColor> : GpuFilter<TColor, Byte>
      where TColor : struct, IColor
   {
      /// <summary>
      /// Create a BoxMax filter.
      /// </summary>
      /// <param name="ksize">Size of the kernel</param>
      /// <param name="anchor">The center of the kernel. User (-1, -1) for the default kernel center.</param>
      /// <param name="borderType">The border type.</param>
      /// <param name="borderValue">The border value.</param>
      public GpuBoxMaxFilter(Size ksize, Point anchor, CvEnum.BORDER_TYPE borderType, MCvScalar borderValue)
      {
         _ptr = GpuInvoke.gpuCreateBoxMaxFilter(_matType, ref ksize, ref anchor, (int)borderType, ref borderValue);
      }
   }

   /// <summary>
   /// BoxMin filter
   /// </summary>
   public class GpuBoxMinFilter<TColor> : GpuFilter<TColor, Byte>
      where TColor : struct, IColor
   {
      /// <summary>
      /// Create a BoxMin filter.
      /// </summary>
      /// <param name="ksize">Size of the kernel</param>
      /// <param name="anchor">The center of the kernel. User (-1, -1) for the default kernel center.</param>
      /// <param name="borderType">The border type.</param>
      /// <param name="borderValue">The border value.</param>
      public GpuBoxMinFilter(Size ksize, Point anchor, CvEnum.BORDER_TYPE borderType, MCvScalar borderValue)
      {
         _ptr = GpuInvoke.gpuCreateBoxMinFilter(_matType, ref ksize, ref anchor, (int)borderType, ref borderValue);
      }
   }

   public static partial class GpuInvoke
   {
      [DllImport(CvInvoke.EXTERN_GPU_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      internal static extern void gpuFilterApply(IntPtr filter, IntPtr image, IntPtr dst, IntPtr stream);

      [DllImport(CvInvoke.EXTERN_GPU_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      internal static extern void gpuFilterRelease(ref IntPtr filter);

      [DllImport(CvInvoke.EXTERN_GPU_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      internal static extern IntPtr gpuCreateLinearFilter(int srcType, int dstType, IntPtr kernel, ref Point anchor, CvEnum.BORDER_TYPE borderMode, ref MCvScalar borderValue);

      [DllImport(CvInvoke.EXTERN_GPU_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      internal static extern IntPtr gpuCreateLaplacianFilter(int srcType, int dstType, int ksize, double scale, int borderMode, ref MCvScalar borderValue);

      [DllImport(CvInvoke.EXTERN_GPU_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      internal static extern IntPtr gpuCreateGaussianFilter(int srcType, int dstType, ref Size ksize, double sigma1, double sigma2, int rowBorderType, int columnBorderType);

      [DllImport(CvInvoke.EXTERN_GPU_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      internal static extern IntPtr gpuCreateSobelFilter(int srcType, int dstType, int dx, int dy, int ksize, double scale, int rowBorderType, int columnBorderType);

      [DllImport(CvInvoke.EXTERN_GPU_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      internal static extern IntPtr gpuCreateBoxMaxFilter(int srcType, ref Size ksize, ref Point anchor, int borderMode, ref MCvScalar borderValue);

      [DllImport(CvInvoke.EXTERN_GPU_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      internal static extern IntPtr gpuCreateBoxMinFilter(int srcType, ref Size ksize, ref Point anchor, int borderMode, ref MCvScalar borderValue);

   }

}
