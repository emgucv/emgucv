//----------------------------------------------------------------------------
//  Copyright (C) 2004-2020 by EMGU Corporation. All rights reserved.       
//----------------------------------------------------------------------------

using System;

namespace Emgu.CV.CvEnum
{
    /// <summary>
    /// Rectangle intersect type
    /// </summary>
    public enum RectIntersectType
    {
        /// <summary>
        /// No intersection
        /// </summary>
        None = 0,
        /// <summary>
        /// There is a partial intersection
        /// </summary>
        Partial = 1,
        /// <summary>
        /// One of the rectangle is fully enclosed in the other
        /// </summary>
        Full = 2
    }
}
