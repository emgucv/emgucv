//----------------------------------------------------------------------------
//  Copyright (C) 2004-2012 by EMGU. All rights reserved.       
//----------------------------------------------------------------------------

using System;
using Emgu.CV.ML.Structure;
using System.Runtime.InteropServices;

namespace Emgu.CV.ML
{
   /// <summary>
   /// Support Vector Machine 
   /// </summary>
   public class SVM : StatModel
   {
      /// <summary>
      /// Create a support Vector Machine
      /// </summary>
      public SVM()
      {
         _ptr = MlInvoke.CvSVMDefaultCreate();
      }

      /// <summary>
      /// Release all the memory associated with the SVM
      /// </summary>
      protected override void DisposeObject()
      {
         MlInvoke.CvSVMRelease(ref _ptr);
      }

      /// <summary>
      /// Get a copy of the SVM parameters
      /// </summary>
      public MCvSVMParams Parameters
      {
         get
         {
            MCvSVMParams p = new MCvSVMParams();
            MlInvoke.CvSVMGetParameters(Ptr, ref p);
            return p;
         }
      }

      /// <summary>
      /// Train the SVM model with the specific paramters
      /// </summary>
      /// <param name="trainData">The training data.</param>
      /// <param name="responses">The response for the training data.</param>
      /// <param name="varIdx">Can be null if not needed. When specified, identifies variables (features) of interest. It is a Matrix&lt;int&gt; of nx1</param>
      /// <param name="sampleIdx">Can be null if not needed. When specified, identifies samples of interest. It is a Matrix&lt;int&gt; of nx1</param>
      /// <param name="parameters">The parameters for SVM</param>
      /// <returns></returns>
      public bool Train(
         Matrix<float> trainData,
         Matrix<float> responses,
         Matrix<Byte> varIdx,
         Matrix<Byte> sampleIdx,
         SVMParams parameters)
      {
         return MlInvoke.CvSVMTrain(
            _ptr, 
            trainData.Ptr, 
            responses.Ptr, 
            varIdx == null ? IntPtr.Zero: varIdx.Ptr, 
            sampleIdx == null ? IntPtr.Zero : varIdx.Ptr, 
            parameters.MCvSVMParams);
      }

      /// <summary>
      /// Get the default parameter grid for the specific SVM type
      /// </summary>
      /// <param name="type">The SVM type</param>
      /// <returns>The default parameter grid for the specific SVM type </returns>
      public static MCvParamGrid GetDefaultGrid(MlEnum.SVM_PARAM_TYPE type)
      {
         MCvParamGrid grid = new MCvParamGrid();
         MlInvoke.CvSVMGetDefaultGrid(type, ref grid);
         return grid;
      }

      /// <summary>
      /// The method trains the SVM model automatically by choosing the optimal parameters C, gamma, p, nu, coef0, degree from CvSVMParams. By the optimality one mean that the cross-validation estimate of the test set error is minimal. 
      /// </summary>
      /// <param name="trainData">The training data.</param>
      /// <param name="responses">The response for the training data.</param>
      /// <param name="varIdx">Can be null if not needed. When specified, identifies variables (features) of interest. It is a Matrix&lt;int&gt; of nx1</param>
      /// <param name="sampleIdx">Can be null if not needed. When specified, identifies samples of interest. It is a Matrix&lt;int&gt; of nx1</param>
      /// <param name="parameters">The parameters for SVM</param>
      /// <param name="kFold">Cross-validation parameter. The training set is divided into k_fold subsets, one subset being used to train the model, the others forming the test set. So, the SVM algorithm is executed k_fold times</param>
      /// <returns></returns>
      public bool TrainAuto(
         Matrix<float> trainData,
         Matrix<float> responses,
         Matrix<Byte> varIdx,
         Matrix<Byte> sampleIdx,
         MCvSVMParams parameters,
         int kFold)
      {
         return TrainAuto(
            trainData,
            responses,
            varIdx,
            sampleIdx,
            parameters,
            kFold,
            GetDefaultGrid(Emgu.CV.ML.MlEnum.SVM_PARAM_TYPE.C),
            GetDefaultGrid(Emgu.CV.ML.MlEnum.SVM_PARAM_TYPE.GAMMA),
            GetDefaultGrid(Emgu.CV.ML.MlEnum.SVM_PARAM_TYPE.P),
            GetDefaultGrid(Emgu.CV.ML.MlEnum.SVM_PARAM_TYPE.NU),
            GetDefaultGrid(Emgu.CV.ML.MlEnum.SVM_PARAM_TYPE.COEF),
            GetDefaultGrid(Emgu.CV.ML.MlEnum.SVM_PARAM_TYPE.DEGREE));
      }

      /// <summary>
      /// The method trains the SVM model automatically by choosing the optimal parameters C, gamma, p, nu, coef0, degree from CvSVMParams. By the optimality one mean that the cross-validation estimate of the test set error is minimal. 
      /// </summary>
      /// <param name="trainData">The training data.</param>
      /// <param name="responses">The response for the training data.</param>
      /// <param name="varIdx">Can be null if not needed. When specified, identifies variables (features) of interest. It is a Matrix&lt;int&gt; of nx1</param>
      /// <param name="sampleIdx">Can be null if not needed. When specified, identifies samples of interest. It is a Matrix&lt;int&gt; of nx1</param>
      /// <param name="parameters">The parameters for SVM</param>
      /// <param name="kFold">Cross-validation parameter. The training set is divided into k_fold subsets, one subset being used to train the model, the others forming the test set. So, the SVM algorithm is executed k_fold times</param>
      /// <param name="cGrid">cGrid</param>
      /// <param name="gammaGrid">gammaGrid</param>
      /// <param name="pGrid">pGrid</param>
      /// <param name="nuGrid">nuGrid</param>
      /// <param name="coefGrid">coedGrid</param>
      /// <param name="degreeGrid">degreeGrid</param>
      /// <returns></returns>
      public bool TrainAuto(
         Matrix<float> trainData,
         Matrix<float> responses,
         Matrix<Byte> varIdx,
         Matrix<Byte> sampleIdx,
         MCvSVMParams parameters,
         int kFold,
         MCvParamGrid cGrid,
         MCvParamGrid gammaGrid,
         MCvParamGrid pGrid,
         MCvParamGrid nuGrid,
         MCvParamGrid coefGrid,
         MCvParamGrid degreeGrid)
      {
         return MlInvoke.CvSVMTrainAuto(
            Ptr,
            trainData.Ptr,
            responses.Ptr,
            varIdx == null ? IntPtr.Zero : varIdx.Ptr,
            sampleIdx == null ? IntPtr.Zero : sampleIdx.Ptr,
            parameters,
            kFold,
            cGrid,
            gammaGrid, 
            pGrid, 
            nuGrid,
            coefGrid,
            degreeGrid);
      }

      #region contribution from Albert G
      /// <summary>
      /// Predicts response for the input sample.
      /// </summary>
      /// <param name="sample">The input sample</param>
      public float Predict(Matrix<float> sample)
      {
         return MlInvoke.CvSVMPredict(Ptr, sample.Ptr);
      }

      /// <summary>
      /// The method retrieves a given support vector
      /// </summary>
      /// <param name="i">The index of the support vector</param>       
      /// <returns>The <paramref name="i"/>th support vector</returns>
      public float[] GetSupportVector(int i)
      {
         int k = GetVarCount();
         float[] res = new float[k];
         IntPtr vector = MlInvoke.CvSVMGetSupportVector(Ptr, i);
         Marshal.Copy(vector, res, 0, k);
         return res;
      }

      /// <summary>
      /// The method retrieves the number of support vectors
      /// </summary>
      /// <returns>The number of support vectors</returns>
      public int GetSupportVectorCount()
      {
         return MlInvoke.CvSVMGetSupportVectorCount(Ptr);
      }

      /// <summary>
      /// The method retrieves the number of vars
      /// </summary>
      /// <returns>The number of variables</returns>
      public int GetVarCount()
      {
         return MlInvoke.CvSVMGetVarCount(Ptr);
      }
      #endregion
   }
}
