//----------------------------------------------------------------------------
//  Copyright (C) 2004-2015 by EMGU Corporation. All rights reserved.       
//----------------------------------------------------------------------------

using System;

namespace Emgu.CV.ML.MlEnum
{




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
      ColSample = 1,
      /// <summary>
      /// Feature vectors are stored as rows
      /// </summary>
      RowSample =0
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
