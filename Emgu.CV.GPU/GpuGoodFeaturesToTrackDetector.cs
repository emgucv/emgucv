//----------------------------------------------------------------------------
//  Copyright (C) 2004-2013 by EMGU. All rights reserved.       
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
   public class GpuGoodFeaturesToTrackDetector<TColor, TDepth> : UnmanagedObject
      where TColor : struct, IColor
      where TDepth : new()
   {
      static int _srcType;

      static GpuGoodFeaturesToTrackDetector()
      {
         using (GpuImage<TColor, TDepth> tmp = new GpuImage<TColor, TDepth>(4, 4))
         {
            _srcType = tmp.Type;
         }
      }
      /// <summary>
      /// Create the Gpu implementation of GoodFeaturesToTrackDetector
      /// </summary>
      public GpuGoodFeaturesToTrackDetector(int maxCorners, double qualityLevel, double minDistance, int blockSize, bool useHarrisDetector, double harrisK)
      {
         _ptr = GpuInvoke.gpuGoodFeaturesToTrackDetectorCreate(_srcType, maxCorners, qualityLevel, minDistance, blockSize, useHarrisDetector, harrisK);
      }

      /// <summary>
      /// Find the good features to track
      /// </summary>
      public GpuMat<float> Detect(GpuImage<Gray, byte> image, GpuImage<Gray, byte> mask)
      {
         GpuMat<float> corners = new GPU.GpuMat<float>();
         GpuInvoke.gpuCornersDetectorDetect(_ptr, image, corners, mask);
         return corners;
      }

      /// <summary>
      /// Release all the unmanaged memory associated with this detector
      /// </summary>
      protected override void DisposeObject()
      {
         GpuInvoke.gpuCornersDetectorRelease(ref _ptr);
      }
   }

   public static partial class GpuInvoke
   {
      [DllImport(CvInvoke.EXTERN_GPU_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      internal extern static IntPtr gpuGoodFeaturesToTrackDetectorCreate(
         int srcType, int maxCorners, double qualityLevel, double minDistance, int blockSize, 
         [MarshalAs(CvInvoke.BoolMarshalType)]
         bool useHarrisDetector, 
         double harrisK );

      [DllImport(CvInvoke.EXTERN_GPU_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      internal extern static void gpuCornersDetectorDetect(IntPtr detector, IntPtr image, IntPtr corners, IntPtr mask);

      [DllImport(CvInvoke.EXTERN_GPU_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      internal extern static void gpuCornersDetectorRelease(ref IntPtr detector);
   }
}
