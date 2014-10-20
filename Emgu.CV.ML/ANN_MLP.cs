//----------------------------------------------------------------------------
//  Copyright (C) 2004-2014 by EMGU Corporation. All rights reserved.       
//----------------------------------------------------------------------------

using System;
using Emgu.CV.ML.MlEnum;
using Emgu.CV.ML.Structure;
using Emgu.CV.Structure;
using Emgu.Util;

namespace Emgu.CV.ML
{
   /// <summary>
   /// Neural network
   /// </summary>
   public class ANN_MLP : UnmanagedObject, IStatModel
   {
      private IntPtr _statModelPtr;
      private IntPtr _algorithmPtr;

      /// <summary>
      /// Create a neural network using the specific parameters
      /// </summary>
      public ANN_MLP(Params p)
      {
         _ptr = MlInvoke.CvANN_MLPCreate(p, ref _statModelPtr, ref _algorithmPtr);
      }

      /// <summary>
      /// Release the memory associated with this neural network
      /// </summary>
      protected override void DisposeObject()
      {
         MlInvoke.CvANN_MLPRelease(ref _ptr);
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

      public class Params : UnmanagedObject
      {
         public Params(
            Mat layerSizes, AnnMlpActivationFunction activateFunc, double fparam1, double fparam2,
            MCvTermCriteria termCrit, AnnMlpTrainMethod trainMethod, double param1, double param2)
         {
            _ptr = MlInvoke.CvANN_MLPParamsCreate(layerSizes, activateFunc, fparam1, fparam2, ref termCrit, trainMethod, param1,
               param2);
         }

         protected override void DisposeObject()
         {
            MlInvoke.CvANN_MLPParamsRelease(ref  _ptr);
         }
      }
   }
}
