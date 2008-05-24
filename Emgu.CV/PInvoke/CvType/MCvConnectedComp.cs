using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;

namespace Emgu.CV
{
    /// <summary>
    /// Managed structure equivalent to CvConnectedComp
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct MCvConnectedComp
    {
        /// <summary>
        /// area of the segmented component
        /// </summary>
        public double area;

        /// <summary>
        /// gray scale value of the segmented component
        /// </summary>
        public float value;

        /// <summary>
        /// ROI of the segmented component
        /// </summary>
        public MCvRect rect;
    }

}
