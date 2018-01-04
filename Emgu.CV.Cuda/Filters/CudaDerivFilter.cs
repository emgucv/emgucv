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

   public class CudaDerivFilter : CudaFilter
   {

      public CudaDerivFilter(
          DepthType srcDepth, int srcChannels, DepthType dstDepth, int dstChannels,
          int dx, int dy,
          int ksize, bool normalize, double scale,
          CvEnum.BorderType rowBorderType = BorderType.Default,
          CvEnum.BorderType columnBorderType = BorderType.Default)
        {
         _ptr = CudaInvoke.cudaCreateDerivFilter(CvInvoke.MakeType(srcDepth, srcChannels), CvInvoke.MakeType(dstDepth, dstChannels), dx, dy, ksize, normalize, scale, rowBorderType, columnBorderType);
      }
   }

   public static partial class CudaInvoke
   {
      [DllImport(CvInvoke.ExternCudaLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      internal static extern IntPtr cudaCreateDerivFilter(
          int srcType, int dstType, 
          int dx, int dy,
          int ksize,
          [MarshalAs(CvInvoke.BoolMarshalType)]
          bool normalize, 
          double scale,
          CvEnum.BorderType rowBorderMode, CvEnum.BorderType columnBorderMode);
   }
}
