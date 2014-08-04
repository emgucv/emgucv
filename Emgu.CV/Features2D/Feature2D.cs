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
   /// The feature 2D base class
   /// </summary>
   public abstract class Feature2D : UnmanagedObject, IFeatureDetector, IDescriptorExtractor
   {
      /// <summary>
      /// The pointer to the feature detector
      /// </summary>
      protected IntPtr _featureDetectorPtr;
      /// <summary>
      /// The pointer to the descriptor extractor.
      /// </summary>
      protected IntPtr _descriptorExtractorPtr;

      /// <summary>
      /// Get the pointer to the feature detector. 
      /// </summary>
      /// <returns>The feature detector</returns>
      public IntPtr FeatureDetectorPtr
      {
         get { return _featureDetectorPtr; }
      }

      /// <summary>
      /// Get the pointer to the descriptor extractor. 
      /// </summary>
      /// <returns>The descriptor extractor</returns>
      public IntPtr DescriptorExtratorPtr
      {
         get { return _descriptorExtractorPtr; }
      }

      IntPtr IAlgorithm.AlgorithmPtr
      {
         get
         {
            return CvInvoke.cveAlgorithmFromFeatureDetector(((IFeatureDetector)this).FeatureDetectorPtr);
         }
      }

      /// <summary>
      /// Detect keypoints in an image and compute the descriptors on the image from the keypoint locations.
      /// </summary>
      /// <param name="image">The image</param>
      /// <param name="mask">The optional mask, can be null if not needed</param>
      /// <param name="keyPoints">The detected keypoints will be stored in this vector</param>
      /// <param name="descriptors">The descriptors from the keypoints</param>
      /// <param name="useProvidedKeyPoints">If true, the method will skip the detection phase and will compute descriptors for the provided keypoints</param>
      public void DetectAndCompute(IInputArray image, IInputArray mask, VectorOfKeyPoint keyPoints, IOutputArray descriptors, bool useProvidedKeyPoints)
      {
         using (InputArray iaImage = image.GetInputArray())
         using (InputArray iaMask = mask == null ? InputArray.GetEmpty() : mask.GetInputArray())
         using (OutputArray oaDescriptors = descriptors.GetOutputArray())
            Feature2DInvoke.CvFeature2DDetectAndCompute(_ptr, iaImage, iaMask, keyPoints, oaDescriptors, useProvidedKeyPoints);
      }

      /// <summary>
      /// Reset the pointers
      /// </summary>
      protected override void DisposeObject()
      {
         _featureDetectorPtr = IntPtr.Zero;
         _descriptorExtractorPtr = IntPtr.Zero;
      }
   }

   internal partial class Feature2DInvoke
   {
      [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      internal extern static void CvFeature2DDetectAndCompute(
         IntPtr feature2D,
         IntPtr image,
         IntPtr mask,
         IntPtr keypoints,
         IntPtr descriptors,
         [MarshalAs(CvInvoke.BoolMarshalType)]
         bool useProvidedKeyPoints);
   }

}
