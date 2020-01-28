//----------------------------------------------------------------------------
//  Copyright (C) 2004-2020 by EMGU Corporation. All rights reserved.       
//----------------------------------------------------------------------------

using System;

namespace Emgu.CV.CvEnum
{
    /// <summary>
    /// Type of Robust Estimation Algorithm
    /// </summary>
    public enum RobustEstimationAlgorithm
    {
        /// <summary>
        /// regular method using all the point pairs
        /// </summary>
        AllPoints = 0,
        /// <summary>
        /// Least-Median robust method
        /// </summary>
        LMEDS = 4,
        /// <summary>
        /// RANSAC-based robust method
        /// </summary>
        Ransac = 8,
        /// <summary>
        /// RHO algorithm
        /// </summary>
        RHO = 16
    }
}
