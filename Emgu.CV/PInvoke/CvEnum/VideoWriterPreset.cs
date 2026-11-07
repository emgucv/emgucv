//----------------------------------------------------------------------------
//  Copyright (C) 2004-2026 by EMGU Corporation. All rights reserved.
//----------------------------------------------------------------------------

using System;

namespace Emgu.CV.CvEnum
{
    /// <summary>
    /// Encoding speed / compression trade-off presets for <see cref="VideoWriter.WriterProperty.Preset"/>.
    /// Available since OpenCV 5.1.
    /// </summary>
    public enum VideoWriterPreset
    {
        /// <summary>
        /// Fastest encoding, lowest compression efficiency.
        /// </summary>
        Ultrafast = 0,

        /// <summary>
        /// Superfast
        /// </summary>
        Superfast = 1,

        /// <summary>
        /// Veryfast
        /// </summary>
        Veryfast = 2,

        /// <summary>
        /// Faster
        /// </summary>
        Faster = 3,

        /// <summary>
        /// Fast
        /// </summary>
        Fast = 4,

        /// <summary>
        /// Medium, the default of most encoders.
        /// </summary>
        Medium = 5,

        /// <summary>
        /// Slow
        /// </summary>
        Slow = 6,

        /// <summary>
        /// Slower
        /// </summary>
        Slower = 7,

        /// <summary>
        /// Veryslow
        /// </summary>
        Veryslow = 8,

        /// <summary>
        /// Slowest encoding, highest compression efficiency.
        /// </summary>
        Placebo = 9,
    }
}
