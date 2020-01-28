//----------------------------------------------------------------------------
//  Copyright (C) 2004-2020 by EMGU Corporation. All rights reserved.       
//----------------------------------------------------------------------------

using System;
using System.Runtime.InteropServices;
using System.Drawing;

namespace Emgu.CV.Structure
{
    /// <summary>
    /// A 2D triangle
    /// </summary>
    [Serializable]
    [StructLayout(LayoutKind.Sequential)]
    public struct Triangle2DF : IConvexPolygonF, IEquatable<Triangle2DF>
    {
        private PointF _v0;
        private PointF _v1;
        private PointF _v2;

        /// <summary>
        /// One of the vertex of the triangle
        /// </summary>
        public PointF V0
        {
            get { return _v0; }
            set { _v0 = value; }
        }

        /// <summary>
        /// One of the vertex of the triangle
        /// </summary>
        public PointF V1
        {
            get { return _v1; }
            set { _v1 = value; }
        }

        /// <summary>
        /// One of the vertex of the triangle
        /// </summary>
        public PointF V2
        {
            get { return _v2; }
            set { _v2 = value; }
        }

        /// <summary>
        /// Create a triangle using the specific vertices
        /// </summary>
        /// <param name="v0">The first vertex</param>
        /// <param name="v1">The second vertex</param>
        /// <param name="v2">The third vertex</param>
        public Triangle2DF(PointF v0, PointF v1, PointF v2)
        {
            _v0 = v0; _v1 = v1; _v2 = v2;
        }

        /// <summary>
        /// Get the area of this triangle
        /// </summary>
        public double Area
        {
            get
            {
                float area = ((V1.X - V0.X) * (V2.Y - V0.Y) - (V1.Y - V0.Y) * (V2.X - V0.X)) * 0.5f;
                return area < 0 ? -area : area;
            }
        }

        /// <summary>
        /// Returns the centroid of this triangle
        /// </summary>
        public PointF Centeroid
        {
            get
            {
                return new PointF((V0.X + V1.X + V2.X) / 3.0f, (V0.Y + V1.Y + V2.Y) / 3.0f);
            }
        }

        /// <summary>
        /// Compare two triangles and return true if equal
        /// </summary>
        /// <param name="tri">the other triangles to compare with</param>
        /// <returns>true if the two triangles equals, false otherwise</returns>
        public bool Equals(Triangle2DF tri)
        {
            return
               (V0.Equals(tri.V0) || V0.Equals(tri.V1) || V0.Equals(tri.V2)) &&
               (V1.Equals(tri.V0) || V1.Equals(tri.V1) || V1.Equals(tri.V2)) &&
               (V2.Equals(tri.V0) || V2.Equals(tri.V1) || V2.Equals(tri.V2));
        }

        #region IConvexPolygonF Members
        /// <summary>
        /// Get the vertices of this triangle
        /// </summary>
        /// <returns>The vertices of this triangle</returns>
        public PointF[] GetVertices()
        {
            return new PointF[] { V0, V1, V2 };
        }

        #endregion
    }
}
