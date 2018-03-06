//----------------------------------------------------------------------------
//  Copyright (C) 2004-2018 by EMGU Corporation. All rights reserved.       
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
    /// Base class for tonemapping algorithms - tools that are used to map HDR image to 8-bit range.
    /// </summary>
    public partial class Tonemap : UnmanagedObject, IAlgorithm
    {
        /// <summary>
        /// The pointer to the unmanaged Tonemap object
        /// </summary>
        protected IntPtr _tonemapPtr;

        /// <summary>
        /// The pointer to the unmanaged Algorithm object
        /// </summary>
        protected IntPtr _algorithmPtr;

        /// <summary>
        /// The pointer to the unamanged Algorith object
        /// </summary>
        public IntPtr AlgorithmPtr
        {
            get
            {
                return _algorithmPtr;
            }
        }

        /// <summary>
        /// Default constructor that creates empty Tonemap
        /// </summary>
        /// <param name="ptr">The pointer to the unmanaged object</param>
        /// <param name="tonemapPtr">The pointer to the tonemap object</param>
        protected Tonemap(IntPtr ptr, IntPtr tonemapPtr)
        {
            _ptr = ptr;
            _tonemapPtr = tonemapPtr;
        }

        /// <summary>
        /// Creates simple linear mapper with gamma correction.
        /// </summary>
        /// <param name="gamma">positive value for gamma correction. Gamma value of 1.0 implies no correction, gamma equal to 2.2f is suitable for most displays. Generally gamma &gt; 1 brightens the image and gamma &lt; 1 darkens it.</param>
        public Tonemap(float gamma = 1.0f)
        {
            _ptr = CvInvoke.cveTonemapCreate(gamma, ref _algorithmPtr);
            _tonemapPtr = _ptr;
        }

        /// <summary>
        /// Tonemaps image.
        /// </summary>
        /// <param name="src">Source image - 32-bit 3-channel Mat</param>
        /// <param name="dst">destination image - 32-bit 3-channel Mat with values in [0, 1] range</param>
        public void Process(IInputArray src, IOutputArray dst)
        {
            using (InputArray iaSrc = src.GetInputArray())
            using (OutputArray oaDst = dst.GetOutputArray())
            {
                CvInvoke.cveTonemapProcess(_tonemapPtr, iaSrc, oaDst);
            }
        }

        /// <summary>
        /// Release the unmanaged memory associated with this Tonemap
        /// </summary>
        protected override void DisposeObject()
        {
            if (IntPtr.Zero != _ptr)
            {
                CvInvoke.cveTonemapRelease(ref _ptr);
            }
            _tonemapPtr = IntPtr.Zero;
            _algorithmPtr = IntPtr.Zero;
        }
    }

    /// <summary>
    /// Adaptive logarithmic mapping is a fast global tonemapping algorithm that scales the image in logarithmic domain.
    /// Since it's a global operator the same function is applied to all the pixels, it is controlled by the bias parameter.
    /// </summary>
    public partial class TonemapDrago : Tonemap
    {
        /// <summary>
        /// Creates TonemapDrago object.
        /// </summary>
        /// <param name="gamma">gamma value for gamma correction.</param>
        /// <param name="saturation">positive saturation enhancement value. 1.0 preserves saturation, values greater than 1 increase saturation and values less than 1 decrease it.</param>
        /// <param name="bias">	value for bias function in [0, 1] range. Values from 0.7 to 0.9 usually give best results, default value is 0.85.</param>
        public TonemapDrago(float gamma = 1.0f, float saturation = 1.0f, float bias = 0.85f)
            :base(IntPtr.Zero, IntPtr.Zero)
        {
            _ptr = CvInvoke.cveTonemapDragoCreate(gamma, saturation, bias, ref _tonemapPtr, ref _algorithmPtr);
        }

        /// <summary>
        /// Release the unmanaged memory associated with this TonemapDrago
        /// </summary>
        protected override void DisposeObject()
        {
            if (IntPtr.Zero != _ptr)
            {
                CvInvoke.cveTonemapDragoRelease(ref _ptr);
            }

            _tonemapPtr = IntPtr.Zero;
            _algorithmPtr = IntPtr.Zero;
        }
    }

    /// <summary>
    /// This algorithm decomposes image into two layers: base layer and detail layer using bilateral filter and compresses contrast of the base layer thus preserving all the details.
    /// This implementation uses regular bilateral filter from opencv.
    /// </summary>
    public partial class TonemapDurand : Tonemap
    {
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
            _ptr = CvInvoke.cveTonemapDurandCreate(gamma, contrast, saturation, sigmaSpace, sigmaColor, ref _tonemapPtr, ref _algorithmPtr);
        }

        /// <summary>
        /// Release the unmanaged memory associated with this TonemapDurand
        /// </summary>
        protected override void DisposeObject()
        {
            if (IntPtr.Zero != _ptr)
            {
                CvInvoke.cveTonemapDragoRelease(ref _ptr);
            }

            _tonemapPtr = IntPtr.Zero;
            _algorithmPtr = IntPtr.Zero;
        }
    }

    /// <summary>
    /// This is a global tonemapping operator that models human visual system.
    /// Mapping function is controlled by adaptation parameter, that is computed using light adaptation and color adaptation.
    /// </summary>
    public partial class TonemapReinhard : Tonemap
    {
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
            _ptr = CvInvoke.cveTonemapReinhardCreate(gamma, intensity, lightAdapt, colorAdapt, ref _tonemapPtr, ref _algorithmPtr);
        }

        /// <summary>
        /// Release the unmanaged memory associated with this TonemapReinhard
        /// </summary>
        protected override void DisposeObject()
        {
            if (IntPtr.Zero != _ptr)
            {
                CvInvoke.cveTonemapReinhardRelease(ref _ptr);
            }

            _tonemapPtr = IntPtr.Zero;
            _algorithmPtr = IntPtr.Zero;
        }
    }

    /// <summary>
    /// This algorithm transforms image to contrast using gradients on all levels of gaussian pyramid, transforms contrast values to HVS response and scales the response. After this the image is reconstructed from new contrast values.
    /// </summary>
    public partial class TonemapMantiuk : Tonemap
    {
        /// <summary>
        /// Creates TonemapMantiuk object
        /// </summary>
        /// <param name="gamma">gamma value for gamma correction.</param>
        /// <param name="scale">contrast scale factor. HVS response is multiplied by this parameter, thus compressing dynamic range. Values from 0.6 to 0.9 produce best results.</param>
        /// <param name="saturation">saturation enhancement value.</param>
        public TonemapMantiuk(float gamma = 1.0f, float scale = 0.7f, float saturation = 1.0f)
            : base(IntPtr.Zero, IntPtr.Zero)
        {
            _ptr = CvInvoke.cveTonemapMantiukCreate(gamma, scale, saturation, ref _tonemapPtr, ref _algorithmPtr);
        }

        /// <summary>
        /// Release the unmanaged memory associated with this TonemapMantiuk
        /// </summary>
        protected override void DisposeObject()
        {
            if (IntPtr.Zero != _ptr)
            {
                CvInvoke.cveTonemapMantiukRelease(ref _ptr);
            }

            _tonemapPtr = IntPtr.Zero;
            _algorithmPtr = IntPtr.Zero;
        }
    }

    public static partial class CvInvoke
    {
        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern void cveTonemapProcess(IntPtr tonemap, IntPtr src, IntPtr dst);

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern IntPtr cveTonemapCreate(float gamma, ref IntPtr algorithm);

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern void cveTonemapRelease(ref IntPtr tonemap);


        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern IntPtr cveTonemapDragoCreate(float gamma, float saturation, float bias, ref IntPtr tonemap, ref IntPtr algorithm);
        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern void cveTonemapDragoRelease(ref IntPtr tonemap);

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern IntPtr cveTonemapDurandCreate(float gamma, float contrast, float saturation, float sigmaSpace, float sigmaColor, ref IntPtr tonemap, ref IntPtr algorithm);
        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern void cveTonemapDurandRelease(ref IntPtr tonemap);

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern IntPtr cveTonemapReinhardCreate(float gamma, float intensity, float lightAdapt, float colorAdapt, ref IntPtr tonemap, ref IntPtr algorithm);
        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern void cveTonemapReinhardRelease(ref IntPtr tonemap);

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern IntPtr cveTonemapMantiukCreate(float gamma, float scale, float saturation, ref IntPtr tonemap, ref IntPtr algorithm);
        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern void cveTonemapMantiukRelease(ref IntPtr tonemap);
    }
}