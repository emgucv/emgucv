using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;

namespace Emgu.CV
{
    /// <summary>
    /// Managed structure equivalent to CvBox2D
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct MCvBox2D : IConvexPolygon<float>
    {
        /// <summary>
        /// The center of the box
        /// </summary>
        public MCvPoint2D32f center;
        /// <summary>
        /// The size of the box
        /// </summary>
        public MCvSize2D32f size;
        /// <summary>
        /// The angle of the box
        /// </summary>
        public float angle;

        /// <summary>
        /// Get the verticies of this 2D Box
        /// </summary>
        public Point2D<float>[] Vertices
        {
            get
            {
                float[] coordinates = new float[8];
                CvInvoke.cvBoxPoints(this, coordinates);
                Point2D<float>[] vertices = new Point2D<float>[4];
                for (int i = 0; i < vertices.Length; i++)
                {
                    vertices[i] = new Point2D<float>(coordinates[i << 1], coordinates[(i << 1) + 1]);
                }
                return vertices;
            }
        }
    }
}
