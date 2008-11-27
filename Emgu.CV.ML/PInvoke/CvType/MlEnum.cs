using System;
using System.Collections.Generic;
using System.Text;

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
}
