//----------------------------------------------------------------------------
//  Copyright (C) 2004-2026 by EMGU Corporation. All rights reserved.       
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
    /// This class contains functions to call into machine learning module
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
        internal static extern bool cveStatModelTrain(IntPtr model, IntPtr samples, DataLayoutType layout, IntPtr responses);

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        [return: MarshalAs(CvInvoke.BoolMarshalType)]
        internal static extern bool cveStatModelTrainWithData(IntPtr model, IntPtr data, int flags);

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern float cveStatModelPredict(IntPtr model, IntPtr samples, IntPtr results, int flags);
        #endregion

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern IntPtr cveTrainDataCreate(
           IntPtr samples, DataLayoutType layout, IntPtr responses,
           IntPtr varIdx, IntPtr sampleIdx,
           IntPtr sampleWeights, IntPtr varType,
           ref IntPtr sharedPtr);

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern void cveTrainDataRelease(ref IntPtr sharedPtr);

        #region NormalBayesClassifier
        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern IntPtr cveNormalBayesClassifierDefaultCreate(ref IntPtr statModel, ref IntPtr algorithm, ref IntPtr sharedPtr);

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern void cveNormalBayesClassifierRelease(ref IntPtr classifier, ref IntPtr sharedPtr);
        #endregion

        #region KNearest
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

        #region EM
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


        #region SVM
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

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern IntPtr cveDTreesCreate(ref IntPtr statModel, ref IntPtr algorithm, ref IntPtr sharedPtr);

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern void cveDTreesRelease(ref IntPtr model, ref IntPtr sharedPtr);
        #endregion

        #region Random tree

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

        #endregion

        #region Boost

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern IntPtr cveBoostCreate(ref IntPtr statModel, ref IntPtr algorithm, ref IntPtr sharedPtr);

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern void cveBoostRelease(ref IntPtr model, ref IntPtr sharedPtr);
        #endregion

        #region Logistic Regression
        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern IntPtr cveLogisticRegressionCreate(ref IntPtr statModel, ref IntPtr algorithm, ref IntPtr sharedPtr);

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern void cveLogisticRegressionRelease(ref IntPtr model, ref IntPtr sharedPtr);

        #endregion

        #region SVM
        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern IntPtr cveSVMSGDDefaultCreate(ref IntPtr statModel, ref IntPtr algorithm, ref IntPtr sharedPtr);

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern void cveSVMSGDRelease(ref IntPtr model, ref IntPtr sharedPtr);

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern void cveSVMSGDSetOptimalParameters(IntPtr model, Emgu.CV.ML.SVMSGD.SvmsgdType svmsgdType, Emgu.CV.ML.SVMSGD.MarginType marginType);

        #endregion
    }
}
