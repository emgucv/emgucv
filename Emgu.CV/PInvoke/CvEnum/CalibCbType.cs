//----------------------------------------------------------------------------
//  Copyright (C) 2004-2020 by EMGU Corporation. All rights reserved.       
//----------------------------------------------------------------------------

using System;

namespace Emgu.CV.CvEnum
{
    /// <summary>
    /// Type of chessboard calibration
    /// </summary>
    [Flags]
    public enum CalibCbType
    {
        /// <summary>
        /// Default type
        /// </summary>
        Default = 0,
        /// <summary>
        /// Use adaptive thresholding to convert the image to black-n-white, rather than a fixed threshold level (computed from the average image brightness)
        /// </summary>
        AdaptiveThresh = 1,
        /// <summary>
        /// Normalize the image using cvNormalizeHist before applying fixed or adaptive thresholding.
        /// </summary>
        NormalizeImage = 2,
        /// <summary>
        /// Use additional criteria (like contour area, perimeter, square-like shape) to filter out false quads that are extracted at the contour retrieval stage
        /// </summary>
        FilterQuads = 4,
        /// <summary>
        /// If it is on, then this check is performed before the main algorithm and if a chessboard is not found, the function returns 0 instead of wasting 0.3-1s on doing the full search.
        /// </summary>
        FastCheck = 8,
        /// <summary>
        /// Run an exhaustive search to improve detection rate.
        /// </summary>
        Exhaustive = 16,
        /// <summary>
        /// Up sample input image to improve sub-pixel accuracy due to aliasing effects. This should be used if an accurate camera calibration is required.
        /// </summary>
        Accuracy = 32
    }

}
