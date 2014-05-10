//----------------------------------------------------------------------------
//  Copyright (C) 2004-2014 by EMGU Corporation. All rights reserved.       
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
   /// Class for extracting Speeded Up Robust Features from an image
   /// </summary>
   public class SURFDetector : Feature2D
   {

      /// <summary>
      /// Create a SURF detector using the specific values
      /// </summary>
      /// <param name="hessianThresh">      
      /// Only features with keypoint.hessian larger than that are extracted.
      /// good default value is ~300-500 (can depend on the average local contrast and sharpness of the image).
      /// user can further filter out some features based on their hessian values and other characteristics
      /// </param>
      /// <param name="extended">      
      /// false means basic descriptors (64 elements each),
      /// true means extended descriptors (128 elements each)
      /// </param>
      /// <param name="nOctaves">
      /// The number of octaves to be used for extraction.
      /// With each next octave the feature size is doubled
      /// </param>
      /// <param name="nOctaveLayers">
      /// The number of layers within each octave
      /// </param>
      /// <param name="upright">
      /// False means that detector computes orientation of each feature. 
      /// True means that the orientation is not computed (which is much, much faster). 
      /// For example, if you match images from a stereo pair, or do image stitching, the matched features likely have very similar angles, and you can speed up feature extraction by setting upright=true.</param>
      public SURFDetector(double hessianThresh, int nOctaves = 4, int nOctaveLayers = 2, bool extended = true, bool upright = false)
      {
         _ptr = NonfreeInvoke.CvSURFDetectorCreate(hessianThresh, nOctaves, nOctaveLayers, extended, upright, ref _featureDetectorPtr, ref _descriptorExtractorPtr);
      }

      /// <summary>
      /// Release the unmanaged memory associated with this detector.
      /// </summary>
      protected override void DisposeObject()
      {
         if (_ptr != IntPtr.Zero)
            NonfreeInvoke.CvSURFDetectorRelease(ref _ptr);
         base.DisposeObject();
      }
   }

   /// <summary>
   /// This class wraps the functional calls to the opencv_nonfree module
   /// </summary>
   public static partial class NonfreeInvoke
   {
      static NonfreeInvoke()
      {
         CvInvoke.CheckLibraryLoaded();
      }

      [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      internal extern static IntPtr CvSURFDetectorCreate(
         double hessianThresh, int nOctaves, int nOctaveLayers, 
         [MarshalAs(CvInvoke.BoolMarshalType)]
         bool extended, 
         [MarshalAs(CvInvoke.BoolMarshalType)]
         bool upright, 
         ref IntPtr featureDetector, ref IntPtr descriptorExtractor);

      [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      internal extern static void CvSURFDetectorRelease(ref IntPtr detector);

      #region Image extensions
      /*
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
      }*/

      #endregion
   }
}