//----------------------------------------------------------------------------
//  Copyright (C) 2004-2021 by EMGU Corporation. All rights reserved.       
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
    /// Adaptive logarithmic mapping is a fast global tonemapping algorithm that scales the image in logarithmic domain.
    /// Since it's a global operator the same function is applied to all the pixels, it is controlled by the bias parameter.
    /// </summary>
    public partial class TonemapDrago : Tonemap
    {
        private IntPtr _sharedPtr;

        /// <summary>
        /// Creates TonemapDrago object.
        /// </summary>
        /// <param name="gamma">gamma value for gamma correction.</param>
        /// <param name="saturation">positive saturation enhancement value. 1.0 preserves saturation, values greater than 1 increase saturation and values less than 1 decrease it.</param>
        /// <param name="bias">	value for bias function in [0, 1] range. Values from 0.7 to 0.9 usually give best results, default value is 0.85.</param>
        public TonemapDrago(float gamma = 1.0f, float saturation = 1.0f, float bias = 0.85f)
            : base(IntPtr.Zero, IntPtr.Zero)
        {
            _ptr = CvInvoke.cveTonemapDragoCreate(gamma, saturation, bias, ref _tonemapPtr, ref _algorithmPtr, ref _sharedPtr);
        }

        /// <summary>
        /// Release the unmanaged memory associated with this TonemapDrago
        /// </summary>
        protected override void DisposeObject()
        {
            if (IntPtr.Zero != _ptr)
            {
                CvInvoke.cveTonemapDragoRelease(ref _ptr, ref _sharedPtr);
            }

            _tonemapPtr = IntPtr.Zero;
            _algorithmPtr = IntPtr.Zero;
        }
    }

 
    public static partial class CvInvoke
    {
        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern IntPtr cveTonemapDragoCreate(float gamma, float saturation, float bias, ref IntPtr tonemap, ref IntPtr algorithm, ref IntPtr sharedPtr);
        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern void cveTonemapDragoRelease(ref IntPtr tonemap, ref IntPtr sharedPtr);

    }
}