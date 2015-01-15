/*
//----------------------------------------------------------------------------
//  Copyright (C) 2004-2015 by EMGU Corporation. All rights reserved.       
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
   public class GridAdaptedFeatureDetector : UnmanagedObject, IFeatureDetector
   {
      static GridAdaptedFeatureDetector()
      {
         CvInvoke.CheckLibraryLoaded();
      }

      private IFeatureDetector _baseDetector;

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
      public GridAdaptedFeatureDetector(IFeatureDetector detector, int maxTotalKeyPoints, int gridRows, int gridCols)
      {
         _baseDetector = detector;
         MaxTotalKeyPoints = maxTotalKeyPoints;
         GridRows = gridRows;
         GridCols = gridCols;
         _ptr = GridAdaptedFeatureDetectorCreate(detector.FeatureDetectorPtr, maxTotalKeyPoints, gridRows, gridCols);
      }

      /// <summary>
      /// Release the unmanaged memory associated with this detector.
      /// </summary>
      protected override void DisposeObject()
      {
         if (_ptr != IntPtr.Zero)
            GridAdaptedFeatureDetectorRelease(ref _ptr);
      }

      #region IFeatureDetector Members
      IntPtr IFeatureDetector.FeatureDetectorPtr
      {
         get
         {
            return _ptr;
         }
      }
      #endregion

      IntPtr IAlgorithm.AlgorithmPtr
      {
         get
         {
            return CvInvoke.cveAlgorithmFromFeatureDetector(((IFeatureDetector)this).FeatureDetectorPtr);
         }
      }

      [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      internal extern static IntPtr GridAdaptedFeatureDetectorCreate(
         IntPtr detector,
         int maxTotalKeypoints, int gridRows, int gridCols);

      [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      internal extern static void GridAdaptedFeatureDetectorRelease(ref IntPtr detector);

   }
}
*/