//----------------------------------------------------------------------------
//  Copyright (C) 2004-2021 by EMGU Corporation. All rights reserved.       
//----------------------------------------------------------------------------

using System;

namespace Emgu.CV.CvEnum
{
    /// <summary>
    /// cvCalcCovarMatrix method types
    /// </summary>
    [Flags]
    public enum CovarMethod
    {
        /// <summary>
        /// Calculates covariation matrix for a set of vectors 
        /// transpose([v1-avg, v2-avg,...]) * [v1-avg,v2-avg,...] 
        /// </summary>
        Scrambled = 0,

        /// <summary>
        /// [v1-avg, v2-avg,...] * transpose([v1-avg,v2-avg,...])
        /// </summary>
        Normal = 1,

        /// <summary>
        /// Do not calc average (i.e. mean vector) - use the input vector instead
        /// (useful for calculating covariance matrix by parts)
        /// </summary>
        UseAvg = 2,

        /// <summary>
        /// Scale the covariance matrix coefficients by number of the vectors
        /// </summary>
        Scale = 4,

        /// <summary>
        /// All the input vectors are stored in a single matrix, as its rows 
        /// </summary>
        Rows = 8,

        /// <summary>
        /// All the input vectors are stored in a single matrix, as its columns
        /// </summary>
        Cols = 16,
    }
}
