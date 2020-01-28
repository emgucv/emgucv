//----------------------------------------------------------------------------
//  Copyright (C) 2004-2020 by EMGU Corporation. All rights reserved.       
//----------------------------------------------------------------------------

using System;
using System.Runtime.InteropServices;

namespace Emgu.CV.Structure
{
    /// <summary>
    /// The range use to setup the histogram
    /// </summary>
    [Serializable]
    [StructLayout(LayoutKind.Sequential)]
    public struct Range : IEquatable<Range>
    {
        static Range()
        {
            CvInvoke.cveGetRangeAll(ref _all);
        }

        private static Range _all = new Range();

        /// <summary>
        /// return the full range.
        /// </summary>
        public static Range All
        {
            get { return _all; }
        }

        private int _start;
        private int _end;

        /// <summary>
        /// Create a range of the specific min/max value
        /// </summary>
        /// <param name="start">The start value of this range</param>
        /// <param name="end">The max value of this range</param>
        public Range(int start, int end)
        {
            _start = start;
            _end = end;
        }

        /// <summary>
        /// The start value of this range
        /// </summary>
        public int Start
        {
            get { return _start; }
            set { _start = value; }
        }

        /// <summary>
        /// The end value of this range
        /// </summary>
        public int End
        {
            get { return _end; }
            set { _end = value; }
        }

        #region IEquatable<Range> Members
        /// <summary>
        /// Return true if the two Range equals
        /// </summary>
        /// <param name="other">The other Range to compare with</param>
        /// <returns>True if the two Range equals</returns>
        public bool Equals(Range other)
        {
            return Start.Equals(other.Start) && End.Equals(other.End);
        }
        #endregion
    }
}
