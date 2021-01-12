//----------------------------------------------------------------------------
//  Copyright (C) 2004-2021 by EMGU Corporation. All rights reserved.       
//----------------------------------------------------------------------------

using System;

namespace Emgu.CV.CvEnum
{
    /// <summary>
    /// Type of floodfill operation
    /// </summary>
    [Flags]
    public enum FloodFillType
    {
        /// <summary>
        /// The default type
        /// </summary>
        Default = 0,
        /// <summary>
        /// If set the difference between the current pixel and seed pixel is considered,
        /// otherwise difference between neighbor pixels is considered (the range is floating).
        /// </summary>
        FixedRange = (1 << 16),
        /// <summary>
        /// If set, the function does not fill the image (new_val is ignored),
        /// but the fills mask (that must be non-NULL in this case).
        /// </summary>
        MaskOnly = (1 << 17)
    }

}
