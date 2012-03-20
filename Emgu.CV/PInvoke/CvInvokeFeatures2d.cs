//----------------------------------------------------------------------------
//  Copyright (C) 2004-2012 by EMGU. All rights reserved.       
//----------------------------------------------------------------------------

﻿using System;
using System.Runtime.InteropServices;
using System.Drawing;
using Emgu.CV.Structure;
using Emgu.CV.Features2D;

namespace Emgu.CV
{
   public static partial class CvInvoke
   {
      /// <summary>
      /// Create a CvSURFParams using the specific values
      /// </summary>
      /// <param name="hessianThreshold">      
      /// only features with keypoint.hessian larger than that are extracted.
      /// good default value is ~300-500 (can depend on the average local contrast and sharpness of the image).
      /// user can further filter out some features based on their hessian values and other characteristics
      /// </param>
      /// <param name="extended">      
      /// 0 means basic descriptors (64 elements each),
      /// 1 means extended descriptors (128 elements each)
      /// </param>
      /// <returns>The MCvSURFParams structure</returns>
      [DllImport(OPENCV_FEATURES2D_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      public static extern SURFDetector cvSURFParams(double hessianThreshold, int extended);

      /// <summary>
      /// Retrieve the star keypoint location from the specific image
      /// </summary>
      /// <param name="img">The image to detect start keypoints</param>
      /// <param name="storage">The storage for the returned sequence</param>
      /// <param name="param">The star detector parameters</param>
      /// <returns>Pointer to the sequence of star keypoint locations</returns>
      [DllImport(OPENCV_FEATURES2D_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      public static extern IntPtr cvGetStarKeypoints(
         IntPtr img,
         IntPtr storage,
         MCvStarDetectorParams param);
   }
}
