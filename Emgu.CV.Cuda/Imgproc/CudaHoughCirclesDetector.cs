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
   public class CudaHoughCirclesDetector : UnmanagedObject
   {
      public CudaHoughCirclesDetector(float dp, float minDist, int cannyThreshold, int votesThreshold, int minRadius, int maxRadius, int maxCircles)
      {
         _ptr = CudaInvoke.cudaHoughCirclesDetectorCreate(dp, minDist, cannyThreshold, votesThreshold, minRadius, maxRadius, maxCircles);
      }

      public GpuMat Detect(CudaImage<Gray, Byte> image)
      {
         GpuMat circles = new GpuMat();
         CudaInvoke.cudaHoughCirclesDetectorDetect(_ptr, image, circles);
         return circles;
      }

      protected override void DisposeObject()
      {
         CudaInvoke.cudaHoughCirclesDetectorRelease(ref _ptr);
      }
   }

   public static partial class CudaInvoke
   {
      [DllImport(CvInvoke.EXTERN_CUDA_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      internal static extern IntPtr cudaHoughCirclesDetectorCreate(float dp, float minDist, int cannyThreshold, int votesThreshold, int minRadius, int maxRadius, int maxCircles);

      [DllImport(CvInvoke.EXTERN_CUDA_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      internal static extern void cudaHoughCirclesDetectorDetect(IntPtr detector, IntPtr src, IntPtr circles);

      [DllImport(CvInvoke.EXTERN_CUDA_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      internal static extern void cudaHoughCirclesDetectorRelease(ref IntPtr detector);
   }
}
