//----------------------------------------------------------------------------
//  Copyright (C) 2004-2015 by EMGU Corporation. All rights reserved.       
//----------------------------------------------------------------------------

using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using Emgu.CV.ML.Structure;
using Emgu.CV.Structure;
using Emgu.Util;

namespace Emgu.CV.ML
{
   /// <summary>
   /// Expectation Maximization model
   /// </summary>
   public partial class EM : UnmanagedObject, IStatModel
   {

      /// <summary>
      /// The type of the mixture covariation matrices
      /// </summary>
      public enum CovarianMatrixType
      {
         /// <summary>
         /// A covariation matrix of each mixture is a scaled identity matrix, ?k*I, so the only parameter to be estimated is ?k. The option may be used in special cases, when the constraint is relevant, or as a first step in the optimization (e.g. in case when the data is preprocessed with PCA). The results of such preliminary estimation may be passed again to the optimization procedure, this time with cov_mat_type=COV_MAT_DIAGONAL
         /// </summary>
         Spherical = 0,
         /// <summary>
         /// A covariation matrix of each mixture may be arbitrary diagonal matrix with positive diagonal elements, that is, non-diagonal elements are forced to be 0's, so the number of free parameters is d  for each matrix. This is most commonly used option yielding good estimation results
         /// </summary>
         Diagonal = 1,
         /// <summary>
         /// A covariation matrix of each mixture may be arbitrary symmetrical positively defined matrix, so the number of free parameters in each matrix is about d2/2. It is not recommended to use this option, unless there is pretty accurate initial estimation of the parameters and/or a huge number of training samples
         /// </summary>
         Generic = 2,

         Default = Diagonal
      }

      private IntPtr _statModel;
      private IntPtr _algorithm;

      /// <summary>
      /// Create an Expectation Maximization model
      /// </summary>
     public EM()
      {
         _ptr = MlInvoke.CvEMDefaultCreate(ref _statModel, ref _algorithm);
      }


      public void trainE(IInputArray samples, IInputArray means0, IInputArray covs0, IInputArray weights0,
         IOutputArray loglikelihoods, IOutputArray labels, IOutputArray probs)
      {
         using (InputArray iaSamples = samples.GetInputArray())
         using (InputArray iaMeans0 = means0.GetInputArray())
         using (InputArray iaCovs0 = covs0 == null ? InputArray.GetEmpty() : covs0.GetInputArray())
         using (InputArray iaWeights = weights0 == null ? InputArray.GetEmpty() : weights0.GetInputArray())
         using (OutputArray oaLogLikelihood = loglikelihoods == null ? OutputArray.GetEmpty() : loglikelihoods.GetOutputArray())
         using (OutputArray oaLabels = labels == null ? OutputArray.GetEmpty() : labels.GetOutputArray())
         using (OutputArray oaProbs = probs == null ? OutputArray.GetEmpty() : probs.GetOutputArray())
         {
            
            MlInvoke.CvEMTrainE(_ptr, iaSamples, iaMeans0, iaCovs0, iaWeights, oaLogLikelihood, oaLabels,
               oaProbs, ref _statModel, ref _algorithm);
            
         }
      }

      public void TrainM(
         IInputArray samples,
         IInputArray probs0,
         IOutputArray logLikelihoods,
         IOutputArray labels,
         IOutputArray probs)
      {
         using (InputArray iaSamples = samples.GetInputArray())
         using (InputArray iaProbs0 = probs0.GetInputArray())
         using (OutputArray oaLogLikelihood = logLikelihoods == null ? OutputArray.GetEmpty() : logLikelihoods.GetOutputArray())
         using (OutputArray oaLabels = labels == null ? OutputArray.GetEmpty() : labels.GetOutputArray())
         using (OutputArray oaProbs = probs == null ? OutputArray.GetEmpty() : probs.GetOutputArray())
         {
            MlInvoke.CvEMTrainM(_ptr, iaSamples, iaProbs0, oaLogLikelihood, oaLabels, oaProbs, ref _statModel, ref _algorithm);
            
         }
      }


      /// <summary>
      /// Predict the probability of the <paramref name="samples"/>
      /// </summary>
      /// <param name="samples">The input samples</param>
      /// <param name="probs">The prediction results, should have the same # of rows as the <paramref name="samples"/></param>
      public MCvPoint2D64f Predict(IInputArray samples, IOutputArray probs = null)
      {
         MCvPoint2D64f result = new MCvPoint2D64f();
         using (InputArray iaSamples = samples.GetInputArray())
         using (OutputArray oaProbs = probs == null ? OutputArray.GetEmpty() : probs.GetOutputArray())
            MlInvoke.CvEMPredict(
              _ptr,
              iaSamples,
              ref result,
              oaProbs);
         return result;
      }

      /// <summary>
      /// Release the memory associated with this EM model
      /// </summary>
      protected override void DisposeObject()
      {
         MlInvoke.CvEMRelease(ref _ptr);
         _statModel = IntPtr.Zero;
         _algorithm = IntPtr.Zero;
      }

      IntPtr IAlgorithm.AlgorithmPtr
      {
         get { return _algorithm; }
      }

      IntPtr IStatModel.StatModelPtr
      {
         get { return _statModel; }
      }
   }
}
