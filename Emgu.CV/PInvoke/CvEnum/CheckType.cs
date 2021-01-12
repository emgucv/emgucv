//----------------------------------------------------------------------------
//  Copyright (C) 2004-2021 by EMGU Corporation. All rights reserved.       
//----------------------------------------------------------------------------

using System;

namespace Emgu.CV.CvEnum
{
    /// <summary>
    /// Enumeration used by cvCheckArr
    /// </summary>
    [Flags]
    public enum CheckType
    {
        /// <summary>
        /// Checks that every element is neither NaN nor Infinity
        /// </summary>
        NanInfinity = 0,
        /// <summary>
        /// If set, the function checks that every value of array is within [minVal,maxVal) range, otherwise it just checks that every element is neither NaN nor Infinity
        /// </summary>
        Range = 1,
        /// <summary>
        /// If set, the function does not raises an error if an element is invalid or out of range
        /// </summary>
        Quite = 2
    }
}
