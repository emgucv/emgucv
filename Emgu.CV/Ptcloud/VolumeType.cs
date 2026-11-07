//----------------------------------------------------------------------------
//  Copyright (C) 2004-2026 by EMGU Corporation. All rights reserved.
//----------------------------------------------------------------------------

namespace Emgu.CV
{
    /// <summary>
    /// The volume type.
    /// </summary>
    public enum VolumeType
    {
        /// <summary>
        /// TSDF
        /// </summary>
        Tsdf = 0,
        /// <summary>
        /// HashTSDF
        /// </summary>
        HashTsdf = 1,
        /// <summary>
        /// ColorTSDF
        /// </summary>
        ColorTsdf = 2,
        /// <summary>
        /// ColorHashTSDF: hashed TSDF storage with color. Available since OpenCV 5.1.
        /// </summary>
        ColorHashTsdf = 3
    }
}
