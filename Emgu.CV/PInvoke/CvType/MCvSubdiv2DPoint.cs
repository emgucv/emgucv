using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;

namespace Emgu.CV
{
    /// <summary>
    /// Managed CvSubdiv2DPoint structure
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct MCvSubdiv2DPoint
    {
        /// <summary>
        /// 
        /// </summary>
        public int flags;
        /// <summary>
        /// 
        /// </summary>
        public MCvSubdiv2DEdge first;
        /// <summary>
        /// 
        /// </summary>
        public MCvPoint2D32f pt;

    }
}
