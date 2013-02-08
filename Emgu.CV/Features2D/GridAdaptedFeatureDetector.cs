//----------------------------------------------------------------------------
//  Copyright (C) 2004-2013 by EMGU. All rights reserved.       
//----------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Text;
using Emgu.CV.Structure;
using Emgu.CV.Features2D;
using Emgu.CV.Util;
using System.Runtime.InteropServices;
using Emgu.Util;

namespace Emgu.CV.Features2D
{
   /// <summary>
   /// Adapts a detector to partition the source image into a grid and detect points in each cell.
   /// </summary>
   public class GridAdaptedFeatureDetector : UnmanagedObject, IKeyPointDetector
   {
      private IKeyPointDetector _baseDetector;

      /// <summary>
      /// Maximum count of keypoints detected on the image. Only the strongest keypoints
      /// </summary>
      public int MaxTotalKeyPoints;

      /// <summary>
      /// Grid rows count
      /// </summary>
      public int GridRows;

      /// <summary>
      /// Grid column count
      /// </summary>
      public int GridCols;

      /// <summary>
      /// Adapts a detector to partition the source image into a grid and detect points in each cell.
      /// </summary>
      /// <param name="detector">Detector that will be adapted</param>
      /// <param name="maxTotalKeyPoints">Maximum count of keypoints detected on the image. Only the strongest keypoints</param>
      /// <param name="gridRows">Grid rows count</param>
      /// <param name="gridCols">Grid column count</param>
      public GridAdaptedFeatureDetector(IKeyPointDetector detector, int maxTotalKeyPoints, int gridRows, int gridCols)
      {
         _baseDetector = detector;
         MaxTotalKeyPoints = maxTotalKeyPoints;
         GridRows = gridRows;
         GridCols = gridCols;
         _ptr = CvInvoke.GridAdaptedFeatureDetectorCreate(detector.FeatureDetectorPtr, maxTotalKeyPoints, gridRows, gridCols);
      }

      /// <summary>
      /// Release the unmanaged memory associated with this detector.
      /// </summary>
      protected override void DisposeObject()
      {
         CvInvoke.GridAdaptedFeatureDetectorRelease(ref _ptr);
      }

      #region IKeyPointDetector Members
      IntPtr IKeyPointDetector.FeatureDetectorPtr
      {
         get
         {
            return _ptr;
         }
      }
      #endregion
   }
}

namespace Emgu.CV
{
   public static partial class CvInvoke
   {
      [DllImport(CvInvoke.EXTERN_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      internal extern static IntPtr GridAdaptedFeatureDetectorCreate(
         IntPtr detector,
         int maxTotalKeypoints, int gridRows, int gridCols);

      /*
      [DllImport(CvInvoke.EXTERN_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      private extern static void GridAdaptedFeatureDetectorDetect(
         IntPtr detector,
         IntPtr image, IntPtr keypoints, IntPtr mask);
      */

      [DllImport(CvInvoke.EXTERN_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      internal extern static void GridAdaptedFeatureDetectorRelease(ref IntPtr detector);
   }
}