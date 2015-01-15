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
   /// Laplacian filter
   /// </summary>
   public class CudaLaplacianFilter: CudaFilter
   {
      /// <summary>
      /// Create a Laplacian filter.
      /// </summary>
      /// <param name="ksize">Either 1 or 3</param>
      /// <param name="scale">Optional scale. Use 1.0 for default</param>
      /// <param name="borderType">The border type.</param>
      /// <param name="borderValue">The border value.</param>
      public CudaLaplacianFilter(
         DepthType srcDepth, int srcChannels,
         DepthType dstDepth, int dstChannels,
         int ksize = 1, double scale = 1.0, 
         CvEnum.BorderType borderType = BorderType.Default, MCvScalar borderValue = new MCvScalar())
      {
         _ptr = CudaInvoke.cudaCreateLaplacianFilter(
            CvInvoke.MakeType(srcDepth, srcChannels), CvInvoke.MakeType(dstDepth, dstChannels), 
            ksize, scale, borderType, ref borderValue);
      }
   }

   public static partial class CudaInvoke
   {
      [DllImport(CvInvoke.ExternCudaLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      internal static extern IntPtr cudaCreateLaplacianFilter(int srcType, int dstType, int ksize, double scale, CvEnum.BorderType borderMode, ref MCvScalar borderValue);
   }
}
