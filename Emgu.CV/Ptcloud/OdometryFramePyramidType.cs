//----------------------------------------------------------------------------
//  Copyright (C) 2004-2026 by EMGU Corporation. All rights reserved.
//----------------------------------------------------------------------------

namespace Emgu.CV
{
    /// <summary>
    /// Indicates which pyramid to access using the <see cref="OdometryFrame.GetPyramidAt"/> method.
    /// </summary>
    public enum OdometryFramePyramidType
    {
        /// <summary>
        /// The pyramid of grayscale images
        /// </summary>
        Image = 0,
        /// <summary>
        /// The pyramid of depth images
        /// </summary>
        Depth = 1,
        /// <summary>
        /// The pyramid of masks
        /// </summary>
        Mask = 2,
        /// <summary>
        /// The pyramid of point clouds, produced from the pyramid of depths
        /// </summary>
        Cloud = 3,
        /// <summary>
        /// The pyramid of dI/dx derivative images
        /// </summary>
        Dix = 4,
        /// <summary>
        /// The pyramid of dI/dy derivative images
        /// </summary>
        Diy = 5,
        /// <summary>
        /// The pyramid of "textured" masks (i.e. additional masks for normals or grayscale images)
        /// </summary>
        TexMask = 6,
        /// <summary>
        /// The pyramid of normals
        /// </summary>
        Norm = 7,
        /// <summary>
        /// The pyramid of normals masks
        /// </summary>
        NormMask = 8
    }
}
