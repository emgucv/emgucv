//----------------------------------------------------------------------------
//  Copyright (C) 2004-2014 by EMGU Corporation. All rights reserved.       
//----------------------------------------------------------------------------

﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Runtime.InteropServices;
using Emgu.CV.Structure;
using Emgu.Util;

namespace Emgu.CV.Cuda
{
   /// <summary>
   /// Cuda implementation of GoodFeaturesToTrackDetector
   /// </summary>
   public class CudaGoodFeaturesToTrackDetector<TColor, TDepth> : UnmanagedObject
      where TColor : struct, IColor
      where TDepth : new()
   {
      static int _srcType;

      static CudaGoodFeaturesToTrackDetector()
      {
         using (CudaImage<TColor, TDepth> tmp = new CudaImage<TColor, TDepth>(4, 4))
         {
            _srcType = tmp.Type;
         }
      }
      /// <summary>
      /// Create the Cuda implementation of GoodFeaturesToTrackDetector
      /// </summary>
      public CudaGoodFeaturesToTrackDetector(int maxCorners, double qualityLevel, double minDistance, int blockSize, bool useHarrisDetector, double harrisK)
      {
         _ptr = CudaInvoke.cudaGoodFeaturesToTrackDetectorCreate(_srcType, maxCorners, qualityLevel, minDistance, blockSize, useHarrisDetector, harrisK);
      }

      /// <summary>
      /// Find the good features to track
      /// </summary>
      public GpuMat<float> Detect(CudaImage<Gray, byte> image, CudaImage<Gray, byte> mask)
      {
         GpuMat<float> corners = new Cuda.GpuMat<float>();
         CudaInvoke.cudaCornersDetectorDetect(_ptr, image, corners, mask);
         return corners;
      }

      /// <summary>
      /// Release all the unmanaged memory associated with this detector
      /// </summary>
      protected override void DisposeObject()
      {
         CudaInvoke.cudaCornersDetectorRelease(ref _ptr);
      }
   }

   public static partial class CudaInvoke
   {
      [DllImport(CvInvoke.ExternCudaLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      internal extern static IntPtr cudaGoodFeaturesToTrackDetectorCreate(
         int srcType, int maxCorners, double qualityLevel, double minDistance, int blockSize, 
         [MarshalAs(CvInvoke.BoolMarshalType)]
         bool useHarrisDetector, 
         double harrisK );

      [DllImport(CvInvoke.ExternCudaLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      internal extern static void cudaCornersDetectorDetect(IntPtr detector, IntPtr image, IntPtr corners, IntPtr mask);

      [DllImport(CvInvoke.ExternCudaLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      internal extern static void cudaCornersDetectorRelease(ref IntPtr detector);
   }
}
