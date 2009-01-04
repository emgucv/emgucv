using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;

namespace Emgu.CV
{
   /// <summary>
   /// The extrinsic camera parameters
   /// </summary>
   public class ExtrinsicCameraParameters
   {
      private RotationVector3D _rotationVector;
      private Matrix<double> _translationVector;

      /// <summary>
      /// Get or Set the rodrigus rotation vector
      /// </summary>
      public RotationVector3D RotationVector
      {
         get
         {
            return _rotationVector;
         }
         set
         {
            _rotationVector = value;
         }
      }

      /// <summary>
      /// Get or Set the translation vector ( as 3 x 1 matrix)
      /// </summary>
      public Matrix<double> TranslationVector
      {
         get
         {
            return _translationVector;
         }
         set
         {
            Debug.Assert(value.Rows == 3 && value.Cols == 1, "Translation must be a 3 x 1 matrix");
            _translationVector = value;
         }
      }

      /// <summary>
      /// Get the 3 x 4 extrinsic matrix: [[r11 r12 r13 t1] [r21 r22 r23 t2] [r31 r32 r33 t2]]
      /// </summary>
      public Matrix<double> ExtrinsicMatrix
      {
         get
         {
            return _rotationVector.RotationMatrix.ConcateHorizontal(_translationVector);
         }
      }

      /// <summary>
      /// Create the extrinsic camera parameters
      /// </summary>
      public ExtrinsicCameraParameters()
      {
         RotationVector = new RotationVector3D();
         TranslationVector = new Matrix<double>(3, 1);
      }

      /// <summary>
      /// Create the extrinsic camera parameters using the specific rotation and translation matrix
      /// </summary>
      /// <param name="rotation">The rotation vector</param>
      /// <param name="translation">The translation vector</param>
      public ExtrinsicCameraParameters(RotationVector3D rotation, Matrix<double> translation)
      {
         RotationVector = rotation;
         TranslationVector = translation;
      }
   }
}
