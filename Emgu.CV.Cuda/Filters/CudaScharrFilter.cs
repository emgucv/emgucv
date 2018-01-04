//----------------------------------------------------------------------------
//  Copyright (C) 2004-2018 by EMGU Corporation. All rights reserved.       
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

    public class ScharrFilter : CudaFilter
    {
        public ScharrFilter(
          DepthType srcDepth, int srcChannels,
          DepthType dstDepth, int dstChannels,
          int dx, int dy,
          double scale,
          CvEnum.BorderType rowBorderMode = BorderType.Default,
          CvEnum.BorderType columnBorderMode = BorderType.Default)
        {
            _ptr = CudaInvoke.cudaCreateScharrFilter(
                CvInvoke.MakeType(srcDepth, srcChannels), CvInvoke.MakeType(dstDepth, dstChannels),
                dx, dy, scale, rowBorderMode, columnBorderMode);
        }
    }

    public static partial class CudaInvoke
    {
        [DllImport(CvInvoke.ExternCudaLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern IntPtr cudaCreateScharrFilter(
            int srcType, int dstType, int dx, int dy,
            double scale, 
            CvEnum.BorderType rowBorderMode, CvEnum.BorderType columnBorderMode);
    }
}
