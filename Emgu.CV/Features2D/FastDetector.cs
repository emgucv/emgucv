//----------------------------------------------------------------------------
//  Copyright (C) 2004-2011 by EMGU. All rights reserved.       
//----------------------------------------------------------------------------

﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using Emgu.CV.Structure;
using Emgu.CV.Util;
using Emgu.Util;

namespace Emgu.CV.Features2D
{
   /// <summary>
   /// FAST(Features from Accelerated Segment Test) keypoint detector
   /// </summary>
   public class FastDetector : DisposableObject, IKeyPointDetector
   {
      #region PInvoke
      [DllImport(CvInvoke.EXTERN_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      private extern static IntPtr CvFASTGetFeatureDetector(
         int threshold,
         [MarshalAs(CvInvoke.BoolMarshalType)]
         bool nonmax_supression);

      [DllImport(CvInvoke.EXTERN_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      private extern static void CvFASTFeatureDetectorRelease(ref IntPtr detector);
      #endregion

      private int _threshold;
      private bool _nonmaxSupression;
      private IntPtr _featureDetectorPtr;

      /// <summary>
      /// FAST threshold
      /// </summary>
      public int Threshold { get { return _threshold; } }
      /// <summary>
      /// Specifiy if non-maximum supression should be used
      /// </summary>
      public bool NonmaxSupression { get { return _nonmaxSupression; } }

      /// <summary>
      /// Create a fast detector with the specific parameters
      /// </summary>
      /// <param name="threshold">FAST threshold</param>
      /// <param name="nonmaxSupression">Specifiy if non-maximum supression should be used</param>
      public FastDetector(int threshold, bool nonmaxSupression)
      {
         _threshold = threshold;
         _nonmaxSupression = nonmaxSupression;
         _featureDetectorPtr = CvFASTGetFeatureDetector(Threshold, NonmaxSupression);
      }

      #region IKeyPointDetector Members
      /// <summary>
      /// Detect the FAST keypoints from the image
      /// </summary>
      /// <param name="image">The image to extract SURF features from</param>
      /// <param name="mask">The optional mask, can be null if not needed</param>
      /// <returns>An array of FAST key points</returns>
      public VectorOfKeyPoint DetectKeyPointsRaw(Image<Gray, Byte> image, Image<Gray, byte> mask)
      {
         VectorOfKeyPoint kpts = new VectorOfKeyPoint();
         CvInvoke.CvFeatureDetectorDetectKeyPoints(_featureDetectorPtr, image, mask, kpts);
         return kpts;
      }

      /// <summary>
      /// Get the feature detector. 
      /// </summary>
      /// <returns>The feature detector</returns>
      IntPtr IKeyPointDetector.FeatureDetectorPtr
      {
         get
         {
            return _featureDetectorPtr;
         }
      }
      #endregion

      /// <summary>
      /// Release the unmanaged memory associated with this detector.
      /// </summary>
      protected override void DisposeObject()
      {
         CvFASTFeatureDetectorRelease(ref _featureDetectorPtr);
      }
   }
}
