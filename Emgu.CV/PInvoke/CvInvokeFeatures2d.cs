//----------------------------------------------------------------------------
//  Copyright (C) 2004-2011 by EMGU. All rights reserved.       
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
      /// Extracts the contours of Maximally Stable Extremal Regions
      /// </summary>
      /// <param name="img">The image where MSER will be extracted</param>
      /// <param name="mask">The mask for region of interest</param>
      /// <param name="contours">The contours where MSER will be stored</param>
      /// <param name="storage">Memory storage</param>
      /// <param name="parameters">MSER parameters</param>
      [DllImport(OPENCV_FEATURES2D_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      public static extern void cvExtractMSER(
         IntPtr img,
         IntPtr mask,
         ref IntPtr contours,
         IntPtr storage,
         MCvMSERParams parameters);

      /// <summary>
      /// Finds robust features in the image. For each feature it returns its location, size, orientation and optionally the descriptor, basic or extended. The function can be used for object tracking and localization, image stitching etc
      /// </summary>
      /// <param name="image">The input 8-bit grayscale image</param>
      /// <param name="mask">The optional input 8-bit mask. The features are only found in the areas that contain more than 50% of non-zero mask pixels</param>
      /// <param name="keypoints">The output parameter; double pointer to the sequence of keypoints. This will be the sequence of MCvSURFPoint structures</param>
      /// <param name="descriptors">The optional output parameter; double pointer to the sequence of descriptors; Depending on the params.extended value, each element of the sequence will be either 64-element or 128-element floating-point (CV_32F) vector. If the parameter is IntPtr.Zero, the descriptors are not computed</param>
      /// <param name="storage">Memory storage where keypoints and descriptors will be stored</param>
      /// <param name="parameters">Various algorithm parameters put to the structure CvSURFParams</param>
      /// <param name="useProvidedKeyPoints">If 1, the provided key points are locations for computing SURF descriptors</param>
      [DllImport(OPENCV_FEATURES2D_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      public static extern void cvExtractSURF(
         IntPtr image, IntPtr mask,
         ref IntPtr keypoints,
         ref IntPtr descriptors,
         IntPtr storage,
         MCvSURFParams parameters,
         int useProvidedKeyPoints);

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
