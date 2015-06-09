//----------------------------------------------------------------------------
//  Copyright (C) 2004-2015 by EMGU Corporation. All rights reserved.       
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
   public class GFTTDetector : Feature2D
   {
      static GFTTDetector()
      {
         CvInvoke.CheckLibraryLoaded();
      }

      /// <summary>
      /// Create a Good Feature to Track detector
      /// </summary>
      /// <remarks>The function first calculates the minimal eigenvalue for every source image pixel using cvCornerMinEigenVal function and stores them in eig_image. Then it performs non-maxima suppression (only local maxima in 3x3 neighborhood remain). The next step is rejecting the corners with the minimal eigenvalue less than quality_level?max(eig_image(x,y)). Finally, the function ensures that all the corners found are distanced enough one from another by considering the corners (the most strongest corners are considered first) and checking that the distance between the newly considered feature and the features considered earlier is larger than min_distance. So, the function removes the features than are too close to the stronger features</remarks>
      /// <param name="maxCorners">The maximum number of features to be detected.</param>
      /// <param name="qualityLevel">Multiplier for the maxmin eigenvalue; specifies minimal accepted quality of image corners.</param>
      /// <param name="minDistance">Limit, specifying minimum possible distance between returned corners; Euclidian distance is used.</param>
      /// <param name="blockSize">Size of the averaging block, passed to underlying cvCornerMinEigenVal or cvCornerHarris used by the function.</param>
      /// <param name="useHarrisDetector">If true, will use Harris corner detector.</param>
      /// <param name="k">K</param>
      public GFTTDetector(int maxCorners = 1000, double qualityLevel = 0.01, double minDistance = 1, int blockSize = 3, bool useHarrisDetector = false, double k = 0.04)
      {
         _ptr = CvInvoke.cveGFTTDetectorCreate(maxCorners, qualityLevel, minDistance, blockSize, useHarrisDetector, k, ref _feature2D);
      }

      /// <summary>
      /// Release the unmanaged memory associated with this detector.
      /// </summary>
      protected override void DisposeObject()
      {
         if(_ptr != IntPtr.Zero)
            CvInvoke.cveGFTTDetectorRelease(ref _ptr);

         base.DisposeObject();
      }


   }
}

namespace Emgu.CV
{
   public static partial class CvInvoke
   {
      [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      internal extern static IntPtr cveGFTTDetectorCreate(
         int maxCorners,
         double qualityLevel,
         double minDistance,
         int blockSize,
         [MarshalAs(CvInvoke.BoolMarshalType)]
         bool useHarrisDetector,
         double k,
         ref IntPtr feature2DPtr);

      [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      internal extern static void cveGFTTDetectorRelease(ref IntPtr detector);
   }
}
