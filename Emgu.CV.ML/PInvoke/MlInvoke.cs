//----------------------------------------------------------------------------
//  Copyright (C) 2004-2015 by EMGU Corporation. All rights reserved.       
//----------------------------------------------------------------------------

using System;
using System.Runtime.InteropServices;
using Emgu.CV.ML.MlEnum;
using Emgu.CV.ML.Structure;
using Emgu.CV;
using Emgu.CV.Structure;
using Emgu.CV.CvEnum;

namespace Emgu.CV.ML
{
   /// <summary>
   /// This class contains functions to call into machine learning library
   /// </summary>
   public partial class MlInvoke
   {
      static MlInvoke()
      {
         //dummy code that is used to involve the static constructor of CvInvoke, if it has not already been called.
         CvInvoke.MakeType(0, 0);
      }

      #region CvStatModel
      /// <summary>
      /// Save the statistic model to the specific file
      /// </summary>
      /// <param name="statModel">The statistic model to save</param>
      /// <param name="fileName">The file name to save to</param>
      [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      internal static extern void StatModelSave(IntPtr statModel, IntPtr fileName);

      /*
      /// <summary>
      /// Load the statistic model from the specific file
      /// </summary>
      /// <param name="statModel">The statistic model to save</param>
      /// <param name="fileName">The file name to load from</param>
      [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      public static extern void StatModelLoad(IntPtr statModel, String fileName);
      */
      /// <summary>
      /// Clear the statistic model
      /// </summary>
      /// <param name="statModel">The model to be cleared</param>
      [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      internal static extern void StatModelClear(IntPtr statModel);


      [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      [return: MarshalAs(CvInvoke.BoolMarshalType)]
      internal static extern bool StatModelTrain(IntPtr model, IntPtr samples, DataLayoutType layout, IntPtr responses);

      [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      [return: MarshalAs(CvInvoke.BoolMarshalType)]
      internal static extern bool StatModelTrainWithData(IntPtr model, IntPtr data, int flags);

      [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      internal static extern float StatModelPredict(IntPtr model, IntPtr samples, IntPtr results, int flags);
      #endregion

      [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      internal static extern IntPtr cveTrainDataCreate(
         IntPtr samples, DataLayoutType layout, IntPtr responses,
         IntPtr varIdx, IntPtr sampleIdx,
         IntPtr sampleWeights, IntPtr varType);

      [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      internal static extern void cveTrainDataRelease(ref IntPtr data);

      #region CvNormalBayesClassifier
      /// <summary>
      /// Create a normal bayes classifier
      /// </summary>
      /// <returns>The normal bayes classifier</returns>
      [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      internal static extern IntPtr CvNormalBayesClassifierDefaultCreate(ref IntPtr statModel, ref IntPtr algorithm);

      /*
      /// <summary>
      /// Create a normal Bayes classifier using the specific training data
      /// </summary>
      /// <param name="trainData">The training data. A 32-bit floating-point, single-channel matrix, one vector per row</param>
      /// <param name="responses">A floating-point matrix of the corresponding output vectors, one vector per row. </param>
      /// <param name="varIdx">Can be IntPtr.Zero if not needed. When specified, identifies variables (features) of interest. It is a Matrix&gt;int&lt; of nx1</param>
      /// <param name="sampleIdx">Can be IntPtr.Zero if not needed. When specified, identifies samples of interest. It is a Matrix&gt;int&lt; of nx1</param>
      /// <returns>The normal Beyes classifier</returns>
      [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      public static extern IntPtr CvNormalBayesClassifierCreate(
         IntPtr trainData,
         IntPtr responses,
         IntPtr varIdx,
         IntPtr sampleIdx);
      */

      /// <summary>
      /// Release the memory associated with the bayes classifier
      /// </summary>
      /// <param name="classifier">The classifier to release</param>
      [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      internal static extern void CvNormalBayesClassifierRelease(ref IntPtr classifier);

      /*
      /// <summary>
      /// Train the classifier using the specific data
      /// </summary>
      /// <param name="classifier">The NormalBayesClassifier</param>
      /// <param name="trainData">The training data. A 32-bit floating-point, single-channel matrix, one vector per row</param>
      /// <param name="responses">A floating-point matrix of the corresponding output vectors, one vector per row. </param>
      /// <param name="varIdx">Can be IntPtr.Zero if not needed. When specified, identifies variables (features) of interest. It is a Matrix&gt;int&lt; of nx1</param>
      /// <param name="sampleIdx">Can be IntPtr.Zero if not needed. When specified, identifies samples of interest. It is a Matrix&gt;int&lt; of nx1</param>
      /// <param name="update">If true, the training data is used to update the classifier; Otherwise, the data in the classifier are cleared before training is performed</param>
      /// <returns>The number of done iterations</returns>
      [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      [return: MarshalAs(CvInvoke.BoolMarshalType)]
      public static extern bool CvNormalBayesClassifierTrain(
         IntPtr classifier,
         IntPtr trainData,
         IntPtr responses,
         IntPtr varIdx,
         IntPtr sampleIdx,
         [MarshalAs(CvInvoke.BoolMarshalType)]
         bool update);

      /// <summary>
      /// Given the NormalBayesClassifier <paramref name="model"/>, predit the probability of the <paramref name="samples"/>
      /// </summary>
      /// <param name="model">The NormalBayesClassifier classifier model</param>
      /// <param name="samples">The input samples</param>
      /// <param name="results">The prediction results, should have the same # of rows as the <paramref name="samples"/></param>
      /// <returns>In case of classification the method returns the class label, in case of regression - the output function value</returns>
      [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      public static extern float CvNormalBayesClassifierPredict(IntPtr model, IntPtr samples, IntPtr results);*/
      #endregion

      #region CvKNearest
      /// <summary>
      /// Create a KNearest classifier
      /// </summary>
      /// <returns>The KNearest classifier</returns>
      [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      internal static extern IntPtr CvKNearestCreate(
         ref IntPtr statModel,
         ref IntPtr algorithm);

      /// <summary>
      /// Release the KNearest classifier
      /// </summary>
      /// <param name="knearest">The classifier to release</param>
      [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      internal static extern void CvKNearestRelease(ref IntPtr knearest);

      /*
      /// <summary>
      /// Create the KNearest classifier using the specific traing data.
      /// </summary>
      /// <param name="isRegression">Specify the output variables type. It can be either categorical (isRegression=false) or ordered (isRegression=true)</param>
      /// <param name="maxK">The number of maximum neighbors that may be passed to the method findNearest.</param>
      /// <param name="trainData">The training data. A 32-bit floating-point, single-channel matrix, one vector per row</param>
      /// <param name="responses">A floating-point matrix of the corresponding output vectors, one vector per row. </param>
      /// <param name="sampleIdx">Can be IntPtr.Zero if not needed. When specified, identifies samples of interest. It is a Matrix&gt;int&lt; of nx1</param>
      /// <returns>The KNearest classifier</returns>
      [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      public static extern IntPtr CvKNearestCreate(
         IntPtr trainData,
         IntPtr responses,
         IntPtr sampleIdx,
         [MarshalAs(CvInvoke.BoolMarshalType)]
         bool isRegression,
         int maxK);

      /// <summary>
      /// Update the KNearest classifier using the specific traing data.
      /// </summary>
      /// <param name="isRegression">Specify the output variables type. It can be either categorical (isRegression=false) or ordered (isRegression=true)</param>
      /// <param name="maxK">The number of maximum neighbors that may be passed to the method findNearest.</param>
      /// <param name="trainData">The training data. A 32-bit floating-point, single-channel matrix, one vector per row</param>
      /// <param name="responses">A floating-point matrix of the corresponding output vectors, one vector per row. </param>
      /// <param name="sampleIdx">Can be IntPtr.Zero if not needed. When specified, identifies samples of interest. It is a Matrix&gt;int&lt; of nx1</param>
      /// <param name="classifier">The KNearest classifier to be updated</param>
      /// <param name="updateBase">
      /// If true, the existing classifer is updated using the new training data;
      /// Otherwise, the classifier is trained from scratch</param>
      /// <returns></returns>
      [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      [return: MarshalAs(CvInvoke.BoolMarshalType)]
      public static extern bool CvKNearestTrain(
         IntPtr classifier,
         IntPtr trainData,
         IntPtr responses,
         IntPtr sampleIdx,
         [MarshalAs(CvInvoke.BoolMarshalType)]
         bool isRegression,
         int maxK,
        [MarshalAs(CvInvoke.BoolMarshalType)]
         bool updateBase);

      /// <summary>
      /// For each input vector (which are rows of the matrix <paramref name="samples"/>) the method finds k &lt;= get_max_k() nearest neighbor. In case of regression, the predicted result will be a mean value of the particular vector's neighbor responses. In case of classification the class is determined by voting.
      /// </summary>
      /// <param name="classifier">The KNearest classifier</param>
      /// <param name="samples">The sample matrix where each row is a sample</param>
      /// <param name="k">The number of nearest neighbor to find</param>
      /// <param name="results">
      /// Can be IntPtr.Zero if not needed.
      /// If regression, return a mean value of the particular vector's neighbor responses;
      /// If classification, return the class determined by voting.
      /// </param>
      /// <param name="kNearestNeighbors">Should be IntPtr.Zero if not needed. Setting it to non-null values incures a performance panalty. A matrix of (k * samples.Rows) rows and (samples.Cols) columns that will be filled the data of the K nearest-neighbor for each sample</param>
      /// <param name="neighborResponses">Should be IntPtr.Zero if not needed. The response of the neighbors. A vector of k*_samples->rows elements.</param>
      /// <param name="dist">Should be IntPtr.Zero if not needed. The distances from the input vectors to the neighbors. A vector of k*_samples->rows elements.</param>
      /// <returns>In case of regression, the predicted result will be a mean value of the particular vector's neighbor responses. In case of classification the class is determined by voting</returns>
      [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      public static extern float CvKNearestFindNearest(
         IntPtr classifier,
         IntPtr samples,
         int k,
         IntPtr results,
         IntPtr[] kNearestNeighbors,
         IntPtr neighborResponses,
         IntPtr dist);
      */
      #endregion

      #region CvEM
      /*
      [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      internal static extern IntPtr cveEmParamsCreate(int nclusters, MlEnum.EmCovarianMatrixType covMatType, ref MCvTermCriteria termcrit);

      [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      internal static extern void cveEmParamsRelease(ref IntPtr p);
      */
      /// <summary>
      /// Create a default EM model
      /// </summary>
      /// <returns>Pointer to the EM model</returns>
      [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      internal static extern IntPtr CvEMDefaultCreate(ref IntPtr statModel, ref IntPtr algorithm);

      /// <summary>
      /// Release the EM model
      /// </summary>
      [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      internal static extern void CvEMRelease(ref IntPtr emModel);

      /*
      /// <summary>
      /// Starts with Expectation step. Initial values of the model parameters will be estimated by the k-means algorithm.
      /// </summary>
      /// <param name="model">The EM model</param>
      /// <param name="samples">The training data. A 32-bit floating-point, single-channel matrix, one vector per row</param>
      /// <param name="labels">Can be IntPtr.Zero if not needed. Optionally computed output "class label" for each sample</param>
      /// <param name="logLikelihoods">The optional output matrix that contains a likelihood logarithm value for each sample. It has nsamples x 1 size and CV_64FC1 type.</param>
      /// <param name="probs">Initial probabilities p_{i,k} of sample i to belong to mixture component k. It is a one-channel floating-point matrix of nsamples x nclusters size.</param>
      /// <returns>The methods return true if the Gaussian mixture model was trained successfully, otherwise it returns false.</returns>
      [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      [return: MarshalAs(CvInvoke.BoolMarshalType)]
      internal static extern bool CvEMTrain(
         IntPtr model,
         IntPtr samples,
         IntPtr logLikelihoods,
         IntPtr labels,
         IntPtr probs
         );
      */
      [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      internal static extern void CvEMTrainE(
         IntPtr model,
         IntPtr samples,
         IntPtr means0,
         IntPtr covs0,
         IntPtr weights0,
         IntPtr logLikelihoods,
         IntPtr labels,
         IntPtr probs,
         ref IntPtr statModel,
         ref IntPtr algorithm);

      [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      internal static extern void CvEMTrainM(
         IntPtr model,
         IntPtr samples,
         IntPtr probs0,
         IntPtr logLikelihoods,
         IntPtr labels,
         IntPtr probs,
         ref IntPtr statModel,
         ref IntPtr algorithm);

      
      /// <summary>
      /// Given the EM <paramref name="model"/>, predict the probability of the <paramref name="samples"/>
      /// </summary>
      /// <param name="model">The EM model</param>
      /// <param name="samples">The input samples</param>
      /// <param name="probs">The prediction results, should have the same # of rows as the <paramref name="samples"/></param>
      /// <param name="result">The result.</param>
      [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      internal static extern void CvEMPredict(
         IntPtr model,
         IntPtr samples,
         ref MCvPoint2D64f result,
         IntPtr probs);
      #endregion


      #region CvSVM

      /// <summary>
      /// Create a default SVM model
      /// </summary>
      /// <returns>Pointer to the SVM model</returns>
      [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      internal static extern IntPtr CvSVMDefaultCreate(ref IntPtr statModel, ref IntPtr algorithm);

      /// <summary>
      /// Release the SVM model and all the memory associated to ir
      /// </summary>
      /// <param name="model">The SVM model to be released</param>
      [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      internal static extern void CvSVMRelease(ref IntPtr model);


      /// <summary>
      /// Get the default parameter grid for the specific SVM type
      /// </summary>
      /// <param name="type">The SVM type</param>
      /// <param name="grid">The parameter grid reference, values will be filled in by the function call</param>
      [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      internal static extern void CvSVMGetDefaultGrid(SVM.ParamType type, ref MCvParamGrid grid);

      /// <summary>
      /// The method trains the SVM model automatically by choosing the optimal parameters C, gamma, p, nu, coef0, degree from CvSVMParams. By the optimality one mean that the cross-validation estimate of the test set error is minimal. 
      /// </summary>
      /// <param name="model">The SVM model</param>
      /// <param name="trainData">The training data.</param>
      /// <param name="kFold">Cross-validation parameter. The training set is divided into k_fold subsets, one subset being used to train the model, the others forming the test set. So, the SVM algorithm is executed k_fold times</param>
      /// <param name="cGrid">cGrid</param>
      /// <param name="gammaGrid">gammaGrid</param>
      /// <param name="pGrid">pGrid</param>
      /// <param name="nuGrid">nuGrid</param>
      /// <param name="coefGrid">coedGrid</param>
      /// <param name="degreeGrid">degreeGrid</param>
      /// <param name="balanced">If true and the problem is 2-class classification then the method creates more balanced cross-validation subsets that is proportions between classes in subsets are close to such proportion in the whole train dataset.</param>
      /// <returns></returns>
      [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      [return: MarshalAs(CvInvoke.BoolMarshalType)]
      internal static extern bool CvSVMTrainAuto(
         IntPtr model,
         IntPtr trainData,
         int kFold,
         ref MCvParamGrid cGrid,
         ref MCvParamGrid gammaGrid,
         ref MCvParamGrid pGrid,
         ref MCvParamGrid nuGrid,
         ref MCvParamGrid coefGrid,
         ref MCvParamGrid degreeGrid,
         [MarshalAs(CvInvoke.BoolMarshalType)]
         bool balanced);

      /// <summary>
      /// The method retrieves a given support vector
      /// </summary>
      /// <param name="model">The SVM model</param>
      /// <param name="supportVectors">The output support vectors</param>
      [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      internal static extern void CvSVMGetSupportVectors(IntPtr model, IntPtr supportVectors);
      #endregion

      #region Decision tree

      /*
      /// <summary>
      /// Create default parameters for CvDTreeParams
      /// </summary>
      /// <returns>Pointer to the default CvDTreeParams</returns>
      [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      internal static extern IntPtr CvDTreeParamsCreate(
         int maxDepth, int minSampleCount,
         double regressionAccuracy,
         [MarshalAs(CvInvoke.BoolMarshalType)]
         bool useSurrogates,
         int maxCategories,
         int CvFolds,
         [MarshalAs(CvInvoke.BoolMarshalType)]
         bool use1SERule,
         [MarshalAs(CvInvoke.BoolMarshalType)]
         bool truncatePrunedTree,
         IntPtr priors);

      /// <summary>
      /// Release the CvDTreeParams
      /// </summary>
      /// <param name="dTreeParam">Pointer to the decision tree parameters to be released</param>
      [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      internal static extern void CvDTreeParamsRelease(ref IntPtr dTreeParam);
      */
      /// <summary>
      /// Create a default decision tree
      /// </summary>
      /// <returns>Pointer to the decision tree</returns>
      [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      internal static extern IntPtr cveDTreesCreate(ref IntPtr statModel, ref IntPtr algorithm);

      /// <summary>
      /// Release the decision tree model
      /// </summary>
      /// <param name="model">The decision tree model to be released</param>
      [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      internal static extern void cveDTreesRelease(ref IntPtr model);
      #endregion

      #region Random tree
      /*
      /// <summary>
      /// Create default parameters for CvRTParams
      /// </summary>
      /// <returns>Pointer to the default CvRTParams</returns>
      [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      internal static extern IntPtr CvRTParamsCreate(
         int maxDepth, int minSampleCount,
         double regressionAccuracy,
         [MarshalAs(CvInvoke.BoolMarshalType)]
         bool useSurrogates,
         int maxCategories, IntPtr priors,
         [MarshalAs(CvInvoke.BoolMarshalType)]
         bool calcVarImportance,
         int nactiveVars,
         ref MCvTermCriteria termCrit);

      /// <summary>
      /// Release the CvRTParams
      /// </summary>
      /// <param name="rTreesParam">Pointer to the random tree parameters to be released</param>
      [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      internal static extern void CvRTParamsRelease(ref IntPtr rTreesParam);
      */
      /// <summary>
      /// Create a default random tree
      /// </summary>
      /// <returns>Pointer to the random tree</returns>
      [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      internal static extern IntPtr cveRTreesCreate(ref IntPtr statModel, ref IntPtr algorithm);

      /// <summary>
      /// Release the random tree model
      /// </summary>
      /// <param name="model">The random tree model to be released</param>
      [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      internal static extern void cveRTreesRelease(ref IntPtr model);

      /*
      /// <summary>
      /// Get the number of Trees in the Random tree
      /// </summary>
      /// <param name="model">The random tree</param>
      /// <returns>The number of Trees in the Random tree</returns>
      [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      public static extern int CvRTreesGetTreeCount(IntPtr model);

      /// <summary>
      /// Get the variable importance
      /// </summary>
      /// <param name="model">The randome tree</param>
      /// <returns>Pointer to the matrix that represents the variable importance</returns>
      [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      public static extern IntPtr CvRTreesGetVarImportance(IntPtr model);*/
      #endregion

      #region Boost
      /*
      /// <summary>
      /// Create default parameters for CvBoost
      /// </summary>
      /// <returns>Pointer to the default CvBoostParams</returns>
      [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      internal static extern IntPtr CvBoostParamsCreate(
         Boost.Type boostType, int weakCount, double weightTrimRate,
         int maxDepth,
         [MarshalAs(CvInvoke.BoolMarshalType)]
         bool useSurrogates, IntPtr priors);

      /// <summary>
      /// Release the CvBoostParams
      /// </summary>
      /// <param name="bTreeParam">Pointer to the boost tree parameters to be released</param>
      [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      internal static extern void CvBoostParamsRelease(ref IntPtr bTreeParam);
      */
      /// <summary>
      /// Create a default boost classifier
      /// </summary>
      /// <returns>Pointer to the boost classifier</returns>
      [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      internal static extern IntPtr cveBoostCreate(ref IntPtr statModel, ref IntPtr algorithm);

      /// <summary>
      /// Release the boost classifier
      /// </summary>
      /// <param name="model">The boost classifier to be released</param>
      [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      internal static extern void cveBoostRelease(ref IntPtr model);
      #endregion

      /*
      #region GBTree
      /// <summary>
      /// Train the boost tree using the specific traning data
      /// </summary>
      /// <param name="model">The Boost Tree model</param>
      /// <param name="tFlag">The data layout type of the train data</param>
      /// <param name="missingMask">Can be IntPtr.Zero if not needed. When specified, it is an 8-bit matrix of the same size as <paramref name="trainData"/>, is used to mark the missed values (non-zero elements of the mask)</param>
      /// <param name="trainData">The training data. A 32-bit floating-point, single-channel matrix, one vector per row</param>
      /// <param name="responses">A floating-point matrix of the corresponding output vectors, one vector per row. </param>
      /// <param name="sampleIdx">Can be IntPtr.Zero if not needed. When specified, identifies samples of interest. It is a Matrix&gt;int&lt; of nx1</param>
      /// <param name="param">The parameters for training the random tree</param>
      /// <param name="varIdx">Can be IntPtr.Zero if not needed. When specified, identifies variables (features) of interest. It is a Matrix&gt;int&lt; of nx1</param>
      /// <param name="varType">The types of input variables</param>
      /// <param name="update">specifies whether the classifier needs to be updated (i.e. the new weak tree classifiers added to the existing ensemble), or the classifier needs to be rebuilt from scratch</param>
      /// <returns></returns>
      [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      [return: MarshalAs(CvInvoke.BoolMarshalType)]
      public static extern bool CvGBTreesTrain(
         IntPtr model,
         IntPtr trainData,
         MlEnum.DataLayoutType tFlag,
         IntPtr responses,
         IntPtr varIdx,
         IntPtr sampleIdx,
         IntPtr varType,
         IntPtr missingMask,
         ref MCvGBTreesParams param,
         [MarshalAs(CvInvoke.BoolMarshalType)]
         bool update);

      /// <summary>
      /// Runs the sample through the trees in the ensemble and returns the output class label based on the weighted voting
      /// </summary>
      /// <param name="model">The Boost Tree model</param>
      /// <param name="sample">The input sample</param>
      /// <param name="missing">Can be IntPtr.Zero if not needed. The optional mask of missing measurements. To handle missing measurements, the weak classifiers must include surrogate splits</param>
      /// <param name="weakResponses">Can be IntPtr.Zero if not needed. a floating-point vector, of responses from each individual weak classifier. The number of elements in the vector must be equal to the slice length.</param>
      /// <param name="slice">The continuous subset of the sequence of weak classifiers to be used for prediction</param>
      /// <param name="rawMode">Normally set to false that implies a regular input. If it is true, the method assumes that all the values of the discrete input variables have been already normalized to 0..num_of_categoriesi-1 ranges. (as the decision tree uses such normalized representation internally). It is useful for faster prediction with tree ensembles. For ordered input variables the flag is not used. </param>      
      /// <returns>The output class label based on the weighted voting</returns>
      [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      public static extern float CvGBTreesPredict(
         IntPtr model,
         IntPtr sample,
         IntPtr missing,
         IntPtr weakResponses,
         ref Emgu.CV.Structure.MCvSlice slice,
         [MarshalAs(CvInvoke.BoolMarshalType)]
         bool rawMode);

      /// <summary>
      /// Create a default Gradient Boosting Trees (GBT)
      /// </summary>
      /// <returns>Pointer to the Gradient Boosting Trees (GBT)</returns>
      [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      public static extern IntPtr CvGBTreesCreate();

      /// <summary>
      /// Release the Gradient Boosting Trees (GBT)
      /// </summary>
      /// <param name="model">The Gradient Boosting Trees (GBT) to be released</param>
      [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      public static extern void CvGBTreesRelease(ref IntPtr model);
      #endregion
      */

      #region Logistic Regression
      [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      internal static extern IntPtr cveLogisticRegressionCreate(ref IntPtr statModel, ref IntPtr algorithm);

      [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      internal static extern void cveLogisticRegressionRelease(ref IntPtr model);

      #endregion
   }
}
