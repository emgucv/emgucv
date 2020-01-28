//----------------------------------------------------------------------------
//  Copyright (C) 2004-2020 by EMGU Corporation. All rights reserved.       
//----------------------------------------------------------------------------

using System;

namespace Emgu.CV.CvEnum
{
    /// <summary>
    /// Type used for Reduce function
    /// </summary>
    public enum ReduceDimension
    {
        /// <summary>
        /// The matrix is reduced to a single row
        /// </summary>
        SingleRow = 0,
        /// <summary>
        /// The matrix is reduced to a single column
        /// </summary>
        SingleCol = 1,
        /// <summary>
        /// The dimension is chosen automatically by analysing the dst size
        /// </summary>
        Auto = -1,
    }

}
