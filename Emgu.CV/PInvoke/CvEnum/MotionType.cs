//----------------------------------------------------------------------------
//  Copyright (C) 2004-2021 by EMGU Corporation. All rights reserved.       
//----------------------------------------------------------------------------

using System;

namespace Emgu.CV.CvEnum
{
    /// <summary>
    /// Motion type for the FindTransformECC function
    /// </summary>
    public enum MotionType
    {
        /// <summary>
        /// Sets a translational motion model; warpMatrix is 2x3 with the first 2x2 part being the unity matrix and the rest two parameters being estimated.
        /// </summary>
        Translation = 0,
        /// <summary>
        /// Sets a Euclidean (rigid) transformation as motion model; three parameters are estimated; warpMatrix is 2x3.
        /// </summary>
        Euclidean = 1,
        /// <summary>
        /// Sets an affine motion model (DEFAULT); six parameters are estimated; warpMatrix is 2x3.
        /// </summary>
        Affine = 2,
        /// <summary>
        /// Sets a homography as a motion model; eight parameters are estimated; warpMatrix is 3x3.
        /// </summary>
        Homography = 3
    }
}
