//----------------------------------------------------------------------------
//  Copyright (C) 2004-2020 by EMGU Corporation. All rights reserved.       
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
        internal extern static void cveStereoMatcherCompute(IntPtr disparitySolver, IntPtr left, IntPtr right, IntPtr disparity);

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal extern static void cveStereoMatcherRelease(ref IntPtr sharedPtr);

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal extern static IntPtr cveStereoBMCreate(int numberOfDisparities, int blockSize, ref IntPtr sharedPtr);

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal extern static IntPtr cveStereoSGBMCreate(
           int minDisparity, int numDisparities, int blockSize,
           int P1, int P2, int disp12MaxDiff,
           int preFilterCap, int uniquenessRatio,
           int speckleWindowSize, int speckleRange,
           StereoSGBM.Mode mode, ref IntPtr stereoMatcher,
           ref IntPtr sharedPtr);

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal extern static void cveStereoSGBMRelease(ref IntPtr sharedPtr);

        /// <summary>
        /// Transforms the image to compensate radial and tangential lens distortion. 
        /// </summary>
        /// <param name="src">The input (distorted) image</param>
        /// <param name="dst">The output (corrected) image</param>
        /// <param name="cameraMatrix">The camera matrix (A) [fx 0 cx; 0 fy cy; 0 0 1].</param>
        /// <param name="distortionCoeffs">The vector of distortion coefficients, 4x1 or 1x4 [k1, k2, p1, p2].</param>
        /// <param name="newCameraMatrix">Camera matrix of the distorted image. By default it is the same as cameraMatrix, but you may additionally scale and shift the result by using some different matrix</param>
        public static void Undistort(
           IInputArray src,
           IOutputArray dst,
           IInputArray cameraMatrix,
           IInputArray distortionCoeffs,
           IInputArray newCameraMatrix = null)
        {
            using (InputArray iaSrc = src.GetInputArray())
            using (OutputArray oaDst = dst.GetOutputArray())
            using (InputArray iaCameraMatrix = cameraMatrix.GetInputArray())
            using (InputArray iaDistortionCoeffs = distortionCoeffs.GetInputArray())
            using (InputArray iaNewCameraMatrix = newCameraMatrix == null ? InputArray.GetEmpty() : newCameraMatrix.GetInputArray())
                cveUndistort(iaSrc, oaDst, iaCameraMatrix, iaDistortionCoeffs, iaNewCameraMatrix);
        }

        [DllImport(ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        private static extern void cveUndistort(
            IntPtr src,
            IntPtr dst,
            IntPtr cameraMatrix,
            IntPtr distortionCoeffs,
            IntPtr newCameraMatrix);

        /// <summary>
        /// This function is an extended version of cvInitUndistortMap. That is, in addition to the correction of lens distortion, the function can also apply arbitrary perspective transformation R and finally it can scale and shift the image according to the new camera matrix
        /// </summary>
        /// <param name="cameraMatrix">The camera matrix A=[fx 0 cx; 0 fy cy; 0 0 1]</param>
        /// <param name="distCoeffs">The vector of distortion coefficients, 4x1, 1x4, 5x1 or 1x5</param>
        /// <param name="R">The rectification transformation in object space (3x3 matrix). R1 or R2, computed by cvStereoRectify can be passed here. If the parameter is IntPtr.Zero, the identity matrix is used</param>
        /// <param name="newCameraMatrix">The new camera matrix A'=[fx' 0 cx'; 0 fy' cy'; 0 0 1]</param>
        /// <param name="depthType">Depth type of the first output map that can be CV_32FC1 or CV_16SC2 .</param>
        /// <param name="map1">The first output map.</param>
        /// <param name="map2">The second output map.</param>
        /// <param name="size">Undistorted image size.</param>
        public static void InitUndistortRectifyMap(
           IInputArray cameraMatrix,
           IInputArray distCoeffs,
           IInputArray R,
           IInputArray newCameraMatrix,
           Size size,
           CvEnum.DepthType depthType,
           IOutputArray map1,
           IOutputArray map2 = null)
        {
            int channels = map2 == null ? 2 : 1;
            using (InputArray iaCameraMatrix = cameraMatrix.GetInputArray())
            using (InputArray iaDistCoeffs = distCoeffs.GetInputArray())
            using (InputArray iaR = R == null ? InputArray.GetEmpty() : R.GetInputArray())
            using (InputArray iaNewCameraMatrix = newCameraMatrix.GetInputArray())
            using (OutputArray oaMap1 = map1.GetOutputArray())
            using (OutputArray oaMap2 = map2 == null ? OutputArray.GetEmpty() : map2.GetOutputArray())
                cveInitUndistortRectifyMap(
                   iaCameraMatrix,
                   iaDistCoeffs,
                   iaR,
                   iaNewCameraMatrix,
                   ref size,
                   CvInvoke.MakeType(depthType, channels),
                   oaMap1, oaMap2);
        }
        [DllImport(ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        private static extern void cveInitUndistortRectifyMap(
           IntPtr cameraMatrix,
           IntPtr distCoeffs,
           IntPtr R,
           IntPtr newCameraMatrix,
           ref Size size,
           int m1type,
           IntPtr map1,
           IntPtr map2);

        /// <summary>
        /// Similar to cvInitUndistortRectifyMap and is opposite to it at the same time. 
        /// The functions are similar in that they both are used to correct lens distortion and to perform the optional perspective (rectification) transformation. 
        /// They are opposite because the function cvInitUndistortRectifyMap does actually perform the reverse transformation in order to initialize the maps properly, while this function does the forward transformation. 
        /// </summary>
        /// <param name="src">The observed point coordinates</param>
        /// <param name="dst">The ideal point coordinates, after undistortion and reverse perspective transformation. </param>
        /// <param name="cameraMatrix">The camera matrix A=[fx 0 cx; 0 fy cy; 0 0 1]</param>
        /// <param name="distCoeffs">The vector of distortion coefficients, 4x1, 1x4, 5x1 or 1x5. </param>
        /// <param name="R">The rectification transformation in object space (3x3 matrix). R1 or R2, computed by cvStereoRectify can be passed here. If the parameter is IntPtr.Zero, the identity matrix is used.</param>
        /// <param name="P">The new camera matrix (3x3) or the new projection matrix (3x4). P1 or P2, computed by cvStereoRectify can be passed here. If the parameter is IntPtr.Zero, the identity matrix is used.</param>
        public static void UndistortPoints(
           IInputArray src,
           IOutputArray dst,
           IInputArray cameraMatrix,
           IInputArray distCoeffs,
           IInputArray R = null,
           IInputArray P = null)
        {
            using (InputArray iaSrc = src.GetInputArray())
            using (OutputArray oaDst = dst.GetOutputArray())
            using (InputArray iaCameraMatrix = cameraMatrix.GetInputArray())
            using (InputArray iaDistCoeffs = distCoeffs.GetInputArray())
            using (InputArray iaR = R == null ? InputArray.GetEmpty() : R.GetInputArray())
            using (InputArray iaP = P == null ? InputArray.GetEmpty() : P.GetInputArray())
                cveUndistortPoints(
                   iaSrc,
                   oaDst,
                   iaCameraMatrix,
                   iaDistCoeffs,
                   iaR,
                   iaP);
        }
        [DllImport(ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        private static extern void cveUndistortPoints(
           IntPtr src,
           IntPtr dst,
           IntPtr cameraMatrix,
           IntPtr distCoeffs,
           IntPtr R,
           IntPtr P);

        /// <summary>
        /// Returns the default new camera matrix.
        /// </summary>
        /// <param name="cameraMatrix">Input camera matrix.</param>
        /// <param name="imgsize">Camera view image size in pixels.</param>
        /// <param name="centerPrincipalPoint">Location of the principal point in the new camera matrix. The parameter indicates whether this location should be at the image center or not.</param>
        /// <returns>The default new camera matrix.</returns>
        public static Mat GetDefaultNewCameraMatrix(IInputArray cameraMatrix, Size imgsize = new Size(), bool centerPrincipalPoint = false)
        {
            Mat m = new Mat();
            using (InputArray iaCameraMatrix = cameraMatrix.GetInputArray())
                cveGetDefaultNewCameraMatrix(iaCameraMatrix, ref imgsize, centerPrincipalPoint, m.Ptr);
            return m;
        }
        [DllImport(ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        private static extern void cveGetDefaultNewCameraMatrix(
            IntPtr cameraMatrix, ref Size imgsize,
            [MarshalAs(CvInvoke.BoolMarshalType)]
            bool centerPrincipalPoint,
            IntPtr cm);

        /// <summary>
        /// Computes an optimal affine transformation between two 2D point sets.
        /// </summary>
        /// <param name="from">First input 2D point set containing (X,Y).</param>
        /// <param name="to">Second input 2D point set containing (x,y).</param>
        /// <param name="inliners">Output vector indicating which points are inliers (1-inlier, 0-outlier).</param>
        /// <param name="method">Robust method used to compute transformation. </param>
        /// <param name="ransacReprojThreshold">Maximum reprojection error in the RANSAC algorithm to consider a point as an inlier. Applies only to RANSAC.</param>
        /// <param name="maxIters">The maximum number of robust method iterations.</param>
        /// <param name="confidence">Confidence level, between 0 and 1, for the estimated transformation. Anything between 0.95 and 0.99 is usually good enough. Values too close to 1 can slow down the estimation significantly. Values lower than 0.8-0.9 can result in an incorrectly estimated transformation.</param>
        /// <param name="refineIters">Maximum number of iterations of refining algorithm (Levenberg-Marquardt). Passing 0 will disable refining, so the output matrix will be output of robust method.</param>
        /// <returns>Output 2D affine transformation matrix 2×3 or empty matrix if transformation could not be estimated.</returns>
        public static Mat EstimateAffine2D(
            PointF[] from, PointF[] to,
            IOutputArray inliners = null,
            CvEnum.RobustEstimationAlgorithm method = CvEnum.RobustEstimationAlgorithm.Ransac, 
            double ransacReprojThreshold = 3,
            int maxIters = 2000, 
            double confidence = 0.99,
            int refineIters = 10)
        {
            using(VectorOfPointF vpFrom = new VectorOfPointF(from))
            using(VectorOfPointF vpTo = new VectorOfPointF(to))
            {
                return EstimateAffine2D(vpFrom, vpTo, inliners, method, ransacReprojThreshold, maxIters, confidence, refineIters);
            }
        }

        /// <summary>
        /// Computes an optimal affine transformation between two 2D point sets.
        /// </summary>
        /// <param name="from">First input 2D point set containing (X,Y).</param>
        /// <param name="to">Second input 2D point set containing (x,y).</param>
        /// <param name="inliners">Output vector indicating which points are inliers (1-inlier, 0-outlier).</param>
        /// <param name="method">Robust method used to compute transformation. </param>
        /// <param name="ransacReprojThreshold">Maximum reprojection error in the RANSAC algorithm to consider a point as an inlier. Applies only to RANSAC.</param>
        /// <param name="maxIters">The maximum number of robust method iterations.</param>
        /// <param name="confidence">Confidence level, between 0 and 1, for the estimated transformation. Anything between 0.95 and 0.99 is usually good enough. Values too close to 1 can slow down the estimation significantly. Values lower than 0.8-0.9 can result in an incorrectly estimated transformation.</param>
        /// <param name="refineIters">Maximum number of iterations of refining algorithm (Levenberg-Marquardt). Passing 0 will disable refining, so the output matrix will be output of robust method.</param>
        /// <returns>Output 2D affine transformation matrix 2×3 or empty matrix if transformation could not be estimated.</returns>
        public static Mat EstimateAffine2D(
            IInputArray from, 
            IInputArray to, 
            IOutputArray inliners = null, 
            CvEnum.RobustEstimationAlgorithm method = CvEnum.RobustEstimationAlgorithm.Ransac, 
            double ransacReprojThreshold = 3,
            int maxIters = 2000, 
            double confidence = 0.99,
            int refineIters= 10)
        {
            Mat affine = new Mat();
            using (InputArray iaFrom = from.GetInputArray())
            using (InputArray iaTo = to.GetInputArray())
            using (OutputArray oaInliners = inliners == null ? OutputArray.GetEmpty() : inliners.GetOutputArray())
            {
                cveEstimateAffine2D(iaFrom, iaTo, oaInliners, method, ransacReprojThreshold, maxIters, confidence, refineIters, affine);
            }
            return affine;
        }

        [DllImport(ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        private static extern void cveEstimateAffine2D(
            IntPtr from, IntPtr to,
            IntPtr inliners,
            CvEnum.RobustEstimationAlgorithm method, double ransacReprojThreshold,
            int maxIters, double confidence,
            int refineIters,
            IntPtr affine);

        /// <summary>
        /// Computes an optimal limited affine transformation with 4 degrees of freedom between two 2D point sets.
        /// </summary>
        /// <param name="from">First input 2D point set.</param>
        /// <param name="to">Second input 2D point set.</param>
        /// <param name="inliners">Output vector indicating which points are inliers.</param>
        /// <param name="method">Robust method used to compute transformation.</param>
        /// <param name="ransacReprojThreshold">Maximum reprojection error in the RANSAC algorithm to consider a point as an inlier. Applies only to RANSAC.</param>
        /// <param name="maxIters">The maximum number of robust method iterations.</param>
        /// <param name="confidence">Confidence level, between 0 and 1, for the estimated transformation. Anything between 0.95 and 0.99 is usually good enough. Values too close to 1 can slow down the estimation significantly. Values lower than 0.8-0.9 can result in an incorrectly estimated transformation.</param>
        /// <param name="refineIters">Maximum number of iterations of refining algorithm (Levenberg-Marquardt). Passing 0 will disable refining, so the output matrix will be output of robust method.</param>
        /// <returns>Output 2D affine transformation (4 degrees of freedom) matrix 2×3 or empty matrix if transformation could not be estimated.</returns>
        public static Mat EstimateAffinePartial2D(
            IInputArray from, IInputArray to,
            IOutputArray inliners,
            CvEnum.RobustEstimationAlgorithm method, 
            double ransacReprojThreshold,
            int maxIters, double confidence,
            int refineIters)
        {
            Mat affine = new Mat();
            using (InputArray iaFrom = from.GetInputArray())
            using (InputArray iaTo = to.GetInputArray())
            using (OutputArray oaInliners = inliners == null ? OutputArray.GetEmpty() : inliners.GetOutputArray())
            {
                cveEstimateAffinePartial2D(
                    iaFrom,
                    iaTo,
                    oaInliners,
                    method,
                    ransacReprojThreshold,
                    maxIters,
                    confidence,
                    refineIters,
                    affine
                    );
            }
            return affine;
        }

        [DllImport(ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        private static extern void cveEstimateAffinePartial2D(
            IntPtr from, IntPtr to,
            IntPtr inliners,
            CvEnum.RobustEstimationAlgorithm method, double ransacReprojThreshold,
            int maxIters, double confidence,
            int refineIters,
            IntPtr affine);

        /// <summary>
        /// Computes Hand-Eye calibration
        /// </summary>
        /// <param name="rGripper2base">
        /// Rotation part extracted from the homogeneous matrix that transforms a point expressed in the gripper frame to the robot base frame.
        /// This is a vector (vector&lt;Mat&gt;) that contains the rotation matrices for all the transformations from gripper frame to robot base frame.
        /// </param>
        /// <param name="tGripper2base">
        /// Translation part extracted from the homogeneous matrix that transforms a point expressed in the gripper frame to the robot base frame.
        /// This is a vector (vector&lt;Mat&gt;) that contains the translation vectors for all the transformations from gripper frame to robot base frame.
        /// </param>
        /// <param name="rTarget2cam">
        /// Rotation part extracted from the homogeneous matrix that transforms a point expressed in the target frame to the camera frame.
        /// This is a vector (vector&lt;Mat&gt;) that contains the rotation matrices for all the transformations from calibration target frame to camera frame.
        /// </param>
        /// <param name="tTarget2cam">
        /// Rotation part extracted from the homogeneous matrix that transforms a point expressed in the target frame to the camera frame.
        /// This is a vector (vector&lt;Mat&gt;) that contains the translation vectors for all the transformations from calibration target frame to camera frame.
        /// </param>
        /// <param name="rCam2gripper">
        /// Estimated rotation part extracted from the homogeneous matrix that transforms a point expressed in the camera frame to the gripper frame.
        /// </param>
        /// <param name="tCam2gripper">
        /// Estimated translation part extracted from the homogeneous matrix that transforms a point expressed in the camera frame to the gripper frame.
        /// </param>
        /// <param name="method">One of the implemented Hand-Eye calibration method</param>
        public static void CalibrateHandEye(
            IInputArrayOfArrays rGripper2base,
            IInputArrayOfArrays tGripper2base,
            IInputArrayOfArrays rTarget2cam,
            IInputArrayOfArrays tTarget2cam,
            IOutputArray rCam2gripper,
            IOutputArray tCam2gripper,
            CvEnum.HandEyeCalibrationMethod method)
        {
            using (InputArray iaRGripper2Base = rGripper2base.GetInputArray())
            using (InputArray iaTGripper2Base = rGripper2base.GetInputArray())
            using (InputArray iaRTarget2Cam = rTarget2cam.GetInputArray())
            using (InputArray iaTTarget2Cam = tTarget2cam.GetInputArray())
            using (OutputArray oaRCam2Gripper = rCam2gripper.GetOutputArray())
            using (OutputArray oaTCam2Gripper = tCam2gripper.GetOutputArray())
            {
                cveCalibrateHandEye(
                    iaRGripper2Base, 
                    iaTGripper2Base,
                    iaRTarget2Cam,
                    iaTTarget2Cam,
                    oaRCam2Gripper,
                    oaTCam2Gripper,
                    method);
            }
        }

        [DllImport(ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        private static extern void cveCalibrateHandEye(
            IntPtr rGripper2base, 
            IntPtr tGripper2base,
            IntPtr rTarget2cam, 
            IntPtr tTarget2cam,
            IntPtr rCam2gripper, 
            IntPtr tCam2gripper,
            CvEnum.HandEyeCalibrationMethod method);
    }
}