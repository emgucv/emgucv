//----------------------------------------------------------------------------
//  Copyright (C) 2004-2012 by EMGU. All rights reserved.       
//----------------------------------------------------------------------------

using System;
using System.Collections.Generic;
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
      /// <summary>
      /// Detect the keypoints in the image
      /// </summary>
      /// <param name="detector">The keypoint detector</param>
      /// <param name="image">The image from which the key point will be detected from</param>
      /// <param name="mask">The optional mask, can be null if not needed</param>
      /// <returns>The key pionts in the image</returns>
      public static VectorOfKeyPoint DetectKeyPointsRaw(this IKeyPointDetector detector, Image<Gray, Byte> image, Image<Gray, Byte> mask)
      {
         VectorOfKeyPoint kpts = new VectorOfKeyPoint();
         CvInvoke.CvFeatureDetectorDetectKeyPoints(detector.FeatureDetectorPtr, image, mask, kpts);
         return kpts;
      }

      /// <summary>
      /// Detect the keypoints from the image
      /// </summary>
      /// <param name="detector">The keypoint detector</param>
      /// <param name="image">The image to extract keypoints from</param>
      /// <param name="mask">The optional mask, can be null if not needed</param>
      /// <returns>An array of key points</returns>
      public static MKeyPoint[] DetectKeyPoints(this IKeyPointDetector detector, Image<Gray, Byte> image, Image<Gray, byte> mask)
      {
         using (VectorOfKeyPoint keypoints = detector.DetectKeyPointsRaw(image, mask))
         {
            return keypoints.ToArray();
         }
      }

      /// <summary>
      /// Compute the descriptor given the image and the point location
      /// </summary>
      /// <param name="extractor">The descriptor extractor</param>
      /// <param name="image">The image where the descriptor will be computed from</param>
      /// <param name="mask">The optional mask, can be null if not needed</param>
      /// <param name="keyPoints">The keypoint where the descriptor will be computed from</param>
      /// <returns>The descriptors founded on the keypoint location</returns>
      public static ImageFeature<TDepth>[] ComputeDescriptors<TDepth>(this IDescriptorExtractor<TDepth> extractor, Image<Gray, Byte> image, Image<Gray, byte> mask, MKeyPoint[] keyPoints)
         where TDepth :struct
      {
         if (keyPoints.Length == 0) return new ImageFeature<TDepth>[0];
         using (VectorOfKeyPoint kpts = new VectorOfKeyPoint())
         {
            kpts.Push(keyPoints);
            using (Matrix<TDepth> descriptor = extractor.ComputeDescriptorsRaw(image, mask, kpts))
            {
               return ImageFeature<TDepth>.ConvertFromRaw(kpts, descriptor);
            }
         }
      }
   }
}
