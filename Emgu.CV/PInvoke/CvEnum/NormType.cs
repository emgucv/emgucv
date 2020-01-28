//----------------------------------------------------------------------------
//  Copyright (C) 2004-2020 by EMGU Corporation. All rights reserved.       
//----------------------------------------------------------------------------

using System;

namespace Emgu.CV.CvEnum
{
    /// <summary>
    /// Type for Norm
    /// </summary>
    [Flags]
    public enum NormType
    {
        /// <summary>
        /// if arr2 is NULL, norm = ||arr1||_C = max_I abs(arr1(I));
        /// if arr2 is not NULL, norm = ||arr1-arr2||_C = max_I abs(arr1(I)-arr2(I))
        /// </summary>
        C = 1,
        /// <summary>
        /// if arr2 is NULL, norm = ||arr1||_L1 = sum_I abs(arr1(I));
        /// if arr2 is not NULL, norm = ||arr1-arr2||_L1 = sum_I abs(arr1(I)-arr2(I))
        /// </summary>
        L1 = 2,
        /// <summary>
        /// if arr2 is NULL, norm = ||arr1||_L2 = sqrt( sum_I arr1(I)^2);
        /// if arr2 is not NULL, norm = ||arr1-arr2||_L2 = sqrt( sum_I (arr1(I)-arr2(I))^2 )
        /// </summary>
        L2 = 4,
        /// <summary>
        /// Norm mask
        /// </summary>
        NormMask = 7,
        /// <summary>
        /// It is used in combination with either CV_C, CV_L1 or CV_L2
        /// </summary>
        Relative = 8,
        /// <summary>
        /// It is used in combination with either CV_C, CV_L1 or CV_L2
        /// </summary>
        Diff = 16,
        /// <summary>
        /// Min Max
        /// </summary>
        MinMax = 32,
        /// <summary>
        /// Diff C
        /// </summary>
        DiffC = (Diff | C),
        /// <summary>
        /// Diff L1
        /// </summary>
        DiffL1 = (Diff | L1),
        /// <summary>
        /// Diff L2
        /// </summary>
        DiffL2 = (Diff | L2),
        /// <summary>
        /// norm = ||arr1-arr2||_C/||arr2||_C
        /// </summary>
        RelativeC = (Relative | C),
        /// <summary>
        /// norm = ||arr1-arr2||_L1/||arr2||_L1
        /// </summary>
        RelativeL1 = (Relative | L1),
        /// <summary>
        /// norm = ||arr1-arr2||_L2/||arr2||_L2
        /// </summary>
        RelativeL2 = (Relative | L2)
    }
}
