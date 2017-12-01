//----------------------------------------------------------------------------
//  Copyright (C) 2004-2017 by EMGU Corporation. All rights reserved.       
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
                _ptr = CudaInvoke.cudaCreateSeparableLinearFilter(CvInvoke.MakeType(srcDepth, srcChannels), CvInvoke.MakeType(dstDepth, dstChannels), iaRowKernel, iaColumnKernel, ref anchor, rowBorderType, columnBorderType);
        }
    }

    public static partial class CudaInvoke
    {
        [DllImport(CvInvoke.ExternCudaLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern IntPtr cudaCreateSeparableLinearFilter(
            int srcType, int dstType, IntPtr rowKernel, IntPtr columnKernel,
            ref Point anchor, CvEnum.BorderType rowBorderMode, CvEnum.BorderType columnBorderMode);
    }
}
