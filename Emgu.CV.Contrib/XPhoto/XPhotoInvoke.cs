//----------------------------------------------------------------------------
//  Copyright (C) 2004-2018 by EMGU Corporation. All rights reserved.       
//----------------------------------------------------------------------------

using System;
using System.Runtime.InteropServices;
using System.Drawing;
using Emgu.CV.CvEnum;
using Emgu.Util;

namespace Emgu.CV.XPhoto
{
    /// <summary>
    /// BM3D denoising transform types
    /// </summary>
    public enum TransformTypes
    {
        /// <summary>
        /// Un-normalized Haar transform
        /// </summary>
        Haar = 0
    }

    /// <summary>
    /// BM3D steps
    /// </summary>
    public enum Bm3dSteps
    {
        /// <summary>
        /// Execute all steps of the algorithm 
        /// </summary>
        All = 0,

        /// <summary>
        /// Execute only first step of the algorithm 
        /// </summary>
        Step1 = 1,

        /// <summary>
        /// Execute only second step of the algorithm
        /// </summary>
        Step2 = 2
    }

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


        /// <summary>
        /// Implements an efficient fixed-point approximation for applying channel gains, which is the last step of multiple white balance algorithms.
        /// </summary>
        /// <param name="src">Input three-channel image in the BGR color space (either CV_8UC3 or CV_16UC3)</param>
        /// <param name="dst">Output image of the same size and type as src.</param>
        /// <param name="gainB">Gain for the B channel</param>
        /// <param name="gainG">Gain for the G channel</param>
        /// <param name="gainR">Gain for the R channel</param>
        public static void ApplyChannelGains(IInputArray src, IOutputArray dst, float gainB, float gainG, float gainR)
        {
            using (InputArray iaSrc = src.GetInputArray())
            using (OutputArray oaDst = dst.GetOutputArray())
            {
                cveApplyChannelGains(iaSrc, oaDst, gainB, gainG, gainR);
            }
        }
        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        private static extern void cveApplyChannelGains(IntPtr src, IntPtr dst, float gainB, float gainG, float gainR);

        /// <summary>
        /// Performs image denoising using the Block-Matching and 3D-filtering algorithm with several computational optimizations. Noise expected to be a gaussian white noise.
        /// </summary>
        /// <param name="src">Input 8-bit or 16-bit 1-channel image.</param>
        /// <param name="dstStep1">Output image of the first step of BM3D with the same size and type as src.</param>
        /// <param name="dstStep2">Output image of the second step of BM3D with the same size and type as src.</param>
        /// <param name="h">Parameter regulating filter strength. Big h value perfectly removes noise but also removes image details, smaller h value preserves details but also preserves some noise.</param>
        /// <param name="templateWindowSize">Size in pixels of the template patch that is used for block-matching. Should be power of 2.</param>
        /// <param name="searchWindowSize">Size in pixels of the window that is used to perform block-matching. Affect performance linearly: greater searchWindowsSize - greater denoising time. Must be larger than templateWindowSize.</param>
        /// <param name="blockMatchingStep1">Block matching threshold for the first step of BM3D (hard thresholding), i.e. maximum distance for which two blocks are considered similar. Value expressed in euclidean distance.</param>
        /// <param name="blockMatchingStep2">Block matching threshold for the second step of BM3D (Wiener filtering), i.e. maximum distance for which two blocks are considered similar. Value expressed in euclidean distance.</param>
        /// <param name="groupSize">Maximum size of the 3D group for collaborative filtering.</param>
        /// <param name="slidingStep">Sliding step to process every next reference block.</param>
        /// <param name="beta">Kaiser window parameter that affects the sidelobe attenuation of the transform of the window. Kaiser window is used in order to reduce border effects. To prevent usage of the window, set beta to zero.</param>
        /// <param name="normType">Norm used to calculate distance between blocks. L2 is slower than L1 but yields more accurate results.</param>
        /// <param name="step">Step of BM3D to be executed. Possible variants are: step 1, step 2, both steps.</param>
        /// <param name="transformType">	Type of the orthogonal transform used in collaborative filtering step. Currently only Haar transform is supported.</param>
        /// <remarks> <c href="http://www.cs.tut.fi/~foi/GCF-BM3D/BM3D_TIP_2007.pdf"/>   </remarks>
        public static void Bm3dDenoising(
            IInputArray src,
            IInputOutputArray dstStep1,
            IOutputArray dstStep2,
            float h = 1,
            int templateWindowSize = 4,
            int searchWindowSize = 16,
            int blockMatchingStep1 = 2500,
            int blockMatchingStep2 = 400,
            int groupSize = 8,
            int slidingStep = 1,
            float beta = 2.0f,
            NormType normType = NormType.L2,
            Bm3dSteps step = Bm3dSteps.All,
            TransformTypes transformType = TransformTypes.Haar)
        {
            using (InputArray iaSrc = src.GetInputArray())
            using (InputOutputArray ioaDstStep1 = dstStep1.GetInputOutputArray())
            using (OutputArray oaStep2 = dstStep2.GetOutputArray())
            {
                cveBm3dDenoising1(iaSrc, ioaDstStep1, oaStep2,
                    h, templateWindowSize, searchWindowSize, blockMatchingStep1, blockMatchingStep2,
                    groupSize, slidingStep, beta, normType, step, transformType);
            }
        }
        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        private static extern void cveBm3dDenoising1(
            IntPtr src,
            IntPtr dstStep1,
            IntPtr dstStep2,
            float h,
            int templateWindowSize,
            int searchWindowSize,
            int blockMatchingStep1,
            int blockMatchingStep2,
            int groupSize,
            int slidingStep,
            float beta,
            NormType normType,
            Bm3dSteps step,
            TransformTypes transformType);

        /// <summary>
        /// Performs image denoising using the Block-Matching and 3D-filtering algorithm with several computational optimizations. Noise expected to be a gaussian white noise.
        /// </summary>
        /// <param name="src">Input 8-bit or 16-bit 1-channel image.</param>
        /// <param name="dst">Output image with the same size and type as src.</param>
        /// <param name="h">Parameter regulating filter strength. Big h value perfectly removes noise but also removes image details, smaller h value preserves details but also preserves some noise.</param>
        /// <param name="templateWindowSize">Size in pixels of the template patch that is used for block-matching. Should be power of 2.</param>
        /// <param name="searchWindowSize">Size in pixels of the window that is used to perform block-matching. Affect performance linearly: greater searchWindowsSize - greater denoising time. Must be larger than templateWindowSize.</param>
        /// <param name="blockMatchingStep1">Block matching threshold for the first step of BM3D (hard thresholding), i.e. maximum distance for which two blocks are considered similar. Value expressed in euclidean distance.</param>
        /// <param name="blockMatchingStep2">Block matching threshold for the second step of BM3D (Wiener filtering), i.e. maximum distance for which two blocks are considered similar. Value expressed in euclidean distance.</param>
        /// <param name="groupSize">Maximum size of the 3D group for collaborative filtering.</param>
        /// <param name="slidingStep">Sliding step to process every next reference block.</param>
        /// <param name="beta">Kaiser window parameter that affects the sidelobe attenuation of the transform of the window. Kaiser window is used in order to reduce border effects. To prevent usage of the window, set beta to zero.</param>
        /// <param name="normType">Norm used to calculate distance between blocks. L2 is slower than L1 but yields more accurate results.</param>
        /// <param name="step">Step of BM3D to be executed. Allowed are only BM3D_STEP1 and BM3D_STEPALL. BM3D_STEP2 is not allowed as it requires basic estimate to be present.</param>
        /// <param name="transformType">Type of the orthogonal transform used in collaborative filtering step. Currently only Haar transform is supported.</param>
        /// <remarks> <c href="http://www.cs.tut.fi/~foi/GCF-BM3D/BM3D_TIP_2007.pdf"/> </remarks>
        public static void Bm3dDenoising(
            IInputArray src,
            IOutputArray dst,
            float h = 1,
            int templateWindowSize = 4,
            int searchWindowSize = 16,
            int blockMatchingStep1 = 2500,
            int blockMatchingStep2 = 400,
            int groupSize = 8,
            int slidingStep = 1,
            float beta = 2.0f,
            NormType normType = NormType.L2,
            Bm3dSteps step = Bm3dSteps.All,
            TransformTypes transformType = TransformTypes.Haar)
        {
            using (InputArray iaSrc = src.GetInputArray())
            using (OutputArray oaDst = dst.GetOutputArray())
            {
                cveBm3dDenoising2(iaSrc, oaDst,
                    h, templateWindowSize, searchWindowSize, blockMatchingStep1, blockMatchingStep2,
                    groupSize, slidingStep, beta, normType, step, transformType);
            }
        }
        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        private static extern void cveBm3dDenoising2(
            IntPtr src,
            IntPtr dst,
            float h,
            int templateWindowSize,
            int searchWindowSize,
            int blockMatchingStep1,
            int blockMatchingStep2,
            int groupSize,
            int slidingStep,
            float beta,
            NormType normType,
            Bm3dSteps step,
            TransformTypes transformType);
    }
}
