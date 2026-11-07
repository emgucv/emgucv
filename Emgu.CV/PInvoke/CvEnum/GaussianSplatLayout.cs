//----------------------------------------------------------------------------
//  Copyright (C) 2004-2026 by EMGU Corporation. All rights reserved.
//----------------------------------------------------------------------------

using System;

namespace Emgu.CV.CvEnum
{
    /// <summary>
    /// Column layout of a decoded 3D Gaussian Splatting scene. A decoded scene is a CV_32F matrix with one row per Gaussian
    /// and <see cref="Stride"/> columns. Available since OpenCV 5.1.
    /// </summary>
    public enum GaussianSplatLayout
    {
        /// <summary>
        /// Floats per Gaussian, i.e. the width of a decoded scene.
        /// </summary>
        Stride = 13,

        /// <summary>
        /// First column of the position, 3 floats: x, y, z.
        /// </summary>
        Pos = 0,

        /// <summary>
        /// First column of the covariance upper triangle, 6 floats: xx, xy, xz, yy, yz, zz.
        /// </summary>
        Cov = 3,

        /// <summary>
        /// First column of the color, 3 floats in [0, 1].
        /// </summary>
        Rgb = 9,

        /// <summary>
        /// Column of the opacity, 1 float in [0, 1].
        /// </summary>
        Alpha = 12
    }
}
