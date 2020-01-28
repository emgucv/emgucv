//----------------------------------------------------------------------------
//  Copyright (C) 2004-2020 by EMGU Corporation. All rights reserved.       
//----------------------------------------------------------------------------

using System;

namespace Emgu.CV.CvEnum
{
    /// <summary>
    /// Calculates fundamental matrix given a set of corresponding points
    /// </summary>
    [Flags]
    public enum FmType
    {
        /// <summary>
        /// for 7-point algorithm. N == 7
        /// </summary>
        SevenPoint = 1,
        /// <summary>
        /// for 8-point algorithm. N &gt;= 8
        /// </summary>
        EightPoint = 2,
        /// <summary>
        /// for LMedS algorithm. N &gt;= 8
        /// </summary>
        LMedsOnly = 4,
        /// <summary>
        /// for RANSAC algorithm. N &gt;= 8
        /// </summary>
        RansacOnly = 8,
        /// <summary>
        /// CV_FM_LMEDS_ONLY | CV_FM_8POINT
        /// </summary>
        LMeds = (LMedsOnly | EightPoint),
        /// <summary>
        /// CV_FM_RANSAC_ONLY | CV_FM_8POINT
        /// </summary>
        Ransac = (RansacOnly | EightPoint)
    }

}
