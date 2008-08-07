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
    [Obsolete("Beta version, API not Finalized")]
    public class Kalman : UnmanagedObject
    {
        /// <summary>
        /// Create a Kalman Filter using the specific values
        /// </summary>
        /// <param name="initialState">The m x 1 matrix</param>
        /// <param name="transitionMatrix">The m x m matrix (A) </param>
        /// <param name="controlMatrix">The m x n matrix (B)</param>
        /// <param name="measurementMatrix">The n x m matrix (H)</param>
        /// <param name="processNoiseCovarianceMatrix">The n x n matrix (Q)</param>
        /// <param name="measurementNoiseCovarianceMatrix">The m x m matrix (R)</param>
        public Kalman(
            Matrix<float> initialState, 
            Matrix<float> transitionMatrix, 
            Matrix<float> controlMatrix, 
            Matrix<float> measurementMatrix,
            Matrix<float> processNoiseCovarianceMatrix,
            Matrix<float> measurementNoiseCovarianceMatrix
            ):
            this(initialState.Rows, measurementMatrix.Rows, controlMatrix == null ? 0 : controlMatrix.Rows)
        {
            int sizeOfState = initialState.Rows;
            int sizeOfMeasurement = measurementMatrix.Rows;
            int sizeOfControl = controlMatrix == null ? 0 : controlMatrix.Rows;

            PredictedState = initialState;
            CorrectedState = initialState;
            TransitionMatrix = transitionMatrix;
            if (controlMatrix != null) ControlMatrix = controlMatrix;
            MeasurementMatrix = measurementMatrix;
            ProcessNoiseCovariance = processNoiseCovarianceMatrix;
            MeasurementNoiseCovariance = measurementNoiseCovarianceMatrix;
        }

        /// <summary>
        /// Create a Kalman Filter using the specific values
        /// </summary>
        /// <param name="initialState">The m x 1 matrix</param>
        /// <param name="transitionMatrix">The m x m matrix (A) </param>
        /// <param name="measurementMatrix">The n x m matrix (H)</param>
        /// <param name="processNoiseCovarianceMatrix">The n x n matrix (Q)</param>
        /// <param name="measurementNoiseCovarianceMatrix">The m x m matrix (R)</param>
        public Kalman(
            Matrix<float> initialState,
            Matrix<float> transitionMatrix,
            Matrix<float> measurementMatrix,
            Matrix<float> processNoiseCovarianceMatrix,
            Matrix<float> measurementNoiseCovarianceMatrix
            )
            :
            this(initialState, transitionMatrix, null, measurementMatrix, processNoiseCovarianceMatrix, measurementNoiseCovarianceMatrix)
        {
        }

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
        /// Get or Set the Predicted State
        /// </summary>
        public Matrix<float> PredictedState
        {
            get
            {
                MCvKalman kalman = MCvKalman; 
                MCvMat mat = (MCvMat) Marshal.PtrToStructure( kalman.state_pre, typeof(MCvMat));
                Matrix<float> res = new Matrix<float>(mat.rows, mat.cols);
                CvInvoke.cvCopy(kalman.state_pre, res.Ptr, IntPtr.Zero);
                return res;
            }
            set
            {
                CvInvoke.cvCopy(value.Ptr, MCvKalman.state_pre, IntPtr.Zero);
            }
        }

        /// <summary>
        /// Get or Set the Corrected State
        /// </summary>
        public Matrix<float> CorrectedState
        {
            get
            {
                MCvKalman kalman = MCvKalman;
                MCvMat mat = (MCvMat)Marshal.PtrToStructure(kalman.state_pre, typeof(MCvMat));
                Matrix<float> res = new Matrix<float>(mat.rows, mat.cols);
                CvInvoke.cvCopy(kalman.state_post, res.Ptr, IntPtr.Zero);
                return res;
            }
            set
            {
                CvInvoke.cvCopy(value.Ptr, MCvKalman.state_post, IntPtr.Zero);
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

        public Matrix<float> ControlMatrix
        {
            set
            {
                CvInvoke.cvCopy(value.Ptr, MCvKalman.control_matrix, IntPtr.Zero);
            }
        }
    }
}
