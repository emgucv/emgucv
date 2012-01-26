//----------------------------------------------------------------------------
//  Copyright (C) 2004-2012 by EMGU. All rights reserved.       
//----------------------------------------------------------------------------

using System;
using Emgu.CV.Structure;

namespace Emgu.CV.ML.Structure
{
   /// <summary>
   /// Parameters for Artificla Neural Network - MultiLayer Perceptron
   /// </summary>
   public struct MCvANN_MLP_TrainParams
   {
      /// <summary>
      /// The termination criteria
      /// </summary>
      public MCvTermCriteria term_crit;
      /// <summary>
      /// The type of training method
      /// </summary>
      public MlEnum.ANN_MLP_TRAIN_METHOD train_method;

      /// <summary>
      /// backpropagation parameters
      /// </summary>
      public double bp_dw_scale;
      /// <summary>
      /// backpropagation parameters
      /// </summary>
      public double bp_moment_scale;

      /// <summary>
      /// rprop parameters
      /// </summary>
      public double rp_dw0;
      /// <summary>
      /// rprop parameters
      /// </summary>
      public double rp_dw_plus;
      /// <summary>
      /// rprop parameters
      /// </summary>
      public double rp_dw_minus;
      /// <summary>
      /// rprop parameters
      /// </summary>
      public double rp_dw_min;
      /// <summary>
      /// rprop parameters
      /// </summary>
      public double rp_dw_max;
   }
}
