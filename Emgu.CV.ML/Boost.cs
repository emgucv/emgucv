//----------------------------------------------------------------------------
//  Copyright (C) 2004-2015 by EMGU Corporation. All rights reserved.       
//----------------------------------------------------------------------------

using System;
using Emgu.CV;
using Emgu.CV.Structure;
using Emgu.CV.ML.Structure;
using Emgu.Util;

namespace Emgu.CV.ML
{
   /// <summary>
   /// Boost Tree 
   /// </summary>
   public class Boost : UnmanagedObject, IStatModel
   {
      /// <summary>
      /// Boost Type
      /// </summary>
      public enum Type
      {
         /// <summary>
         /// Discrete AdaBoost.
         /// </summary>
         Discrete=0,
         /// <summary>
         /// Real AdaBoost. It is a technique that utilizes confidence-rated predictions and works well with categorical data.
         /// </summary>
         Real=1,
         /// <summary>
         /// LogitBoost. It can produce good regression fits.
         /// </summary>
         Logit=2,
         /// <summary>
         /// Gentle AdaBoost. It puts less weight on outlier data points and for that reason is often good with regression data.
         /// </summary>
         Gentle=3
      }
      /*
      /// <summary>
      /// Boosting training parameters.
      /// </summary>
      public class Params : UnmanagedObject
      {
         /// <summary>
         /// Initializes a new instance of the <see cref="Params"/> class.
         /// </summary>
         /// <param name="boostType">Type of the boosting algorithm.</param>
         /// <param name="weakCount">The number of weak classifiers.</param>
         /// <param name="weightTrimRate"> A threshold between 0 and 1 used to save computational time. Samples with summary weight &lt;= 1 - weight_trim_rate do not participate in the next iteration of training. Set this parameter to 0 to turn off this functionality</param>
         /// <param name="maxDepth">The maximum depth.</param>
         /// <param name="useSurrogates">if set to <c>true</c> [use surrogates].</param>
         /// <param name="priors">The priors.</param>
         public Params(
            Type boostType = Type.Real, int weakCount = 100, double weightTrimRate = 0.95,
            int maxDepth = 1,
            bool useSurrogates = false, Mat priors = null)
         {
            IntPtr priorsPtr = (priors == null ? IntPtr.Zero : priors.Ptr);
            _ptr = MlInvoke.CvBoostParamsCreate(boostType, weakCount, weightTrimRate, maxDepth, useSurrogates,
               priorsPtr);
         }

         /// <summary>
         /// Release the unmanaged resources
         /// </summary>
         protected override void DisposeObject()
         {
            MlInvoke.CvBoostParamsRelease(ref _ptr);
         }
      }*/

      private IntPtr _statModel;
      private IntPtr _algorithm;

      /// <summary>
      /// Create a default Boost classifier
      /// </summary>
      public Boost()
      {
         _ptr = MlInvoke.cveBoostCreate(ref _statModel, ref _algorithm);
      }

      /// <summary>
      /// Release the Boost classifier and all memory associate with it
      /// </summary>
      protected override void DisposeObject()
      {
         MlInvoke.cveBoostRelease(ref _ptr);
         _statModel = IntPtr.Zero;
         _algorithm = IntPtr.Zero;
      }

      IntPtr IStatModel.StatModelPtr
      {
         get { return _statModel; }
      }

      IntPtr IAlgorithm.AlgorithmPtr
      {
         get { return _algorithm; }
      }
   }
}
