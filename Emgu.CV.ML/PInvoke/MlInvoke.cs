//----------------------------------------------------------------------------
//  Copyright (C) 2004-2011 by EMGU. All rights reserved.       
//----------------------------------------------------------------------------

using System;
using System.Runtime.InteropServices;
using Emgu.CV.ML.Structure;
using Emgu.CV;

namespace Emgu.CV.ML
{
   /// <summary>
   /// This class contains functions to call into machine learning library
   /// </summary>
   public class MlInvoke
   {
      #region CvStatModel
      /// <summary>
      /// Save the statistic model to the specific file
      /// </summary>
      /// <param name="statModel">The statistic model to save</param>
      /// <param name="fileName">The file name to save to</param>
      /// <param name="name">Pass IntPtr.Zero</param>
      [DllImport(CvInvoke.EXTERN_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      public static extern void StatModelSave(IntPtr statModel, String fileName, IntPtr name);

      /// <summary>
      /// Load the statistic model from the specific file
      /// </summary>
      /// <param name="statModel">The statistic model to save</param>
      /// <param name="fileName">The file name to load from</param>
      /// <param name="name">Pass IntPtr.Zero</param>
      [DllImport(CvInvoke.EXTERN_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      public static extern void StatModelLoad(IntPtr statModel, String fileName, IntPtr name);

      /// <summary>
      /// Clear the statistic model
      /// </summary>
      /// <param name="statModel">The model to be cleared</param>
      [DllImport(CvInvoke.EXTERN_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      public static extern void StatModelClear(IntPtr statModel);
      #endregion

      #region CvNormalBayesClassifier
      /// <summary>
      /// Create a normal bayes classifier
      /// </summary>
      /// <returns>The normal bayes classifier</returns>
      [DllImport(CvInvoke.EXTERN_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      public static extern IntPtr CvNormalBayesClassifierDefaultCreate();

      /// <summary>
      /// Create a normal Bayes classifier using the specific training data
      /// </summary>
      /// <param name="trainData">The training data. A 32-bit floating-point, single-channel matrix, one vector per row</param>
      /// <param name="responses">A floating-point matrix of the corresponding output vectors, one vector per row. </param>
      /// <param name="varIdx">Can be IntPtr.Zero if not needed. When specified, identifies variables (features) of interest. It is a Matrix&gt;int&lt; of nx1</param>
      /// <param name="sampleIdx">Can be IntPtr.Zero if not needed. When specified, identifies samples of interest. It is a Matrix&gt;int&lt; of nx1</param>
      /// <returns>The normal Beyes classifier</returns>
      [DllImport(CvInvoke.EXTERN_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      public static extern IntPtr CvNormalBayesClassifierCreate(
         IntPtr trainData,
         IntPtr responses,
         IntPtr varIdx,
         IntPtr sampleIdx);

      /// <summary>
      /// Release the memory associated with the bayes classifier
      /// </summary>
      /// <param name="classifier">The classifier to release</param>
      [DllImport(CvInvoke.EXTERN_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      public static extern void CvNormalBayesClassifierRelease(IntPtr classifier);

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
      [DllImport(CvInvoke.EXTERN_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
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
      [DllImport(CvInvoke.EXTERN_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      public static extern float CvNormalBayesClassifierPredict(IntPtr model, IntPtr samples, IntPtr results);
      #endregion

      #region CvKNearest
      /// <summary>
      /// Create a KNearest classifier
      /// </summary>
      /// <returns>The KNearest classifier</returns>
      [DllImport(CvInvoke.EXTERN_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      public static extern IntPtr CvKNearestDefaultCreate();

      /// <summary>
      /// Release the KNearest classifer
      /// </summary>
      /// <param name="knearest">The classifier to release</param>
      [DllImport(CvInvoke.EXTERN_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      public static extern void CvKNearestRelease(IntPtr knearest);

      /// <summary>
      /// Create the KNearest classifier using the specific traing data.
      /// </summary>
      /// <param name="isRegression">Specify the output variables type. It can be either categorical (isRegression=false) or ordered (isRegression=true)</param>
      /// <param name="maxK">The number of maximum neighbors that may be passed to the method findNearest.</param>
      /// <param name="trainData">The training data. A 32-bit floating-point, single-channel matrix, one vector per row</param>
      /// <param name="responses">A floating-point matrix of the corresponding output vectors, one vector per row. </param>
      /// <param name="sampleIdx">Can be IntPtr.Zero if not needed. When specified, identifies samples of interest. It is a Matrix&gt;int&lt; of nx1</param>
      /// <returns>The KNearest classifier</returns>
      [DllImport(CvInvoke.EXTERN_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
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
      [DllImport(CvInvoke.EXTERN_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
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
      [DllImport(CvInvoke.EXTERN_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      public static extern float CvKNearestFindNearest(
         IntPtr classifier,
         IntPtr samples,
         int k,
         IntPtr results,
         IntPtr[] kNearestNeighbors,
         IntPtr neighborResponses,
         IntPtr dist);
      #endregion

      #region CvEM
      /// <summary>
      /// Create a default EM model
      /// </summary>
      /// <returns>Pointer to the EM model</returns>
      [DllImport(CvInvoke.EXTERN_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      public static extern IntPtr CvEMDefaultCreate();

      /// <summary>
      /// Release the EM model
      /// </summary>
      [DllImport(CvInvoke.EXTERN_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      public static extern void CvEMRelease(IntPtr emModel);

      /// <summary>
      /// Train the EM model using the specific training data
      /// </summary>
      /// <param name="model">The EM model</param>
      /// <param name="samples">The training data. A 32-bit floating-point, single-channel matrix, one vector per row</param>
      /// <param name="sampleIdx">Can be IntPtr.Zero if not needed. When specified, identifies samples of interest. It is a Matrix&gt;int&lt; of nx1</param>
      /// <param name="parameters">The parameters for EM</param>
      /// <param name="labels">Can be IntPtr.Zero if not needed. Optionally computed output "class label" for each sample</param>
      /// <returns></returns>
      [DllImport(CvInvoke.EXTERN_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      [return: MarshalAs(CvInvoke.BoolMarshalType)]
      public static extern bool CvEMTrain(
         IntPtr model,
         IntPtr samples,
         IntPtr sampleIdx,
         MCvEMParams parameters,
         IntPtr labels);

      /// <summary>
      /// Given the EM <paramref name="model"/>, predit the probability of the <paramref name="samples"/>
      /// </summary>
      /// <param name="model">The EM model</param>
      /// <param name="samples">The input samples</param>
      /// <param name="probs">The prediction results, should have the same # of rows as the <paramref name="samples"/></param>
      /// <returns>In case of classification the method returns the class label, in case of regression - the output function value</returns>
      [DllImport(CvInvoke.EXTERN_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      public static extern float CvEMPredict(
         IntPtr model,
         IntPtr samples,
         IntPtr probs);

      /// <summary>
      /// Get the means of the clusters from the EM model
      /// </summary>
      /// <param name="model">The EM model</param>
      /// <returns>The means of the clusters of the EM model</returns>
      [DllImport(CvInvoke.EXTERN_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      public static extern IntPtr CvEMGetMeans(IntPtr model);

      /// <summary>
      /// Get the covariance matrices of the clusters from the EM model
      /// </summary>
      /// <param name="model">The EM model</param>
      /// <returns>The covariance matrices of the clusters of the EM model</returns>
      [DllImport(CvInvoke.EXTERN_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      public static extern IntPtr CvEMGetCovs(IntPtr model);

      /// <summary>
      /// Get the weights of the clusters from the EM model
      /// </summary>
      /// <param name="model">The EM model</param>
      /// <returns>The weights of the clusters of the EM model</returns>
      [DllImport(CvInvoke.EXTERN_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      public static extern IntPtr CvEMGetWeights(IntPtr model);

      /// <summary>
      /// Get the probabilities from the EM model
      /// </summary>
      /// <param name="model">The EM model</param>
      /// <returns>The probabilities of the EM model </returns>
      [DllImport(CvInvoke.EXTERN_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      public static extern IntPtr CvEMGetProbs(IntPtr model);

      /// <summary>
      /// Get the number of clusters from the EM model
      /// </summary>
      /// <param name="model">The EM model</param>
      /// <returns>The number of clusters of the EM model</returns>
      [DllImport(CvInvoke.EXTERN_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      public static extern int CvEMGetNclusters(IntPtr model);
      #endregion

      #region CvSVM
      /// <summary>
      /// Create a default SVM model
      /// </summary>
      /// <returns>Pointer to the SVM model</returns>
      [DllImport(CvInvoke.EXTERN_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      public static extern IntPtr CvSVMDefaultCreate();

      /// <summary>
      /// Release the SVM model and all the memory associated to ir
      /// </summary>
      /// <param name="model">The SVM model to be released</param>
      [DllImport(CvInvoke.EXTERN_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      public static extern void CvSVMRelease(IntPtr model);

      /// <summary>
      /// Train the SVM model with the specific paramters
      /// </summary>
      /// <param name="model">The SVM model</param>
      /// <param name="trainData">The training data. A 32-bit floating-point, single-channel matrix</param>
      /// <param name="responses">The response for the training data. It's usually a 32-bit floating point matrix; In classification problem, it can be an Int32 matrix.</param>
      /// <param name="varIdx">Can be IntPtr.Zero if not needed. When specified, identifies variables (features) of interest. It is a Matrix&gt;int&lt; of nx1</param>
      /// <param name="sampleIdx">Can be IntPtr.Zero if not needed. When specified, identifies samples of interest. It is a Matrix&gt;int&lt; of nx1</param>
      /// <param name="parameters">The parameters for SVM</param>
      /// <returns></returns>
      [DllImport(CvInvoke.EXTERN_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      [return: MarshalAs(CvInvoke.BoolMarshalType)]
      public static extern bool CvSVMTrain(
         IntPtr model,
         IntPtr trainData,
         IntPtr responses,
         IntPtr varIdx,
         IntPtr sampleIdx,
         MCvSVMParams parameters);

      /// <summary>
      /// Get the default parameter grid for the specific SVM type
      /// </summary>
      /// <param name="type">The SVM type</param>
      /// <param name="grid">The parameter grid reference, values will be filled in by the funtion call</param>
      [DllImport(CvInvoke.EXTERN_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      public static extern void CvSVMGetDefaultGrid(MlEnum.SVM_PARAM_TYPE type, ref MCvParamGrid grid);

      /// <summary>
      /// The method trains the SVM model automatically by choosing the optimal parameters C, gamma, p, nu, coef0, degree from CvSVMParams. By the optimality one mean that the cross-validation estimate of the test set error is minimal. 
      /// </summary>
      /// <param name="model">The SVM model</param>
      /// <param name="trainData">The training data.</param>
      /// <param name="responses">The response for the training data.</param>
      /// <param name="varIdx">Can be null if not needed. When specified, identifies variables (features) of interest. It is a Matrix&gt;int&lt; of nx1</param>
      /// <param name="sampleIdx">Can be null if not needed. When specified, identifies samples of interest. It is a Matrix&gt;int&lt; of nx1</param>
      /// <param name="parameters">The parameters for SVM</param>
      /// <param name="kFold">Cross-validation parameter. The training set is divided into k_fold subsets, one subset being used to train the model, the others forming the test set. So, the SVM algorithm is executed k_fold times</param>
      /// <param name="cGrid">cGrid</param>
      /// <param name="gammaGrid">gammaGrid</param>
      /// <param name="pGrid">pGrid</param>
      /// <param name="nuGrid">nuGrid</param>
      /// <param name="coefGrid">coedGrid</param>
      /// <param name="degreeGrid">degreeGrid</param>
      /// <returns></returns>
      [DllImport(CvInvoke.EXTERN_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      [return: MarshalAs(CvInvoke.BoolMarshalType)]
      public static extern bool CvSVMTrainAuto(
         IntPtr model,
         IntPtr trainData,
         IntPtr responses,
         IntPtr varIdx,
         IntPtr sampleIdx,
         MCvSVMParams parameters,
         int kFold,
         MCvParamGrid cGrid,
         MCvParamGrid gammaGrid,
         MCvParamGrid pGrid,
         MCvParamGrid nuGrid,
         MCvParamGrid coefGrid,
         MCvParamGrid degreeGrid);

      /// <summary>
      /// Get the parameters of the SVM model
      /// </summary>
      /// <param name="model">The SVM model</param>
      /// <param name="param">The parameters of the SVM model</param>
      [DllImport(CvInvoke.EXTERN_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      public static extern void CvSVMGetParameters(IntPtr model, ref MCvSVMParams param);

      #region contribution from Albert G
      /// <summary>
      /// Predicts response for the input sample.
      /// </summary>
      /// <param name="model">The SVM model</param>
      /// <param name="_sample">The input sample</param>
      [DllImport(CvInvoke.EXTERN_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      public static extern float CvSVMPredict(IntPtr model, IntPtr _sample);

      /// <summary>
      /// The method retrieves a given support vector
      /// </summary>
      /// <param name="model">The SVM model</param>
      /// <param name="i">The index of the support vector</param>
      [DllImport(CvInvoke.EXTERN_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      public static extern IntPtr CvSVMGetSupportVector(IntPtr model, int i);

      /// <summary>
      /// The method retrieves the number of support vectors
      /// </summary>
      /// <param name="model">The SVM model</param>
      [DllImport(CvInvoke.EXTERN_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      public static extern int CvSVMGetSupportVectorCount(IntPtr model);

      /// <summary>
      /// The method retrieves the number of vars
      /// </summary>
      /// <param name="model">The SVM model</param>
      [DllImport(CvInvoke.EXTERN_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      public static extern int CvSVMGetVarCount(IntPtr model);
      #endregion
      #endregion

      #region CvANN_MLP
      /// <summary>
      /// Create a neural network using the specific parameters
      /// </summary>
      /// <param name="layerSizes">The size of the layer</param>
      /// <param name="activeFunction">Activation function</param>
      /// <param name="fParam1">Free parameters of the activation function, alpha</param>
      /// <param name="fParam2">Free parameters of the activation function, beta</param>
      /// <returns>The nearual network created</returns>
      [DllImport(CvInvoke.EXTERN_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      public static extern IntPtr CvANN_MLPCreate(
         IntPtr layerSizes,
         MlEnum.ANN_MLP_ACTIVATION_FUNCTION activeFunction,
         double fParam1,
         double fParam2);

      /// <summary>
      /// Release the ANN_MLP model
      /// </summary>
      /// <param name="model">The ANN_MLP model to be released</param>
      [DllImport(CvInvoke.EXTERN_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      public static extern void CvANN_MLPRelease(IntPtr model);

      /// <summary>
      /// Train the ANN_MLP model with the specific paramters
      /// </summary>
      /// <param name="model">The ANN_MLP model</param>
      /// <param name="trainData">The training data. A 32-bit floating-point, single-channel matrix, one vector per row</param>
      /// <param name="responses">A floating-point matrix of the corresponding output vectors, one vector per row. </param>
      /// <param name="sampleWeights">(RPROP only) The optional floating-point vector of weights for each sample. Some samples may be more important than others for training, e.g. user may want to gain the weight of certain classes to find the right balance between hit-rate and false-alarm rate etc</param>
      /// <param name="sampleIdx">Can be IntPtr.Zero if not needed. When specified, identifies samples of interest. It is a Matrix&gt;int&lt; of nx1</param>
      /// <param name="parameters">The parameters for ANN_MLP</param>
      /// <param name="flags">The traning flag</param>
      /// <returns>The number of done iterations</returns>
      [DllImport(CvInvoke.EXTERN_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      public static extern int CvANN_MLPTrain(
         IntPtr model,
         IntPtr trainData,
         IntPtr responses,
         IntPtr sampleWeights,
         IntPtr sampleIdx,
         ref MCvANN_MLP_TrainParams parameters,
         MlEnum.ANN_MLP_TRAINING_FLAG flags);

      /// <summary>
      /// Given the <paramref name="model"/>, predit the <paramref name="outputs"/> response of the <paramref name="inputs"/> samples
      /// </summary>
      /// <param name="model">The ANN_MLP model</param>
      /// <param name="inputs">The input samples</param>
      /// <param name="outputs">The prediction results, should have the same # of rows as the inputs</param>
      /// <returns>In case of classification the method returns the class label, in case of regression - the output function value</returns>
      [DllImport(CvInvoke.EXTERN_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      public static extern float CvANN_MLPPredict(
         IntPtr model,
         IntPtr inputs,
         IntPtr outputs);

      /// <summary>
      /// Get the number of layers in the ANN_MLP
      /// </summary>
      /// <param name="model">The ANN_MLP model</param>
      [DllImport(CvInvoke.EXTERN_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      public static extern int CvANN_MLPGetLayerCount(IntPtr model);
      #endregion

      #region Decision tree
      /// <summary>
      /// Create default parameters for CvDTreeParams
      /// </summary>
      /// <returns>Pointer to the default CvDTreeParams</returns>
      [DllImport(CvInvoke.EXTERN_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      public static extern IntPtr CvDTreeParamsCreate();

      /// <summary>
      /// Release the CvDTreeParams
      /// </summary>
      /// <param name="dTreeParam">Pointer to the decision tree parameters to be released</param>
      [DllImport(CvInvoke.EXTERN_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      public static extern void CvDTreeParamsRelease(IntPtr dTreeParam);

      /// <summary>
      /// Create a default decision tree
      /// </summary>
      /// <returns>Pointer to the decision tree</returns>
      [DllImport(CvInvoke.EXTERN_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      public static extern IntPtr CvDTreeCreate();

      /// <summary>
      /// Release the decision tree model
      /// </summary>
      /// <param name="model">The decision tree model to be released</param>
      [DllImport(CvInvoke.EXTERN_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      public static extern void CvDTreeRelease(IntPtr model);

      /// <summary>
      /// Train the decision tree using the specific training data
      /// </summary>
      /// <param name="model">The Decision Tree model</param>
      /// <param name="tflag">The data layout type of the train data</param>
      /// <param name="varIdx">Can be IntPtr.Zero if not needed. When specified, identifies variables (features) of interest. It is a Matrix&gt;int&lt; of nx1</param>
      /// <param name="varType">The types of input variables</param>
      /// <param name="missingMask">Can be IntPtr.Zero if not needed. When specified, it is an 8-bit matrix of the same size as <paramref name="trainData"/>, is used to mark the missed values (non-zero elements of the mask)</param>
      /// <param name="trainData">The training data. A 32-bit floating-point, single-channel matrix, one vector per row</param>
      /// <param name="responses">A floating-point matrix of the corresponding output vectors, one vector per row. </param>
      /// <param name="sampleIdx">Can be IntPtr.Zero if not needed. When specified, identifies samples of interest. It is a Matrix&gt;int&lt; of nx1</param>
      /// <param name="param">The parameters for training the decision tree</param>
      /// <returns></returns>
      [DllImport(CvInvoke.EXTERN_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      [return: MarshalAs(CvInvoke.BoolMarshalType)]
      public static extern bool CvDTreeTrain(
         IntPtr model,
         IntPtr trainData,
         MlEnum.DATA_LAYOUT_TYPE tflag,
         IntPtr responses,
         IntPtr varIdx,
         IntPtr sampleIdx,
         IntPtr varType,
         IntPtr missingMask,
         MCvDTreeParams param);

      /// <summary>
      /// The method takes the feature vector and the optional missing measurement mask on input, traverses the decision tree and returns the reached leaf node on output. The prediction result, either the class label or the estimated function value, may be retrieved as value field of the CvDTreeNode structure
      /// </summary>
      /// <param name="model">The decision tree model</param>
      /// <param name="sample">The sample to be predicted</param>
      /// <param name="missingDataMask">Can be IntPtr.Zero if not needed. When specified, it is an 8-bit matrix of the same size as <i>trainData</i>, is used to mark the missed values (non-zero elements of the mask)</param>
      /// <param name="rawMode">Normally set to false that implies a regular input. If it is true, the method assumes that all the values of the discrete input variables have been already normalized to 0..num_of_categoriesi-1 ranges. (as the decision tree uses such normalized representation internally). It is useful for faster prediction with tree ensembles. For ordered input variables the flag is not used. </param>
      /// <returns>Pointer to the reached leaf node on output. The prediction result, either the class label or the estimated function value, may be retrieved as value field of the CvDTreeNode structure</returns>
      [DllImport(CvInvoke.EXTERN_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      public static extern IntPtr CvDTreePredict(
         IntPtr model,
         IntPtr sample,
         IntPtr missingDataMask,
         [MarshalAs(CvInvoke.BoolMarshalType)]
         bool rawMode);
      #endregion

      #region Random tree
      /// <summary>
      /// Create default parameters for CvRTParams
      /// </summary>
      /// <returns>Pointer to the default CvRTParams</returns>
      [DllImport(CvInvoke.EXTERN_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      public static extern IntPtr CvRTParamsCreate();

      /// <summary>
      /// Release the CvRTParams
      /// </summary>
      /// <param name="rTreesParam">Pointer to the random tree parameters to be released</param>
      [DllImport(CvInvoke.EXTERN_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      public static extern void CvRTParamsRelease(IntPtr rTreesParam);

      /// <summary>
      /// Create a default random tree
      /// </summary>
      /// <returns>Pointer to the random tree</returns>
      [DllImport(CvInvoke.EXTERN_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      public static extern IntPtr CvRTreesCreate();

      /// <summary>
      /// Release the random tree model
      /// </summary>
      /// <param name="model">The random tree model to be released</param>
      [DllImport(CvInvoke.EXTERN_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      public static extern void CvRTreesRelease(IntPtr model);

      /// <summary>
      /// Get the number of Trees in the Random tree
      /// </summary>
      /// <param name="model">The random tree</param>
      /// <returns>The number of Trees in the Random tree</returns>
      [DllImport(CvInvoke.EXTERN_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      public static extern int CvRTreesGetTreeCount(IntPtr model);

      /// <summary>
      /// Train the random tree using the specific traning data
      /// </summary>
      /// <param name="model">The Random Tree model</param>
      /// <param name="tFlag">The data layout type of the train data</param>
      /// <param name="missingMask">Can be IntPtr.Zero if not needed. When specified, it is an 8-bit matrix of the same size as <paramref name="trainData"/>, is used to mark the missed values (non-zero elements of the mask)</param>
      /// <param name="trainData">The training data. A 32-bit floating-point, single-channel matrix, one vector per row</param>
      /// <param name="responses">A floating-point matrix of the corresponding output vectors, one vector per row. </param>
      /// <param name="sampleIdx">Can be IntPtr.Zero if not needed. When specified, identifies samples of interest. It is a Matrix&lt;int&gt; of nx1</param>
      /// <param name="param">The parameters for training the random tree</param>
      /// <param name="varIdx">Can be IntPtr.Zero if not needed. When specified, identifies variables (features) of interest. It is a Matrix&lt;int&gt; of nx1</param>
      /// <param name="varType">The types of input variables</param>
      /// <returns></returns>
      [DllImport(CvInvoke.EXTERN_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      [return: MarshalAs(CvInvoke.BoolMarshalType)]
      public static extern bool CvRTreesTrain(
         IntPtr model,
         IntPtr trainData,
         MlEnum.DATA_LAYOUT_TYPE tFlag,
         IntPtr responses,
         IntPtr varIdx,
         IntPtr sampleIdx,
         IntPtr varType,
         IntPtr missingMask,
         MCvRTParams param);

      /// <summary>
      /// The method takes the feature vector and the optional missing measurement mask on input, traverses the random tree and returns the cumulative result from all the trees in the forest (the class that receives the majority of voices, or the mean of the regression function estimates)
      /// </summary>
      /// <param name="model">The decision tree model</param>
      /// <param name="sample">The sample to be predicted</param>
      /// <param name="missingDataMask">Can be IntPtr.Zero if not needed. When specified, it is an 8-bit matrix of the same size as <i>trainData</i>, is used to mark the missed values (non-zero elements of the mask)</param>
      /// <returns>The cumulative result from all the trees in the forest (the class that receives the majority of voices, or the mean of the regression function estimates)</returns>
      [DllImport(CvInvoke.EXTERN_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      public static extern float CvRTreesPredict(
         IntPtr model,
         IntPtr sample,
         IntPtr missingDataMask);

      /// <summary>
      /// Get the variable importance
      /// </summary>
      /// <param name="model">The randome tree</param>
      /// <returns>Pointer to the matrix that represents the variable importance</returns>
      [DllImport(CvInvoke.EXTERN_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      public static extern IntPtr CvRTreesGetVarImportance(IntPtr model);
      #endregion

      #region Extreme Random tree
      /// <summary>
      /// Create a default extreme random tree
      /// </summary>
      /// <returns>Pointer to the extreme random tree</returns>
      [DllImport(CvInvoke.EXTERN_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      public static extern IntPtr CvERTreesCreate();

      /// <summary>
      /// Release the extreme random tree model
      /// </summary>
      /// <param name="model">The extreme random tree model to be released</param>
      [DllImport(CvInvoke.EXTERN_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      public static extern void CvERTreesRelease(IntPtr model);
      #endregion

      #region Boost
      /// <summary>
      /// Create default parameters for CvBoost
      /// </summary>
      /// <returns>Pointer to the default CvBoostParams</returns>
      [DllImport(CvInvoke.EXTERN_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      public static extern IntPtr CvBoostParamsCreate();

      /// <summary>
      /// Release the CvBoostParams
      /// </summary>
      /// <param name="bTreeParam">Pointer to the boost tree parameters to be released</param>
      [DllImport(CvInvoke.EXTERN_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      public static extern void CvBoostParamsRelease(IntPtr bTreeParam);

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
      [DllImport(CvInvoke.EXTERN_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      [return: MarshalAs(CvInvoke.BoolMarshalType)]
      public static extern bool CvBoostTrain(
         IntPtr model,
         IntPtr trainData,
         MlEnum.DATA_LAYOUT_TYPE tFlag,
         IntPtr responses,
         IntPtr varIdx,
         IntPtr sampleIdx,
         IntPtr varType,
         IntPtr missingMask,
         MCvBoostParams param,
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
      [DllImport(CvInvoke.EXTERN_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      public static extern float CvBoostPredict(
         IntPtr model,
         IntPtr sample,
         IntPtr missing,
         IntPtr weakResponses,
         Emgu.CV.Structure.MCvSlice slice,
         [MarshalAs(CvInvoke.BoolMarshalType)]
         bool rawMode);

      /// <summary>
      /// Create a default boost classicfier
      /// </summary>
      /// <returns>Pointer to the boost classicfier</returns>
      [DllImport(CvInvoke.EXTERN_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      public static extern IntPtr CvBoostCreate();

      /// <summary>
      /// Release the boost classicfier
      /// </summary>
      /// <param name="model">The boost classicfier to be released</param>
      [DllImport(CvInvoke.EXTERN_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      public static extern void CvBoostRelease(IntPtr model);
      #endregion
   }
}
