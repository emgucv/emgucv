//----------------------------------------------------------------------------
//  Copyright (C) 2004-2026 by EMGU Corporation. All rights reserved.
//----------------------------------------------------------------------------

using System;

namespace Emgu.CV.CvEnum
{
    /// <summary>
    /// Flags for PutText when rendering with a TrueType/OpenType font
    /// </summary>
    [Flags]
    public enum PutTextFlags
    {
        /// <summary>
        /// Put the text to the right from the origin
        /// </summary>
        AlignLeft = 0,
        /// <summary>
        /// Center the text at the origin. Not implemented yet.
        /// </summary>
        AlignCenter = 1,
        /// <summary>
        /// Put the text to the left of the origin
        /// </summary>
        AlignRight = 2,
        /// <summary>
        /// Alignment mask
        /// </summary>
        AlignMask = 3,
        /// <summary>
        /// Treat the target image as having top-left origin
        /// </summary>
        OriginTopLeft = 0,
        /// <summary>
        /// Treat the target image as having bottom-left origin
        /// </summary>
        OriginBottomLeft = 32,
        /// <summary>
        /// Wrap text to the next line if it does not fit
        /// </summary>
        Wrap = 128
    }
}
