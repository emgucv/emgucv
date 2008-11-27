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
      /// <param name="sample_idx"></param>
      /// <param name="parameter"></param>
      /// <param name="labels"></param>
      /// <returns></returns>
      [DllImport(EXTERN_LIBRARY)]
      public static extern bool CvEMTrain(
         IntPtr model,
         IntPtr samples,
         IntPtr sample_idx,
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
      #endregion
   }
}
