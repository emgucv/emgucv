//----------------------------------------------------------------------------
//  Copyright (C) 2004-2020 by EMGU Corporation. All rights reserved.       
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
        /// <summary>
        /// Finds perspective transformation H=||h_ij|| between the source and the destination planes
        /// </summary>
        /// <param name="srcPoints">Point coordinates in the original plane</param>
        /// <param name="dstPoints">Point coordinates in the destination plane</param>
        /// <param name="method">FindHomography method</param>
        /// <param name="ransacReprojThreshold">
        /// The maximum allowed reprojection error to treat a point pair as an inlier. 
        /// The parameter is only used in RANSAC-based homography estimation. 
        /// E.g. if dst_points coordinates are measured in pixels with pixel-accurate precision, it makes sense to set this parameter somewhere in the range ~1..3
        /// </param>
        /// <param name="mask">Optional output mask set by a robust method ( CV_RANSAC or CV_LMEDS ). Note that the input mask values are ignored.</param>
        /// <returns>The 3x3 homography matrix if found. Null if not found.</returns>
        public static Mat FindHomography(
           PointF[] srcPoints,
           PointF[] dstPoints,
           CvEnum.RobustEstimationAlgorithm method = RobustEstimationAlgorithm.AllPoints,
           double ransacReprojThreshold = 3,
           IOutputArray mask = null)
        {
            GCHandle srcHandle = GCHandle.Alloc(srcPoints, GCHandleType.Pinned);
            GCHandle dstHandle = GCHandle.Alloc(dstPoints, GCHandleType.Pinned);
            try
            {
                using (
                   Mat srcPointMatrix = new Mat(srcPoints.Length, 2, DepthType.Cv32F, 1, srcHandle.AddrOfPinnedObject(), 8))
                using (
                   Mat dstPointMatrix = new Mat(dstPoints.Length, 2, DepthType.Cv32F, 1, dstHandle.AddrOfPinnedObject(), 8))
                {
                    return CvInvoke.FindHomography(srcPointMatrix, dstPointMatrix, method, ransacReprojThreshold, mask);
                }
            }
            finally
            {
                srcHandle.Free();
                dstHandle.Free();
            }
        }

        /// <summary>
        /// Finds perspective transformation H=||hij|| between the source and the destination planes
        /// </summary>
        /// <param name="srcPoints">Point coordinates in the original plane, 2xN, Nx2, 3xN or Nx3 array (the latter two are for representation in homogeneous coordinates), where N is the number of points. </param>
        /// <param name="dstPoints">Point coordinates in the destination plane, 2xN, Nx2, 3xN or Nx3 array (the latter two are for representation in homogeneous coordinates) </param>
        /// <param name="method">The type of the method</param>
        /// <param name="ransacReprojThreshold">The maximum allowed re-projection error to treat a point pair as an inlier. The parameter is only used in RANSAC-based homography estimation. E.g. if dst_points coordinates are measured in pixels with pixel-accurate precision, it makes sense to set this parameter somewhere in the range ~1..3</param>
        /// <param name="mask">The optional output mask set by a robust method (RANSAC or LMEDS). </param>
        /// <returns>Output 3x3 homography matrix. Homography matrix is determined up to a scale, thus it is normalized to make h33=1</returns>
        public static Mat FindHomography(
           IInputArray srcPoints,
           IInputArray dstPoints,
           CvEnum.RobustEstimationAlgorithm method = CvEnum.RobustEstimationAlgorithm.AllPoints,
           double ransacReprojThreshold = 3,
           IOutputArray mask = null)
        {
            Mat homography = new Mat();
            using (InputArray iaSrcPoints = srcPoints.GetInputArray())
            using (InputArray iaDstPoints = dstPoints.GetInputArray())
            using (OutputArray oaHomography = homography.GetOutputArray())
            using (OutputArray oaMask = mask == null ? OutputArray.GetEmpty() : mask.GetOutputArray())
                cveFindHomography(iaSrcPoints, iaDstPoints, oaHomography, method, ransacReprojThreshold, oaMask);
            return homography;
        }

        [DllImport(ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        private static extern void cveFindHomography(IntPtr srcPoints, IntPtr dstPoints, IntPtr homography,
           CvEnum.RobustEstimationAlgorithm method, double ransacReprojThreshold, IntPtr mask);

        /// <summary>
        /// Converts a rotation vector to rotation matrix or vice versa. Rotation vector is a compact representation of rotation matrix. Direction of the rotation vector is the rotation axis and the length of the vector is the rotation angle around the axis. 
        /// </summary>
        /// <param name="src">The input rotation vector (3x1 or 1x3) or rotation matrix (3x3). </param>
        /// <param name="dst">The output rotation matrix (3x3) or rotation vector (3x1 or 1x3), respectively</param>
        /// <param name="jacobian">Optional output Jacobian matrix, 3x9 or 9x3 - partial derivatives of the output array components w.r.t the input array components</param>
        public static void Rodrigues(IInputArray src, IOutputArray dst, IOutputArray jacobian = null)
        {
            using (InputArray iaSrc = src.GetInputArray())
            using (OutputArray oaDst = dst.GetOutputArray())
            using (OutputArray oaJacobian = jacobian == null ? OutputArray.GetEmpty() : jacobian.GetOutputArray())
                cveRodrigues(iaSrc, oaDst, oaJacobian);
        }

        [DllImport(ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        private static extern void cveRodrigues(IntPtr src, IntPtr dst, IntPtr jacobian);

        #region Epipolar Geometry, Stereo Correspondence
        /// <summary>
        /// Calculates an essential matrix from the corresponding points in two images.
        /// </summary>
        /// <param name="points1">Array of N (N &gt;= 5) 2D points from the first image. The point coordinates should be floating-point (single or double precision).</param>
        /// <param name="points2">Array of the second image points of the same size and format as points1</param>
        /// <param name="cameraMatrix">Camera matrix K=[[fx 0 cx][0 fy cy][0 0 1]]. Note that this function assumes that points1 and points2 are feature points from cameras with the same camera matrix.</param>
        /// <param name="method">Method for computing a fundamental matrix. RANSAC for the RANSAC algorithm. LMEDS for the LMedS algorithm</param>
        /// <param name="prob">Parameter used for the RANSAC or LMedS methods only. It specifies a desirable level of confidence (probability) that the estimated matrix is correct.</param>
        /// <param name="threshold">Parameter used for RANSAC. It is the maximum distance from a point to an epipolar line in pixels, beyond which the point is considered an outlier and is not used for computing the final fundamental matrix. It can be set to something like 1-3, depending on the accuracy of the point localization, image resolution, and the image noise.</param>
        /// <param name="mask">Output array of N elements, every element of which is set to 0 for outliers and to 1 for the other points. The array is computed only in the RANSAC and LMedS methods.</param>
        /// <returns>The essential mat</returns>
        public static Mat FindEssentialMat(IInputArray points1, IInputArray points2, IInputArray cameraMatrix,
           CvEnum.FmType method = CvEnum.FmType.Ransac, double prob = 0.999, double threshold = 1.0,
           IOutputArray mask = null)
        {
            using (InputArray iaPoints1 = points1.GetInputArray())
            using (InputArray iaPoints2 = points2.GetInputArray())
            using (InputArray iaCameraMatrix = cameraMatrix.GetInputArray())
            using (OutputArray oaMask = mask == null ? OutputArray.GetEmpty() : mask.GetOutputArray())
            {
                Mat essentialMat = new Mat();
                cveFindEssentialMat(iaPoints1, iaPoints2, iaCameraMatrix, method, prob, threshold, oaMask, essentialMat);
                return essentialMat;
            }
        }

        [DllImport(ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        private static extern void cveFindEssentialMat(IntPtr points1, IntPtr points2, IntPtr cameraMatrix, CvEnum.FmType method, double prob, double threshold, IntPtr mask, IntPtr essentialMat);

        /// <summary>
        /// Calculates fundamental matrix using one of four methods listed above and returns the number of fundamental matrices found (1 or 3) and 0, if no matrix is found. 
        /// </summary>
        /// <param name="points1">Array of N points from the first image. The point coordinates should be floating-point (single or double precision).</param>
        /// <param name="points2">Array of the second image points of the same size and format as points1 </param>
        /// <param name="method">Method for computing the fundamental matrix </param>
        /// <param name="param1">Parameter used for RANSAC. It is the maximum distance from a point to an epipolar line in pixels, beyond which the point is considered an outlier and is not used for computing the final fundamental matrix. It can be set to something like 1-3, depending on the accuracy of the point localization, image resolution, and the image noise.</param>
        /// <param name="param2">Parameter used for the RANSAC or LMedS methods only. It specifies a desirable level of confidence (probability) that the estimated matrix is correct.</param>
        /// <param name="mask">The optional pointer to output array of N elements, every element of which is set to 0 for outliers and to 1 for the "inliers", i.e. points that comply well with the estimated epipolar geometry. The array is computed only in RANSAC and LMedS methods. For other methods it is set to all 1.</param>
        /// <returns>The calculated fundamental matrix </returns>
        public static Mat FindFundamentalMat(IInputArray points1, IInputArray points2,
           CvEnum.FmType method = CvEnum.FmType.Ransac, double param1 = 3, double param2 = 0.99, IOutputArray mask = null)
        {
            Mat f = new Mat();
            using (InputArray iaPoints1 = points1.GetInputArray())
            using (InputArray iaPoints2 = points2.GetInputArray())
            using (OutputArray oaF = f.GetOutputArray())
            using (OutputArray oaMask = mask == null ? OutputArray.GetEmpty() : mask.GetOutputArray())
                cveFindFundamentalMat(iaPoints1, iaPoints2, oaF, method, param1, param2, oaMask);
            return f;
        }

        [DllImport(ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        private static extern void cveFindFundamentalMat(IntPtr points1, IntPtr points2, IntPtr dst, CvEnum.FmType method,
           double param1, double param2, IntPtr mask);


        /// <summary>
        /// For every point in one of the two images of stereo-pair the function cvComputeCorrespondEpilines finds equation of a line that contains the corresponding point (i.e. projection of the same 3D point) in the other image. Each line is encoded by a vector of 3 elements l=[a,b,c]^T, so that: 
        /// l^T*[x, y, 1]^T=0, or
        /// a*x + b*y + c = 0
        /// From the fundamental matrix definition (see cvFindFundamentalMatrix discussion), line l2 for a point p1 in the first image (which_image=1) can be computed as: 
        /// l2=F*p1 and the line l1 for a point p2 in the second image (which_image=1) can be computed as: 
        /// l1=F^T*p2Line coefficients are defined up to a scale. They are normalized (a2+b2=1) are stored into correspondent_lines
        /// </summary>
        /// <param name="points">The input points. 2xN, Nx2, 3xN or Nx3 array (where N number of points). Multi-channel 1xN or Nx1 array is also acceptable.</param>
        /// <param name="whichImage">Index of the image (1 or 2) that contains the points</param>
        /// <param name="fundamentalMatrix">Fundamental matrix </param>
        /// <param name="correspondentLines">Computed epilines, 3xN or Nx3 array </param>
        public static void ComputeCorrespondEpilines(IInputArray points, int whichImage, IInputArray fundamentalMatrix,
           IOutputArray correspondentLines)
        {
            using (InputArray iaPoints = points.GetInputArray())
            using (InputArray iaFundamentalMatrix = fundamentalMatrix.GetInputArray())
            using (OutputArray oaCorrespondentLines = correspondentLines.GetOutputArray())
                cveComputeCorrespondEpilines(iaPoints, whichImage, iaFundamentalMatrix, oaCorrespondentLines);
        }

        [DllImport(ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        private static extern void cveComputeCorrespondEpilines(
           IntPtr points,
           int whichImage,
           IntPtr fundamentalMatrix,
           IntPtr correspondentLines);

        /// <summary>
        /// Converts points from Euclidean to homogeneous space.
        /// </summary>
        /// <param name="src">Input vector of N-dimensional points.</param>
        /// <param name="dst">Output vector of N+1-dimensional points.</param>
        public static void ConvertPointsToHomogeneous(IInputArray src, IOutputArray dst)
        {
            using (InputArray iaSrc = src.GetInputArray())
            using (OutputArray oaDst = dst.GetOutputArray())
                cveConvertPointsToHomogeneous(iaSrc, oaDst);
        }

        [DllImport(ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        private static extern void cveConvertPointsToHomogeneous(IntPtr src, IntPtr dst);


        /// <summary>
        /// Converts points from homogeneous to Euclidean space.
        /// </summary>
        /// <param name="src">Input vector of N-dimensional points.</param>
        /// <param name="dst">Output vector of N-1-dimensional points.</param>
        public static void ConvertPointsFromHomogeneous(IInputArray src, IOutputArray dst)
        {
            using (InputArray iaSrc = src.GetInputArray())
            using (OutputArray oaDst = dst.GetOutputArray())
                cveConvertPointsFromHomogeneous(iaSrc, oaDst);
        }

        [DllImport(ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        private static extern void cveConvertPointsFromHomogeneous(IntPtr src, IntPtr dst);

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
        /// Returns the new camera matrix based on the free scaling parameter.
        /// </summary>
        /// <param name="cameraMatrix">	Input camera matrix.</param>
        /// <param name="distCoeffs">Input vector of distortion coefficients (k1,k2,p1,p2[,k3[,k4,k5,k6[,s1,s2,s3,s4[,?x,?y]]]]) of 4, 5, 8, 12 or 14 elements. If the vector is NULL/empty, the zero distortion coefficients are assumed.</param>
        /// <param name="imageSize">Original image size.</param>
        /// <param name="alpha">Free scaling parameter between 0 (when all the pixels in the undistorted image are valid) and 1 (when all the source image pixels are retained in the undistorted image).</param>
        /// <param name="newImgSize">Image size after rectification. By default,it is set to imageSize .</param>
        /// <param name="validPixROI">output rectangle that outlines all-good-pixels region in the undistorted image. </param>
        /// <param name="centerPrincipalPoint">indicates whether in the new camera matrix the principal point should be at the image center or not. By default, the principal point is chosen to best fit a subset of the source image (determined by alpha) to the corrected image.</param>
        /// <returns>The new camera matrix based on the free scaling parameter.</returns>
        public static Mat GetOptimalNewCameraMatrix(
            IInputArray cameraMatrix, IInputArray distCoeffs,
            Size imageSize, double alpha, Size newImgSize,
            ref Rectangle validPixROI,
            bool centerPrincipalPoint = false)
        {
            Mat m = new Mat();
            using (InputArray iaCameraMatrix = cameraMatrix.GetInputArray())
            using (InputArray iaDistCoeffs = distCoeffs == null ? InputArray.GetEmpty() : distCoeffs.GetInputArray())
            {
                cveGetOptimalNewCameraMatrix(iaCameraMatrix, iaDistCoeffs, ref imageSize, alpha, ref newImgSize, ref validPixROI, centerPrincipalPoint, m);
            }
            return m;
        }

        [DllImport(ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        private static extern void cveGetOptimalNewCameraMatrix(
            IntPtr cameraMatrix, IntPtr distCoeffs,
            ref Size imageSize, double alpha, ref Size newImgSize,
            ref Rectangle validPixROI,
            [MarshalAs(CvInvoke.BoolMarshalType)]
            bool centerPrincipalPoint,
            IntPtr newCameraMatrix);

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
        /// Computes projections of 3D points to the image plane given intrinsic and extrinsic camera parameters. Optionally, the function computes jacobians - matrices of partial derivatives of image points as functions of all the input parameters w.r.t. the particular parameters, intrinsic and/or extrinsic. The jacobians are used during the global optimization in cvCalibrateCamera2 and cvFindExtrinsicCameraParams2. The function itself is also used to compute back-projection error for with current intrinsic and extrinsic parameters.
        /// Note, that with intrinsic and/or extrinsic parameters set to special values, the function can be used to compute just extrinsic transformation or just intrinsic transformation (i.e. distortion of a sparse set of points). 
        /// </summary>
        /// <param name="objectPoints">The array of object points, 3xN or Nx3, where N is the number of points in the view</param>
        /// <param name="rvec">The rotation vector, 1x3 or 3x1</param>
        /// <param name="tvec">The translation vector, 1x3 or 3x1</param>
        /// <param name="cameraMatrix">The camera matrix (A) [fx 0 cx; 0 fy cy; 0 0 1]. </param>
        /// <param name="distCoeffs">The vector of distortion coefficients, 4x1 or 1x4 [k1, k2, p1, p2]. If it is IntPtr.Zero, all distortion coefficients are considered 0's</param>
        /// <param name="imagePoints">The output array of image points, 2xN or Nx2, where N is the total number of points in the view</param>
        /// <param name="aspectRatio">Aspect ratio</param>
        /// <param name="jacobian">Optional output 2Nx(10+&lt;numDistCoeffs&gt;) jacobian matrix of derivatives of image points with respect to components of the rotation vector, translation vector, focal lengths, coordinates of the principal point and the distortion coefficients. In the old interface different components of the jacobian are returned via different output parameters.</param>
        public static void ProjectPoints(IInputArray objectPoints, IInputArray rvec, IInputArray tvec,
           IInputArray cameraMatrix, IInputArray distCoeffs, IOutputArray imagePoints, IOutputArray jacobian = null,
           double aspectRatio = 0)
        {
            using (InputArray iaObjectPoints = objectPoints.GetInputArray())
            using (InputArray iaRvec = rvec.GetInputArray())
            using (InputArray iaTvec = tvec.GetInputArray())
            using (InputArray iaCameraMatrix = cameraMatrix.GetInputArray())
            using (InputArray iaDistCoeffs = distCoeffs == null ? InputArray.GetEmpty() : distCoeffs.GetInputArray())
            using (OutputArray oaImagePoints = imagePoints.GetOutputArray())
            using (OutputArray oaJacobian = jacobian == null ? OutputArray.GetEmpty() : jacobian.GetOutputArray())
                cveProjectPoints(iaObjectPoints, iaRvec, iaTvec, iaCameraMatrix, iaDistCoeffs,
                   oaImagePoints, oaJacobian, aspectRatio);
        }

        [DllImport(ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        private static extern void cveProjectPoints(IntPtr objPoints, IntPtr rvec, IntPtr tvec, IntPtr cameraMatrix,
           IntPtr distCoeffs, IntPtr imagePoints, IntPtr jacobian, double aspectRatio);


        /// <summary>
        /// Estimates intrinsic camera parameters and extrinsic parameters for each of the views
        /// </summary>
        /// <param name="objectPoints">The 3D location of the object points. The first index is the index of image, second index is the index of the point</param>
        /// <param name="imagePoints">The 2D image location of the points. The first index is the index of the image, second index is the index of the point</param>
        /// <param name="imageSize">The size of the image, used only to initialize intrinsic camera matrix</param>
        /// <param name="rotationVectors">The output 3xM or Mx3 array of rotation vectors (compact representation of rotation matrices, see cvRodrigues2). </param>
        /// <param name="translationVectors">The output 3xM or Mx3 array of translation vectors</param>/// <param name="calibrationType">cCalibration type</param>
        /// <param name="termCriteria">The termination criteria</param>
        /// <param name="cameraMatrix">The output camera matrix (A) [fx 0 cx; 0 fy cy; 0 0 1]. If CV_CALIB_USE_INTRINSIC_GUESS and/or CV_CALIB_FIX_ASPECT_RATION are specified, some or all of fx, fy, cx, cy must be initialized</param>
        /// <param name="distortionCoeffs">The output 4x1 or 1x4 vector of distortion coefficients [k1, k2, p1, p2]</param>
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
        /// Estimates intrinsic camera parameters and extrinsic parameters for each of the views
        /// </summary>
        /// <param name="objectPoints">The joint matrix of object points, 3xN or Nx3, where N is the total number of points in all views</param>
        /// <param name="imagePoints">The joint matrix of corresponding image points, 2xN or Nx2, where N is the total number of points in all views</param>
        /// <param name="imageSize">Size of the image, used only to initialize intrinsic camera matrix</param>
        /// <param name="cameraMatrix">The output camera matrix (A) [fx 0 cx; 0 fy cy; 0 0 1]. If CV_CALIB_USE_INTRINSIC_GUESS and/or CV_CALIB_FIX_ASPECT_RATION are specified, some or all of fx, fy, cx, cy must be initialized</param>
        /// <param name="distortionCoeffs">The output 4x1 or 1x4 vector of distortion coefficients [k1, k2, p1, p2]</param>
        /// <param name="rotationVectors">The output 3xM or Mx3 array of rotation vectors (compact representation of rotation matrices, see cvRodrigues2). </param>
        /// <param name="translationVectors">The output 3xM or Mx3 array of translation vectors</param>
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
        /// Estimates extrinsic camera parameters using known intrinsic parameters and extrinsic parameters for each view. The coordinates of 3D object points and their correspondent 2D projections must be specified. This function also minimizes back-projection error. 
        /// </summary>
        /// <param name="objectPoints">The array of object points</param>
        /// <param name="imagePoints">The array of corresponding image points</param>
        /// <param name="intrinsicMatrix">The camera matrix (A) [fx 0 cx; 0 fy cy; 0 0 1]. </param>
        /// <param name="distortionCoeffs">The vector of distortion coefficients, 4x1 or 1x4 [k1, k2, p1, p2]. If it is IntPtr.Zero, all distortion coefficients are considered 0's.</param>
        /// <param name="rotationVector">The output 3x1 or 1x3 rotation vector (compact representation of a rotation matrix, see cvRodrigues2). </param>
        /// <param name="translationVector">The output 3x1 or 1x3 translation vector</param>
        /// <param name="useExtrinsicGuess">Use the input rotation and translation parameters as a guess</param>
        /// <param name="method">Method for solving a PnP problem</param>
        /// <returns>True if successful</returns>
        public static bool SolvePnP(
           MCvPoint3D32f[] objectPoints,
           PointF[] imagePoints,
           IInputArray intrinsicMatrix,
           IInputArray distortionCoeffs,
           IOutputArray rotationVector,
           IOutputArray translationVector,
           bool useExtrinsicGuess = false,
           CvEnum.SolvePnpMethod method = CvEnum.SolvePnpMethod.Iterative)
        {
            using (VectorOfPoint3D32F objPtVec = new VectorOfPoint3D32F(objectPoints))
            using (VectorOfPointF imgPtVec = new VectorOfPointF(imagePoints))
                return CvInvoke.SolvePnP(objPtVec, imgPtVec, intrinsicMatrix, distortionCoeffs, rotationVector,
                   translationVector, false, method);
        }

        /// <summary>
        /// Estimates extrinsic camera parameters using known intrinsic parameters and extrinsic parameters for each view. The coordinates of 3D object points and their correspondent 2D projections must be specified. This function also minimizes back-projection error
        /// </summary>
        /// <param name="objectPoints">The array of object points, 3xN or Nx3, where N is the number of points in the view</param>
        /// <param name="imagePoints">The array of corresponding image points, 2xN or Nx2, where N is the number of points in the view</param>
        /// <param name="intrinsicMatrix">The camera matrix (A) [fx 0 cx; 0 fy cy; 0 0 1]. </param>
        /// <param name="distortionCoeffs">The vector of distortion coefficients, 4x1 or 1x4 [k1, k2, p1, p2]. If it is IntPtr.Zero, all distortion coefficients are considered 0's.</param>
        /// <param name="rotationVector">The output 3x1 or 1x3 rotation vector (compact representation of a rotation matrix, see cvRodrigues2). </param>
        /// <param name="translationVector">The output 3x1 or 1x3 translation vector</param>
        /// <param name="useExtrinsicGuess">Use the input rotation and translation parameters as a guess</param>
        /// <param name="flags">Method for solving a PnP problem</param>
        /// <returns>True if successful</returns>
        public static bool SolvePnP(
           IInputArray objectPoints,
           IInputArray imagePoints,
           IInputArray intrinsicMatrix,
           IInputArray distortionCoeffs,
           IOutputArray rotationVector,
           IOutputArray translationVector,
           bool useExtrinsicGuess = false,
           CvEnum.SolvePnpMethod flags = CvEnum.SolvePnpMethod.Iterative
           )
        {
            using (InputArray iaObjectPoints = objectPoints.GetInputArray())
            using (InputArray iaImagePoints = imagePoints.GetInputArray())
            using (InputArray iaIntrisicMatrix = intrinsicMatrix.GetInputArray())
            using (InputArray iaDistortionCoeffs = distortionCoeffs.GetInputArray())
            using (OutputArray oaRotationVector = rotationVector.GetOutputArray())
            using (OutputArray oaTranslationVector = translationVector.GetOutputArray())
                return cveSolvePnP(
                   iaObjectPoints,
                   iaImagePoints,
                   iaIntrisicMatrix,
                   iaDistortionCoeffs,
                   oaRotationVector,
                   oaTranslationVector,
                   useExtrinsicGuess,
                   flags);
        }

        [DllImport(ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        [return: MarshalAs(CvInvoke.BoolMarshalType)]
        private static extern bool cveSolvePnP(
           IntPtr objectPoints, IntPtr imagePoints, IntPtr cameraMatrix, IntPtr distCoeffs,
           IntPtr rvec, IntPtr tvec,
           [MarshalAs(CvInvoke.BoolMarshalType)] bool useExtrinsicGuess,
           CvEnum.SolvePnpMethod flags);

        /// <summary>
        /// Finds an object pose from 3D-2D point correspondences using the RANSAC scheme.
        /// </summary>
        /// <param name="objectPoints">Array of object points in the object coordinate space, 3xN/Nx3 1-channel or 1xN/Nx1 3-channel, where N is the number of points. VectorOfPoint3D32f can be also passed here.</param>
        /// <param name="imagePoints">Array of corresponding image points, 2xN/Nx2 1-channel or 1xN/Nx1 2-channel, where N is the number of points. VectorOfPointF can be also passed here.</param>
        /// <param name="cameraMatrix">Input camera matrix</param>
        /// <param name="distCoeffs">Input vector of distortion coefficients of 4, 5, 8 or 12 elements. If the vector is null/empty, the zero distortion coefficients are assumed.</param>
        /// <param name="rvec">Output rotation vector </param>
        /// <param name="tvec">Output translation vector.</param>
        /// <param name="useExtrinsicGuess">If true, the function uses the provided rvec and tvec values as initial approximations of the rotation and translation vectors, respectively, and further optimizes them.</param>
        /// <param name="iterationsCount">Number of iterations.</param>
        /// <param name="reprojectionError">Inlier threshold value used by the RANSAC procedure. The parameter value is the maximum allowed distance between the observed and computed point projections to consider it an inlier.</param>
        /// <param name="confident">The probability that the algorithm produces a useful result.</param>
        /// <param name="inliers">Output vector that contains indices of inliers in objectPoints and imagePoints .</param>
        /// <param name="flags">Method for solving a PnP problem </param>
        /// <returns>True if successful</returns>
        public static bool SolvePnPRansac(
           IInputArray objectPoints,
           IInputArray imagePoints,
           IInputArray cameraMatrix,
           IInputArray distCoeffs,
           IOutputArray rvec,
           IOutputArray tvec,
           bool useExtrinsicGuess = false,
           int iterationsCount = 100,
           float reprojectionError = 8.0f,
           double confident = 0.99,
           IOutputArray inliers = null,
           CvEnum.SolvePnpMethod flags = SolvePnpMethod.Iterative)
        {
            using (InputArray iaObjectPoints = objectPoints.GetInputArray())
            using (InputArray iaImagePoints = imagePoints.GetInputArray())
            using (InputArray iaCameraMatrix = cameraMatrix.GetInputArray())
            using (InputArray iaDistortionCoeffs = distCoeffs == null ? InputArray.GetEmpty() : distCoeffs.GetInputArray())
            using (OutputArray oaRotationVector = rvec.GetOutputArray())
            using (OutputArray oaTranslationVector = tvec.GetOutputArray())
            using (OutputArray oaInliers = inliers == null ? OutputArray.GetEmpty() : inliers.GetOutputArray())
                return cveSolvePnPRansac(
                   iaObjectPoints, iaImagePoints, iaCameraMatrix, iaDistortionCoeffs,
                   oaRotationVector, oaTranslationVector,
                   useExtrinsicGuess, iterationsCount, reprojectionError, confident,
                   oaInliers, flags);
        }

        [DllImport(ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        [return: MarshalAs(CvInvoke.BoolMarshalType)]
        private static extern bool cveSolvePnPRansac(
           IntPtr objectPoints,
           IntPtr imagePoints,
           IntPtr cameraMatrix,
           IntPtr distCoeffs,
           IntPtr rvec,
           IntPtr tvec,
           [MarshalAs(CvInvoke.BoolMarshalType)]
           bool useExtrinsicGuess,
           int iterationsCount,
           float reprojectionError,
           double confident,
           IntPtr inliers,
           CvEnum.SolvePnpMethod flags);

        /// <summary>
        /// Finds an object pose from 3 3D-2D point correspondences.
        /// </summary>
        /// <param name="objectPoints">Array of object points in the object coordinate space, 3x3 1-channel or 1x3/3x1 3-channel. VectorOfPoint3f can be also passed here.</param>
        /// <param name="imagePoints">Array of corresponding image points, 3x2 1-channel or 1x3/3x1 2-channel. VectorOfPoint2f can be also passed here.</param>
        /// <param name="cameraMatrix">Input camera matrix A=[[fx 0 0] [0 fy 0] [cx cy 1]] .</param>
        /// <param name="distCoeffs">Input vector of distortion coefficients (k1,k2,p1,p2[,k3[,k4,k5,k6[,s1,s2,s3,s4[,τx,τy]]]]) of 4, 5, 8, 12 or 14 elements. If the vector is NULL/empty, the zero distortion coefficients are assumed.</param>
        /// <param name="rvecs">Output rotation vectors (see Rodrigues ) that, together with tvecs , brings points from the model coordinate system to the camera coordinate system. A P3P problem has up to 4 solutions.</param>
        /// <param name="tvecs">Output translation vectors.</param>
        /// <param name="flags">Method for solving a P3P problem: either P3P or AP3P</param>
        /// <returns>Number of solutions</returns>
        public static int SolveP3P(
            IInputArray objectPoints,
            IInputArray imagePoints,
            IInputArray cameraMatrix,
            IInputArray distCoeffs,
            IOutputArrayOfArrays rvecs,
            IOutputArrayOfArrays tvecs,
            CvEnum.SolvePnpMethod flags)
        {
            using (InputArray iaObjectPoints = objectPoints.GetInputArray())
            using (InputArray iaImagePoints = imagePoints.GetInputArray())
            using (InputArray iaCameraMatrix = cameraMatrix.GetInputArray())
            using (InputArray iaDistortionCoeffs = distCoeffs == null ? InputArray.GetEmpty() : distCoeffs.GetInputArray())
            using (OutputArray oaRotationVectors = rvecs.GetOutputArray())
            using (OutputArray oaTranslationVectors = tvecs.GetOutputArray())
            {
                return cveSolveP3P(iaObjectPoints, iaImagePoints, iaCameraMatrix, iaDistortionCoeffs, oaRotationVectors,
                    oaTranslationVectors, flags);

            }
        }
        [DllImport(ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        private static extern int cveSolveP3P(
            IntPtr objectPoints,
            IntPtr imagePoints,
            IntPtr cameraMatrix,
            IntPtr distCoeffs,
            IntPtr rvec,
            IntPtr tvec,
            CvEnum.SolvePnpMethod flags);

        /// <summary>
        /// Refine a pose (the translation and the rotation that transform a 3D point expressed in the object coordinate frame to the camera coordinate frame) from a 3D-2D point correspondences and starting from an initial solution
        /// </summary>
        /// <param name="objectPoints">Array of object points in the object coordinate space, Nx3 1-channel or 1xN/Nx1 3-channel, where N is the number of points. VectorOfPoint3f can also be passed here.</param>
        /// <param name="imagePoints">Array of corresponding image points, Nx2 1-channel or 1xN/Nx1 2-channel, where N is the number of points. VectorOfPoint2f can also be passed here.</param>
        /// <param name="cameraMatrix">Input camera matrix A=[[fx,0,0],[0,fy,0][cx,cy,1]].</param>
        /// <param name="distCoeffs">Input vector of distortion coefficients (k1,k2,p1,p2[,k3[,k4,k5,k6[,s1,s2,s3,s4[,τx,τy]]]]) of 4, 5, 8, 12 or 14 elements. If the vector is NULL/empty, the zero distortion coefficients are assumed.</param>
        /// <param name="rvec">Input/Output rotation vector (see Rodrigues ) that, together with tvec, brings points from the model coordinate system to the camera coordinate system. Input values are used as an initial solution.</param>
        /// <param name="tvec">Input/Output translation vector. Input values are used as an initial solution.</param>
        /// <param name="criteria">Criteria when to stop the Levenberg-Marquard iterative algorithm.</param>
        public static void SolvePnPRefineLM(
            IInputArray objectPoints,
            IInputArray imagePoints,
            IInputArray cameraMatrix,
            IInputArray distCoeffs,
            IInputOutputArray rvec,
            IInputOutputArray tvec,
            MCvTermCriteria criteria)
        {
            using (InputArray iaObjectPoints = objectPoints.GetInputArray())
            using (InputArray iaImagePoints = imagePoints.GetInputArray())
            using (InputArray iaCameraMatrix = cameraMatrix.GetInputArray())
            using (InputArray iaDistortionCoeffs = distCoeffs == null ? InputArray.GetEmpty() : distCoeffs.GetInputArray())
            using (InputOutputArray ioaRotationVector = rvec.GetInputOutputArray())
            using (InputOutputArray ioaTranslationVector = tvec.GetInputOutputArray())
                cveSolvePnPRefineLM(iaObjectPoints, iaImagePoints, iaCameraMatrix, iaDistortionCoeffs, ioaRotationVector, ioaTranslationVector, ref criteria);
        }

        [DllImport(ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        private static extern void cveSolvePnPRefineLM(
            IntPtr objectPoints,
            IntPtr imagePoints,
            IntPtr cameraMatrix,
            IntPtr distCoeffs,
            IntPtr rvec,
            IntPtr tvec,
            ref MCvTermCriteria criteria);

        /// <summary>
        /// Refine a pose (the translation and the rotation that transform a 3D point expressed in the object coordinate frame to the camera coordinate frame) from a 3D-2D point correspondences and starting from an initial solution.
        /// </summary>
        /// <param name="objectPoints">Array of object points in the object coordinate space, Nx3 1-channel or 1xN/Nx1 3-channel, where N is the number of points. VectorOfPoint3f can also be passed here.</param>
        /// <param name="imagePoints">Array of corresponding image points, Nx2 1-channel or 1xN/Nx1 2-channel, where N is the number of points. VectorOfPoint2f can also be passed here.</param>
        /// <param name="cameraMatrix">Input camera matrix A=[[fx,0,0],[0,fy,0][cx,cy,1]].</param>
        /// <param name="distCoeffs">Input vector of distortion coefficients (k1,k2,p1,p2[,k3[,k4,k5,k6[,s1,s2,s3,s4[,τx,τy]]]]) of 4, 5, 8, 12 or 14 elements. If the vector is NULL/empty, the zero distortion coefficients are assumed.</param>
        /// <param name="rvec">Input/Output rotation vector (see Rodrigues ) that, together with tvec, brings points from the model coordinate system to the camera coordinate system. Input values are used as an initial solution.</param>
        /// <param name="tvec">Input/Output translation vector. Input values are used as an initial solution.</param>
        /// <param name="criteria">Criteria when to stop the Levenberg-Marquard iterative algorithm.</param>
        /// <param name="VVSlambda">Gain for the virtual visual servoing control law, equivalent to the α gain in the Damped Gauss-Newton formulation.</param>
        public static void SolvePnPRefineVVS(
            IInputArray objectPoints,
            IInputArray imagePoints,
            IInputArray cameraMatrix,
            IInputArray distCoeffs,
            IInputOutputArray rvec,
            IInputOutputArray tvec,
            MCvTermCriteria criteria,
            double VVSlambda)
        {
            using (InputArray iaObjectPoints = objectPoints.GetInputArray())
            using (InputArray iaImagePoints = imagePoints.GetInputArray())
            using (InputArray iaCameraMatrix = cameraMatrix.GetInputArray())
            using (InputArray iaDistortionCoeffs = distCoeffs == null ? InputArray.GetEmpty() : distCoeffs.GetInputArray())
            using (InputOutputArray ioaRotationVector = rvec.GetInputOutputArray())
            using (InputOutputArray ioaTranslationVector = tvec.GetInputOutputArray())
            {
                cveSolvePnPRefineVVS(iaObjectPoints, iaImagePoints, iaCameraMatrix, iaDistortionCoeffs, ioaRotationVector, ioaTranslationVector,
                    ref criteria, VVSlambda);
            }
        }

        [DllImport(ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        private static extern void cveSolvePnPRefineVVS(
            IntPtr objectPoints,
            IntPtr imagePoints,
            IntPtr cameraMatrix,
            IntPtr distCoeffs,
            IntPtr rvec,
            IntPtr tvec,
            ref MCvTermCriteria criteria,
            double VVSlambda);

        /// <summary>
        /// Finds an object pose from 3D-2D point correspondences. 
        /// </summary>
        /// <param name="objectPoints">Array of object points in the object coordinate space, Nx3 1-channel or 1xN/Nx1 3-channel, where N is the number of points. VectorOfPoint3f can also be passed here.</param>
        /// <param name="imagePoints">Array of corresponding image points, Nx2 1-channel or 1xN/Nx1 2-channel, where N is the number of points. VectorOfPoint2f can also be passed here.</param>
        /// <param name="cameraMatrix">Input camera matrix A=[[fx,0,0],[0,fy,0][cx,cy,1]].</param>
        /// <param name="distCoeffs">Input vector of distortion coefficients (k1,k2,p1,p2[,k3[,k4,k5,k6[,s1,s2,s3,s4[,τx,τy]]]]) of 4, 5, 8, 12 or 14 elements. If the vector is NULL/empty, the zero distortion coefficients are assumed.</param>
        /// <param name="rvecs">Vector of output rotation vectors (see Rodrigues ) that, together with tvecs, brings points from the model coordinate system to the camera coordinate system.</param>
        /// <param name="tvecs">Vector of output translation vectors.</param>
        /// <param name="useExtrinsicGuess">Parameter used for SOLVEPNP_ITERATIVE. If true, the function uses the provided rvec and tvec values as initial approximations of the rotation and translation vectors, respectively, and further optimizes them.</param>
        /// <param name="flags">Method for solving a PnP problem</param>
        /// <param name="rvec">Rotation vector used to initialize an iterative PnP refinement algorithm, when flag is SOLVEPNP_ITERATIVE and useExtrinsicGuess is set to true.</param>
        /// <param name="tvec">Translation vector used to initialize an iterative PnP refinement algorithm, when flag is SOLVEPNP_ITERATIVE and useExtrinsicGuess is set to true.</param>
        /// <param name="reprojectionError">Optional vector of reprojection error, that is the RMS error between the input image points and the 3D object points projected with the estimated pose.</param>
        /// <returns></returns>
        public static int SolvePnPGeneric(
            IInputArray objectPoints,
            IInputArray imagePoints,
            IInputArray cameraMatrix,
            IInputArray distCoeffs,
            IOutputArrayOfArrays rvecs,
            IOutputArrayOfArrays tvecs,
            bool useExtrinsicGuess = false,
            CvEnum.SolvePnpMethod flags = SolvePnpMethod.Iterative,
            IInputArray rvec = null,
            IInputArray tvec = null,
            IOutputArray reprojectionError = null)
        {
            using (InputArray iaObjectPoints = objectPoints.GetInputArray())
            using (InputArray iaImagePoints = imagePoints.GetInputArray())
            using (InputArray iaCameraMatrix = cameraMatrix.GetInputArray())
            using (InputArray iaDistCoeffs = distCoeffs.GetInputArray())
            using (OutputArray oaRotationVector = rvecs.GetOutputArray())
            using (OutputArray oaTranslationVector = tvecs.GetOutputArray())
            using (InputArray iaRvec = rvec == null ? InputArray.GetEmpty() : rvec.GetInputArray())
            using (InputArray iaTvec = tvec == null ? InputArray.GetEmpty() : tvec.GetInputArray())
            using (OutputArray oaReporjectionError =
                reprojectionError == null ? OutputArray.GetEmpty() : reprojectionError.GetOutputArray())
            {
                return cveSolvePnPGeneric(
                    iaObjectPoints,
                    iaImagePoints,
                    iaCameraMatrix,
                    iaDistCoeffs,
                    oaRotationVector,
                    oaTranslationVector,
                    useExtrinsicGuess,
                    flags,
                    iaRvec,
                    iaTvec,
                    oaReporjectionError);
            }
        }

        [DllImport(ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        private static extern int cveSolvePnPGeneric(
            IntPtr objectPoints,
            IntPtr imagePoints,
            IntPtr cameraMatrix,
            IntPtr distCoeffs,
            IntPtr rvecs,
            IntPtr tvecs,
            [MarshalAs(CvInvoke.BoolMarshalType)]
            bool useExtrinsicGuess,
            CvEnum.SolvePnpMethod flags,
            IntPtr rvec,
            IntPtr tvec,
            IntPtr reprojectionError);


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
        /// Reconstructs points by triangulation.
        /// </summary>
        /// <param name="projMat1">3x4 projection matrix of the first camera.</param>
        /// <param name="projMat2">3x4 projection matrix of the second camera.</param>
        /// <param name="projPoints1">2xN array of feature points in the first image. It can be also a vector of feature points or two-channel matrix of size 1xN or Nx1</param>
        /// <param name="projPoints2">2xN array of corresponding points in the second image. It can be also a vector of feature points or two-channel matrix of size 1xN or Nx1.</param>
        /// <param name="points4D">4xN array of reconstructed points in homogeneous coordinates.</param>
        public static void TriangulatePoints(IInputArray projMat1, IInputArray projMat2, IInputArray projPoints1,
           IInputArray projPoints2, IOutputArray points4D)
        {
            using (InputArray iaProjMat1 = projMat1.GetInputArray())
            using (InputArray iaProjMat2 = projMat2.GetInputArray())
            using (InputArray iaProjPoints1 = projPoints1.GetInputArray())
            using (InputArray iaProjPoints2 = projPoints2.GetInputArray())
            using (OutputArray oaPoints4D = points4D.GetOutputArray())
                cveTriangulatePoints(iaProjMat1, iaProjMat2, iaProjPoints1, iaProjPoints2, oaPoints4D);
        }

        [DllImport(ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        private static extern void cveTriangulatePoints(IntPtr projMat1, IntPtr projMat2, IntPtr projPoints1,
           IntPtr projPoints2, IntPtr points4D);

        /// <summary>
        /// Refines coordinates of corresponding points.
        /// </summary>
        /// <param name="f">3x3 fundamental matrix.</param>
        /// <param name="points1">1xN array containing the first set of points.</param>
        /// <param name="points2">1xN array containing the second set of points.</param>
        /// <param name="newPoints1">The optimized points1.</param>
        /// <param name="newPoints2">The optimized points2.</param>
        public static void CorrectMatches(IInputArray f, IInputArray points1, IInputArray points2, IOutputArray newPoints1,
           IOutputArray newPoints2)
        {
            using (InputArray iaF = f.GetInputArray())
            using (InputArray iaPoints1 = points1.GetInputArray())
            using (InputArray iaPoints2 = points2.GetInputArray())
            using (OutputArray oaNewPoints1 = newPoints1.GetOutputArray())
            using (OutputArray oaNewPoints2 = newPoints2.GetOutputArray())
                cveCorrectMatches(iaF, iaPoints1, iaPoints2, oaNewPoints1, oaNewPoints2);
        }

        [DllImport(ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        private static extern void cveCorrectMatches(IntPtr f, IntPtr points1, IntPtr points2, IntPtr newPoints1,
           IntPtr newPoints2);

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
            using (OutputArray oaSharpness = (sharpness == null? OutputArray.GetEmpty() : sharpness.GetOutputArray()))
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
        internal static extern void cveFisheyeDistortPoints(IntPtr undistored, IntPtr distorted, IntPtr K, IntPtr D,
           double alpha);

        [DllImport(ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern void cveFisheyeUndistorPoints(IntPtr distorted, IntPtr undistorted, IntPtr K, IntPtr D,
           IntPtr R, IntPtr P);


        [DllImport(ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern void cveFisheyeInitUndistorRectifyMap(IntPtr K, IntPtr D, IntPtr R, IntPtr P, ref Size size,
           DepthType m1Type, IntPtr map1, IntPtr map2);

        [DllImport(ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern void cveFisheyeUndistorImage(IntPtr distorted, IntPtr undistored, IntPtr K, IntPtr D,
           IntPtr Knew, ref Size newSize);

        [DllImport(ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern void cveFisheyeEstimateNewCameraMatrixForUndistorRectify(
           IntPtr K, IntPtr D,
           ref Size imageSize, IntPtr R, IntPtr P, double balance, ref Size newSize, double fovScale);

        [DllImport(ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern void cveFisheyeSteteoRectify(
           IntPtr K1, IntPtr D1, IntPtr K2,
           IntPtr D2, ref Size imageSize,
           IntPtr R, IntPtr tvec, IntPtr R1, IntPtr R2, IntPtr P1,
           IntPtr P2, IntPtr Q, int flags,
           ref Size newImageSize, double balance, double fovScale);


        [DllImport(ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern void cveFisheyeCalibrate(
           IntPtr objectPoints, IntPtr imagePoints, ref Size imageSize,
           IntPtr K, IntPtr D, IntPtr rvecs, IntPtr tvecs, int flags,
           ref MCvTermCriteria criteria);

        [DllImport(ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern void cveFisheyeStereoCalibrate(
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
        public static void UndistorPoints(IInputArray distorted, IOutputArray undistorted, IInputArray K, IInputArray D,
           IInputArray R = null, IInputArray P = null)
        {
            using (InputArray iaDistorted = distorted.GetInputArray())
            using (OutputArray oaUndistorted = undistorted.GetOutputArray())
            using (InputArray iaK = K.GetInputArray())
            using (InputArray iaD = D.GetInputArray())
            using (InputArray iaR = R == null ? InputArray.GetEmpty() : R.GetInputArray())
            using (InputArray iaP = P == null ? InputArray.GetEmpty() : P.GetInputArray())
            {
                CvInvoke.cveFisheyeUndistorPoints(iaDistorted, oaUndistorted, iaK, iaD, iaR, iaP);
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
        /// <param name="m1Type">Type of the first output map that can be CV_32FC1 or CV_16SC2 . See convertMaps() for details.</param>
        /// <param name="map1">The first output map.</param>
        /// <param name="map2">The second output map.</param>
        public static void InitUndistorRectifyMap(IInputArray K, IInputArray D, IInputArray R, IInputArray P, Size size,
           DepthType m1Type, IOutputArray map1, IOutputArray map2)
        {
            using (InputArray iaK = K.GetInputArray())
            using (InputArray iaD = D.GetInputArray())
            using (InputArray iaR = R.GetInputArray())
            using (InputArray iaP = P.GetInputArray())
            using (OutputArray oaMap1 = map1.GetOutputArray())
            using (OutputArray oaMap2 = map2.GetOutputArray())
            {
                CvInvoke.cveFisheyeInitUndistorRectifyMap(iaK, iaD, iaR, iaP, ref size, m1Type, oaMap1, oaMap2);
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
        public static void UndistorImage(IInputArray distorted, IOutputArray undistored, IInputArray K, IInputArray D,
           IInputArray Knew = null, Size newSize = new Size())
        {
            using (InputArray iaDistorted = distorted.GetInputArray())
            using (OutputArray oaUndistorted = undistored.GetOutputArray())
            using (InputArray iaK = K.GetInputArray())
            using (InputArray iaD = D.GetInputArray())
            using (InputArray iaKnew = Knew == null ? InputArray.GetEmpty() : Knew.GetInputArray())
            {
                CvInvoke.cveFisheyeUndistorImage(iaDistorted, oaUndistorted, iaK, iaD, iaKnew, ref newSize);
            }
        }

        /// <summary>
        /// Estimates new camera matrix for undistortion or rectification.
        /// </summary>
        /// <param name="K">Camera matrix</param>
        /// <param name="D">Input vector of distortion coefficients (k1,k2,k3,k4).</param>
        /// <param name="imageSize"></param>
        /// <param name="R">Rectification transformation in the object space: 3x3 1-channel, or vector: 3x1/1x3 1-channel or 1x1 3-channel</param>
        /// <param name="P">New camera matrix (3x3) or new projection matrix (3x4)</param>
        /// <param name="balance">Sets the new focal length in range between the min focal length and the max focal length. Balance is in range of [0, 1]</param>
        /// <param name="newSize"></param>
        /// <param name="fovScale">Divisor for new focal length.</param>
        public static void EstimateNewCameraMatrixForUndistorRectify(IInputArray K, IInputArray D,
           Size imageSize, IInputArray R, IOutputArray P, double balance = 0.0, Size newSize = new Size(),
           double fovScale = 1.0)
        {
            using (InputArray iaK = K.GetInputArray())
            using (InputArray iaD = D.GetInputArray())
            using (InputArray iaR = R.GetInputArray())
            using (OutputArray oaP = P.GetOutputArray())
            {
                CvInvoke.cveFisheyeEstimateNewCameraMatrixForUndistorRectify(iaK, iaD, ref imageSize, iaR, oaP, balance,
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
        public static void SteteoRectify(IInputArray K1, IInputArray D1, IInputArray K2, IInputArray D2, Size imageSize,
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
                CvInvoke.cveFisheyeSteteoRectify(iaK1, iaD1, iaK2, iaD2, ref imageSize, iaR, iaTvec, oaR1, oaR2, oaP1, oaP2,
                   oaQ, flags, ref newImageSize, balance, fovScale);
            }
        }

        /// <summary>
        /// Performs camera calibaration.
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
        public static void Calibrate(IInputArray objectPoints, IInputArray imagePoints, Size imageSize,
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
                CvInvoke.cveFisheyeCalibrate(iaObjectPoints, iaImagePoints, ref imageSize, ioaK, ioaD, oaRvecs, oaTvecs,
                  (int)flags, ref criteria);
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
        public static void StereoCalibrate(IInputArray objectPoints, IInputArray imagePoints1,
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
                CvInvoke.cveFisheyeStereoCalibrate(iaObjectPoints, iaImagePoints1, iaImagePoints2, ioaK1, ioaD1, ioaK2, ioaD2,
                   ref imageSize, oaR, oaT, (int)flags, ref criteria);
            }
        }
    }
}