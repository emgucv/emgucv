using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;

namespace Emgu.CV.ML
{
   /// <summary>
   /// This class contains functions to call into machine learning library
   /// </summary>
   public class MlInvoke
   {
      private const string EXTERN_LIBRARY = "cvextern.dll";

      #region CvStatModel
      /// <summary>
      /// Save the statistic model to the specific file
      /// </summary>
      /// <param name="statModel">The statistic model to save</param>
      /// <param name="fileName">The file name to save to</param>
      [DllImport(EXTERN_LIBRARY)]
      public static extern void StatModelSave(IntPtr statModel, String fileName);

      /// <summary>
      /// Load the statistic model from the specific file
      /// </summary>
      /// <param name="statModel">The statistic model to save</param>
      /// <param name="fileName">The file name to load from</param>
      [DllImport(EXTERN_LIBRARY)]
      public static extern void StatModelLoad(IntPtr statModel, String fileName);

      /// <summary>
      /// Clear the statistic model
      /// </summary>
      /// <param name="statModel">The model to be cleared</param>
      [DllImport(EXTERN_LIBRARY)]
      public static extern void StatModelClear(IntPtr statModel);
      #endregion

      #region CvNormalBayesClassifier
      /// <summary>
      /// Create a normal bayes classifier
      /// </summary>
      /// <returns>The normal bayes classifier</returns>
      [DllImport(EXTERN_LIBRARY)]
      public static extern IntPtr CvNormalBayesClassifierDefaultCreate();

      [DllImport(EXTERN_LIBRARY)]
      public static extern IntPtr CvNormalBayesClassifierCreate(IntPtr _train_data, IntPtr _responses, IntPtr _var_idx, IntPtr _sample_idx);

      /// <summary>
      /// Release the memory associated with the bayes classifier
      /// </summary>
      /// <param name="classifier">The classifier to release</param>
      [DllImport(EXTERN_LIBRARY)]
      public static extern void CvNormalBayesClassifierRelease(IntPtr classifier);

      [DllImport(EXTERN_LIBRARY)]
      public static extern bool CvNormalBayesClassifierTrain(
         IntPtr classifier,
         IntPtr _train_data,
         IntPtr _responses,
         IntPtr _var_idx,
         IntPtr _sample_idx,
         bool update);

      [DllImport(EXTERN_LIBRARY)]
      public static extern float CvNormalBayesClassifierPredict(IntPtr model, IntPtr samples, IntPtr results);
      #endregion

      #region CvKNearest
      /// <summary>
      /// Create a KNearest classifier
      /// </summary>
      /// <returns>The KNearest classifier</returns>
      [DllImport(EXTERN_LIBRARY)]
      public static extern IntPtr CvKNearestDefaultCreate();

      /// <summary>
      /// Release the KNearest classifer
      /// </summary>
      /// <param name="knearest">The classifier to release</param>
      [DllImport(EXTERN_LIBRARY)]
      public static extern void CvKNearestRelease(IntPtr knearest);

      [DllImport(EXTERN_LIBRARY)]
      public static extern IntPtr CvKNearestCreate(
         IntPtr TrainData,
         IntPtr Responses,
         IntPtr SampleIdx,
         bool IsRegression,
         int max_k);

      [DllImport(EXTERN_LIBRARY)]
      public static extern float CvKNearestFindNearest(
         IntPtr lassifier, 
         IntPtr samples, 
         int k, 
         IntPtr results,
         IntPtr[] neighbors, 
         IntPtr neighborResponses, 
         IntPtr dist);
      #endregion

      #region CvEM
      /// <summary>
      /// Create a default EM model
      /// </summary>
      /// <returns>Pointer to the EM model</returns>
      [DllImport(EXTERN_LIBRARY)]
      public static extern IntPtr CvEMDefaultCreate();

      /// <summary>
      /// Release the EM model
      /// </summary>
      [DllImport(EXTERN_LIBRARY)]
      public static extern void CvEMRelease(IntPtr emModel);

      /// <summary>
      /// Train the EM model using the specific training data
      /// </summary>
      /// <param name="model"></param>
      /// <param name="samples"></param>
      /// <param name="sampleIdx"></param>
      /// <param name="parameter"></param>
      /// <param name="labels"></param>
      /// <returns></returns>
      [DllImport(EXTERN_LIBRARY)]
      public static extern bool CvEMTrain(
         IntPtr model,
         IntPtr samples,
         IntPtr sampleIdx,
         MCvEMParams parameter,
         IntPtr labels);


      /// <summary>
      /// 
      /// </summary>
      /// <param name="model"></param>
      /// <param name="sample"></param>
      /// <param name="probs"></param>
      /// <returns></returns>
      [DllImport(EXTERN_LIBRARY)]
      public static extern float CvEMPredict(
         IntPtr model, 
         IntPtr sample, 
         IntPtr probs );

      /// <summary>
      /// Get the means of the clusters from the EM model
      /// </summary>
      /// <param name="model">The EM model</param>
      /// <returns>The means of the clusters of the EM model</returns>
      [DllImport(EXTERN_LIBRARY)]
      public static extern IntPtr CvEMGetMeans(IntPtr model);

      /// <summary>
      /// Get the covariance matrices of the clusters from the EM model
      /// </summary>
      /// <param name="model">The EM model</param>
      /// <returns>The covariance matrices of the clusters of the EM model</returns>
      [DllImport(EXTERN_LIBRARY)]
      public static extern IntPtr CvEMGetCovs(IntPtr model);

      /// <summary>
      /// Get the weights of the clusters from the EM model
      /// </summary>
      /// <param name="model">The EM model</param>
      /// <returns>The weights of the clusters of the EM model</returns>
      [DllImport(EXTERN_LIBRARY)]
      public static extern IntPtr CvEMGetWeights(IntPtr model);

      /// <summary>
      /// Get the probabilities from the EM model
      /// </summary>
      /// <param name="model">The EM model</param>
      /// <returns>The probabilities of the EM model </returns>
      [DllImport(EXTERN_LIBRARY)]
      public static extern IntPtr CvEMGetProbs(IntPtr model);

      /// <summary>
      /// Get the number of clusters from the EM model
      /// </summary>
      /// <param name="model">The EM model</param>
      /// <returns>The number of clusters of the EM model</returns>
      [DllImport(EXTERN_LIBRARY)]
      public static extern int CvEMGetNclusters(IntPtr model);
      #endregion

      #region CvSVM
      /// <summary>
      /// Create a default SVM model
      /// </summary>
      /// <returns>Pointer to the SVM model</returns>
      [DllImport(EXTERN_LIBRARY)]
      public static extern IntPtr CvSVMDefaultCreate();

      /// <summary>
      /// Release the SVM model and all the memory associated to ir
      /// </summary>
      /// <param name="model">The SVM model to be released</param>
      [DllImport(EXTERN_LIBRARY)]
      public static extern void CvSVMRelease(IntPtr model);

      /// <summary>
      /// Train the SVM model with the specific paramters
      /// </summary>
      /// <param name="model">The SVM model</param>
      /// <param name="trainData"></param>
      /// <param name="responses"></param>
      /// <param name="varIdx"></param>
      /// <param name="sampleIdx"></param>
      /// <param name="parameters"></param>
      /// <returns></returns>
      [DllImport(EXTERN_LIBRARY)]
      public static extern bool CvSVMTrain(
         IntPtr model,
         IntPtr trainData,
         IntPtr responses,
         IntPtr varIdx,
         IntPtr sampleIdx,
         MCvSVMParams parameters);
      #endregion

      #region CvANN_MLP
      [DllImport(EXTERN_LIBRARY)]
      public static extern IntPtr CvANN_MLPCreate(
         IntPtr layerSizes,
         MlEnum.ANN_MLP_ACTIVATION_FUNCTION activFunc,
         double fParam1,
         double fParam2);

      /// <summary>
      /// Release the ANN_MLP model
      /// </summary>
      /// <param name="model">The ANN_MLP model to be released</param>
      [DllImport(EXTERN_LIBRARY)]
      public static extern void CvANN_MLPRelease(IntPtr model);

      [DllImport(EXTERN_LIBRARY)]
      public static extern int CvANN_MLPTrain(
         IntPtr model, 
         IntPtr inputs, 
         IntPtr outputs,
         IntPtr sampleWeights, 
         IntPtr sampleIdx,
         MCvANN_MLP_TrainParams parameters,
         MlEnum.ANN_MLP_TRAINING_FLAG flags );

      [DllImport(EXTERN_LIBRARY)]
      public static extern float CvANN_MLPPredict(
         IntPtr model,
         IntPtr inputs,
         IntPtr outputs);
      #endregion 
   }
}
