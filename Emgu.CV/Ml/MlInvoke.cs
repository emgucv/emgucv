//----------------------------------------------------------------------------
//  Copyright (C) 2004-2021 by EMGU Corporation. All rights reserved.       
//----------------------------------------------------------------------------

using System;
using System.Runtime.InteropServices;
using Emgu.CV.ML.MlEnum;
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
            CvInvoke.Init();
        }

        #region CvStatModel
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
           IntPtr sampleWeights, IntPtr varType,
           ref IntPtr sharedPtr);

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern void cveTrainDataRelease(ref IntPtr sharedPtr);

        #region CvNormalBayesClassifier
        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern IntPtr cveNormalBayesClassifierDefaultCreate(ref IntPtr statModel, ref IntPtr algorithm, ref IntPtr sharedPtr);

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

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern void cveNormalBayesClassifierRelease(ref IntPtr classifier, ref IntPtr sharedPtr);

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
        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern IntPtr cveKNearestCreate(
           ref IntPtr statModel,
           ref IntPtr algorithm,
           ref IntPtr sharedPtr);

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern void cveKNearestRelease(ref IntPtr sharedPtr);

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern float cveKNearestFindNearest(
            IntPtr classifier,
            IntPtr samples,
            int k,
            IntPtr results,
            IntPtr neighborResponses,
            IntPtr dist);

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
        internal static extern IntPtr cveEMDefaultCreate(ref IntPtr statModel, ref IntPtr algorithm, ref IntPtr sharedPtr);

        /// <summary>
        /// Release the EM model
        /// </summary>
        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern void cveEMRelease(ref IntPtr emModel, ref IntPtr sharedPtr);

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
        internal static extern void cveEMTrainE(
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
        internal static extern void cveEMTrainM(
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
        internal static extern void cveEMPredict(
           IntPtr model,
           IntPtr samples,
           ref MCvPoint2D64f result,
           IntPtr probs);
        #endregion


        #region CvSVM
        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern IntPtr cveSVMDefaultCreate(ref IntPtr statModel, ref IntPtr algorithm, ref IntPtr sharedPtr);

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern void cveSVMRelease(ref IntPtr model, ref IntPtr sharedPtr);

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern void cveSVMGetDefaultGrid(SVM.ParamType type, ref MCvParamGrid grid);

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        [return: MarshalAs(CvInvoke.BoolMarshalType)]
        internal static extern bool cveSVMTrainAuto(
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

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern void cveSVMGetSupportVectors(IntPtr model, IntPtr supportVectors);
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

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern IntPtr cveDTreesCreate(ref IntPtr statModel, ref IntPtr algorithm, ref IntPtr sharedPtr);

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern void cveDTreesRelease(ref IntPtr model, ref IntPtr sharedPtr);
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
        internal static extern IntPtr cveRTreesCreate(ref IntPtr statModel, ref IntPtr algorithm, ref IntPtr sharedPtr);

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern void cveRTreesGetVotes(IntPtr model, IntPtr samples, IntPtr results, DTrees.Flags flags);

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern void cveRTreesRelease(ref IntPtr model, ref IntPtr sharedPtr);

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

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern IntPtr cveBoostCreate(ref IntPtr statModel, ref IntPtr algorithm, ref IntPtr sharedPtr);

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern void cveBoostRelease(ref IntPtr model, ref IntPtr sharedPtr);
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
        internal static extern IntPtr cveLogisticRegressionCreate(ref IntPtr statModel, ref IntPtr algorithm, ref IntPtr sharedPtr);

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern void cveLogisticRegressionRelease(ref IntPtr model, ref IntPtr sharedPtr);

        #endregion

        #region CvSVM
        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern IntPtr cveSVMSGDDefaultCreate(ref IntPtr statModel, ref IntPtr algorithm, ref IntPtr sharedPtr);

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern void cveSVMSGDRelease(ref IntPtr model, ref IntPtr sharedPtr);

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern void cveSVMSGDSetOptimalParameters(IntPtr model, Emgu.CV.ML.SVMSGD.SvmsgdType svmsgdType, Emgu.CV.ML.SVMSGD.MarginType marginType);

        #endregion
    }
}
