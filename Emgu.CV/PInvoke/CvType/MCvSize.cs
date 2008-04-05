using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;

namespace Emgu.CV
{
    /// <summary>
    /// Manager structure equivalent to CvSize
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct MCvSize
    {
        /// <summary>
        /// The width of the size
        /// </summary>
        public int width;
        /// <summary>
        /// The height of the size
        /// </summary>
        public int height;

        /// <summary>
        /// Create a new MCvSize using the specific width and height
        /// </summary>
        /// <param name="w">width</param>
        /// <param name="h">height</param>
        public MCvSize(int w, int h)
        {
            width = w; height = h;
        }
    }
}
