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
using Emgu.Util;

namespace Emgu.CV.Cuda
{
    public class RowSumFilter : CudaFilter
    {
        public RowSumFilter(DepthType srcDepth, int srcChannels, DepthType dstDepth, int dstChannels, int ksize, int anchor, CvEnum.BorderType borderType = BorderType.Default, MCvScalar borderValue = new MCvScalar())
        {
            _ptr = CudaInvoke.cudaCreateRowSumFilter(CvInvoke.MakeType(srcDepth, srcChannels), CvInvoke.MakeType(dstDepth, dstChannels), ksize, anchor, borderType, ref borderValue);
        }
    }

    public static partial class CudaInvoke
    {
        [DllImport(CvInvoke.ExternCudaLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern IntPtr cudaCreateRowSumFilter(int srcType, int dstType, int ksize, int anchor, CvEnum.BorderType borderMode, ref MCvScalar borderVal);
    }
}
