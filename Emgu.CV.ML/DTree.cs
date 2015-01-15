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

      public class Params : UnmanagedObject
      {
         public Params(int maxDepth, int minSampleCount,
            double regressionAccuracy, bool useSurrogates,
            int maxCategories, int CvFolds,
            bool use1SERule, bool truncatePrunedTree,
            Mat priors = null)
         {
            _ptr = MlInvoke.CvDTreeParamsCreate(
               maxDepth, minSampleCount,
               regressionAccuracy, useSurrogates,
               maxCategories, CvFolds, use1SERule, truncatePrunedTree,
               priors ?? IntPtr.Zero);
         }

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
