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
    /// A generalized Deriv operator.
    /// </summary>
    public class CudaDerivFilter : CudaFilter
    {
        /// <summary>
        /// Creates a generalized Deriv operator.
        /// </summary>
        /// <param name="srcDepth">Source image depth.</param>
        /// <param name="srcChannels">Source image channels.</param>
        /// <param name="dstDepth">Destination array depth.</param>
        /// <param name="dstChannels">Destination array channels.</param>
        /// <param name="dx">Derivative order in respect of x.</param>
        /// <param name="dy">Derivative order in respect of y.</param>
        /// <param name="ksize">Aperture size.</param>
        /// <param name="normalize">Flag indicating whether to normalize (scale down) the filter coefficients or not.</param>
        /// <param name="scale">Optional scale factor for the computed derivative values. By default, no scaling is applied.</param>
        /// <param name="rowBorderType">Pixel extrapolation method in the vertical direction. </param>
        /// <param name="columnBorderType">Pixel extrapolation method in the horizontal direction.</param>
        public CudaDerivFilter(
            DepthType srcDepth, int srcChannels, DepthType dstDepth, int dstChannels,
            int dx, int dy,
            int ksize, 
            bool normalize = false, double scale = 1,
            CvEnum.BorderType rowBorderType = BorderType.Default,
            CvEnum.BorderType columnBorderType = BorderType.NegativeOne)
        {
            _ptr = CudaInvoke.cudaCreateDerivFilter(CvInvoke.MakeType(srcDepth, srcChannels), CvInvoke.MakeType(dstDepth, dstChannels), dx, dy, ksize, normalize, scale, rowBorderType, columnBorderType, ref _sharedPtr);
        }
    }

    public static partial class CudaInvoke
    {
        [DllImport(CvInvoke.ExternCudaLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern IntPtr cudaCreateDerivFilter(
            int srcType, int dstType,
            int dx, int dy,
            int ksize,
            [MarshalAs(CvInvoke.BoolMarshalType)]
            bool normalize,
            double scale,
            CvEnum.BorderType rowBorderMode, CvEnum.BorderType columnBorderMode, ref IntPtr sharedPtr);
    }
}
