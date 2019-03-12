//----------------------------------------------------------------------------
//  Copyright (C) 2004-2019 by EMGU Corporation. All rights reserved.       
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
    /// Convolution
    /// </summary>
    public class CudaConvolution : SharedPtrObject
    {
        public CudaConvolution(Size userBlockSize)
        {

            _ptr = CudaInvoke.cudaConvolutionCreate(ref userBlockSize, ref _sharedPtr);
        }

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
