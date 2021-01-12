//----------------------------------------------------------------------------
//  Copyright (C) 2004-2021 by EMGU Corporation. All rights reserved.       
//----------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using Emgu.CV.CvEnum;
using Emgu.CV.Structure;
using Emgu.CV.Text;
using Emgu.CV.Util;
using Emgu.Util;

namespace Emgu.CV.XImgproc
{
    /// <summary>
    /// Interface for realizations of Domain Transform filter.
    /// </summary>
    public class DTFilter : UnmanagedObject
    {
        private IntPtr _sharedPtr;

        /// <summary>
        /// The three modes for filtering 2D signals in the article.
        /// </summary>
        public enum Mode
        {
            /// <summary>
            /// NC
            /// </summary>
            NC,
            /// <summary>
            /// IC
            /// </summary>
            IC,
            /// <summary>
            /// RF
            /// </summary>
            RF
        }

        /// <summary>
        /// Create instance of DTFilter and produce initialization routines.
        /// </summary>
        /// <param name="guide">Guided image (used to build transformed distance, which describes edge structure of guided image).</param>
        /// <param name="sigmaSpatial">Parameter in the original article, it's similar to the sigma in the coordinate space into bilateralFilter.</param>
        /// <param name="sigmaColor">Parameter in the original article, it's similar to the sigma in the color space into bilateralFilter.</param>
        /// <param name="mode">One form three modes DTF_NC, DTF_RF and DTF_IC which corresponds to three modes for filtering 2D signals in the article.</param>
        /// <param name="numIters">Optional number of iterations used for filtering, 3 is quite enough.</param>
        public DTFilter(IInputArray guide, double sigmaSpatial, double sigmaColor, Mode mode = Mode.NC, int numIters = 3)
        {
            using (InputArray iaGuide = guide.GetInputArray())
                _ptr = XImgprocInvoke.cveDTFilterCreate(iaGuide, sigmaSpatial, sigmaColor, mode, numIters, ref _sharedPtr);
        }

        /// <summary>
        /// Produce domain transform filtering operation on source image.
        /// </summary>
        /// <param name="src">Filtering image with unsigned 8-bit or floating-point 32-bit depth and up to 4 channels.</param>
        /// <param name="dst">Destination image.</param>
        /// <param name="dDepth">Optional depth of the output image. dDepth can be set to Default, which will be equivalent to src.depth().</param>
        public void Filter(IInputArray src, IOutputArray dst, DepthType dDepth = DepthType.Default)
        {
            using (InputArray iaSrc = src.GetInputArray())
            using (OutputArray oaDst = dst.GetOutputArray())
            {
                XImgprocInvoke.cveDTFilterFilter(_ptr, iaSrc, oaDst, dDepth);
            }

        }

        /// <summary>
        /// Release the unmanaged memory associated with this object
        /// </summary>
        protected override void DisposeObject()
        {
            if (_ptr != IntPtr.Zero)
            {
                XImgprocInvoke.cveDTFilterRelease(ref _ptr, ref _sharedPtr);
            }
        }
    }


    public static partial class XImgprocInvoke
    {
        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern IntPtr cveDTFilterCreate(IntPtr guide, double sigmaSpatial, double sigmaColor, DTFilter.Mode mode, int numIters, ref IntPtr sharedPtr);

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern void cveDTFilterFilter(IntPtr filter, IntPtr src, IntPtr dst, DepthType dDepth);

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern void cveDTFilterRelease(ref IntPtr filter, ref IntPtr sharedPtr);
    }
}
