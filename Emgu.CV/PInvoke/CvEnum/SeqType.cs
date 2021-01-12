//----------------------------------------------------------------------------
//  Copyright (C) 2004-2021 by EMGU Corporation. All rights reserved.       
//----------------------------------------------------------------------------

using System;

namespace Emgu.CV.CvEnum
{
    /// <summary>
    /// Sequence type for point sets
    /// </summary>
    public enum SeqType
    {
        /// <summary>
        /// Point set
        /// </summary>
        PointSet = (SeqKind.Generic | SeqEltype.Point),
        /// <summary>
        /// Point 3D set
        /// </summary>
        Point3DSet = (SeqKind.Generic | SeqEltype.Point3D),
        /// <summary>
        /// Polyline
        /// </summary>
        Polyline = (SeqKind.Curve | SeqEltype.Point),
        /// <summary>
        /// Polygon
        /// </summary>
        Polygon = (SeqFlag.Closed | Polyline),
        //CV_SEQ_CONTOUR         =POLYGON,
        /// <summary>
        /// Simple Polygon
        /// </summary>
        SimplePolygon = (SeqFlag.Simple | Polygon)
    }

}
