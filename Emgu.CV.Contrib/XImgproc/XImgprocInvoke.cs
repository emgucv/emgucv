//----------------------------------------------------------------------------
//  Copyright (C) 2004-2016 by EMGU Corporation. All rights reserved.       
//----------------------------------------------------------------------------

using System;
using System.Runtime.InteropServices;
using System.Drawing;
using Emgu.CV.CvEnum;

namespace Emgu.CV.XImgproc
{
   /// <summary>
   /// Domain Transform filter type
   /// </summary>
   public enum DtFilterType
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
      RF,
   }

   /// <summary>
   /// Extended Image Processing
   /// </summary>
   public static partial class XImgprocInvoke
   {
      static XImgprocInvoke()
      {
         CvInvoke.CheckLibraryLoaded();
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
      /// <param name="type">Threshold type</param>
      /// <param name="blockSize">Block size</param>
      /// <param name="delta">delta</param>
      /// <param name="maxValue">Maximum value to use with CV_THRESH_BINARY and CV_THRESH_BINARY_INV thresholding types</param>
      public static void NiBlackThreshold(IInputArray src, IOutputArray dst, double maxValue, ThresholdType type, int blockSize,
         double delta)
      {
         using (InputArray iaSrc = src.GetInputArray())
         using (OutputArray oaDst = dst.GetOutputArray())
         {
            cveNiBlackThreshold(iaSrc, oaDst, maxValue, type, blockSize, delta);
         }
      }
      [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      private static extern void cveNiBlackThreshold(IntPtr src, IntPtr dst, double maxValue, ThresholdType type, int blockSize, double delta);

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
   }
}
