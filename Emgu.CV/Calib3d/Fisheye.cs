//----------------------------------------------------------------------------
//  Copyright (C) 2004-2022 by EMGU Corporation. All rights reserved.       
//----------------------------------------------------------------------------

using System;
using System.Runtime.InteropServices;
using System.Drawing;
using Emgu.CV.CvEnum;
using Emgu.CV.Structure;
using Emgu.CV.Util;

namespace Emgu.CV
{
    public static partial class CvInvoke
    {
        [DllImport(ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern void cveFisheyeProjectPoints(
            IntPtr objectPoints,
            IntPtr imagePoints,
            IntPtr rvec,
            IntPtr tvec,
            IntPtr K,
            IntPtr D,
            double alpha,
            IntPtr jacobian);


        [DllImport(ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern void cveFisheyeDistortPoints(
            IntPtr undistored, 
            IntPtr distorted, 
            IntPtr K, 
            IntPtr D,
            double alpha);

        [DllImport(ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern void cveFisheyeUndistortPoints(
            IntPtr distorted, 
            IntPtr undistorted, 
            IntPtr K, 
            IntPtr D,
            IntPtr R, 
            IntPtr P);


        [DllImport(ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern void cveFisheyeInitUndistortRectifyMap(
            IntPtr K, 
            IntPtr D, 
            IntPtr R, 
            IntPtr P, 
            ref Size size,
            int m1Type, 
            IntPtr map1, 
            IntPtr map2);

        [DllImport(ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern void cveFisheyeUndistortImage(
            IntPtr distorted, 
            IntPtr undistored, 
            IntPtr K, 
            IntPtr D,
            IntPtr Knew, 
            ref Size newSize);

        [DllImport(ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern void cveFisheyeEstimateNewCameraMatrixForUndistortRectify(
           IntPtr K, 
           IntPtr D,
           ref Size imageSize, 
           IntPtr R, 
           IntPtr P, 
           double balance, 
           ref Size newSize, 
           double fovScale);

        [DllImport(ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern void cveFisheyeStereoRectify(
           IntPtr K1, IntPtr D1, IntPtr K2,
           IntPtr D2, ref Size imageSize,
           IntPtr R, IntPtr tvec, IntPtr R1, IntPtr R2, IntPtr P1,
           IntPtr P2, IntPtr Q, int flags,
           ref Size newImageSize, double balance, double fovScale);


        [DllImport(ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern double cveFisheyeCalibrate(
           IntPtr objectPoints, IntPtr imagePoints, ref Size imageSize,
           IntPtr K, IntPtr D, IntPtr rvecs, IntPtr tvecs, int flags,
           ref MCvTermCriteria criteria);

        [DllImport(ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern double cveFisheyeStereoCalibrate(
           IntPtr objectPoints, IntPtr imagePoints1,
           IntPtr imagePoints2, IntPtr K1, IntPtr D1, IntPtr K2, IntPtr D2,
           ref Size imageSize, IntPtr R, IntPtr T, int flags, ref MCvTermCriteria criteria);
    }

    /// <summary>
    /// Fisheye Camera model
    /// </summary>
    public static class Fisheye
    {

        /// <summary>
        /// Fisheye calibration flag.
        /// </summary>
        public enum CalibrationFlag
        {
            /// <summary>
            /// Default flag
            /// </summary>
            Default = 0,
            /// <summary>
            /// cameraMatrix contains valid initial values of fx, fy, cx, cy that are optimized further. Otherwise, (cx, cy) is initially set to the image center ( imageSize is used), and focal distances are computed in a least-squares fashion.
            /// </summary>
            UseIntrinsicGuess = 1,
            /// <summary>
            /// Extrinsic will be recomputed after each iteration of intrinsic optimization.
            /// </summary>
            RecomputeExtrinsic = 2,
            /// <summary>
            /// The functions will check validity of condition number.
            /// </summary>
            CheckCond = 4,
            /// <summary>
            /// Skew coefficient (alpha) is set to zero and stay zero.
            /// </summary>
            FixSkew = 8,
            /// <summary>
            /// Selected distortion coefficients are set to zeros and stay zero.
            /// </summary>
            FixK1 = 16,
            /// <summary>
            /// Selected distortion coefficients are set to zeros and stay zero.
            /// </summary>
            FixK2 = 32,
            /// <summary>
            /// Selected distortion coefficients are set to zeros and stay zero.
            /// </summary>
            FixK3 = 64,
            /// <summary>
            /// Selected distortion coefficients are set to zeros and stay zero.
            /// </summary>
            FixK4 = 128,
            /// <summary>
            /// Fix intrinsic
            /// </summary>
            FixIntrinsic = 256
        }

        /// <summary>
        /// Projects points using fisheye model. The function computes projections of 3D points to the image plane given intrinsic and extrinsic camera parameters. Optionally, the function computes Jacobians - matrices of partial derivatives of image points coordinates (as functions of all the input parameters) with respect to the particular parameters, intrinsic and/or extrinsic.
        /// </summary>
        /// <param name="objectPoints">Array of object points, 1xN/Nx1 3-channel (or vector&lt;Point3f&gt; ), where N is the number of points in the view.</param>
        /// <param name="imagePoints">Output array of image points, 2xN/Nx2 1-channel or 1xN/Nx1 2-channel, or vector&lt;Point2f&gt;.</param>
        /// <param name="rvec">rotation vector</param>
        /// <param name="tvec">translation vector</param>
        /// <param name="K">Camera matrix</param>
        /// <param name="D">Input vector of distortion coefficients (k1,k2,k3,k4).</param>
        /// <param name="alpha">The skew coefficient.</param>
        /// <param name="jacobian">Optional output 2Nx15 jacobian matrix of derivatives of image points with respect to components of the focal lengths, coordinates of the principal point, distortion coefficients, rotation vector, translation vector, and the skew. In the old interface different components of the jacobian are returned via different output parameters.</param>
        public static void ProjectPoints(
            IInputArray objectPoints,
            IOutputArray imagePoints,
            IInputArray rvec,
            IInputArray tvec,
            IInputArray K,
            IInputArray D,
            double alpha = 0,
            IOutputArray jacobian = null)
        {
            using (InputArray iaObjectPoints = objectPoints.GetInputArray())
            using (OutputArray oaImagePoints = imagePoints.GetOutputArray())
            using (InputArray iaRvec = rvec.GetInputArray())
            using (InputArray iaTvec = tvec.GetInputArray())
            using (InputArray iaK = K.GetInputArray())
            using (InputArray iaD = D.GetInputArray())
            using (OutputArray oaJacobian = jacobian == null ? OutputArray.GetEmpty() : jacobian.GetOutputArray())
            {
                CvInvoke.cveFisheyeProjectPoints(iaObjectPoints, oaImagePoints, iaRvec, iaTvec, iaK, iaD, alpha, oaJacobian);
            }
        }

        /// <summary>
        /// Distorts 2D points using fisheye model.
        /// </summary>
        /// <param name="undistored">Array of object points, 1xN/Nx1 2-channel (or vector&lt;Point2f&gt; ), where N is the number of points in the view.</param>
        /// <param name="distorted">	Output array of image points, 1xN/Nx1 2-channel, or vector&lt;Point2f&gt; .</param>
        /// <param name="K">Camera matrix</param>
        /// <param name="D">Input vector of distortion coefficients (k1,k2,k3,k4).</param>
        /// <param name="alpha">The skew coefficient.</param>
        public static void DistortPoints(IInputArray undistored, IOutputArray distorted, IInputArray K, IInputArray D,
           double alpha = 0)
        {
            using (InputArray iaUndistorted = undistored.GetInputArray())
            using (OutputArray oaDistorted = distorted.GetOutputArray())
            using (InputArray iaK = K.GetInputArray())
            using (InputArray iaD = D.GetInputArray())
            {
                CvInvoke.cveFisheyeDistortPoints(iaUndistorted, oaDistorted, iaK, iaD, alpha);
            }
        }

        /// <summary>
        /// Transforms an image to compensate for fisheye lens distortion.
        /// </summary>
        /// <param name="distorted">Array of object points, 1xN/Nx1 2-channel (or vector&lt;Point2f&gt; ), where N is the number of points in the view.</param>
        /// <param name="undistorted">Output array of image points, 1xN/Nx1 2-channel, or vector&lt;Point2f&gt;.</param>
        /// <param name="K">Camera matrix</param>
        /// <param name="D">Input vector of distortion coefficients (k1,k2,k3,k4).</param>
        /// <param name="R">Rectification transformation in the object space: 3x3 1-channel, or vector: 3x1/1x3 1-channel or 1x1 3-channel</param>
        /// <param name="P">New camera matrix (3x3) or new projection matrix (3x4)</param>
        public static void UndistortPoints(IInputArray distorted, IOutputArray undistorted, IInputArray K, IInputArray D,
           IInputArray R = null, IInputArray P = null)
        {
            using (InputArray iaDistorted = distorted.GetInputArray())
            using (OutputArray oaUndistorted = undistorted.GetOutputArray())
            using (InputArray iaK = K.GetInputArray())
            using (InputArray iaD = D.GetInputArray())
            using (InputArray iaR = R == null ? InputArray.GetEmpty() : R.GetInputArray())
            using (InputArray iaP = P == null ? InputArray.GetEmpty() : P.GetInputArray())
            {
                CvInvoke.cveFisheyeUndistortPoints(iaDistorted, oaUndistorted, iaK, iaD, iaR, iaP);
            }
        }

        /// <summary>
        /// Computes undistortion and rectification maps for image transform by cv::remap(). If D is empty zero distortion is used, if R or P is empty identity matrixes are used.
        /// </summary>
        /// <param name="K">Camera matrix</param>
        /// <param name="D">	Input vector of distortion coefficients (k1,k2,k3,k4).</param>
        /// <param name="R">Rectification transformation in the object space: 3x3 1-channel, or vector: 3x1/1x3 1-channel or 1x1 3-channel</param>
        /// <param name="P">New camera matrix (3x3) or new projection matrix (3x4)</param>
        /// <param name="size">Undistorted image size.</param>
        /// <param name="depthType">Depth type of the first output map. (The combination with <paramref name="channels"/> can be one of CV_32FC1 or CV_16SC2)</param>
        /// <param name="channels">Number of channels of the first output map. (The combination with <paramref name="depthType"/> can be one of CV_32FC1 or CV_16SC2)</param>
        /// <param name="map1">The first output map.</param>
        /// <param name="map2">The second output map.</param>
        public static void InitUndistortRectifyMap(
            IInputArray K, 
            IInputArray D, 
            IInputArray R, 
            IInputArray P, 
            Size size,
            CvEnum.DepthType depthType,
            int channels,
            IOutputArray map1, 
            IOutputArray map2)
        {
            using (InputArray iaK = K.GetInputArray())
            using (InputArray iaD = D.GetInputArray())
            using (InputArray iaR = R.GetInputArray())
            using (InputArray iaP = P.GetInputArray())
            using (OutputArray oaMap1 = map1.GetOutputArray())
            using (OutputArray oaMap2 = map2.GetOutputArray())
            {
                CvInvoke.cveFisheyeInitUndistortRectifyMap(
                    iaK, 
                    iaD, 
                    iaR, 
                    iaP, 
                    ref size, 
                    CvInvoke.MakeType(depthType, channels), 
                    oaMap1, 
                    oaMap2);
            }
        }

        /// <summary>
        /// Transforms an image to compensate for fisheye lens distortion. The function is simply a combination of fisheye::initUndistortRectifyMap (with unity R ) and remap (with bilinear interpolation). 
        /// </summary>
        /// <param name="distorted">Image with fisheye lens distortion.</param>
        /// <param name="undistored">Output image with compensated fisheye lens distortion.</param>
        /// <param name="K">Camera matrix </param>
        /// <param name="D">Input vector of distortion coefficients (k1,k2,k3,k4).</param>
        /// <param name="Knew">Camera matrix of the distorted image. By default, it is the identity matrix but you may additionally scale and shift the result by using a different matrix.</param>
        /// <param name="newSize">The function transforms an image to compensate radial and tangential lens distortion.</param>
        public static void UndistortImage(IInputArray distorted, IOutputArray undistored, IInputArray K, IInputArray D,
           IInputArray Knew = null, Size newSize = new Size())
        {
            using (InputArray iaDistorted = distorted.GetInputArray())
            using (OutputArray oaUndistorted = undistored.GetOutputArray())
            using (InputArray iaK = K.GetInputArray())
            using (InputArray iaD = D.GetInputArray())
            using (InputArray iaKnew = Knew == null ? InputArray.GetEmpty() : Knew.GetInputArray())
            {
                CvInvoke.cveFisheyeUndistortImage(iaDistorted, oaUndistorted, iaK, iaD, iaKnew, ref newSize);
            }
        }

        /// <summary>
        /// Estimates new camera matrix for undistortion or rectification.
        /// </summary>
        /// <param name="K">Camera matrix</param>
        /// <param name="D">Input vector of distortion coefficients (k1,k2,k3,k4).</param>
        /// <param name="imageSize">Size of the image</param>
        /// <param name="R">Rectification transformation in the object space: 3x3 1-channel, or vector: 3x1/1x3 1-channel or 1x1 3-channel</param>
        /// <param name="P">New camera matrix (3x3) or new projection matrix (3x4)</param>
        /// <param name="balance">Sets the new focal length in range between the min focal length and the max focal length. Balance is in range of [0, 1]</param>
        /// <param name="newSize">the new size</param>
        /// <param name="fovScale">Divisor for new focal length.</param>
        public static void EstimateNewCameraMatrixForUndistortRectify(
            IInputArray K,
            IInputArray D,
            Size imageSize,
            IInputArray R,
            IOutputArray P,
            double balance = 0.0,
            Size newSize = new Size(),
            double fovScale = 1.0)
        {
            using (InputArray iaK = K.GetInputArray())
            using (InputArray iaD = D.GetInputArray())
            using (InputArray iaR = R.GetInputArray())
            using (OutputArray oaP = P.GetOutputArray())
            {
                CvInvoke.cveFisheyeEstimateNewCameraMatrixForUndistortRectify(iaK, iaD, ref imageSize, iaR, oaP, balance,
                   ref newSize, fovScale);
            }
        }

        /// <summary>
        /// Stereo rectification for fisheye camera model.
        /// </summary>
        /// <param name="K1">First camera matrix.</param>
        /// <param name="D1">First camera distortion parameters.</param>
        /// <param name="K2">Second camera matrix.</param>
        /// <param name="D2">Second camera distortion parameters.</param>
        /// <param name="imageSize">Size of the image used for stereo calibration.</param>
        /// <param name="R">Rotation matrix between the coordinate systems of the first and the second cameras.</param>
        /// <param name="tvec">Translation vector between coordinate systems of the cameras.</param>
        /// <param name="R1">Output 3x3 rectification transform (rotation matrix) for the first camera.</param>
        /// <param name="R2">Output 3x3 rectification transform (rotation matrix) for the second camera.</param>
        /// <param name="P1">Output 3x4 projection matrix in the new (rectified) coordinate systems for the first camera.</param>
        /// <param name="P2">Output 3x4 projection matrix in the new (rectified) coordinate systems for the second camera.</param>
        /// <param name="Q">	Output 4×4 disparity-to-depth mapping matrix (see reprojectImageTo3D ).</param>
        /// <param name="flags">Operation flags that may be zero or ZeroDisparity . If the flag is set, the function makes the principal points of each camera have the same pixel coordinates in the rectified views. And if the flag is not set, the function may still shift the images in the horizontal or vertical direction (depending on the orientation of epipolar lines) to maximize the useful image area.</param>
        /// <param name="newImageSize">New image resolution after rectification. The same size should be passed to initUndistortRectifyMap. When (0,0) is passed (default), it is set to the original imageSize . Setting it to larger value can help you preserve details in the original image, especially when there is a big radial distortion.</param>
        /// <param name="balance">Sets the new focal length in range between the min focal length and the max focal length. Balance is in range of [0, 1].</param>
        /// <param name="fovScale">Divisor for new focal length.</param>
        public static void StereoRectify(IInputArray K1, IInputArray D1, IInputArray K2, IInputArray D2, Size imageSize,
           IInputArray R, IInputArray tvec, IOutputArray R1, IOutputArray R2, IOutputArray P1, IOutputArray P2,
           IOutputArray Q, int flags,
           Size newImageSize = new Size(), double balance = 0.0, double fovScale = 1.0)
        {
            using (InputArray iaK1 = K1.GetInputArray())
            using (InputArray iaD1 = D1.GetInputArray())
            using (InputArray iaK2 = K2.GetInputArray())
            using (InputArray iaD2 = D2.GetInputArray())
            using (InputArray iaR = R.GetInputArray())
            using (InputArray iaTvec = tvec.GetInputArray())
            using (OutputArray oaR1 = R1.GetOutputArray())
            using (OutputArray oaR2 = R2.GetOutputArray())
            using (OutputArray oaP1 = P1.GetOutputArray())
            using (OutputArray oaP2 = P2.GetOutputArray())
            using (OutputArray oaQ = Q.GetOutputArray())
            {
                CvInvoke.cveFisheyeStereoRectify(iaK1, iaD1, iaK2, iaD2, ref imageSize, iaR, iaTvec, oaR1, oaR2, oaP1, oaP2,
                   oaQ, flags, ref newImageSize, balance, fovScale);
            }
        }

        /// <summary>
        /// Performs camera calibration.
        /// </summary>
        /// <param name="objectPoints">vector of vectors of calibration pattern points in the calibration pattern coordinate space.</param>
        /// <param name="imagePoints">vector of vectors of the projections of calibration pattern points. imagePoints.size() and objectPoints.size() and imagePoints[i].size() must be equal to objectPoints[i].size() for each i.</param>
        /// <param name="imageSize">Size of the image used only to initialize the intrinsic camera matrix.</param>
        /// <param name="K">Output 3x3 floating-point camera matrix. If UseIntrisicGuess is specified, some or all of fx, fy, cx, cy must be initialized before calling the function. </param>
        /// <param name="D">Output vector of distortion coefficients (k1,k2,k3,k4).</param>
        /// <param name="rvecs">Output vector of rotation vectors (see Rodrigues ) estimated for each pattern view. That is, each k-th rotation vector together with the corresponding k-th translation vector (see the next output parameter description) brings the calibration pattern from the model coordinate space (in which object points are specified) to the world coordinate space, that is, a real position of the calibration pattern in the k-th pattern view (k=0.. M -1).</param>
        /// <param name="tvecs">Output vector of translation vectors estimated for each pattern view.</param>
        /// <param name="flags">Different flags</param>
        /// <param name="criteria">Termination criteria for the iterative optimization algorithm.</param>
        /// <returns>The calibration error</returns>
        public static double Calibrate(IInputArray objectPoints, IInputArray imagePoints, Size imageSize,
           IInputOutputArray K, IInputOutputArray D, IOutputArray rvecs, IOutputArray tvecs, CalibrationFlag flags,
           MCvTermCriteria criteria)
        {
            using (InputArray iaObjectPoints = objectPoints.GetInputArray())
            using (InputArray iaImagePoints = imagePoints.GetInputArray())
            using (InputOutputArray ioaK = K.GetInputOutputArray())
            using (InputOutputArray ioaD = D.GetInputOutputArray())
            using (OutputArray oaRvecs = rvecs.GetOutputArray())
            using (OutputArray oaTvecs = tvecs.GetOutputArray())
            {
                return CvInvoke.cveFisheyeCalibrate(
                    iaObjectPoints,
                    iaImagePoints,
                    ref imageSize,
                    ioaK,
                    ioaD,
                    oaRvecs,
                    oaTvecs,
                    (int)flags,
                    ref criteria);
            }
        }

        /// <summary>
        /// Performs stereo calibration.
        /// </summary>
        /// <param name="objectPoints">Vector of vectors of the calibration pattern points.</param>
        /// <param name="imagePoints1">Vector of vectors of the projections of the calibration pattern points, observed by the first camera.</param>
        /// <param name="imagePoints2">Vector of vectors of the projections of the calibration pattern points, observed by the second camera.</param>
        /// <param name="K1">Input/output first camera matrix.If FixIntrinsic is specified, some or all of the matrix components must be initialized.</param>
        /// <param name="D1">Input/output vector of distortion coefficients (k1,k2,k3,k4) of 4 elements.</param>
        /// <param name="K2">Input/output second camera matrix. The parameter is similar to <paramref name="K1"/> </param>
        /// <param name="D2">Input/output lens distortion coefficients for the second camera. The parameter is similar to <paramref name="D1"/></param>
        /// <param name="imageSize">Size of the image used only to initialize intrinsic camera matrix.</param>
        /// <param name="R">Output rotation matrix between the 1st and the 2nd camera coordinate systems.</param>
        /// <param name="T">Output translation vector between the coordinate systems of the cameras.</param>
        /// <param name="flags">Fish eye calibration flags</param>
        /// <param name="criteria">Termination criteria for the iterative optimization algorithm.</param>
        /// <returns>The calibration error</returns>
        public static double StereoCalibrate(IInputArray objectPoints, IInputArray imagePoints1,
           IInputArray imagePoints2, IInputOutputArray K1, IInputOutputArray D1, IInputOutputArray K2,
           IInputOutputArray D2, Size imageSize, IOutputArray R, IOutputArray T, CalibrationFlag flags, MCvTermCriteria criteria)
        {
            using (InputArray iaObjectPoints = objectPoints.GetInputArray())
            using (InputArray iaImagePoints1 = imagePoints1.GetInputArray())
            using (InputArray iaImagePoints2 = imagePoints2.GetInputArray())
            using (InputOutputArray ioaK1 = K1.GetInputOutputArray())
            using (InputOutputArray ioaD1 = D1.GetInputOutputArray())
            using (InputOutputArray ioaK2 = K2.GetInputOutputArray())
            using (InputOutputArray ioaD2 = D2.GetInputOutputArray())
            using (OutputArray oaR = R.GetOutputArray())
            using (OutputArray oaT = T.GetOutputArray())
            {
                return CvInvoke.cveFisheyeStereoCalibrate(
                    iaObjectPoints,
                    iaImagePoints1,
                    iaImagePoints2,
                    ioaK1,
                    ioaD1,
                    ioaK2,
                    ioaD2,
                    ref imageSize,
                    oaR,
                    oaT,
                    (int)flags,
                    ref criteria);
            }
        }
    }
}