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
    public class Triangle3D<T> where T : IComparable, new()
    {
        /// <summary>
        /// The vertices for this triangle
        /// </summary>
        private Point3D<T>[] _vertices;

        /// <summary>
        /// Get or set the vertices of this triangle
        /// </summary>
        public Point3D<T>[] Vertices
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
        public Triangle3D(Point3D<T> v1, Point3D<T> v2, Point3D<T> v3)
        {
            _vertices = new Point3D<T>[] { v1, v2, v3 };
        }

        /// <summary>
        /// Compare two triangles and return true if equal
        /// </summary>
        /// <param name="tri">the other triangles to compare with</param>
        /// <returns>true if the two triangles equals, false otherwise</returns>
        public bool Equals(Triangle3D<T> tri)
        {
            Point3D<T>[] v2 = tri.Vertices;
            Point3D<T>[] v1 = Vertices;

            return Array.TrueForAll<Point3D<T>>(v1, delegate(Point3D<T> v) { return Array.Exists<Point3D<T>>(v2, v.Equals); });
            //bool res = true;
            //foreach (Point3D<T> v in v1)
            //    res = res && Array.Exists<Point3D<T>>(v2, v.Equals) ;

            //return res;
        }
    }
}
