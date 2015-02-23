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
   public partial class LogisticRegression : UnmanagedObject, IStatModel
   {
      private IntPtr _statModelPtr;
      private IntPtr _algorithmPtr;

      /// <summary>
      /// Specifies the kind of training method used.
      /// </summary>
      public enum TrainType
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
      /// Initializes a new instance of the <see cref="LogisticRegression"/> class.
      /// </summary>
      /// <param name="p">The parameters.</param>
      public LogisticRegression()
      {
         _ptr = MlInvoke.cveLogisticRegressionCreate(ref _statModelPtr, ref _algorithmPtr);
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
