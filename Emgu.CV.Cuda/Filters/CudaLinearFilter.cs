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

namespace Emgu.CV.Cuda
{
   /// <summary>
   /// Applies arbitrary linear filter to the image. In-place operation is supported. When the aperture is partially outside the image, the function interpolates outlier pixel values from the nearest pixels that is inside the image
   /// </summary>
   public class CudaLinearFilter<TColor, TDepth> : CudaFilter<TColor, TDepth>
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
      public CudaLinearFilter(Matrix<float> kernel, System.Drawing.Point anchor, CvEnum.BORDER_TYPE borderType, MCvScalar borderValue)
      {
         _ptr = CudaInvoke.cudaCreateLinearFilter(_matType, _matType, kernel, ref anchor, borderType, ref borderValue);
      }
   }

   public static partial class CudaInvoke
   {
      [DllImport(CvInvoke.EXTERN_CUDA_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      internal static extern IntPtr cudaCreateLinearFilter(int srcType, int dstType, IntPtr kernel, ref Point anchor, CvEnum.BORDER_TYPE borderMode, ref MCvScalar borderValue);
   }
}
