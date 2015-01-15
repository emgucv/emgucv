/*
//----------------------------------------------------------------------------
//  Copyright (C) 2004-2015 by EMGU Corporation. All rights reserved.       
//----------------------------------------------------------------------------

using System;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;

namespace Emgu.CV
{
   /// <summary>
   /// A 3x3 homography matrix. This matrix defines an perspective transform
   /// </summary>
#if !NETFX_CORE
   [Serializable]
#endif
   public class HomographyMatrix : Matrix<double> 
   {

#if !NETFX_CORE
      /// <summary>
      /// Constructor used to deserialize homography matrix
      /// </summary>
      /// <param name="info">The serialization info</param>
      /// <param name="context">The streaming context</param>
      public HomographyMatrix(SerializationInfo info, StreamingContext context)
         : base(info, context)
      {
      }
#endif

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
      /// <param name="thresholdForDeterminant">A number &gt; 1. A good number will be 10. If the deteminate of the homography matrix is in the range of [1/threshold, threshold], true is returned</param>
      /// <returns>True, if the deteminate of the homography matrix is in the range of [1/threshold, threshold]</returns>
      public bool IsValid(double thresholdForDeterminant)
      {
         if (thresholdForDeterminant == 0) return true;

         if (thresholdForDeterminant < 1.0) thresholdForDeterminant = 1.0 / thresholdForDeterminant;

         double det = CvInvoke.Determinant(this);
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

         using (Matrix<float> pointMat = new Matrix<float>(points.Length, 1, 2, handle.AddrOfPinnedObject(), 0))
         using (Matrix<float> homographyMat = Convert<float>())
         {
            CvInvoke.PerspectiveTransform(pointMat, pointMat, homographyMat);
         }

         handle.Free();
      }

      /// <summary>
      /// Return a clone of the Matrix
      /// </summary>
      /// <returns>A clone of the Matrix</returns>
      public new HomographyMatrix Clone()
      {
         HomographyMatrix m = new HomographyMatrix();
         Mat.CopyTo(m);
         return m;
      }
   }
}
*/