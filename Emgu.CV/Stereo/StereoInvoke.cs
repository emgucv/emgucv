//----------------------------------------------------------------------------
//  Copyright (C) 2004-2022 by EMGU Corporation. All rights reserved.       
//----------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Emgu.CV.Structure;
using Emgu.CV;
using Emgu.CV.Util;
using System.Drawing;

namespace Emgu.CV
{
    public static partial class CvInvoke
    {
        /// <summary>
        /// Computes disparity map for the specified stereo pair
        /// </summary>
        /// <param name="matcher">The stereo matcher</param>
        /// <param name="left">Left 8-bit single-channel image.</param>
        /// <param name="right">Right image of the same size and the same type as the left one.</param>
        /// <param name="disparity">Output disparity map. It has the same size as the input images. Some algorithms, like StereoBM or StereoSGBM compute 16-bit fixed-point disparity map (where each disparity value has 4 fractional bits), whereas other algorithms output 32-bit floating-point disparity map</param>
        public static void Compute(this IStereoMatcher matcher, IInputArray left, IInputArray right, IOutputArray disparity)
        {
            using (InputArray iaLeft = left.GetInputArray())
            using (InputArray iaRight = right.GetInputArray())
            using (OutputArray oaDisparity = disparity.GetOutputArray())
                cveStereoMatcherCompute(matcher.StereoMatcherPtr, iaLeft, iaRight, oaDisparity);
        }

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern void cveStereoMatcherCompute(IntPtr disparitySolver, IntPtr left, IntPtr right, IntPtr disparity);

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern void cveStereoMatcherRelease(ref IntPtr sharedPtr);

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern IntPtr cveStereoBMCreate(int numberOfDisparities, int blockSize, ref IntPtr sharedPtr);

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern IntPtr cveStereoSGBMCreate(
           int minDisparity, int numDisparities, int blockSize,
           int P1, int P2, int disp12MaxDiff,
           int preFilterCap, int uniquenessRatio,
           int speckleWindowSize, int speckleRange,
           StereoSGBM.Mode mode, ref IntPtr stereoMatcher,
           ref IntPtr sharedPtr);

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern void cveStereoSGBMRelease(ref IntPtr sharedPtr);


        /// <summary>
        /// Computes the rectification transformations without knowing intrinsic parameters of the cameras and their relative position in space, hence the suffix "Uncalibrated". Another related difference from cvStereoRectify is that the function outputs not the rectification transformations in the object (3D) space, but the planar perspective transformations, encoded by the homography matrices H1 and H2. The function implements the following algorithm [Hartley99]. 
        /// </summary>
        /// <remarks>
        /// Note that while the algorithm does not need to know the intrinsic parameters of the cameras, it heavily depends on the epipolar geometry. Therefore, if the camera lenses have significant distortion, it would better be corrected before computing the fundamental matrix and calling this function. For example, distortion coefficients can be estimated for each head of stereo camera separately by using cvCalibrateCamera2 and then the images can be corrected using cvUndistort2
        /// </remarks>
        /// <param name="points1">The array of 2D points</param>
        /// <param name="points2">The array of 2D points</param>
        /// <param name="f">Fundamental matrix. It can be computed using the same set of point pairs points1 and points2 using cvFindFundamentalMat</param>
        /// <param name="imgSize">Size of the image</param>
        /// <param name="h1">The rectification homography matrices for the first images</param>
        /// <param name="h2">The rectification homography matrices for the second images</param>
        /// <param name="threshold">If the parameter is greater than zero, then all the point pairs that do not comply the epipolar geometry well enough (that is, the points for which fabs(points2[i]T*F*points1[i])&gt;threshold) are rejected prior to computing the homographies</param>
        /// <returns>True if successful</returns>
        public static bool StereoRectifyUncalibrated(
            IInputArray points1,
            IInputArray points2,
            IInputArray f,
            Size imgSize,
            IOutputArray h1,
            IOutputArray h2,
            double threshold = 5)
        {
            using (InputArray iaPoints1 = points1.GetInputArray())
            using (InputArray iaPoints2 = points2.GetInputArray())
            using (InputArray iaF = f.GetInputArray())
            using (OutputArray oaH1 = h1.GetOutputArray())
            using (OutputArray oaH2 = h2.GetOutputArray())
                return cveStereoRectifyUncalibrated(iaPoints1, iaPoints2, iaF, ref imgSize, oaH1, oaH2, threshold);
        }

        [DllImport(ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        [return: MarshalAs(CvInvoke.BoolMarshalType)]
        private static extern bool cveStereoRectifyUncalibrated(IntPtr points1, IntPtr points2, IntPtr f, ref Size imgSize,
           IntPtr h1, IntPtr h2, double threshold);


        /// <summary>
        /// computes the rotation matrices for each camera that (virtually) make both camera image planes the same plane. Consequently, that makes all the epipolar lines parallel and thus simplifies the dense stereo correspondence problem. On input the function takes the matrices computed by cvStereoCalibrate and on output it gives 2 rotation matrices and also 2 projection matrices in the new coordinates. The function is normally called after cvStereoCalibrate that computes both camera matrices, the distortion coefficients, R and T
        /// </summary>
        /// <param name="cameraMatrix1">The camera matrices [fx_k 0 cx_k; 0 fy_k cy_k; 0 0 1]</param>
        /// <param name="cameraMatrix2">The camera matrices [fx_k 0 cx_k; 0 fy_k cy_k; 0 0 1]</param>
        /// <param name="distCoeffs1">The vectors of distortion coefficients for first camera, 4x1, 1x4, 5x1 or 1x5</param>
        /// <param name="distCoeffs2">The vectors of distortion coefficients for second camera, 4x1, 1x4, 5x1 or 1x5</param>
        /// <param name="imageSize">Size of the image used for stereo calibration</param>
        /// <param name="r">The rotation matrix between the 1st and the 2nd cameras' coordinate systems</param>
        /// <param name="t">The translation vector between the cameras' coordinate systems</param>
        /// <param name="r1">3x3 Rectification transforms (rotation matrices) for the first camera</param>
        /// <param name="r2">3x3 Rectification transforms (rotation matrices) for the second camera</param>
        /// <param name="p1">3x4 Projection matrices in the new (rectified) coordinate systems</param>
        /// <param name="p2">3x4 Projection matrices in the new (rectified) coordinate systems</param>
        /// <param name="q">The optional output disparity-to-depth mapping matrix, 4x4, see cvReprojectImageTo3D. </param>
        /// <param name="flags">The operation flags, use ZeroDisparity for default</param>
        /// <param name="alpha">Use -1 for default</param>
        /// <param name="newImageSize">Use Size.Empty for default</param>
        /// <param name="validPixRoi1">The valid pixel ROI for image1</param>
        /// <param name="validPixRoi2">The valid pixel ROI for image2</param>
        public static void StereoRectify(
           IInputArray cameraMatrix1, IInputArray distCoeffs1,
           IInputArray cameraMatrix2, IInputArray distCoeffs2,
           Size imageSize, IInputArray r, IInputArray t,
           IOutputArray r1, IOutputArray r2,
           IOutputArray p1, IOutputArray p2,
           IOutputArray q, CvEnum.StereoRectifyType flags,
           double alpha, Size newImageSize,
           ref Rectangle validPixRoi1, ref Rectangle validPixRoi2)
        {
            using (InputArray iaCameraMatrix1 = cameraMatrix1.GetInputArray())
            using (InputArray iaDistCoeffs1 = distCoeffs1.GetInputArray())
            using (InputArray iaCameraMatrix2 = cameraMatrix2.GetInputArray())
            using (InputArray iaDistCoeffs2 = distCoeffs2.GetInputArray())
            using (InputArray iaR = r.GetInputArray())
            using (InputArray iaT = t.GetInputArray())
            using (OutputArray oaR1 = r1.GetOutputArray())
            using (OutputArray oaR2 = r2.GetOutputArray())
            using (OutputArray oaP1 = p1.GetOutputArray())
            using (OutputArray oaP2 = p2.GetOutputArray())
            using (OutputArray oaQ = q.GetOutputArray())
                cveStereoRectify(
                   iaCameraMatrix1, iaDistCoeffs1,
                   iaCameraMatrix2, iaDistCoeffs2,
                   ref imageSize, iaR, iaT,
                   oaR1, oaR2,
                   oaP1, oaP2,
                   oaQ, flags,
                   alpha, ref newImageSize, ref validPixRoi1, ref validPixRoi2);
        }

        [DllImport(ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        private static extern void cveStereoRectify(
           IntPtr cameraMatrix1,
           IntPtr distCoeffs1,
           IntPtr cameraMatrix2,
           IntPtr distCoeffs2,
           ref Size imageSize,
           IntPtr r,
           IntPtr t,
           IntPtr r1,
           IntPtr r2,
           IntPtr p1,
           IntPtr p2,
           IntPtr q,
           CvEnum.StereoRectifyType flags,
           double alpha,
           ref Size newImageSize,
           ref Rectangle validPixRoi1,
           ref Rectangle validPixRoi2
           );
    }
}