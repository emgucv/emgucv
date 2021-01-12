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
    /// A vertical or horizontal Scharr operator.
    /// </summary>
    public class ScharrFilter : CudaFilter
    {
        /// <summary>
        /// Creates a vertical or horizontal Scharr operator.
        /// </summary>
        /// <param name="srcDepth">Source image depth.</param>
        /// <param name="srcChannels">Source image channels.</param>
        /// <param name="dstDepth">Destination array depth.</param>
        /// <param name="dstChannels">Destination array channels.</param>
        /// <param name="dx">Order of the derivative x.</param>
        /// <param name="dy">Order of the derivative y.</param>
        /// <param name="scale">Optional scale factor for the computed derivative values. By default, no scaling is applied. </param>
        /// <param name="rowBorderMode">Pixel extrapolation method in the vertical direction. For details, see borderInterpolate.</param>
        /// <param name="columnBorderMode">Pixel extrapolation method in the horizontal direction.</param>
        public ScharrFilter(
          DepthType srcDepth, int srcChannels,
          DepthType dstDepth, int dstChannels,
          int dx, int dy,
          double scale = 1.0,
          CvEnum.BorderType rowBorderMode = BorderType.Default,
          CvEnum.BorderType columnBorderMode = BorderType.NegativeOne)
        {
            _ptr = CudaInvoke.cudaCreateScharrFilter(
                CvInvoke.MakeType(srcDepth, srcChannels), CvInvoke.MakeType(dstDepth, dstChannels),
                dx, dy, scale, rowBorderMode, columnBorderMode, ref _sharedPtr);
        }
    }

    public static partial class CudaInvoke
    {
        [DllImport(CvInvoke.ExternCudaLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern IntPtr cudaCreateScharrFilter(
            int srcType, int dstType, int dx, int dy,
            double scale, 
            CvEnum.BorderType rowBorderMode, CvEnum.BorderType columnBorderMode, ref IntPtr sharedPtr);
    }
}
