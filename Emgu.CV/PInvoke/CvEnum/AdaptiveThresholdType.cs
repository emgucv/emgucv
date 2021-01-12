//----------------------------------------------------------------------------
//  Copyright (C) 2004-2021 by EMGU Corporation. All rights reserved.       
//----------------------------------------------------------------------------

using System;

namespace Emgu.CV.CvEnum
{
    /// <summary>
    /// Types of Adaptive Threshold
    /// </summary>
    public enum AdaptiveThresholdType
    {
        /// <summary>
        /// Indicates that Mean minus C should be used for adaptive threshold.
        /// </summary>
        MeanC = 0,
        /// <summary>
        /// Indicates that Gaussian minus C should be used for adaptive threshold.
        /// </summary>
        GaussianC = 1
    }
}
