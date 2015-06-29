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
         /// <summary>
         /// The default
         /// </summary>
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

     /// <summary>
     /// Estimate the Gaussian mixture parameters from a samples set. This variation starts with Expectation step. You need to provide initial means of mixture components. Optionally you can pass initial weights and covariance matrices of mixture components.
     /// </summary>
     /// <param name="samples">Samples from which the Gaussian mixture model will be estimated. It should be a one-channel matrix, each row of which is a sample. If the matrix does not have CV_64F type it will be converted to the inner matrix of such type for the further computing.</param>
     /// <param name="means0">Initial means of mixture components. It is a one-channel matrix of nclusters x dims size. If the matrix does not have CV_64F type it will be converted to the inner matrix of such type for the further computing.</param>
     /// <param name="covs0">The vector of initial covariance matrices of mixture components. Each of covariance matrices is a one-channel matrix of dims x dims size. If the matrices do not have CV_64F type they will be converted to the inner matrices of such type for the further computing.</param>
     /// <param name="weights0">Initial weights of mixture components. It should be a one-channel floating-point matrix with 1 x nclusters or nclusters x 1 size.</param>
     /// <param name="loglikelihoods">The optional output matrix that contains a likelihood logarithm value for each sample. It has nsamples x 1 size and CV_64FC1 type.</param>
     /// <param name="labels">The optional output "class label" (indices of the most probable mixture component for each sample). It has nsamples x 1 size and CV_32SC1 type.</param>
     /// <param name="probs">The optional output matrix that contains posterior probabilities of each Gaussian mixture component given the each sample. It has nsamples x nclusters size and CV_64FC1 type.</param>
      public void trainE(
         IInputArray samples, 
         IInputArray means0, 
         IInputArray covs0 = null, 
         IInputArray weights0 = null,
         IOutputArray loglikelihoods = null, 
         IOutputArray labels = null, 
         IOutputArray probs = null)
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

      /// <summary>
      /// Estimate the Gaussian mixture parameters from a samples set.
      /// This variation starts with Expectation step. Initial values of the model parameters will be estimated by the k-means algorithm.
      /// Unlike many of the ML models, EM is an unsupervised learning algorithm and it does not take responses (class labels or function values) as input. Instead, it computes the Maximum Likelihood Estimate of the Gaussian mixture parameters from an input sample set, stores all the parameters inside the structure, and optionally computes the output "class label" for each sample.
      /// The trained model can be used further for prediction, just like any other classifier.
      /// </summary>
      /// <param name="samples">Samples from which the Gaussian mixture model will be estimated. It should be a one-channel matrix, each row of which is a sample. If the matrix does not have CV_64F type it will be converted to the inner matrix of such type for the further computing.</param>
      /// <param name="probs0">The probs0.</param>
      /// <param name="logLikelihoods">The optional output matrix that contains a likelihood logarithm value for each sample. It has nsamples x 1 size and CV_64FC1 type.</param>
      /// <param name="labels">The optional output "class label" for each sample(indices of the most probable mixture component for each sample). It has nsamples x 1 size and CV_32SC1 type.</param>
      /// <param name="probs">The optional output matrix that contains posterior probabilities of each Gaussian mixture component given the each sample. It has nsamples x nclusters size and CV_64FC1 type.</param>
      public void TrainM(
         IInputArray samples,
         IInputArray probs0,
         IOutputArray logLikelihoods = null,
         IOutputArray labels = null,
         IOutputArray probs = null)
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
