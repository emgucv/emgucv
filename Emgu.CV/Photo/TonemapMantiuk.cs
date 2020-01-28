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

namespace Emgu.CV
{
    /// <summary>
    /// This algorithm transforms image to contrast using gradients on all levels of gaussian pyramid, transforms contrast values to HVS response and scales the response. After this the image is reconstructed from new contrast values.
    /// </summary>
    public partial class TonemapMantiuk : Tonemap
    {
        private IntPtr _sharedPtr;

        /// <summary>
        /// Creates TonemapMantiuk object
        /// </summary>
        /// <param name="gamma">gamma value for gamma correction.</param>
        /// <param name="scale">contrast scale factor. HVS response is multiplied by this parameter, thus compressing dynamic range. Values from 0.6 to 0.9 produce best results.</param>
        /// <param name="saturation">saturation enhancement value.</param>
        public TonemapMantiuk(float gamma = 1.0f, float scale = 0.7f, float saturation = 1.0f)
            : base(IntPtr.Zero, IntPtr.Zero)
        {
            _ptr = CvInvoke.cveTonemapMantiukCreate(gamma, scale, saturation, ref _tonemapPtr, ref _algorithmPtr, ref _sharedPtr);
        }

        /// <summary>
        /// Release the unmanaged memory associated with this TonemapMantiuk
        /// </summary>
        protected override void DisposeObject()
        {
            if (IntPtr.Zero != _ptr)
            {
                CvInvoke.cveTonemapMantiukRelease(ref _ptr, ref _sharedPtr);
                _tonemapPtr = IntPtr.Zero;
                _algorithmPtr = IntPtr.Zero;
            }
        }
    }

    public static partial class CvInvoke
    {
        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern IntPtr cveTonemapMantiukCreate(float gamma, float scale, float saturation, ref IntPtr tonemap, ref IntPtr algorithm, ref IntPtr sharedPtr);
        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern void cveTonemapMantiukRelease(ref IntPtr tonemap, ref IntPtr sharedPtr);
    }
}