//----------------------------------------------------------------------------
//  Copyright (C) 2004-2020 by EMGU Corporation. All rights reserved.       
//----------------------------------------------------------------------------

using System;
using Emgu.CV;
using Emgu.CV.Structure;
using Emgu.Util;

namespace Emgu.CV.ML
{
    /// <summary>
    /// Boost Tree 
    /// </summary>
    partial class Boost : UnmanagedObject, IStatModel
    {
        private IntPtr _sharedPtr;

        /// <summary>
        /// Boost Type
        /// </summary>
        public enum Type
        {
            /// <summary>
            /// Discrete AdaBoost.
            /// </summary>
            Discrete = 0,
            /// <summary>
            /// Real AdaBoost. It is a technique that utilizes confidence-rated predictions and works well with categorical data.
            /// </summary>
            Real = 1,
            /// <summary>
            /// LogitBoost. It can produce good regression fits.
            /// </summary>
            Logit = 2,
            /// <summary>
            /// Gentle AdaBoost. It puts less weight on outlier data points and for that reason is often good with regression data.
            /// </summary>
            Gentle = 3
        }

        private IntPtr _statModel;
        private IntPtr _algorithm;

        /// <summary>
        /// Create a default Boost classifier
        /// </summary>
        public Boost()
        {
            _ptr = MlInvoke.cveBoostCreate(ref _statModel, ref _algorithm, ref _sharedPtr);
        }

        /// <summary>
        /// Release the Boost classifier and all memory associate with it
        /// </summary>
        protected override void DisposeObject()
        {
            if (IntPtr.Zero != _ptr)
            {
                MlInvoke.cveBoostRelease(ref _ptr, ref _sharedPtr);
                _statModel = IntPtr.Zero;
                _algorithm = IntPtr.Zero;
            }
        }

        IntPtr IStatModel.StatModelPtr
        {
            get { return _statModel; }
        }

        IntPtr IAlgorithm.AlgorithmPtr
        {
            get { return _algorithm; }
        }
    }
}
