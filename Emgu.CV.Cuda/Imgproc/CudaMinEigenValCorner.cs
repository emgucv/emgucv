//----------------------------------------------------------------------------
//  Copyright (C) 2004-2021 by EMGU Corporation. All rights reserved.       
//----------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Structure;
using Emgu.Util;

namespace Emgu.CV.Cuda
{
    /// <summary>
    /// Implementation for the minimum eigen value of a 2x2 derivative covariation matrix (the cornerness criteria).
    /// </summary>
    public class CudaMinEigenValCorner : CudaCornernessCriteria
    {
        /// <summary>
        /// Creates implementation for the minimum eigen value of a 2x2 derivative covariation matrix (the cornerness criteria).
        /// </summary>
        /// <param name="srcDepth">Input source depth. Only 8U and 32F are supported for now.</param>
        /// <param name="srcChannels">Input source type. Only single channel are supported for now.</param>
        /// <param name="blockSize">Neighborhood size.</param>
        /// <param name="kSize">Aperture parameter for the Sobel operator.</param>
        /// <param name="borderType">Pixel extrapolation method. Only BORDER_REFLECT101 and BORDER_REPLICATE are supported for now.</param>
        public CudaMinEigenValCorner(DepthType srcDepth, int srcChannels, int blockSize, int kSize, CvEnum.BorderType borderType = BorderType.Reflect101)
        {
            _ptr = CudaInvoke.cudaCreateMinEigenValCorner(CvInvoke.MakeType(srcDepth, srcChannels), blockSize, kSize, borderType, ref _sharedPtr);
        }
    }


    public static partial class CudaInvoke
    {
        [DllImport(CvInvoke.ExternCudaLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern IntPtr cudaCreateMinEigenValCorner(int srcType, int blockSize, int ksize, CvEnum.BorderType borderType, ref IntPtr sharedPtr);
    }
}
