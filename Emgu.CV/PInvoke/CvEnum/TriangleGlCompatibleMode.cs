//----------------------------------------------------------------------------
//  Copyright (C) 2004-2026 by EMGU Corporation. All rights reserved.
//----------------------------------------------------------------------------

using System;

namespace Emgu.CV.CvEnum
{
    /// <summary>
    /// GL compatibility settings for the triangle rasterizer. Available since OpenCV 5.1.
    /// </summary>
    public enum TriangleGlCompatibleMode
    {
        /// <summary>
        /// Color and depth have their natural values and are converted to internal formats if needed.
        /// </summary>
        Disabled = 0,

        /// <summary>
        /// Color is natural, depth is transformed from [-zNear; -zFar] to [0; 1] by z_far * (z + z_near) / (z * (z_far - z_near)).
        /// In this mode the input/output depth buffer is considered to be in this format, so no conversions are performed.
        /// </summary>
        InvDepth = 1
    }
}
