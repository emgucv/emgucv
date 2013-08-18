//----------------------------------------------------------------------------
//  Copyright (C) 2004-2013 by EMGU. All rights reserved.       
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
   ///Class for generation of image features which are distributed densely and regularly over the image.
   /// </summary>
   public class DenseFeatureDetector : UnmanagedObject, IKeyPointDetector
   {
      /// <summary>
      /// Create a dense feature detector.
      /// </summary>
      /// <param name="initFeatureScale">Initial feature scale. Use 1.0 for default.</param>
      /// <param name="featureScaleLevels">The number of scale levels. Use 1 for default</param>
      /// <param name="featureScaleMul">
      /// The level parameters (a feature scale, a node size, a size of boundary) are multiplied by <paramref name="featureScaleMul"/> with level index
      /// growing depending on input flags. Use 0.1f for default.
      /// </param>
      /// <param name="initXyStep">Initial X Y steps. Use 6 for default.</param>
      /// <param name="initImgBound">Initial image boundary. Use 0 for default.</param>
      /// <param name="varyXyStepWithScale">The grid node size is multiplied if varyXyStepWithScale is true. Use true for default.</param>
      /// <param name="varyImgBoundWithScale">Size of image boundary is multiplied if varyImgBoundWithScale is true. Use false for default.</param>
      public DenseFeatureDetector(
         float initFeatureScale,
         int featureScaleLevels,
         float featureScaleMul,
         int initXyStep,
         int initImgBound,
         bool varyXyStepWithScale,
         bool varyImgBoundWithScale)
      {
         _ptr = CvInvoke.CvDenseFeatureDetectorCreate(initFeatureScale, featureScaleLevels, featureScaleMul, initXyStep, initImgBound, varyXyStepWithScale, varyImgBoundWithScale);
      }

      #region IKeyPointDetector Members
      /// <summary>
      /// Get the feature detector. 
      /// </summary>
      /// <returns>The feature detector</returns>
      IntPtr IKeyPointDetector.FeatureDetectorPtr
      {
         get
         {
            return Ptr;
         }
      }
      #endregion

      /// <summary>
      /// Release the unmanaged memory associated with this detector.
      /// </summary>
      protected override void DisposeObject()
      {
         CvInvoke.CvDenseFeatureDetectorRelease(ref _ptr);
      }
   }
}

namespace Emgu.CV
{
   public static partial class CvInvoke
   {
      [DllImport(CvInvoke.EXTERN_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      internal extern static IntPtr CvDenseFeatureDetectorCreate(
         float initFeatureScale,
         int featureScaleLevels,
         float featureScaleMul,
         int initXyStep,
         int initImgBound,
         [MarshalAs(CvInvoke.BoolMarshalType)]
         bool varyXyStepWithScale,
         [MarshalAs(CvInvoke.BoolMarshalType)]
         bool varyImgBoundWithScale);

      [DllImport(CvInvoke.EXTERN_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      internal extern static void CvDenseFeatureDetectorRelease(ref IntPtr detector);
   }
}