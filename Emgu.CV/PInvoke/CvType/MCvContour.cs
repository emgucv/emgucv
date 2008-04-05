using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;

namespace Emgu.CV
{
    /// <summary>
    /// Managed structure equivalent to CvContour
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct MCvContour
    {
        ///<summary>
        /// micsellaneous flags 
        ///</summary>
        public int flags;
        ///<summary>
        /// size of sequence header 
        ///</summary>
        public int header_size;
        ///<summary>
        /// previous sequence 
        ///</summary>
        public IntPtr h_prev;
        ///<summary>
        /// next sequence 
        ///</summary>
        public IntPtr h_next;
        ///<summary>
        /// 2nd previous sequence 
        ///</summary>
        public IntPtr v_prev;
        ///<summary>
        /// 2nd next sequence 
        ///</summary>
        public IntPtr v_next;
        ///<summary>
        /// total number of elements 
        ///</summary>
        public int total;
        ///<summary>
        /// size of sequence element in bytes 
        ///</summary>
        public int elem_size;
        ///<summary>
        /// maximal bound of the last block 
        ///</summary>
        public IntPtr block_max;
        ///<summary>
        /// current write pointer 
        ///</summary>
        public IntPtr ptr;
        ///<summary>
        /// how many elements allocated when the seq grows 
        ///</summary>
        public int delta_elems;
        ///<summary>
        /// where the seq is stored 
        ///</summary>
        public IntPtr storage;
        ///<summary>
        /// free blocks list 
        ///</summary>
        public IntPtr free_blocks;
        ///<summary>
        /// pointer to the first sequence block 
        ///</summary>
        public IntPtr first;

        /// <summary>
        /// 
        /// </summary>
        public MCvRect rect;
        /// <summary>
        /// 
        /// </summary>
        public int color;
        /// <summary>
        /// 
        /// </summary>
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)]
        public int[] reserved;
    }
}
