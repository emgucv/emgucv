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
      #region PInvoke
      [DllImport(CvInvoke.EXTERN_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      private extern static IntPtr GridAdaptedFeatureDetectorCreate(
         IntPtr detector,
         int maxTotalKeypoints, int gridRows, int gridCols);

      /*
      [DllImport(CvInvoke.EXTERN_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      private extern static void GridAdaptedFeatureDetectorDetect(
         IntPtr detector,
         IntPtr image, IntPtr keypoints, IntPtr mask);
      */

      [DllImport(CvInvoke.EXTERN_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      private extern static void GridAdaptedFeatureDetectorRelease(ref IntPtr detector);
      #endregion

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
         GridAdaptedFeatureDetectorRelease(ref _ptr);
      }

      #region IKeyPointDetector Members
      IntPtr IKeyPointDetector.FeatureDetectorPtr
      {
         get
         {
            return  _ptr;
         }
      }

      /// <summary>
      /// Detect the keypoints from the image
      /// </summary>
      /// <param name="image">The image to extract features from</param>
      /// <param name="mask">The optional mask, can be null if not needed</param>
      /// <returns>An array of key points</returns>
      public VectorOfKeyPoint DetectKeyPointsRaw(Image<Gray, byte> image, Image<Gray, byte> mask)
      {
         VectorOfKeyPoint kpts = new VectorOfKeyPoint();
         CvInvoke.CvFeatureDetectorDetectKeyPoints(_ptr, image, mask, kpts);
         return kpts;
      }

      #endregion
   }
}
