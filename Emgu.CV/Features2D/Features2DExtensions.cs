//----------------------------------------------------------------------------
//  Copyright (C) 2004-2013 by EMGU. All rights reserved.       
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
      /// <param name="image">The image from which the features will be detected from</param>
      /// <param name="mask">The optional mask.</param>
      /// <returns>The features in the image</returns>
      public static VectorOfKeyPoint DetectRaw(this IFeatureDetector detector, Image<Gray, Byte> image, Image<Gray, Byte> mask = null)
      {
         VectorOfKeyPoint kpts = new VectorOfKeyPoint();
         CvFeatureDetectorDetectKeyPoints(detector.FeatureDetectorPtr, image, mask, kpts);
         return kpts;
      }

      /// <summary>
      /// Detect the keypoints from the image
      /// </summary>
      /// <param name="detector">The keypoint detector</param>
      /// <param name="image">The image to extract keypoints from</param>
      /// <param name="mask">The optional mask.</param>
      /// <returns>An array of key points</returns>
      public static MKeyPoint[] Detect(this IFeatureDetector detector, Image<Gray, Byte> image, Image<Gray, byte> mask = null)
      {
         using (VectorOfKeyPoint keypoints = detector.DetectRaw(image, mask))
         {
            return keypoints.ToArray();
         }
      }

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
      }

      /// <summary>
      /// Compute the descriptors on the image from the given keypoint locations.
      /// </summary>
      /// <param name="extractor">The descriptor extractor</param>
      /// <param name="image">The image to compute descriptors from</param>
      /// <param name="keyPoints">The keypoints where the descriptor computation is perfromed</param>
      /// <returns>The descriptors from the given keypoints</returns>
      public static Matrix<TDescriptor> Compute<TColor, TDescriptor>(this IDescriptorExtractor<TColor, TDescriptor> extractor, Image<TColor, Byte> image, VectorOfKeyPoint keyPoints)
         where TColor : struct, IColor
         where TDescriptor : struct
      {
         using (Mat descriptors = new Mat())
         {
            CvDescriptorExtractorCompute(extractor.DescriptorExtratorPtr, image, keyPoints, descriptors.Ptr);
            if (keyPoints.Size == 0)
               return null;
            Matrix<TDescriptor> result = new Matrix<TDescriptor>(descriptors.Size);
            descriptors.CopyTo(result);
            
            return result;
         }
      }

      /// <summary>
      /// Get the number of elements in the descriptor.
      /// </summary>
      /// <typeparam name="TColor">The type of image the descriptor extractor operates on</typeparam>
      /// <typeparam name="TDescriptor">The depth of the type of descriptor</typeparam>
      /// <param name="extractor">The descriptor extractor</param>
      /// <returns>The number of elements in the descriptor</returns>
      public static int GetDescriptorSize<TColor, TDescriptor>(this IDescriptorExtractor<TColor, TDescriptor> extractor)
         where TColor : struct, IColor
         where TDescriptor : struct
      {
         return CvDescriptorExtractorGetDescriptorSize(extractor.DescriptorExtratorPtr);
      }

      [DllImport(CvInvoke.EXTERN_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      internal extern static void CvFeatureDetectorDetectKeyPoints(
         IntPtr detector,
         IntPtr image,
         IntPtr mask,
         IntPtr keypoints);

      [DllImport(CvInvoke.EXTERN_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      internal extern static void CvDescriptorExtractorCompute(IntPtr extractor, IntPtr image, IntPtr keypoints, IntPtr descriptors);

      [DllImport(CvInvoke.EXTERN_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      internal extern static int CvDescriptorExtractorGetDescriptorSize(IntPtr extractor);
   }
}
