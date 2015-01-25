//----------------------------------------------------------------------------
//  Copyright (C) 2004-2015 by EMGU Corporation. All rights reserved.       
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

      /// <summary>
      /// Parameters of the MLP and of the training algorithm. 
      /// </summary>
      public class Params : UnmanagedObject
      {
         /// <summary>
         /// Initializes a new instance of the <see cref="Params"/> class.
         /// </summary>
         /// <param name="layerSizes">Integer vector specifying the number of neurons in each layer including the input and output layers.</param>
         /// <param name="activateFunc">Parameter specifying the activation function for each neuron.</param>
         /// <param name="fparam1">The first parameter of activation function.</param>
         /// <param name="fparam2">The second parameter of the activation function.</param>
         /// <param name="termCrit">Termination criteria of the training algorithm. You can specify the maximum number of iterations (maxCount) and/or how much the error could change between the iterations to make the algorithm continue (epsilon).</param>
         /// <param name="trainMethod">Training method of the MLP.</param>
         /// <param name="param1">Parameter of the training method. It is rp_dw0 for RPROP and bp_dw_scale for BACKPROP.</param>
         /// <param name="param2">Parameter of the training method. It is rp_dw_min for RPROP and bp_moment_scale for BACKPROP.</param>
         public Params(
            Mat layerSizes, AnnMlpActivationFunction activateFunc, double fparam1, double fparam2,
            MCvTermCriteria termCrit, AnnMlpTrainMethod trainMethod, double param1, double param2)
         {
            _ptr = MlInvoke.CvANN_MLPParamsCreate(layerSizes, activateFunc, fparam1, fparam2, ref termCrit, trainMethod, param1,
               param2);
         }

         /// <summary>
         /// Release the unmanaged resources
         /// </summary>
         protected override void DisposeObject()
         {
            MlInvoke.CvANN_MLPParamsRelease(ref  _ptr);
         }
      }
   }
}
