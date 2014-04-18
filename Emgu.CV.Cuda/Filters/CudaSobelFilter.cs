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
   /// Sobel filter
   /// </summary>
   public class CudaSobelFilter<TColor, TDepth> : CudaFilter<TColor, TDepth>
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
      public CudaSobelFilter(int dx, int dy, int ksize, double scale, CvEnum.BorderType rowBorderType, CvEnum.BorderType columnBorderType)
      {
         _ptr = CudaInvoke.cudaCreateSobelFilter(_matType, _matType, dx, dy, ksize, scale, (int)rowBorderType, (int)columnBorderType);
      }
   }

   public static partial class CudaInvoke
   {
      [DllImport(CvInvoke.ExternCudaLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      internal static extern IntPtr cudaCreateSobelFilter(int srcType, int dstType, int dx, int dy, int ksize, double scale, int rowBorderType, int columnBorderType);
   }
}
