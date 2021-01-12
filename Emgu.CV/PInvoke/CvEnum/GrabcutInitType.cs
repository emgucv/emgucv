//----------------------------------------------------------------------------
//  Copyright (C) 2004-2021 by EMGU Corporation. All rights reserved.       
//----------------------------------------------------------------------------

using System;

namespace Emgu.CV.CvEnum
{
    /// <summary>
    /// Grabcut initialization type
    /// </summary>
    public enum GrabcutInitType
    {
        /// <summary>
        /// Initialize with rectangle
        /// </summary>
        InitWithRect = 0,
        /// <summary>
        /// Initialize with mask
        /// </summary>
        InitWithMask = 1,
        /// <summary>
        /// Eval
        /// </summary>
        Eval = 2
    }

}
