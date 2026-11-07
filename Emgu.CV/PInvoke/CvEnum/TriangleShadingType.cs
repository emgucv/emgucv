//----------------------------------------------------------------------------
//  Copyright (C) 2004-2026 by EMGU Corporation. All rights reserved.
//----------------------------------------------------------------------------

using System;

namespace Emgu.CV.CvEnum
{
    /// <summary>
    /// Triangle fill settings for the triangle rasterizer. Available since OpenCV 5.1.
    /// </summary>
    public enum TriangleShadingType
    {
        /// <summary>
        /// A white color is used for the whole triangle.
        /// </summary>
        White = 0,

        /// <summary>
        /// The color of the first vertex of each triangle is used.
        /// </summary>
        Flat = 1,

        /// <summary>
        /// A color is interpolated between the 3 vertices with perspective correction.
        /// </summary>
        Shaded = 2
    }
}
