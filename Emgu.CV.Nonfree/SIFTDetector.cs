//----------------------------------------------------------------------------
//  Copyright (C) 2004-2013 by EMGU. All rights reserved.       
//----------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;
using Emgu.CV.Structure;
using Emgu.CV.Util;
using Emgu.CV;
using Emgu.Util;
using Emgu.CV.Features2D;

namespace Emgu.CV.Nonfree
{
   /// <summary>
   /// Wrapped SIFT detector
   /// </summary>
   public class SIFTDetector : Feature2D<float>
   {
      /// <summary>
      /// Create a SIFTDetector using the specific values
      /// </summary>
      /// <param name="nFeatures">The desired number of features. Use 0 for un-restricted number of features</param>
      /// <param name="nOctaveLayers">The number of octave layers. Use 3 for default</param>
      /// <param name="contrastThreshold">Contrast threshold. Use 0.04 as default</param>
      /// <param name="edgeThreshold">Detector parameter. Use 10.0 as default</param>
      /// <param name="sigma">Use 1.6 as default</param>
      public SIFTDetector(
         int nFeatures, int nOctaveLayers,
         double contrastThreshold, double edgeThreshold,
         double sigma)
      {
         _ptr = NonfreeInvoke.CvSIFTDetectorCreate(nFeatures, nOctaveLayers, contrastThreshold, edgeThreshold, sigma, ref _featureDetectorPtr, ref _descriptorExtractorPtr);
      }

      /// <summary>
      /// Create a SIFT detector with the default parameters
      /// </summary>
      public SIFTDetector()
         : this(0, 3, 0.04, 10, 1.6)
      {
      }

      /// <summary>
      /// Release the unmanaged resources associated with this object
      /// </summary>
      protected override void DisposeObject()
      {
         NonfreeInvoke.CvSIFTDetectorRelease(ref _ptr);
         base.DisposeObject();
      }
   }

   public static partial class NonfreeInvoke
   {
      [DllImport(CvInvoke.EXTERN_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      internal extern static IntPtr CvSIFTDetectorCreate(
         int nFeatures, int nOctaveLayers,
         double contrastThreshold, double edgeThreshold,
         double sigma, ref IntPtr featureDetector, ref IntPtr descriptorExtractor);

      [DllImport(CvInvoke.EXTERN_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      internal extern static void CvSIFTDetectorRelease(ref IntPtr detector);
   }
}