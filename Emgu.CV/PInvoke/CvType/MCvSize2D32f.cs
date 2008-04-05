using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;

namespace Emgu.CV
{
    /// <summary>
    /// Managed structure equivalent to CvSize2D32f
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct MCvSize2D32f
    {
        /// <summary>
        /// The width of the size
        /// </summary>
        public float width;

        /// <summary>
        /// The height of the size
        /// </summary>
        public float height;

        /// <summary>
        /// Create a new MCvSize2D32f using the specific width and height
        /// </summary>
        /// <param name="w">width</param>
        /// <param name="h">height</param>
        public MCvSize2D32f(float w, float h)
        {
            width = w; height = h;
        }
    }
}
