//----------------------------------------------------------------------------
//  Copyright (C) 2004-2015 by EMGU Corporation. All rights reserved.       
//----------------------------------------------------------------------------

﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Runtime.InteropServices;
﻿using Emgu.CV.CvEnum;
﻿using Emgu.CV.Structure;
using Emgu.Util;

namespace Emgu.CV.Cuda
{
   /// <summary>
   /// Cuda implementation of GoodFeaturesToTrackDetector
   /// </summary>
   public class CudaGoodFeaturesToTrackDetector : UnmanagedObject
   {
     
      /// <summary>
      /// Create the Cuda implementation of GoodFeaturesToTrackDetector
      /// </summary>
      public CudaGoodFeaturesToTrackDetector(DepthType srcDepth, int srcChannels, int maxCorners = 1000, double qualityLevel = 0.01, double minDistance = 0, int blockSize = 3, bool useHarrisDetector = false, double harrisK = 0.04)
      {
         _ptr = CudaInvoke.cudaGoodFeaturesToTrackDetectorCreate(CvInvoke.MakeType(srcDepth, srcChannels), maxCorners, qualityLevel, minDistance, blockSize, useHarrisDetector, harrisK);
      }

      /// <summary>
      /// Find the good features to track
      /// </summary>
      public void Detect(IInputArray image, IOutputArray corners, IInputArray mask = null, Stream stream = null)
      {
         using (InputArray iaImage = image.GetInputArray())
         using (OutputArray oaCorners = corners.GetOutputArray())
         using (InputArray iaMask = (mask == null ? mask.GetInputArray() : InputArray.GetEmpty()))
            CudaInvoke.cudaCornersDetectorDetect(_ptr, iaImage, oaCorners, iaMask, stream);
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
      internal extern static void cudaCornersDetectorDetect(IntPtr detector, IntPtr image, IntPtr corners, IntPtr mask, IntPtr stream);

      [DllImport(CvInvoke.ExternCudaLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      internal extern static void cudaCornersDetectorRelease(ref IntPtr detector);
   }
}
