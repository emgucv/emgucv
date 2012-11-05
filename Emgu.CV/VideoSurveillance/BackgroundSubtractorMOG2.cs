//----------------------------------------------------------------------------
//  Copyright (C) 2004-2012 by EMGU. All rights reserved.       
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
   /// "Improved adaptive Gausian mixture model for background subtraction"
   /// Z.Zivkovic
   /// International Conference Pattern Recognition, UK, August, 2004.
   /// http://www.zoranz.net/Publications/zivkovic2004ICPR.pdf
   /// </summary>
   public class BackgroundSubtractorMOG2 : BackgroundSubtractor
   {
      /// <summary>
      /// Create an "Improved adaptive Gausian mixture model for background subtraction".
      /// </summary>
      /// <param name="history">The length of the history. Use 0 for default</param>
      /// <param name="varThreshold">The maximum allowed number of mixture comonents.  Actual number is determined dynamically per pixel.</param>
      /// <param name="bShadowDetection">If true, perform shadow detection.</param>
      public BackgroundSubtractorMOG2(int history, float varThreshold, bool bShadowDetection)
      {
         _ptr = CvInvoke.CvBackgroundSubtractorMOG2Create(history, varThreshold, bShadowDetection);
      }

      /// <summary>
      /// Release all the unmanaged memory associated with this background model.
      /// </summary>
      protected override void DisposeObject()
      {
         CvInvoke.CvBackgroundSubtractorMOG2Release(ref _ptr);
      }
   }
}

namespace Emgu.CV
{
   public static partial class CvInvoke
   {
      [DllImport(CvInvoke.EXTERN_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      internal static extern IntPtr CvBackgroundSubtractorMOG2Create(
         int history,
         float varThreshold,
         [MarshalAs(CvInvoke.BoolMarshalType)]
         bool bShadowDetection);

      [DllImport(CvInvoke.EXTERN_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      internal static extern void CvBackgroundSubtractorMOG2Release(ref IntPtr bgSubstractor);
   }
}