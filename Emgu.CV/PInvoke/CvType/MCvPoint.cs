using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;

namespace Emgu.CV
{
    /// <summary>
    /// Managed Structure equivalent to CvPoint
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct MCvPoint
    {
        /// <summary>
        /// x-coordinate
        /// </summary>
        public int x;
        /// <summary>
        /// y-coordinate
        /// </summary>
        public int y;

        /// <summary>
        /// Create a MCvPoint structure with the specific x and y coordinates
        /// </summary>
        /// <param name="x">x-coordinate</param>
        /// <param name="y">y-coordinate</param>
        public MCvPoint(int x, int y)
        {
            this.x = x; this.y = y;
        }
    }
}
