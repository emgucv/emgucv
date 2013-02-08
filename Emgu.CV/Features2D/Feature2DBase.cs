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
   public abstract class Feature2DBase<TDescriptor> : UnmanagedObject, IKeyPointDetector, IDescriptorExtractor<Gray, TDescriptor>
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
      /// Compute the descriptors on the image from the given keypoint locations.
      /// </summary>
      /// <param name="image">The image to compute descriptors from</param>
      /// <param name="keyPoints">The keypoints where the descriptor computation is perfromed</param>
      /// <param name="mask">The optional mask, can be null if not needed</param>
      /// <returns>The descriptors from the given keypoints</returns>
      public Matrix<TDescriptor> ComputeDescriptorsRaw(Image<Gray, byte> image, Image<Gray, byte> mask, VectorOfKeyPoint keyPoints)
      {
         return DetectAndComputeHelper(image, mask, keyPoints, true);
      }

      /// <summary>
      /// Compute the descriptors on the image from the given keypoint locations.
      /// </summary>
      /// <param name="image">The image to compute descriptors from</param>
      /// <param name="keyPoints">The detected keypoints will be stored in this vector</param>
      /// <param name="mask">The optional mask, can be null if not needed</param>
      /// <returns>The descriptors from the given keypoints</returns>
      public Matrix<TDescriptor> DetectAndCompute(Image<Gray, Byte> image, Image<Gray, Byte> mask, VectorOfKeyPoint keyPoints)
      {
         return DetectAndComputeHelper(image, mask, keyPoints, false);
      }

      private Matrix<TDescriptor> DetectAndComputeHelper(Image<Gray, Byte> image, Image<Gray, Byte> mask, VectorOfKeyPoint keyPoints, bool useProvidedKeyPoints)
      {
         using (Mat descriptorMat = new Mat())
         {
            CvInvoke.CvFeature2DDetectAndCompute(_ptr, image, mask, keyPoints, descriptorMat, useProvidedKeyPoints);
            if (keyPoints.Size == 0)
               return null;
            Matrix<TDescriptor> result = new Matrix<TDescriptor>(descriptorMat.Size);
            CvInvoke.cvMatCopyToCvArr(descriptorMat, result);
            return result;
         }
      }

      /// <summary>
      /// Detect image features from the given image
      /// </summary>
      /// <param name="image">The image to detect features from</param>
      /// <param name="mask">The optional mask, can be null if not needed</param>
      /// <returns>The Image features detected from the given image</returns>
      public ImageFeature<TDescriptor>[] DetectFeatures(Image<Gray, Byte> image, Image<Gray, byte> mask)
      {
         using (VectorOfKeyPoint pts = new VectorOfKeyPoint())
         using (Matrix<TDescriptor> descVec = DetectAndCompute(image, mask, pts))
         {
            return ImageFeature<TDescriptor>.ConvertFromRaw(pts, descVec);
         }
      }

      /// <summary>
      /// Get the pointer to the descriptor extractor. 
      /// </summary>
      /// <returns>The descriptor extractor</returns>
      public IntPtr DescriptorExtratorPtr
      {
         get { return _descriptorExtractorPtr; }
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
}

namespace Emgu.CV
{
   public partial class CvInvoke
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
