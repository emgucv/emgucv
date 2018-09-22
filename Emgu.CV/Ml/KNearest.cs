//----------------------------------------------------------------------------
//  Copyright (C) 2004-2018 by EMGU Corporation. All rights reserved.       
//----------------------------------------------------------------------------

using System;
using System.Diagnostics;
using Emgu.Util;

namespace Emgu.CV.ML
{
    /// <summary>
    /// The KNearest classifier
    /// </summary>
    public partial class KNearest : UnmanagedObject, IStatModel
    {
        private IntPtr _sharedPtr;
        private IntPtr _statModelPtr;
        private IntPtr _algorithmPtr;

        /// <summary>
        /// Create a default KNearest classifier
        /// </summary>
        public KNearest()
        {
            _ptr = MlInvoke.cveKNearestCreate(ref _statModelPtr, ref _algorithmPtr, ref _sharedPtr);
        }

        /// <summary>
        /// Release the classifier and all the memory associated with it
        /// </summary>
        protected override void DisposeObject()
        {
            if (IntPtr.Zero != _ptr)
            {
                MlInvoke.cveKNearestRelease(ref _ptr, ref _sharedPtr);
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
