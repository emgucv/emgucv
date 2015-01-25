//----------------------------------------------------------------------------
//  Copyright (C) 2004-2015 by EMGU Corporation. All rights reserved.       
//----------------------------------------------------------------------------

using System;
using Emgu.CV.CvEnum;
using Emgu.CV.ML.MlEnum;
using Emgu.CV.Structure;
using Emgu.Util;
using Emgu.CV.ML.Structure;
using System.Runtime.InteropServices;

namespace Emgu.CV.ML
{
   /// <summary>
   /// Support Vector Machine 
   /// </summary>
   public class SVM : UnmanagedObject, IStatModel
   {
      private IntPtr _statModelPtr;
      private IntPtr _algorithmPtr;

      /// <summary>
      /// Create a support Vector Machine
      /// </summary>
      public SVM(Params p)
      {
         _ptr = MlInvoke.CvSVMDefaultCreate(p, ref _statModelPtr, ref _algorithmPtr);
      }

      /// <summary>
      /// Release all the memory associated with the SVM
      /// </summary>
      protected override void DisposeObject()
      {
         MlInvoke.CvSVMRelease(ref _ptr);
         _statModelPtr = IntPtr.Zero;
         _algorithmPtr = IntPtr.Zero;
      }

      /// <summary>
      /// SVM training parameters.
      /// </summary>
      public class Params : UnmanagedObject
      {
         /// <summary>
         /// Initializes a new instance of the <see cref="Params"/> class.
         /// </summary>
         /// <param name="svmType">Type of a SVM formulation.</param>
         /// <param name="kernelType">Type of a SVM kernel.</param>
         /// <param name="degree">The degree of a kernel function (POLY).</param>
         /// <param name="gamma">The gamma of a kernel function (POLY / RBF / SIGMOID / CHI2).</param>
         /// <param name="coef0">The coef0 of a kernel function (POLY / SIGMOID)..</param>
         /// <param name="c">The c of a SVM optimization problem (C_SVC / EPS_SVR / NU_SVR).</param>
         /// <param name="nu">The nu of a SVM optimization problem (NU_SVC / ONE_CLASS / NU_SVR).</param>
         /// <param name="p">The p of a SVM optimization problem (EPS_SVR)..</param>
         /// <param name="classWeights"> Optional weights in the C_SVC problem , assigned to particular classes. They are multiplied by C so the parameter C of class #i becomes classWeights(i) * C. Thus these weights affect the misclassification penalty for different classes. The larger weight, the larger penalty on misclassification of data from the corresponding class.</param>
         /// <param name="termCrit">Termination criteria of the iterative SVM training procedure which solves a partial case of constrained quadratic optimization problem. You can specify tolerance and/or the maximum number of iterations.</param>
         public Params(
            MlEnum.SvmType svmType = SvmType.CSvc, MlEnum.SvmKernelType kernelType = SvmKernelType.Rbf, double degree = 0, double gamma = 1, double coef0 = 0,
            double c = 1, double nu = 0, double p = 0, Mat classWeights = null, MCvTermCriteria termCrit = new MCvTermCriteria())
         {
            if (termCrit.Epsilon == 0f && termCrit.MaxIter == 0 && termCrit.Type == 0)
            {
               termCrit = new MCvTermCriteria(1000, 1.0e-7);
            }
            _ptr = MlInvoke.CvSVMParamsCreate(
                     svmType, kernelType, degree, gamma, coef0, c, nu, p, classWeights ?? IntPtr.Zero,
                     ref termCrit);
         }

         /*
         /// <summary>
         /// Initializes a new instance of the <see cref="Params"/> class.
         /// </summary>
         /// <param name="svmType">Type of a SVM formulation.</param>
         /// <param name="kernelType">Type of a SVM kernel.</param>
         /// <param name="degree">The degree of a kernel function (POLY).</param>
         /// <param name="gamma">The gamma of a kernel function (POLY / RBF / SIGMOID / CHI2).</param>
         /// <param name="coef0">The coef0 of a kernel function (POLY / SIGMOID)..</param>
         /// <param name="c">The c of a SVM optimization problem (C_SVC / EPS_SVR / NU_SVR).</param>
         /// <param name="nu">The nu of a SVM optimization problem (NU_SVC / ONE_CLASS / NU_SVR).</param>
         /// <param name="p">The p of a SVM optimization problem (EPS_SVR)..</param>
         /// <param name="classWeights"> Optional weights in the C_SVC problem , assigned to particular classes. They are multiplied by C so the parameter C of class #i becomes classWeights(i) * C. Thus these weights affect the misclassification penalty for different classes. The larger weight, the larger penalty on misclassification of data from the corresponding class.</param>
         public Params(
            MlEnum.SvmType svmType = SvmType.CSvc, MlEnum.SvmKernelType kernelType = SvmKernelType.Rbf, double degree = 0, double gamma = 1, double coef0 = 0,
            double c = 1, double nu = 0, double p = 0, Mat classWeights = null)
            : this(svmType, kernelType, degree, gamma, coef0, c, nu, p, classWeights, new MCvTermCriteria(1000, 1.0e-7) )
         {

         }*/

         /// <summary>
         /// Release the unmanaged resources
         /// </summary>
         protected override void DisposeObject()
         {
            MlInvoke.CvSVMParamsRelease(ref _ptr);
         }
      }

      /// <summary>
      /// Get the default parameter grid for the specific SVM type
      /// </summary>
      /// <param name="type">The SVM type</param>
      /// <returns>The default parameter grid for the specific SVM type </returns>
      public static MCvParamGrid GetDefaultGrid(MlEnum.SvmParamType type)
      {
         MCvParamGrid grid = new MCvParamGrid();
         MlInvoke.CvSVMGetDefaultGrid(type, ref grid);
         return grid;
      }

      /// <summary>
      /// The method trains the SVM model automatically by choosing the optimal parameters C, gamma, p, nu, coef0, degree from CvSVMParams. By the optimality one mean that the cross-validation estimate of the test set error is minimal. 
      /// </summary>
      /// <param name="trainData">The training data.</param>
      /// <param name="kFold">Cross-validation parameter. The training set is divided into k_fold subsets, one subset being used to train the model, the others forming the test set. So, the SVM algorithm is executed k_fold times</param>
      /// <returns></returns>
      public bool TrainAuto(
         TrainData trainData,
         int kFold)
      {
         return TrainAuto(
            trainData,
            kFold,
            GetDefaultGrid(Emgu.CV.ML.MlEnum.SvmParamType.C),
            GetDefaultGrid(Emgu.CV.ML.MlEnum.SvmParamType.Gamma),
            GetDefaultGrid(Emgu.CV.ML.MlEnum.SvmParamType.P),
            GetDefaultGrid(Emgu.CV.ML.MlEnum.SvmParamType.Nu),
            GetDefaultGrid(Emgu.CV.ML.MlEnum.SvmParamType.Coef),
            GetDefaultGrid(Emgu.CV.ML.MlEnum.SvmParamType.Degree));
      }

      /// <summary>
      /// The method trains the SVM model automatically by choosing the optimal parameters C, gamma, p, nu, coef0, degree from CvSVMParams. By the optimality one mean that the cross-validation estimate of the test set error is minimal. 
      /// </summary>
      /// <param name="trainData">The training data.</param>
      /// <param name="kFold">Cross-validation parameter. The training set is divided into k_fold subsets, one subset being used to train the model, the others forming the test set. So, the SVM algorithm is executed k_fold times</param>
      /// <param name="cGrid">cGrid</param>
      /// <param name="gammaGrid">gammaGrid</param>
      /// <param name="pGrid">pGrid</param>
      /// <param name="nuGrid">nuGrid</param>
      /// <param name="coefGrid">coedGrid</param>
      /// <param name="degreeGrid">degreeGrid</param>
      /// <returns></returns>
      public bool TrainAuto(
         TrainData trainData,
         int kFold,
         MCvParamGrid cGrid,
         MCvParamGrid gammaGrid,
         MCvParamGrid pGrid,
         MCvParamGrid nuGrid,
         MCvParamGrid coefGrid,
         MCvParamGrid degreeGrid,
         bool balanced = false)
      {
         return MlInvoke.CvSVMTrainAuto(
            Ptr,
            trainData.Ptr,
            kFold,
            ref cGrid,
            ref gammaGrid, 
            ref pGrid, 
            ref nuGrid,
            ref coefGrid,
            ref degreeGrid,
            balanced);
      }

      public Mat GetSupportVectors()
      {
         Mat m = new Mat();
         MlInvoke.CvSVMGetSupportVectors(_ptr, m);
         return m;
      }

      IntPtr IStatModel.StatModelPtr
      {
         get { return _statModelPtr; }
      }

      IntPtr IAlgorithm.AlgorithmPtr
      {
         get { return _algorithmPtr; }
      }
   }
}
