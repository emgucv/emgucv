//----------------------------------------------------------------------------
//  Copyright (C) 2004-2013 by EMGU. All rights reserved.       
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
   public class EM : UnmanagedObject
   {
      /// <summary>
      /// Create an Expectation Maximization model
      /// </summary>
      /// <param name="nclusters">The number of mixture components in the Gaussian mixture model. Use 5 for default.</param>
      /// <param name="covMatType">Constraint on covariance matrices which defines type of matrices</param>
      /// <param name="termcrit">The termination criteria of the EM algorithm. The EM algorithm can be terminated by the number of iterations termCrit.maxCount (number of M-steps) or when relative change of likelihood logarithm is less than termCrit.epsilon. Default maximum number of iterations is 100</param>
      public EM(int nclusters, MlEnum.EM_COVARIAN_MATRIX_TYPE covMatType, MCvTermCriteria termcrit)
      {
         _ptr = MlInvoke.CvEMDefaultCreate(nclusters, covMatType, termcrit);
      }

      /// <summary>
      /// Starts with Expectation step. Initial values of the model parameters will be estimated by the k-means algorithm.
      /// </summary>
      /// <param name="samples">The training data. A 32-bit floating-point, single-channel matrix, one vector per row</param>
      /// <param name="labels">Can be null if not needed. Optionally computed output "class label" for each sample</param>
      /// <param name="logLikelihoods">Can be null if not needed. The optional output matrix that contains a likelihood logarithm value for each sample. It has nsamples x 1 size and CV_64FC1 type.</param>
      /// <param name="probs">Can be null if not needed. Initial probabilities p_{i,k} of sample i to belong to mixture component k. It is a one-channel floating-point matrix of nsamples x nclusters size.</param>
      /// <returns>The methods return true if the Gaussian mixture model was trained successfully, otherwise it returns false.</returns>
      public bool Train(Matrix<float> samples, Matrix<Int32> labels, Matrix<float> probs, Matrix<double> logLikelihoods)
      {
         return MlInvoke.CvEMTrain(_ptr, samples, labels, probs, logLikelihoods);
      }

      /// <summary>
      /// Predit the probability of the <paramref name="samples"/>
      /// </summary>
      /// <param name="samples">The input samples</param>
      /// <param name="probs">The prediction results, should have the same # of rows as the <paramref name="samples"/></param>
      /// <param name="likelihood">The likelihood logarithm value</param>
      /// <returns>an index of the most probable mixture component for the given sample.</returns>
      public double Predict(Matrix<float> samples, Matrix<float> probs, out double likelihood)
      {
         likelihood = 0;
         return  MlInvoke.CvEMPredict(
            _ptr,
            samples.Ptr,
            probs == null ? IntPtr.Zero : probs.Ptr,
            ref likelihood);
      }

      /// <summary>
      /// Release the memory associated with this EM model
      /// </summary>
      protected override void DisposeObject()
      {
         MlInvoke.CvEMRelease(ref _ptr);
      }
   }
}
