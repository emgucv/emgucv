//----------------------------------------------------------------------------
//  Copyright (C) 2004-2020 by EMGU Corporation. All rights reserved.       
//----------------------------------------------------------------------------

using System;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Runtime.InteropServices;
using Emgu.CV.Structure;
using Emgu.Util;

namespace Emgu.CV.XPhoto
{

    /// <summary>
    /// This algorithm decomposes image into two layers: base layer and detail layer using bilateral filter and compresses contrast of the base layer thus preserving all the details.
    /// This implementation uses regular bilateral filter from opencv.
    /// </summary>
    public partial class TonemapDurand : Tonemap
    {
        private IntPtr _sharedPtr;

        /// <summary>
        /// Creates TonemapDurand object.
        /// </summary>
        /// <param name="gamma">gamma value for gamma correction. </param>
        /// <param name="contrast">resulting contrast on logarithmic scale, i. e. log(max / min), where max and min are maximum and minimum luminance values of the resulting image.</param>
        /// <param name="saturation">saturation enhancement value. </param>
        /// <param name="sigmaSpace">bilateral filter sigma in color space</param>
        /// <param name="sigmaColor">bilateral filter sigma in coordinate space</param>
        public TonemapDurand(float gamma = 1.0f, float contrast = 4.0f, float saturation = 1.0f, float sigmaSpace = 2.0f, float sigmaColor = 2.0f)
            : base(IntPtr.Zero, IntPtr.Zero)
        {
            _ptr = XPhotoInvoke.cveTonemapDurandCreate(gamma, contrast, saturation, sigmaSpace, sigmaColor, ref _tonemapPtr, ref _algorithmPtr, ref _sharedPtr);
        }

        /// <summary>
        /// Release the unmanaged memory associated with this TonemapDurand
        /// </summary>
        protected override void DisposeObject()
        {
            if (IntPtr.Zero != _ptr)
            {
                XPhotoInvoke.cveTonemapDurandRelease(ref _ptr, ref _sharedPtr);
                _tonemapPtr = IntPtr.Zero;
                _algorithmPtr = IntPtr.Zero;
            }
        }
    }

 
    public static partial class XPhotoInvoke
    {
        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern IntPtr cveTonemapDurandCreate(float gamma, float contrast, float saturation, float sigmaSpace, float sigmaColor, ref IntPtr tonemap, ref IntPtr algorithm, ref IntPtr sharedPtr);
        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern void cveTonemapDurandRelease(ref IntPtr tonemap, ref IntPtr sharedPtr);
    }
}