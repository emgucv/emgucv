//----------------------------------------------------------------------------
//  Copyright (C) 2004-2021 by EMGU Corporation. All rights reserved.       
//----------------------------------------------------------------------------

using System;
using System.Runtime.InteropServices;
using Emgu.CV.ML.MlEnum;
using Emgu.CV.Structure;
using Emgu.Util;

namespace Emgu.CV.ML
{
    /// <summary>
    /// Neural network
    /// </summary>
    public partial class ANN_MLP : UnmanagedObject, IStatModel
    {
        private IntPtr _sharedPtr;

        /// <summary>
        /// Possible activation functions
        /// </summary>
        public enum AnnMlpActivationFunction
        {
            /// <summary>
            /// Identity function: f(x)=x
            /// </summary>
            Identity = 0,
            /// <summary>
            /// Symmetrical sigmoid: f(x)=beta*(1-e^{-alpha x})/(1+e^{-alpha x})
            /// </summary>
            /// <remarks>If you are using the default sigmoid activation function with the default parameter values
            /// fparam1 = 0 and fparam2 = 0 then the function used is y = 1.7159 * tanh(2/3 * x), so the output
            /// will range from[-1.7159, 1.7159], instead of[0, 1].
            /// </remarks>
            SigmoidSym = 1,
            /// <summary>
            /// Gaussian function: f(x)=beta e^{-alpha x*x}
            /// </summary>
            Gaussian = 2,
            /// <summary>
            /// ReLU function: f(x)=max(0,x)
            /// </summary>
            Relu = 3,
            /// <summary>
            /// Leaky ReLU function:
            /// for x>0, $f(x)=x;
            /// and x&lt;=0, f(x)=alpha x 
            /// </summary>
            LeakyRelu = 4

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
            Rprop = 1,
            /// <summary>
            /// The simulated annealing algorithm.
            /// </summary>
            Anneal = 2
        }


        private IntPtr _statModelPtr;
        private IntPtr _algorithmPtr;

        /// <summary>
        /// Create a neural network using the specific parameters
        /// </summary>
        public ANN_MLP()
        {
            _ptr = MlInvoke.cveANN_MLPCreate(ref _statModelPtr, ref _algorithmPtr, ref _sharedPtr);
        }


        /// <summary>
        /// Release the memory associated with this neural network
        /// </summary>
        protected override void DisposeObject()
        {
            if (IntPtr.Zero != _ptr)
            {
                MlInvoke.cveANN_MLPRelease(ref _ptr, ref _sharedPtr);
                _statModelPtr = IntPtr.Zero;
                _algorithmPtr = IntPtr.Zero;
            }
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
        /// <param name="param1">param1 passed to setRpropDW0 for ANN_MLP::RPROP and to setBackpropWeightScale for ANN_MLP::BACKPROP and to initialT for ANN_MLP::ANNEAL.</param>
        /// <param name="param2">param2 passed to setRpropDWMin for ANN_MLP::RPROP and to setBackpropMomentumScale for ANN_MLP::BACKPROP and to finalT for ANN_MLP::ANNEAL.</param>
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
           ref IntPtr algorithm, 
           ref IntPtr sharedPtr);

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern void cveANN_MLPRelease(ref IntPtr model, ref IntPtr sharedPtr);

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern void cveANN_MLPSetLayerSizes(IntPtr model, IntPtr layerSizes);

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern void cveANN_MLPSetActivationFunction(IntPtr model, ANN_MLP.AnnMlpActivationFunction type, double param1, double param2);

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern void cveANN_MLPSetTrainMethod(IntPtr model, ANN_MLP.AnnMlpTrainMethod method, double param1, double param2);


    }
}
