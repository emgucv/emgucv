//----------------------------------------------------------------------------
//  Copyright (C) 2004-2020 by EMGU Corporation. All rights reserved.       
//----------------------------------------------------------------------------

using System;

namespace Emgu.CV.CvEnum
{
    /// <summary>
    /// Type of circles grid calibration
    /// </summary>
    [Flags]
    public enum CalibCgType
    {
        /// <summary>
        /// Symmetric grid
        /// </summary>
        SymmetricGrid = 1,
        /// <summary>
        /// Asymmetric grid
        /// </summary>
        AsymmetricGrid = 2,
        /// <summary>
        /// Clustering
        /// </summary>
        Clustering = 4,
    }

}
