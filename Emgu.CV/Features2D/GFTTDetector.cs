//----------------------------------------------------------------------------
//  Copyright (C) 2004-2013 by EMGU. All rights reserved.       
//----------------------------------------------------------------------------

﻿using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;
using Emgu.CV.Structure;
using Emgu.CV.Util;
using Emgu.Util;

namespace Emgu.CV.Features2D
{
   /// <summary>
   ///  Wrapping class for feature detection using the goodFeaturesToTrack() function.
   /// </summary>
   public class GFTTDetector : UnmanagedObject, IKeyPointDetector
   {
      /// <summary>
      /// Create a Good Feature to Track detector
      /// </summary>
      /// <remarks>The function first calculates the minimal eigenvalue for every source image pixel using cvCornerMinEigenVal function and stores them in eig_image. Then it performs non-maxima suppression (only local maxima in 3x3 neighborhood remain). The next step is rejecting the corners with the minimal eigenvalue less than quality_level?max(eig_image(x,y)). Finally, the function ensures that all the corners found are distanced enough one from another by considering the corners (the most strongest corners are considered first) and checking that the distance between the newly considered feature and the features considered earlier is larger than min_distance. So, the function removes the features than are too close to the stronger features</remarks>
      /// <param name="maxCorners">The maximum number of features to be detected. Use 1000 for default</param>
      /// <param name="qualityLevel">Multiplier for the maxmin eigenvalue; specifies minimal accepted quality of image corners. Use 0.01 for default.</param>
      /// <param name="minDistance">Limit, specifying minimum possible distance between returned corners; Euclidian distance is used. Use 1 for default</param>
      /// <param name="blockSize">Size of the averaging block, passed to underlying cvCornerMinEigenVal or cvCornerHarris used by the function. Use 3 for default</param>
      /// <param name="useHarrisDetector">If true, will use Harris corner detector. Use false as default</param>
      /// <param name="k">K, use 0.04 for default</param>
      public GFTTDetector(int maxCorners, double qualityLevel, double minDistance, int blockSize, bool useHarrisDetector, double k)
      {
         _ptr = CvInvoke.CvGFTTDetectorCreate(maxCorners, qualityLevel, minDistance, blockSize, useHarrisDetector, k);
      }

      #region IKeyPointDetector Members
      /// <summary>
      /// Get the feature detector. 
      /// </summary>
      /// <returns>The feature detector</returns>
      IntPtr IKeyPointDetector.FeatureDetectorPtr
      {
         get
         {
            return Ptr;
         }
      }
      #endregion

      /// <summary>
      /// Release the unmanaged memory associated with this detector.
      /// </summary>
      protected override void DisposeObject()
      {
         CvInvoke.CvGFTTDetectorRelease(ref _ptr);
      }
   }
}

namespace Emgu.CV
{
   public static partial class CvInvoke
   {
      [DllImport(CvInvoke.EXTERN_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      internal extern static IntPtr CvGFTTDetectorCreate(int maxCorners, double qualityLevel, double minDistance, int blockSize, bool useHarrisDetector, double k);

      [DllImport(CvInvoke.EXTERN_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      internal extern static void CvGFTTDetectorRelease(ref IntPtr detector);
   }
}