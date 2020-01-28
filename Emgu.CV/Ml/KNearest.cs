//----------------------------------------------------------------------------
//  Copyright (C) 2004-2020 by EMGU Corporation. All rights reserved.       
//----------------------------------------------------------------------------

using System;
using System.Diagnostics;
using Emgu.CV.Util;
using Emgu.Util;

namespace Emgu.CV.ML
{
    /// <summary>
    /// The KNearest classifier
    /// </summary>
    public partial class KNearest : SharedPtrObject, IStatModel
    {
        /// <summary>
        /// The type of KNearest search
        /// </summary>
        public enum Types
        {
            /// <summary>
            /// Using brute force
            /// </summary>
            BruteForce = 1,
            /// <summary>
            /// Using kd tree
            /// </summary>
            KdTree = 2
        }

        
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
            if (IntPtr.Zero != _sharedPtr)
            {
                MlInvoke.cveKNearestRelease(ref _sharedPtr);
                _ptr = IntPtr.Zero;
                _statModelPtr = IntPtr.Zero;
                _algorithmPtr = IntPtr.Zero;
            }
        }

        /// <summary>
        /// Finds the neighbors and predicts responses for input vectors.
        /// </summary>
        /// <param name="samples">Input samples stored by rows. It is a single-precision floating-point matrix of &lt;number_of_samples&gt; * k size.</param>
        /// <param name="k">Number of used nearest neighbors. Should be greater than 1.</param>
        /// <param name="results">Vector with results of prediction (regression or classification) for each input sample. It is a single-precision floating-point vector with &lt;number_of_samples&gt; elements.</param>
        /// <param name="neighborResponses">Optional output values for corresponding neighbors. It is a single- precision floating-point matrix of &lt;number_of_samples&gt; * k size.</param>
        /// <param name="dist">Optional output distances from the input vectors to the corresponding neighbors. It is a single-precision floating-point matrix of &lt;number_of_samples&gt; * k size.</param>
        /// <returns>If only a single input vector is passed, the predicted value is returned by the method.</returns>
        public float FindNearest(
            IInputArray samples,
            int k,
            IOutputArray results,
            IOutputArray neighborResponses = null,
            IOutputArray dist = null)
        {
            using (InputArray iaSamples = samples.GetInputArray())
            using (OutputArray oaResults = results.GetOutputArray())
            using (OutputArray oaNeighborResponses = neighborResponses == null ? OutputArray.GetEmpty() : neighborResponses.GetOutputArray())
            using (OutputArray oaDist = dist == null ? OutputArray.GetEmpty() : dist.GetOutputArray())
            {
                return MlInvoke.cveKNearestFindNearest(
                    _ptr,
                    iaSamples,
                    k,
                    oaResults,
                    oaNeighborResponses,
                    oaDist);
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
