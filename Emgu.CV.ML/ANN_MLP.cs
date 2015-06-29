//----------------------------------------------------------------------------
//  Copyright (C) 2004-2015 by EMGU Corporation. All rights reserved.       
//----------------------------------------------------------------------------

using System;
using System.Runtime.InteropServices;
using Emgu.CV.ML.MlEnum;
using Emgu.CV.ML.Structure;
using Emgu.CV.Structure;
using Emgu.Util;

namespace Emgu.CV.ML
{
   /// <summary>
   /// Neural network
   /// </summary>
   public partial class ANN_MLP : UnmanagedObject, IStatModel
   {
      /// <summary>
      /// Possible activation functions
      /// </summary>
      public enum AnnMlpActivationFunction
      {
         /// <summary>
         /// Identity
         /// </summary>
         Identity = 0,
         /// <summary>
         /// sigmoid symmetric
         /// </summary>
         SigmoidSym = 1,
         /// <summary>
         /// Gaussian
         /// </summary>
         Gaussian = 2
      }

      /// <summary>
      /// Training method for ANN_MLP
      /// </summary>
      public enum AnnMlpTrainMethod
      {
         /// <summary>
         /// Back-propagation algorithm
         /// </summary>
         Backprop = 0,
         /// <summary>
         /// Batch RPROP algorithm
         /// </summary>
         Rprop = 1
      }


      private IntPtr _statModelPtr;
      private IntPtr _algorithmPtr;

      /// <summary>
      /// Create a neural network using the specific parameters
      /// </summary>
      public ANN_MLP()
      {
         _ptr = MlInvoke.cveANN_MLPCreate(ref _statModelPtr, ref _algorithmPtr);
      }


      /// <summary>
      /// Release the memory associated with this neural network
      /// </summary>
      protected override void DisposeObject()
      {
         MlInvoke.cveANN_MLPRelease(ref _ptr);
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
      /// Sets the layer sizes.
      /// </summary>
      /// <param name="layerSizes">Integer vector specifying the number of neurons in each layer including the input and output layers. The very first element specifies the number of elements in the input layer. The last element - number of elements in the output layer.</param>
      public void SetLayerSizes(IInputArray layerSizes)
      {
         using (InputArray iaLayerSizes = layerSizes.GetInputArray())
            MlInvoke.cveANN_MLPSetLayerSizes(_ptr, iaLayerSizes);
      }

      /// <summary>
      /// Initialize the activation function for each neuron.
      /// </summary>
      /// <param name="function">Currently the default and the only fully supported activation function is SigmoidSym </param>
      /// <param name="param1">The first parameter of the activation function.</param>
      /// <param name="param2">The second parameter of the activation function.</param>
      public void SetActivationFunction(ANN_MLP.AnnMlpActivationFunction function, double param1 = 0, double param2 = 0)
      {
         MlInvoke.cveANN_MLPSetActivationFunction(_ptr, function, param1, param2);
      }

      /// <summary>
      /// Sets training method and common parameters.
      /// </summary>
      /// <param name="method">The training method.</param>
      /// <param name="param1">The param1.</param>
      /// <param name="param2">The param2.</param>
      public void SetTrainMethod(ANN_MLP.AnnMlpTrainMethod method = AnnMlpTrainMethod.Rprop, double param1 = 0, double param2 = 0)
      {
         MlInvoke.cveANN_MLPSetTrainMethod(_ptr, method, param1, param2);
      }
   }

   public static partial class MlInvoke
   {

      [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      internal static extern IntPtr cveANN_MLPCreate(
         ref IntPtr statModel,
         ref IntPtr algorithm);

      /// <summary>
      /// Release the ANN_MLP model
      /// </summary>
      /// <param name="model">The ANN_MLP model to be released</param>
      [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      internal static extern void cveANN_MLPRelease(ref IntPtr model);

      [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      internal static extern void cveANN_MLPSetLayerSizes(IntPtr model, IntPtr layerSizes);

      [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      internal static extern void cveANN_MLPSetActivationFunction(IntPtr model, ANN_MLP.AnnMlpActivationFunction type, double param1, double param2);

      [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      internal static extern void cveANN_MLPSetTrainMethod(IntPtr model, ANN_MLP.AnnMlpTrainMethod method, double param1, double param2);


   }
}
