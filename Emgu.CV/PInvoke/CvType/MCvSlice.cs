using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;

namespace Emgu.CV
{
    /// <summary>
    /// Managed structure equivalent to CvSlice
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct MCvSlice
    {
        /// <summary>
        /// Start index
        /// </summary>
        public int start_index;
        /// <summary>
        /// End index
        /// </summary>
        public int end_index;

        /// <summary>
        /// Create a new MCvSlice using the specific start and end index
        /// </summary>
        /// <param name="start">start index</param>
        /// <param name="end">end index</param>
        public MCvSlice(int start, int end)
        {
            start_index = start;
            end_index = end;
        }
    }
}
