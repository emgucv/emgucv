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
   /// Gaussian filter
   /// </summary>
   public class CudaGaussianFilter<TColor, TDepth> : CudaFilter<TColor, TDepth>
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
      public CudaGaussianFilter(Size ksize, double sigma1, double sigma2, CvEnum.BORDER_TYPE rowBorderType, CvEnum.BORDER_TYPE columnBorderType)
      {
         _ptr = CudaInvoke.cudaCreateGaussianFilter(_matType, _matType, ref ksize, sigma1, sigma2, (int)rowBorderType, (int)columnBorderType);
      }
   }

   public static partial class CudaInvoke
   {
      [DllImport(CvInvoke.EXTERN_CUDA_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      internal static extern IntPtr cudaCreateGaussianFilter(int srcType, int dstType, ref Size ksize, double sigma1, double sigma2, int rowBorderType, int columnBorderType);
   }
}
