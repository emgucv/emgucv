//----------------------------------------------------------------------------
//  Copyright (C) 2004-2026 by EMGU Corporation. All rights reserved.
//----------------------------------------------------------------------------

using System;

namespace Emgu.CV.CvEnum
{
    /// <summary>
    /// Face culling settings for the triangle rasterizer: which faces are drawn. Available since OpenCV 5.1.
    /// </summary>
    public enum TriangleCullingMode
    {
        /// <summary>
        /// All faces are drawn, no culling is actually performed.
        /// </summary>
        None = 0,

        /// <summary>
        /// Triangles whose vertices are given in clockwise order are drawn.
        /// </summary>
        Cw = 1,

        /// <summary>
        /// Triangles whose vertices are given in counterclockwise order are drawn.
        /// </summary>
        Ccw = 2
    }
}
