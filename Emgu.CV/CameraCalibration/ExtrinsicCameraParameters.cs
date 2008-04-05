using System;
using System.Collections.Generic;
using System.Text;

namespace Emgu.CV.CameraCalibration
{
    /// <summary>
    /// The extrinsic camera parameters
    /// </summary>
    public class ExtrinsicCameraParameters 
    {
        private RotationVector _rotationVector;
        private Matrix<float> _translationVector;

        /// <summary>
        /// Get or Set the rodrigus rotation vector
        /// </summary>
        public RotationVector RotationVector
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
        public Matrix<float> TranslationVector
        {
            get
            {
                return _translationVector;
            }
            set
            {
                _translationVector = value;
            }
        }

        /// <summary>
        /// Create the extrinsic camera parameters
        /// </summary>
        public ExtrinsicCameraParameters()
        {
            _rotationVector = new RotationVector();
            _translationVector = new Matrix<float>(3, 1);
        }
    }
}
