//----------------------------------------------------------------------------
//  Copyright (C) 2004-2026 by EMGU Corporation. All rights reserved.       
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
        RHO = 16,
        /// <summary>
        /// USAC algorithm, default settings
        /// </summary>
        UsacDefault = 32,
        /// <summary>
        /// USAC, parallel version
        /// </summary>
        UsacParallel = 33,
        /// <summary>
        /// USAC, fundamental matrix 8 points
        /// </summary>
        UsacFm8Pts = 34,
        /// <summary>
        /// USAC, fast settings
        /// </summary>
        UsacFast = 35,
        /// <summary>
        /// USAC, accurate settings
        /// </summary>
        UsacAccurate = 36,
        /// <summary>
        /// USAC, sorted points, runs PROSAC
        /// </summary>
        UsacProsac = 37,
        /// <summary>
        /// USAC, runs MAGSAC++
        /// </summary>
        UsacMagsac = 38
    }
}
