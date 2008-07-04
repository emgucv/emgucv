using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;

namespace Emgu.CV
{
    /// <summary>
    /// Quad-edge of planar subdivision
    /// </summary>
    public struct MCvQuadEdge2D
    {
        /// <summary>
        /// flags
        /// </summary>
        public int flags;

        /// <summary>
        /// Pointers to struct CvSubdiv2DPoint
        /// </summary>
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
        public IntPtr[] pt;

        /// <summary>
        /// Quad-edges, for each of the edges, lower 2 bits is index (0..3) and upper bits are quad-edge pointer
        /// </summary>
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
        public MCvSubdiv2DEdge[] next;
    }
}
