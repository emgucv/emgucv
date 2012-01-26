//----------------------------------------------------------------------------
//  Copyright (C) 2004-2012 by EMGU. All rights reserved.       
//----------------------------------------------------------------------------

using System;
using Emgu.CV.Structure;

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
      private Matrix<double> _probs;
      private Matrix<double> _weights;
      private Matrix<double> _means;
      private Matrix<double>[] _covs;
      private MCvTermCriteria _termCrit;

      //private IntPtr[] _covsPtr;
      //private GCHandle _covsPtrHandle;

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
      public Matrix<double>[] Covs
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
      public Matrix<double> Means
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
      public Matrix<double> Probs
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
      public Matrix<double> Weights
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
