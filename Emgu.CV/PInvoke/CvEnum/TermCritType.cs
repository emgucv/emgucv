//----------------------------------------------------------------------------
//  Copyright (C) 2004-2025 by EMGU Corporation. All rights reserved.       
//----------------------------------------------------------------------------

using System;

namespace Emgu.CV.CvEnum
{
    /// <summary>
    /// CV TERMCRIT type
    /// </summary>
    [Flags]
    public enum TermCritType
    {
        /// <summary>
        /// The maximum number of iterations or elements to compute
        /// </summary>
        Count = 1,
        /// <summary>
        /// The maximum number of iterations or elements to compute
        /// </summary>
        MaxIter = 1,
        /// <summary>
        /// The desired accuracy or change in parameters at whic the iterative algorithm stops.
        /// </summary>
        Eps = 2
    }
}
