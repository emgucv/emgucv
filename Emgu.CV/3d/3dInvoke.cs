//----------------------------------------------------------------------------
//  Copyright (C) 2004-2022 by EMGU Corporation. All rights reserved.       
//----------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Emgu.CV.Structure;
using Emgu.CV.CvEnum;
using Emgu.CV.Util;
using Emgu.Util;
using System.Drawing;

namespace Emgu.CV
{
    public static partial class CvInvoke
    {
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
        /// Computes an RQ decomposition of 3x3 matrices.
        /// </summary>
        /// <param name="src">3x3 input matrix.</param>
        /// <param name="mtxR">Output 3x3 upper-triangular matrix.</param>
        /// <param name="mtxQ">Output 3x3 orthogonal matrix.</param>
        /// <param name="Qx">Optional output 3x3 rotation matrix around x-axis.</param>
        /// <param name="Qy">Optional output 3x3 rotation matrix around y-axis.</param>
        /// <param name="Qz">Optional output 3x3 rotation matrix around z-axis.</param>
        /// <returns>The euler angles</returns>
        public static MCvPoint3D64f RQDecomp3x3(
            IInputArray src,
            IOutputArray mtxR,
            IOutputArray mtxQ,
            IOutputArray Qx = null,
            IOutputArray Qy = null,
            IOutputArray Qz = null)
        {
            MCvPoint3D64f results = new MCvPoint3D64f();
            using (InputArray iaSrc = src.GetInputArray())
            using (OutputArray oaMtxR = mtxR.GetOutputArray())
            using (OutputArray oaMtxQ = mtxQ.GetOutputArray())
            using (OutputArray oaQx = Qx == null ? OutputArray.GetEmpty() : Qx.GetOutputArray())
            using (OutputArray oaQy = Qy == null ? OutputArray.GetEmpty() : Qy.GetOutputArray())
            using (OutputArray oaQz = Qz == null ? OutputArray.GetEmpty() : Qz.GetOutputArray())
            {
                cveRQDecomp3x3(
                    iaSrc,
                    ref results,
                    oaMtxR,
                    oaMtxQ,
                    oaQx,
                    oaQy,
                    oaQz
                );
                return results;
            }
        }

        [DllImport(ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        private static extern void cveRQDecomp3x3(
            IntPtr src,
            ref MCvPoint3D64f result,
            IntPtr mtxR,
            IntPtr mtxQ,
            IntPtr Qx,
            IntPtr Qy,
            IntPtr Qz);


        /// <summary>
        /// Decomposes a projection matrix into a rotation matrix and a camera intrinsic matrix.
        /// </summary>
        /// <param name="projMatrix">3x4 input projection matrix P.</param>
        /// <param name="cameraMatrix">Output 3x3 camera intrinsic matrix A</param>
        /// <param name="rotMatrix">Output 3x3 external rotation matrix R.</param>
        /// <param name="transVect">Output 4x1 translation vector T.</param>
        /// <param name="rotMatrixX">Optional 3x3 rotation matrix around x-axis.</param>
        /// <param name="rotMatrixY">Optional 3x3 rotation matrix around y-axis.</param>
        /// <param name="rotMatrixZ">Optional 3x3 rotation matrix around z-axis.</param>
        /// <param name="eulerAngles">Optional three-element vector containing three Euler angles of rotation in degrees.</param>
        public static void DecomposeProjectionMatrix(
            IInputArray projMatrix,
            IOutputArray cameraMatrix,
            IOutputArray rotMatrix,
            IOutputArray transVect,
            IOutputArray rotMatrixX = null,
            IOutputArray rotMatrixY = null,
            IOutputArray rotMatrixZ = null,
            IOutputArray eulerAngles = null)
        {
            using (InputArray iaProjMatrix = projMatrix.GetInputArray())
            using (OutputArray oaCameraMatrix = cameraMatrix.GetOutputArray())
            using (OutputArray oaRotMatrix = rotMatrix.GetOutputArray())
            using (OutputArray oaTransVect = transVect.GetOutputArray())
            using (OutputArray oaRotMatrixX = rotMatrixX == null ? OutputArray.GetEmpty() : rotMatrixX.GetOutputArray())
            using (OutputArray oaRotMatrixY = rotMatrixY == null ? OutputArray.GetEmpty() : rotMatrixY.GetOutputArray())
            using (OutputArray oaRotMatrixZ = rotMatrixZ == null ? OutputArray.GetEmpty() : rotMatrixZ.GetOutputArray())
            using (OutputArray oaEulerAngles = eulerAngles == null
                ? OutputArray.GetEmpty()
                : eulerAngles.GetOutputArray())
            {
                cveDecomposeProjectionMatrix(
                    iaProjMatrix,
                    oaCameraMatrix,
                    oaRotMatrix,
                    oaTransVect,
                    oaRotMatrixX,
                    oaRotMatrixY,
                    oaRotMatrixZ,
                    oaEulerAngles);
            }
        }

        [DllImport(ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        private static extern void cveDecomposeProjectionMatrix(
            IntPtr projMatrix,
            IntPtr cameraMatrix,
            IntPtr rotMatrix,
            IntPtr transVect,
            IntPtr rotMatrixX,
            IntPtr rotMatrixY,
            IntPtr rotMatrixZ,
            IntPtr eulerAngles);


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
           IntPtr objectPoints,
           IntPtr imagePoints,
           IntPtr cameraMatrix,
           IntPtr distCoeffs,
           IntPtr rvec,
           IntPtr tvec,
           [MarshalAs(CvInvoke.BoolMarshalType)]
           bool useExtrinsicGuess,
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
        /// <param name="useExtrinsicGuess">Parameter used for SolvePnpMethod.Iterative. If true, the function uses the provided rvec and tvec values as initial approximations of the rotation and translation vectors, respectively, and further optimizes them.</param>
        /// <param name="flags">Method for solving a PnP problem</param>
        /// <param name="rvec">Rotation vector used to initialize an iterative PnP refinement algorithm, when flag is SOLVEPNP_ITERATIVE and useExtrinsicGuess is set to true.</param>
        /// <param name="tvec">Translation vector used to initialize an iterative PnP refinement algorithm, when flag is SOLVEPNP_ITERATIVE and useExtrinsicGuess is set to true.</param>
        /// <param name="reprojectionError">Optional vector of reprojection error, that is the RMS error between the input image points and the 3D object points projected with the estimated pose.</param>
        /// <returns>The number of solutions</returns>
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
        /// Decompose an essential matrix to possible rotations and translation.
        /// </summary>
        /// <param name="e">The input essential matrix.</param>
        /// <param name="r1">One possible rotation matrix.</param>
        /// <param name="r2">Another possible rotation matrix.</param>
        /// <param name="t">One possible translation.</param>
        /// <remarks>This function decomposes the essential matrix E using svd decomposition. In general, four possible poses exist for the decomposition of E. They are [R1,t], [R1,−t], [R2,t], [R2,−t]</remarks>
        public static void DecomposeEssentialMat(IInputArray e, IOutputArray r1, IOutputArray r2, IOutputArray t)
        {
            using (InputArray iaE = e.GetInputArray())
            using (OutputArray oaR1 = r1.GetOutputArray())
            using (OutputArray oaR2 = r2.GetOutputArray())
            using (OutputArray oaT = t.GetOutputArray())
            {
                cveDecomposeEssentialMat(iaE, oaR1, oaR2, oaT);
            }
        }

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern void cveDecomposeEssentialMat(
            IntPtr e,
            IntPtr r1,
            IntPtr r2,
            IntPtr t);

        /// <summary>
        /// Decompose a homography matrix to rotation(s), translation(s) and plane normal(s).
        /// </summary>
        /// <param name="h">The input homography matrix between two images.</param>
        /// <param name="k">The input camera intrinsic matrix.</param>
        /// <param name="rotations">Array of rotation matrices.</param>
        /// <param name="translations">Array of translation matrices.</param>
        /// <param name="normals">Array of plane normal matrices.</param>
        /// <returns>Number of solutions</returns>
        public static int DecomposeHomographyMat(
            IInputArray h,
            IInputArray k,
            IOutputArrayOfArrays rotations,
            IOutputArrayOfArrays translations,
            IOutputArrayOfArrays normals)
        {
            using (InputArray iaH = h.GetInputArray())
            using (InputArray iaK = k.GetInputArray())
            using (OutputArray oaRotations = rotations.GetOutputArray())
            using (OutputArray oaTranslations = translations.GetOutputArray())
            using (OutputArray oaNormals = normals.GetOutputArray())
            {
                return cveDecomposeHomographyMat(iaH, iaK, oaRotations, oaTranslations, oaNormals);
            }
        }

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern int cveDecomposeHomographyMat(
            IntPtr h,
            IntPtr k,
            IntPtr rotations,
            IntPtr translations,
            IntPtr normals);

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
        /// Calculates an essential matrix from the corresponding points in two images.
        /// </summary>
        /// <param name="points1">Array of N (N &gt;= 5) 2D points from the first image. The point coordinates should be floating-point (single or double precision).</param>
        /// <param name="points2">Array of the second image points of the same size and format as points1</param>
        /// <param name="cameraMatrix">Camera matrix K=[[fx 0 cx][0 fy cy][0 0 1]]. Note that this function assumes that points1 and points2 are feature points from cameras with the same camera matrix.</param>
        /// <param name="method">Method for computing a fundamental matrix. RANSAC for the RANSAC algorithm. LMEDS for the LMedS algorithm</param>
        /// <param name="prob">Parameter used for the RANSAC or LMedS methods only. It specifies a desirable level of confidence (probability) that the estimated matrix is correct.</param>
        /// <param name="threshold">Parameter used for RANSAC. It is the maximum distance from a point to an epipolar line in pixels, beyond which the point is considered an outlier and is not used for computing the final fundamental matrix. It can be set to something like 1-3, depending on the accuracy of the point localization, image resolution, and the image noise.</param>
        /// <param name="mask">Output array of N elements, every element of which is set to 0 for outliers and to 1 for the other points. The array is computed only in the RANSAC and LMedS methods.</param>
        /// <param name="maxIter">The maximum number of robust method iterations</param>
        /// <returns>The essential mat</returns>
        public static Mat FindEssentialMat(
            IInputArray points1,
            IInputArray points2,
            IInputArray cameraMatrix,
            CvEnum.FmType method = CvEnum.FmType.Ransac,
            double prob = 0.999,
            double threshold = 1.0,
            int maxIter = 1000,
            IOutputArray mask = null)
        {
            using (InputArray iaPoints1 = points1.GetInputArray())
            using (InputArray iaPoints2 = points2.GetInputArray())
            using (InputArray iaCameraMatrix = cameraMatrix.GetInputArray())
            using (OutputArray oaMask = mask == null ? OutputArray.GetEmpty() : mask.GetOutputArray())
            {
                Mat essentialMat = new Mat();
                cveFindEssentialMat(
                    iaPoints1,
                    iaPoints2,
                    iaCameraMatrix,
                    method,
                    prob,
                    threshold,
                    maxIter,
                    oaMask,
                    essentialMat);
                return essentialMat;
            }
        }

        [DllImport(ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        private static extern void cveFindEssentialMat(
            IntPtr points1,
            IntPtr points2,
            IntPtr cameraMatrix,
            CvEnum.FmType method,
            double prob,
            double threshold,
            int maxIter,
            IntPtr mask,
            IntPtr essentialMat);

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
            using (VectorOfPointF vpFrom = new VectorOfPointF(from))
            using (VectorOfPointF vpTo = new VectorOfPointF(to))
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
            int refineIters = 10)
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
        /// <param name="depthType">Depth type of the first output map. (The combination with <paramref name="channels"/> can be one of CV_32FC1, CV_32FC2 or CV_16SC2)</param>
        /// <param name="channels">Number of channels of the first output map. (The combination with <paramref name="depthType"/> can be one of CV_32FC1, CV_32FC2 or CV_16SC2)</param>
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
           int channels,
           IOutputArray map1,
           IOutputArray map2)
        {
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

    }
}