using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using Emgu.CV.Structure;
using Emgu.CV.ML.Structure;

namespace Emgu.CV.ML
{
   /// <summary>
   /// Expectation Maximization model
   /// </summary>
   public class EM : StatModel
   {
      /// <summary>
      /// Create an Expectation Maximization model
      /// </summary>
      public EM()
      {
         _ptr = MlInvoke.CvEMDefaultCreate();
      }

      /// <summary>
      /// Creaet an Expectation Maximization model using the specific training parameters
      /// </summary>
      /// <param name="samples">The samples to be trained</param>
      /// <param name="sampleIdx"></param>
      /// <param name="parameters"></param>
      /// <param name="labels"></param>
      public EM(Matrix<float> samples, Matrix<float> sampleIdx, EMParams parameters, Matrix<Int32> labels)
         : this()
      {
         Train(samples, sampleIdx, parameters, labels);
      }

      /// <summary>
      /// Train the EM model using the specific training data
      /// </summary>
      /// <param name="samples">The training data. A 32-bit floating-point, single-channel matrix, one vector per row</param>
      /// <param name="sampleIdx">Can be null if not needed. When specified, identifies samples of interest. It is a Matrix&gt;int&lt; of nx1</param>
      /// <param name="parameters">The parameters for EM</param>
      /// <param name="labels">Can be null if not needed. Optionally computed output "class label" for each sample</param>
      /// <returns></returns>
      public bool Train(Matrix<float> samples, Matrix<float> sampleIdx, EMParams parameters, Matrix<Int32> labels)
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
            IntPtr[] covsPtr = Array.ConvertAll<CvArray<double>, IntPtr>(parameters.Covs,
               delegate(CvArray<double> m) { return m.Ptr; });
            covsPtrHandle = GCHandle.Alloc(covsPtr, GCHandleType.Pinned);
            param.covs = covsPtrHandle.Value.AddrOfPinnedObject();
         }
         param.term_crit = parameters.TermCrit;
 
         bool res = MlInvoke.CvEMTrain(
            _ptr, 
            samples.Ptr, 
            sampleIdx == null? IntPtr.Zero : sampleIdx.Ptr, 
            param,
            labels == null? IntPtr.Zero : labels.Ptr);

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
         return MlInvoke.CvEMPredict(
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
            return MlInvoke.CvEMGetNclusters(_ptr);
         }
      }

      /// <summary>
      /// Get the mean of the clusters
      /// </summary>
      /// <returns>The mean of the clusters</returns>
      public Matrix<double> GetMeans()
      {
         return IntPtrToDoubleMatrix( MlInvoke.CvEMGetMeans(_ptr));
      }

      /// <summary>
      /// Get the weights of the clusters
      /// </summary>
      /// <returns>The weights of the clusters</returns>
      public Matrix<double> GetWeights()
      {
         return IntPtrToDoubleMatrix(MlInvoke.CvEMGetWeights(_ptr));
      }

      /// <summary>
      /// Get the probability matrix
      /// </summary>
      /// <returns></returns>
      public Matrix<double> GetProbabilities()
      {
         return IntPtrToDoubleMatrix(MlInvoke.CvEMGetProbs(_ptr));
      }

      /// <summary>
      /// Get the covariance matrices for each cluster
      /// </summary>
      /// <returns></returns>
      public Matrix<double>[] GetCovariances()
      {
         Int64 ptrToCovs = MlInvoke.CvEMGetCovs(_ptr).ToInt64();
         if (ptrToCovs == 0) return null;

         int ncluster = NumberOfClusters;
         int step = Marshal.SizeOf(typeof(IntPtr));

         Matrix<double>[] covarianceMatrices = new Matrix<double>[ncluster];

         for (int n = 0; n < ncluster; ptrToCovs += step, n++)
         {
            IntPtr covMatPtr = Marshal.ReadIntPtr(new IntPtr(ptrToCovs));
            covarianceMatrices[n] = IntPtrToDoubleMatrix(covMatPtr);
         }
         return covarianceMatrices;
      }

      private Matrix<double> IntPtrToDoubleMatrix(IntPtr matPtr)
      {
         if (matPtr == IntPtr.Zero) return null;

         int rows, cols;
         GetMatrixInfo(matPtr, out rows, out cols);
         Matrix<double> res = new Matrix<double>(rows, cols);
         CvInvoke.cvCopy(matPtr, res, IntPtr.Zero);

         return res;
      }

      private void GetMatrixInfo(IntPtr matPtr, out int rows, out int cols)
      {
         MCvMat mat = (MCvMat)Marshal.PtrToStructure(matPtr, typeof(MCvMat));
         rows = mat.rows;
         cols = mat.cols;
      }

      /// <summary>
      /// Release the memory associated with this EM model
      /// </summary>
      protected override void DisposeObject()
      {
         MlInvoke.CvEMRelease(_ptr);
      }
   }
}
