//----------------------------------------------------------------------------
//  Copyright (C) 2004-2017 by EMGU Corporation. All rights reserved.       
//----------------------------------------------------------------------------

using System;
using System.Runtime.InteropServices;
using System.Drawing;
using Emgu.Util;

namespace Emgu.CV.XPhoto
{
    /// <summary>
    /// The base class for auto white balance algorithms.
    /// </summary>
    public abstract class WhiteBalancer : UnmanagedObject
    {
        /// <summary>
        /// Pointer to the native white balancer object
        /// </summary>
        protected IntPtr _whiteBalancerPtr;

        /// <summary>
        /// Applies white balancing to the input image.
        /// </summary>
        /// <param name="src">Input image</param>
        /// <param name="dst">White balancing result</param>
        public void BalanceWhite(IInputArray src, IOutputArray dst)
        {
            using (InputArray iaSrc = src.GetInputArray())
            using (OutputArray oaDst = dst.GetOutputArray())
                XPhotoInvoke.cveWhiteBalancerBalanceWhite(_whiteBalancerPtr, iaSrc, oaDst);
        }

        /// <summary>
        /// Reset the pointer to the native white balancer object
        /// </summary>
        protected override void DisposeObject()
        {
            _whiteBalancerPtr = IntPtr.Zero;
        }
    }

    /// <summary>
    /// A simple white balance algorithm that works by independently stretching each of the input image channels to the specified range. For increased robustness it ignores the top and bottom p% of pixel values.
    /// </summary>
    public partial class SimpleWB : WhiteBalancer
    {
        /// <summary>
        /// Creates a simple white balancer
        /// </summary>
        public SimpleWB()
        {
            _ptr = XPhotoInvoke.cveSimpleWBCreate(ref _whiteBalancerPtr);
        }

        /// <summary>
        /// Release all the unmanaged memory associated with this white balancer
        /// </summary>
        protected override void DisposeObject()
        {
            if (_ptr != IntPtr.Zero)
            {
                XPhotoInvoke.cveSimpleWBRelease(ref _ptr);
            }
            base.DisposeObject();
        }
    }

    /// <summary>
    /// Gray-world white balance algorithm.
    /// This algorithm scales the values of pixels based on a gray-world assumption which states that the average of all channels should result in a gray image.
    /// It adds a modification which thresholds pixels based on their saturation value and only uses pixels below the provided threshold in finding average pixel values.
    /// Saturation is calculated using the following for a 3-channel RGB image per pixel I and is in the range [0, 1]:
    /// Saturation[I]= max(R,G,B)−min(R,G,B) / max(R,G,B)
    /// A threshold of 1 means that all pixels are used to white-balance, while a threshold of 0 means no pixels are used. Lower thresholds are useful in white-balancing saturated images.
    /// Currently supports images of type CV_8UC3 and CV_16UC3.
    /// </summary>
    public partial class GrayworldWB : WhiteBalancer
    {
        /// <summary>
        /// Creates a gray-world white balancer
        /// </summary>
        public GrayworldWB()
        {
            _ptr = XPhotoInvoke.cveGrayworldWBCreate(ref _whiteBalancerPtr);
        }

        /// <summary>
        /// Release all the unmanaged memory associated with this white balancer
        /// </summary>
        protected override void DisposeObject()
        {
            if (_ptr != IntPtr.Zero)
            {
                XPhotoInvoke.cveGrayworldWBRelease(ref _ptr);
            }
            base.DisposeObject();
        }
    }

    /// <summary>
    /// More sophisticated learning-based automatic white balance algorithm.
    /// As GrayworldWB, this algorithm works by applying different gains to the input image channels, but their computation is a bit more involved compared to the simple gray-world assumption. 
    /// More details about the algorithm can be found in: Dongliang Cheng, Brian Price, Scott Cohen, and Michael S Brown. Effective learning-based illuminant estimation using simple features. In Proceedings of the IEEE Conference on Computer Vision and Pattern Recognition, pages 1000–1008, 2015.
    /// To mask out saturated pixels this function uses only pixels that satisfy the following condition:
    /// max(R,G,B) / range_max_val &lt; saturation_thresh 
    /// Currently supports images of type CV_8UC3 and CV_16UC3.
    /// </summary>
    public partial class LearningBasedWB : WhiteBalancer
    {
        /// <summary>
        /// Create a learning based white balancer.
        /// </summary>
        public LearningBasedWB()
        {
            _ptr = XPhotoInvoke.cveLearningBasedWBCreate(ref _whiteBalancerPtr);
        }

        /// <summary>
        /// Release all the unmanaged memory associated with this white balancer
        /// </summary>
        protected override void DisposeObject()
        {
            if (_ptr != IntPtr.Zero)
            {
                XPhotoInvoke.cveLearningBasedWBRelease(ref _ptr);
            }
            base.DisposeObject();
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
