using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;

namespace Emgu.CV
{
    /// <summary>
    /// Managed structure equivalent to CvMat
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
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
        /// for uniform histograms 
        /// </summary>
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = (int)CvEnum.GENERAL.CV_MAX_DIM)]
        public Range[] thresh;

        /// <summary>
        /// for non-uniform histograms
        /// </summary>
        public IntPtr thresh2;

        /// <summary>
        /// embedded matrix header for array histograms
        /// </summary>
        public MCvMatND mat;

        /// <summary>
        /// A range
        /// </summary>
        [StructLayout(LayoutKind.Sequential)]
        public struct Range
        {
            /// <summary>
            /// The min value of this bin
            /// </summary>
            float min;
            /// <summary>
            /// The max value of this bin
            /// </summary>
            float max;
        }
    }
}
