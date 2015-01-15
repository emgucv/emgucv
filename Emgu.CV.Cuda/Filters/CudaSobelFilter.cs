//----------------------------------------------------------------------------
//  Copyright (C) 2004-2015 by EMGU Corporation. All rights reserved.       
//----------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;
using System.Drawing;
using Emgu.CV.CvEnum;
using Emgu.CV.Features2D;
using Emgu.CV.Structure;
using Emgu.CV.Util;
using Emgu.Util;

namespace Emgu.CV.Cuda
{
   /// <summary>
   /// Sobel filter
   /// </summary>
   public class CudaSobelFilter : CudaFilter
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
      public CudaSobelFilter(
         DepthType srcDepth, int srcChannels, 
         DepthType dstDepth, int dstChannels,
         int dx, int dy, int ksize = 3, double scale = 1.0, 
         CvEnum.BorderType rowBorderType = BorderType.Default, CvEnum.BorderType columnBorderType = BorderType.NegativeOne)
      {
         _ptr = CudaInvoke.cudaCreateSobelFilter(CvInvoke.MakeType(srcDepth, srcChannels), CvInvoke.MakeType(dstDepth, dstChannels), 
            dx, dy, ksize, scale, rowBorderType, columnBorderType);
      }
   }

   public static partial class CudaInvoke
   {
      [DllImport(CvInvoke.ExternCudaLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      internal static extern IntPtr cudaCreateSobelFilter(int srcType, int dstType, int dx, int dy, int ksize, double scale, CvEnum.BorderType rowBorderType, CvEnum.BorderType columnBorderType);
   }
}
