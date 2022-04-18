//----------------------------------------------------------------------------
//  Copyright (C) 2004-2022 by EMGU Corporation. All rights reserved.       
//----------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Emgu.CV.Structure;
using Emgu.CV.CvEnum;
using Emgu.CV.Util;
using System.Drawing;
using Emgu.CV.Features2D;

namespace Emgu.CV
{
    public static partial class CvInvoke
    {

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
            using (InputArray iaTGripper2Base = tGripper2base.GetInputArray())
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

        #region Epipolar Geometry, Stereo Correspondence

        /// <summary>
        /// Transforms 1-channel disparity map to 3-channel image, a 3D surface.
        /// </summary>
        /// <param name="disparity">Disparity map</param>
        /// <param name="image3D">3-channel, 16-bit integer or 32-bit floating-point image - the output map of 3D points</param>
        /// <param name="q">The reprojection 4x4 matrix, can be arbitrary, e.g. the one, computed by cvStereoRectify</param>
        /// <param name="handleMissingValues">Indicates, whether the function should handle missing values (i.e. points where the disparity was not computed). 
        /// If handleMissingValues=true, then pixels with the minimal disparity that corresponds to the outliers (see StereoMatcher::compute ) 
        /// are transformed to 3D points with a very large Z value (currently set to 10000).</param>
        /// <param name="ddepth">The optional output array depth. If it is -1, the output image will have CV_32F depth. ddepth can also be set to CV_16S, CV_32S or CV_32F.</param>
        public static void ReprojectImageTo3D(IInputArray disparity, IOutputArray image3D, IInputArray q,
           bool handleMissingValues = false, CvEnum.DepthType ddepth = CvEnum.DepthType.Default)
        {
            using (InputArray iaDisparity = disparity.GetInputArray())
            using (OutputArray oaImage3D = image3D.GetOutputArray())
            using (InputArray iaQ = q.GetInputArray())
                cveReprojectImageTo3D(iaDisparity, oaImage3D, iaQ, handleMissingValues, ddepth);
        }

        [DllImport(ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        private static extern void cveReprojectImageTo3D(
           IntPtr disparity,
           IntPtr image3D,
           IntPtr q,
           [MarshalAs(CvInvoke.BoolMarshalType)] bool handleMissingValues,
           CvEnum.DepthType ddepth);

        #endregion

        /*
        /// <summary>
        /// Calculates disparity for stereo-pair 
        /// </summary>
        /// <param name="leftImage">Left image of stereo pair, rectified grayscale 8-bit image</param>
        /// <param name="rightImage">Right image of stereo pair, rectified grayscale 8-bit image</param>
        /// <param name="mode">Algorithm used to find a disparity</param>
        /// <param name="depthImage">Destination depth image, grayscale 8-bit image that codes the scaled disparity, so that the zero disparity (corresponding to the points that are very far from the cameras) maps to 0, maximum disparity maps to 255.</param>
        /// <param name="maxDisparity">Maximum possible disparity. The closer the objects to the cameras, the larger value should be specified here. Too big values slow down the process significantly</param>
        /// <param name="param1">constant occlusion penalty</param>
        /// <param name="param2">constant match reward</param>
        /// <param name="param3">defines a highly reliable region (set of contiguous pixels whose reliability is at least param3)</param>
        /// <param name="param4">defines a moderately reliable region</param>
        /// <param name="param5">defines a slightly reliable region</param>
        [DllImport(OpencvCalib3dLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        public extern static void cvFindStereoCorrespondence(
           IntPtr leftImage, IntPtr rightImage,
           int mode, IntPtr depthImage,
           int maxDisparity,
           double param1, double param2, double param3,
           double param4, double param5);
        */

        #region Camera Calibration

        /// <summary>
        /// Finds an initial camera matrix from 3D-2D point correspondences.
        /// </summary>
        /// <param name="objectPoints">Vector of vectors of the calibration pattern points in the calibration pattern coordinate space.</param>
        /// <param name="imagePoints">Vector of vectors of the projections of the calibration pattern points.</param>
        /// <param name="imageSize">Image size in pixels used to initialize the principal point.</param>
        /// <param name="aspectRatio">If it is zero or negative, both fx and fy are estimated independently. Otherwise, fx=fy*aspectRatio.</param>
        /// <returns>An initial camera matrix for the camera calibration process.</returns>
        /// <remarks>Currently, the function only supports planar calibration patterns, which are patterns where each object point has z-coordinate =0.</remarks>
        public static Mat InitCameraMatrix2D(IInputArrayOfArrays objectPoints, IInputArrayOfArrays imagePoints, Size imageSize,
            double aspectRatio = 1.0)
        {
            Mat m = new Mat();
            using (InputArray iaObjectPoints = objectPoints.GetInputArray())
            using (InputArray iaImagePoints = imagePoints.GetInputArray())
                cveInitCameraMatrix2D(iaObjectPoints, iaImagePoints, ref imageSize, aspectRatio, m);
            return m;
        }

        [DllImport(ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        private static extern void cveInitCameraMatrix2D(
            IntPtr objectPoints,
            IntPtr imagePoints,
            ref Size imageSize,
            double aspectRatio,
            IntPtr cameraMatrix);

        /// <summary>
        /// Computes projections of 3D points to the image plane given intrinsic and extrinsic camera parameters. 
        /// Optionally, the function computes jacobians - matrices of partial derivatives of image points as functions of all the input parameters w.r.t. the particular parameters, intrinsic and/or extrinsic. 
        /// The jacobians are used during the global optimization in cvCalibrateCamera2 and cvFindExtrinsicCameraParams2. 
        /// The function itself is also used to compute back-projection error for with current intrinsic and extrinsic parameters. 
        /// </summary>
        /// <remarks>Note, that with intrinsic and/or extrinsic parameters set to special values, the function can be used to compute just extrinsic transformation or just intrinsic transformation (i.e. distortion of a sparse set of points) </remarks>
        /// <param name="objectPoints">The array of object points.</param>
        /// <param name="rvec">The rotation vector, 1x3 or 3x1</param>
        /// <param name="tvec">The translation vector, 1x3 or 3x1</param>
        /// <param name="cameraMatrix">The camera matrix (A) [fx 0 cx; 0 fy cy; 0 0 1]. </param>
        /// <param name="distCoeffs">The vector of distortion coefficients, 4x1 or 1x4 [k1, k2, p1, p2]. If it is IntPtr.Zero, all distortion coefficients are considered 0's</param>
        /// <returns>The output array of image points, 2xN or Nx2, where N is the total number of points in the view</returns>
        /// <param name="aspectRatio">Aspect ratio</param>
        /// <param name="jacobian">Optional output 2Nx(10+&lt;numDistCoeffs&gt;) jacobian matrix of derivatives of image points with respect to components of the rotation vector, translation vector, focal lengths, coordinates of the principal point and the distortion coefficients. In the old interface different components of the jacobian are returned via different output parameters.</param>
        /// <returns>The array of image points which is the projection of <paramref name="objectPoints"/></returns>
        public static PointF[] ProjectPoints(
         MCvPoint3D32f[] objectPoints,
         IInputArray rvec, IInputArray tvec, IInputArray cameraMatrix, IInputArray distCoeffs,
         IOutputArray jacobian = null, double aspectRatio = 0)
        {
            PointF[] imagePoints = new PointF[objectPoints.Length];

            GCHandle handle1 = GCHandle.Alloc(objectPoints, GCHandleType.Pinned);
            GCHandle handle2 = GCHandle.Alloc(imagePoints, GCHandleType.Pinned);
            using (
               Mat pointMatrix = new Mat(objectPoints.Length, 1, DepthType.Cv32F, 3, handle1.AddrOfPinnedObject(),
                  3 * sizeof(float)))
            using (
               Mat imagePointMatrix = new Mat(imagePoints.Length, 1, DepthType.Cv32F, 2, handle2.AddrOfPinnedObject(),
                  2 * sizeof(float)))
                CvInvoke.ProjectPoints(
                   pointMatrix,
                   rvec,
                   tvec,
                   cameraMatrix,
                   distCoeffs,
                   imagePointMatrix,
                   jacobian,
                   aspectRatio);
            handle1.Free();
            handle2.Free();
            return imagePoints;
        }


        /// <summary>
        /// Finds the camera intrinsic and extrinsic parameters from several views of a calibration pattern.
        /// </summary>
        /// <param name="objectPoints">
        /// In the new interface it is a vector of vectors of calibration pattern points in the calibration pattern coordinate space.
        /// The outer vector contains as many elements as the number of pattern views. If the same calibration pattern is shown in each
        /// view and it is fully visible, all the vectors will be the same. Although, it is possible to use partially occluded patterns
        /// or even different patterns in different views. Then, the vectors will be different. Although the points are 3D, they all
        /// lie in the calibration pattern's XY coordinate plane (thus 0 in the Z-coordinate), if the used calibration pattern is a
        /// planar rig. In the old interface all the vectors of object points from different views are concatenated together.
        /// </param>
        /// <param name="imagePoints">
        /// In the new interface it is a vector of vectors of the projections of calibration pattern points.
        /// In the old interface all the vectors of object points from different views are concatenated together.
        /// </param>
        /// <param name="imageSize">Size of the image used only to initialize the camera intrinsic matrix.</param>
        /// <param name="rotationVectors">
        /// Output vector of rotation vectors (Rodrigues) estimated for each pattern view. That is, each i-th rotation vector together
        /// with the corresponding i-th translation vector (see the next output parameter description) brings the calibration pattern from
        /// the object coordinate space (in which object points are specified) to the camera coordinate space. In more technical terms,
        /// the tuple of the i-th rotation and translation vector performs a change of basis from object coordinate space to camera
        /// coordinate space. Due to its duality, this tuple is equivalent to the position of the calibration pattern with respect to the
        /// camera coordinate space.
        /// </param>
        /// <param name="calibrationType">The camera calibration flags.</param>
        /// <param name="translationVectors">Output vector of translation vectors estimated for each pattern view, see parameter describtion above.</param>
        /// <param name="termCriteria">The termination criteria</param>
        /// <param name="cameraMatrix">Input/output 3x3 floating-point camera intrinsic matrix A [fx 0 cx; 0 fy cy; 0 0 1]. If CV_CALIB_USE_INTRINSIC_GUESS and/or CV_CALIB_FIX_ASPECT_RATION are specified, some or all of fx, fy, cx, cy must be initialized</param>
        /// <param name="distortionCoeffs">Input/output vector of distortion coefficients (k1,k2,p1,p2[,k3[,k4,k5,k6[,s1,s2,s3,s4[,τx,τy]]]]) of 4, 5, 8, 12 or 14 elements.</param>
        /// <returns>The final reprojection error</returns>
        public static double CalibrateCamera(
           MCvPoint3D32f[][] objectPoints,
           PointF[][] imagePoints,
           Size imageSize,
           IInputOutputArray cameraMatrix,
           IInputOutputArray distortionCoeffs,
           CvEnum.CalibType calibrationType,
           MCvTermCriteria termCriteria,
           out Mat[] rotationVectors,
           out Mat[] translationVectors)
        {
            System.Diagnostics.Debug.Assert(objectPoints.Length == imagePoints.Length,
               "The number of images for objects points should be equal to the number of images for image points");
            int imageCount = objectPoints.Length;

            using (VectorOfVectorOfPoint3D32F vvObjPts = new VectorOfVectorOfPoint3D32F(objectPoints))
            using (VectorOfVectorOfPointF vvImgPts = new VectorOfVectorOfPointF(imagePoints))
            {
                double reprojectionError;
                using (VectorOfMat rVecs = new VectorOfMat())
                using (VectorOfMat tVecs = new VectorOfMat())
                {
                    reprojectionError = CvInvoke.CalibrateCamera(
                       vvObjPts,
                       vvImgPts,
                       imageSize,
                       cameraMatrix,
                       distortionCoeffs,
                       rVecs,
                       tVecs,
                       calibrationType,
                       termCriteria);

                    rotationVectors = new Mat[imageCount];
                    translationVectors = new Mat[imageCount];
                    for (int i = 0; i < imageCount; i++)
                    {
                        rotationVectors[i] = new Mat();
                        using (Mat matR = rVecs[i])
                            matR.CopyTo(rotationVectors[i]);
                        translationVectors[i] = new Mat();
                        using (Mat matT = tVecs[i])
                            matT.CopyTo(translationVectors[i]);
                    }
                }
                return reprojectionError;
            }
        }

        /// <summary>
        /// Finds the camera intrinsic and extrinsic parameters from several views of a calibration pattern.
        /// </summary>
        /// <param name="objectPoints">
        /// In the new interface it is a vector of vectors of calibration pattern points in the calibration pattern coordinate space.
        /// The outer vector contains as many elements as the number of pattern views. If the same calibration pattern is shown in each
        /// view and it is fully visible, all the vectors will be the same. Although, it is possible to use partially occluded patterns
        /// or even different patterns in different views. Then, the vectors will be different. Although the points are 3D, they all
        /// lie in the calibration pattern's XY coordinate plane (thus 0 in the Z-coordinate), if the used calibration pattern is a
        /// planar rig. In the old interface all the vectors of object points from different views are concatenated together.
        /// </param>
        /// <param name="imagePoints">
        /// In the new interface it is a vector of vectors of the projections of calibration pattern points.
        /// In the old interface all the vectors of object points from different views are concatenated together.
        /// </param>
        /// <param name="imageSize">Size of the image used only to initialize the camera intrinsic matrix.</param>
        /// <param name="cameraMatrix">Input/output 3x3 floating-point camera intrinsic matrix A [fx 0 cx; 0 fy cy; 0 0 1]. If CV_CALIB_USE_INTRINSIC_GUESS and/or CV_CALIB_FIX_ASPECT_RATION are specified, some or all of fx, fy, cx, cy must be initialized</param>
        /// <param name="distortionCoeffs">Input/output vector of distortion coefficients (k1,k2,p1,p2[,k3[,k4,k5,k6[,s1,s2,s3,s4[,τx,τy]]]]) of 4, 5, 8, 12 or 14 elements.</param>
        /// <param name="rotationVectors">
        /// Output vector of rotation vectors (Rodrigues) estimated for each pattern view. That is, each i-th rotation vector together
        /// with the corresponding i-th translation vector (see the next output parameter description) brings the calibration pattern from
        /// the object coordinate space (in which object points are specified) to the camera coordinate space. In more technical terms,
        /// the tuple of the i-th rotation and translation vector performs a change of basis from object coordinate space to camera
        /// coordinate space. Due to its duality, this tuple is equivalent to the position of the calibration pattern with respect to the
        /// camera coordinate space.
        /// </param>
        /// <param name="translationVectors">Output vector of translation vectors estimated for each pattern view, see parameter describtion above.</param>
        /// <param name="flags">Different flags</param>
        /// <param name="termCriteria">The termination criteria</param>
        /// <returns>The final reprojection error</returns>
        public static double CalibrateCamera(
           IInputArray objectPoints,
           IInputArray imagePoints,
           Size imageSize,
           IInputOutputArray cameraMatrix,
           IInputOutputArray distortionCoeffs,
           IOutputArray rotationVectors,
           IOutputArray translationVectors,
           CvEnum.CalibType flags,
           MCvTermCriteria termCriteria)
        {
            using (InputArray iaObjectPoints = objectPoints.GetInputArray())
            using (InputArray iaImagePoints = imagePoints.GetInputArray())
            using (InputOutputArray ioaCameraMatrix = cameraMatrix.GetInputOutputArray())
            using (InputOutputArray ioaDistortionCoeffs = distortionCoeffs.GetInputOutputArray())
            using (OutputArray oaRotationVectors = rotationVectors.GetOutputArray())
            using (OutputArray oaTranslationVectors = translationVectors.GetOutputArray())
                return cveCalibrateCamera(
                   iaObjectPoints,
                   iaImagePoints,
                   ref imageSize,
                   ioaCameraMatrix,
                   ioaDistortionCoeffs,
                   oaRotationVectors,
                   oaTranslationVectors,
                   flags,
                   ref termCriteria);
        }

        [DllImport(ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        private static extern double cveCalibrateCamera(
           IntPtr objectPoints,
           IntPtr imagePoints,
           ref Size imageSize,
           IntPtr cameraMatrix,
           IntPtr distortionCoeffs,
           IntPtr rotationVectors,
           IntPtr translationVectors,
           CvEnum.CalibType flags,
           ref MCvTermCriteria termCriteria);

        /// <summary>
        /// Computes various useful camera (sensor/lens) characteristics using the computed camera calibration matrix, image frame resolution in pixels and the physical aperture size
        /// </summary>
        /// <param name="cameraMatrix">The matrix of intrinsic parameters</param>
        /// <param name="imageSize">Image size in pixels</param>
        /// <param name="apertureWidth">Aperture width in real-world units (optional input parameter). Set it to 0 if not used</param>
        /// <param name="apertureHeight">Aperture width in real-world units (optional input parameter). Set it to 0 if not used</param>
        /// <param name="fovx">Field of view angle in x direction in degrees</param>
        /// <param name="fovy">Field of view angle in y direction in degrees </param>
        /// <param name="focalLength">Focal length in real-world units </param>
        /// <param name="principalPoint">The principal point in real-world units </param>
        /// <param name="aspectRatio">The pixel aspect ratio ~ fy/f</param>
        public static void CalibrationMatrixValues(
           IInputArray cameraMatrix, Size imageSize, double apertureWidth, double apertureHeight,
           ref double fovx, ref double fovy, ref double focalLength, ref MCvPoint2D64f principalPoint,
           ref double aspectRatio)
        {
            using (InputArray iaCameraMatrix = cameraMatrix.GetInputArray())
                cveCalibrationMatrixValues(
                   iaCameraMatrix, ref imageSize, apertureWidth, apertureHeight, ref fovx, ref fovy, ref focalLength,
                   ref principalPoint, ref aspectRatio);
        }

        [DllImport(ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        private static extern void cveCalibrationMatrixValues(
           IntPtr cameraMatrix, ref Size imageSize, double apertureWidth, double apertureHeight,
           ref double fovx, ref double fovy, ref double focalLength, ref MCvPoint2D64f principalPoint,
           ref double aspectRatio);


        /// <summary>
        /// Estimates transformation between the 2 cameras making a stereo pair. If we have a stereo camera, where the relative position and orientatation of the 2 cameras is fixed, and if we computed poses of an object relative to the first camera and to the second camera, (R1, T1) and (R2, T2), respectively (that can be done with cvFindExtrinsicCameraParams2), obviously, those poses will relate to each other, i.e. given (R1, T1) it should be possible to compute (R2, T2) - we only need to know the position and orientation of the 2nd camera relative to the 1st camera. That's what the described function does. It computes (R, T) such that:
        /// R2=R*R1,
        /// T2=R*T1 + T
        /// </summary>
        /// <param name="objectPoints">The 3D location of the object points. The first index is the index of image, second index is the index of the point</param>
        /// <param name="imagePoints1">The 2D image location of the points for camera 1. The first index is the index of the image, second index is the index of the point</param>
        /// <param name="imagePoints2">The 2D image location of the points for camera 2. The first index is the index of the image, second index is the index of the point</param>
        /// <param name="cameraMatrix1">The input/output camera matrices [fxk 0 cxk; 0 fyk cyk; 0 0 1]. If CV_CALIB_USE_INTRINSIC_GUESS or CV_CALIB_FIX_ASPECT_RATIO are specified, some or all of the elements of the matrices must be initialized</param>
        /// <param name="distCoeffs1">The input/output vectors of distortion coefficients for each camera, 4x1, 1x4, 5x1 or 1x5</param>
        /// <param name="cameraMatrix2">The input/output camera matrices [fxk 0 cxk; 0 fyk cyk; 0 0 1]. If CV_CALIB_USE_INTRINSIC_GUESS or CV_CALIB_FIX_ASPECT_RATIO are specified, some or all of the elements of the matrices must be initialized</param>
        /// <param name="distCoeffs2">The input/output vectors of distortion coefficients for each camera, 4x1, 1x4, 5x1 or 1x5</param>
        /// <param name="imageSize">Size of the image, used only to initialize intrinsic camera matrix</param>
        /// <param name="r">The rotation matrix between the 1st and the 2nd cameras' coordinate systems </param>
        /// <param name="t">The translation vector between the cameras' coordinate systems</param>
        /// <param name="e">The optional output essential matrix</param>
        /// <param name="f">The optional output fundamental matrix </param>
        /// <param name="termCrit">Termination criteria for the iterative optimization algorithm</param>
        /// <param name="flags">The calibration flags</param>
        /// <returns>The final value of the re-projection error.</returns>
        public static double StereoCalibrate(
           MCvPoint3D32f[][] objectPoints,
           PointF[][] imagePoints1,
           PointF[][] imagePoints2,
           IInputOutputArray cameraMatrix1,
           IInputOutputArray distCoeffs1,
           IInputOutputArray cameraMatrix2,
           IInputOutputArray distCoeffs2,
           Size imageSize,
           IOutputArray r,
           IOutputArray t,
           IOutputArray e,
           IOutputArray f,
           CvEnum.CalibType flags,
           MCvTermCriteria termCrit)
        {
            System.Diagnostics.Debug.Assert(
               objectPoints.Length == imagePoints1.Length && objectPoints.Length == imagePoints2.Length,
               "The number of images for objects points should be equal to the number of images for image points");

            using (VectorOfVectorOfPoint3D32F objectPointVec = new VectorOfVectorOfPoint3D32F(objectPoints))
            using (VectorOfVectorOfPointF imagePoints1Vec = new VectorOfVectorOfPointF(imagePoints1))
            using (VectorOfVectorOfPointF imagePoints2Vec = new VectorOfVectorOfPointF(imagePoints2))
            {
                return CvInvoke.StereoCalibrate(
                   objectPointVec,
                   imagePoints1Vec,
                   imagePoints2Vec,
                   cameraMatrix1,
                   distCoeffs1,
                   cameraMatrix2,
                   distCoeffs2,
                   imageSize,
                   r,
                   t,
                   e,
                   f,
                   flags,
                   termCrit);
            }
        }

        /// <summary>
        /// Estimates transformation between the 2 cameras making a stereo pair. If we have a stereo camera, where the relative position and orientatation of the 2 cameras is fixed, and if we computed poses of an object relative to the fist camera and to the second camera, (R1, T1) and (R2, T2), respectively (that can be done with cvFindExtrinsicCameraParams2), obviously, those poses will relate to each other, i.e. given (R1, T1) it should be possible to compute (R2, T2) - we only need to know the position and orientation of the 2nd camera relative to the 1st camera. That's what the described function does. It computes (R, T) such that:
        /// R2=R*R1,
        /// T2=R*T1 + T
        /// </summary>
        /// <param name="objectPoints">The joint matrix of object points, 3xN or Nx3, where N is the total number of points in all views</param>
        /// <param name="imagePoints1">The joint matrix of corresponding image points in the views from the 1st camera, 2xN or Nx2, where N is the total number of points in all views</param>
        /// <param name="imagePoints2">The joint matrix of corresponding image points in the views from the 2nd camera, 2xN or Nx2, where N is the total number of points in all views</param>
        /// <param name="cameraMatrix1">The input/output camera matrices [fxk 0 cxk; 0 fyk cyk; 0 0 1]. If CV_CALIB_USE_INTRINSIC_GUESS or CV_CALIB_FIX_ASPECT_RATIO are specified, some or all of the elements of the matrices must be initialized</param>
        /// <param name="distCoeffs1">The input/output vectors of distortion coefficients for each camera, 4x1, 1x4, 5x1 or 1x5</param>
        /// <param name="cameraMatrix2">The input/output camera matrices [fxk 0 cxk; 0 fyk cyk; 0 0 1]. If CV_CALIB_USE_INTRINSIC_GUESS or CV_CALIB_FIX_ASPECT_RATIO are specified, some or all of the elements of the matrices must be initialized</param>
        /// <param name="distCoeffs2">The input/output vectors of distortion coefficients for each camera, 4x1, 1x4, 5x1 or 1x5</param>
        /// <param name="imageSize">Size of the image, used only to initialize intrinsic camera matrix</param>
        /// <param name="r">The rotation matrix between the 1st and the 2nd cameras' coordinate systems </param>
        /// <param name="t">The translation vector between the cameras' coordinate systems</param>
        /// <param name="e">The optional output essential matrix</param>
        /// <param name="f">The optional output fundamental matrix </param>
        /// <param name="termCrit">Termination criteria for the iterative optimization algorithm</param>
        /// <param name="flags">The calibration flags</param>
        /// <returns>The final value of the re-projection error.</returns>
        public static double StereoCalibrate(
           IInputArray objectPoints,
           IInputArray imagePoints1,
           IInputArray imagePoints2,
           IInputOutputArray cameraMatrix1,
           IInputOutputArray distCoeffs1,
           IInputOutputArray cameraMatrix2,
           IInputOutputArray distCoeffs2,
           Size imageSize,
           IOutputArray r,
           IOutputArray t,
           IOutputArray e,
           IOutputArray f,
           CvEnum.CalibType flags,
           MCvTermCriteria termCrit)
        {
            using (InputArray iaObjectPoints = objectPoints.GetInputArray())
            using (InputArray iaImagePoints1 = imagePoints1.GetInputArray())
            using (InputArray iaImagePoints2 = imagePoints2.GetInputArray())
            using (InputOutputArray ioaCameraMatrix1 = cameraMatrix1.GetInputOutputArray())
            using (InputOutputArray ioaCameraMatrix2 = cameraMatrix2.GetInputOutputArray())
            using (InputOutputArray ioaDistCoeffs1 = distCoeffs1.GetInputOutputArray())
            using (InputOutputArray ioaDistCoeffs2 = distCoeffs2.GetInputOutputArray())
            using (OutputArray oaR = r.GetOutputArray())
            using (OutputArray oaT = t.GetOutputArray())
            using (OutputArray oaE = e.GetOutputArray())
            using (OutputArray oaF = f.GetOutputArray())
                return cveStereoCalibrate(
                   iaObjectPoints, iaImagePoints1, iaImagePoints2,
                   ioaCameraMatrix1, ioaDistCoeffs1,
                   ioaCameraMatrix2, ioaDistCoeffs2,
                   ref imageSize,
                   oaR, oaT, oaE, oaF,
                   flags, ref termCrit);
        }

        [DllImport(ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        private static extern double cveStereoCalibrate(
           IntPtr objectPoints,
           IntPtr imagePoints1,
           IntPtr imagePoints2,
           IntPtr cameraMatrix1,
           IntPtr distCoeffs1,
           IntPtr cameraMatrix2,
           IntPtr distCoeffs2,
           ref Size imageSize,
           IntPtr r,
           IntPtr t,
           IntPtr e,
           IntPtr f,
           CvEnum.CalibType flags,
           ref MCvTermCriteria termCrit);


        /// <summary>
        /// Finds subpixel-accurate positions of the chessboard corners
        /// </summary>
        /// <param name="image">Source chessboard view; it must be 8-bit grayscale or color image</param>
        /// <param name="corners">Pointer to the output array of corners(PointF) detected</param>
        /// <param name="regionSize">region size</param>
        /// <returns>True if successful</returns>
        public static bool Find4QuadCornerSubpix(IInputArray image, IInputOutputArray corners, Size regionSize)
        {
            using (InputArray iaImage = image.GetInputArray())
            using (InputOutputArray ioaCorners = corners.GetInputOutputArray())
            {
                return cveFind4QuadCornerSubpix(iaImage, ioaCorners, ref regionSize);
            }
        }

        [DllImport(ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        [return: MarshalAs(CvInvoke.BoolMarshalType)]
        private static extern bool cveFind4QuadCornerSubpix(IntPtr image, IntPtr corners, ref Size regionSize);


        /// <summary>
        /// Attempts to determine whether the input image is a view of the chessboard pattern and locate internal chessboard corners
        /// </summary>
        /// <param name="image">Source chessboard view; it must be 8-bit grayscale or color image</param>
        /// <param name="patternSize">The number of inner corners per chessboard row and column</param>
        /// <param name="corners">Pointer to the output array of corners(PointF) detected</param>
        /// <param name="flags">Various operation flags</param>
        /// <returns>True if all the corners have been found and they have been placed in a certain order (row by row, left to right in every row), otherwise, if the function fails to find all the corners or reorder them, it returns 0</returns>
        /// <remarks>The coordinates detected are approximate, and to determine their position more accurately, the user may use the function cvFindCornerSubPix</remarks>
        public static bool FindChessboardCorners(IInputArray image, Size patternSize, IOutputArray corners,
           CvEnum.CalibCbType flags = CvEnum.CalibCbType.AdaptiveThresh | CvEnum.CalibCbType.NormalizeImage)
        {
            using (InputArray iaImage = image.GetInputArray())
            using (OutputArray oaCorners = corners.GetOutputArray())
                return cveFindChessboardCorners(iaImage, ref patternSize, oaCorners, flags);
        }

        [DllImport(ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        [return: MarshalAs(CvInvoke.BoolMarshalType)]
        private static extern bool cveFindChessboardCorners(IntPtr image, ref Size patternSize, IntPtr corners,
           CvEnum.CalibCbType flags);

        /// <summary>
        /// Filters off small noise blobs (speckles) in the disparity map.
        /// </summary>
        /// <param name="img">The input 16-bit signed disparity image</param>
        /// <param name="newVal">The disparity value used to paint-off the speckles</param>
        /// <param name="maxSpeckleSize">The maximum speckle size to consider it a speckle. Larger blobs are not affected by the algorithm</param>
        /// <param name="maxDiff">Maximum difference between neighbor disparity pixels to put them into the same blob. Note that since StereoBM, StereoSGBM and may be other algorithms return a fixed-point disparity map, where disparity values are multiplied by 16, this scale factor should be taken into account when specifying this parameter value.</param>
        /// <param name="buf">The optional temporary buffer to avoid memory allocation within the function.</param>
        public static void FilterSpeckles(IInputOutputArray img, double newVal, int maxSpeckleSize, double maxDiff, IInputOutputArray buf = null)
        {
            using (InputOutputArray ioaImg = img.GetInputOutputArray())
            using (InputOutputArray ioaBuf = buf == null ? InputOutputArray.GetEmpty() : buf.GetInputOutputArray())
            {
                cveFilterSpeckles(ioaImg, newVal, maxSpeckleSize, maxDiff, ioaBuf);
            }
        }

        [DllImport(ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        private static extern void cveFilterSpeckles(IntPtr img, double newVal, int maxSpeckleSize, double maxDiff, IntPtr buf);

        /// <summary>
        /// Finds the positions of internal corners of the chessboard using a sector based approach.
        /// </summary>
        /// <param name="image">Source chessboard view. It must be an 8-bit grayscale or color image.</param>
        /// <param name="patternSize">Number of inner corners per a chessboard row and column ( patternSize = cv::Size(points_per_row,points_per_colum) = cv::Size(columns,rows) ).</param>
        /// <param name="corners">Output array of detected corners.</param>
        /// <param name="flags">Various operation flags</param>
        /// <returns>True if chessboard corners found</returns>
        public static bool FindChessboardCornersSB(
            IInputArray image,
            Size patternSize,
            IOutputArray corners,
            CvEnum.CalibCbType flags = CalibCbType.Default)
        {
            using (InputArray iaImage = image.GetInputArray())
            using (OutputArray oaCorners = corners.GetOutputArray())
            {
                return cveFindChessboardCornersSB(iaImage, ref patternSize, oaCorners, flags);
            }
        }
        [DllImport(ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        [return: MarshalAs(BoolMarshalType)]
        private static extern bool cveFindChessboardCornersSB(IntPtr image, ref Size patternSize, IntPtr corners, CvEnum.CalibCbType flags);

        /// <summary>
        /// Draws the individual chessboard corners detected (as red circles) in case if the board was not found (pattern_was_found=0) or the colored corners connected with lines when the board was found (pattern_was_found != 0). 
        /// </summary>
        /// <param name="image">The destination image; it must be 8-bit color image</param>
        /// <param name="patternSize">The number of inner corners per chessboard row and column</param>
        /// <param name="corners">The array of corners detected</param>
        /// <param name="patternWasFound">Indicates whether the complete board was found (!=0) or not (=0). One may just pass the return value cvFindChessboardCorners here. </param>
        public static void DrawChessboardCorners(IInputOutputArray image, Size patternSize, IInputArray corners,
           bool patternWasFound)
        {
            using (InputOutputArray ioaImage = image.GetInputOutputArray())
            using (InputArray iaCorners = corners.GetInputArray())
                cveDrawChessboardCorners(ioaImage, ref patternSize, iaCorners, patternWasFound);
        }

        [DllImport(ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        private static extern void cveDrawChessboardCorners(IntPtr image, ref Size patternSize, IntPtr corners,
           [MarshalAs(CvInvoke.BoolMarshalType)] bool patternWasFound);

        #endregion

        /*
        #region Pose Estimation
        /// <summary>
        /// Allocates memory for the object structure and computes the object inverse matrix. 
        /// </summary>
        /// <remarks>The preprocessed object data is stored in the structure CvPOSITObject, internal for OpenCV, which means that the user cannot directly access the structure data. The user may only create this structure and pass its pointer to the function. 
        /// Object is defined as a set of points given in a coordinate system. The function cvPOSIT computes a vector that begins at a camera-related coordinate system center and ends at the points[0] of the object. 
        /// Once the work with a given object is finished, the function cvReleasePOSITObject must be called to free memory</remarks>
        /// <param name="points3D">A two dimensional array contains the points of the 3D object model, the second dimension must be 3. </param>
        /// <param name="pointCount">Number of object points</param>
        /// <returns>A pointer to the CvPOSITObject</returns>
        [DllImport(OpencvCalib3dLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        public static extern IntPtr cvCreatePOSITObject(float[,] points3D, int pointCount);


        /// <summary>
        /// Implements POSIT algorithm. Image coordinates are given in a camera-related coordinate system. The focal length may be retrieved using camera calibration functions. At every iteration of the algorithm new perspective projection of estimated pose is computed. 
        /// </summary>
        /// <remarks>Difference norm between two projections is the maximal distance between corresponding points. </remarks>
        /// <param name="positObject">Pointer to the object structure</param>
        /// <param name="imagePoints">2D array to the object points projections on the 2D image plane, the second dimension must be 2.</param>
        /// <param name="focalLength">Focal length of the camera used</param>
        /// <param name="criteria">Termination criteria of the iterative POSIT algorithm. The parameter criteria.epsilon serves to stop the algorithm if the difference is small.</param>
        /// <param name="rotationMatrix">A vector which contains the 9 elements of the 3x3 rotation matrix</param>
        /// <param name="translationVector">Translation vector (3x1)</param>
        [DllImport(OpencvCalib3DLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        public static extern void cvPOSIT(
           IntPtr positObject, float[,] imagePoints, double focalLength,
           MCvTermCriteria criteria, 
           float[] rotationMatrix, float[] translationVector);


        /// <summary>
        /// Implements POSIT algorithm. Image coordinates are given in a camera-related coordinate system. The focal length may be retrieved using camera calibration functions. At every iteration of the algorithm new perspective projection of estimated pose is computed. 
        /// </summary>
        /// <remarks>Difference norm between two projections is the maximal distance between corresponding points. </remarks>
        /// <param name="positObject">Pointer to the object structure</param>
        /// <param name="imagePoints">2D array to the object points projections on the 2D image plane, the second dimension must be 2.</param>
        /// <param name="focalLength">Focal length of the camera used</param>
        /// <param name="criteria">Termination criteria of the iterative POSIT algorithm. The parameter criteria.epsilon serves to stop the algorithm if the difference is small.</param>
        /// <param name="rotationMatrix">A vector which contains the 9 elements of the 3x3 rotation matrix</param>
        /// <param name="translationVector">Translation vector (3x1)</param>
  #if ANDROID
        public static void cvPOSIT(
           IntPtr positObject, IntPtr imagePoints, double focalLength,
           MCvTermCriteria criteria, 
           IntPtr rotationMatrix, IntPtr translationVector)
        {
           cvPOSIT(positObject, imagePoints, focalLength, criteria.Type, criteria.MaxIter, criteria.Epsilon, rotationMatrix, translationVector);
        }

        [DllImport(OpencvCalib3dLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        private static extern void cvPOSIT(
           IntPtr positObject, IntPtr imagePoints, double focalLength,
           CvEnum.TermCritType type,
           int maxIter,
           double epsilon,
           IntPtr rotationMatrix, IntPtr translationVector);
  #else
        [DllImport(OpencvCalib3dLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        public static extern void cvPOSIT(
           IntPtr positObject, IntPtr imagePoints, double focalLength,
           MCvTermCriteria criteria,
           IntPtr rotationMatrix, IntPtr translationVector);
  #endif

        /// <summary>
        /// The function cvReleasePOSITObject releases memory previously allocated by the function cvCreatePOSITObject. 
        /// </summary>
        /// <param name="positObject">pointer to CvPOSIT structure</param>
        [DllImport(OpencvCalib3dLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        public static extern void cvReleasePOSITObject(ref IntPtr positObject);
        #endregion
        */




        /// <summary>
        /// Estimates the sharpness of a detected chessboard.
        /// Image sharpness, as well as brightness, are a critical parameter for accuracte camera calibration. For accessing these parameters for filtering out problematic calibraiton images, this method calculates edge profiles by traveling from black to white chessboard cell centers. Based on this, the number of pixels is calculated required to transit from black to white. This width of the transition area is a good indication of how sharp the chessboard is imaged and should be below ~3.0 pixels.
        /// </summary>
        /// <param name="image">Gray image used to find chessboard corners</param>
        /// <param name="patternSize">Size of a found chessboard pattern</param>
        /// <param name="corners">Corners found by findChessboardCorners(SB)</param>
        /// <param name="riseDistance">Rise distance 0.8 means 10% ... 90% of the final signal strength</param>
        /// <param name="vertical">By default edge responses for horizontal lines are calculated</param>
        /// <param name="sharpness">Optional output array with a sharpness value for calculated edge responses</param>
        /// <returns>Scalar(average sharpness, average min brightness, average max brightness,0)</returns>
        public static MCvScalar EstimateChessboardSharpness(
            IInputArray image,
            Size patternSize,
            IInputArray corners,
            float riseDistance = 0.8f,
            bool vertical = false,
            IOutputArray sharpness = null)
        {
            MCvScalar result = new MCvScalar();
            using (InputArray iaImage = image.GetInputArray())
            using (InputArray iaCorners = corners.GetInputArray())
            using (OutputArray oaSharpness = (sharpness == null ? OutputArray.GetEmpty() : sharpness.GetOutputArray()))
                cveEstimateChessboardSharpness(
                    iaImage,
                    ref patternSize,
                    iaCorners,
                    riseDistance,
                    vertical,
                    oaSharpness,
                    ref result
                    );
            return result;
        }

        [DllImport(ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        private static extern void cveEstimateChessboardSharpness(
            IntPtr image,
            ref Size patternSize,
            IntPtr corners,
            float riseDistance,
            [MarshalAs(BoolMarshalType)]
            bool vertical,
            IntPtr sharpness,
            ref MCvScalar result);

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

    }
}