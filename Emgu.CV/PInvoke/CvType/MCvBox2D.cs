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
    public struct MCvBox2D
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
    }
}
