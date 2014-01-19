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
   public class StarDetector : UnmanagedObject, IFeatureDetector
   {
      #region IFeatureDetector Members
      /// <summary>
      /// Get the feature detector. 
      /// </summary>
      /// <returns>The feature detector</returns>
      IntPtr IFeatureDetector.FeatureDetectorPtr
      {
         get
         {
            return _ptr;
         }
      }
      #endregion

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
      /// Create a star detector with the specific parameters
      /// </summary>
      /// <param name="maxSize">
      /// Maximum size of the features. The following
      /// values of the parameter are supported:
      /// 4, 6, 8, 11, 12, 16, 22, 23, 32, 45, 46, 64, 90, 128</param>
      /// <param name="responseThreshold">
      /// Threshold for the approximated laplacian,
      /// used to eliminate weak features. The larger it is,
      /// the less features will be retrieved
      /// </param>
      /// <param name="lineThresholdProjected">
      /// Another threshold for the laplacian to eliminate edges.
      /// The larger the threshold, the more points you get.
      /// </param>
      /// <param name="lineThresholdBinarized">
      /// Another threshold for the feature size to eliminate edges. 
      /// The larger the threshold, the more points you get.</param>
      /// <param name="suppressNonmaxSize">
      ///
      /// </param>
      public StarDetector(int maxSize = 45, int responseThreshold = 30, int lineThresholdProjected = 10, int lineThresholdBinarized = 8, int suppressNonmaxSize = 5)
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