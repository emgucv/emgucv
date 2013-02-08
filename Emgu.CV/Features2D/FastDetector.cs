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
   /// FAST(Features from Accelerated Segment Test) keypoint detector. 
   /// See Detects corners using FAST algorithm by E. Rosten (”Machine learning for high-speed corner
   /// detection”, 2006).
   /// </summary>
   public class FastDetector : UnmanagedObject, IKeyPointDetector
   {
      private int _threshold;
      private bool _nonmaxSupression;

      /// <summary>
      /// Threshold on difference between intensity of center pixel and pixels on circle around
      /// this pixel. See description of the algorithm.
      /// </summary>
      public int Threshold { get { return _threshold; } }
      /// <summary>
      /// If it is true then non-maximum supression will be applied to detected corners
      /// (keypoints)
      /// </summary>
      public bool NonmaxSupression { get { return _nonmaxSupression; } }

      /// <summary>
      /// Create a fast detector with the specific parameters
      /// </summary>
      /// <param name="threshold">Threshold on difference between intensity of center pixel and pixels on circle around
      /// this pixel. Use 10 for default.</param>
      /// <param name="nonmaxSupression">Specifiy if non-maximum supression should be used. Use true for default.</param>
      public FastDetector(int threshold, bool nonmaxSupression)
      {
         _threshold = threshold;
         _nonmaxSupression = nonmaxSupression;
         _ptr = CvInvoke.CvFASTGetFeatureDetector(Threshold, NonmaxSupression);
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
         CvInvoke.CvFASTFeatureDetectorRelease(ref _ptr);
      }
   }
}

namespace Emgu.CV
{
   public static partial class CvInvoke
   {
      [DllImport(CvInvoke.EXTERN_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      internal extern static IntPtr CvFASTGetFeatureDetector(
         int threshold,
         [MarshalAs(CvInvoke.BoolMarshalType)]
         bool nonmax_supression);

      [DllImport(CvInvoke.EXTERN_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      internal extern static void CvFASTFeatureDetectorRelease(ref IntPtr detector);
   }
}