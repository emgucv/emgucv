//----------------------------------------------------------------------------
//  Copyright (C) 2004-2021 by EMGU Corporation. All rights reserved.       
//----------------------------------------------------------------------------

using System;

namespace Emgu.CV.CvEnum
{
    /// <summary>
    /// Connected components algorithm output formats
    /// </summary>
    public enum ConnectedComponentsTypes
    {
        /// <summary>
        /// The leftmost (x) coordinate which is the inclusive start of the bounding box in the horizontal direction.
        /// </summary>
        Left = 0,

        /// <summary>
        /// The topmost (y) coordinate which is the inclusive start of the bounding box in the vertical direction.
        /// </summary>
        Top,

        /// <summary>
        /// The horizontal size of the bounding box.
        /// </summary>
        Width,

        /// <summary>
        /// The vertical size of the bounding box.
        /// </summary>
        Height,

        /// <summary>
        /// The total area (in pixels) of the connected component.
        /// </summary>
        Area,

        /// <summary>
        /// Max
        /// </summary>
        Max

    }

}
