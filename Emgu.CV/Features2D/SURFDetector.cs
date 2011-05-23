//----------------------------------------------------------------------------
//  Copyright (C) 2004-2011 by EMGU. All rights reserved.       
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
   public class SURFDetector : DisposableObject, IKeyPointDetector, IDescriptorExtractor
   {
      #region PInvoke

      [DllImport(CvInvoke.EXTERN_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      private extern static IntPtr CvSURFGetFeatureDetector(ref MCvSURFParams detector);

      [DllImport(CvInvoke.EXTERN_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      private extern static IntPtr CvSURFGetDescriptorExtractor(ref MCvSURFParams detector);

      /*
      [DllImport(CvInvoke.EXTERN_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      private extern static void CvSURFDetectorDetectFeature(
         ref SURFDetector detector,
         IntPtr image,
         IntPtr mask,
         IntPtr keypoints,
         IntPtr descriptors);*/

      [DllImport(CvInvoke.EXTERN_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      private extern static void CvSURFDetectorComputeDescriptors(
         ref MCvSURFParams detector,
         IntPtr image,
         IntPtr mask,
         IntPtr keypoints,
         IntPtr descriptors);

      [DllImport(CvInvoke.EXTERN_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      private extern static void CvSURFDetectorComputeDescriptorsBGR(
         ref MCvSURFParams detector,
         IntPtr image,
         IntPtr keypoints,
         IntPtr descriptors);

      [DllImport(CvInvoke.EXTERN_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      private extern static void CvSURFFeatureDetectorRelease(ref IntPtr detector);

      [DllImport(CvInvoke.EXTERN_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      private extern static void CvSURFDescriptorExtractorRelease(ref IntPtr extractor);
      #endregion

      /// <summary>
      /// Get the SURF parameters
      /// </summary>
      public MCvSURFParams SURFParams
      {
         get
         {
            MCvSURFParams p = new MCvSURFParams();
            p.Extended = _extended;
            p.HessianThreshold = HessianThreshold;
            p.NOctaves = NOctaves;
            p.NOctaveLayers = NOctaveLayers;
            return p;
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
         : this(hessianThresh, extendedFlag, 3, 4)
      {
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
      {
         _extended = extendedFlag ? 1 : 0;
         _hessianThreshold = hessianThresh;
         _nOctaves = nOctaves;
         _nOctaveLayers = nOctaveLayers;

         MCvSURFParams tmp = SURFParams;
         _featureDetectorPtr = CvSURFGetFeatureDetector(ref tmp);
         _featureDetectorPtr = CvSURFGetDescriptorExtractor(ref tmp);
      }

      private int _extended;
      private double _hessianThreshold;
      private int _nOctaves;
      private int _nOctaveLayers;

      /// <summary>
      /// true means basic descriptors (64 elements each),
      /// false means extended descriptors (128 elements each)
      /// </summary>
      public bool Extended { get { return _extended == 1; } }

      /// <summary>
      /// Only features with keypoint.hessian larger than that are extracted.
      /// good default value is ~300-500 (can depend on the average local contrast and sharpness of the image).
      /// user can further filter out some features based on their hessian values and other characteristics
      /// </summary>
      public double HessianThreshold { get { return _hessianThreshold; } }

      /// <summary>
      /// The number of octaves to be used for extraction.
      /// With each next octave the feature size is doubled (3 by default)
      /// </summary>
      public int NOctaves { get { return _nOctaves; } }

      /// <summary>
      /// The number of layers within each octave (4 by default)
      /// </summary>
      public int NOctaveLayers { get { return _nOctaveLayers; } }

      private IntPtr _featureDetectorPtr;
      private IntPtr _descriptorExtractorPtr;

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
      public ImageFeature[] DetectFeatures(Image<Gray, Byte> image, Image<Gray, byte> mask)
      {
         using (VectorOfKeyPoint kpts = DetectKeyPointsRaw(image, mask))
         using (Matrix<float> desc = ComputeDescriptorsRaw(image, mask, kpts))
         {
            return Features2DTracker.ConvertToImageFeature(kpts, desc);
         }
      }

      /// <summary>
      /// Compute the descriptor given the bgr image and the point location, using oppponent color (CGIV 2008 "Color Descriptors for Object Category Recognition").
      /// </summary>
      /// <param name="image">The image where the descriptor will be computed from</param>
      /// <param name="keyPoints">The keypoint where the descriptor will be computed from. Keypoints for which a descriptor cannot be computed are removed.</param>
      /// <returns>The descriptors founded on the keypoint location.</returns>
      public Matrix<float> ComputeDescriptorsRaw(Image<Bgr, Byte> image, VectorOfKeyPoint keyPoints)
      {
         int count = keyPoints.Size;
         if (count == 0) return null;
         int sizeOfdescriptor = Extended ? 128 : 64;
         Matrix<float> descriptors = new Matrix<float>(keyPoints.Size, sizeOfdescriptor * 3, 1);
         MCvSURFParams p = SURFParams;
         CvSURFDetectorComputeDescriptorsBGR(ref p, image, keyPoints, descriptors);
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
         int sizeOfdescriptor = Extended ? 128 : 64;
         using (VectorOfKeyPoint pts = new VectorOfKeyPoint())
         {
            pts.Push(keyPoints);
            using (Matrix<float> descriptors = ComputeDescriptorsRaw(image, mask, pts))
               return Features2DTracker.ConvertToImageFeature(pts, descriptors);
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
         CvInvoke.CvFeatureDetectorDetectKeyPoints(_featureDetectorPtr, image, mask, kpts);
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
            return _featureDetectorPtr;
         }
      }
      #endregion

      /// <summary>
      /// Release the unmanaged memory associated with this detector.
      /// </summary>
      protected override void DisposeObject()
      {
         CvSURFFeatureDetectorRelease(ref _featureDetectorPtr);
         CvSURFDescriptorExtractorRelease(ref _descriptorExtractorPtr);
      }

      #region IDescriptorExtractor Members
      /// <summary>
      /// Compute the descriptor given the image and the point location
      /// </summary>
      /// <param name="image">The image where the descriptor will be computed from</param>
      /// <param name="mask">The optional mask, can be null if not needed</param>
      /// <param name="keyPoints">The keypoint where the descriptor will be computed from. Keypoints for which a descriptor cannot be computed are removed.</param>
      /// <returns>The descriptors founded on the keypoint location</returns>
      public Matrix<float> ComputeDescriptorsRaw(Image<Gray, Byte> image, Image<Gray, byte> mask, VectorOfKeyPoint keyPoints)
      {
         int count = keyPoints.Size;
         if (count == 0) return null;
         int sizeOfdescriptor = Extended ? 128 : 64;
         Matrix<float> descriptors = new Matrix<float>(keyPoints.Size, sizeOfdescriptor, 1);
         MCvSURFParams p = SURFParams;
         CvSURFDetectorComputeDescriptors(ref p, image, mask, keyPoints, descriptors);
         return descriptors;
      }

      IntPtr IDescriptorExtractor.DescriptorExtratorPtr
      {
         get { return _descriptorExtractorPtr; }
      }

      #endregion
   }
}
