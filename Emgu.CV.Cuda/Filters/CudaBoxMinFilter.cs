//----------------------------------------------------------------------------
//  Copyright (C) 2004-2014 by EMGU Corporation. All rights reserved.       
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
   /// BoxMin filter
   /// </summary>
   public class CudaBoxMinFilter<TColor> : CudaFilter<TColor, Byte>
      where TColor : struct, IColor
   {
      /// <summary>
      /// Create a BoxMin filter.
      /// </summary>
      /// <param name="ksize">Size of the kernel</param>
      /// <param name="anchor">The center of the kernel. User (-1, -1) for the default kernel center.</param>
      /// <param name="borderType">The border type.</param>
      /// <param name="borderValue">The border value.</param>
      public CudaBoxMinFilter(Size ksize, Point anchor, CvEnum.BorderType borderType, MCvScalar borderValue)
      {
         _ptr = CudaInvoke.cudaCreateBoxMinFilter(_matType, ref ksize, ref anchor, (int)borderType, ref borderValue);
      }
   }

   public static partial class CudaInvoke
   {
      [DllImport(CvInvoke.ExternCudaLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      internal static extern IntPtr cudaCreateBoxMinFilter(int srcType, ref Size ksize, ref Point anchor, int borderMode, ref MCvScalar borderValue);
   }
}
