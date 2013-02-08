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
   /// Wrapped SIFT detector
   /// </summary>
   public class SIFTDetector : Feature2DBase<float>
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
         _ptr = CvInvoke.CvSIFTDetectorCreate(nFeatures, nOctaveLayers, contrastThreshold, edgeThreshold, sigma, ref _featureDetectorPtr, ref _descriptorExtractorPtr);
      }

      /// <summary>
      /// Create a SIFT detector with the default parameters
      /// </summary>
      public SIFTDetector()
         : this(0, 3, 0.04, 10, 1.6)
      {
      }

      /*
      /// <summary>
      /// Get the size of the descriptor
      /// </summary>
      public override int DescriptorSize
      {
         get
         {
            return CvInvoke.CvSIFTDetectorGetDescriptorSize(_ptr);
         }
      }

      /// <summary>
      /// Compute the descriptor given the bgr image and the point location, using oppponent color (CGIV 2008 "Color Descriptors for Object Category Recognition").
      /// </summary>
      /// <param name="image">The image where the descriptor will be computed from</param>
      /// <param name="keyPoints">The keypoint where the descriptor will be computed from</param>
      /// <returns>The descriptors founded on the keypoint location</returns>
      public Matrix<float> ComputeDescriptorsRaw(Image<Bgr, Byte> image, VectorOfKeyPoint keyPoints)
      {
         int count = keyPoints.Size;
         if (count == 0) return null;
         Matrix<float> descriptors = new Matrix<float>(count, DescriptorSize * 3, 1);
         CvSIFTDetectorComputeDescriptorsBGR(_ptr, image, keyPoints, descriptors);
         return descriptors;
      }*/

      /// <summary>
      /// Release the unmanaged resources associated with this object
      /// </summary>
      protected override void DisposeObject()
      {
         CvInvoke.CvSIFTDetectorRelease(ref _ptr);
         base.DisposeObject();
      }

      /*
      /// <summary>
      /// Compute the descriptor given the image and the point location
      /// </summary>
      /// <param name="image">The image where the descriptor will be computed from</param>
      /// <param name="mask">The optional mask, can be null if not needed</param>
      /// <param name="keyPoints">The keypoint where the descriptor will be computed from. Keypoints for which a descriptor cannot be computed are removed.</param>
      /// <returns>The descriptors founded on the keypoint location</returns>
      private Matrix<float> ComputeDescriptorsRawHelper(CvArray<Byte> image, Image<Gray, byte> mask, VectorOfKeyPoint keyPoints)
      {
         if (mask != null)
            keyPoints.FilterByPixelsMask(mask);

         int count = keyPoints.Size;
         if (count == 0) return null;
         Matrix<float> descriptors = new Matrix<float>(count, image.NumberOfChannels * DescriptorSize, 1);
         CvInvoke.CvSIFTDetectorComputeDescriptors(_ptr, image, keyPoints, descriptors);
         return descriptors;
      }

      /// <summary>
      /// Compute the descriptor given the image and the point location, using oppponent color (CGIV 2008 "Color Descriptors for Object Category Recognition").
      /// </summary>
      /// <param name="image">The image where the descriptor will be computed from</param>
      /// <param name="mask">The optional mask, can be null if not needed</param>
      /// <param name="keyPoints">The keypoint where the descriptor will be computed from. Keypoints for which a descriptor cannot be computed are removed.</param>
      /// <returns>The descriptors founded on the keypoint location</returns>
      public Matrix<float> ComputeDescriptorsRaw(Image<Bgr, Byte> image, Image<Gray, Byte> mask, VectorOfKeyPoint keyPoints)
      {
         return ComputeDescriptorsRawHelper(image, mask, keyPoints);
      }*/
   }
}

namespace Emgu.CV
{
   public static partial class CvInvoke
   {
      /*
      [DllImport(CvInvoke.EXTERN_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      private extern static void CvSIFTDetectorDetectFeature(
         IntPtr detector,
         IntPtr image,
         IntPtr mask,
         IntPtr keypoints,
         IntPtr descriptors);

      [DllImport(CvInvoke.EXTERN_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      internal extern static void CvSIFTDetectorComputeDescriptors(
         IntPtr detector,
         IntPtr image,
         IntPtr keypoints,
         IntPtr descriptors);

      [DllImport(CvInvoke.EXTERN_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      internal extern static int CvSIFTDetectorGetDescriptorSize(IntPtr detector);*/

      [DllImport(CvInvoke.EXTERN_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      internal extern static IntPtr CvSIFTDetectorCreate(
         int nFeatures, int nOctaveLayers,
         double contrastThreshold, double edgeThreshold,
         double sigma, ref IntPtr featureDetector, ref IntPtr descriptorExtractor);

      [DllImport(CvInvoke.EXTERN_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      internal extern static void CvSIFTDetectorRelease(ref IntPtr detector);
   }
}