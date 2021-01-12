//----------------------------------------------------------------------------
//  Copyright (C) 2004-2021 by EMGU Corporation. All rights reserved.       
//----------------------------------------------------------------------------

using System;

namespace Emgu.CV.CvEnum
{
    /// <summary>
    /// Type for cvCalcOpticalFlowPyrLK
    /// </summary>
    public enum LKFlowFlag
    {
        /// <summary>
        /// The default type
        /// </summary>
        Default = 0,
        /// <summary>
        /// Uses initial estimations, stored in nextPts; if the flag is not set, then prevPts is copied to nextPts and is considered the initial estimate.
        /// </summary>
        UserInitialFlow = 4,
        /// <summary>
        /// use minimum eigen values as an error measure (see minEigThreshold description); if the flag is not set, then L1 distance between patches around the original and a moved point, divided by number of pixels in a window, is used as a error measure.
        /// </summary>
        LKGetMinEigenvals = 8,
    }
}
