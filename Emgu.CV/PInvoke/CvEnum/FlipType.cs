//----------------------------------------------------------------------------
//  Copyright (C) 2004-2021 by EMGU Corporation. All rights reserved.       
//----------------------------------------------------------------------------

using System;

namespace Emgu.CV.CvEnum
{
    /// <summary>
    /// Enumeration used by cvFlip
    /// </summary>
    public enum FlipType
    {
        /// <summary>
        /// Vertical flipping of the image to switch between top-left and bottom-left image origin.
        /// </summary>
        Vertical = 0,
        /// <summary>
        /// Flip horizontally
        /// </summary>
        Horizontal = 1,
        /// <summary>
        /// Flip both vertically and horizontally
        /// </summary>
        Both = -1
    }

}
