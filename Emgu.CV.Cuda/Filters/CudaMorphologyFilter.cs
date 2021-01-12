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
   /// Morphology filter
   /// </summary>
   public class CudaMorphologyFilter : CudaFilter
   {
      /// <summary>
      /// Create a Morphology filter.
      /// </summary>
      /// <param name="op">Type of morphological operation</param>
      /// <param name="kernel">2D 8-bit structuring element for the morphological operation.</param>
      /// <param name="anchor">Anchor position within the structuring element. Negative values mean that the anchor is at the center.</param>
      /// <param name="iterations">Number of times erosion and dilation to be applied.</param>
      /// <param name="srcDepth">The depth type of the source image</param>
      /// <param name="srcChannels">The number of channels in the source image</param>
      public CudaMorphologyFilter(CvEnum.MorphOp op, DepthType srcDepth, int srcChannels, IInputArray kernel, Point anchor, int iterations)
      {
         using (InputArray iaKernel = kernel.GetInputArray())
            _ptr = CudaInvoke.cudaCreateMorphologyFilter(op, CvInvoke.MakeType(srcDepth, srcChannels), iaKernel, ref anchor, iterations, ref _sharedPtr);
      }
   }

   public static partial class CudaInvoke
   {
      [DllImport(CvInvoke.ExternCudaLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      internal static extern IntPtr cudaCreateMorphologyFilter(CvEnum.MorphOp op, int srcType, IntPtr kernel, ref Point anchor, int iterations, ref IntPtr sharedPtr);
   }
}
