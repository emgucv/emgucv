//----------------------------------------------------------------------------
//  Copyright (C) 2004-2015 by EMGU Corporation. All rights reserved.       
//----------------------------------------------------------------------------

using System;
using Emgu.CV;
using Emgu.Util;
using Emgu.CV.Structure;
using Emgu.CV.ML.Structure;
using System.Runtime.InteropServices;
using System.Diagnostics;

namespace Emgu.CV.ML
{
   /// <summary>
   /// Random trees
   /// </summary>
   public partial class RTrees : UnmanagedObject, IStatModel
   {
      private IntPtr _statModelPtr;
      private IntPtr _algorithmPtr;

      /*
      /// <summary>
      /// Training parameters of random trees.
      /// </summary>
      public class Params : UnmanagedObject
      {
         /// <summary>
         /// Initializes a new instance of the <see cref="Params"/> class.
         /// </summary>
         /// <param name="maxDepth">The depth of the tree. A low value will likely underfit and conversely a high value will likely overfit. The optimal value can be obtained using cross validation or other suitable methods.</param>
         /// <param name="minSampleCount">The minimum samples required at a leaf node for it to be split. A reasonable value is a small percentage of the total data e.g. 1%.</param>
         /// <param name="regressionAccuracy">The regression accuracy.</param>
         /// <param name="useSurrogates">if set to <c>true</c> use surrogates.</param>
         /// <param name="maxCategories">Cluster possible values of a categorical variable into K &lt;= maxCategories clusters to find a suboptimal split. If a discrete variable, on which the training procedure tries to make a split, takes more than max_categories values, the precise best subset estimation may take a very long time because the algorithm is exponential. Instead, many decision trees engines (including ML) try to find sub-optimal split in this case by clustering all the samples into maxCategories clusters that is some categories are merged together. The clustering is applied only in n&gt;2-class classification problems for categorical variables with N &gt; max_categories possible values. In case of regression and 2-class classification the optimal split can be found efficiently without employing clustering, thus the parameter is not used in these cases.</param>
         /// <param name="priors">The priors.</param>
         /// <param name="calcVarImportance">if set to <c>true</c> then variable importance will be calculated and then it can be retrieved by RTrees::getVarImportance..</param>
         /// <param name="nactiveVars">The size of the randomly selected subset of features at each tree node and that are used to find the best split(s). If you set it to 0 then the size will be set to the square root of the total number of features.</param>
         /// <param name="termCrit">The termination criteria that specifies when the training algorithm stops - either when the specified number of trees is trained and added to the ensemble or when sufficient accuracy (measured as OOB error) is achieved. Typically the more trees you have the better the accuracy. However, the improvement in accuracy generally diminishes and asymptotes pass a certain number of trees. Also to keep in mind, the number of tree increases the prediction time linearly.</param>
         public Params(
            int maxDepth = 5, int minSampleCount = 10,
            double regressionAccuracy = 0, bool useSurrogates = false,
            int maxCategories = 10, Mat priors = null,
            bool calcVarImportance = false, int nactiveVars = 0,
            MCvTermCriteria termCrit = new MCvTermCriteria())
         {
            if (termCrit.Epsilon == 0 && termCrit.MaxIter == 0 && termCrit.Type == 0)
            {
               termCrit = new MCvTermCriteria(50, 0.1);
            }
            IntPtr priorsPtr = (priors == null ? IntPtr.Zero : priors.Ptr);
            _ptr =
               MlInvoke.CvRTParamsCreate(
                  maxDepth, minSampleCount, regressionAccuracy, useSurrogates,
                  maxCategories, priorsPtr, calcVarImportance, nactiveVars, ref termCrit
            );
         }

         /// <summary>
         /// Release the unmanaged resources
         /// </summary>
         protected override void DisposeObject()
         {
            MlInvoke.CvRTParamsRelease(ref _ptr);
         }
      }*/

      /// <summary>
      /// Create a random tree
      /// </summary>
      public RTrees()
      {
         _ptr = MlInvoke.cveRTreesCreate(ref _statModelPtr, ref _algorithmPtr);
      }


      /// <summary>
      /// Release the random tree and all memory associate with it
      /// </summary>
      protected override void DisposeObject()
      {
         MlInvoke.cveRTreesRelease(ref _ptr);
         _statModelPtr = IntPtr.Zero;
         _algorithmPtr = IntPtr.Zero;
      }

      /*

      /// <summary>
      /// Get the number of Trees in the Random tree
      /// </summary>
      public int TreeCount
      {
         get
         {
            return MlInvoke.CvRTreesGetTreeCount(Ptr);
         }
      }

      /// <summary>
      /// Get the variable importance matrix
      /// </summary>
      public Matrix<float> VarImportance
      {
         get
         {
            IntPtr matPtr = MlInvoke.CvRTreesGetVarImportance(Ptr);
            if (matPtr == IntPtr.Zero) return null;
            MCvMat mat = (MCvMat)Marshal.PtrToStructure(matPtr, typeof(MCvMat));

            Matrix<float> res = new Matrix<float>(mat.Rows, mat.Cols, 1, mat.Data, mat.Step);
            Debug.Assert(mat.Type == res.MCvMat.Type, "Matrix type is not float");
            return res;
         }
      }*/

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
