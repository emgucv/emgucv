using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;

namespace Emgu.CV
{
    /// <summary>
    /// A triangle
    /// </summary>
    /// <typeparam name="T">The depth of the triangle</typeparam>
    public class Triangle<T> : IConvexPolygon<T> where T: IComparable, new()
    {
        /// <summary>
        /// The vertices for this triangle
        /// </summary>
        private Point2D<T>[] _vertices;

        /// <summary>
        /// Get or set the vertices of this triangle
        /// </summary>
        public Point2D<T>[] Vertices
        {
            get { return _vertices; }
            set 
            {
                Debug.Assert(value.Length == 3, "The number of vertices for triangle must be 3");
                _vertices = value; 
            }
        }
	
        /// <summary>
        /// Create a triangle using the specific vertices
        /// </summary>
        /// <param name="v1">The first vertex</param>
        /// <param name="v2">The second vertex</param>
        /// <param name="v3">The third vertex</param>
        public Triangle(Point2D<T> v1, Point2D<T> v2, Point2D<T> v3 )
        {
            _vertices = new Point2D<T>[] { v1, v2, v3 };
        }

        /// <summary>
        /// Compare two triangles and return true if equal
        /// </summary>
        /// <param name="tri">the other triangles to compare with</param>
        /// <returns>true if the two triangles equals, false otherwise</returns>
        public bool Equals(Triangle<T> tri)
        {
            Point2D<T>[] v2 = tri.Vertices;
            Point2D<T>[] v1 = Vertices;

            bool res = true;
            foreach (Point2D<T> v in v1)
                res = res && (Array.FindIndex<Point2D<T>>(v2, delegate(Point2D<T> value) { return v.Equals(value); }) >= 0);

            return res;
        }
    }
}
