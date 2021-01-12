//----------------------------------------------------------------------------
//  Copyright (C) 2004-2021 by EMGU Corporation. All rights reserved.       
//----------------------------------------------------------------------------

using System;

namespace Emgu.CV.CvEnum
{
    /// <summary>
    /// The available flags for Farneback optical flow computation
    /// </summary>
    [Flags]
    public enum OpticalflowFarnebackFlag
    {
        /// <summary>
        /// Default
        /// </summary>
        Default = 0,
        /// <summary>
        /// Use the input flow as the initial flow approximation
        /// </summary>
        UseInitialFlow = 4,
        /// <summary>
        /// Use a Gaussian winsize x winsizefilter instead of box
        /// filter of the same size for optical flow estimation. Usually, this option gives more accurate
        /// flow than with a box filter, at the cost of lower speed (and normally winsize for a
        /// Gaussian window should be set to a larger value to achieve the same level of robustness)
        /// </summary>
        FarnebackGaussian = 256
    }

}
