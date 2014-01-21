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
   /// The feature 2D base class
   /// </summary>
   /// <typeparam name="TDescriptor">The type of data in the descriptor. Can be either float or byte</typeparam>
   public abstract class Feature2D<TDescriptor> : UnmanagedObject, IFeatureDetector, IDescriptorExtractor<Gray, TDescriptor>
            where TDescriptor : struct
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

      
      /// <summary>
      /// Detect keypoints in an image and compute the descriptors on the image from the keypoint locations.
      /// </summary>
      /// <param name="image">The image</param>
      /// <param name="keyPoints">The detected keypoints will be stored in this vector</param>
      /// <param name="mask">The optional mask, can be null if not needed</param>
      /// <returns>The descriptors from the given keypoints</returns>
      private Matrix<TDescriptor> DetectAndCompute(Image<Gray, Byte> image, Image<Gray, Byte> mask, VectorOfKeyPoint keyPoints)
      {
         using (Mat descriptor = new Mat())
         {
            DetectAndCompute(image, mask, keyPoints, descriptor, false);
            if (descriptor.IsEmpty)
               return null;
            
            Matrix<TDescriptor> res = new Matrix<TDescriptor>(descriptor.Size);
            descriptor.CopyTo(res);
            return res;
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
         Feature2DInvoke.CvFeature2DDetectAndCompute(_ptr, image.InputArrayPtr, mask == null ? IntPtr.Zero : mask.InputArrayPtr, keyPoints, descriptors.OutputArrayPtr, useProvidedKeyPoints);
      }

      /// <summary>
      /// Detect image features from the given image
      /// </summary>
      /// <param name="image">The image to detect features from</param>
      /// <param name="mask">The optional mask, can be null if not needed</param>
      /// <returns>The Image features detected from the given image</returns>
      public ImageFeature<TDescriptor>[] DetectAndCompute(Image<Gray, Byte> image, Image<Gray, byte> mask)
      {
         using (VectorOfKeyPoint pts = new VectorOfKeyPoint())
         using (Matrix<TDescriptor> descVec = DetectAndCompute(image, mask, pts))
         {
            return ImageFeature<TDescriptor>.ConvertFromRaw(pts, descVec);
         }
      }

      /*
      /// <summary>
      /// Get the size of the descriptor.
      /// </summary>
      public abstract int DescriptorSize
      {
         get;
      }*/

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
      [DllImport(CvInvoke.EXTERN_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
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
