using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;

namespace Emgu.CV
{
    /// <summary>
    /// Managed structure equivalent to CvMat
    /// </summary>
    public struct MCvHistogram
    {
        /// <summary>
        /// 
        /// </summary>
        public int type;
        
        /// <summary>
        /// Pointer to CvArr
        /// </summary>
        public IntPtr bins;

        /// <summary>
        /// 
        /// </summary>
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = (int)CvEnum.GENERAL.CV_MAX_DIM*2)]
        public float[] thresh;

        /// <summary>
        /// 
        /// </summary>
        public IntPtr thresh2;

        /// <summary>
        /// 
        /// </summary>
        public MCvMatND mat;
    }
}
