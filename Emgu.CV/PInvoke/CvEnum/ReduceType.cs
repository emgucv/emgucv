//----------------------------------------------------------------------------
//  Copyright (C) 2004-2026 by EMGU Corporation. All rights reserved.       
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
        ReduceMin = 3,
        /// <summary>
        /// the output is the sum of all squared rows/columns of the matrix.
        /// </summary>
        ReduceSum2 = 4,
        /// <summary>
        /// The output is the product of the elements. Available since OpenCV 5.1.
        /// </summary>
        /// <remarks>For ReduceProd, ReduceL1, ReduceL2, ReduceLogSum and ReduceLogSumExp the dim argument of <see cref="CvInvoke.Reduce"/> is the axis to reduce, in [-dims, dims). This means <see cref="ReduceDimension.Auto"/> (-1) selects the last axis for these types.</remarks>
        ReduceProd = 5,
        /// <summary>
        /// The output is the sum of the absolute values. Available since OpenCV 5.1.
        /// </summary>
        ReduceL1 = 6,
        /// <summary>
        /// The output is the square root of the sum of the squares. Available since OpenCV 5.1.
        /// </summary>
        ReduceL2 = 7,
        /// <summary>
        /// The output is the logarithm of the sum. Available since OpenCV 5.1.
        /// </summary>
        ReduceLogSum = 8,
        /// <summary>
        /// The output is log(sum(exp(x))), computed without overflow. Available since OpenCV 5.1.
        /// </summary>
        ReduceLogSumExp = 9
    }

}
