//----------------------------------------------------------------------------
//  Copyright (C) 2004-2020 by EMGU Corporation. All rights reserved.       
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
        /// Iteration
        /// </summary>
        Iter = 1,
        /// <summary>
        /// Epsilon
        /// </summary>
        Eps = 2
    }
}
