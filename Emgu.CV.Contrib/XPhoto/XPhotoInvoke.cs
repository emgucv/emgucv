//----------------------------------------------------------------------------
//  Copyright (C) 2004-2016 by EMGU Corporation. All rights reserved.       
//----------------------------------------------------------------------------

using System;
using System.Runtime.InteropServices;
using System.Drawing;
using System.Runtime.CompilerServices;
using Emgu.Util;

namespace Emgu.CV.XPhoto
{
    public abstract class WhiteBalancer : UnmanagedObject
    {
        protected IntPtr _whiteBalancerPtr;

        public void BalanceWhite(IInputArray src, IOutputArray dst)
        {
            using (InputArray iaSrc = src.GetInputArray())
            using (OutputArray oaDst = dst.GetOutputArray())
                XPhotoInvoke.cveWhiteBalancerBalanceWhite(_whiteBalancerPtr, iaSrc, oaDst);
        }
    }

    /// <summary>
    /// Class that contains entry points for the XPhoto module.
    /// </summary>
    public static partial class XPhotoInvoke
    {
        static XPhotoInvoke()
        {
            CvInvoke.CheckLibraryLoaded();
        }

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern void cveWhiteBalancerBalanceWhite(IntPtr whiteBalancer, IntPtr src, IntPtr dst);

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern IntPtr cveSimpleWBCreate(ref IntPtr whiteBalancer);
        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern void cveSimpleWBRelease(ref IntPtr whiteBalancer);

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern IntPtr cveGrayworldWBCreate(ref IntPtr whiteBalancer);
        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern void cveGrayworldWBRelease(ref IntPtr whiteBalancer);

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern IntPtr cveLearningBasedWBCreate(ref IntPtr whiteBalancer);
        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern void cveLearningBasedWBRelease(ref IntPtr whiteBalancer);

        /// <summary>
        /// The function implements simple dct-based denoising, link: http://www.ipol.im/pub/art/2011/ys-dct/.
        /// </summary>
        /// <param name="src">Source image</param>
        /// <param name="dst">Destination image</param>
        /// <param name="sigma">Expected noise standard deviation</param>
        /// <param name="psize">Size of block side where dct is computed</param>
        public static void DctDenoising(Mat src, Mat dst, double sigma, int psize = 16)
        {
            cveDctDenoising(src, dst, sigma, psize);
        }

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        private static extern void cveDctDenoising(IntPtr src, IntPtr dst, double sigma, int psize);

        /// <summary>
        /// Inpaint type
        /// </summary>
        public enum InpaintType
        {
            /// <summary>
            /// Shift map
            /// </summary>
            Shiftmap = 0
        }

        /// <summary>
        /// The function implements different single-image inpainting algorithms
        /// </summary>
        /// <param name="src">source image, it could be of any type and any number of channels from 1 to 4. In case of 3- and 4-channels images the function expect them in CIELab colorspace or similar one, where first color component shows intensity, while second and third shows colors. Nonetheless you can try any colorspaces.</param>
        /// <param name="mask">mask (CV_8UC1), where non-zero pixels indicate valid image area, while zero pixels indicate area to be inpainted</param>
        /// <param name="dst">destination image</param>
        /// <param name="algorithmType">algorithm type</param>
        public static void Inpaint(Mat src, Mat mask, Mat dst, InpaintType algorithmType)
        {
            cveXInpaint(src, mask, dst, algorithmType);
        }

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        private static extern void cveXInpaint(IntPtr src, IntPtr mask, IntPtr dst, InpaintType algorithmType);
    }
}
