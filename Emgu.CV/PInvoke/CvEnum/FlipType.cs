//----------------------------------------------------------------------------
//  Copyright (C) 2004-2021 by EMGU Corporation. All rights reserved.       
//----------------------------------------------------------------------------

using System;

namespace Emgu.CV.CvEnum
{
    /// <summary>
    /// Enumeration used by cvFlip
    /// </summary>
    [Flags]
    public enum FlipType
    {
        /// <summary>
        /// No flipping
        /// </summary>
        None = 0,
        /// <summary>
        /// Flip horizontally
        /// </summary>
        Horizontal = 1,
        /// <summary>
        /// Flip vertically
        /// </summary>
        Vertical = 2
    }

}
