//----------------------------------------------------------------------------
//  Copyright (C) 2004-2026 by EMGU Corporation. All rights reserved.       
//----------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using Emgu.CV.Structure;
using Emgu.CV.Text;
using Emgu.CV.Util;
using Emgu.Util;
using System.Diagnostics;
using System.Drawing;
using Emgu.CV.CvEnum;

namespace Emgu.CV.Dai
{

    /// <summary>
    /// DepthAI NeuralNetwork
    /// </summary>
    public partial class NeuralNetwork : SharedPtrObject, INode
    {
        private IntPtr _nodePtr;

        internal NeuralNetwork(IntPtr neuralNetworkPtr, IntPtr neuralNetworkSharedPtr, IntPtr nodePtr)
        {
            _ptr = neuralNetworkPtr;
            _sharedPtr = neuralNetworkSharedPtr;
            _nodePtr = nodePtr;
        }

        /// <summary>
        /// Release all unmanaged memory associated with the NeuralNetwork.
        /// </summary>
        protected override void DisposeObject()
        {
            if (_sharedPtr != IntPtr.Zero)
            {
                DaiInvoke.daiNeuralNetworkRelease(ref _sharedPtr);
				_ptr = IntPtr.Zero;
                _nodePtr = IntPtr.Zero;
            }
        }

        /// <summary>
        /// Retrieves the input node of the neural network.
        /// </summary>
        /// <returns>
        /// A <see cref="NodeInput"/> object representing the input node of the neural network.
        /// </returns>
        public NodeInput GetInput()
        {
            return new NodeInput(DaiInvoke.daiNeuralNetworkGetInput(_ptr), false);
        }

        /// <summary>
        /// Sets the path to the blob file for the neural network.
        /// </summary>
        /// <param name="path">The string representing the path to the blob file.</param>
        public void SetBlobPath(String path)
        {
            using (CvString csPath = new CvString(path))
            {
                DaiInvoke.daiNeuralNetworkSetBlobPath(_ptr, csPath);
            }
        }

        /// <summary>
        /// Gets the pointer to the underlying native node object in the neural network.
        /// </summary>
        /// <value>
        /// The pointer to the native node object.
        /// </value>
        public IntPtr NodePtr
        {
            get { return _nodePtr; }
        }
    }

    /// <summary>
    /// Entry points for the DepthAI module.
    /// </summary>
    public static partial class DaiInvoke
    {
        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern void daiNeuralNetworkRelease(ref IntPtr neuralNetworkSharedPtr);

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern IntPtr daiNeuralNetworkGetInput(IntPtr neuralNetwork);

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern void daiNeuralNetworkSetBlobPath(IntPtr neuralNetwork, IntPtr path);
    }
}