//----------------------------------------------------------------------------
//  Copyright (C) 2004-2014 by EMGU Corporation. All rights reserved.       
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
   /// BRISK: Binary Robust Invariant Scalable Keypoints
   /// </summary>
   public class Brisk : Feature2D
   {
      static Brisk()
      {
         CvInvoke.CheckLibraryLoaded();
      }

      /// <summary>
      /// Create a BRISK keypoint detector and descriptor extractor.
      /// </summary>
      /// <param name="thresh">Feature parameters.</param>
      /// <param name="octaves">The number of octave layers.</param>
      /// <param name="patternScale">Pattern scale</param>
      public Brisk(int thresh = 30, int octaves = 3, float patternScale = 1.0f)
      {
         _ptr = CvBriskCreate(thresh, octaves, patternScale, ref _featureDetectorPtr, ref _descriptorExtractorPtr);
      }

      /// <summary>
      /// Release the unmanaged resources associated with this object
      /// </summary>
      protected override void DisposeObject()
      {
         CvBriskRelease(ref _ptr);
         base.DisposeObject();
      }

      [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      internal extern static IntPtr CvBriskCreate(int thresh, int octaves, float patternScale, ref IntPtr featureDetector, ref IntPtr descriptorExtractor);

      [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      internal extern static void CvBriskRelease(ref IntPtr detector);
   }
}
