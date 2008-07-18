using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;

namespace Emgu.CV
{
    /// <summary>
    /// Managed Structure equivalent to CvPoint2D64f
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct MCvPoint2D64f
    {
        /// <summary>
        /// x-coordinate
        /// </summary>
        public double x;

        /// <summary>
        /// y-coordinate
        /// </summary>
        public double y;

        /// <summary>
        /// Create a MCvPoint2D64f structure with the specific x and y coordinates
        /// </summary>
        /// <param name="x">x-coordinate</param>
        /// <param name="y">y-coordinate</param>
        public MCvPoint2D64f(double x, double y)
        {
            this.x = x; this.y = y;
        }
    }
}

