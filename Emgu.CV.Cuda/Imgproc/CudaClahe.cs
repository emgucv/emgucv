//----------------------------------------------------------------------------
//  Copyright (C) 2004-2021 by EMGU Corporation. All rights reserved.       
//----------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Text;
using Emgu.CV.Features2D;
using Emgu.CV.Structure;
using Emgu.CV.Util;
using Emgu.Util;

namespace Emgu.CV.Cuda
{
    /// <summary>
    /// Contrast Limited Adaptive Histogram Equalization
    /// </summary>
    public class CudaClahe : SharedPtrObject
    {
        /// <summary>
        /// Create the Contrast Limited Adaptive Histogram Equalization
        /// </summary>
        /// <param name="clipLimit">Threshold for contrast limiting. Use 40.0 for default</param>
        /// <param name="tileGridSize">Size of grid for histogram equalization. Input image will be divided into equally sized rectangular tiles. This parameter defines the number of tiles in row and column. Use (8, 8) for default</param>
        public CudaClahe(double clipLimit, Size tileGridSize)
        {
            _ptr = CudaInvoke.cudaCLAHECreate(clipLimit, ref tileGridSize, ref _sharedPtr);
        }

        /// <summary>
        /// Equalizes the histogram of a grayscale image using Contrast Limited Adaptive Histogram Equalization.
        /// </summary>
        /// <param name="source">Source image</param>
        /// <param name="dst">Destination image</param>
        /// <param name="stream">Use a Stream to call the function asynchronously (non-blocking) or null to call the function synchronously (blocking).</param>
        public void Apply(IInputArray source, IOutputArray dst, Stream stream = null)
        {
            using (InputArray iaSource = source.GetInputArray())
            using (OutputArray oaDst = dst.GetOutputArray())
                CudaInvoke.cudaCLAHEApply(_ptr, iaSource, oaDst, stream);
        }

        /// <summary>
        /// Release all the unmanaged memory associated with this object
        /// </summary>
        protected override void DisposeObject()
        {
            if (_sharedPtr != IntPtr.Zero)
            {
                CudaInvoke.cudaCLAHERelease(ref _sharedPtr);
                _ptr = IntPtr.Zero;
            }
        }
    }

    public static partial class CudaInvoke
    {
        [DllImport(CvInvoke.ExternCudaLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern IntPtr cudaCLAHECreate(double clipLimit, ref Size tileGridSize, ref IntPtr sharedPtr);

        [DllImport(CvInvoke.ExternCudaLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern void cudaCLAHEApply(IntPtr clahe, IntPtr src, IntPtr dst, IntPtr stream);

        [DllImport(CvInvoke.ExternCudaLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern void cudaCLAHERelease(ref IntPtr clahe);
    }
}
