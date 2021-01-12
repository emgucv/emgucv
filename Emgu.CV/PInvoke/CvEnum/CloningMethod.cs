//----------------------------------------------------------------------------
//  Copyright (C) 2004-2021 by EMGU Corporation. All rights reserved.       
//----------------------------------------------------------------------------

using System;

namespace Emgu.CV.CvEnum
{

    /// <summary>
    /// Seamless clone method
    /// </summary>
    public enum CloningMethod
    {
        /// <summary>
        /// The power of the method is fully expressed when inserting objects with complex outlines into a new background
        /// </summary>
        Normal = 1,
        /// <summary>
        /// The classic method, color-based selection and alpha masking might be time consuming and often leaves an undesirable halo. Seamless cloning, even averaged with the original image, is not effective. Mixed seamless cloning based on a loose selection proves effective.
        /// </summary>
        Mixed = 2,
        /// <summary>
        /// Monochrome transfer
        /// </summary>
        MonochromeTransfer = 3
    }

}
