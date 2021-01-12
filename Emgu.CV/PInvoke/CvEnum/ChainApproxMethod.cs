//----------------------------------------------------------------------------
//  Copyright (C) 2004-2021 by EMGU Corporation. All rights reserved.       
//----------------------------------------------------------------------------

using System;

namespace Emgu.CV.CvEnum
{
    /// <summary>
    /// contour approximation method
    /// </summary>
    public enum ChainApproxMethod : int
    {
        /// <summary>
        /// output contours in the Freeman chain code. All other methods output polygons (sequences of vertices). 
        /// </summary>
        ChainCode = 0,
        /// <summary>
        /// translate all the points from the chain code into points;
        /// </summary>
        ChainApproxNone = 1,
        /// <summary>
        /// compress horizontal, vertical, and diagonal segments, that is, the function leaves only their ending points; 
        /// </summary>
        ChainApproxSimple = 2,
        /// <summary>
        /// 
        /// </summary>
        ChainApproxTc89L1 = 3,
        /// <summary>
        /// apply one of the flavors of Teh-Chin chain approximation algorithm
        /// </summary>
        ChainApproxTc89Kcos = 4,
        /// <summary>
        /// use completely different contour retrieval algorithm via linking of horizontal segments of 1s. Only LIST retrieval mode can be used with this method
        /// </summary>
        LinkRuns = 5
    }

}
