//----------------------------------------------------------------------------
//  Copyright (C) 2004-2012 by EMGU. All rights reserved.       
//----------------------------------------------------------------------------

using System;
using Emgu.Util;
using Emgu.CV.Structure;

namespace Emgu.CV
{
   /// <summary>
   /// Kalman Filter 
   /// </summary>
   public class Kalman : DisposableObject
   {
      private Matrix<float> _statePre;
      private Matrix<float> _statePost;
      private Matrix<float> _transitionMatrix;
      private Matrix<float> _processNoiseCov;
      private Matrix<float> _measurementMatrix;
      private Matrix<float> _measurementNoiseCov;
      private Matrix<float> _errorCovPre;
      private Matrix<float> _errorCovPost;
      private Matrix<float> _gain;
      private Matrix<float> _controlMatrix;
      private Matrix<float> _temp1;
      private Matrix<float> _temp2;
      private Matrix<float> _temp3;
      private Matrix<float> _temp4;
      private Matrix<float> _temp5;

      private MCvKalman _kalman;

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
          )
         : this(
         initialState.Rows,
         measurementMatrix.Rows,
         controlMatrix == null ? 0 : controlMatrix.Rows)
      {
         PredictedState = initialState.Clone();
         CorrectedState = initialState.Clone();
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
      /// <param name="dynamParams">dimensionality of the state vector</param>
      /// <param name="measureParams">dimensionality of the measurement vector </param>
      /// <param name="controlParams">dimensionality of the control vector </param>
      public Kalman(int dynamParams, int measureParams, int controlParams)
      {
         _kalman.DP = dynamParams;
         _kalman.MP = measureParams;
         _kalman.CP = controlParams;

         PredictedState = new Matrix<float>(dynamParams, 1);

         CorrectedState = new Matrix<float>(dynamParams, 1);

         TransitionMatrix = new Matrix<float>(dynamParams, dynamParams);
         TransitionMatrix.SetIdentity();
        
         ProcessNoiseCovariance = new Matrix<float>(dynamParams, dynamParams);
         ProcessNoiseCovariance.SetIdentity();

         MeasurementMatrix = new Matrix<float>(measureParams, dynamParams);

         MeasurementNoiseCovariance = new Matrix<float>(measureParams, measureParams);
         MeasurementNoiseCovariance.SetIdentity();

         ErrorCovariancePre = new Matrix<float>(dynamParams, dynamParams);

         ErrorCovariancePost = new Matrix<float>(dynamParams, dynamParams);

         Gain = new Matrix<float>(dynamParams, measureParams);

         if (controlParams > 0)
         {
            ControlMatrix = new Matrix<float>(dynamParams, controlParams);
         }

         _temp1 = new Matrix<float>(dynamParams, dynamParams);
         _kalman.temp1 = _temp1.Ptr;
         _temp2 = new Matrix<float>(measureParams, dynamParams);
         _kalman.temp2 = _temp2.Ptr;
         _temp3 = new Matrix<float>(measureParams, measureParams);
         _kalman.temp3 = _temp3.Ptr;
         _temp4 = new Matrix<float>(measureParams, dynamParams);
         _kalman.temp4 = _temp4.Ptr;
         _temp5 = new Matrix<float>(measureParams, 1);
         _kalman.temp5 = _temp5.Ptr;

         //_kalman.Temp1 = _temp1.MCvMat.data;
         //_kalman.Temp2 = _temp2.MCvMat.data;

      }

      /// <summary>
      /// Get the MCvKalman structure
      /// </summary>
      public MCvKalman MCvKalman
      {
         get
         {
            return _kalman;
         }
      }

      /// <summary>
      /// Estimates the subsequent stochastic model state by its current state and stores it at PredictedState
      /// </summary>
      /// <param name="control">the control vector</param>
      /// <returns>The predicted state</returns>
      public Matrix<float> Predict(Matrix<float> control)
      {
         CvInvoke.cvKalmanPredict(
            ref _kalman, 
            control == null ? IntPtr.Zero : control.Ptr);
         return PredictedState;
      }

      /// <summary>
      /// Adjusts stochastic model state on the basis of the given measurement of the model state
      /// </summary>
      /// <remarks>The function stores adjusted state at kalman->state_post and returns it on output</remarks>
      /// <param name="measurement">The measurement data</param>
      /// <returns>The corrected state</returns>
      public Matrix<float> Correct(Matrix<float> measurement)
      {
         CvInvoke.cvKalmanCorrect(ref _kalman, measurement.Ptr);
         return CorrectedState;
      }

      /// <summary>
      /// Estimates the subsequent stochastic model state by its current state
      /// </summary>
      /// <returns>The predicted state</returns>
      public Matrix<float> Predict()
      {
         return Predict(null);
      }

      /// <summary>
      /// Get or Set the Predicted State
      /// </summary>
      public Matrix<float> PredictedState
      {
         get
         {
            return _statePre;
         }
         set
         {
            _statePre = value;
            _kalman.state_pre = value.Ptr;
            _kalman.PosterState = value.MCvMat.data;
         }
      }

      /// <summary>
      /// Get or Set the Corrected State
      /// </summary>
      public Matrix<float> CorrectedState
      {
         get
         {
            return _statePost;
         }
         set
         {
            _statePost = value;
            _kalman.state_post = value.Ptr;
            _kalman.PriorState = value.MCvMat.data;
         }
      }

      /// <summary>
      /// Get or Set the measurement matrix
      /// </summary>
      public Matrix<float> MeasurementMatrix
      {
         set
         {
            _measurementMatrix = value;
            _kalman.measurement_matrix = value.Ptr;
            _kalman.MeasurementMatr = value.MCvMat.data;
         }
         get
         {
            return _measurementMatrix;
         }
      }

      /// <summary>
      /// Get or Set the state transition matrix
      /// </summary>
      public Matrix<float> TransitionMatrix
      {
         set
         {
            _transitionMatrix = value;
            _kalman.transition_matrix = value.Ptr;
            _kalman.DynamMatr = value.MCvMat.data;
         }
         get
         {
            return _transitionMatrix;
         }
      }

      /// <summary>
      /// Get or Set the process noise covariance matrix
      /// </summary>
      public Matrix<float> ProcessNoiseCovariance
      {
         set
         {
            _processNoiseCov = value;
            _kalman.process_noise_cov = value.Ptr;
            _kalman.PNCovariance = value.MCvMat.data;
         }
         get
         {
            return _processNoiseCov;
         }
      }

      /// <summary>
      /// Get or Set the measurement noise covariance matrix
      /// </summary>
      public Matrix<float> MeasurementNoiseCovariance
      {
         set
         {
            _measurementNoiseCov = value;
            _kalman.measurement_noise_cov = value.Ptr;
            _kalman.MNCovariance = value.MCvMat.data;
         }
         get
         {
            return _measurementNoiseCov;
         }
      }

      /// <summary>
      /// Get or Set the posteriori error estimate covariance matrix
      /// </summary>
      public Matrix<float> ErrorCovariancePost
      {
         set
         {
            _errorCovPost = value;
            _kalman.error_cov_post = value.Ptr;
            _kalman.PosterErrorCovariance = value.MCvMat.data;
         }
         get
         {
            return _errorCovPost;
         }
      }

      /// <summary>
      /// Get or Set the prior error convariance matrix
      /// </summary>
      public Matrix<float> ErrorCovariancePre
      {
         set
         {
            _errorCovPre = value;
            _kalman.error_cov_pre = value.Ptr;
            _kalman.PriorErrorCovariance = value.MCvMat.data;
         }
         get
         {
            return _errorCovPre;
         }
      }

      /// <summary>
      /// Get or Set the control matrix 
      /// </summary>
      public Matrix<float> ControlMatrix
      {
         set
         {
            _controlMatrix = value;
            _kalman.control_matrix = value.Ptr;
         }
         get
         {
            return _controlMatrix;
         }
      }

      /// <summary>
      /// Get or Set the Kalman Gain
      /// </summary>
      public Matrix<float> Gain
      {
         set
         {
            _gain = value;
            _kalman.gain = value.Ptr;
            _kalman.KalmGainMatr = value.MCvMat.data;
         }
         get
         {
            return _gain;
         }
      }

      /// <summary>
      /// Release unmanaged resource
      /// </summary>
      protected override void DisposeObject()
      {
      }

      /// <summary>
      /// Release all the matrix associated to this object
      /// </summary>
      protected override void ReleaseManagedResources()
      {
         if (_statePre != null) _statePre.Dispose();
         if (_statePost != null) _statePost.Dispose();
         if (_transitionMatrix != null) _transitionMatrix.Dispose();
         if (_processNoiseCov != null) _processNoiseCov.Dispose();
         if (_measurementMatrix != null) _measurementMatrix.Dispose();
         if (_measurementNoiseCov != null) _measurementNoiseCov.Dispose();
         if (_errorCovPre != null) _errorCovPre.Dispose();
         if (_errorCovPost != null) _errorCovPost.Dispose();
         if (_gain != null) _gain.Dispose();
         if (_controlMatrix != null) _controlMatrix.Dispose();
         if (_temp1 != null) _temp1.Dispose();
         if (_temp2 != null) _temp2.Dispose();
         if (_temp3 != null) _temp3.Dispose();
         if (_temp4 != null) _temp4.Dispose();
         if (_temp5 != null) _temp5.Dispose();
      }
   }
}
