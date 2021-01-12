//----------------------------------------------------------------------------
//  Copyright (C) 2004-2021 by EMGU Corporation. All rights reserved.       
//----------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;
using Emgu.CV.Features2D;
using Emgu.CV.Structure;
using Emgu.CV.Util;
using Emgu.Util;
using System.Drawing;

namespace Emgu.CV.Cuda
{
    /// <summary>
    /// Base class for convolution (or cross-correlation) operator.
    /// </summary>
    public class CudaConvolution : SharedPtrObject
    {
        /// <summary>
        /// Create a Cuda Convolution object.
        /// </summary>
        /// <param name="userBlockSize">Block size. If you leave default value Size(0,0) then automatic estimation of block size will be used (which is optimized for speed). By varying user_block_size you can reduce memory requirements at the cost of speed.</param>
        public CudaConvolution(Size userBlockSize = new Size())
        {
            _ptr = CudaInvoke.cudaConvolutionCreate(ref userBlockSize, ref _sharedPtr);
        }

        /// <summary>
        /// Computes a convolution (or cross-correlation) of two images.
        /// </summary>
        /// <param name="image">Source image. Only CV_32FC1 images are supported for now.</param>
        /// <param name="templ">Template image. The size is not greater than the image size. The type is the same as image .</param>
        /// <param name="result">Result image. If image is W x H and templ is w x h, then result must be W-w+1 x H-h+1.</param>
        /// <param name="ccorr">Flags to evaluate cross-correlation instead of convolution.</param>
        /// <param name="stream">Stream for the asynchronous version</param>
        public void Convolve(
            IInputArray image,
            IInputArray templ,
            IOutputArray result,
            bool ccorr,
            Stream stream = null)
        {
            using (InputArray iaImage = image.GetInputArray())
            using (InputArray iaTempl = templ.GetInputArray())
            using (OutputArray oaResult = result.GetOutputArray())
            {
                CudaInvoke.cudaConvolutionConvolve(_ptr, iaImage, iaTempl, oaResult, ccorr, stream);
            }
        }

        /// <summary>
        /// Release all the unmanaged memory associated with this object
        /// </summary>
        protected override void DisposeObject()
        {
            if (_sharedPtr != IntPtr.Zero)
            {
                CudaInvoke.cudaConvolutionRelease(ref _sharedPtr);
                _ptr = IntPtr.Zero;
            }
        }
    }

    public static partial class CudaInvoke
    {
        [DllImport(CvInvoke.ExternCudaLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern IntPtr cudaConvolutionCreate(ref Size userBlockSize, ref IntPtr sharedPtr);

        [DllImport(CvInvoke.ExternCudaLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern void cudaConvolutionConvolve(
        IntPtr convolution,
        IntPtr image,
        IntPtr templ,
        IntPtr result,
        [MarshalAs(CvInvoke.BoolMarshalType)]
        bool ccorr,
        IntPtr stream);

        [DllImport(CvInvoke.ExternCudaLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern void cudaConvolutionRelease(ref IntPtr convolution);

    }
}
