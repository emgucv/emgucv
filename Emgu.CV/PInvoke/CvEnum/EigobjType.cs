//----------------------------------------------------------------------------
//  Copyright (C) 2004-2021 by EMGU Corporation. All rights reserved.       
//----------------------------------------------------------------------------

using System;

namespace Emgu.CV.CvEnum
{
    /// <summary>
    /// IO type for eigen object related functions
    /// </summary>
    public enum EigobjType
    {
        /// <summary>
        /// No callback
        /// </summary>
        NoCallback = 0,
        /// <summary>
        /// Input callback
        /// </summary>
        InputCallback = 1,
        /// <summary>
        /// Output callback
        /// </summary>
        OutputCallback = 2,
        /// <summary>
        /// Both callback
        /// </summary>
        BothCallback = 3
    }
}
