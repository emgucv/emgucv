//----------------------------------------------------------------------------
//  Copyright (C) 2004-2014 by EMGU Corporation. All rights reserved.       
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
      public class Params : UnmanagedObject
      {
         public Params(
            int boostType, int weakCount, double weightTrimRate,
            int maxDepth,
            bool useSurrogates, Mat priors = null)
         {
            _ptr = MlInvoke.CvBoostParamsCreate(boostType, weakCount, weightTrimRate, maxDepth, useSurrogates,
               priors ?? IntPtr.Zero);
         }


         protected override void DisposeObject()
         {
            MlInvoke.CvBoostParamsRelease(ref _ptr);
         }
      }

      private IntPtr _statModel;
      private IntPtr _algorithm;

      /// <summary>
      /// Create a default Boost classifier
      /// </summary>
      public Boost(Params p)
      {
         _ptr = MlInvoke.CvBoostCreate(p, ref _statModel, ref _algorithm);
      }

      /// <summary>
      /// Release the Boost classifier and all memory associate with it
      /// </summary>
      protected override void DisposeObject()
      {
         MlInvoke.CvBoostRelease(ref _ptr);
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
