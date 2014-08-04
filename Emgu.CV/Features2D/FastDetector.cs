//----------------------------------------------------------------------------
//  Copyright (C) 2004-2014 by EMGU Corporation. All rights reserved.       
//----------------------------------------------------------------------------

 using System;
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
   public class FastDetector : UnmanagedObject, IFeatureDetector
   {
      static FastDetector()
      {
         CvInvoke.CheckLibraryLoaded();
      }
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
      /// this pixel.</param>
      /// <param name="nonmaxSupression">Specifiy if non-maximum supression should be used.</param>
      public FastDetector(int threshold = 10, bool nonmaxSupression = true)
      {
         _threshold = threshold;
         _nonmaxSupression = nonmaxSupression;
         _ptr = CvFASTGetFeatureDetector(Threshold, NonmaxSupression);
      }

      #region IFeatureDetector Members
      /// <summary>
      /// Get the feature detector. 
      /// </summary>
      /// <returns>The feature detector</returns>
      IntPtr IFeatureDetector.FeatureDetectorPtr
      {
         get
         {
            return Ptr;
         }
      }
      #endregion

      IntPtr IAlgorithm.AlgorithmPtr
      {
         get
         {
            return CvInvoke.cveAlgorithmFromFeatureDetector(((IFeatureDetector)this).FeatureDetectorPtr);
         }
      }

      /// <summary>
      /// Release the unmanaged memory associated with this detector.
      /// </summary>
      protected override void DisposeObject()
      {
         if (_ptr != IntPtr.Zero)
            CvFASTFeatureDetectorRelease(ref _ptr);
      }

      [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      internal extern static IntPtr CvFASTGetFeatureDetector(
         int threshold,
         [MarshalAs(CvInvoke.BoolMarshalType)]
         bool nonmaxSupression);

      [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      internal extern static void CvFASTFeatureDetectorRelease(ref IntPtr detector);
   }
}
