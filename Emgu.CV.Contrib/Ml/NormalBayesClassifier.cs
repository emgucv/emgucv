//----------------------------------------------------------------------------
//  Copyright (C) 2004-2026 by EMGU Corporation. All rights reserved.       
//----------------------------------------------------------------------------

using System;
using Emgu.Util;

namespace Emgu.CV.ML
{
    /// <summary>
    /// A Normal Bayes Classifier
    /// </summary>
    public class NormalBayesClassifier : UnmanagedObject, IStatModel
    {
        private IntPtr _sharedPtr;

        private IntPtr _statModelPtr;
        private IntPtr _algorithmPtr;

        /// <summary>
        /// Create a normal Bayes classifier
        /// </summary>
        public NormalBayesClassifier()
        {
            _ptr = MlInvoke.cveNormalBayesClassifierDefaultCreate(ref _statModelPtr, ref _algorithmPtr, ref _sharedPtr);
        }

        /// <summary>
        /// Release the memory associated with this classifier
        /// </summary>
        protected override void DisposeObject()
        {
            if (_ptr != IntPtr.Zero)
            {
                MlInvoke.cveNormalBayesClassifierRelease(ref _ptr, ref _sharedPtr);
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
