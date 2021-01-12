//----------------------------------------------------------------------------
//  Copyright (C) 2004-2021 by EMGU Corporation. All rights reserved.       
//----------------------------------------------------------------------------

using System;

namespace Emgu.CV.CvEnum
{
    /// <summary>
    /// Flags for sorting
    /// </summary>
    [Flags]
    public enum SortFlags
    {
        /// <summary>
        /// Each matrix row is sorted independently
        /// </summary>
        SortEveryRow = 0,
        /// <summary>
        /// Each matrix column is sorted
        /// independently; this flag and SortEveryRow are
        /// mutually exclusive.
        /// </summary>
        SortEveryColumn = 1,
        /// <summary>
        /// Each matrix row is sorted in the ascending order.
        /// </summary>
        SortAscending = 0,
        /// <summary>
        /// Each matrix row is sorted in the 
        /// descending order; this flag and SortAscending are also
        /// mutually exclusive.
        /// </summary>
        SortDescending = 16
    }

}
