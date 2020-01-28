//----------------------------------------------------------------------------
//  Copyright (C) 2004-2020 by EMGU Corporation. All rights reserved.       
//----------------------------------------------------------------------------

using System;
using Emgu.CV;
using Emgu.Util;

namespace Emgu.CV.ML
{
    /// <summary>
    /// Decision Trees 
    /// </summary>
    public partial class DTrees : UnmanagedObject, IStatModel
    {
        private IntPtr _sharedPtr;

        private IntPtr _statModelPtr;
        private IntPtr _algorithmPtr;

        /// <summary>
        /// Predict options
        /// </summary>
        public enum Flags
        {
            /// <summary>
            /// Predict auto
            /// </summary>
            PredictAuto = 0,
            /// <summary>
            /// Predict sum
            /// </summary>
            PredictSum = (1 << 8),
            /// <summary>
            /// Predict max vote
            /// </summary>
            PredictMaxVote = (2 << 8),
            /// <summary>
            /// Predict mask
            /// </summary>
            PredictMask = (3 << 8)
        }

        /// <summary>
        /// Create a default decision tree
        /// </summary>
        public DTrees()
        {
            _ptr = MlInvoke.cveDTreesCreate(ref _statModelPtr, ref _algorithmPtr, ref _sharedPtr);
        }

        /// <summary>
        /// Release the decision tree and all the memory associate with it
        /// </summary>
        protected override void DisposeObject()
        {
            if (IntPtr.Zero != _ptr)
            {
                MlInvoke.cveDTreesRelease(ref _ptr, ref _sharedPtr);
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
