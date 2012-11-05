//----------------------------------------------------------------------------
//  Copyright (C) 2004-2012 by EMGU. All rights reserved.       
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
   /// Gaussian Mixture-based Backbround/Foreground Segmentation Algorithm.
   /// The class implements the following algorithm:
   /// "An improved adaptive background mixture model for real-time tracking with shadow detection"
   /// P. KadewTraKuPong and R. Bowden,
   /// Proc. 2nd European Workshp on Advanced Video-Based Surveillance Systems, 2001."
   /// http://personal.ee.surrey.ac.uk/Personal/R.Bowden/publications/avbs01/avbs01.pdf
   /// </summary>
   public class BackgroundSubtractorMOG : BackgroundSubtractor
   {
      /// <summary>
      /// Create an "Improved adaptive Gausian mixture model for background subtraction".
      /// </summary>
      /// <param name="history">The length of the history. Use 0 for default.</param>
      /// <param name="nmixtures">The maximum number of gaussian mixtures. Use 0 for default.</param>
      /// <param name="backgroundRatio">Use 0 for default.</param>
      /// <param name="noiseSigma">Use 0 for default.</param>
      public BackgroundSubtractorMOG(int history, int nmixtures, double backgroundRatio, double noiseSigma)
      {
         _ptr = CvInvoke.CvBackgroundSubtractorMOGCreate(history, nmixtures, backgroundRatio, noiseSigma);
      }

      /// <summary>
      /// Release all the unmanaged memory associated with this background model.
      /// </summary>
      protected override void DisposeObject()
      {
         CvInvoke.CvBackgroundSubtractorMOGRelease(ref _ptr);
      }
   }
}

namespace Emgu.CV
{
   public static partial class CvInvoke
   {
      [DllImport(CvInvoke.EXTERN_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      internal static extern IntPtr CvBackgroundSubtractorMOGCreate(int history, int nmixtures, double backgroundRatio, double noiseSigma);

      [DllImport(CvInvoke.EXTERN_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      internal static extern void CvBackgroundSubtractorMOGRelease(ref IntPtr bgSubstractor);
   }
}