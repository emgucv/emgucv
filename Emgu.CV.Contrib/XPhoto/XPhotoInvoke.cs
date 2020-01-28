//----------------------------------------------------------------------------
//  Copyright (C) 2004-2020 by EMGU Corporation. All rights reserved.       
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
        internal static extern IntPtr cveSimpleWBCreate(ref IntPtr whiteBalancer, ref IntPtr sharedPtr);
        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern void cveSimpleWBRelease(ref IntPtr whiteBalancer, ref IntPtr sharedPtr);

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern IntPtr cveGrayworldWBCreate(ref IntPtr whiteBalancer, ref IntPtr sharedPtr);
        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern void cveGrayworldWBRelease(ref IntPtr whiteBalancer, ref IntPtr sharedPtr);

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern IntPtr cveLearningBasedWBCreate(ref IntPtr whiteBalancer, ref IntPtr sharedPtr);
        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern void cveLearningBasedWBRelease(ref IntPtr whiteBalancer, ref IntPtr sharedPtr);

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
            Shiftmap = 0,
            /// <summary>
            /// Performs Frequency Selective Reconstruction (FSR). Slower but better inpainting
            /// </summary>
            FsrBest = 1,
            /// <summary>
            /// Performs Frequency Selective Reconstruction (FSR). Faster inpainting. 
            /// </summary>
            FsrFast = 2
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
        /// <param name="transformType">Type of the orthogonal transform used in collaborative filtering step. Currently only Haar transform is supported.</param>
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

        /// <summary>
        /// Oil Painting effect
        /// </summary>
        /// <param name="src">Input three-channel or one channel image (either CV_8UC3 or CV_8UC1)</param>
        /// <param name="dst">Output image of the same size and type as src.</param>
        /// <param name="size">Neighbouring size is 2-size+1</param>
        /// <param name="dynRatio">Image is divided by dynRatio before histogram processing</param>
        /// <param name="code">Color space conversion code(see ColorConversionCodes). Histogram will used only first plane</param>
        public static void OilPainting(IInputArray src, IOutputArray dst, int size, int dynRatio, CvEnum.ColorConversion code = ColorConversion.Bgr2Gray)
        {
            using (InputArray iaSrc = src.GetInputArray())
            using (OutputArray oaDst = dst.GetOutputArray())
            {
                cveOilPainting(iaSrc, oaDst, size, dynRatio, code);
            }
        }

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        private static extern void cveOilPainting(
            IntPtr src,
            IntPtr dst,
            int size,
            int dynRatio,
            CvEnum.ColorConversion code);
    }
}
