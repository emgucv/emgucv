//----------------------------------------------------------------------------
//  Copyright (C) 2004-2015 by EMGU Corporation. All rights reserved.       
//----------------------------------------------------------------------------

using System;
using Emgu.CV;
using Emgu.CV.ML.Structure;
using System.Runtime.InteropServices;
using Emgu.Util;

namespace Emgu.CV.ML
{
   /// <summary>
   /// Decision Tree 
   /// </summary>
   public class DTree : UnmanagedObject , IStatModel
   {
      private IntPtr _statModelPtr;
      private IntPtr _algorithmPtr;

      /// <summary>
      /// The structure contains all the decision tree training parameters.
      /// </summary>
      public class Params : UnmanagedObject
      {
         /// <summary>
         /// Initializes a new instance of the <see cref="Params"/> class.
         /// </summary>
         /// <param name="maxDepth">The maximum possible depth of the tree. That is the training algorithms attempts to split a node while its depth is less than maxDepth. The root node has zero depth. The actual depth may be smaller if the other termination criteria are met (see the outline of the training procedure in the beginning of the section), and/or if the tree is pruned.</param>
         /// <param name="minSampleCount">If the number of samples in a node is less than this parameter then the node will not be split.</param>
         /// <param name="regressionAccuracy">Termination criteria for regression trees. If all absolute differences between an estimated value in a node and values of train samples in this node are less than this parameter then the node will not be split further.</param>
         /// <param name="useSurrogates">If true then surrogate splits will be built. These splits allow to work with missing data and compute variable importance correctly. .. note:: currently it’s not implemented.</param>
         /// <param name="maxCategories">Cluster possible values of a categorical variable into K&lt;=maxCategories clusters to find a suboptimal split. If a discrete variable, on which the training procedure tries to make a split, takes more than maxCategories values, the precise best subset estimation may take a very long time because the algorithm is exponential. Instead, many decision trees engines (including our implementation) try to find sub-optimal split in this case by clustering all the samples into maxCategories clusters that is some categories are merged together. The clustering is applied only in n &gt; 2-class classification problems for categorical variables with N &gt; max_categories possible values. In case of regression and 2-class classification the optimal split can be found efficiently without employing clustering, thus the parameter is not used in these cases.</param>
         /// <param name="CvFolds">If CVFolds &gt; 1 then algorithms prunes the built decision tree using K-fold cross-validation procedure where K is equal to CVFolds.</param>
         /// <param name="use1SERule">If set to <c>true</c> then a pruning will be harsher. This will make a tree more compact and more resistant to the training data noise but a bit less accurate.</param>
         /// <param name="truncatePrunedTree">If set to <c>true</c> then pruned branches are physically removed from the tree. Otherwise they are retained and it is possible to get results from the original unpruned (or pruned less aggressively) tree.</param>
         /// <param name="priors">The array of a priori class probabilities, sorted by the class label value. The parameter can be used to tune the decision tree preferences toward a certain class. For example, if you want to detect some rare anomaly occurrence, the training base will likely contain much more normal cases than anomalies, so a very good classification performance will be achieved just by considering every case as normal. To avoid this, the priors can be specified, where the anomaly probability is artificially increased (up to 0.5 or even greater), so the weight of the misclassified anomalies becomes much bigger, and the tree is adjusted properly. You can also think about this parameter as weights of prediction categories which determine relative weights that you give to misclassification. That is, if the weight of the first category is 1 and the weight of the second category is 10, then each mistake in predicting the second category is equivalent to making 10 mistakes in predicting the first category.</param>
         public Params(int maxDepth = int.MaxValue, int minSampleCount = 10,
            double regressionAccuracy = 0.01f, bool useSurrogates = false,
            int maxCategories = 10, int CvFolds=10,
            bool use1SERule = true, bool truncatePrunedTree = true,
            Mat priors = null)
         {
            _ptr = MlInvoke.CvDTreeParamsCreate(
               maxDepth, minSampleCount,
               regressionAccuracy, useSurrogates,
               maxCategories, CvFolds, use1SERule, truncatePrunedTree,
               priors ?? IntPtr.Zero);
         }

         /// <summary>
         /// Release the unmanaged resources
         /// </summary>
         protected override void DisposeObject()
         {
            MlInvoke.CvDTreeParamsRelease(ref _ptr);
         }
      }

      /// <summary>
      /// Create a default decision tree
      /// </summary>
      public DTree(Params p)
      {
         _ptr = MlInvoke.CvDTreeCreate(p, ref _statModelPtr, ref _algorithmPtr);
      }

      /// <summary>
      /// Release the decision tree and all the memory associate with it
      /// </summary>
      protected override void DisposeObject()
      {
         MlInvoke.CvDTreeRelease(ref _ptr);
         _statModelPtr = IntPtr.Zero;
         _algorithmPtr = IntPtr.Zero;
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
