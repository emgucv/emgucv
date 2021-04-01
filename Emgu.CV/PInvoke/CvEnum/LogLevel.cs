//----------------------------------------------------------------------------
//  Copyright (C) 2004-2021 by EMGU Corporation. All rights reserved.       
//----------------------------------------------------------------------------

using System;

namespace Emgu.CV.CvEnum
{
    /// <summary>
    /// Log level
    /// </summary>
    public enum LogLevel
    {
        /// <summary>
        /// For using in setLogVevel() call
        /// </summary>
        Silent = 0,
        /// <summary>
        /// Fatal (critical) error (unrecoverable internal error)
        /// </summary>
        Fatal = 1,
        /// <summary>
        /// Error message
        /// </summary>
        Error = 2,
        /// <summary>
        /// Warning message
        /// </summary>
        Warning = 3,
        /// <summary>
        /// Info message
        /// </summary>
        Info = 4,
        /// <summary>
        /// Debug message. Disabled in the "Release" build.
        /// </summary>
        Debug = 5,
        /// <summary>
        /// Verbose (trace) messages. Requires verbosity level. Disabled in the "Release" build.
        /// </summary>
        Verbose = 6,
    }
}
