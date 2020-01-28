//----------------------------------------------------------------------------
//  Copyright (C) 2004-2020 by EMGU Corporation. All rights reserved.       
//----------------------------------------------------------------------------

using System;

namespace Emgu.CV.CvEnum
{
    /// <summary>
    /// Morphology operation type
    /// </summary>
    public enum MorphOp
    {
        /// <summary>
        /// Erode
        /// </summary>
        Erode = 0,
        /// <summary>
        /// Dilate
        /// </summary>
        Dilate = 1,
        /// <summary>
        /// Open
        /// </summary>
        Open = 2,
        /// <summary>
        /// Close
        /// </summary>
        Close = 3,
        /// <summary>
        /// Gradient
        /// </summary>
        Gradient = 4,
        /// <summary>
        /// Top hat
        /// </summary>
        Tophat = 5,
        /// <summary>
        /// Black hat
        /// </summary>
        Blackhat = 6,
        /// <summary>
        /// Hit or miss. Only supported for CV_8UC1 binary images.
        /// </summary>
        HitMiss = 7
    }

}
