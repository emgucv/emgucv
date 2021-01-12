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
   /// Box filter
   /// </summary>
   public class CudaBoxFilter : CudaFilter
   {
      /// <summary>
      /// Create a BoxMax filter.
      /// </summary>
      /// <param name="ksize">Size of the kernel</param>
      /// <param name="anchor">The center of the kernel. User (-1, -1) for the default kernel center.</param>
      /// <param name="borderType">The border type.</param>
      /// <param name="borderValue">The border value.</param>
      /// <param name="srcDepth">The source image depth type</param>
      /// <param name="srcChannels">The number of channels in the source image</param>
      /// <param name="dstDepth">The destination image depth type</param>
      /// <param name="dstChannels">The number of channels in the destination image</param>
      public CudaBoxFilter(DepthType srcDepth, int srcChannels, DepthType dstDepth, int dstChannels, Size ksize, Point anchor, CvEnum.BorderType borderType = BorderType.Default, MCvScalar borderValue = new MCvScalar())
      {
         _ptr = CudaInvoke.cudaCreateBoxFilter(CvInvoke.MakeType(srcDepth, srcChannels), CvInvoke.MakeType(dstDepth, dstChannels), ref ksize, ref anchor, borderType, ref borderValue, ref _sharedPtr);
      }
   }

   public static partial class CudaInvoke
   {
      [DllImport(CvInvoke.ExternCudaLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      internal static extern IntPtr cudaCreateBoxFilter(int srcType, int dstType, ref Size ksize, ref Point anchor, CvEnum.BorderType borderMode, ref MCvScalar borderValue, ref IntPtr sharedPtr);
   }
}
