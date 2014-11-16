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
   /// See Detects corners using FAST algorithm by E. Rosten ("Machine learning for high-speed corner
   /// detection, 2006).
   /// </summary>
   public class FastDetector : Feature2D
   {
      static FastDetector()
      {
         CvInvoke.CheckLibraryLoaded();
      }

      public enum DetectorType
      {
         Type5_8 = 0,
         Type7_12 = 1,
         Type9_16 = 2
      }

      /// <summary>
      /// Create a fast detector with the specific parameters
      /// </summary>
      /// <param name="threshold">Threshold on difference between intensity of center pixel and pixels on circle around
      /// this pixel.</param>
      /// <param name="nonmaxSupression">Specify if non-maximum suppression should be used.</param>
      public FastDetector(int threshold = 10, bool nonmaxSupression = true, DetectorType type = DetectorType.Type9_16)
      {
         _ptr = CvFASTGetFeatureDetector(threshold, nonmaxSupression, type, ref _feature2D);
      }

      /// <summary>
      /// Release the unmanaged memory associated with this detector.
      /// </summary>
      protected override void DisposeObject()
      {
         if (_ptr != IntPtr.Zero)
            CvFASTFeatureDetectorRelease(ref _ptr);
         base.DisposeObject();
      }

      [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      internal extern static IntPtr CvFASTGetFeatureDetector(
         int threshold,
         [MarshalAs(CvInvoke.BoolMarshalType)]
         bool nonmaxSupression, 
         DetectorType type,
         ref IntPtr feature2D);

      [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      internal extern static void CvFASTFeatureDetectorRelease(ref IntPtr detector);
   }
}
