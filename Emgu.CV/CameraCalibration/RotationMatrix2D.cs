using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Runtime.InteropServices;

namespace Emgu.CV
{
   /// <summary>
   /// A 2D rotation matrix
   /// </summary>
   ///<typeparam name="T">The depth of the rotation matrix, should be float / double</typeparam>
   [Serializable]
   public class RotationMatrix2D<T> : Matrix<T> where T: struct
   {
      /// <summary>
      /// Create an empty (2x3) 2D rotation matrix
      /// </summary>
      public RotationMatrix2D()
         : base(2, 3)
      { }

      /// <summary>
      /// Create a 2D rotation matrix
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

      /*
      /// <summary>
      /// Rotate the points inplace
      /// </summary>
      /// <param name="points">the points to be rotated, its value is changed</param>
      public void RotatePoints(PointF[] points)
      {
         GCHandle handle = GCHandle.Alloc(points, GCHandleType.Pinned);
         IntPtr matHeader = Marshal.AllocHGlobal(StructSize.MCvMat);
         CvInvoke.cvInitMatHeader(matHeader, points.Length, 2, Emgu.CV.CvEnum.MAT_DEPTH.CV_32F, handle.AddrOfPinnedObject(), StructSize.PointF);
         Marshal.FreeHGlobal(matHeader);

         using (Matrix<float> tmp = new Matrix<float>(points.Length, 3))
         {
            tmp.SetValue(1.0);

            using (Matrix<float> cols = tmp.GetCols(0, 2))
            {
               CvInvoke.cvCopy(matHeader, cols, IntPtr.Zero);
            }

            CvInvoke.cvGEMM(tmp, Ptr, 1.0, IntPtr.Zero, 0.0, matHeader, Emgu.CV.CvEnum.GEMM_TYPE.CV_GEMM_B_T);
         }
         handle.Free();
      }
      */
   }
}
