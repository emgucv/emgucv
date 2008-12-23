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

      /// <summary>
      /// Create a normal Bayes classifier using the specific training data
      /// </summary>
      /// <param name="trainData">The training data. A 32-bit floating-point, single-channel matrix, one vector per row</param>
      /// <param name="responses">A floating-point matrix of the corresponding output vectors, one vector per row. </param>
      /// <param name="varIdx">Can be IntPtr.Zero if not needed. When specified, identifies variables (features) of interest. It is a Matrix&gt;int&lt; of nx1</param>
      /// <param name="sampleIdx">Can be IntPtr.Zero if not needed. When specified, identifies samples of interest. It is a Matrix&gt;int&lt; of nx1</param>
      /// <returns>The normal Beyes classifier</returns>
      [DllImport(EXTERN_LIBRARY)]
      public static extern IntPtr CvNormalBayesClassifierCreate(
         IntPtr trainData, 
         IntPtr responses, 
         IntPtr varIdx, 
         IntPtr sampleIdx);

      /// <summary>
      /// Release the memory associated with the bayes classifier
      /// </summary>
      /// <param name="classifier">The classifier to release</param>
      [DllImport(EXTERN_LIBRARY)]
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
      [DllImport(EXTERN_LIBRARY)]
      public static extern bool CvNormalBayesClassifierTrain(
         IntPtr classifier,
         IntPtr trainData,
         IntPtr responses,
         IntPtr varIdx,
         IntPtr sampleIdx,
         bool update);

      /// <summary>
      /// Given the NormalBayesClassifier <paramref name="model"/>, predit the probability of the <paramref name="samples"/>
      /// </summary>
      /// <param name="model">The NormalBayesClassifier classifier model</param>
      /// <param name="samples">The input samples</param>
      /// <param name="results">The prediction results, should have the same # of rows as the <paramref name="samples"/></param>
      /// <returns>In case of classification the method returns the class label, in case of regression - the output function value</returns>
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

      /// <summary>
      /// Create the KNearest classifier using the specific traing data.
      /// </summary>
      /// <param name="isRegression">Specify the output variables type. It can be either categorical (isRegression=false) or ordered (isRegression=true)</param>
      /// <param name="maxK">The number of maximum neighbors that may be passed to the method findNearest.</param>
      /// <param name="trainData">The training data. A 32-bit floating-point, single-channel matrix, one vector per row</param>
      /// <param name="responses">A floating-point matrix of the corresponding output vectors, one vector per row. </param>
      /// <param name="sampleIdx">Can be IntPtr.Zero if not needed. When specified, identifies samples of interest. It is a Matrix&gt;int&lt; of nx1</param>
      /// <returns>The KNearest classifier</returns>
      [DllImport(EXTERN_LIBRARY)]
      public static extern IntPtr CvKNearestCreate(
         IntPtr trainData,
         IntPtr responses,
         IntPtr sampleIdx,
         bool isRegression,
         int maxK);

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
      [DllImport(EXTERN_LIBRARY)]
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
      /// <param name="model">The EM model</param>
      /// <param name="samples">The training data. A 32-bit floating-point, single-channel matrix, one vector per row</param>
      /// <param name="sampleIdx">Can be IntPtr.Zero if not needed. When specified, identifies samples of interest. It is a Matrix&gt;int&lt; of nx1</param>
      /// <param name="parameters">The parameters for EM</param>
      /// <param name="labels">Can be IntPtr.Zero if not needed. Optionally computed output "class label" for each sample</param>
      /// <returns></returns>
      [DllImport(EXTERN_LIBRARY)]
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
      [DllImport(EXTERN_LIBRARY)]
      public static extern float CvEMPredict(
         IntPtr model, 
         IntPtr samples, 
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
      /// <param name="trainData">The training data. A 32-bit floating-point, single-channel matrix</param>
      /// <param name="responses">The response for the training data. It's usually a 32-bit floating point matrix; In classification problem, it can be an Int32 matrix.</param>
      /// <param name="varIdx">Can be IntPtr.Zero if not needed. When specified, identifies variables (features) of interest. It is a Matrix&gt;int&lt; of nx1</param>
      /// <param name="sampleIdx">Can be IntPtr.Zero if not needed. When specified, identifies samples of interest. It is a Matrix&gt;int&lt; of nx1</param>
      /// <param name="parameters">The parameters for SVM</param>
      /// <returns></returns>
      [DllImport(EXTERN_LIBRARY)]
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
      [DllImport(EXTERN_LIBRARY)]
      public static extern void CvSVMGetDefaultGrid(MlEnum.SVM_TYPE type, ref MCvParamGrid grid);

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
      [DllImport(EXTERN_LIBRARY)]
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
      [DllImport(EXTERN_LIBRARY)]
      public static extern IntPtr CvANN_MLPCreate(
         IntPtr layerSizes,
         MlEnum.ANN_MLP_ACTIVATION_FUNCTION activeFunction,
         double fParam1,
         double fParam2);

      /// <summary>
      /// Release the ANN_MLP model
      /// </summary>
      /// <param name="model">The ANN_MLP model to be released</param>
      [DllImport(EXTERN_LIBRARY)]
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
      [DllImport(EXTERN_LIBRARY)]
      public static extern int CvANN_MLPTrain(
         IntPtr model, 
         IntPtr trainData, 
         IntPtr responses,
         IntPtr sampleWeights, 
         IntPtr sampleIdx,
         MCvANN_MLP_TrainParams parameters,
         MlEnum.ANN_MLP_TRAINING_FLAG flags );

      /// <summary>
      /// Given the <paramref name="model"/>, predit the <paramref name="outputs"/> response of the <paramref name="inputs"/> samples
      /// </summary>
      /// <param name="model">The ANN_MLP model</param>
      /// <param name="inputs">The input samples</param>
      /// <param name="outputs">The prediction results, should have the same # of rows as the inputs</param>
      /// <returns>In case of classification the method returns the class label, in case of regression - the output function value</returns>
      [DllImport(EXTERN_LIBRARY)]
      public static extern float CvANN_MLPPredict(
         IntPtr model,
         IntPtr inputs,
         IntPtr outputs);
      #endregion 
   }
}
