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
   /// Laplacian filter
   /// </summary>
   public class CudaLaplacianFilter<TColor, TDepth> : CudaFilter<TColor, TDepth>
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
      public CudaLaplacianFilter(int ksize, double scale, CvEnum.BORDER_TYPE borderType, MCvScalar borderValue)
      {
         _ptr = CudaInvoke.cudaCreateLaplacianFilter(_matType, _matType, ksize, scale, (int)borderType, ref borderValue);
      }
   }

   public static partial class CudaInvoke
   {
      [DllImport(CvInvoke.EXTERN_CUDA_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      internal static extern IntPtr cudaCreateLaplacianFilter(int srcType, int dstType, int ksize, double scale, int borderMode, ref MCvScalar borderValue);
   }
}
