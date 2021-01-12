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
using Emgu.CV;
using Emgu.Util;

namespace Emgu.CV.Cuda
{
    /// <summary>
    /// SeparableLinearFilter
    /// </summary>
    public class SeparableLinearFilter : CudaFilter
    {
        /// <summary>
        /// Create a SeparableLinearFilter
        /// </summary>
        /// <param name="srcDepth">Source array depth</param>
        /// <param name="srcChannels">Source array channels</param>
        /// <param name="dstDepth">Destination array depth</param>
        /// <param name="dstChannels">Destination array channels</param>
        /// <param name="rowKernel">Horizontal filter coefficients. Support kernels with size &lt;= 32 .</param>
        /// <param name="columnKernel">Vertical filter coefficients. Support kernels with size &lt;= 32 .</param>
        /// <param name="anchor">Anchor position within the kernel. Negative values mean that anchor is positioned at the aperture center.</param>
        /// <param name="rowBorderType">Pixel extrapolation method in the vertical direction</param>
        /// <param name="columnBorderType">Pixel extrapolation method in the horizontal direction</param>
        public SeparableLinearFilter(
            DepthType srcDepth, int srcChannels,
            DepthType dstDepth, int dstChannels,
            IInputArray rowKernel,
            IInputArray columnKernel,
            Point anchor,
            CvEnum.BorderType rowBorderType = BorderType.Default,
         CvEnum.BorderType columnBorderType = BorderType.Default)
        {
            using (InputArray iaRowKernel = rowKernel.GetInputArray())
            using (InputArray iaColumnKernel = columnKernel.GetInputArray())
                _ptr = CudaInvoke.cudaCreateSeparableLinearFilter(CvInvoke.MakeType(srcDepth, srcChannels), CvInvoke.MakeType(dstDepth, dstChannels), iaRowKernel, iaColumnKernel, ref anchor, rowBorderType, columnBorderType, ref _sharedPtr);
        }
    }

    public static partial class CudaInvoke
    {
        [DllImport(CvInvoke.ExternCudaLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern IntPtr cudaCreateSeparableLinearFilter(
            int srcType, int dstType, IntPtr rowKernel, IntPtr columnKernel,
            ref Point anchor, CvEnum.BorderType rowBorderMode, CvEnum.BorderType columnBorderMode, ref IntPtr sharedPtr);
    }
}
