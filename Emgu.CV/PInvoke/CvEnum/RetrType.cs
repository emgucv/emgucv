//----------------------------------------------------------------------------
//  Copyright (C) 2004-2021 by EMGU Corporation. All rights reserved.       
//----------------------------------------------------------------------------

using System;

namespace Emgu.CV.CvEnum
{
    /// <summary>
    /// Contour retrieval mode
    /// </summary>
    public enum RetrType
    {
        /// <summary>
        /// Retrieve only the extreme outer contours 
        /// </summary>
        External = 0,
        /// <summary>
        /// Retrieve all the contours and puts them in the list 
        /// </summary>
        List = 1,
        /// <summary>
        /// Retrieve all the contours and organizes them into two-level hierarchy: top level are external boundaries of the components, second level are bounda boundaries of the holes 
        /// </summary>
        Ccomp = 2,
        /// <summary>
        /// Retrieve all the contours and reconstructs the full hierarchy of nested contours 
        /// </summary>
        Tree = 3,
        /// <summary>
        /// Flood fill type
        /// </summary>
        Floodfill = 4
    }
}
