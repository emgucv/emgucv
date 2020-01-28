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
    /// This is a global tonemapping operator that models human visual system.
    /// Mapping function is controlled by adaptation parameter, that is computed using light adaptation and color adaptation.
    /// </summary>
    public partial class TonemapReinhard : Tonemap
    {
        private IntPtr _sharedPtr;

        /// <summary>
        /// Creates TonemapReinhard object.
        /// </summary>
        /// <param name="gamma">gamma value for gamma correction</param>
        /// <param name="intensity">result intensity in [-8, 8] range. Greater intensity produces brighter results.</param>
        /// <param name="lightAdapt">light adaptation in [0, 1] range. If 1 adaptation is based only on pixel value, if 0 it's global, otherwise it's a weighted mean of this two cases.</param>
        /// <param name="colorAdapt">chromatic adaptation in [0, 1] range. If 1 channels are treated independently, if 0 adaptation level is the same for each channel.</param>
        public TonemapReinhard(float gamma = 1.0f, float intensity = 0.0f, float lightAdapt = 1.0f, float colorAdapt = 0.0f)
            : base(IntPtr.Zero, IntPtr.Zero)
        {
            _ptr = CvInvoke.cveTonemapReinhardCreate(gamma, intensity, lightAdapt, colorAdapt, ref _tonemapPtr, ref _algorithmPtr, ref _sharedPtr);
        }

        /// <summary>
        /// Release the unmanaged memory associated with this TonemapReinhard
        /// </summary>
        protected override void DisposeObject()
        {
            if (IntPtr.Zero != _ptr)
            {
                CvInvoke.cveTonemapReinhardRelease(ref _ptr, ref _sharedPtr);
                _tonemapPtr = IntPtr.Zero;
                _algorithmPtr = IntPtr.Zero;
            }
        }
    }


    public static partial class CvInvoke
    {
        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern IntPtr cveTonemapReinhardCreate(float gamma, float intensity, float lightAdapt, float colorAdapt, ref IntPtr tonemap, ref IntPtr algorithm, ref IntPtr sharedPtr);
        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern void cveTonemapReinhardRelease(ref IntPtr tonemap, ref IntPtr sharedPtr);

    }
}