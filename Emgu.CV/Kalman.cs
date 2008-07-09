using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;

namespace Emgu.CV
{
    /// <summary>
    /// A Kalman filter object
    /// </summary>
    /// <remarks>Beta: Non working version</remarks>
    public class Kalman : UnmanagedObject
    {
        /// <summary>
        /// Allocates CvKalman and all its matrices and initializes them somehow. 
        /// </summary>
        /// <param name="dynam_params">dimensionality of the state vector</param>
        /// <param name="measure_params">dimensionality of the measurement vector </param>
        /// <param name="control_params">dimensionality of the control vector </param>
        public Kalman(int dynam_params, int measure_params, int control_params)
        {
            _ptr = CvInvoke.cvCreateKalman(dynam_params, measure_params, control_params);
        }

        /// <summary>
        /// Release the memory associated to kalman
        /// </summary>
        protected override void DisposeObject()
        {
            CvInvoke.cvReleaseKalman(ref _ptr);
        }

        /// <summary>
        /// Get the MCvKalman structure
        /// </summary>
        public MCvKalman MCvKalman
        {
            get
            {
                return (MCvKalman)Marshal.PtrToStructure(Ptr, typeof(MCvKalman));
            }
        }

        /// <summary>
        /// Estimates the subsequent stochastic model state by its current state and stores it at kalman->state_pre
        /// </summary>
        /// <param name="control">the control vector</param>
        public void Predict(Matrix<float> control)
        {
            CvInvoke.cvKalmanPredict(_ptr, control.Ptr);
        }

        /// <summary>
        /// Get the Prediction
        /// </summary>
        public Matrix<float> Prediction
        {
            get
            {
                MCvKalman kalman = MCvKalman; 
                MCvMat mat = (MCvMat) Marshal.PtrToStructure( kalman.state_pre, typeof(MCvMat));
                Matrix<float> res = new Matrix<float>(mat.rows, mat.cols);
                CvInvoke.cvCopy(kalman.state_pre, res.Ptr, IntPtr.Zero);
                return res;
            }
        }

        /// <summary>
        /// Set the measurement matrix
        /// </summary>
        public Matrix<float> MeasurementMatrix
        {
            set
            {
                CvInvoke.cvCopy(value.Ptr, MCvKalman.measurement_matrix, IntPtr.Zero);
            }
        }

        /// <summary>
        /// Set the state transition matrix
        /// </summary>
        public Matrix<float> TransitionMatrix
        {
            set
            {
                CvInvoke.cvCopy(value.Ptr, MCvKalman.transition_matrix, IntPtr.Zero);
            }
        }

        /// <summary>
        /// Set the process noise covariance matrix
        /// </summary>
        public Matrix<float> ProcessNoiseCovariance
        {
            set
            {
                CvInvoke.cvCopy(value.Ptr, MCvKalman.process_noise_cov, IntPtr.Zero);
            }
        }

        /// <summary>
        /// Set the measurement noise covariance matrix
        /// </summary>
        public Matrix<float> MeasurementNoiseCovariance
        {
            set
            {
                CvInvoke.cvCopy(value.Ptr, MCvKalman.measurement_noise_cov, IntPtr.Zero);
            }
        }

        /// <summary>
        /// Set the posteriori error estimate covariance matrix
        /// </summary>
        public Matrix<float> ErrorCovariancePost
        {
            set
            {
                CvInvoke.cvCopy(value.Ptr, MCvKalman.error_cov_post, IntPtr.Zero);
            }
        }
    }
}
