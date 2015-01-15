//----------------------------------------------------------------------------
//  Copyright (C) 2004-2015 by EMGU Corporation. All rights reserved.       
//----------------------------------------------------------------------------

using System;
using System.Runtime.InteropServices;
using Emgu.CV;
using Emgu.CV.Structure;
using Emgu.Util;
using Emgu.CV.VideoSurveillance;

namespace Emgu.CV.BgSegm
{
   /// <summary>
   /// Background Subtractor module based on the algorithm given in:
   /// Andrew B. Godbehere, Akihiro Matsukawa, Ken Goldberg, 
   /// “Visual Tracking of Human Visitors under Variable-Lighting Conditions for a Responsive Audio Art Installation”, 
   /// American Control Conference, Montreal, June 2012.
   /// </summary>
   public class BackgroundSubtractorGMG : BackgroundSubtractor
   {
      /// <summary>
      /// Create a background subtractor module based on GMG
      /// </summary>
      /// <param name="initializationFrames">Number of frames used to initialize the background models.</param>
      /// <param name="decisionThreshold">Threshold value, above which it is marked foreground, else background.</param>
      public BackgroundSubtractorGMG(int initializationFrames, double decisionThreshold)
      {
         _ptr = CvBackgroundSubtractorGMGCreate(initializationFrames, decisionThreshold);
      }

      /// <summary>
      /// Release all the unmanaged memory associated with this background model.
      /// </summary>
      protected override void DisposeObject()
      {
         CvBackgroundSubtractorGMGRelease(ref _ptr);
      }

      [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      internal static extern IntPtr CvBackgroundSubtractorGMGCreate(int initializationFrames, double decisionThreshold);

      [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      internal static extern void CvBackgroundSubtractorGMGRelease(ref IntPtr bgSubstractor);
   }
}
