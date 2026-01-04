//----------------------------------------------------------------------------
//  Copyright (C) 2004-2026 by EMGU Corporation. All rights reserved.       
//----------------------------------------------------------------------------

using System;

namespace Emgu.CV.CvEnum
{
    /// <summary>
    /// Flags that allow to modify some functions behavior. Used as set of flags.
    /// </summary>
    public enum AlgorithmHint
    {
        /// <summary>
        /// Default algorithm behaviour defined during OpenCV build
        /// </summary>
        Default = 0,
        /// <summary>
        /// Use generic portable implementation
        /// </summary>
        Accurate = 1,
        /// <summary>
        /// Allow alternative approximations to get faster implementation. Behaviour and result depends on a platform
        /// </summary>
        Approx = 2,

    }

}
