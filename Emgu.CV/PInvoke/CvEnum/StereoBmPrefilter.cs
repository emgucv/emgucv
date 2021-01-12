//----------------------------------------------------------------------------
//  Copyright (C) 2004-2021 by EMGU Corporation. All rights reserved.       
//----------------------------------------------------------------------------

using System;

namespace Emgu.CV.CvEnum
{
    /// <summary>
    /// Stereo Block Matching Prefilter type
    /// </summary>
    public enum StereoBmPrefilter
    {
        /// <summary>
        /// No prefilter
        /// </summary>
        NormalizedResponse = 0,
        /// <summary>
        /// XSobel
        /// </summary>
        XSobel = 1
    }
}
