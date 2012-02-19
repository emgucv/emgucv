//----------------------------------------------------------------------------
//  Copyright (C) 2004-2012 by EMGU. All rights reserved.       
//----------------------------------------------------------------------------

﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Runtime.InteropServices;
using Emgu.CV.Structure;
using Emgu.Util;

namespace Emgu.CV.GPU
{
   /// <summary>
   /// Gpu implementation of GoodFeaturesToTrackDetector
   /// </summary>
   public class GpuGoodFeaturesToTrackDetector : UnmanagedObject
   {
      /// <summary>
      /// Create the Gpu implementation of GoodFeaturesToTrackDetector
      /// </summary>
      public GpuGoodFeaturesToTrackDetector(int maxCorners, double qualityLevel, double minDistance)
      {
         _ptr = GpuInvoke.gpuGoodFeaturesToTrackDetectorCreate(maxCorners, qualityLevel, minDistance);
      }

      /// <summary>
      /// Find the good features to track
      /// </summary>
      public GpuMat<float> Detect(GpuImage<Gray, byte> image, GpuImage<Gray, byte> mask)
      {
         GpuMat<float> corners = new GPU.GpuMat<float>();
         GpuInvoke.gpuGoodFeaturesToTrackDetectorDetect(_ptr, image, corners, mask);
         return corners;
      }

      /// <summary>
      /// Release all the unmanaged memory associated with this detector
      /// </summary>
      protected override void DisposeObject()
      {
         GpuInvoke.gpuGoodFeaturesToTrackDetectorRelease(ref _ptr);
      }
   }

   public static partial class GpuInvoke
   {
      [DllImport(CvInvoke.EXTERN_GPU_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      internal extern static IntPtr gpuGoodFeaturesToTrackDetectorCreate(int maxCorners, double qualityLevel, double minDistance);

      [DllImport(CvInvoke.EXTERN_GPU_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      internal extern static void gpuGoodFeaturesToTrackDetectorDetect(IntPtr detector, IntPtr image, IntPtr corners, IntPtr mask);

      [DllImport(CvInvoke.EXTERN_GPU_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      internal extern static void gpuGoodFeaturesToTrackDetectorRelease(ref IntPtr detector);
   }
}
