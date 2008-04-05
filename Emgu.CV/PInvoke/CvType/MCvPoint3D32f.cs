using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;

namespace Emgu.CV
{
    /// <summary>
    /// Managed Structure equivalent to CvPoint2D32f
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct MCvPoint3D32f
    {
                /// <summary>
        /// x-coordinate
        /// </summary>
        public float x;

        /// <summary>
        /// y-coordinate
        /// </summary>
        public float y;

        /// <summary>
        /// z-coordinate
        /// </summary>
        public float z;

        /// <summary>
        /// Create a MCvPoint2D32f structure with the specific x and y coordinates
        /// </summary>
        /// <param name="x">x-coordinate</param>
        /// <param name="y">y-coordinate</param>
        /// <param name="z">z-coordinate</param>
        public MCvPoint3D32f(float x, float y, float z)
        {
            this.x = x; 
            this.y = y;
            this.z = z;
        }
    }
}
