using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;
using System.Diagnostics;
using Emgu.CV.Structure;

namespace Emgu.CV
{
   /// <summary>
   /// A homography matrix
   /// </summary>
   [Serializable]
   public class HomographyMatrix : Matrix<double> 
   {
      /// <summary>
      /// Constructor used to deserialize 2D rotation matrix
      /// </summary>
      /// <param name="info">The serialization info</param>
      /// <param name="context">The streaming context</param>
      public HomographyMatrix(SerializationInfo info, StreamingContext context)
         : base(info, context)
      {
      }

      /// <summary>
      /// Create an empty homography matrix
      /// </summary>
      public HomographyMatrix()
         : base(3, 3)
      {
      }

      /// <summary>
      /// Check if the homography matrix is valid.
      /// </summary>
      /// <param name="thresholdForDeterminant">A number &gt; 1. A good number will be 10, if if the deteminate of the homography matrix is in the range of [1/threshold, threshold], true is returned</param>
      /// <returns>True, if the deteminate of the homography matrix is in the range of [1/threshold, threshold]</returns>
      public bool IsValid(double thresholdForDeterminant)
      {
         if (thresholdForDeterminant == 0) return true;

         if (thresholdForDeterminant < 1.0) thresholdForDeterminant = 1.0 / thresholdForDeterminant;

         double det = CvInvoke.cvDet(Ptr);
         if (det > thresholdForDeterminant | det < (1.0 / thresholdForDeterminant))
         {
            return false;
         }
         return true;
      }

      /// <summary>
      /// Get the homography projection of the points. The projected value will be saved to the input point array
      /// </summary>
      /// <param name="points">The points to apply homography transform</param>
      public void ProjectPoints(PointF[] points) 
      {
         GCHandle handle = GCHandle.Alloc(points, GCHandleType.Pinned);
         using (Matrix<double> homogeneousCoordinates = new Matrix<double>(points.Length, 3))
         using (Matrix<double> firstTwoColums = homogeneousCoordinates.GetCols(0, 2))
         using (Matrix<double> reshape = firstTwoColums.Reshape(2, points.Length))
         using (Matrix<double> destCornerCoordinate = new Matrix<double>(points.Length, 3))
         using (Matrix<float> pointMat = new Matrix<float>(points.Length, 1, 2, handle.AddrOfPinnedObject(), 0))
         {
            homogeneousCoordinates.SetValue(1);
            CvInvoke.cvConvert(pointMat, reshape);
            CvInvoke.cvGEMM(homogeneousCoordinates, Ptr, 1.0, IntPtr.Zero, 0.0, destCornerCoordinate, Emgu.CV.CvEnum.GEMM_TYPE.CV_GEMM_B_T);
            CvInvoke.cvConvertPointsHomogeneous(destCornerCoordinate, pointMat);
         }
         handle.Free();
      }
   }
}
