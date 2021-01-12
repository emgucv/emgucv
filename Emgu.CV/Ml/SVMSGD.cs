//----------------------------------------------------------------------------
//  Copyright (C) 2004-2021 by EMGU Corporation. All rights reserved.       
//----------------------------------------------------------------------------

using System;
using Emgu.CV.CvEnum;
using Emgu.CV.ML.MlEnum;
using Emgu.CV.Structure;
using Emgu.Util;
using System.Runtime.InteropServices;

namespace Emgu.CV.ML
{
    /// <summary>
    /// Support Vector Machine 
    /// </summary>
    public partial class SVMSGD : UnmanagedObject, IStatModel
    {
        private IntPtr _sharedPtr;

        /// <summary>
        /// SVMSGD type.
        /// ASGD is often the preferable choice.
        /// </summary>
        public enum SvmsgdType
        {
            /// <summary>
            /// Stochastic Gradient Descent
            /// </summary>
            Sgd,
            /// <summary>
            /// Average Stochastic Gradient Descent
            /// </summary>
            Asgd 
        }
        
        /// <summary>
        /// Margin type
        /// </summary>
        public enum MarginType
        {
            /// <summary>
            /// General case, suits to the case of non-linearly separable sets, allows outliers.
            /// </summary>
            SoftMargin,
            /// <summary>
            /// More accurate for the case of linearly separable sets.
            /// </summary>
            HardMargin  
        }

        private IntPtr _statModelPtr;
        private IntPtr _algorithmPtr;

        /// <summary>
        /// Create a support Vector Machine
        /// </summary>
        public SVMSGD()
        {
            _ptr = MlInvoke.cveSVMSGDDefaultCreate(ref _statModelPtr, ref _algorithmPtr, ref _sharedPtr);
        }

        /// <summary>
        /// Set the optimal parameters for the given model type
        /// </summary>
        /// <param name="svmsgdType">SVMSGD type</param>
        /// <param name="marginType">Margin type</param>
        public void SetOptimalParameters(
            Emgu.CV.ML.SVMSGD.SvmsgdType svmsgdType,
            Emgu.CV.ML.SVMSGD.MarginType marginType)
        {
            MlInvoke.cveSVMSGDSetOptimalParameters(_ptr, svmsgdType, marginType);
        }

        /// <summary>
        /// Release all the memory associated with the SVMSGD model
        /// </summary>
        protected override void DisposeObject()
        {
            if (IntPtr.Zero != _ptr)
            {
                MlInvoke.cveSVMSGDRelease(ref _ptr, ref _sharedPtr);
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
    }
}
