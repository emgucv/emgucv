//----------------------------------------------------------------------------
//  Copyright (C) 2004-2014 by EMGU Corporation. All rights reserved.       
//----------------------------------------------------------------------------

using System;
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

      public class Params : UnmanagedObject
      {

         public Params(
            MlEnum.SvmType svmType, MlEnum.SvmKernelType kernelType, double degree, double gamma, double coef0,
            double c, double nu, double p, Mat classWeights, MCvTermCriteria termCrit)
         {
            _ptr = MlInvoke.CvSVMParamsCreate(
                     svmType, kernelType, degree, gamma, coef0, c, nu, p, classWeights ?? IntPtr.Zero,
                     ref termCrit);
         }

         public Params(
            MlEnum.SvmType svmType = SvmType.CSvc, MlEnum.SvmKernelType kernelType = SvmKernelType.Rbf, double degree = 0, double gamma = 1, double coef0 = 0,
            double c = 1, double nu = 0, double p = 0, Mat classWeights = null)
            : this(svmType, kernelType, degree, gamma, coef0, c, nu, p, classWeights, new MCvTermCriteria(1000, 1.0e-7) )
         {

         }

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
