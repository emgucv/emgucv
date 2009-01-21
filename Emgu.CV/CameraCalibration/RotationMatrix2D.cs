using System;
using System.Collections.Generic;
using System.Text;

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
      public RotationMatrix2D(System.Drawing.PointF center, T angle, T scale)
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
      public void SetRotation(System.Drawing.PointF center, T angle, T scale)
      {
         CvInvoke.cv2DRotationMatrix(center, System.Convert.ToDouble(angle), System.Convert.ToDouble(scale), Ptr);
      }
   }
}
