//----------------------------------------------------------------------------
//  Copyright (C) 2004-2021 by EMGU Corporation. All rights reserved.       
//----------------------------------------------------------------------------

using System;

namespace Emgu.CV.CvEnum
{
    /// <summary>
    /// The return value for solveLP function
    /// </summary>
    public enum SolveLPResult
    {
        /// <summary>
        /// Problem is unbounded (target function can achieve arbitrary high values)
        /// </summary>
        Unbounded = -2,
        /// <summary>
        /// Problem is unfeasible (there are no points that satisfy all the constraints imposed)
        /// </summary>
        Unfeasible = -1,
        /// <summary>
        /// There is only one maximum for target function
        /// </summary>
        Single = 0,
        /// <summary>
        /// there are multiple maxima for target function - the arbitrary one is returned
        /// </summary>
        Multi = 1
    }
}
