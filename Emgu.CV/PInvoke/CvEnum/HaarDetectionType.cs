//----------------------------------------------------------------------------
//  Copyright (C) 2004-2020 by EMGU Corporation. All rights reserved.       
//----------------------------------------------------------------------------

using System;

namespace Emgu.CV.CvEnum
{
    /// <summary>
    /// The types for haar detection
    /// </summary>
    [Flags]
    public enum HaarDetectionType
    {
        /// <summary>
        /// The default type where no optimization is done.
        /// </summary>
        Default = 0,
        /// <summary>
        /// If it is set, the function uses Canny edge detector to reject some image regions that contain too few or too much edges and thus can not contain the searched object. The particular threshold values are tuned for face detection and in this case the pruning speeds up the processing
        /// </summary>
        DoCannyPruning = 1,
        /// <summary>
        /// For each scale factor used the function will downscale the image rather than "zoom" the feature coordinates in the classifier cascade. Currently, the option can only be used alone, i.e. the flag can not be set together with the others
        /// </summary>
        ScaleImage = 2,
        /// <summary>
        /// If it is set, the function finds the largest object (if any) in the image. That is, the output sequence will contain one (or zero) element(s)
        /// </summary>
        FindBiggestObject = 4,
        /// <summary>
        /// It should be used only when CV_HAAR_FIND_BIGGEST_OBJECT is set and min_neighbors &gt; 0. If the flag is set, the function does not look for candidates of a smaller size as soon as it has found the object (with enough neighbor candidates) at the current scale. Typically, when min_neighbors is fixed, the mode yields less accurate (a bit larger) object rectangle than the regular single-object mode (flags=CV_HAAR_FIND_BIGGEST_OBJECT), but it is much faster, up to an order of magnitude. A greater value of min_neighbors may be specified to improve the accuracy
        /// </summary>
        DoRoughSearch = 8
    }
}
