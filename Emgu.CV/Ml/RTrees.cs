//----------------------------------------------------------------------------
//  Copyright (C) 2004-2021 by EMGU Corporation. All rights reserved.       
//----------------------------------------------------------------------------

using System;
using Emgu.CV;
using Emgu.Util;
using Emgu.CV.Structure;
using System.Runtime.InteropServices;
using System.Diagnostics;

namespace Emgu.CV.ML
{
    /// <summary>
    /// Random trees
    /// </summary>
    public partial class RTrees : UnmanagedObject, IStatModel
    {
        private IntPtr _sharedPtr;

        private IntPtr _statModelPtr;
        private IntPtr _algorithmPtr;

        /// <summary>
        /// Create a random tree
        /// </summary>
        public RTrees()
        {
            _ptr = MlInvoke.cveRTreesCreate(ref _statModelPtr, ref _algorithmPtr, ref _sharedPtr);
        }

        /// <summary>
        /// Returns the result of each individual tree in the forest.
        /// In case the model is a regression problem, the method will return each of the trees'
        /// results for each of the sample cases.If the model is a classifier, it will return
        /// a Mat with samples + 1 rows, where the first row gives the class number and the
        /// following rows return the votes each class had for each sample.
        /// </summary>
        /// <param name="samples">Array containing the samples for which votes will be calculated.</param>
        /// <param name="results">Array where the result of the calculation will be written.</param>
        /// <param name="flags">Flags for defining the type of RTrees.</param>
        public void GetVotes(IInputArray samples, IOutputArray results, DTrees.Flags flags)
        {
            using (InputArray iaSamples = samples.GetInputArray())
            using (OutputArray oaResults = results.GetOutputArray())
                MlInvoke.cveRTreesGetVotes(_ptr, iaSamples, oaResults, flags);
        }

        /// <summary>
        /// Release the random tree and all memory associate with it
        /// </summary>
        protected override void DisposeObject()
        {
            if (_ptr != IntPtr.Zero)
            {
                MlInvoke.cveRTreesRelease(ref _ptr, ref _sharedPtr);
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
