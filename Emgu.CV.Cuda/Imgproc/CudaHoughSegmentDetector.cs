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
   /// <summary>
   /// Base class for line segments detector algorithm.
   /// </summary>
   public class CudaHoughSegmentDetector : UnmanagedObject
   {
      /// <summary>
      /// Create a hough segment detector
      /// </summary>
      /// <param name="rho">Distance resolution of the accumulator in pixels.</param>
      /// <param name="theta">Angle resolution of the accumulator in radians.</param>
      /// <param name="minLineLength"> Minimum line length. Line segments shorter than that are rejected.</param>
      /// <param name="maxLineGap">Maximum allowed gap between points on the same line to link them.</param>
      /// <param name="maxLines">Maximum number of output lines.</param>
      public CudaHoughSegmentDetector(float rho, float theta, int minLineLength, int maxLineGap, int maxLines = 4096)
      {
         _ptr = CudaInvoke.cudaHoughSegmentDetectorCreate(rho, theta, minLineLength, maxLineGap, maxLines);
      }

      public GpuMat Detect(CudaImage<Gray, Byte> image)
      {
         GpuMat lines = new GpuMat();
         CudaInvoke.cudaHoughSegmentDetectorDetect(_ptr, image, lines);
         return lines;
      }

      /// <summary>
      /// Release the unmanaged memory associated with this segment detector
      /// </summary>
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
