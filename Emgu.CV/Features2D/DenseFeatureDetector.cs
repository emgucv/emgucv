/*
//----------------------------------------------------------------------------
//  Copyright (C) 2004-2015 by EMGU Corporation. All rights reserved.       
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
   public class DenseFeatureDetector : UnmanagedObject, IFeatureDetector
   {
      static DenseFeatureDetector()
      {
         CvInvoke.CheckLibraryLoaded();
      }

      /// <summary>
      /// Create a dense feature detector.
      /// </summary>
      /// <param name="initFeatureScale">Initial feature scale.</param>
      /// <param name="featureScaleLevels">The number of scale levels.</param>
      /// <param name="featureScaleMul">
      /// The level parameters (a feature scale, a node size, a size of boundary) are multiplied by <paramref name="featureScaleMul"/> with level index
      /// growing depending on input flags.
      /// </param>
      /// <param name="initXyStep">Initial X Y steps.</param>
      /// <param name="initImgBound">Initial image boundary.</param>
      /// <param name="varyXyStepWithScale">The grid node size is multiplied if varyXyStepWithScale is true.</param>
      /// <param name="varyImgBoundWithScale">Size of image boundary is multiplied if varyImgBoundWithScale is true.</param>
      public DenseFeatureDetector(
         float initFeatureScale = 1.0f,
         int featureScaleLevels = 1,
         float featureScaleMul = 0.1f,
         int initXyStep = 6,
         int initImgBound = 0,
         bool varyXyStepWithScale = true,
         bool varyImgBoundWithScale = false)
      {
         _ptr = CvDenseFeatureDetectorCreate(initFeatureScale, featureScaleLevels, featureScaleMul, initXyStep, initImgBound, varyXyStepWithScale, varyImgBoundWithScale);
      }

      #region IFeatureDetector Members
      /// <summary>
      /// Get the feature detector. 
      /// </summary>
      /// <returns>The feature detector</returns>
      IntPtr IFeatureDetector.FeatureDetectorPtr
      {
         get
         {
            return Ptr;
         }
      }
      #endregion

      IntPtr IAlgorithm.AlgorithmPtr
      {
         get
         {
            return CvInvoke.cveAlgorithmFromFeatureDetector(((IFeatureDetector)this).FeatureDetectorPtr);
         }
      }

      /// <summary>
      /// Release the unmanaged memory associated with this detector.
      /// </summary>
      protected override void DisposeObject()
      {
         if (_ptr != IntPtr.Zero)
            CvDenseFeatureDetectorRelease(ref _ptr);
      }

      [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
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

      [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      internal extern static void CvDenseFeatureDetectorRelease(ref IntPtr detector);
   }
}
*/