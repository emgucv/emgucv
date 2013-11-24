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
   public class CudaHoughLinesDetector : UnmanagedObject
   {
      public CudaHoughLinesDetector(float rho, float theta, int threshold, bool doSort, int maxLines)
      {
         _ptr = CudaInvoke.cudaHoughLinesDetectorCreate(rho, theta, threshold, doSort, maxLines);
      }

      public GpuMat Detect(CudaImage<Gray, Byte> image)
      {
         GpuMat lines = new GpuMat();
         CudaInvoke.cudaHoughLinesDetectorDetect(_ptr, image, lines);
         return lines;
      }

      protected override void DisposeObject()
      {
         CudaInvoke.cudaHoughLinesDetectorRelease(ref _ptr);
      }
   }

   public static partial class CudaInvoke
   {
      [DllImport(CvInvoke.EXTERN_CUDA_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      internal static extern IntPtr cudaHoughLinesDetectorCreate(
         float rho, float theta, int threshold, 
         [MarshalAs(CvInvoke.BoolMarshalType)]
         bool doSort, 
         int maxLines);

      [DllImport(CvInvoke.EXTERN_CUDA_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      internal static extern void cudaHoughLinesDetectorDetect(IntPtr detector, IntPtr src, IntPtr lines);

      [DllImport(CvInvoke.EXTERN_CUDA_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      internal static extern void cudaHoughLinesDetectorRelease(ref IntPtr detector);
   }
}
