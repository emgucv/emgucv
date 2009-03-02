using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;
using Emgu.CV.Structure;

namespace Emgu.CV
{
   /// <summary>
   /// A (2x3) 2D rotation matrix
   /// </summary>
   ///<typeparam name="T">The depth of the rotation matrix, should be float / double</typeparam>
   [Serializable]
   public class RotationMatrix2D<T> : Matrix<T> where T: struct
   {
      /// <summary>
      /// Constructor used to deserialize 2D rotation matrix
      /// </summary>
      /// <param name="info">The serialization info</param>
      /// <param name="context">The streaming context</param>
      public RotationMatrix2D(SerializationInfo info, StreamingContext context)
         : base(info, context)
      {
      }

      /// <summary>
      /// Create an empty (2x3) 2D rotation matrix
      /// </summary>
      public RotationMatrix2D()
         : base(2, 3)
      { }

      /// <summary>
      /// Create a (2x3) 2D rotation matrix
      /// </summary>
      /// <param name="center">Center of the rotation in the source image</param>
      /// <param name="angle">The rotation angle in degrees. Positive values mean couter-clockwise rotation (the coordiate origin is assumed at top-left corner). </param>
      /// <param name="scale">Isotropic scale factor.</param>
      public RotationMatrix2D(PointF center, double angle, double scale)
         : this()
      {
         SetRotation(center, angle, scale);
      }

      /// <summary>
      /// Set the values of the rotation matrix
      /// </summary>
      /// <param name="center">Center of the rotation in the source image</param>
      /// <param name="angle">The rotation angle in degrees. Positive values mean couter-clockwise rotation (the coordiate origin is assumed at top-left corner). </param>
      /// <param name="scale">Isotropic scale factor.</param>
      public void SetRotation(PointF center, double angle, double scale)
      {
         CvInvoke.cv2DRotationMatrix(center, angle, scale, Ptr);
      }

      /// <summary>
      /// Rotate the points inplace
      /// </summary>
      /// <param name="points">The points to be rotated, its value will be modified</param>
      public void RotatePoints(MCvPoint2D64f [] points)
      {
         GCHandle handle = GCHandle.Alloc(points, GCHandleType.Pinned);
         using (Matrix<double> mat = new Matrix<double>(points.Length, 2, handle.AddrOfPinnedObject()))
         using (Matrix<double> tmp = new Matrix<double>(points.Length, 3))
         {
            tmp.SetValue(1.0);

            using (Matrix<double> cols = tmp.GetCols(0, 2))
            {
               CvInvoke.cvCopy(mat, cols, IntPtr.Zero);
            }

            Matrix<double> rotationMatrix = this as Matrix<double> ?? Convert<double>();

            CvInvoke.cvGEMM(
               tmp, 
               rotationMatrix, 
               1.0, 
               IntPtr.Zero, 
               0.0, 
               mat, 
               Emgu.CV.CvEnum.GEMM_TYPE.CV_GEMM_B_T);

            if (!Object.ReferenceEquals(rotationMatrix, this)) rotationMatrix.Dispose();
         }
         handle.Free();
      }

      /// <summary>
      /// Rotate the points inplace
      /// </summary>
      /// <param name="points">The points to be rotated, its value will be modified</param>
      public void RotatePoints(PointF[] points)
      {
         GCHandle handle = GCHandle.Alloc(points, GCHandleType.Pinned);
         using (Matrix<float> mat = new Matrix<float>(points.Length, 2, handle.AddrOfPinnedObject()))
         using (Matrix<float> tmp = new Matrix<float>(points.Length, 3))
         {
            tmp.SetValue(1.0);

            using (Matrix<float> cols = tmp.GetCols(0, 2))
            {
               CvInvoke.cvCopy(mat, cols, IntPtr.Zero);
            }
            
            Matrix<float> rotationMatrix = this as Matrix<float> ?? Convert<float>();

            CvInvoke.cvGEMM(
               tmp,
               rotationMatrix,
               1.0,
               IntPtr.Zero,
               0.0,
               mat,
               Emgu.CV.CvEnum.GEMM_TYPE.CV_GEMM_B_T);

            if (!Object.ReferenceEquals(rotationMatrix, this)) rotationMatrix.Dispose();
         }
         handle.Free();
      }
   }
}
