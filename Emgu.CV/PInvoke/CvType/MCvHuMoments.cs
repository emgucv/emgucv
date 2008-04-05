using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;

namespace Emgu.CV
{
    /// <summary>
    /// Hu invariants
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct MCvHuMoments
    {
        /// <summary>
        /// Hu invariants
        /// </summary>
        public double hu1, hu2, hu3, hu4, hu5, hu6, hu7;
    }
}
