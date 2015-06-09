//----------------------------------------------------------------------------
//  Copyright (C) 2004-2015 by EMGU Corporation. All rights reserved.       
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

      /// <summary>
      /// One of the three neighborhoods as defined in the paper
      /// </summary>
      public enum DetectorType
      {
         /// <summary>
         /// The type5_8
         /// </summary>
         Type5_8 = 0,
         /// <summary>
         /// The type7_12
         /// </summary>
         Type7_12 = 1,
         /// <summary>
         /// The type9_16
         /// </summary>
         Type9_16 = 2
      }

      /// <summary>
      /// Create a fast detector with the specific parameters
      /// </summary>
      /// <param name="threshold">Threshold on difference between intensity of center pixel and pixels on circle around
      /// this pixel.</param>
      /// <param name="nonmaxSupression">Specify if non-maximum suppression should be used.</param>
      /// <param name="type">One of the three neighborhoods as defined in the paper</param>
      public FastDetector(int threshold = 10, bool nonmaxSupression = true, DetectorType type = DetectorType.Type9_16)
      {
         _ptr = CvInvoke.cveFASTGetFeatureDetector(threshold, nonmaxSupression, type, ref _feature2D);
      }

      /// <summary>
      /// Release the unmanaged memory associated with this detector.
      /// </summary>
      protected override void DisposeObject()
      {
         if (_ptr != IntPtr.Zero)
            CvInvoke.cveFASTFeatureDetectorRelease(ref _ptr);
         base.DisposeObject();
      }

   }

}

namespace Emgu.CV
{
   public static partial class CvInvoke
   {
      [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      internal extern static IntPtr cveFASTGetFeatureDetector(
         int threshold,
         [MarshalAs(CvInvoke.BoolMarshalType)]
         bool nonmaxSupression,
         Features2D.FastDetector.DetectorType type,
         ref IntPtr feature2D);

      [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      internal extern static void cveFASTFeatureDetectorRelease(ref IntPtr detector);
   }
}

