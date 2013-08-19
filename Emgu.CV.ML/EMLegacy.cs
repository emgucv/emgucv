
//----------------------------------------------------------------------------
//  Copyright (C) 2004-2013 by EMGU. All rights reserved.       
//----------------------------------------------------------------------------

using System;
using System.Runtime.InteropServices;
using Emgu.CV.Structure;
using Emgu.CV.ML.Structure;
using System.Diagnostics;

namespace Emgu.CV.ML
{
   /// <summary>
   /// Expectation Maximization model
   /// </summary>
   public class EMLegacy : StatModel
   {
      /// <summary>
      /// Create an Expectation Maximization model
      /// </summary>
      public EMLegacy()
      {
         _ptr = MlInvoke.CvEMLegacyDefaultCreate();
      }

      /// <summary>
      /// Creaet an Expectation Maximization model using the specific training parameters
      /// </summary>
      /// <param name="samples">The training data. A 32-bit floating-point, single-channel matrix, one vector per row</param>
      /// <param name="parameters">The parameters for EM</param>
      /// <param name="labels">Can be null if not needed. Optionally computed output "class label" for each sample</param>
      public EMLegacy(Matrix<float> samples, EMParams parameters, Matrix<Int32> labels)
         : this()
      {
         Train(samples, parameters, labels);
      }

      /// <summary>
      /// Train the EM model using the specific training data
      /// </summary>
      /// <param name="samples">The training data. A 32-bit floating-point, single-channel matrix, one vector per row</param>
      /// <param name="parameters">The parameters for EM</param>
      /// <param name="labels">Can be null if not needed. Optionally computed output "class label" for each sample</param>
      /// <returns></returns>
      public bool Train(Matrix<float> samples, EMParams parameters, Matrix<Int32> labels)
      {
         MCvEMParams param = new MCvEMParams();
         param.nclusters = parameters.Nclusters;
         param.cov_mat_type = parameters.CovMatType;
         param.start_step = parameters.StartStep;
         param.probs = parameters.Probs == null ? IntPtr.Zero : parameters.Probs.Ptr;
         param.means = parameters.Means == null ? IntPtr.Zero : parameters.Means.Ptr;
         param.weights = parameters.Weights == null ? IntPtr.Zero : parameters.Weights.Ptr;

         GCHandle? covsPtrHandle = null;
         if (parameters.Covs == null)
            param.covs = IntPtr.Zero;
         else
         {
            IntPtr[] covsPtr = 
#if NETFX_CORE
               Extensions.
#else
               Array.
#endif
               ConvertAll<Matrix<double>, IntPtr>(parameters.Covs, delegate(Matrix<double> m) { return m.Ptr; });
            covsPtrHandle = GCHandle.Alloc(covsPtr, GCHandleType.Pinned);
            param.covs = covsPtrHandle.Value.AddrOfPinnedObject();
         }
         param.term_crit = parameters.TermCrit;

         bool res = MlInvoke.CvEMLegacyTrain(
            _ptr,
            samples.Ptr,
            IntPtr.Zero,
            param,
            labels == null ? IntPtr.Zero : labels.Ptr);

         if (covsPtrHandle.HasValue)
            covsPtrHandle.Value.Free();

         return res;
      }

      /// <summary>
      /// Predit the probability of the <paramref name="samples"/>
      /// </summary>
      /// <param name="samples">The input samples</param>
      /// <param name="probs">The prediction results, should have the same # of rows as the <paramref name="samples"/></param>
      /// <returns>In case of classification the method returns the class label, in case of regression - the output function value</returns>
      public float Predict(Matrix<float> samples, Matrix<float> probs)
      {
         return MlInvoke.CvEMLegacyPredict(
            _ptr,
            samples.Ptr,
            probs == null ? IntPtr.Zero : probs.Ptr);
      }

      /// <summary>
      /// Get the number of clusters of this EM model
      /// </summary>
      public int NumberOfClusters
      {
         get
         {
            return MlInvoke.CvEMLegacyGetNclusters(_ptr);
         }
      }

      /// <summary>
      /// Get the mean of the clusters
      /// </summary>
      public Matrix<double> Means
      {
         get
         {
            return IntPtrToDoubleMatrix(MlInvoke.CvEMLegacyGetMeans(_ptr));
         }
      }

      /// <summary>
      /// Get the weights of the clusters
      /// </summary>
      public Matrix<double> Weights
      {
         get
         {
            return IntPtrToDoubleMatrix(MlInvoke.CvEMLegacyGetWeights(_ptr));
         }
      }

      /// <summary>
      /// Get the matrix of probability. 
      /// A entry on the m_th row and n_th col indicates the probability of the m_th data point given the n_th cluster.
      /// </summary>
      public Matrix<double> Probabilities
      {
         get
         {
            return IntPtrToDoubleMatrix(MlInvoke.CvEMLegacyGetProbs(_ptr));
         }
      }

      /// <summary>
      /// Get the covariance matrices for each cluster
      /// </summary>
      /// <returns>Get the array of covariance matrix for each data point.</returns>
      public Matrix<double>[] GetCovariances()
      {
         IntPtr ptrToCovs = MlInvoke.CvEMLegacyGetCovs(_ptr);
         if (ptrToCovs == IntPtr.Zero) return null;

         int ncluster = NumberOfClusters;
         IntPtr[] covPtrs = new IntPtr[ncluster];
         Marshal.Copy(ptrToCovs, covPtrs, 0, ncluster);

         return 
#if NETFX_CORE
            Extensions.
#else
            Array.
#endif
            ConvertAll<IntPtr, Matrix<double>>(covPtrs, IntPtrToDoubleMatrix);
      }

      private static Matrix<double> IntPtrToDoubleMatrix(IntPtr matPtr)
      {
         if (matPtr == IntPtr.Zero) return null;
         MCvMat mat = (MCvMat)Marshal.PtrToStructure(matPtr, typeof(MCvMat));
         Matrix<double> result = new Matrix<double>(mat.rows, mat.cols, 1, mat.data, mat.step);
         Debug.Assert(mat.type == result.MCvMat.type, "Matrix type is not double");
         return result;
      }

      /// <summary>
      /// Release the memory associated with this EM model
      /// </summary>
      protected override void DisposeObject()
      {
         MlInvoke.CvEMLegacyRelease(ref _ptr);
      }
   }
}
