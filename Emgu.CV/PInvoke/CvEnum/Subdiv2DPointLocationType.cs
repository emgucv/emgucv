//----------------------------------------------------------------------------
//  Copyright (C) 2004-2021 by EMGU Corporation. All rights reserved.       
//----------------------------------------------------------------------------

using System;

namespace Emgu.CV.CvEnum
{
    /// <summary>
    /// The result type of cvSubdiv2DLocate.
    /// </summary>
    public enum Subdiv2DPointLocationType
    {
        /// <summary>
        /// One of input arguments is invalid.
        /// </summary>
        Error = -2,
        /// <summary>
        /// Point is outside the subdivision reference rectangle
        /// </summary>
        OutsideRect = -1,
        /// <summary>
        /// Point falls into some facet
        /// </summary>
        Inside = 0,
        /// <summary>
        /// Point coincides with one of subdivision vertices
        /// </summary>
        Vertex = 1,
        /// <summary>
        /// Point falls onto the edge
        /// </summary>
        OnEdge = 2
    }
}
