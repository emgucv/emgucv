//----------------------------------------------------------------------------
//  Copyright (C) 2004-2011 by EMGU. All rights reserved.       
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
   /// Wrapped SIFT detector structure
   /// </summary>
   public class SIFTDetector : UnmanagedObject, IKeyPointDetector, IDescriptorExtractor
   {
      #region PInvoke
      [DllImport(CvInvoke.EXTERN_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      private extern static void CvSIFTDetectorDetectKeyPoints(
         IntPtr detector,
         IntPtr image,
         IntPtr mask,
         IntPtr keypoints);

      /*
      [DllImport(CvInvoke.EXTERN_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      private extern static void CvSIFTDetectorDetectFeature(
         IntPtr detector,
         IntPtr image,
         IntPtr mask,
         IntPtr keypoints,
         IntPtr descriptors);*/

      [DllImport(CvInvoke.EXTERN_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      private extern static int CvSIFTDetectorGetDescriptorSize(IntPtr detector);

      [DllImport(CvInvoke.EXTERN_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      private extern static void CvSIFTDetectorComputeDescriptors(
         IntPtr detector,
         IntPtr image,
         IntPtr mask,
         IntPtr keypoints,
         IntPtr descriptors);

      [DllImport(CvInvoke.EXTERN_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      private extern static IntPtr CvSIFTDetectorCreate(
         int nOctaves, int nOctaveLayers, int firstOctave, AngleMode angleMode, //common parameters
         double threshold, double edgeThreshold, //detector parameters
         double magnification, [MarshalAs(CvInvoke.BoolMarshalType)] bool isNormalize, [MarshalAs(CvInvoke.BoolMarshalType)] bool recalculateAngles); //descriptor parameters

      [DllImport(CvInvoke.EXTERN_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      private extern static void CvSIFTDetectorRelease(ref IntPtr detector);
      #endregion

      /// <summary>
      /// The angle mode for the key point detector
      /// </summary>
      public enum AngleMode
      {
         /// <summary>
         /// First angle
         /// </summary>
         FIRST_ANGLE = 0,
         /// <summary>
         /// Average angle
         /// </summary>
         AVERAGE_ANGLE = 1
      }

      /// <summary>
      /// Create a SIFTDetector using the specific values
      /// </summary>
      /// <param name="nOctaves">The number of octaves. Use 4 for default</param>
      /// <param name="nOctaveLayers">The number of octaves layers. Use 3 for default</param>
      /// <param name="firstOctave">Use -1 for default</param>
      /// <param name="threshold">Detector parameter. Use 0.04 / nOctavesLayers / 2.0 as default</param>
      /// <param name="edgeThreshold">Detector parameter. Use 10.0 as default</param>
      /// <param name="angleMode">Angle mode</param>
      /// <param name="magnification">Descriptor parameter. Use 3.0 as default</param>
      /// <param name="isNormalize">Descriptor parameter. Use true as default</param>
      /// <param name="recalculateAngles">Descriptor parameter. Use true as default</param>
      public SIFTDetector(
         int nOctaves, int nOctaveLayers, int firstOctave, AngleMode angleMode,//common parameters
         double threshold, double edgeThreshold,  //detector parameters
         double magnification, bool isNormalize, bool recalculateAngles) //descriptor parameters
      {
         _ptr = CvSIFTDetectorCreate(nOctaves, nOctaveLayers, firstOctave, angleMode, threshold, edgeThreshold, magnification, isNormalize, recalculateAngles);
      }

      /// <summary>
      /// Create a SIFT detector with the default parameters
      /// </summary>
      public SIFTDetector()
         : this(4, 3, -1, SIFTDetector.AngleMode.AVERAGE_ANGLE, 0.04 / 3 / 2.0, 10.0, 3.0, true, true)
      {
      }

      /// <summary>
      /// Detect the SURF keypoints from the image
      /// </summary>
      /// <param name="image">The image to extract SURF features from</param>
      /// <param name="mask">The optional mask, can be null if not needed</param>
      /// <returns>An array of SURF key points</returns>
      public VectorOfKeyPoint DetectKeyPointsRaw(Image<Gray, Byte> image, Image<Gray, byte> mask)
      {
         VectorOfKeyPoint kpts = new VectorOfKeyPoint();
         CvSIFTDetectorDetectKeyPoints(_ptr, image, mask, kpts);
         return kpts;
      }

      /// <summary>
      /// Detect the SURF keypoints from the image
      /// </summary>
      /// <param name="image">The image to extract SURF features from</param>
      /// <param name="mask">The optional mask, can be null if not needed</param>
      /// <returns>An array of SURF key points</returns>
      public MKeyPoint[] DetectKeyPoints(Image<Gray, Byte> image, Image<Gray, byte> mask)
      {
         using (VectorOfKeyPoint kpts = DetectKeyPointsRaw(image, mask))
         {
            return kpts.ToArray();
         }
      }

      /// <summary>
      /// Detect image features from the given image
      /// </summary>
      /// <param name="image">The image to detect features from</param>
      /// <param name="mask">The optional mask, can be null if not needed</param>
      /// <returns>The Image features detected from the given image</returns>
      public ImageFeature[] DetectFeatures(Image<Gray, Byte> image, Image<Gray, byte> mask)
      {
         using (VectorOfKeyPoint pts = DetectKeyPointsRaw(image, mask))
         using (Matrix<float> descVec = ComputeDescriptorsRaw(image, mask, pts))
         {
            return Features2DTracker.ConvertToImageFeature(pts, descVec);
         }
      }

      /// <summary>
      /// Get the size of the descriptor
      /// </summary>
      public int DescriptorSize
      {
         get
         {
            return CvSIFTDetectorGetDescriptorSize(_ptr);
         }
      }

      /// <summary>
      /// Compute the descriptor given the image and the point location
      /// </summary>
      /// <param name="image">The image where the descriptor will be computed from</param>
      /// <param name="mask">The optional mask, can be null if not needed</param>
      /// <param name="keyPoints">The keypoint where the descriptor will be computed from</param>
      /// <returns>The image features founded on the keypoint location</returns>
      public Matrix<float> ComputeDescriptorsRaw(Image<Gray, Byte> image, Image<Gray, byte> mask, VectorOfKeyPoint keyPoints)
      {
         int count = keyPoints.Size;
         if (count == 0) return null;
         Matrix<float> descriptors = new Matrix<float>(count, DescriptorSize, 1);
         CvSIFTDetectorComputeDescriptors(_ptr, image, mask, keyPoints, descriptors);
         return descriptors;
      }

      /// <summary>
      /// Compute the descriptor given the image and the point location
      /// </summary>
      /// <param name="image">The image where the descriptor will be computed from</param>
      /// <param name="mask">The optional mask, can be null if not needed</param>
      /// <param name="keyPoints">The keypoint where the descriptor will be computed from</param>
      /// <returns>The image features founded on the keypoint location</returns>
      public ImageFeature[] ComputeDescriptors(Image<Gray, Byte> image, Image<Gray, byte> mask, MKeyPoint[] keyPoints)
      {
         if (keyPoints.Length == 0) return new ImageFeature[0];
         using (VectorOfKeyPoint kpts = new VectorOfKeyPoint())
         {
            kpts.Push(keyPoints);
            using (Matrix<float> descriptor = ComputeDescriptorsRaw(image, mask, kpts))
            {
               return Features2DTracker.ConvertToImageFeature(kpts, descriptor);
            }
         }
      }

      /// <summary>
      /// Release the unmanaged resources associated with this object
      /// </summary>
      protected override void DisposeObject()
      {
         CvSIFTDetectorRelease(ref _ptr);
      }

      #region IKeyPointDetector Members
      /// <summary>
      /// Detect the keypoints in the image
      /// </summary>
      /// <param name="image">The image from which the key point will be detected from</param>
      /// <returns>The key pionts in the image</returns>
      public MKeyPoint[] DetectKeyPoints(Image<Gray, byte> image)
      {
         return DetectKeyPoints(image, null);
      }

      #endregion

      #region IDescriptorGenerator Members
      /// <summary>
      /// Compute the ImageFeature on the image from the given keypoint locations.
      /// </summary>
      /// <param name="image">The image to compute descriptors from</param>
      /// <param name="keyPoints">The keypoints where the descriptor computation is perfromed</param>
      /// <returns>The ImageFeature from the given keypoints</returns>
      public ImageFeature[] ComputeDescriptors(Image<Gray, byte> image, MKeyPoint[] keyPoints)
      {
         return ComputeDescriptors(image, null, keyPoints);
      }

      #endregion
   }
}
