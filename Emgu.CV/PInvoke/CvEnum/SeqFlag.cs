//----------------------------------------------------------------------------
//  Copyright (C) 2004-2020 by EMGU Corporation. All rights reserved.       
//----------------------------------------------------------------------------

using System;

namespace Emgu.CV.CvEnum
{
    /// <summary>
    /// Sequence flag
    /// </summary>
    public enum SeqFlag
    {
        /// <summary>
        /// Close sequence
        /// </summary>
        Closed = (1 << SeqConst.Shift),
        /// <summary>
        /// Simple sequence
        /// </summary>
        Simple = (2 << SeqConst.Shift),
        /// <summary>
        /// Convex sequence
        /// </summary>
        Convex = (4 << SeqConst.Shift),
        /// <summary>
        /// Hole
        /// </summary>
        Hole = (8 << SeqConst.Shift)
    }
}
