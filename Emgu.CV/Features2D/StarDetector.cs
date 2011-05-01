//----------------------------------------------------------------------------
//  Copyright (C) 2004-2011 by EMGU. All rights reserved.       
//----------------------------------------------------------------------------

using System;
using System.Runtime.InteropServices;
using Emgu.CV.Structure;
using Emgu.CV.Util;
using Emgu.Util;

namespace Emgu.CV.Features2D
{
   /// <summary>
   /// StarDetector
   /// </summary>
   public class StarDetector : DisposableObject, IKeyPointDetector
   {
      #region PInvoke
      [DllImport(CvInvoke.EXTERN_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      private extern static IntPtr CvStarGetFeatureDetector(ref MCvStarDetectorParams detector);

      [DllImport(CvInvoke.EXTERN_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      private extern static void CvStarFeatureDetectorRelease(ref IntPtr detector);
      #endregion

      #region IKeyPointDetector Members
      /// <summary>
      /// Get the feature detector. 
      /// </summary>
      /// <returns>The feature detector</returns>
      IntPtr IKeyPointDetector.FeatureDetectorPtr
      {
         get
         {
            return _featureDetectorPtr;
         }
      }
      #endregion

      /// <summary>
      /// Get the Star detector parameters
      /// </summary>
      /// <returns></returns>
      public MCvStarDetectorParams GetStarDetectorParameters()
      {
         MCvStarDetectorParams p = new MCvStarDetectorParams();
         p.MaxSize = MaxSize;
         p.ResponseThreshold = ResponseThreshold;
         p.LineThresholdProjected = LineThresholdProjected;
         p.LineThresholdBinarized = LineThresholdBinarized;
         p.SuppressNonmaxSize = SuppressNonmaxSize;
         return p;
      }

      private int _maxSize;
      private int _responseThreshold;
      private int _lineThresholdProjected;
      private int _lineThresholdBinarized;
      private int _suppressNonmaxSize;

      private IntPtr _featureDetectorPtr;

      /// <summary>
      /// Maximum size of the features. The following
      /// values of the parameter are supported:
      /// 4, 6, 8, 11, 12, 16, 22, 23, 32, 45, 46, 64, 90, 128
      /// </summary>
      public int MaxSize { get { return _maxSize; } }
      /// <summary>
      /// Threshold for the approximated laplacian,
      /// used to eliminate weak features. The larger it is,
      /// the less features will be retrieved
      /// </summary>
      public int ResponseThreshold { get { return _responseThreshold; } }
      /// <summary>
      /// Another threshold for the laplacian to
      /// eliminate edges.
      /// The larger the threshold, the more points you get.
      /// </summary>
      public int LineThresholdProjected { get { return _lineThresholdProjected; } }
      /// <summary>
      /// Another threshold for the feature
      /// size to eliminate edges. 
      /// The larger the threshold, the more points you get.
      /// </summary>
      public int LineThresholdBinarized { get { return _lineThresholdBinarized; } }
      /// <summary>
      /// 
      /// </summary>
      public int SuppressNonmaxSize { get { return _suppressNonmaxSize; } }

      /// <summary>
      /// Construct default star detector
      /// </summary>
      public StarDetector()
         : this(45, 30, 10, 8, 5)
      {
      }

      /// <summary>
      /// Create a star detector with the specific parameters
      /// </summary>
      /// <param name="maxSize">
      /// Use 45 as default. Maximum size of the features. The following
      /// values of the parameter are supported:
      /// 4, 6, 8, 11, 12, 16, 22, 23, 32, 45, 46, 64, 90, 128</param>
      /// <param name="responseThreshold">
      /// Use 30 as default. Threshold for the approximated laplacian,
      /// used to eliminate weak features. The larger it is,
      /// the less features will be retrieved
      /// </param>
      /// <param name="lineThresholdProjected">
      /// Use 10 as default. Another threshold for the laplacian to eliminate edges.
      /// The larger the threshold, the more points you get.
      /// </param>
      /// <param name="lineThresholdBinarized">
      /// Use 8 as default. Another threshold for the feature size to eliminate edges. 
      /// The larger the threshold, the more points you get.</param>
      /// <param name="suppressNonmaxSize">
      /// Use 5 as default.
      /// </param>
      public StarDetector(int maxSize, int responseThreshold, int lineThresholdProjected, int lineThresholdBinarized, int suppressNonmaxSize)
      {
         _maxSize = maxSize;
         _responseThreshold = responseThreshold;
         _lineThresholdProjected = lineThresholdProjected;
         _lineThresholdBinarized = lineThresholdBinarized;
         _suppressNonmaxSize = suppressNonmaxSize;

         MCvStarDetectorParams p = GetStarDetectorParameters();
         _featureDetectorPtr = CvStarGetFeatureDetector(ref p);
      }

      /// <summary>
      /// Detect the keypoints in the image
      /// </summary>
      /// <param name="image">The image from which the key point will be detected from</param>
      /// <param name="mask">The optional mask, can be null if not needed</param>
      /// <returns>The key pionts in the image</returns>
      public VectorOfKeyPoint DetectKeyPointsRaw(Image<Gray, Byte> image, Image<Gray, Byte> mask)
      {
         VectorOfKeyPoint kpts = new VectorOfKeyPoint();
         CvInvoke.CvFeatureDetectorDetectKeyPoints(_featureDetectorPtr, image, mask, kpts);
         return kpts;
      }

      /// <summary>
      /// Detect the keypoints in the image
      /// </summary>
      /// <param name="image">The image from which the key point will be detected from</param>
      /// <returns>The key pionts in the image</returns>
      public MKeyPoint[] DetectKeyPoints(Image<Gray, Byte> image)
      {
         using (VectorOfKeyPoint kpts = DetectKeyPointsRaw(image, null))
         {
            return kpts.ToArray();
         }
      }

      /// <summary>
      /// Release the unmanaged memory associated with this detector.
      /// </summary>
      protected override void DisposeObject()
      {
         CvStarFeatureDetectorRelease(ref _featureDetectorPtr);
      }
   }
}
