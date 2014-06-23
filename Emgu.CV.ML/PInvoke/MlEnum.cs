//----------------------------------------------------------------------------
//  Copyright (C) 2004-2014 by EMGU Corporation. All rights reserved.       
//----------------------------------------------------------------------------

using System;

namespace Emgu.CV.ML.MlEnum
{
   /// <summary>
   /// The type of the mixture covariation matrices
   /// </summary>
   public enum EmCovarianMatrixType
   {
      /// <summary>
      /// A covariation matrix of each mixture is a scaled identity matrix, ?k*I, so the only parameter to be estimated is ?k. The option may be used in special cases, when the constraint is relevant, or as a first step in the optimization (e.g. in case when the data is preprocessed with PCA). The results of such preliminary estimation may be passed again to the optimization procedure, this time with cov_mat_type=COV_MAT_DIAGONAL
      /// </summary>
      Spherical = 0, 
      /// <summary>
      /// A covariation matrix of each mixture may be arbitrary diagonal matrix with positive diagonal elements, that is, non-diagonal elements are forced to be 0's, so the number of free parameters is d  for each matrix. This is most commonly used option yielding good estimation results
      /// </summary>
      Diagonal = 1, 
      /// <summary>
      /// A covariation matrix of each mixture may be arbitrary symmetrical positively defined matrix, so the number of free parameters in each matrix is about d2/2. It is not recommended to use this option, unless there is pretty accurate initial estimation of the parameters and/or a huge number of training samples
      /// </summary>
      Generic = 2
   }

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
   /// The type of SVM parameters
   /// </summary>
   public enum SvmParamType
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

   /// <summary>
   /// SVM kernel type
   /// </summary>
   public enum SvmKernelType
   {
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
      Sigmoid = 3
   }

   /// <summary>
   /// Training method for ANN_MLP
   /// </summary>
   public enum AnnMlpTrainMethod
   {
      /// <summary>
      /// Back-propagation algorithmn
      /// </summary>
      Backprop = 0, 
      /// <summary>
      /// Batch RPROP algorithm
      /// </summary>
      Rprop = 1 
   }

   /// <summary>
   /// Possible activation functions
   /// </summary>
   public enum AnnMlpActivationFunction
   {
      /// <summary>
      /// Identity
      /// </summary>
      Identity = 0,
      /// <summary>
      /// sigmoif symetric
      /// </summary>
      SigmoidSym = 1, 
      /// <summary>
      /// Gaussian
      /// </summary>
      Gaussian = 2
   }

   /// <summary>
   /// The flags for the neural network training function
   /// </summary>
   [Flags]
   public enum AnnMlpTrainingFlag
   {
      /// <summary>
      /// 
      /// </summary>
      Default = 0,
      /// <summary>
      /// 
      /// </summary>
      UpdateWeights = 1, 
      /// <summary>
      /// 
      /// </summary>
      NoInputScale = 2, 
      /// <summary>
      /// 
      /// </summary>
      NoOutputScale = 4
   }

   /// <summary>
   /// The data layout type
   /// </summary>
   public enum DataLayoutType
   {
      /// <summary>
      /// Feature vectors are stored as cols
      /// </summary>
      ColSample = 0,
      /// <summary>
      /// Feature vectors are stored as rows
      /// </summary>
      RowSample =1
   }

   /// <summary>
   /// Boosting type
   /// </summary>
   public enum BoostType
   {
      /// <summary>
      /// Discrete AdaBoost
      /// </summary>
      Discrete = 0, 
      /// <summary>
      /// Real AdaBoost
      /// </summary>
      Real = 1, 
      /// <summary>
      /// LogitBoost
      /// </summary>
      Logit = 2, 
      /// <summary>
      /// Gentle AdaBoost
      /// </summary>
      Gentle = 3
   }

   /// <summary>
   /// Splitting criteria, used to choose optimal splits during a weak tree construction
   /// </summary>
   public enum BoostSplitCreiteria
   {
      /// <summary>
      /// Use the default criteria for the particular boosting method, see below
      /// </summary>
      Default = 0, 
      /// <summary>
      /// Use Gini index. This is default option for Real AdaBoost; may be also used for Discrete AdaBoost
      /// </summary>
      Gini = 1, 
      /// <summary>
      /// Use misclassification rate. This is default option for Discrete AdaBoost; may be also used for Real AdaBoost
      /// </summary>
      Misclass = 3, 
      /// <summary>
      /// Use least squares criteria. This is default and the only option for LogitBoost and Gentle AdaBoost
      /// </summary>
      Sqerr = 4 
   }

   /// <summary>
   /// Variable type
   /// </summary>
   public enum VarType
   {
      /// <summary>
      /// Numerical or Ordered
      /// </summary>
      Numerical  =   0,
      /// <summary>
      /// Catagorical
      /// </summary>
      Categorical   = 1
   }
}
