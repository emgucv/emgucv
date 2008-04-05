using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;

namespace Emgu.CV
{
    /// <summary>
    /// spatial and central moments
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct MCvMoments
    {
        /// <summary>
        /// spatial moments
        /// </summary>
        public double m00, m10, m01, m20, m11, m02, m30, m21, m12, m03;

        /// <summary>
        /// central moments
        /// </summary>
        public double mu20, mu11, mu02, mu30, mu21, mu12, mu03;

        /// <summary>
        /// m00 != 0 ? 1/sqrt(m00) : 0
        /// </summary>
        public double inv_sqrt_m00;
    }
}
