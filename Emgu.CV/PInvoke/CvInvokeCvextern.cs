//----------------------------------------------------------------------------
//  Copyright (C) 2004-2013 by EMGU. All rights reserved.       
//----------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Emgu.CV.Structure;
using System.Drawing;

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
      /// <param name="iterCount">The number of iternations</param>
      /// <param name="type">The initilization type</param>
      [DllImport(EXTERN_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      public extern static void CvGrabCut(
         IntPtr img,
         IntPtr mask,
         ref Rectangle rect,
         IntPtr bgdModel,
         IntPtr fgdModel,
         int iterCount,
         CvEnum.GRABCUT_INIT_TYPE type);

      /// <summary>
      /// Calculate square root of each source array element. in the case of multichannel
      /// arrays each channel is processed independently. The function accuracy is approximately
      /// the same as of the built-in std::sqrt.
      /// </summary>
      /// <param name="src">The source floating-point array</param>
      /// <param name="dst">The destination array; will have the same size and the same type as src</param>
      [DllImport(EXTERN_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention, EntryPoint="cvArrSqrt")]
      public extern static void cvSqrt(IntPtr src, IntPtr dst);

      /// <summary>
      /// Apply color map to the image
      /// </summary>
      /// <param name="src">
      /// The source image.         
      /// This function expects Image&lt;Bgr, Byte&gt; or Image&lt;Gray, Byte&gt;. If the wrong image type is given, the original image
      /// will be returned.</param>
      /// <param name="dst">The destination image</param>
      /// <param name="colorMapType">The type of color map</param>
      [DllImport(EXTERN_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention, EntryPoint = "CvApplyColorMap")]
      public extern static void ApplyColorMap(IntPtr src, IntPtr dst, CvEnum.ColorMapType colorMapType);

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
      [DllImport(EXTERN_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      [return: MarshalAs(CvInvoke.BoolMarshalType)]
      internal extern static bool cvCheckRange(
         IntPtr arr,
         [MarshalAs(CvInvoke.BoolMarshalType)]
         bool quiet,
         ref Point pos,
         double minVal,
         double maxVal);

      [DllImport(CvInvoke.EXTERN_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      internal extern static void CvFeatureDetectorDetectKeyPoints(
         IntPtr detector,
         IntPtr image,
         IntPtr mask,
         IntPtr keypoints);

      [DllImport(EXTERN_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention, EntryPoint = "CvEstimateAffine3D")]
      internal extern static int  _CvEstimateAffine3D(IntPtr src, IntPtr dst, IntPtr affineEstimate, IntPtr inliers, double ransacThreshold, double confidence);

      /// <summary>
      /// Computes an optimal affine transformation between two 3D point sets.
      /// </summary>
      /// <param name="src">First input 3D point set.</param>
      /// <param name="dst">Second input 3D point set.</param>
      /// <param name="estimate">Output 3D affine transformation matrix.</param>
      /// <param name="inliers">Output vector indicating which points are inliers.</param>
      /// <param name="ransacThreshold">Maximum reprojection error in the RANSAC algorithm to consider a point as an inlier.</param>
      /// <param name="confidence">Confidence level, between 0 and 1, for the estimated transformation. Anything between 0.95 and 0.99 is usually good enough. Values too close to 1 can slow down the estimation significantly. Values lower than 0.8-0.9 can result in an incorrectly estimated transformation.</param>
      /// <returns></returns>
      public static int CvEstimateAffine3D(MCvPoint3D32f[] src, MCvPoint3D32f[] dst, out Matrix<double> estimate, out Byte[] inliers, double ransacThreshold, double confidence)
      {
         GCHandle srcHandle = GCHandle.Alloc(src, GCHandleType.Pinned);
         GCHandle dstHandle = GCHandle.Alloc(dst, GCHandleType.Pinned);
         int result;

         estimate = new Matrix<double>(3, 4);
         using (Util.Mat affineEstimate = new Util.Mat())
         using (Matrix<float> srcMat = new Matrix<float>(1,  src.Length, 3, srcHandle.AddrOfPinnedObject(), Marshal.SizeOf(typeof(MCvPoint3D32f)) * src.Length))
         using (Matrix<float> dstMat = new Matrix<float>(1,  dst.Length, 3, dstHandle.AddrOfPinnedObject(), Marshal.SizeOf(typeof(MCvPoint3D32f)) * dst.Length ))
         using (Util.VectorOfByte vectorOfByte = new Util.VectorOfByte())
         {
            result = _CvEstimateAffine3D(srcMat, dstMat, affineEstimate, vectorOfByte, ransacThreshold, confidence);
            inliers = vectorOfByte.ToArray();
            CvInvoke.cvMatCopyToCvArr(affineEstimate, estimate);  
         }

         srcHandle.Free();
         dstHandle.Free();

         return result;
      }

      [DllImport(EXTERN_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention, EntryPoint = "CvMinMaxIdx")]
      internal extern static void _CvMinMaxIdx(IntPtr src, ref double minVal, ref double maxVal, IntPtr minIdx, IntPtr maxIdx, IntPtr mask);

      /// <summary>
      /// Finds the global minimum and maximum in an array
      /// </summary>
      /// <param name="src">Input single-channel array.</param>
      /// <param name="minVal">The returned minimum value</param>
      /// <param name="maxVal">The returned maximum value</param>
      /// <param name="minIdx">The returned minimum location</param>
      /// <param name="maxIdx">The returned maximum location</param>
      /// <param name="mask">The extremums are searched across the whole array if mask is IntPtr.Zert. Otherwise, search is performed in the specified array region.</param>
      public static void CvMinMaxIdx(IntPtr src, out double minVal, out double maxVal, int[] minIdx, int[] maxIdx, IntPtr mask)
      {
         GCHandle minHandle = GCHandle.Alloc(minIdx, GCHandleType.Pinned);
         GCHandle maxHandle = GCHandle.Alloc(maxIdx, GCHandleType.Pinned);
         minVal = 0;
         maxVal = 0;
         _CvMinMaxIdx(src, ref minVal, ref maxVal, minHandle.AddrOfPinnedObject(), maxHandle.AddrOfPinnedObject(), mask);
         minHandle.Free();
         maxHandle.Free();
      }

      /// <summary>
      /// Applies arbitrary linear filter to the image. In-place operation is supported. When the aperture is partially outside the image, the function interpolates outlier pixel values from the nearest pixels that is inside the image
      /// </summary>
      /// <param name="src">The source image</param>
      /// <param name="dst">The destination image</param>
      /// <param name="kernel">Convolution kernel, single-channel floating point matrix. If you want to apply different kernels to different channels, split the image using cvSplit into separate color planes and process them individually</param>
      /// <param name="anchor">The anchor of the kernel that indicates the relative position of a filtered point within the kernel. The anchor shoud lie within the kernel. The special default value (-1,-1) means that it is at the kernel center</param>
      public static void cvFilter2D(IntPtr src, IntPtr dst, IntPtr kernel, Point anchor)
      {
         cvFilter2D(src, dst, kernel, anchor, 0, CvEnum.BORDER_TYPE.REPLICATE);
      }

      /// <summary>
      /// Applies arbitrary linear filter to the image. In-place operation is supported. When the aperture is partially outside the image, the function interpolates outlier pixel values from the nearest pixels that is inside the image
      /// </summary>
      /// <param name="src">The source image</param>
      /// <param name="dst">The destination image</param>
      /// <param name="kernel">Convolution kernel, single-channel floating point matrix. If you want to apply different kernels to different channels, split the image using cvSplit into separate color planes and process them individually</param>
      /// <param name="anchor">The anchor of the kernel that indicates the relative position of a filtered point within the kernel. The anchor shoud lie within the kernel. The special default value (-1,-1) means that it is at the kernel center</param>
      /// <param name="delta">The optional value added to the filtered pixels before storing them in dst</param>
      /// <param name="borderType">The pixel extrapolation method</param>
      [DllImport(EXTERN_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention, EntryPoint="CvFilter2D")]
      public static extern void cvFilter2D(IntPtr src, IntPtr dst, IntPtr kernel, Point anchor, double delta, Emgu.CV.CvEnum.BORDER_TYPE borderType);

      /// <summary>
      /// Contrast Limited Adaptative Histogram Equalization (CLAHE)
      /// </summary>
      /// <param name="srcArr">The source image</param>
      /// <param name="clipLimit">Clip Limit, use 40 for default</param>
      /// <param name="tileGridSize">Tile grid size, use (8, 8) for default</param>
      /// <param name="dstArr">The destination image</param>
      [DllImport(EXTERN_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      public static extern void cvCLAHE(IntPtr srcArr, double clipLimit, Size tileGridSize, IntPtr dstArr);

      /// <summary>
      /// Perform image denoising using Non-local Means Denoising algorithm: 
      /// http://www.ipol.im/pub/algo/bcm_non_local_means_denoising/ 
      /// with several computational optimizations. Noise expected to be a gaussian white noise.
      /// </summary>
      /// <param name="src">Input 8-bit 1-channel, 2-channel or 3-channel image.</param>
      /// <param name="dst">Output image with the same size and type as src.</param>
      /// <param name="h">Parameter regulating filter strength. Big h value perfectly removes noise but also removes image details, smaller h value preserves details but also preserves some noise. Recommended value 3</param>
      /// <param name="templateWindowSize">Size in pixels of the template patch that is used to compute weights. Should be odd. Recommended value 7 pixels</param>
      /// <param name="searchWindowSize">Size in pixels of the window that is used to compute weighted average for given pixel. Should be odd. Affect performance linearly: greater searchWindowsSize - greater denoising time. Recommended value 21 pixels</param>
      [DllImport(EXTERN_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      public static extern void cvFastNlMeansDenoising(IntPtr src, IntPtr dst, float h, int templateWindowSize, int searchWindowSize);

      /// <summary>
      /// Perform image denoising using Non-local Means Denoising algorithm (modified for color image): 
      /// http://www.ipol.im/pub/algo/bcm_non_local_means_denoising/ 
      /// with several computational optimizations. Noise expected to be a gaussian white noise.
      /// The function converts image to CIELAB colorspace and then separately denoise L and AB components with given h parameters using fastNlMeansDenoising function.
      /// </summary>
      /// <param name="src">Input 8-bit 1-channel, 2-channel or 3-channel image.</param>
      /// <param name="dst">Output image with the same size and type as src.</param>
      /// <param name="h">Parameter regulating filter strength. Big h value perfectly removes noise but also removes image details, smaller h value preserves details but also preserves some noise. Recommended value 3</param>
      /// <param name="hColor">The same as h but for color components. For most images value equals 10 will be enought to remove colored noise and do not distort colors.</param>
      /// <param name="templateWindowSize">Size in pixels of the template patch that is used to compute weights. Should be odd. Recommended value 7 pixels</param>
      /// <param name="searchWindowSize">Size in pixels of the window that is used to compute weighted average for given pixel. Should be odd. Affect performance linearly: greater searchWindowsSize - greater denoising time. Recommended value 21 pixels</param>
      [DllImport(EXTERN_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      public static extern void cvFastNlMeansDenoisingColored(IntPtr src, IntPtr dst, float h, float hColor, int templateWindowSize, int searchWindowSize);

      /// <summary>
      /// The class implements the “Dual TV L1” optical flow algorithm.
      /// </summary>
      /// <param name="prev">The first 8-bit single-channel input image.</param>
      /// <param name="next">The second input image of the same size and the same type as prev.</param>
      /// <param name="flow">The computed flow image that has the same size as prev and type CV_32FC2 .</param>
      [DllImport(EXTERN_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      public static extern void cvCalcOpticalFlowDualTVL1(IntPtr prev, IntPtr next, IntPtr flow);

      /// <summary>
      /// This function retrive the Open CV structure sizes in unmanaged code
      /// </summary>
      /// <param name="sizes">The structure that will hold the Open CV structure sizes</param>
      [DllImport(EXTERN_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention, EntryPoint="getCvStructSizes")]
      public static extern void GetCvStructSizes(ref CvStructSizes sizes);

      /*
      public static void TestDrawLine(IntPtr img, int startX, int startY, int endX, int endY, MCvScalar color)
      {
         TestDrawLine(img, startX, startY, endX, endY, color.v0, color.v1, color.v2, color.v3);
      }

      [DllImport(CvInvoke.EXTERN_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention, EntryPoint="testDrawLine")]
      private static extern void TestDrawLine(IntPtr img, int startX, int startY, int endX, int endY, double v0, double v1, double v2, double v3);
      */

      /// <summary>
      /// Implements the chamfer matching algorithm on images taking into account both distance from
      /// the template pixels to the nearest pixels and orientation alignment between template and image
      /// contours.
      /// </summary>
      /// <param name="img">The edge image where search is performed</param>
      /// <param name="templ">The template (an edge image)</param>
      /// <param name="contours">The output contours</param>
      /// <param name="cost">The cost associated with the matching</param>
      /// <param name="templScale">The template scale, use 1 for default</param>
      /// <param name="maxMatches">The maximum number of matches, use 20 for default</param>
      /// <param name="minMatchDistance">The minimum match distance. use 1.0 for default</param>
      /// <param name="padX">PadX, use 3 for default</param>
      /// <param name="padY">PadY, use 3 for default</param>
      /// <param name="scales">Scales, use 5 for default</param>
      /// <param name="minScale">Minimum scale, use 0.6 for default</param>
      /// <param name="maxScale">Maximum scale, use 1.6 for default</param>
      /// <param name="orientationWeight">Orientation weight, use 0.5 for default</param>
      /// <param name="truncate">Truncate, use 20 for default</param>
      /// <returns>The number of matches</returns>
      public static int cvChamferMatching(Image<Gray, Byte> img, Image<Gray, Byte> templ,
         out Point[][] contours, out float[] cost,
         double templScale, int maxMatches,
         double minMatchDistance, int padX,
         int padY, int scales, double minScale, double maxScale,
         double orientationWeight, double truncate)
      {
         using (Emgu.CV.Util.VectorOfVectorOfPoint vecOfVecOfPoint = new Util.VectorOfVectorOfPoint())
         using (Emgu.CV.Util.VectorOfFloat vecOfFloat = new Util.VectorOfFloat())
         {
            int count = _cvChamferMatching(img, templ, vecOfVecOfPoint, vecOfFloat, templScale, maxMatches, minMatchDistance, padX, padY, scales, minScale, maxScale, orientationWeight, truncate);
            contours = vecOfVecOfPoint.ToArray();
            cost = vecOfFloat.ToArray();
            return count;
         }
      }

      [DllImport(EXTERN_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention, EntryPoint = "cvChamferMatching")]
      private static extern int _cvChamferMatching(
         IntPtr img, IntPtr templ,
         IntPtr results, IntPtr cost,
         double templScale, int maxMatches,
         double minMatchDistance, int padX,
         int padY, int scales, double minScale, double maxScale,
         double orientationWeight, double truncate);
   }
}
