//----------------------------------------------------------------------------
//  Copyright (C) 2004-2012 by EMGU. All rights reserved.       
//----------------------------------------------------------------------------

using System;
using System.Runtime.InteropServices;
using Emgu.CV.Features2D;
using Emgu.CV.Structure;
using Emgu.CV.Util;
using Emgu.Util;

namespace Emgu.CV.Features2D
{
   /// <summary>
   /// Wrapped CvSURFParams structure
   /// </summary>
   public class SURFDetector : UnmanagedObject, IKeyPointDetector, IDescriptorExtractor<float>
   {
      private MCvSURFParams _surfParams;

      /// <summary>
      /// Get the SURF parameters
      /// </summary>
      public MCvSURFParams SURFParams
      {
         get
         {
            return _surfParams;
         }
      }

      /// <summary>
      /// Create a MCvSURFParams using the specific values
      /// </summary>
      /// <param name="hessianThresh">      
      /// Only features with keypoint.hessian larger than that are extracted.
      /// good default value is ~300-500 (can depend on the average local contrast and sharpness of the image).
      /// user can further filter out some features based on their hessian values and other characteristics
      /// </param>
      /// <param name="extendedFlag">      
      /// false means basic descriptors (64 elements each),
      /// true means extended descriptors (128 elements each)
      /// </param>
      public SURFDetector(double hessianThresh, bool extendedFlag)
         : this(new MCvSURFParams(hessianThresh, extendedFlag))
      {
      }

      /// <summary>
      /// Create a SURF detector with the specific surfParameters
      /// </summary>
      /// <param name="surfParams">The surf parameters</param>
      public SURFDetector(MCvSURFParams surfParams)
      {
         _surfParams = surfParams;
         _ptr = CvInvoke.CvSURFDetectorCreate(ref surfParams);
      }

      /// <summary>
      /// Create a MCvSURFParams using the specific values
      /// </summary>
      /// <param name="hessianThresh">      
      /// Only features with keypoint.hessian larger than that are extracted.
      /// good default value is ~300-500 (can depend on the average local contrast and sharpness of the image).
      /// user can further filter out some features based on their hessian values and other characteristics
      /// </param>
      /// <param name="extendedFlag">      
      /// false means basic descriptors (64 elements each),
      /// true means extended descriptors (128 elements each)
      /// </param>
      /// <param name="nOctaves">
      /// The number of octaves to be used for extraction.
      /// With each next octave the feature size is doubled (3 by default)
      /// </param>
      /// <param name="nOctaveLayers">
      /// The number of layers within each octave (4 by default)
      /// </param>
      public SURFDetector(double hessianThresh, bool extendedFlag, int nOctaves, int nOctaveLayers)
         : this(new MCvSURFParams(hessianThresh, extendedFlag, nOctaves, nOctaveLayers))
      {
      }

      /// <summary>
      /// Detect the SURF keypoints from the image
      /// </summary>
      /// <param name="image">The image to extract SURF features from</param>
      /// <param name="mask">The optional mask, can be null if not needed</param>
      /// <returns>An array of SURF key points</returns>
      public MKeyPoint[] DetectKeyPoints(Image<Gray, Byte> image, Image<Gray, byte> mask)
      {
         using (VectorOfKeyPoint keypoints = DetectKeyPointsRaw(image, mask))
         {
            return keypoints.ToArray();
         }
      }

      /// <summary>
      /// Detect image features from the given image
      /// </summary>
      /// <param name="image">The image to detect features from</param>
      /// <param name="mask">The optional mask, can be null if not needed</param>
      /// <returns>The Image features detected from the given image</returns>
      public ImageFeature<float>[] DetectFeatures(Image<Gray, Byte> image, Image<Gray, byte> mask)
      {
         using (VectorOfKeyPoint kpts = DetectKeyPointsRaw(image, mask))
         using (Matrix<float> desc = ComputeDescriptorsRaw(image, mask, kpts))
         {
            return ImageFeature<float>.ConvertFromRaw(kpts, desc);
         }
      }

      /// <summary>
      /// Compute the descriptor given the bgr image and the point location, using oppponent color (CGIV 2008 "Color Descriptors for Object Category Recognition").
      /// </summary>
      /// <param name="image">The image where the descriptor will be computed from</param>
      /// <param name="mask">The optional mask, can be null if not needed</param>
      /// <param name="keyPoints">The keypoint where the descriptor will be computed from. Keypoints for which a descriptor cannot be computed are removed.</param>
      /// <returns>The descriptors founded on the keypoint location.</returns>
      private Matrix<float> ComputeDescriptorsRawHelper(CvArray<Byte> image, Image<Gray, byte> mask, VectorOfKeyPoint keyPoints)
      {
         if (mask != null)
            keyPoints.FilterByPixelsMask(mask);
         int count = keyPoints.Size;
         if (count == 0) return null;
         int sizeOfdescriptor = _surfParams.Extended ? 128 : 64;
         Matrix<float> descriptors = new Matrix<float>(keyPoints.Size, sizeOfdescriptor * image.NumberOfChannels, 1);
         CvInvoke.CvSURFDetectorComputeDescriptors(_ptr, image, keyPoints, descriptors);
         return descriptors;
      }

      /// <summary>
      /// Compute the descriptor given the bgr image and the point location, using oppponent color (CGIV 2008 "Color Descriptors for Object Category Recognition").
      /// </summary>
      /// <param name="image">The image where the descriptor will be computed from</param>
      /// <param name="mask">The optional mask, can be null if not needed</param>
      /// <param name="keyPoints">The keypoint where the descriptor will be computed from. Keypoints for which a descriptor cannot be computed are removed.</param>
      /// <returns>The descriptors founded on the keypoint location.</returns>
      public Matrix<float> ComputeDescriptorsRaw(Image<Bgr, Byte> image, Image<Gray, byte> mask, VectorOfKeyPoint keyPoints)
      {
         return ComputeDescriptorsRawHelper(image, mask, keyPoints);
      }

      /// <summary>
      /// Compute the descriptor given the image and the point location
      /// </summary>
      /// <param name="image">The image where the descriptor will be computed from</param>
      /// <param name="mask">The optional mask, can be null if not needed</param>
      /// <param name="keyPoints">The keypoint where the descriptor will be computed from</param>
      /// <returns>The image features founded on the keypoint location</returns>
      public ImageFeature<float>[] ComputeDescriptors(Image<Gray, Byte> image, Image<Gray, byte> mask, MKeyPoint[] keyPoints)
      {
         int sizeOfdescriptor = _surfParams.Extended ? 128 : 64;
         using (VectorOfKeyPoint pts = new VectorOfKeyPoint())
         {
            pts.Push(keyPoints);
            using (Matrix<float> descriptors = ComputeDescriptorsRaw(image, mask, pts))
               return ImageFeature<float>.ConvertFromRaw(pts, descriptors);
         }
      }

      #region IKeyPointDetector Members
      /// <summary>
      /// Detect the SURF keypoints from the image
      /// </summary>
      /// <param name="image">The image to extract SURF features from</param>
      /// <param name="mask">The optional mask, can be null if not needed</param>
      /// <returns>An array of SURF key points</returns>
      public VectorOfKeyPoint DetectKeyPointsRaw(Image<Gray, Byte> image, Image<Gray, byte> mask)
      {
         VectorOfKeyPoint kpts = new VectorOfKeyPoint();
         CvInvoke.CvFeatureDetectorDetectKeyPoints(_ptr, image, mask, kpts);
         return kpts;
      }

      /// <summary>
      /// Get the feature detector. 
      /// </summary>
      /// <returns>The feature detector</returns>
      public IntPtr FeatureDetectorPtr
      {
         get
         {
            return _ptr;
         }
      }
      #endregion

      /// <summary>
      /// Release the unmanaged memory associated with this detector.
      /// </summary>
      protected override void DisposeObject()
      {
         CvInvoke.CvSURFDetectorRelease(ref _ptr);
      }

      #region IDescriptorExtractor<float> Members
      /// <summary>
      /// Compute the descriptor given the image and the point location
      /// </summary>
      /// <param name="image">The image where the descriptor will be computed from</param>
      /// <param name="mask">The optional mask, can be null if not needed</param>
      /// <param name="keyPoints">The keypoint where the descriptor will be computed from. Keypoints for which a descriptor cannot be computed are removed.</param>
      /// <returns>The descriptors founded on the keypoint location</returns>
      public Matrix<float> ComputeDescriptorsRaw(Image<Gray, Byte> image, Image<Gray, byte> mask, VectorOfKeyPoint keyPoints)
      {
         return ComputeDescriptorsRawHelper(image, mask, keyPoints);
      }

      IntPtr IDescriptorExtractor<float>.DescriptorExtratorPtr
      {
         get { return _ptr; }
      }

      #endregion
   }
}

namespace Emgu.CV
{
   public static partial class CvInvoke
   {
      [DllImport(CvInvoke.EXTERN_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      internal extern static IntPtr CvSURFDetectorCreate(ref MCvSURFParams detector);

      /*
      [DllImport(CvInvoke.EXTERN_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      private extern static void CvSURFDetectorDetectFeature(
         ref SURFDetector detector,
         IntPtr image,
         IntPtr mask,
         IntPtr keypoints,
         IntPtr descriptors);*/

      [DllImport(CvInvoke.EXTERN_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      internal extern static void CvSURFDetectorComputeDescriptors(
         IntPtr detector,
         IntPtr image,
         IntPtr keypoints,
         IntPtr descriptors);

      [DllImport(CvInvoke.EXTERN_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      internal extern static void CvSURFDetectorRelease(ref IntPtr detector);
   }
}