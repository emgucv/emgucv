//----------------------------------------------------------------------------
//  Copyright (C) 2004-2018 by EMGU Corporation. All rights reserved.       
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
   
   public class MedianFilter : CudaFilter
   {
      public MedianFilter(DepthType srcDepth, int srcChannels, int windowSize, int partition)
      {
         _ptr = CudaInvoke.cudaCreateMedianFilter(CvInvoke.MakeType(srcDepth, srcChannels), windowSize, partition);
      }
   }

   public static partial class CudaInvoke
   {
      [DllImport(CvInvoke.ExternCudaLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      internal static extern IntPtr cudaCreateMedianFilter(int srcType, int windowSize, int partition);
   }
}
