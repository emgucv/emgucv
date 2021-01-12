//----------------------------------------------------------------------------
//  Copyright (C) 2004-2021 by EMGU Corporation. All rights reserved.       
//----------------------------------------------------------------------------

using System;

namespace Emgu.CV.CvEnum
{
    /// <summary>
    /// The type for CopyMakeBorder function
    /// </summary>
    public enum BorderType
    {
        /// <summary>
        /// Used by some cuda methods, will pass the value -1 to the function
        /// </summary>
        NegativeOne = -1,

        /// <summary>
        /// Border is filled with the fixed value, passed as last parameter of the function
        /// </summary>
        Constant = 0,
        /// <summary>
        /// The pixels from the top and bottom rows, the left-most and right-most columns are replicated to fill the border
        /// </summary>
        Replicate = 1,
        /// <summary>
        /// Reflect
        /// </summary>
        Reflect = 2,
        /// <summary>
        /// Wrap
        /// </summary>
        Wrap = 3,
        /// <summary>
        /// Reflect 101
        /// </summary>
        Reflect101 = 4,
        /// <summary>
        /// Transparent
        /// </summary>
        Transparent = 5,

        /// <summary>
        /// The default border interpolation type.
        /// </summary>
        Default = Reflect101,
        /// <summary>
        /// Do not look outside of ROI
        /// </summary>
        Isolated = 16
    }
}
