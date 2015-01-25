//----------------------------------------------------------------------------
//  Copyright (C) 2004-2015 by EMGU Corporation. All rights reserved.       
//----------------------------------------------------------------------------

using System;
using System.Diagnostics;
using Emgu.CV.Structure;
using Emgu.Util;

namespace Emgu.CV.ML
{
   /// <summary>
   /// ML implements logistic regression, which is a probabilistic classification technique. 
   /// </summary>
   public class LogisticRegression : UnmanagedObject, IStatModel
   {
      private IntPtr _statModelPtr;
      private IntPtr _algorithmPtr;

      /// <summary>
      /// Specifies the kind of training method used.
      /// </summary>
      public enum TrainMethod
      {
         Batch,
         MiniBatch
      }

      /// <summary>
      /// Specifies the kind of regularization to be applied. 
      /// </summary>
      public enum NormalizationMethod
      {
         L1,
         L2
      }

      /// <summary>
      /// Parameters of the Logistic Regression training algorithm.
      /// </summary>
      public class Params : UnmanagedObject
      {
         /// <summary>
         /// Initializes a new instance of the <see cref="Params"/> class.
         /// </summary>
         /// <param name="learningRate">Specifies the learning rate.</param>
         /// <param name="iters">Specifies the number of iterations.</param>
         /// <param name="method">Specifies the kind of training method used.</param>
         /// <param name="normalization">Specifies the kind of regularization to be applied. </param>
         /// <param name="reg">To enable or disable regularization. Set to positive integer (greater than zero) to enable and to 0 to disable.</param>
         /// <param name="batchSize">Specifies the number of training samples taken in each step of Mini-Batch Gradient Descent. Will only be used if using LogisticRegression::MINI_BATCH training algorithm. It has to take values less than the total number of training samples.</param>
         public Params(
            double learningRate = 0.001,
            int iters = 1000,
            TrainMethod method = TrainMethod.Batch,
            NormalizationMethod normalization = NormalizationMethod.L2,
            int reg = 1,
            int batchSize = 1)
         {
            _ptr = MlInvoke.cveLogisticRegressionParamsCreate(learningRate, iters, method, normalization, reg, batchSize);
         }

         /// <summary>
         /// Release the unmanaged resources
         /// </summary>
         protected override void DisposeObject()
         {
            MlInvoke.cveLogisticRegressionParamsRelease(ref _ptr);
         }
      }

      /// <summary>
      /// Initializes a new instance of the <see cref="LogisticRegression"/> class.
      /// </summary>
      /// <param name="p">The parameters.</param>
      public LogisticRegression(Params p)
      {
         _ptr = MlInvoke.cveLogisticRegressionCreate(p, ref _statModelPtr, ref _algorithmPtr);
      }

      /// <summary>
      /// Return the pointer to the StatModel object
      /// </summary>
      public IntPtr StatModelPtr
      {
         get { return _statModelPtr; }
      }

      /// <summary>
      /// Return the pointer to the algorithm object
      /// </summary>
      public IntPtr AlgorithmPtr
      {
         get { return _algorithmPtr; }
      }

      /// <summary>
      /// Release the unmanaged resources
      /// </summary>
      protected override void DisposeObject()
      {
         if (_ptr != IntPtr.Zero)
            MlInvoke.cveLogisticRegressionRelease(ref _ptr);
      }
   }
}
