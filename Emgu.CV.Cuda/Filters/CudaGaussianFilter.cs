//----------------------------------------------------------------------------
//  Copyright (C) 2004-2021 by EMGU Corporation. All rights reserved.       
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
   /// Gaussian filter
   /// </summary>
   public class CudaGaussianFilter : CudaFilter
   {
      /// <summary>
      /// Create a Gaussian filter.
      /// </summary>
      /// <param name="ksize">The size of the kernel</param>
      /// <param name="sigma1">This parameter may specify Gaussian sigma (standard deviation). If it is zero, it is calculated from the kernel size.</param>
      /// <param name="sigma2">In case of non-square Gaussian kernel the parameter may be used to specify a different (from param3) sigma in the vertical direction. Use 0 for default</param>
      /// <param name="rowBorderType">The row border type.</param>
      /// <param name="columnBorderType">The column border type.</param>
      /// <param name="srcDepth">The depth type of the source image</param>
      /// <param name="srcChannels">The number of channels in the source image</param>
      /// <param name="dstDepth">The depth type of the destination image</param>
      /// <param name="dstChannels">The number of channels in the destination image</param>
      public CudaGaussianFilter(
         DepthType srcDepth, int srcChannels,
         DepthType dstDepth, int dstChannels,
         Size ksize, 
         double sigma1, double sigma2 = 0, 
         CvEnum.BorderType rowBorderType = BorderType.Default, CvEnum.BorderType columnBorderType = BorderType.NegativeOne)
      {
         _ptr = CudaInvoke.cudaCreateGaussianFilter(
            CvInvoke.MakeType(srcDepth, srcChannels), CvInvoke.MakeType(dstDepth, dstChannels), 
            ref ksize, sigma1, sigma2, (int)rowBorderType, (int)columnBorderType, ref _sharedPtr);
      }
   }

   public static partial class CudaInvoke
   {
      [DllImport(CvInvoke.ExternCudaLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      internal static extern IntPtr cudaCreateGaussianFilter(int srcType, int dstType, ref Size ksize, double sigma1, double sigma2, int rowBorderType, int columnBorderType, ref IntPtr sharedPtr);
   }
}
