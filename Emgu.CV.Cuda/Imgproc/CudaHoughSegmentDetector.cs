//----------------------------------------------------------------------------
//  Copyright (C) 2004-2013 by EMGU. All rights reserved.       
//----------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using Emgu.CV;
using Emgu.CV.Structure;
using Emgu.Util;

namespace Emgu.CV.Cuda
{
   public class CudaHoughSegmentDetector : UnmanagedObject
   {
      public CudaHoughSegmentDetector(float rho, float theta, int minLineLength, int maxLineGap, int maxLines)
      {
         _ptr = CudaInvoke.cudaHoughSegmentDetectorCreate(rho, theta, minLineLength, maxLineGap, maxLines);
      }

      public GpuMat Detect(CudaImage<Gray, Byte> image)
      {
         GpuMat lines = new GpuMat();
         CudaInvoke.cudaHoughSegmentDetectorDetect(_ptr, image, lines);
         return lines;
      }

      protected override void DisposeObject()
      {
         CudaInvoke.cudaHoughSegmentDetectorRelease(ref _ptr);
      }
   }

   public static partial class CudaInvoke
   {
      [DllImport(CvInvoke.EXTERN_CUDA_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      internal static extern IntPtr cudaHoughSegmentDetectorCreate(float rho, float theta, int minLineLength, int maxLineGap, int maxLines);

      [DllImport(CvInvoke.EXTERN_CUDA_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      internal static extern void cudaHoughSegmentDetectorDetect(IntPtr detector, IntPtr src, IntPtr lines);

      [DllImport(CvInvoke.EXTERN_CUDA_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      internal static extern void cudaHoughSegmentDetectorRelease(ref IntPtr detector);
   }
}
