//----------------------------------------------------------------------------
//  Copyright (C) 2004-2020 by EMGU Corporation. All rights reserved.       
//----------------------------------------------------------------------------

using System;

namespace Emgu.CV.CvEnum
{
    /// <summary>
    /// Type used in cvStereoRectify
    /// </summary>
    public enum StereoRectifyType
    {
        /// <summary>
        /// Shift one of the image in horizontal or vertical direction (depending on the orientation of epipolar lines) in order to maximise the useful image area
        /// </summary>
        Default = 0,
        /// <summary>
        /// Makes the principal points of each camera have the same pixel coordinates in the rectified views
        /// </summary>
        CalibZeroDisparity = 1024
    }
}
