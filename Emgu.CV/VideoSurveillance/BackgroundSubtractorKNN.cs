//----------------------------------------------------------------------------
//  Copyright (C) 2004-2015 by EMGU Corporation. All rights reserved.       
//----------------------------------------------------------------------------

using System;
using System.Drawing;
using System.Runtime.InteropServices;
using Emgu.CV;
using Emgu.CV.Structure;
using Emgu.Util;

namespace Emgu.CV.VideoSurveillance
{
   /// <summary>
   /// K-nearest neighbors - based Background/Foreground Segmentation Algorithm.
   /// </summary>
   public class BackgroundSubtractorKNN : BackgroundSubtractor
   {
      /// <summary>
      /// Create a K-nearest neighbors - based Background/Foreground Segmentation Algorithm.
      /// </summary>
      /// <param name="history">Length of the history.</param>
      /// <param name="dist2Threshold">Threshold on the squared distance between the pixel and the sample to decide whether a pixel is close to that sample. This parameter does not affect the background update.</param>
      /// <param name="detectShadows">If true, the algorithm will detect shadows and mark them. It decreases the speed a bit, so if you do not need this feature, set the parameter to false.</param>
      public BackgroundSubtractorKNN(int history, double dist2Threshold, bool detectShadows)
      {
         _ptr = CvBackgroundSubtractorKNNCreate(history, dist2Threshold, detectShadows);
      }

      /// <summary>
      /// Release all the unmanaged memory associated with this background model.
      /// </summary>
      protected override void DisposeObject()
      {
         CvBackgroundSubtractorKNNRelease(ref _ptr);
      }

      [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      internal static extern IntPtr CvBackgroundSubtractorKNNCreate(
         int history,
         double dist2Threshold,
         [MarshalAs(CvInvoke.BoolMarshalType)]
         bool detectShadows);

      [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      internal static extern void CvBackgroundSubtractorKNNRelease(ref IntPtr bgSubstractor);
   }
}
