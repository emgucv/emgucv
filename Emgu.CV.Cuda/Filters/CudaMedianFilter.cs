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
    /// median filtering for each point of the source image.
    /// </summary>
    public class MedianFilter : CudaFilter
   {
        /// <summary>
        /// Create a median filter
        /// </summary>
        /// <param name="srcDepth">Type of of source image. Only 8U images are supported for now.</param>
        /// <param name="srcChannels">Type of of source image. Only single channel images are supported for now.</param>
        /// <param name="windowSize">Size of the kernerl used for the filtering. Uses a (windowSize x windowSize) filter.</param>
        /// <param name="partition">Specifies the parallel granularity of the workload. This parameter should be used GPU experts when optimizing performance.</param>
        public MedianFilter(DepthType srcDepth, int srcChannels, int windowSize, int partition = 128)
      {
         _ptr = CudaInvoke.cudaCreateMedianFilter(CvInvoke.MakeType(srcDepth, srcChannels), windowSize, partition, ref _sharedPtr);
      }
   }

   public static partial class CudaInvoke
   {
      [DllImport(CvInvoke.ExternCudaLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      internal static extern IntPtr cudaCreateMedianFilter(int srcType, int windowSize, int partition, ref IntPtr sharedPtr);
   }
}
