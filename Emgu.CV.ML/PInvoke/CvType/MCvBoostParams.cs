//----------------------------------------------------------------------------
//  Copyright (C) 2004-2012 by EMGU. All rights reserved.       
//----------------------------------------------------------------------------

using System;
using System.Runtime.InteropServices;

namespace Emgu.CV.ML.Structure
{
   /// <summary>
   /// An OpenCV Boost Tree parameters
   /// </summary>
   public struct MCvBoostParams
   {
      /// <summary>
      /// Get the default Decision tree training parameters
      /// </summary>
      /// <returns>The default Decision tree training parameters</returns>
      public static MCvBoostParams GetDefaultParameter()
      {
         IntPtr ptr = MlInvoke.CvBoostParamsCreate();
         MCvBoostParams p = (MCvBoostParams)Marshal.PtrToStructure(ptr, typeof(MCvBoostParams));
         MlInvoke.CvBoostParamsRelease(ref ptr);
         return p;
      }

      #region DTreeParams
      /// <summary>
      /// If a discrete variable, on which the training procedure tries to make a split, takes more than max_categories values, the precise best subset estimation may take a very long time (as the algorithm is exponential). Instead, many decision trees engines (including ML) try to find sub-optimal split in this case by clustering all the samples into max_categories clusters (i.e. some categories are merged together).
      ///Note that this technique is used only in N(>2)-class classification problems. In case of regression and 2-class classification the optimal split can be found efficiently without employing clustering, thus the parameter is not used in these cases.
      /// </summary>
      public int maxCategories;
      /// <summary>
      /// This parameter specifies the maximum possible depth of the tree. That is the training algorithms attempts to split a node while its depth is less than max_depth. The actual depth may be smaller if the other termination criteria are met (see the outline of the training procedure in the beginning of the section), and/or if the tree is pruned.
      /// </summary>
      public int maxDepth;
      /// <summary>
      /// A node is not split if the number of samples directed to the node is less than the parameter value. 
      /// </summary>
      public int minSampleCount;
      /// <summary>
      /// If this parameter is &gt;1, the tree is pruned using cv_folds-fold cross validation.
      /// </summary>
      public int cvFolds;
      /// <summary>
      /// If true, surrogate splits are built. Surrogate splits are needed to handle missing measurements and for variable importance estimation.
      /// </summary>
      [MarshalAs(CvInvoke.BoolMarshalType)]
      public bool useSurrogates;
      /// <summary>
      /// If true, the tree is truncated a bit more by the pruning procedure. That leads to compact, and more resistant to the training data noise, but a bit less accurate decision tree. 
      /// </summary>
      [MarshalAs(CvInvoke.BoolMarshalType)]
      public bool use1seRule;
      /// <summary>
      /// If true, the cut off nodes (with Tn&lt;=CvDTree::pruned_tree_idx) are physically removed from the tree. Otherwise they are kept, and by decreasing CvDTree::pruned_tree_idx (e.g. setting it to -1) it is still possible to get the results from the original un-pruned (or pruned less aggressively) tree. 
      /// </summary>
      [MarshalAs(CvInvoke.BoolMarshalType)]
      public bool truncatePrunedTree;
      /// <summary>
      /// Another stop criteria - only for regression trees. As soon as the estimated node value differs from the node training samples responses by less than the parameter value, the node is not split further.
      /// </summary>
      public float regressionAccuracy;
      /// <summary>
      /// The array of a priori class probabilities, sorted by the class label value. The parameter can be used to tune the decision tree preferences toward a certain class. For example, if users want to detect some rare anomaly occurrence, the training base will likely contain much more normal cases than anomalies, so a very good classification performance will be achieved just by considering every case as normal. To avoid this, the priors can be specified, where the anomaly probability is artificially increased (up to 0.5 or even greater), so the weight of the misclassified anomalies becomes much bigger, and the tree is adjusted properly. 
      /// </summary>
      /// <remarks>A note about memory management: the field priors  is a pointer to the array of floats. The array should be allocated by user, and released just after the CvDTreeParams structure is passed to CvDTreeTrainData or CvDTree constructors/methods (as the methods make a copy of the array).</remarks>
      public IntPtr priors;
      #endregion

      /// <summary>
      /// Boosting type
      /// </summary>
      public MlEnum.BOOST_TYPE boostType;
      /// <summary>
      /// The number of weak classifiers to build
      /// </summary>
      public int weakCount;
      /// <summary>
      /// Splitting criteria, used to choose optimal splits during a weak tree construction
      /// </summary>
      public int splitCriteria;
      /// <summary>
      /// The weight trimming ratio, within 0..1. See the discussion of it above. If the parameter is ?0 or >1, the trimming is not used, all the samples are used at each iteration. The default value is 0.95
      /// </summary>
      public double weightTrimRate;

   }
}
