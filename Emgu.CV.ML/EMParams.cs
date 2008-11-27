using System;
using System.Collections.Generic;
using System.Text;

namespace Emgu.CV.ML
{
   /// <summary>
   /// The parameters for the EM model
   /// </summary>
   public class EMParams
   {
      private int _nclusters;
      private MlEnum.EM_COVARIAN_MATRIX_TYPE _covMatType;
      private MlEnum.EM_INIT_STEP_TYPE _startStep;
      private Matrix<float> _probs;
      private Matrix<float> _weights;
      private Matrix<float> _means;
      private Matrix<float>[] _covs;
      private MCvTermCriteria _termCrit;

      /// <summary>
      /// Create EM parameters with default value
      /// </summary>
      public EMParams()
      {
         Nclusters = 10;
         CovMatType = Emgu.CV.ML.MlEnum.EM_COVARIAN_MATRIX_TYPE.COV_MAT_DIAGONAL;
         StartStep = Emgu.CV.ML.MlEnum.EM_INIT_STEP_TYPE.START_AUTO_STEP;
         _termCrit = new MCvTermCriteria(100, 1.0e-6);
      }

      /// <summary>
      /// Get the equivalent MCvEMParams structure
      /// </summary>
      public MCvEMParams MCvEMParams
      {
         get
         {
            MCvEMParams param = new MCvEMParams();
            param.nclusters = _nclusters;
            param.cov_mat_type = _covMatType;
            param.start_step = _startStep;
            param.probs = _probs == null? IntPtr.Zero: _probs.Ptr;
            param.means = _means == null? IntPtr.Zero : _means.Ptr;
            param.weights = _weights == null ? IntPtr.Zero : _weights.Ptr;
            param.covs = _covs == null ? null :
               Array.ConvertAll<Matrix<float>, IntPtr>(_covs,
               delegate(Matrix<float> m) { return m.Ptr; });
            param.term_crit = _termCrit;
            return param;
         }
      }

      /// <summary>
      /// Get or set the type of Covariance matrix
      /// </summary>
      public MlEnum.EM_COVARIAN_MATRIX_TYPE CovMatType
      {
         get
         {
            return _covMatType;
         }
         set
         {
            _covMatType = value;
         }
      }

      /// <summary>
      /// Get or Set the Covariance matrices
      /// </summary>
      public Matrix<float>[] Covs
      {
         get
         {
            return _covs;
         }
         set
         {
            _covs = value;
         }
      }

      /// <summary>
      /// Get or set the means
      /// </summary>
      public Matrix<float> Means
      {
         get
         {
            return _means;
         }
         set
         {
            _means = value;
         }
      }

      /// <summary>
      /// Get or Set the number of clusters
      /// </summary>
      public int Nclusters
      {
         get
         {
            return _nclusters;
         }
         set
         {
            _nclusters = value;
         }
      }

      /// <summary>
      /// Get or Set the probabilities
      /// </summary>
      public Matrix<float> Probs
      {
         get
         {
            return _probs;
         }
         set
         {
            _probs = value;
         }
      }

      /// <summary>
      /// Get or Set the start step
      /// </summary>
      public MlEnum.EM_INIT_STEP_TYPE StartStep
      {
         get
         {
            return _startStep;
         }
         set
         {
            _startStep = value;
         }
      }

      /// <summary>
      /// Get or Set the termination criteria
      /// </summary>
      public MCvTermCriteria TermCrit
      {
         get
         {
            return _termCrit;
         }
         set
         {
            _termCrit = value;
         }
      }

      /// <summary>
      /// Get or Set the weights
      /// </summary>
      public Matrix<float> Weights
      {
         get
         {
            return _weights;
         }
         set
         {
            _weights = value;
         }
      }
   }
}
