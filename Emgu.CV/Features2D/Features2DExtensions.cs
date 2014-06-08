//----------------------------------------------------------------------------
//  Copyright (C) 2004-2014 by EMGU Corporation. All rights reserved.       
//----------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Emgu.CV.Structure;
using Emgu.CV;
using Emgu.CV.Util;

namespace Emgu.CV.Features2D
{
   /// <summary>
   /// A collection of Extension methods for IKeyPointDetector
   /// </summary>
   public static class Features2DExtensions
   {
      static Features2DExtensions()
      {
         CvInvoke.CheckLibraryLoaded();
      }

      /// <summary>
      /// Detect the features in the image
      /// </summary>
      /// <param name="detector">The feature detector</param>
      /// <param name="keypoints">The result vector of keypoints</param>
      /// <param name="image">The image from which the features will be detected from</param>
      /// <param name="mask">The optional mask.</param>
      public static void DetectRaw(this IFeatureDetector detector, IInputArray image, VectorOfKeyPoint keypoints, IInputArray mask = null)
      {
         using (InputArray iaImage = image.GetInputArray())
         using (InputArray iaMask = mask == null ? InputArray.GetEmpty() : mask.GetInputArray())
         CvFeatureDetectorDetectKeyPoints(detector.FeatureDetectorPtr, iaImage, keypoints.Ptr, iaMask );   
      }

      /// <summary>
      /// Detect the keypoints from the image
      /// </summary>
      /// <param name="detector">The keypoint detector</param>
      /// <param name="image">The image to extract keypoints from</param>
      /// <param name="mask">The optional mask.</param>
      /// <returns>An array of key points</returns>
      public static MKeyPoint[] Detect(this IFeatureDetector detector, IInputArray image, IInputArray mask = null)
      {
         using (VectorOfKeyPoint keypoints = new VectorOfKeyPoint())
         {
            detector.DetectRaw(image, keypoints, mask);
            return keypoints.ToArray();
         }
      }

      /*
      /// <summary>
      /// Compute the descriptor given the image and the point location
      /// </summary>
      /// <param name="extractor">The descriptor extractor</param>
      /// <param name="image">The image where the descriptor will be computed from</param>
      /// <param name="keyPoints">The keypoint where the descriptor will be computed from</param>
      /// <returns>The descriptors founded on the keypoint location</returns>
      public static ImageFeature<TDescriptor>[] Compute<TColor, TDescriptor>(this IDescriptorExtractor<TColor, TDescriptor> extractor, Image<TColor, Byte> image, MKeyPoint[] keyPoints)
         where TColor : struct, IColor
         where TDescriptor : struct
      {
         if (keyPoints.Length == 0) return new ImageFeature<TDescriptor>[0];
         using (VectorOfKeyPoint kpts = new VectorOfKeyPoint())
         {
            kpts.Push(keyPoints);
            using (Matrix<TDescriptor> descriptor = extractor.Compute(image, kpts))
            {
               return ImageFeature<TDescriptor>.ConvertFromRaw(kpts, descriptor);
            }
         }
      }*/

      /// <summary>
      /// Compute the descriptors on the image from the given keypoint locations.
      /// </summary>
      /// <param name="extractor">The descriptor extractor</param>
      /// <param name="image">The image to compute descriptors from</param>
      /// <param name="keyPoints">The keypoints where the descriptor computation is perfromed</param>
      /// <param name="descriptors">The descriptors from the given keypoints</param>
      public static void Compute(this IDescriptorExtractor extractor, IInputArray image, VectorOfKeyPoint keyPoints, IOutputArray descriptors)
      {
         using (InputArray iaImage = image.GetInputArray())
         using (OutputArray oaDescriptors = descriptors.GetOutputArray())
         CvDescriptorExtractorCompute(extractor.DescriptorExtratorPtr, iaImage, keyPoints.Ptr, oaDescriptors);  
      }

      /// <summary>
      /// Get the number of elements in the descriptor.
      /// </summary>
      /// <param name="extractor">The descriptor extractor</param>
      /// <returns>The number of elements in the descriptor</returns>
      public static int GetDescriptorSize(this IDescriptorExtractor extractor)
      {
         return CvDescriptorExtractorGetDescriptorSize(extractor.DescriptorExtratorPtr);
      }

      [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      internal extern static void CvFeatureDetectorDetectKeyPoints(
         IntPtr detector,
         IntPtr image,
         IntPtr keypoints,
         IntPtr mask);

      [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      internal extern static void CvDescriptorExtractorCompute(IntPtr extractor, IntPtr image, IntPtr keypoints, IntPtr descriptors);

      [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      internal extern static int CvDescriptorExtractorGetDescriptorSize(IntPtr extractor);
   }
}
