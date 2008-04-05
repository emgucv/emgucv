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
    public struct MCvMatND
    {
        /// <summary>
        /// 
        /// </summary>
        int type;
        /// <summary>
        /// 
        /// </summary>
        int dims;
        /// <summary>
        /// 
        /// </summary>
        IntPtr refcount;
        /// <summary>
        /// 
        /// </summary>
        int hdr_refcount;
        /// <summary>
        /// 
        /// </summary>
        IntPtr data;
        /// <summary>
        /// 
        /// </summary>
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = (int)CvEnum.GENERAL.CV_MAX_DIM)]
        public int[] size;

    }
}
