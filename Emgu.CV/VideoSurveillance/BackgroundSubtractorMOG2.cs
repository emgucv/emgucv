//----------------------------------------------------------------------------
//  Copyright (C) 2004-2015 by EMGU Corporation. All rights reserved.       
//----------------------------------------------------------------------------

using System;
using Emgu.Util;
using Emgu.CV;
using Emgu.CV.Structure;
using System.Drawing;
using System.Runtime.InteropServices;

namespace Emgu.CV.VideoSurveillance
{
   /// <summary>
   /// The class implements the following algorithm:
   /// "Improved adaptive Gaussian mixture model for background subtraction"
   /// Z.Zivkovic
   /// International Conference Pattern Recognition, UK, August, 2004.
   /// http://www.zoranz.net/Publications/zivkovic2004ICPR.pdf
   /// </summary>
   public class BackgroundSubtractorMOG2 : BackgroundSubtractor
   {
      /// <summary>
      /// Create an "Improved adaptive Gaussian mixture model for background subtraction".
      /// </summary>
      /// <param name="history">The length of the history.</param>
      /// <param name="varThreshold">The maximum allowed number of mixture components. Actual number is determined dynamically per pixel.</param>
      /// <param name="shadowDetection">If true, the algorithm will detect shadows and mark them. It decreases the speed a bit, so if you do not need this feature, set the parameter to false.</param>
      public BackgroundSubtractorMOG2(int history = 500, float varThreshold = 16, bool shadowDetection = true)
      {
         _ptr = CvBackgroundSubtractorMOG2Create(history, varThreshold, shadowDetection);
      }

      /// <summary>
      /// Release all the unmanaged memory associated with this background model.
      /// </summary>
      protected override void DisposeObject()
      {
         CvBackgroundSubtractorMOG2Release(ref _ptr);
      }

      [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      internal static extern IntPtr CvBackgroundSubtractorMOG2Create(
         int history,
         float varThreshold,
         [MarshalAs(CvInvoke.BoolMarshalType)]
         bool bShadowDetection);

      [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      internal static extern void CvBackgroundSubtractorMOG2Release(ref IntPtr bgSubstractor);
   }
}