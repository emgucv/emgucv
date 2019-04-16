//----------------------------------------------------------------------------
//  Copyright (C) 2004-2019 by EMGU Corporation. All rights reserved.       
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
