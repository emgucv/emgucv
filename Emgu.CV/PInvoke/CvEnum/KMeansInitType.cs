//----------------------------------------------------------------------------
//  Copyright (C) 2004-2020 by EMGU Corporation. All rights reserved.       
//----------------------------------------------------------------------------

using System;

namespace Emgu.CV.CvEnum
{
    /// <summary>
    /// KMeans initialization type
    /// </summary>
    public enum KMeansInitType
    {
        /// <summary>
        /// Chooses random centers for k-Means initialization
        /// </summary>
        RandomCenters = 0,
        /// <summary>
        /// Uses the user-provided labels for K-Means initialization
        /// </summary>
        UseInitialLabels = 1,
        /// <summary>
        /// Uses k-Means++ algorithm for initialization
        /// </summary>
        PPCenters = 2
    }

}
