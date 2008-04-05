using System;
using System.Collections.Generic;
using System.Text;

namespace Emgu.CV
{
    /// <summary>
    /// A 2D rotation matrix
    /// </summary>
    public class RotationMatrix2D: Matrix<float>
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
        public RotationMatrix2D(Point2D<float> center, double angle, double scale)
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
        public void SetRotation(Point2D<float> center, double angle, double scale)
        {
            CvInvoke.cv2DRotationMatrix(new MCvPoint2D32f(center.X, center.Y), angle, scale, Ptr);
        }
    }
}
