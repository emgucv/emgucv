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
   /// BoxMin filter
   /// </summary>
   public class CudaBoxMinFilter : CudaFilter
   {
      /// <summary>
      /// Create a BoxMin filter.
      /// </summary>
      /// <param name="ksize">Size of the kernel</param>
      /// <param name="anchor">The center of the kernel. User (-1, -1) for the default kernel center.</param>
      /// <param name="borderType">The border type.</param>
      /// <param name="borderValue">The border value.</param>
      public CudaBoxMinFilter(DepthType srcDepth, int srcChannels, Size ksize, Point anchor, CvEnum.BorderType borderType = BorderType.Default, MCvScalar borderValue = new MCvScalar())
      {
         _ptr = CudaInvoke.cudaCreateBoxMinFilter(CvInvoke.MakeType(srcDepth, srcChannels), ref ksize, ref anchor, borderType, ref borderValue);
      }
   }

   public static partial class CudaInvoke
   {
      [DllImport(CvInvoke.ExternCudaLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      internal static extern IntPtr cudaCreateBoxMinFilter(int srcType, ref Size ksize, ref Point anchor, CvEnum.BorderType borderMode, ref MCvScalar borderValue);
   }
}
