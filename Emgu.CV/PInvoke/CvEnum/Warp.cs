//----------------------------------------------------------------------------
//  Copyright (C) 2004-2020 by EMGU Corporation. All rights reserved.       
//----------------------------------------------------------------------------

using System;

namespace Emgu.CV.CvEnum
{
    /// <summary>
    /// Types for WarpAffine
    /// </summary>
    public enum Warp
    {
        /// <summary>
        /// Neither FILL_OUTLIERS nor CV_WRAP_INVERSE_MAP
        /// </summary>
        Default = 0,
        /// <summary>
        /// Fill all the destination image pixels. If some of them correspond to outliers in the source image, they are set to fillval.
        /// </summary>
        FillOutliers = 8,
        /// <summary>
        /// Indicates that matrix is inverse transform from destination image to source and, thus, can be used directly for pixel interpolation. Otherwise, the function finds the inverse transform from map_matrix.
        /// </summary>
        InverseMap = 16
    }
}
