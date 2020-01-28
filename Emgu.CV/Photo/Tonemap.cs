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
    /// Base class for tonemapping algorithms - tools that are used to map HDR image to 8-bit range.
    /// </summary>
    public partial class Tonemap : UnmanagedObject, IAlgorithm
    {
        private IntPtr _sharedPtr;

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
            _ptr = CvInvoke.cveTonemapCreate(gamma, ref _algorithmPtr, ref _sharedPtr);
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
                CvInvoke.cveTonemapRelease(ref _ptr, ref _sharedPtr);
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
        internal static extern IntPtr cveTonemapCreate(float gamma, ref IntPtr algorithm, ref IntPtr sharedPtr);

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern void cveTonemapRelease(ref IntPtr tonemap, ref IntPtr sharedPtr);
    }
}