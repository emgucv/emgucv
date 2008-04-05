using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;

namespace Emgu.CV
{
    /// <summary>
    /// Managed structure equivalent to CvMat
    /// </summary>
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
    public struct MCvMat
    {
        /// <summary>
        /// CvMat signature (CV_MAT_MAGIC_VAL), element type and flags
        /// </summary>
        public int type;

        /// <summary>
        /// full row length in bytes
        /// </summary>
        public int step;

        /// <summary>
        /// underlying data reference counter
        /// </summary>
        public IntPtr refcount;

        /// <summary>
        /// 
        /// </summary>
        public int hdr_refcount;

        /// <summary>
        /// data pointers
        /// </summary>
        public IntPtr data;

        /// <summary>
        /// number of rows
        /// </summary>
        public int rows;

        /// <summary>
        /// number of columns
        /// </summary>
        public int cols;

        /// <summary>
        /// Width
        /// </summary>
        public int width { get { return cols; } }

        /// <summary>
        /// Height
        /// </summary>
        public int height { get { return rows; } }
    }
}
