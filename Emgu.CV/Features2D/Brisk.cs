//----------------------------------------------------------------------------
//  Copyright (C) 2004-2012 by EMGU. All rights reserved.       
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
   public class Brisk : Feature2DBase<byte>
   {
      /// <summary>
      /// Create a BRISK keypoint detector and descriptor extractor.
      /// </summary>
      /// <param name="thresh">Feature parameters, use 30 for default.</param>
      /// <param name="octaves">The number of octave layers. Use 3 for default</param>
      /// <param name="patternScale">Pattern scale, use 1.0f for default</param>
      public Brisk(int thresh, int octaves, float patternScale)
      {
         _ptr = CvInvoke.CvBriskCreate(thresh, octaves, patternScale, ref _featureDetectorPtr, ref _descriptorExtractorPtr);
      }

      /// <summary>
      /// Release the unmanaged resources associated with this object
      /// </summary>
      protected override void DisposeObject()
      {
         CvInvoke.CvBriskRelease(ref _ptr);
         base.DisposeObject();
      }
   }
}

namespace Emgu.CV
{
   public static partial class CvInvoke
   {
      [DllImport(CvInvoke.EXTERN_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      internal extern static IntPtr CvBriskCreate(int thresh, int octaves, float patternScale, ref IntPtr featureDetector, ref IntPtr descriptorExtractor);

      [DllImport(CvInvoke.EXTERN_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      internal extern static void CvBriskRelease(ref IntPtr detector);
   }
}
