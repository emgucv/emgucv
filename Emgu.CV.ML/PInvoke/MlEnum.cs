//----------------------------------------------------------------------------
//  Copyright (C) 2004-2012 by EMGU. All rights reserved.       
//----------------------------------------------------------------------------

using System;

namespace Emgu.CV.ML.MlEnum
{
   /// <summary>
   /// The type of the mixture covariation matrices
   /// </summary>
   public enum EM_COVARIAN_MATRIX_TYPE
   {
      /// <summary>
      /// A covariation matrix of each mixture is a scaled identity matrix, ?k*I, so the only parameter to be estimated is ?k. The option may be used in special cases, when the constraint is relevant, or as a first step in the optimization (e.g. in case when the data is preprocessed with PCA). The results of such preliminary estimation may be passed again to the optimization procedure, this time with cov_mat_type=COV_MAT_DIAGONAL
      /// </summary>
      COV_MAT_SPHERICAL = 0, 
      /// <summary>
      /// A covariation matrix of each mixture may be arbitrary diagonal matrix with positive diagonal elements, that is, non-diagonal elements are forced to be 0's, so the number of free parameters is d  for each matrix. This is most commonly used option yielding good estimation results
      /// </summary>
      COV_MAT_DIAGONAL = 1, 
      /// <summary>
      /// A covariation matrix of each mixture may be arbitrary symmetrical positively defined matrix, so the number of free parameters in each matrix is about d2/2. It is not recommended to use this option, unless there is pretty accurate initial estimation of the parameters and/or a huge number of training samples
      /// </summary>
      COV_MAT_GENERIC = 2
   }

   /// <summary>
   /// The initial step the algorithm starts from
   /// </summary>
   public enum EM_INIT_STEP_TYPE
   {
      /// <summary>
      /// The algorithm starts with E-step. 
      /// At least, the initial values of mean vectors, CvEMParams.Means must be passed. 
      /// Optionally, the user may also provide initial values for weights (CvEMParams.Weights) and/or covariation matrices (CvEMParams.Covs).
      /// </summary>
      START_E_STEP = 1, 
      /// <summary>
      /// The algorithm starts with M-step. The initial probabilities p_{i,k} must be provided
      /// </summary>
      START_M_STEP = 2, 
      /// <summary>
      /// No values are required from the user, k-means algorithm is used to estimate initial mixtures parameters
      /// </summary>
      START_AUTO_STEP = 0
   }

   /// <summary>
   /// Type of SVM
   /// </summary>
   public enum SVM_TYPE
   {
      /// <summary>
      /// n-class classification (n>=2), allows imperfect separation of classes with penalty multiplier C for outliers
      /// </summary>
      C_SVC = 100,
      /// <summary>
      /// n-class classification with possible imperfect separation. Parameter nu (in the range 0..1, the larger the value, the smoother the decision boundary) is used instead of C
      /// </summary>
      NU_SVC = 101,
      /// <summary>
      /// one-class SVM. All the training data are from the same class, SVM builds a boundary that separates the class from the rest of the feature space
      /// </summary>
      ONE_CLASS = 102, 
      /// <summary>
      /// Regression. The distance between feature vectors from the training set and the fitting hyper-plane must be less than p. For outliers the penalty multiplier C is used
      /// </summary>
      EPS_SVR = 103,
      /// <summary>
      /// Regression; nu is used instead of p.
      /// </summary>
      NU_SVR = 104
   }

   /// <summary>
   /// The type of SVM parameters
   /// </summary>
   public enum SVM_PARAM_TYPE
   {
      /// <summary>
      /// C
      /// </summary>
      C = 0,
      /// <summary>
      /// Gamma
      /// </summary>
      GAMMA = 1,
      /// <summary>
      /// P
      /// </summary>
      P = 2,
      /// <summary>
      /// NU
      /// </summary>
      NU = 3,
      /// <summary>
      /// COEF
      /// </summary>
      COEF = 4,
      /// <summary>
      /// DEGREE
      /// </summary>
      DEGREE = 5
   }

   /// <summary>
   /// SVM kernel type
   /// </summary>
   public enum SVM_KERNEL_TYPE
   {
      /// <summary>
      /// No mapping is done, linear discrimination (or regression) is done in the original feature space. It is the fastest option. d(x,y) = x y == (x,y)
      /// </summary>
      LINEAR = 0,
      /// <summary>
      /// polynomial kernel: d(x,y) = (gamma*(xy)+coef0)^degree
      /// </summary>
      POLY = 1, 
      /// <summary>
      /// Radial-basis-function kernel; a good choice in most cases: d(x,y) = exp(-gamma*|x-y|^2)
      /// </summary>
      RBF = 2, 
      /// <summary>
      /// sigmoid function is used as a kernel: d(x,y) = tanh(gamma*(xy)+coef0)
      /// </summary>
      SIGMOID = 3
   }

   /// <summary>
   /// Training method for ANN_MLP
   /// </summary>
   public enum ANN_MLP_TRAIN_METHOD
   {
      /// <summary>
      /// Back-propagation algorithmn
      /// </summary>
      BACKPROP = 0, 
      /// <summary>
      /// Batch RPROP algorithm
      /// </summary>
      RPROP = 1 
   }

   /// <summary>
   /// Possible activation functions
   /// </summary>
   public enum ANN_MLP_ACTIVATION_FUNCTION
   {
      /// <summary>
      /// Identity
      /// </summary>
      IDENTITY = 0,
      /// <summary>
      /// sigmoif symetric
      /// </summary>
      SIGMOID_SYM = 1, 
      /// <summary>
      /// Gaussian
      /// </summary>
      GAUSSIAN = 2
   }

   /// <summary>
   /// The flags for the neural network training function
   /// </summary>
   [Flags]
   public enum ANN_MLP_TRAINING_FLAG
   {
      /// <summary>
      /// 
      /// </summary>
      DEFAULT = 0,
      /// <summary>
      /// 
      /// </summary>
      UPDATE_WEIGHTS = 1, 
      /// <summary>
      /// 
      /// </summary>
      NO_INPUT_SCALE = 2, 
      /// <summary>
      /// 
      /// </summary>
      NO_OUTPUT_SCALE = 4
   }

   /// <summary>
   /// The data layout type
   /// </summary>
   public enum DATA_LAYOUT_TYPE
   {
      /// <summary>
      /// Feature vectors are stored as cols
      /// </summary>
      COL_SAMPLE = 0,
      /// <summary>
      /// Feature vectors are stored as rows
      /// </summary>
      ROW_SAMPLE =1
   }

   /// <summary>
   /// Boosting type
   /// </summary>
   public enum BOOST_TYPE
   {
      /// <summary>
      /// Discrete AdaBoost
      /// </summary>
      DISCRETE = 0, 
      /// <summary>
      /// Real AdaBoost
      /// </summary>
      REAL = 1, 
      /// <summary>
      /// LogitBoost
      /// </summary>
      LOGIT = 2, 
      /// <summary>
      /// Gentle AdaBoost
      /// </summary>
      GENTLE = 3
   }

   /// <summary>
   /// Splitting criteria, used to choose optimal splits during a weak tree construction
   /// </summary>
   public enum BOOST_SPLIT_CREITERIA
   {
      /// <summary>
      /// Use the default criteria for the particular boosting method, see below
      /// </summary>
      DEFAULT = 0, 
      /// <summary>
      /// Use Gini index. This is default option for Real AdaBoost; may be also used for Discrete AdaBoost
      /// </summary>
      GINI = 1, 
      /// <summary>
      /// Use misclassification rate. This is default option for Discrete AdaBoost; may be also used for Real AdaBoost
      /// </summary>
      MISCLASS = 3, 
      /// <summary>
      /// Use least squares criteria. This is default and the only option for LogitBoost and Gentle AdaBoost
      /// </summary>
      SQERR = 4 
   }

   /// <summary>
   /// Variable type
   /// </summary>
   public enum VAR_TYPE
   {
      /// <summary>
      /// Numerical or Ordered
      /// </summary>
      NUMERICAL  =   0,
      /// <summary>
      /// Catagorical
      /// </summary>
      CATEGORICAL   = 1
   }
}
