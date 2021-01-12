//----------------------------------------------------------------------------
//  Copyright (C) 2004-2021 by EMGU Corporation. All rights reserved.       
//----------------------------------------------------------------------------

using System;

namespace Emgu.CV.CvEnum
{
    /// <summary>
    /// Type used for Reduce function
    /// </summary>
    public enum ReduceType
    {
        /// <summary>
        /// The output is the sum of all the matrix rows/columns
        /// </summary>
        ReduceSum = 0,
        /// <summary>
        /// The output is the mean vector of all the matrix rows/columns
        /// </summary>
        ReduceAvg = 1,
        /// <summary>
        /// The output is the maximum (column/row-wise) of all the matrix rows/columns
        /// </summary>
        ReduceMax = 2,
        /// <summary>
        /// The output is the minimum (column/row-wise) of all the matrix rows/columns
        /// </summary>
        ReduceMin = 3
    }

}
