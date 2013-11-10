//----------------------------------------------------------------------------
//  Copyright (C) 2004-2013 by EMGU. All rights reserved.       
//----------------------------------------------------------------------------

using System;
using System.Runtime.InteropServices;
using Emgu.CV.Features2D;
using Emgu.CV.Structure;
using Emgu.CV.Util;
using Emgu.Util;

namespace Emgu.CV.Nonfree
{
   /// <summary>
   /// Wrapped CvSURFParams structure
   /// </summary>
   public class SURFDetector : Feature2D<float>
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
         _ptr = NonfreeInvoke.CvSURFDetectorCreate(ref surfParams, ref _featureDetectorPtr, ref _descriptorExtractorPtr);
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
      /// Release the unmanaged memory associated with this detector.
      /// </summary>
      protected override void DisposeObject()
      {
         NonfreeInvoke.CvSURFDetectorRelease(ref _ptr);
         base.DisposeObject();
      }
   }

   /// <summary>
   /// This class wraps the functional calls to the opencv_nonfree module
   /// </summary>
   public static partial class NonfreeInvoke
   {
      [DllImport(CvInvoke.EXTERN_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      internal extern static IntPtr CvSURFDetectorCreate(ref MCvSURFParams detector, ref IntPtr featureDetector, ref IntPtr descriptorExtractor);

      [DllImport(CvInvoke.EXTERN_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      internal extern static void CvSURFDetectorRelease(ref IntPtr detector);

      #region Image extensions
      /// <summary>
      /// Finds robust features in the image (basic descriptor is returned in this case). For each feature it returns its location, size, orientation and optionally the descriptor, basic or extended. The function can be used for object tracking and localization, image stitching etc
      /// </summary>
      /// <param name="image">The image where SURF features will be extracted from</param>
      /// <param name="param">The SURF parameters</param>
      /// <returns>The SURF features</returns>
      public static SURFFeature[] ExtractSURF(this Image<Gray, Byte> image, ref MCvSURFParams param)
      {
         return ExtractSURF(image, null, ref param);
      }

      /// <summary>
      /// Finds robust features in the image (basic descriptor is returned in this case). For each feature it returns its location, size, orientation and optionally the descriptor, basic or extended. The function can be used for object tracking and localization, image stitching etc
      /// </summary>
      /// <param name="image">The image where SURF features will be extracted from</param>
      /// <param name="mask">The optional input 8-bit mask, can be null if not needed. The features are only found in the areas that contain more than 50% of non-zero mask pixels</param>
      /// <param name="param">The SURF parameters</param>
      /// <returns>The SURF features</returns>
      public static SURFFeature[] ExtractSURF(this Image<Gray, Byte> image, Image<Gray, Byte> mask, ref MCvSURFParams param)
      {
         using (MemStorage stor = new MemStorage())
         {
            IntPtr descriptorPtr = new IntPtr();
            IntPtr keypointsPtr = new IntPtr();

            CvInvoke.cvExtractSURF(
           image.Ptr, mask == null ? IntPtr.Zero : mask.Ptr,
           ref keypointsPtr,
           ref descriptorPtr,
           stor.Ptr,
           param,
           0);
            Seq<MCvSURFPoint> keypoints = new Seq<MCvSURFPoint>(keypointsPtr, stor);

            MCvSURFPoint[] surfPoints = keypoints.ToArray();

            SURFFeature[] res = new SURFFeature[surfPoints.Length];

            int elementsInDescriptor = (param.Extended == 0) ? 64 : 128;

            for (int i = 0; i < res.Length; i++)
            {
               float[] descriptor = new float[elementsInDescriptor];
               Marshal.Copy(CvInvoke.cvGetSeqElem(descriptorPtr, i), descriptor, 0, elementsInDescriptor);
               res[i] = new SURFFeature(ref surfPoints[i], descriptor);
            }

            return res;
         }
      }

      #endregion
   }
}