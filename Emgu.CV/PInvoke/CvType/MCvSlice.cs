//----------------------------------------------------------------------------
//  Copyright (C) 2004-2020 by EMGU Corporation. All rights reserved.       
//----------------------------------------------------------------------------

using System;
using System.Runtime.InteropServices;

namespace Emgu.CV.Structure
{
    /// <summary>
    /// Managed structure equivalent to CvSlice
    /// </summary>
    [Serializable]
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

        /// <summary>
        /// Get the equivalent of CV_WHOLE_SEQ
        /// </summary>
        public static MCvSlice WholeSeq
        {
            get { return new MCvSlice(0, 0x3fffffff); }
        }

    }
}
