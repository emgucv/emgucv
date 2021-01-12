//----------------------------------------------------------------------------
//  Copyright (C) 2004-2021 by EMGU Corporation. All rights reserved.       
//----------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Emgu.CV.Features2D;
using Emgu.CV.Structure;
using System.Drawing;
using Emgu.CV.Util;
using Emgu.Util;

namespace Emgu.CV
{
    public partial class CvInvoke
    {
        /// <summary>
        /// The grab cut algorithm for segmentation
        /// </summary>
        /// <param name="img">The 8-bit 3-channel image to be segmented</param>
        /// <param name="mask">Input/output 8-bit single-channel mask. The mask is initialized by the function
        /// when mode is set to GC_INIT_WITH_RECT. Its elements may have one of following values:
        /// 0 (GC_BGD) defines an obvious background pixels.
        /// 1 (GC_FGD) defines an obvious foreground (object) pixel.
        /// 2 (GC_PR_BGR) defines a possible background pixel.
        /// 3 (GC_PR_FGD) defines a possible foreground pixel.
        ///</param>
        /// <param name="rect">The rectangle to initialize the segmentation</param>
        /// <param name="bgdModel">
        /// Temporary array for the background model. Do not modify it while you are
        /// processing the same image.
        /// </param>
        /// <param name="fgdModel">
        /// Temporary arrays for the foreground model. Do not modify it while you are
        /// processing the same image.
        /// </param>
        /// <param name="iterCount">The number of iterations</param>
        /// <param name="type">The initialization type</param>
        public static void GrabCut(
           IInputArray img,
           IInputOutputArray mask,
           Rectangle rect,
           IInputOutputArray bgdModel,
           IInputOutputArray fgdModel,
           int iterCount,
           CvEnum.GrabcutInitType type)
        {
            using (InputArray iaImg = img.GetInputArray())
            using (InputOutputArray ioaMask = mask == null ? InputOutputArray.GetEmpty() : mask.GetInputOutputArray())
            using (InputOutputArray ioaBgdModel = bgdModel.GetInputOutputArray())
            using (InputOutputArray ioaFgdModel = fgdModel.GetInputOutputArray())
                cveGrabCut(iaImg, ioaMask, ref rect, ioaBgdModel, ioaFgdModel, iterCount, type);
        }

        [DllImport(ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        private static extern void cveGrabCut(
           IntPtr img,
           IntPtr mask,
           ref Rectangle rect,
           IntPtr bgdModel,
           IntPtr fgdModel,
           int iterCount,
           CvEnum.GrabcutInitType type);

        /// <summary>
        /// Calculate square root of each source array element. in the case of multichannel
        /// arrays each channel is processed independently. The function accuracy is approximately
        /// the same as of the built-in std::sqrt.
        /// </summary>
        /// <param name="src">The source floating-point array</param>
        /// <param name="dst">The destination array; will have the same size and the same type as src</param>
        public static void Sqrt(IInputArray src, IOutputArray dst)
        {
            using (InputArray iaSrc = src.GetInputArray())
            using (OutputArray oaDst = dst.GetOutputArray())
                cveSqrt(iaSrc, oaDst);
        }

        [DllImport(ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        private static extern void cveSqrt(IntPtr src, IntPtr dst);

        /// <summary>
        /// Applies a GNU Octave/MATLAB equivalent colormap on a given image.
        /// </summary>
        /// <param name="src"> The source image, grayscale or colored of type CV_8UC1 or CV_8UC3</param>
        /// <param name="dst"> The result is the colormapped source image</param>
        /// <param name="colorMapType">The type of color map</param>
        public static void ApplyColorMap(IInputArray src, IOutputArray dst, CvEnum.ColorMapType colorMapType)
        {
            using (InputArray iaSrc = src.GetInputArray())
            using (OutputArray oaDst = dst.GetOutputArray())
                cveApplyColorMap1(iaSrc, oaDst, colorMapType);
        }

        [DllImport(ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        private static extern void cveApplyColorMap1(IntPtr src, IntPtr dst, CvEnum.ColorMapType colorMapType);

        /// <summary>
        /// Applies a user colormap on a given image.
        /// </summary>
        /// <param name="src"> The source image, grayscale or colored of type CV_8UC1 or CV_8UC3. </param>
        /// <param name="dst"> The result is the colormapped source image. </param>
        /// <param name="userColorMap">The colormap to apply of type CV_8UC1 or CV_8UC3 and size 256</param>
        public static void ApplyColorMap(IInputArray src, IOutputArray dst, IInputArray userColorMap)
        {
            using (InputArray iaSrc = src.GetInputArray())
            using (OutputArray oaDst = dst.GetOutputArray())
            using (InputArray iaUserColorMap = userColorMap.GetInputArray())
                cveApplyColorMap2(iaSrc, oaDst, iaUserColorMap);
        }

        [DllImport(ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        private static extern void cveApplyColorMap2(IntPtr src, IntPtr dst, IntPtr userColorMap);

        /// <summary>
        /// Check that every array element is neither NaN nor +- inf. The functions also check that each value
        /// is between minVal and maxVal. in the case of multi-channel arrays each channel is processed
        /// independently. If some values are out of range, position of the first outlier is stored in pos, 
        /// and then the functions either return false (when quiet=true) or throw an exception.
        /// </summary>
        /// <param name="arr">The array to check</param>
        /// <param name="quiet">The flag indicating whether the functions quietly return false when the array elements are
        /// out of range, or they throw an exception</param>
        /// <param name="pos">This will be filled with the position of the first outlier</param>
        /// <param name="minVal">The inclusive lower boundary of valid values range</param>
        /// <param name="maxVal">The exclusive upper boundary of valid values range</param>
        /// <returns>If quiet, return true if all values are in range</returns>
        public static bool CheckRange(
           IInputArray arr,
           bool quiet,
           ref Point pos,
           double minVal,
           double maxVal)
        {
            using (InputArray iaArr = arr.GetInputArray())
                return cveCheckRange(iaArr, quiet, ref pos, minVal, maxVal);
        }

        [DllImport(ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        [return: MarshalAs(CvInvoke.BoolMarshalType)]
        private static extern bool cveCheckRange(
           IntPtr arr,
           [MarshalAs(CvInvoke.BoolMarshalType)] bool quiet,
           ref Point pos,
           double minVal,
           double maxVal);

        /// <summary>
        /// Converts NaN's to the given number
        /// </summary>
        /// <param name="a">The array where NaN needs to be converted</param>
        /// <param name="val">The value to convert to</param>
        public static void PatchNaNs(IInputOutputArray a, double val = 0)
        {
            using (InputOutputArray ioaA = a.GetInputOutputArray())
                cvePatchNaNs(ioaA, val);
        }
        [DllImport(ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        private static extern void cvePatchNaNs(IntPtr a, double val);
        /// <summary>
        /// Computes an optimal affine transformation between two 3D point sets.
        /// </summary>
        /// <param name="src">First input 3D point set.</param>
        /// <param name="dst">Second input 3D point set.</param>
        /// <param name="estimate">Output 3D affine transformation matrix.</param>
        /// <param name="inliers">Output vector indicating which points are inliers.</param>
        /// <param name="ransacThreshold">Maximum reprojection error in the RANSAC algorithm to consider a point as an inlier.</param>
        /// <param name="confidence">Confidence level, between 0 and 1, for the estimated transformation. Anything between 0.95 and 0.99 is usually good enough. Values too close to 1 can slow down the estimation significantly. Values lower than 0.8-0.9 can result in an incorrectly estimated transformation.</param>
        /// <returns>The result</returns>
        public static int EstimateAffine3D(MCvPoint3D32f[] src, MCvPoint3D32f[] dst, out Matrix<double> estimate,
           out Byte[] inliers, double ransacThreshold, double confidence)
        {
            GCHandle srcHandle = GCHandle.Alloc(src, GCHandleType.Pinned);
            GCHandle dstHandle = GCHandle.Alloc(dst, GCHandleType.Pinned);
            int result;

            estimate = new Matrix<double>(3, 4);

            int sizeOfPoint3D32f = Toolbox.SizeOf<MCvPoint3D64f>();

            using (
               Matrix<float> srcMat = new Matrix<float>(1, src.Length, 3, srcHandle.AddrOfPinnedObject(),
                  sizeOfPoint3D32f * src.Length))
            using (
               Matrix<float> dstMat = new Matrix<float>(1, dst.Length, 3, dstHandle.AddrOfPinnedObject(),
                  sizeOfPoint3D32f * dst.Length))
            using (Util.VectorOfByte vectorOfByte = new Util.VectorOfByte())
            {
                result = EstimateAffine3D(srcMat, dstMat, estimate, vectorOfByte, ransacThreshold, confidence);
                inliers = vectorOfByte.ToArray();
            }

            srcHandle.Free();
            dstHandle.Free();

            return result;
        }

        /// <summary>
        /// Computes an optimal affine transformation between two 3D point sets.
        /// </summary>
        /// <param name="src"> First input 3D point set.</param>
        /// <param name="dst">Second input 3D point set.</param>
        /// <param name="affineEstimate">Output 3D affine transformation matrix 3 x 4</param>
        /// <param name="inliers"> Output vector indicating which points are inliers.</param>
        /// <param name="ransacThreshold">Maximum reprojection error in the RANSAC algorithm to consider a point as an inlier.</param>
        /// <param name="confidence">Confidence level, between 0 and 1, for the estimated transformation. Anything between 0.95 and 0.99 is usually good enough. Values too close to 1 can slow down the estimation significantly. Values lower than 0.8-0.9 can result in an incorrectly estimated transformation.</param>
        /// <returns>the result</returns>
        public static int EstimateAffine3D(IInputArray src, IInputArray dst, IOutputArray affineEstimate,
           IOutputArray inliers, double ransacThreshold = 3, double confidence = 0.99)
        {
            using (InputArray iaSrc = src.GetInputArray())
            using (InputArray iaDst = dst.GetInputArray())
            using (OutputArray oaAffineEstimate = affineEstimate.GetOutputArray())
            using (OutputArray oaInliners = inliers.GetOutputArray())
                return cveEstimateAffine3D(iaSrc, iaDst, oaAffineEstimate, oaInliners, ransacThreshold, confidence);
        }

        [DllImport(ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern int cveEstimateAffine3D(IntPtr src, IntPtr dst, IntPtr affineEstimate, IntPtr inliers,
           double ransacThreshold, double confidence);


        /// <summary>
        /// Finds the global minimum and maximum in an array
        /// </summary>
        /// <param name="src">Input single-channel array.</param>
        /// <param name="minVal">The returned minimum value</param>
        /// <param name="maxVal">The returned maximum value</param>
        /// <param name="minIdx">The returned minimum location</param>
        /// <param name="maxIdx">The returned maximum location</param>
        /// <param name="mask">The extremums are searched across the whole array if mask is IntPtr.Zert. Otherwise, search is performed in the specified array region.</param>
        public static void MinMaxIdx(IInputArray src, out double minVal, out double maxVal, int[] minIdx, int[] maxIdx,
           IInputArray mask = null)
        {
            GCHandle minHandle = GCHandle.Alloc(minIdx, GCHandleType.Pinned);
            GCHandle maxHandle = GCHandle.Alloc(maxIdx, GCHandleType.Pinned);
            minVal = 0;
            maxVal = 0;
            using (InputArray iaSrc = src.GetInputArray())
            using (InputArray iaMask = mask == null ? InputArray.GetEmpty() : mask.GetInputArray())
                cveMinMaxIdx(iaSrc, ref minVal, ref maxVal, minHandle.AddrOfPinnedObject(), maxHandle.AddrOfPinnedObject(),
                   iaMask);
            minHandle.Free();
            maxHandle.Free();
        }

        [DllImport(ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        private static extern void cveMinMaxIdx(IntPtr src, ref double minVal, ref double maxVal, IntPtr minIdx,
           IntPtr maxIdx, IntPtr mask);

        /// <summary>
        /// Applies arbitrary linear filter to the image. In-place operation is supported. When the aperture is partially outside the image, the function interpolates outlier pixel values from the nearest pixels that is inside the image
        /// </summary>
        /// <param name="src">The source image</param>
        /// <param name="dst">The destination image</param>
        /// <param name="kernel">Convolution kernel, single-channel floating point matrix. If you want to apply different kernels to different channels, split the image using cvSplit into separate color planes and process them individually</param>
        /// <param name="anchor">The anchor of the kernel that indicates the relative position of a filtered point within the kernel. The anchor shoud lie within the kernel. The special default value (-1,-1) means that it is at the kernel center</param>
        /// <param name="delta">The optional value added to the filtered pixels before storing them in dst</param>
        /// <param name="borderType">The pixel extrapolation method.</param>
        public static void Filter2D(IInputArray src, IOutputArray dst, IInputArray kernel, Point anchor, double delta = 0,
           Emgu.CV.CvEnum.BorderType borderType = CvEnum.BorderType.Default)
        {
            using (InputArray iaSrc = src.GetInputArray())
            using (OutputArray oaDst = dst.GetOutputArray())
            using (InputArray iaKernel = kernel.GetInputArray())
                cveFilter2D(iaSrc, oaDst, iaKernel, ref anchor, delta, borderType);
        }

        [DllImport(ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        private static extern void cveFilter2D(IntPtr src, IntPtr dst, IntPtr kernel, ref Point anchor, double delta,
           Emgu.CV.CvEnum.BorderType borderType);

        /// <summary>
        /// The function applies a separable linear filter to the image. That is, first, every row of src is filtered with the 1D kernel kernelX. Then, every column of the result is filtered with the 1D kernel kernelY. The final result shifted by delta is stored in dst .
        /// </summary>
        /// <param name="src">Source image.</param>
        /// <param name="dst">Destination image of the same size and the same number of channels as src.</param>
        /// <param name="ddepth">Destination image depth</param>
        /// <param name="kernelX">Coefficients for filtering each row.</param>
        /// <param name="kernelY">Coefficients for filtering each column.</param>
        /// <param name="anchor">Anchor position within the kernel. The value (-1,-1) means that the anchor is at the kernel center.</param>
        /// <param name="delta">Value added to the filtered results before storing them.</param>
        /// <param name="borderType">Pixel extrapolation method</param>
        public static void SepFilter2D(IInputArray src, IOutputArray dst, CvEnum.DepthType ddepth, IInputArray kernelX, IInputArray kernelY, Point anchor, double delta = 0, CvEnum.BorderType borderType = CvEnum.BorderType.Default)
        {
            using (InputArray iaSrc = src.GetInputArray())
            using (OutputArray oaDst = dst.GetOutputArray())
            using (InputArray iaKernelX = kernelX.GetInputArray())
            using (InputArray iaKernelY = kernelY.GetInputArray())
                cveSepFilter2D(iaSrc, oaDst, ddepth, iaKernelX, iaKernelY, ref anchor, delta, borderType);
        }
        [DllImport(ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        private static extern void cveSepFilter2D(IntPtr src, IntPtr dst, CvEnum.DepthType ddepth, IntPtr kernelX, IntPtr kernelY, ref Point anchor, double delta, CvEnum.BorderType borderType);

        /// <summary>
        /// Performs linear blending of two images:
        /// dst(i, j)=weights1(i, j) x src1(i, j) + weights2(i, j) x src2(i, j)
        /// </summary>
        /// <param name="src1">It has a type of CV_8UC(n) or CV_32FC(n), where n is a positive integer.</param>
        /// <param name="src2">It has the same type and size as src1.</param>
        /// <param name="weights1">It has a type of CV_32FC1 and the same size with src1.</param>
        /// <param name="weights2">It has a type of CV_32FC1 and the same size with src1.</param>
        /// <param name="dst">It is created if it does not have the same size and type with src1.</param>
        public static void BlendLinear(IInputArray src1, IInputArray src2, IInputArray weights1, IInputArray weights2, IOutputArray dst)
        {
            using (InputArray iaSrc1 = src1.GetInputArray())
            using (InputArray iaSrc2 = src2.GetInputArray())
            using (InputArray iaWeights1 = weights1.GetInputArray())
            using (InputArray iaWeights2 = weights2.GetInputArray())
            using (OutputArray oaDst = dst.GetOutputArray())
                cveBlendLinear(iaSrc1, iaSrc2, iaWeights1, iaWeights2, oaDst);
        }
        [DllImport(ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        private static extern void cveBlendLinear(IntPtr src1, IntPtr src2, IntPtr weights1, IntPtr weights2, IntPtr dst);


        /// <summary>
        /// Contrast Limited Adaptive Histogram Equalization (CLAHE)
        /// </summary>
        /// <param name="src">The source image</param>
        /// <param name="clipLimit">Clip Limit, use 40 for default</param>
        /// <param name="tileGridSize">Tile grid size, use (8, 8) for default</param>
        /// <param name="dst">The destination image</param>
        public static void CLAHE(IInputArray src, double clipLimit, Size tileGridSize, IOutputArray dst)
        {
            using (InputArray iaSrc = src.GetInputArray())
            using (OutputArray oaDst = dst.GetOutputArray())
                cveCLAHE(iaSrc, clipLimit, ref tileGridSize, oaDst);
        }

        [DllImport(ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        private static extern void cveCLAHE(IntPtr srcArr, double clipLimit, ref Size tileGridSize, IntPtr dstArr);


        /// <summary>
        /// This function retrieve the Open CV structure sizes in unmanaged code
        /// </summary>
        /// <returns>The structure that will hold the Open CV structure sizes</returns>
        public static CvStructSizes GetCvStructSizes()
        {
            CvStructSizes sizes = new CvStructSizes();
            cveGetCvStructSizes(ref sizes);
            return sizes;
        }

        [DllImport(ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        private static extern void cveGetCvStructSizes(ref CvStructSizes sizes);

        /// <summary>
        /// Get the dictionary that hold the Open CV build flags. The key is a String and the value is type double. If it is a flag, 0 means false and 1 means true 
        /// </summary>
        /// <returns></returns>
        public static Dictionary<String, double> ConfigDict
        {
            get
            {
                using (VectorOfCvString vs = new VectorOfCvString())
                using (VectorOfDouble vd = new VectorOfDouble())
                {
                    cveGetConfigDict(vs, vd);

                    String[] keys = vs.ToArray();
                    double[] values = vd.ToArray();

                    Dictionary<String, double> dict = new Dictionary<string, double>();
                    for (int i = 0; i < keys.Length; i++)
                    {
                        dict[keys[i]] = values[i];
                    }

                    return dict;
                }
                
            }
            
        }

        [DllImport(ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        private static extern void cveGetConfigDict(IntPtr names, IntPtr values);

        /*
        public static void TestDrawLine(IntPtr img, int startX, int startY, int endX, int endY, MCvScalar color)
        {
           TestDrawLine(img, startX, startY, endX, endY, color.v0, color.v1, color.v2, color.v3);
        }

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention, EntryPoint="testDrawLine")]
        private static extern void TestDrawLine(IntPtr img, int startX, int startY, int endX, int endY, double v0, double v1, double v2, double v3);

        /// <summary>
        /// Implements the chamfer matching algorithm on images taking into account both distance from
        /// the template pixels to the nearest pixels and orientation alignment between template and image
        /// contours.
        /// </summary>
        /// <param name="img">The edge image where search is performed</param>
        /// <param name="templ">The template (an edge image)</param>
        /// <param name="contours">The output contours</param>
        /// <param name="cost">The cost associated with the matching</param>
        /// <param name="templScale">The template scale</param>
        /// <param name="maxMatches">The maximum number of matches</param>
        /// <param name="minMatchDistance">The minimum match distance</param>
        /// <param name="padX">PadX</param>
        /// <param name="padY">PadY</param>
        /// <param name="scales">Scales</param>
        /// <param name="minScale">Minimum scale</param>
        /// <param name="maxScale">Maximum scale</param>
        /// <param name="orientationWeight">Orientation weight</param>
        /// <param name="truncate">Truncate</param>
        /// <returns>The number of matches</returns>
        public static int ChamferMatching(Mat img, Mat templ,
           out Point[][] contours, out float[] cost,
           double templScale = 1, int maxMatches = 20,
           double minMatchDistance = 1.0, int padX = 3,
           int padY = 3, int scales = 5, double minScale = 0.6, double maxScale = 1.6,
           double orientationWeight = 0.5, double truncate = 20)
        {
           using (Emgu.CV.Util.VectorOfVectorOfPoint vecOfVecOfPoint = new Util.VectorOfVectorOfPoint())
           using (Emgu.CV.Util.VectorOfFloat vecOfFloat = new Util.VectorOfFloat())
           {
              int count = cveChamferMatching(img, templ, vecOfVecOfPoint, vecOfFloat, templScale, maxMatches, minMatchDistance, padX, padY, scales, minScale, maxScale, orientationWeight, truncate);
              contours = vecOfVecOfPoint.ToArrayOfArray();
              cost = vecOfFloat.ToArray();
              return count;
           }
        }
        [DllImport(ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        private static extern int cveChamferMatching(
           IntPtr img, IntPtr templ,
           IntPtr results, IntPtr cost,
           double templScale, int maxMatches,
           double minMatchDistance, int padX,
           int padY, int scales, double minScale, double maxScale,
           double orientationWeight, double truncate);
        */

        /// <summary>
        /// Finds centers in the grid of circles
        /// </summary>
        /// <param name="image">Source chessboard view</param>
        /// <param name="patternSize">The number of inner circle per chessboard row and column</param>
        /// <param name="flags">Various operation flags</param>
        /// <param name="featureDetector">The feature detector. Use a SimpleBlobDetector for default</param>
        /// <returns>The center of circles detected if the chess board pattern is found, otherwise null is returned</returns>
        public static PointF[] FindCirclesGrid(Image<Gray, Byte> image, Size patternSize, CvEnum.CalibCgType flags, Feature2D featureDetector)
        {
            using (Util.VectorOfPointF vec = new Util.VectorOfPointF())
            {
                bool patternFound =
                   FindCirclesGrid(
                      image,
                      patternSize,
                      vec,
                      flags,
                      featureDetector
                      );
                return patternFound ? vec.ToArray() : null;
            }
        }

        /// <summary>
        /// Finds centers in the grid of circles
        /// </summary>
        /// <param name="image">Source chessboard view</param>
        /// <param name="patternSize">The number of inner circle per chessboard row and column</param>
        /// <param name="flags">Various operation flags</param>
        /// <param name="featureDetector">The feature detector. Use a SimpleBlobDetector for default</param>
        /// <param name="centers">output array of detected centers.</param>
        /// <returns>True if grid found.</returns>
        public static bool FindCirclesGrid(IInputArray image, Size patternSize, IOutputArray centers, CvEnum.CalibCgType flags, Feature2D featureDetector)
        {
            using (InputArray iaImage = image.GetInputArray())
            using (OutputArray oaCenters = centers.GetOutputArray())
                return cveFindCirclesGrid(iaImage, ref patternSize, oaCenters, flags, featureDetector.Feature2DPtr);
        }
        [DllImport(ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        [return: MarshalAs(CvInvoke.BoolMarshalType)]
        private static extern bool cveFindCirclesGrid(IntPtr image, ref Size patternSize, IntPtr centers, CvEnum.CalibCgType flags, IntPtr blobDetector);

        /*
        /// <summary>
        /// Applies the adaptive bilateral filter to an image.
        /// </summary>
        /// <param name="src">The source image</param>
        /// <param name="dst">The destination image; will have the same size and the same type as src</param>
        /// <param name="ksize">The kernel size. This is the neighborhood where the local variance will be calculated, and where pixels will contribute (in a weighted manner).</param>
        /// <param name="sigmaSpace">Filter sigma in the coordinate space. Larger value of the parameter means that farther pixels will influence each other (as long as their colors are close enough; see sigmaColor). Then d>0, it specifies the neighborhood size regardless of sigmaSpace, otherwise d is proportional to sigmaSpace.</param>
        /// <param name="maxSigmaColor">Maximum allowed sigma color (will clamp the value calculated in the ksize neighborhood. Larger value of the parameter means that more dissimilar pixels will influence each other (as long as their colors are close enough; see sigmaColor). Then d>0, it specifies the neighborhood size regardless of sigmaSpace, otherwise d is proportional to sigmaSpace. Use 20 for default.</param>
        /// <param name="anchor">Use (-1, -1) for default</param>
        /// <param name="borderType">Pixel extrapolation method.</param>
        public static void AdaptiveBilateralFilter(IInputArray src, IOutputArray dst, Size ksize, double sigmaSpace, double maxSigmaColor, Point anchor, CvEnum.BorderType borderType = CvEnum.BorderType.Default)
        {
           cveAdaptiveBilateralFilter(src.InputArrayPtr, dst.OutputArrayPtr, ref ksize, sigmaSpace, maxSigmaColor, ref anchor, borderType);
        }
        [DllImport(ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        private static extern void cveAdaptiveBilateralFilter(IntPtr src, IntPtr dst, ref Size ksize, double sigmaSpace, double maxSigmaColor, ref Point anchor, CvEnum.BorderType borderType);
        */
    }
}
