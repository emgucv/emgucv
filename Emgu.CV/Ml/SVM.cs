//----------------------------------------------------------------------------
//  Copyright (C) 2004-2020 by EMGU Corporation. All rights reserved.       
//----------------------------------------------------------------------------

using System;
using Emgu.CV.CvEnum;
using Emgu.CV.ML.MlEnum;
using Emgu.CV.Structure;
using Emgu.Util;
using System.Runtime.InteropServices;

namespace Emgu.CV.ML
{
    /// <summary>
    /// Wrapped CvParamGrid structure used by SVM
    /// </summary>
    public struct MCvParamGrid
    {
        /// <summary>
        /// Minimum value
        /// </summary>
        public double MinVal;
        /// <summary>
        /// Maximum value
        /// </summary>
        public double MaxVal;
        /// <summary>
        /// step
        /// </summary>
        public double Step;
    }

    /// <summary>
    /// Support Vector Machine 
    /// </summary>
    public partial class SVM : UnmanagedObject, IStatModel
    {
        private IntPtr _sharedPtr;

        /// <summary>
        /// Type of SVM
        /// </summary>
        public enum SvmType
        {
            /// <summary>
            /// n-class classification (n>=2), allows imperfect separation of classes with penalty multiplier C for outliers
            /// </summary>
            CSvc = 100,
            /// <summary>
            /// n-class classification with possible imperfect separation. Parameter nu (in the range 0..1, the larger the value, the smoother the decision boundary) is used instead of C
            /// </summary>
            NuSvc = 101,
            /// <summary>
            /// one-class SVM. All the training data are from the same class, SVM builds a boundary that separates the class from the rest of the feature space
            /// </summary>
            OneClass = 102,
            /// <summary>
            /// Regression. The distance between feature vectors from the training set and the fitting hyper-plane must be less than p. For outliers the penalty multiplier C is used
            /// </summary>
            EpsSvr = 103,
            /// <summary>
            /// Regression; nu is used instead of p.
            /// </summary>
            NuSvr = 104
        }


        /// <summary>
        /// SVM kernel type
        /// </summary>
        public enum SvmKernelType
        {
            /// <summary>
            /// Custom svm kernel type
            /// </summary>
            Custom = -1,
            /// <summary>
            /// No mapping is done, linear discrimination (or regression) is done in the original feature space. It is the fastest option. d(x,y) = x y == (x,y)
            /// </summary>
            Linear = 0,
            /// <summary>
            /// polynomial kernel: d(x,y) = (gamma*(xy)+coef0)^degree
            /// </summary>
            Poly = 1,
            /// <summary>
            /// Radial-basis-function kernel; a good choice in most cases: d(x,y) = exp(-gamma*|x-y|^2)
            /// </summary>
            Rbf = 2,
            /// <summary>
            /// sigmoid function is used as a kernel: d(x,y) = tanh(gamma*(xy)+coef0)
            /// </summary>
            Sigmoid = 3,
            /// <summary>
            /// Exponential Chi2 kernel, similar to the RBF kernel
            /// </summary>
            Chi2 = 4,
            /// <summary>
            /// Histogram intersection kernel. A fast kernel. K(xi,xj)=min(xi,xj).
            /// </summary>
            Inter = 5
        }

        /// <summary>
        /// The type of SVM parameters
        /// </summary>
        public enum ParamType
        {
            /// <summary>
            /// C
            /// </summary>
            C = 0,
            /// <summary>
            /// Gamma
            /// </summary>
            Gamma = 1,
            /// <summary>
            /// P
            /// </summary>
            P = 2,
            /// <summary>
            /// NU
            /// </summary>
            Nu = 3,
            /// <summary>
            /// COEF
            /// </summary>
            Coef = 4,
            /// <summary>
            /// DEGREE
            /// </summary>
            Degree = 5
        }
        
        private IntPtr _statModelPtr;
        private IntPtr _algorithmPtr;

        /// <summary>
        /// Create a support Vector Machine
        /// </summary>
        public SVM()
        {
            _ptr = MlInvoke.cveSVMDefaultCreate(ref _statModelPtr, ref _algorithmPtr, ref _sharedPtr);
        }

        /// <summary>
        /// Release all the memory associated with the SVM
        /// </summary>
        protected override void DisposeObject()
        {
            if (IntPtr.Zero != _ptr)
            {
                MlInvoke.cveSVMRelease(ref _ptr, ref _sharedPtr);
                _statModelPtr = IntPtr.Zero;
                _algorithmPtr = IntPtr.Zero;
            }
        }



        /// <summary>
        /// Get the default parameter grid for the specific SVM type
        /// </summary>
        /// <param name="type">The SVM type</param>
        /// <returns>The default parameter grid for the specific SVM type </returns>
        public static MCvParamGrid GetDefaultGrid(SVM.ParamType type)
        {
            MCvParamGrid grid = new MCvParamGrid();
            MlInvoke.cveSVMGetDefaultGrid(type, ref grid);
            return grid;
        }

        /// <summary>
        /// The method trains the SVM model automatically by choosing the optimal parameters C, gamma, p, nu, coef0, degree from CvSVMParams. By the optimality one mean that the cross-validation estimate of the test set error is minimal. 
        /// </summary>
        /// <param name="trainData">The training data.</param>
        /// <param name="kFold">Cross-validation parameter. The training set is divided into k_fold subsets, one subset being used to train the model, the others forming the test set. So, the SVM algorithm is executed k_fold times</param>
        /// <returns>True if training is successful.</returns>
        public bool TrainAuto(
           TrainData trainData,
           int kFold = 10)
        {
            return TrainAuto(
               trainData,
               kFold,
               GetDefaultGrid(ParamType.C),
               GetDefaultGrid(ParamType.Gamma),
               GetDefaultGrid(ParamType.P),
               GetDefaultGrid(ParamType.Nu),
               GetDefaultGrid(ParamType.Coef),
               GetDefaultGrid(ParamType.Degree));
        }

        /// <summary>
        /// The method trains the SVM model automatically by choosing the optimal parameters C, gamma, p, nu, coef0, degree from CvSVMParams. By the optimality one mean that the cross-validation estimate of the test set error is minimal. 
        /// </summary>
        /// <param name="trainData">The training data.</param>
        /// <param name="kFold">Cross-validation parameter. The training set is divided into k_fold subsets, one subset being used to train the model, the others forming the test set. So, the SVM algorithm is executed k_fold times</param>
        /// <param name="cGrid">cGrid</param>
        /// <param name="gammaGrid">grid for gamma</param>
        /// <param name="pGrid">grid for p</param>
        /// <param name="nuGrid">grid for nu</param>
        /// <param name="coefGrid">grid for coeff</param>
        /// <param name="degreeGrid">grid for degree</param>
        /// <param name="balanced">If true and the problem is 2-class classification then the method creates more balanced cross-validation subsets that is proportions between classes in subsets are close to such proportion in the whole train dataset.</param>
        /// <returns>True if training is successful.</returns>
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
            return MlInvoke.cveSVMTrainAuto(
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

        /// <summary>
        /// Retrieves all the support vectors.
        /// </summary>
        /// <returns>All the support vector as floating-point matrix, where support vectors are stored as matrix rows.</returns>
        public Mat GetSupportVectors()
        {
            Mat m = new Mat();
            MlInvoke.cveSVMGetSupportVectors(_ptr, m);
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
