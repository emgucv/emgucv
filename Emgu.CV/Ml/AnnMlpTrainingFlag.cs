//----------------------------------------------------------------------------
//  Copyright (C) 2004-2020 by EMGU Corporation. All rights reserved.       
//----------------------------------------------------------------------------

using System;

namespace Emgu.CV.ML.MlEnum
{
    /// <summary>
    /// The flags for the neural network training function
    /// </summary>
    [Flags]
    public enum AnnMlpTrainingFlag
    {
        /// <summary>
        /// Default
        /// </summary>
        Default = 0,
        /// <summary>
        /// Update weights
        /// </summary>
        UpdateWeights = 1,
        /// <summary>
        /// No input scale
        /// </summary>
        NoInputScale = 2,
        /// <summary>
        /// No output scale
        /// </summary>
        NoOutputScale = 4
    }

}
