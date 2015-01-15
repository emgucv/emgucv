//----------------------------------------------------------------------------
//  Copyright (C) 2004-2015 by EMGU Corporation. All rights reserved.       
//----------------------------------------------------------------------------

using System;

namespace Emgu.CV.ML.Structure
{
   /// <summary>
   /// An OpenCV decision Tree Node
   /// </summary>
   public struct MCvDTreeNode
   {
      /// <summary>
      /// The assigned to the node normalized class index (to 0..class_count-1 range), it is used internally in classification trees and tree ensembles.
      /// </summary>
      public int classIdx;
      /// <summary>
      /// The tree index in a ordered sequence of trees. The indices are used during and after the pruning procedure. The root node has the maximum value Tn  of the whole tree, child nodes have Tn less than or equal to the parent's Tn, and the nodes with Tn&lt;=CvDTree::pruned_tree_idx are not taken into consideration at the prediction stage (the corresponding branches are considered as cut-off), even if they have not been physically deleted from the tree at the pruning stage. 
      /// </summary>
      public int Tn;
      /// <summary>
      /// The value assigned to the tree node. It is either a class label, or the estimated function value.
      /// </summary>
      public double value;

      /// <summary>
      /// Pointer to the parent tree node
      /// </summary>
      public IntPtr parent;
      /// <summary>
      /// Pointer to the left tree node
      /// </summary>
      public IntPtr left;
      /// <summary>
      /// Pointer to the right tree node
      /// </summary>
      public IntPtr right;

      /// <summary>
      /// Pointer to CvDTreeSplit
      /// </summary>
      public IntPtr split;

      /// <summary>
      /// The number of samples that fall into the node at the training stage. It is used to resolve the difficult cases - when the variable for the primary split is missing, and all the variables for other surrogate splits are missing too,
      ///the sample is directed to the left if left-&gt;sample_count&gt;right-&gt;sample_count and to the right otherwise
      /// </summary>
      public int sampleCount;
      /// <summary>
      /// The node depth, the root node depth is 0, the child nodes depth is the parent's depth + 1. 
      /// </summary>
      public int depth;

      #region internal parameters
      /// <summary>
      /// Internal parameters
      /// </summary>
      public IntPtr numValid;
      /// <summary>
      /// Internal parameters
      /// </summary>
      public int offset;
      /// <summary>
      /// Internal parameters
      /// </summary>
      public int bufIdx;
      /// <summary>
      /// Internal parameters
      /// </summary>
      public double maxlr;

      /// <summary>
      /// Global pruning data
      /// </summary>
      public int complexity;
      /// <summary>
      /// Global pruning data
      /// </summary>
      public double alpha;
      /// <summary>
      /// Global pruning data
      /// </summary>
      public double nodeRisk;
      
      /// <summary>
      /// Global pruning data
      /// </summary>
      public double treeRisk;

      /// <summary>
      /// Global pruning data
      /// </summary>
      public double treeError;

      /// <summary>
      /// Cross-validation pruning data
      /// </summary>
      public IntPtr cvTn;
      /// <summary>
      /// Cross-validation pruning data
      /// </summary>
      public IntPtr cvNodeRisk;
      /// <summary>
      /// Cross-validation pruning data
      /// </summary>
      public IntPtr cvNodeError;
      #endregion 
   }
}
