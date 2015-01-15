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
   public class EM : UnmanagedObject, IStatModel
   {
      public class Params :UnmanagedObject
      {
         /// <summary>
         /// Create EM parameters
         /// </summary>
         /// <param name="nclusters">The number of mixture components in the Gaussian mixture model. Use 5 for default.</param>
         /// <param name="covMatType">Constraint on covariance matrices which defines type of matrices</param>
         /// <param name="termcrit">The termination criteria of the EM algorithm. The EM algorithm can be terminated by the number of iterations termCrit.maxCount (number of M-steps) or when relative change of likelihood logarithm is less than termCrit.epsilon. Default maximum number of iterations is 100</param>
         public Params(int nclusters, MlEnum.EmCovarianMatrixType covMatType, MCvTermCriteria termcrit)
         {
            _ptr = MlInvoke.cveEmParamsCreate(nclusters, covMatType, ref termcrit);
         }

         protected override void DisposeObject()
         {
            MlInvoke.cveEmParamsRelease(ref _ptr);
         }
      }

      private IntPtr _statModel;
      private IntPtr _algorithm;

      /// <summary>
      /// Create an Expectation Maximization model
      /// </summary>
     public EM(Params p)
      {
         _ptr = MlInvoke.CvEMDefaultCreate(p, ref _statModel, ref _algorithm);
      }


      /*
      /// <summary>
      /// Starts with Expectation step. Initial values of the model parameters will be estimated by the k-means algorithm.
      /// </summary>
      /// <param name="samples">The training data. A 32-bit floating-point, single-channel matrix, one vector per row</param>
      /// <param name="labels">Can be null if not needed. Optionally computed output "class label" for each sample</param>
      /// <param name="logLikelihoods">Can be null if not needed. The optional output matrix that contains a likelihood logarithm value for each sample. It has nsamples x 1 size and CV_64FC1 type.</param>
      /// <param name="probs">Can be null if not needed. Initial probabilities p_{i,k} of sample i to belong to mixture component k. It is a one-channel floating-point matrix of nsamples x nclusters size.</param>
      /// <returns>The methods return true if the Gaussian mixture model was trained successfully, otherwise it returns false.</returns>
      public bool Train(IInputArray samples, IOutputArray logLikelihoods = null, IOutputArray labels = null, IOutputArray probs = null)
      {
         using (InputArray iaSamples = samples.GetInputArray())
         using (OutputArray oaLoglikelihoods = logLikelihoods == null ? OutputArray.GetEmpty() : logLikelihoods.GetOutputArray())
         using (OutputArray oaLabels = labels == null ? OutputArray.GetEmpty() : labels.GetOutputArray())
         using (OutputArray oaProbs = probs == null ? OutputArray.GetEmpty() : probs.GetOutputArray())
            return MlInvoke.CvEMTrain(_ptr, iaSamples, oaLoglikelihoods, oaLabels, oaProbs);
      }*/

      public EM(IInputArray samples, IInputArray means0, IInputArray covs0, IInputArray weights0,
         IOutputArray loglikelihoods, IOutputArray labels, IOutputArray probs, Params p)
      {
         using (InputArray iaSamples = samples.GetInputArray())
         using (InputArray iaMeans0 = means0.GetInputArray())
         using (InputArray iaCovs0 = covs0 == null ? InputArray.GetEmpty() : covs0.GetInputArray())
         using (InputArray iaWeights = weights0 == null ? InputArray.GetEmpty() : weights0.GetInputArray())
         using (OutputArray oaLogLikelihood = loglikelihoods == null ? OutputArray.GetEmpty() : loglikelihoods.GetOutputArray())
         using (OutputArray oaLabels = labels == null ? OutputArray.GetEmpty() : labels.GetOutputArray())
         using (OutputArray oaProbs = probs == null ? OutputArray.GetEmpty() : probs.GetOutputArray())
         {
            
            _ptr = MlInvoke.CvEMTrainStartWithE(iaSamples, iaMeans0, iaCovs0, iaWeights, oaLogLikelihood, oaLabels,
               oaProbs, p, ref _statModel, ref _algorithm);
            
         }
      }

      public EM(
         IInputArray samples,
         IInputArray probs0,
         IOutputArray logLikelihoods,
         IOutputArray labels,
         IOutputArray probs,
         Params p)
      {
         using (InputArray iaSamples = samples.GetInputArray())
         using (InputArray iaProbs0 = probs0.GetInputArray())
         using (OutputArray oaLogLikelihood = logLikelihoods == null ? OutputArray.GetEmpty() : logLikelihoods.GetOutputArray())
         using (OutputArray oaLabels = labels == null ? OutputArray.GetEmpty() : labels.GetOutputArray())
         using (OutputArray oaProbs = probs == null ? OutputArray.GetEmpty() : probs.GetOutputArray())
         {
            _ptr = MlInvoke.CvEMTrainStartWithM(iaSamples, iaProbs0, oaLogLikelihood, oaLabels, oaProbs, p, ref _statModel, ref _algorithm);
            
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
