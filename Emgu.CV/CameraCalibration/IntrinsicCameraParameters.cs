using System;
using System.Collections.Generic;
using System.Text;

namespace Emgu.CV.CameraCalibration
{
    /// <summary>
    /// The intrinsic camera parameters
    /// </summary>
    public class IntrinsicCameraParameters
    {
        private Matrix<float> _intrinsicMatrix;
        private Matrix<float> _distortionCoeffs;

        /// <summary>
        /// Get or Set the DistortionCoeffs ( as a 3x1 matrix )
        /// </summary>
        public Matrix<float> DistortionCoeffs
        {
            get { return _distortionCoeffs; }
            set { _distortionCoeffs = value; }
        }

        /// <summary>
        /// Get or Set the intrinsic matrix
        /// </summary>
        public Matrix<float> IntrinsicMatrix
        {
            get { return _intrinsicMatrix; }
            set { _intrinsicMatrix = value; }
        }

        /// <summary>
        /// Create the intrinsic camera parameters
        /// </summary>
        public IntrinsicCameraParameters()
        {
            _intrinsicMatrix = new Matrix<float>(3, 3);
            _distortionCoeffs = new Matrix<float>(3, 1);
        }
    }
}
