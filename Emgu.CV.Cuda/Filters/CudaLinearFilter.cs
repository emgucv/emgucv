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
   /// Applies arbitrary linear filter to the image. In-place operation is supported. When the aperture is partially outside the image, the function interpolates outlier pixel values from the nearest pixels that is inside the image
   /// </summary>
   public class CudaLinearFilter : CudaFilter
   {
      /// <summary>
      /// Create a Gpu LinearFilter
      /// </summary>
      /// <param name="kernel">Convolution kernel, single-channel floating point matrix (e.g. Emgu.CV.Matrix). If you want to apply different kernels to different channels, split the gpu image into separate color planes and process them individually</param>
      /// <param name="anchor">The anchor of the kernel that indicates the relative position of a filtered point within the kernel. The anchor shoud lie within the kernel. The special default value (-1,-1) means that it is at the kernel center</param>
      /// <param name="borderType">Border type. Use REFLECT101 as default.</param>
      /// <param name="borderValue">The border value</param>
      /// <param name="srcDepth">The depth type of the source image</param>
      /// <param name="srcChannels">The number of channels in the source image</param>
      /// <param name="dstDepth">The depth type of the dest image</param>
      /// <param name="dstChannels">The number of channels in the dest image</param>
      public CudaLinearFilter(
         DepthType srcDepth, int srcChannels,
         DepthType dstDepth, int dstChannels,
         IInputArray kernel,
         System.Drawing.Point anchor,
         CvEnum.BorderType borderType = BorderType.Default, MCvScalar borderValue = new MCvScalar())
      {
         using (InputArray iaKernel = kernel.GetInputArray())
            _ptr = CudaInvoke.cudaCreateLinearFilter(
               CvInvoke.MakeType(srcDepth, srcChannels), CvInvoke.MakeType(dstDepth, dstChannels),
               iaKernel, ref anchor, borderType, ref borderValue, ref _sharedPtr);
      }
   }

   public static partial class CudaInvoke
   {
      [DllImport(CvInvoke.ExternCudaLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      internal static extern IntPtr cudaCreateLinearFilter(int srcType, int dstType, IntPtr kernel, ref Point anchor, CvEnum.BorderType borderMode, ref MCvScalar borderValue, ref IntPtr sharedPtr);
   }
}
