//----------------------------------------------------------------------------
//  Copyright (C) 2004-2020 by EMGU Corporation. All rights reserved.       
//----------------------------------------------------------------------------

using System;
using Emgu.CV.ML.MlEnum;
using Emgu.Util;
using System.Runtime.InteropServices;

namespace Emgu.CV.ML
{
    /// <summary>
    /// Train data
    /// </summary>
    public class TrainData : Emgu.CV.Util.SharedPtrObject
    {

        /// <summary>
        /// Creates training data from in-memory arrays.
        /// </summary>
        /// <param name="samples">Matrix of samples. It should have CV_32F type.</param>
        /// <param name="layoutType">Type of the layout.</param>
        /// <param name="response">Matrix of responses. If the responses are scalar, they should be stored as a single row or as a single column. The matrix should have type CV_32F or CV_32S (in the former case the responses are considered as ordered by default; in the latter case - as categorical)</param>
        /// <param name="varIdx">Vector specifying which variables to use for training. It can be an integer vector (CV_32S) containing 0-based variable indices or byte vector (CV_8U) containing a mask of active variables.</param>
        /// <param name="sampleIdx">Vector specifying which samples to use for training. It can be an integer vector (CV_32S) containing 0-based sample indices or byte vector (CV_8U) containing a mask of training samples.</param>
        /// <param name="sampleWeight">Optional vector with weights for each sample. It should have CV_32F type.</param>
        /// <param name="varType">Optional vector of type CV_8U and size &lt;number_of_variables_in_samples&gt; + &lt;number_of_variables_in_responses&gt;, containing types of each input and output variable.</param>
        public TrainData(
           IInputArray samples, DataLayoutType layoutType, IInputArray response,
           IInputArray varIdx = null, IInputArray sampleIdx = null,
           IInputArray sampleWeight = null, IInputArray varType = null
           )
        {
            using (InputArray iaSamples = samples.GetInputArray())
            using (InputArray iaResponse = response.GetInputArray())
            using (InputArray iaVarIdx = varIdx == null ? InputArray.GetEmpty() : varIdx.GetInputArray())
            using (InputArray iaSampleIdx = sampleIdx == null ? InputArray.GetEmpty() : sampleIdx.GetInputArray())
            using (InputArray iaSampleWeight = sampleWeight == null ? InputArray.GetEmpty() : sampleWeight.GetInputArray())
            using (InputArray iaVarType = varType == null ? InputArray.GetEmpty() : varType.GetInputArray())
            {
                _ptr = MlInvoke.cveTrainDataCreate(
                    iaSamples, layoutType, iaResponse, iaVarIdx, iaSampleIdx, iaSampleWeight,
                    iaVarType, ref _sharedPtr);
            }
        }

        /// <summary>
        /// Release the unmanaged resources
        /// </summary>
        protected override void DisposeObject()
        {
            if (IntPtr.Zero != _sharedPtr)
            {
                MlInvoke.cveTrainDataRelease(ref _sharedPtr);
                _ptr = IntPtr.Zero;
            }
        }
    }
}
