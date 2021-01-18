//----------------------------------------------------------------------------
//  Copyright (C) 2004-2021 by EMGU Corporation. All rights reserved.       
//----------------------------------------------------------------------------

using System;
using System.Runtime.InteropServices;
using System.Drawing;
using Emgu.CV.CvEnum;

namespace Emgu.CV.XImgproc
{
    /// <summary>
    /// Extended Image Processing
    /// </summary>
    public static partial class XImgprocInvoke
    {
        static XImgprocInvoke()
        {
            CvInvoke.Init();
        }

        /// <summary>
        /// Applies the joint bilateral filter to an image.
        /// </summary>
        /// <param name="joint">Joint 8-bit or floating-point, 1-channel or 3-channel image.</param>
        /// <param name="src">Source 8-bit or floating-point, 1-channel or 3-channel image with the same depth as joint image.</param>
        /// <param name="dst">Destination image of the same size and type as src .</param>
        /// <param name="d">Diameter of each pixel neighborhood that is used during filtering. If it is non-positive, it is computed from sigmaSpace .</param>
        /// <param name="sigmaColor">Filter sigma in the color space. A larger value of the parameter means that farther colors within the pixel neighborhood (see sigmaSpace ) will be mixed together, resulting in larger areas of semi-equal color.</param>
        /// <param name="sigmaSpace">Filter sigma in the coordinate space. A larger value of the parameter means that farther pixels will influence each other as long as their colors are close enough (see sigmaColor ). When d&gt;0 , it specifies the neighborhood size regardless of sigmaSpace . Otherwise, d is proportional to sigmaSpace .</param>
        /// <param name="borderType">Border type</param>
        public static void JointBilateralFilter(
           IInputArray joint, IInputArray src, IOutputArray dst, int d,
           double sigmaColor, double sigmaSpace, CvEnum.BorderType borderType = BorderType.Reflect101)
        {
            using (InputArray iaJoint = joint.GetInputArray())
            using (InputArray iaSrc = src.GetInputArray())
            using (OutputArray oaDst = dst.GetOutputArray())
                cveJointBilateralFilter(iaJoint, iaSrc,
                   oaDst, d, sigmaColor, sigmaSpace, borderType);
        }
        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        private static extern void cveJointBilateralFilter(IntPtr joint, IntPtr src, IntPtr dst, int d, double sigmaColor, double sigmaSpace, CvEnum.BorderType borderType);

        /// <summary>
        /// Applies the bilateral texture filter to an image. It performs structure-preserving texture filter. 
        /// </summary>
        /// <param name="src">Source image whose depth is 8-bit UINT or 32-bit FLOAT</param>
        /// <param name="dst">Destination image of the same size and type as src.</param>
        /// <param name="fr">Radius of kernel to be used for filtering. It should be positive integer</param>
        /// <param name="numIter">Number of iterations of algorithm, It should be positive integer</param>
        /// <param name="sigmaAlpha">Controls the sharpness of the weight transition from edges to smooth/texture regions, where a bigger value means sharper transition. When the value is negative, it is automatically calculated.</param>
        /// <param name="sigmaAvg">Range blur parameter for texture blurring. Larger value makes result to be more blurred. When the value is negative, it is automatically calculated as described in the paper.</param>
        /// <remarks>For more details about this filter see: Hojin Cho, Hyunjoon Lee, Henry Kang, and Seungyong Lee. Bilateral texture filtering. ACM Transactions on Graphics, 33(4):128:1–128:8, July 2014.</remarks>
        public static void BilateralTextureFilter(
            IInputArray src, IOutputArray dst, 
            int fr = 3, int numIter = 1,
            double sigmaAlpha = -1.0, double sigmaAvg = -1.0)
        {
            using (InputArray iaSrc = src.GetInputArray())
            using (OutputArray oaDst = dst.GetOutputArray())
            {
                cveBilateralTextureFilter(iaSrc, oaDst, fr, numIter, sigmaAlpha, sigmaAvg);
            }
        }
        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        private static extern void cveBilateralTextureFilter(IntPtr src, IntPtr dst, int fr, int numIter, double sigmaAlpha, double sigmaAvg);

        /// <summary>
        /// Applies the rolling guidance filter to an image
        /// </summary>
        /// <param name="src">Source 8-bit or floating-point, 1-channel or 3-channel image.</param>
        /// <param name="dst">Destination image of the same size and type as src.</param>
        /// <param name="d">Diameter of each pixel neighborhood that is used during filtering. If it is non-positive, it is computed from sigmaSpace .</param>
        /// <param name="sigmaColor">Filter sigma in the color space. A larger value of the parameter means that farther colors within the pixel neighborhood (see sigmaSpace ) will be mixed together, resulting in larger areas of semi-equal color.</param>
        /// <param name="sigmaSpace">Filter sigma in the coordinate space. A larger value of the parameter means that farther pixels will influence each other as long as their colors are close enough (see sigmaColor ). When d>0 , it specifies the neighborhood size regardless of sigmaSpace . Otherwise, d is proportional to sigmaSpace .</param>
        /// <param name="numOfIter">Number of iterations of joint edge-preserving filtering applied on the source image.</param>
        /// <param name="borderType">Border type</param>
        public static void RollingGuidanceFilter(
            IInputArray src, IOutputArray dst, int d = -1, double sigmaColor = 25,
            double sigmaSpace = 3, int numOfIter = 4, CvEnum.BorderType borderType = BorderType.Default)
        {
            using (InputArray iaSrc = src.GetInputArray())
            using (OutputArray oaDst = dst.GetOutputArray())
            {
                cveRollingGuidanceFilter(iaSrc, oaDst, d, sigmaColor, sigmaSpace, numOfIter, borderType);
            }
        }

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        private static extern void cveRollingGuidanceFilter(IntPtr src, IntPtr dst, int d, double sigmaColor, double sigmaSpace, int numOfIter, CvEnum.BorderType borderType);

        /// <summary>
        /// Simple one-line Fast Global Smoother filter call.
        /// </summary>
        /// <param name="guide">image serving as guide for filtering. It should have 8-bit depth and either 1 or 3 channels.</param>
        /// <param name="src">source image for filtering with unsigned 8-bit or signed 16-bit or floating-point 32-bit depth and up to 4 channels.</param>
        /// <param name="dst">destination image.</param>
        /// <param name="lambda">parameter defining the amount of regularization</param>
        /// <param name="sigmaColor">parameter, that is similar to color space sigma in bilateralFilter.</param>
        /// <param name="lambdaAttenuation">internal parameter, defining how much lambda decreases after each iteration. Normally, it should be 0.25. Setting it to 1.0 may lead to streaking artifacts.</param>
        /// <param name="numIter">number of iterations used for filtering, 3 is usually enough.</param>
        public static void FastGlobalSmootherFilter(IInputArray guide, IInputArray src, IOutputArray dst, double lambda,
         double sigmaColor, double lambdaAttenuation = 0.25, int numIter = 3)
        {
            using (InputArray iaGuide = guide.GetInputArray())
            using (InputArray iaSrc = src.GetInputArray())
            using (OutputArray oaDst = dst.GetOutputArray())
            {
                cveFastGlobalSmootherFilter(iaGuide, iaSrc, oaDst, lambda, sigmaColor, lambdaAttenuation, numIter);
            }
        }
        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        private static extern void cveFastGlobalSmootherFilter(IntPtr guide, IntPtr src, IntPtr dst, double lambda, double sigmaColor, double lambdaAttenuation, int numIter);

        /// <summary>
        /// Global image smoothing via L0 gradient minimization.
        /// </summary>
        /// <param name="src">Source image for filtering with unsigned 8-bit or signed 16-bit or floating-point depth.</param>
        /// <param name="dst">Destination image.</param>
        /// <param name="lambda">Parameter defining the smooth term weight.</param>
        /// <param name="kappa">Parameter defining the increasing factor of the weight of the gradient data term.</param>
        public static void L0Smooth(IInputArray src, IOutputArray dst, double lambda = 0.02, double kappa = 2)
        {
            using (InputArray iaSrc = src.GetInputArray())
            using (OutputArray oaDst = dst.GetOutputArray())
            {
                cveL0Smooth(iaSrc, oaDst, lambda, kappa);
            }
        }
        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        private static extern void cveL0Smooth(IntPtr src, IntPtr dst, double lambda, double kappa);

        /// <summary>
        /// Simple one-line Adaptive Manifold Filter call.
        /// </summary>
        /// <param name="joint">joint (also called as guided) image or array of images with any numbers of channels.</param>
        /// <param name="src">filtering image with any numbers of channels.</param>
        /// <param name="dst">output image.</param>
        /// <param name="sigmaS">spatial standard deviation.</param>
        /// <param name="sigmaR">color space standard deviation, it is similar to the sigma in the color space into bilateralFilter.</param>
        /// <param name="adjustOutliers">optional, specify perform outliers adjust operation or not, (Eq. 9) in the original paper.</param>
        public static void AmFilter(IInputArray joint, IInputArray src, IOutputArray dst, double sigmaS, double sigmaR,
           bool adjustOutliers = false)
        {
            using (InputArray iaJoint = joint.GetInputArray())
            using (InputArray iaSrc = src.GetInputArray())
            using (OutputArray oaDst = dst.GetOutputArray())
                cveAmFilter(iaJoint, iaSrc, oaDst, sigmaS, sigmaR, adjustOutliers);
        }
        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        private static extern void cveAmFilter(
           IntPtr joint,
           IntPtr src,
           IntPtr dst,
           double sigmaS,
           double sigmaR,
           [MarshalAs(CvInvoke.BoolMarshalType)]
         bool adjustOutliers);

        /// <summary>
        /// Simple one-line Guided Filter call.
        /// </summary>
        /// <param name="guide">guided image (or array of images) with up to 3 channels, if it have more then 3 channels then only first 3 channels will be used.</param>
        /// <param name="src">filtering image with any numbers of channels.</param>
        /// <param name="dst">output image.</param>
        /// <param name="radius">radius of Guided Filter.</param>
        /// <param name="eps">regularization term of Guided Filter. eps^2 is similar to the sigma in the color space into bilateralFilter.</param>
        /// <param name="dDepth">optional depth of the output image.</param>
        public static void GuidedFilter(IInputArray guide, IInputArray src, IOutputArray dst, int radius, double eps,
           CvEnum.DepthType dDepth = DepthType.Default)
        {
            using (InputArray iaGuide = guide.GetInputArray())
            using (InputArray iaSrc = src.GetInputArray())
            using (OutputArray oaDst = dst.GetOutputArray())
                cveGuidedFilter(iaGuide, iaSrc, oaDst, radius, eps, dDepth);
        }
        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        private static extern void cveGuidedFilter(IntPtr guide, IntPtr src, IntPtr dst, int radius, double eps, DepthType dDepth);

        /// <summary>
        /// Simple one-line Domain Transform filter call. 
        /// </summary>
        /// <param name="guide">guided image (also called as joint image) with unsigned 8-bit or floating-point 32-bit depth and up to 4 channels.</param>
        /// <param name="src">filtering image with unsigned 8-bit or floating-point 32-bit depth and up to 4 channels.</param>
        /// <param name="dst">output image</param>
        /// <param name="sigmaSpatial">parameter in the original article, it's similar to the sigma in the coordinate space into bilateralFilter.</param>
        /// <param name="sigmaColor">parameter in the original article, it's similar to the sigma in the color space into bilateralFilter.</param>
        /// <param name="mode">Dt filter mode</param>
        /// <param name="numIters">optional number of iterations used for filtering, 3 is quite enough.</param>
        public static void DtFilter(IInputArray guide, IInputArray src, IOutputArray dst,
           double sigmaSpatial, double sigmaColor, DtFilterType mode, int numIters = 3)
        {
            using (InputArray iaGuide = guide.GetInputArray())
            using (InputArray iaSrc = src.GetInputArray())
            using (OutputArray oaDst = dst.GetOutputArray())
                cveDtFilter(iaGuide, iaSrc, oaDst, sigmaSpatial, sigmaColor, mode, numIters);
        }

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        private static extern void cveDtFilter(IntPtr guide, IntPtr src, IntPtr dst, double sigmaSpatial, double sigmaColor, DtFilterType mode, int numIters);

        /// <summary>
        /// Niblack threshold
        /// </summary>
        /// <param name="src">The source image</param>
        /// <param name="dst">The output result</param>
        /// <param name="type">Value that defines which local binarization algorithm should be used.</param>
        /// <param name="blockSize">Block size</param>
        /// <param name="delta">delta</param>
        /// <param name="maxValue">Maximum value to use with CV_THRESH_BINARY and CV_THRESH_BINARY_INV thresholding types</param>
        public static void NiBlackThreshold(IInputArray src, IOutputArray dst, double maxValue, LocalBinarizationMethods type, int blockSize,
           double delta)
        {
            using (InputArray iaSrc = src.GetInputArray())
            using (OutputArray oaDst = dst.GetOutputArray())
            {
                cveNiBlackThreshold(iaSrc, oaDst, maxValue, type, blockSize, delta);
            }
        }
        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        private static extern void cveNiBlackThreshold(IntPtr src, IntPtr dst, double maxValue, LocalBinarizationMethods type, int blockSize, double delta);

        /// <summary>
        /// Computes the estimated covariance matrix of an image using the sliding window forumlation.
        /// </summary>
        /// <param name="src">The source image. Input image must be of a complex type.</param>
        /// <param name="dst">The destination estimated covariance matrix. Output matrix will be size (windowRows*windowCols, windowRows*windowCols).</param>
        /// <param name="windowRows">The number of rows in the window.</param>
        /// <param name="windowCols">The number of cols in the window. The window size parameters control the accuracy of the estimation. The sliding window moves over the entire image from the top-left corner to the bottom right corner. Each location of the window represents a sample. If the window is the size of the image, then this gives the exact covariance matrix. For all other cases, the sizes of the window will impact the number of samples and the number of elements in the estimated covariance matrix.</param>
        public static void CovarianceEstimation(IInputArray src, IOutputArray dst, int windowRows, int windowCols)
        {
            using (InputArray iaSrc = src.GetInputArray())
            using (OutputArray oaDst = dst.GetOutputArray())
                cveCovarianceEstimation(iaSrc, oaDst, windowRows, windowCols);
        }
        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        private static extern void cveCovarianceEstimation(IntPtr src, IntPtr dst, int windowRows, int windowCols);


        /// <summary>
        /// Applies weighted median filter to an image.
        /// </summary>
        /// <param name="joint">Joint 8-bit, 1-channel or 3-channel image.</param>
        /// <param name="src">Source 8-bit or floating-point, 1-channel or 3-channel image.</param>
        /// <param name="dst">Destination image.</param>
        /// <param name="r">Radius of filtering kernel, should be a positive integer.</param>
        /// <param name="sigma">Filter range standard deviation for the joint image.</param>
        /// <param name="weightType">The type of weight definition</param>
        /// <param name="mask">A 0-1 mask that has the same size with I. This mask is used to ignore the effect of some pixels. If the pixel value on mask is 0, the pixel will be ignored when maintaining the joint-histogram. This is useful for applications like optical flow occlusion handling.</param>
        /// <remarks>For more details about this implementation, please see: Qi Zhang, Li Xu, and Jiaya Jia. 100+ times faster weighted median filter (wmf). In Computer Vision and Pattern Recognition (CVPR), 2014 IEEE Conference on, pages 2830–2837. IEEE, 2014.</remarks>
        public static void WeightedMedianFilter(IInputArray joint, IInputArray src, IOutputArray dst, int r,
            double sigma = 25.5, WMFWeightType weightType = WMFWeightType.Exp, Mat mask = null)
        {
            using (InputArray iaJoint = joint.GetInputArray())
            using (InputArray iaSrc = src.GetInputArray())
            using (OutputArray oaDst = dst.GetOutputArray())
            {

                cveWeightedMedianFilter(iaJoint, iaSrc, oaDst, r, sigma, weightType, mask == null ? IntPtr.Zero : mask);
            }
        }
        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        private static extern void cveWeightedMedianFilter(IntPtr joint, IntPtr src, IntPtr dst, int r, double sigma, WMFWeightType weightType, IntPtr mask);

        /// <summary>
        /// Applies Paillou filter to an image.
        /// </summary>
        /// <param name="op">Source 8-bit or 16bit image, 1-channel or 3-channel image.</param>
        /// <param name="dst">result CV_32F image with same number of channel than op.</param>
        /// <param name="alpha">see paper</param>
        /// <param name="omega">see paper</param>
        /// <remarks>For more details about this implementation, please see: Philippe Paillou. Detecting step edges in noisy sar images: a new linear operator. IEEE transactions on geoscience and remote sensing, 35(1):191–196, 1997.</remarks>
        public static void GradientPaillouY(IInputArray op, IOutputArray dst, double alpha, double omega)
        {
            using (InputArray iaOp = op.GetInputArray())
            using (OutputArray oaDst = dst.GetOutputArray())
                cveGradientPaillouY(iaOp, oaDst, alpha, omega);
        }
        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        private static extern void cveGradientPaillouY(IntPtr op, IntPtr dst, double alpha, double omega);

        /// <summary>
        /// Applies Paillou filter to an image.
        /// </summary>
        /// <param name="op">Source 8-bit or 16bit image, 1-channel or 3-channel image.</param>
        /// <param name="dst">result CV_32F image with same number of channel than op.</param>
        /// <param name="alpha">see paper</param>
        /// <param name="omega">see paper</param>
        /// <remarks>For more details about this implementation, please see: Philippe Paillou. Detecting step edges in noisy sar images: a new linear operator. IEEE transactions on geoscience and remote sensing, 35(1):191–196, 1997.</remarks>
        public static void GradientPaillouX(IInputArray op, IOutputArray dst, double alpha, double omega)
        {
            using (InputArray iaOp = op.GetInputArray())
            using (OutputArray oaDst = dst.GetOutputArray())
                cveGradientPaillouX(iaOp, oaDst, alpha, omega);
        }
        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        private static extern void cveGradientPaillouX(IntPtr op, IntPtr dst, double alpha, double omega);

        /// <summary>
        /// Applies Y Deriche filter to an image.
        /// </summary>
        /// <param name="op">Source 8-bit or 16bit image, 1-channel or 3-channel image.</param>
        /// <param name="dst">result CV_32FC image with same number of channel than _op.</param>
        /// <param name="alphaDerive">see paper</param>
        /// <param name="alphaMean">see paper</param>
        /// <remarks>For more details about this implementation, please see <see href="http://citeseerx.ist.psu.edu/viewdoc/download?doi=10.1.1.476.5736&amp;rep=rep1&amp;type=pdf">here</see> </remarks>
        public static void GradientDericheY(IInputArray op, IOutputArray dst, double alphaDerive, double alphaMean)
        {
            using (InputArray iaOp = op.GetInputArray())
            using (OutputArray oaDst = dst.GetOutputArray())
                cveGradientDericheY(iaOp, oaDst, alphaDerive, alphaMean);
        }
        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        private static extern void cveGradientDericheY(IntPtr op, IntPtr dst, double alphaDerive, double alphaMean);

        /// <summary>
        /// Applies X Deriche filter to an image.
        /// </summary>
        /// <param name="op">Source 8-bit or 16bit image, 1-channel or 3-channel image.</param>
        /// <param name="dst">result CV_32FC image with same number of channel than _op.</param>
        /// <param name="alphaDerive">see paper</param>
        /// <param name="alphaMean">see paper</param>
        /// <remarks>For more details about this implementation, please see http://citeseerx.ist.psu.edu/viewdoc/download?doi=10.1.1.476.5736&amp;rep=rep1&amp;type=pdf </remarks>
        public static void GradientDericheX(IInputArray op, IOutputArray dst, double alphaDerive, double alphaMean)
        {
            using (InputArray iaOp = op.GetInputArray())
            using (OutputArray oaDst = dst.GetOutputArray())
                cveGradientDericheX(iaOp, oaDst, alphaDerive, alphaMean);
        }
        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        private static extern void cveGradientDericheX(IntPtr op, IntPtr dst, double alphaDerive, double alphaMean);

        /// <summary>
        /// Applies a binary blob thinning operation, to achieve a skeletization of the input image. 
        /// The function transforms a binary blob image into a skeletized form using the technique of Zhang-Suen.
        /// </summary>
        /// <param name="src">Source 8-bit single-channel image, containing binary blobs, with blobs having 255 pixel values.</param>
        /// <param name="dst">Destination image of the same size and the same type as src. The function can work in-place.</param>
        /// <param name="thinningType">Value that defines which thinning algorithm should be used.</param>
        public static void Thinning(IInputArray src, IOutputArray dst, ThinningTypes thinningType)
        {
            using (InputArray iaSrc = src.GetInputArray())
            using (OutputArray oaDst = dst.GetOutputArray())
                cveThinning(iaSrc, oaDst, thinningType);
        }
        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        private static extern void cveThinning(IntPtr src, IntPtr dst, ThinningTypes thinningType);

        /// <summary>
        /// Performs anisotropic diffusion on an image.
        /// </summary>
        /// <param name="src">Grayscale Source image.</param>
        /// <param name="dst">Destination image of the same size and the same number of channels as src .</param>
        /// <param name="alpha">The amount of time to step forward by on each iteration (normally, it's between 0 and 1).</param>
        /// <param name="K">sensitivity to the edges</param>
        /// <param name="niters">The number of iterations</param>
        public static void AnisotropicDiffusion(IInputArray src, IOutputArray dst, float alpha, float K, int niters)
        {
            using (InputArray iaSrc = src.GetInputArray())
            using (OutputArray oaDst = dst.GetOutputArray())
                cveAnisotropicDiffusion(iaSrc, oaDst, alpha, K, niters);
        }

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        private static extern void cveAnisotropicDiffusion(IntPtr src, IntPtr dst, float alpha, float K, int niters);

        /// <summary>
        /// Brighten the edges of the image
        /// </summary>
        /// <param name="original">The source image</param>
        /// <param name="edgeview">The result</param>
        /// <param name="contrast">Contrast</param>
        /// <param name="shortrange">Short Range</param>
        /// <param name="longrange">Long Range</param>
        public static void BrightEdges(Mat original, Mat edgeview, int contrast, int shortrange, int longrange)
        {
            cveBrightEdges(original, edgeview, contrast, shortrange, longrange);
        }

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        private static extern void cveBrightEdges(IntPtr original, IntPtr edgeview, int contrast, int shortrange, int longrange);
    }
}
