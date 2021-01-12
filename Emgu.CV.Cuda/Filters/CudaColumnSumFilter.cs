//----------------------------------------------------------------------------
//  Copyright (C) 2004-2021 by EMGU Corporation. All rights reserved.       
//----------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;
using System.Drawing;
using Emgu.CV.CvEnum;
using Emgu.CV.Features2D;
using Emgu.CV.Structure;
using Emgu.CV.Util;
using Emgu.Util;

namespace Emgu.CV.Cuda
{
    /// <summary>
    /// A vertical 1D box filter.
    /// </summary>
    public class ColumnSumFilter : CudaFilter
    {
        /// <summary>
        /// Creates a vertical 1D box filter.
        /// </summary>
        /// <param name="srcDepth">Input image depth.</param>
        /// <param name="srcChannels">Input image channel.</param>
        /// <param name="dstDepth">Output image depth.</param>
        /// <param name="dstChannels">Output image channel.</param>
        /// <param name="ksize">Kernel size.</param>
        /// <param name="anchor">Anchor point. The default value (-1) means that the anchor is at the kernel center.</param>
        /// <param name="borderType">Pixel extrapolation method.</param>
        /// <param name="borderValue">Default border value.</param>
        public ColumnSumFilter(
            DepthType srcDepth, int srcChannels, 
            DepthType dstDepth, int dstChannels, 
            int ksize, int anchor = -1, 
            CvEnum.BorderType borderType = BorderType.Default, 
            MCvScalar borderValue = new MCvScalar())
        {
            _ptr = CudaInvoke.cudaCreateColumnSumFilter(CvInvoke.MakeType(srcDepth, srcChannels), CvInvoke.MakeType(dstDepth, dstChannels), ksize, anchor, borderType, ref borderValue, ref _sharedPtr);
        }
    }

    public static partial class CudaInvoke
    {
        [DllImport(CvInvoke.ExternCudaLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern IntPtr cudaCreateColumnSumFilter(int srcType, int dstType, int ksize, int anchor, CvEnum.BorderType borderMode, ref MCvScalar borderVal, ref IntPtr sharedPtr);
    }
}
