//----------------------------------------------------------------------------
//  Copyright (C) 2004-2013 by EMGU. All rights reserved.       
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
   public class StarDetector : UnmanagedObject, IKeyPointDetector
   {
      #region IKeyPointDetector Members
      /// <summary>
      /// Get the feature detector. 
      /// </summary>
      /// <returns>The feature detector</returns>
      IntPtr IKeyPointDetector.FeatureDetectorPtr
      {
         get
         {
            return _ptr;
         }
      }
      #endregion
      /*
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
      }*/

      private int _maxSize;
      private int _responseThreshold;
      private int _lineThresholdProjected;
      private int _lineThresholdBinarized;
      private int _suppressNonmaxSize;

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
         _lineThresholdProjected = lineThresholdBinarized;
         _lineThresholdBinarized = lineThresholdBinarized;
         _suppressNonmaxSize = suppressNonmaxSize;
         _ptr = CvInvoke.CvStarDetectorCreate(maxSize, responseThreshold, lineThresholdProjected, lineThresholdBinarized, suppressNonmaxSize);
      }

      /// <summary>
      /// Release the unmanaged memory associated with this detector.
      /// </summary>
      protected override void DisposeObject()
      {
         CvInvoke.CvStarDetectorRelease(ref _ptr);
      }
   }
}

namespace Emgu.CV
{
   public static partial class CvInvoke
   {
      [DllImport(CvInvoke.EXTERN_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      internal extern static IntPtr CvStarDetectorCreate(int maxSize, int responseThreshold, int lineThresholdProjected, int lineThresholdBinarized, int suppressNonmaxSize);

      [DllImport(CvInvoke.EXTERN_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      internal extern static void CvStarDetectorRelease(ref IntPtr detector);
   }
}