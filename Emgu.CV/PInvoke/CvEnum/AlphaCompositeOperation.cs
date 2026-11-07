//----------------------------------------------------------------------------
//  Copyright (C) 2004-2026 by EMGU Corporation. All rights reserved.
//----------------------------------------------------------------------------

using System;

namespace Emgu.CV.CvEnum
{
    /// <summary>
    /// Porter-Duff compositing operators for AlphaComposite. Available since OpenCV 5.1.
    /// </summary>
    public enum AlphaCompositeOperation
    {
        /// <summary>
        /// Erases both images.
        /// </summary>
        Clear = 0,
        /// <summary>
        /// Keeps the overlay only.
        /// </summary>
        Source = 1,
        /// <summary>
        /// Keeps the background only.
        /// </summary>
        Dest = 2,
        /// <summary>
        /// Draws the overlay on top of the background (source-over).
        /// </summary>
        Over = 3,
        /// <summary>
        /// Draws the overlay underneath the background.
        /// </summary>
        DestOver = 4,
        /// <summary>
        /// Keeps the overlay where the background is opaque.
        /// </summary>
        In = 5,
        /// <summary>
        /// Keeps the background where the overlay is opaque.
        /// </summary>
        DestIn = 6,
        /// <summary>
        /// Keeps the overlay where the background is transparent.
        /// </summary>
        Out = 7,
        /// <summary>
        /// Keeps the background where the overlay is transparent.
        /// </summary>
        DestOut = 8,
        /// <summary>
        /// Draws the overlay on top, clipped to the shape of the background.
        /// </summary>
        Atop = 9,
        /// <summary>
        /// Draws the background on top, clipped to the shape of the overlay.
        /// </summary>
        DestAtop = 10,
        /// <summary>
        /// Keeps the non-overlapping parts of both.
        /// </summary>
        Xor = 11,
        /// <summary>
        /// Adds the two images, clamped.
        /// </summary>
        Plus = 12
    }
}
