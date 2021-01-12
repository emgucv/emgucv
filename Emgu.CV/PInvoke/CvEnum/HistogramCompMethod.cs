//----------------------------------------------------------------------------
//  Copyright (C) 2004-2021 by EMGU Corporation. All rights reserved.       
//----------------------------------------------------------------------------

using System;

namespace Emgu.CV.CvEnum
{
    /// <summary>
    /// Histogram comparison method
    /// </summary>
    public enum HistogramCompMethod
    {
        /// <summary>
        /// Correlation 
        /// </summary>
        Correl = 0,
        /// <summary>
        /// Chi-Square
        /// </summary>
        Chisqr = 1,
        /// <summary>
        /// Intersection
        /// </summary>
        Intersect = 2,
        /// <summary>
        /// Bhattacharyya distance
        /// </summary>
        Bhattacharyya = 3,
        /// <summary>
        ///  Synonym for Bhattacharyya
        /// </summary>
        Hellinger = Bhattacharyya,
        /// <summary>
        /// Alternative Chi-Square
        /// </summary>
        ChisqrAlt = 4
    }

}
