//----------------------------------------------------------------------------
//  Copyright (C) 2004-2021 by EMGU Corporation. All rights reserved.       
//----------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using Emgu.CV.Structure;
using Emgu.CV.Text;
using Emgu.CV.Util;
using Emgu.Util;
using System.Diagnostics;
using System.Drawing;
using Emgu.CV.CvEnum;

namespace Emgu.CV.Aruco
{
    /// <summary>
    /// Entry points for the Aruco module.
    /// </summary>
    public static partial class ArucoInvoke
    {
        static ArucoInvoke()
        {
            CvInvoke.Init();
        }

        /// <summary>
        /// Draw a canonical marker image.
        /// </summary>
        /// <param name="dict">dictionary of markers indicating the type of markers</param>
        /// <param name="id">identifier of the marker that will be returned. It has to be a valid id in the specified dictionary.</param>
        /// <param name="sidePixels">size of the image in pixels</param>
        /// <param name="img">output image with the marker</param>
        /// <param name="borderBits">width of the marker border.</param>
        public static void DrawMarker(Dictionary dict, int id, int sidePixels, IOutputArray img, int borderBits = 1)
        {
            using (OutputArray oaImg = img.GetOutputArray())
                cveArucoDrawMarker(dict, id, sidePixels, oaImg, borderBits);
        }
        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern void cveArucoDrawMarker(
            IntPtr dictionary, 
            int id, 
            int sidePixels, 
            IntPtr img, 
            int borderBits);


        /// <summary>
        /// Performs marker detection in the input image. Only markers included in the specific dictionary are searched. For each detected marker, it returns the 2D position of its corner in the image and its corresponding identifier. Note that this function does not perform pose estimation.
        /// </summary>
        /// <param name="image">input image</param>
        /// <param name="dict">indicates the type of markers that will be searched</param>
        /// <param name="corners">Vector of detected marker corners. For each marker, its four corners are provided, (e.g VectorOfVectorOfPointF ). For N detected markers, the dimensions of this array is Nx4. The order of the corners is clockwise.</param>
        /// <param name="ids">vector of identifiers of the detected markers. The identifier is of type int (e.g. VectorOfInt). For N detected markers, the size of ids is also N. The identifiers have the same order than the markers in the imgPoints array.</param>
        /// <param name="parameters">marker detection parameters</param>
        /// <param name="rejectedImgPoints">contains the imgPoints of those squares whose inner code has not a correct codification. Useful for debugging purposes.</param>
        public static void DetectMarkers(
           IInputArray image, Dictionary dict, IOutputArrayOfArrays corners,
           IOutputArray ids, DetectorParameters parameters,
           IOutputArrayOfArrays rejectedImgPoints = null
           )
        {
            using (InputArray iaImage = image.GetInputArray())
            using (OutputArray oaCorners = corners.GetOutputArray())
            using (OutputArray oaIds = ids.GetOutputArray())
            using (OutputArray oaRejectedImgPoints = rejectedImgPoints != null ? rejectedImgPoints.GetOutputArray() : OutputArray.GetEmpty())
            {
                cveArucoDetectMarkers(iaImage, dict, oaCorners, oaIds, ref parameters, oaRejectedImgPoints);
            }
        }
        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern void cveArucoDetectMarkers(IntPtr image, IntPtr dictionary, IntPtr corners,
           IntPtr ids, ref DetectorParameters parameters,
           IntPtr rejectedImgPoints);

        /// <summary>
        /// Given the pose estimation of a marker or board, this function draws the axis of the world coordinate system, i.e. the system centered on the marker/board. Useful for debugging purposes.
        /// </summary>
        /// <param name="image">input/output image. It must have 1 or 3 channels. The number of channels is not altered.</param>
        /// <param name="cameraMatrix">input 3x3 floating-point camera matrix</param>
        /// <param name="distCoeffs">vector of distortion coefficients (k1,k2,p1,p2[,k3[,k4,k5,k6],[s1,s2,s3,s4]]) of 4, 5, 8 or 12 elements</param>
        /// <param name="rvec">rotation vector of the coordinate system that will be drawn.</param>
        /// <param name="tvec">translation vector of the coordinate system that will be drawn.</param>
        /// <param name="length">length of the painted axis in the same unit than tvec (usually in meters)</param>
        public static void DrawAxis(
           IInputOutputArray image, IInputArray cameraMatrix, IInputArray distCoeffs,
           IInputArray rvec, IInputArray tvec, float length)
        {
            using (InputOutputArray ioaImage = image.GetInputOutputArray())
            using (InputArray iaCameraMatrix = cameraMatrix.GetInputArray())
            using (InputArray iaDistCoeffs = distCoeffs.GetInputArray())
            using (InputArray iaRvec = rvec.GetInputArray())
            using (InputArray iaTvec = tvec.GetInputArray())
            {
                cveArucoDrawAxis(ioaImage, iaCameraMatrix, iaDistCoeffs, iaRvec, iaTvec, length);
            }
        }
        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern void cveArucoDrawAxis(IntPtr image, IntPtr cameraMatrix, IntPtr distCoeffs, IntPtr rvec, IntPtr tvec, float length);

        /// <summary>
        /// This function receives the detected markers and returns their pose estimation respect to the camera individually. So for each marker, one rotation and translation vector is returned. The returned transformation is the one that transforms points from each marker coordinate system to the camera coordinate system. The marker corrdinate system is centered on the middle of the marker, with the Z axis perpendicular to the marker plane. The coordinates of the four corners of the marker in its own coordinate system are: (-markerLength/2, markerLength/2, 0), (markerLength/2, markerLength/2, 0), (markerLength/2, -markerLength/2, 0), (-markerLength/2, -markerLength/2, 0)
        /// </summary>
        /// <param name="corners">vector of already detected markers corners. For each marker, its four corners are provided, (e.g VectorOfVectorOfPointF ). For N detected markers, the dimensions of this array should be Nx4. The order of the corners should be clockwise.</param>
        /// <param name="markerLength">the length of the markers' side. The returning translation vectors will be in the same unit. Normally, unit is meters.</param>
        /// <param name="cameraMatrix">input 3x3 floating-point camera matrix</param>
        /// <param name="distCoeffs">vector of distortion coefficients (k1,k2,p1,p2[,k3[,k4,k5,k6],[s1,s2,s3,s4]]) of 4, 5, 8 or 12 elements</param>
        /// <param name="rvecs">array of output rotation vectors. Each element in rvecs corresponds to the specific marker in imgPoints.</param>
        /// <param name="tvecs">array of output translation vectors (e.g. VectorOfPoint3D32F ). Each element in tvecs corresponds to the specific marker in imgPoints.</param>
        public static void EstimatePoseSingleMarkers(IInputArrayOfArrays corners, float markerLength,
           IInputArray cameraMatrix, IInputArray distCoeffs,
           IOutputArrayOfArrays rvecs, IOutputArrayOfArrays tvecs)
        {
            using (InputArray iaCorners = corners.GetInputArray())
            using (InputArray iaCameraMatrix = cameraMatrix.GetInputArray())
            using (InputArray iaDistCoeffs = distCoeffs.GetInputArray())
            using (OutputArray oaRvecs = rvecs.GetOutputArray())
            using (OutputArray oaTvecs = tvecs.GetOutputArray())
            {
                cveArucoEstimatePoseSingleMarkers(iaCorners, markerLength, iaCameraMatrix, iaDistCoeffs, oaRvecs, oaTvecs);
            }
        }
        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern void cveArucoEstimatePoseSingleMarkers(IntPtr corners, float markerLength,
           IntPtr cameraMatrix, IntPtr distCoeffs,
           IntPtr rvecs, IntPtr tvecs);

        /// <summary>
        /// Refine not detected markers based on the already detected and the board layout.
        /// </summary>
        /// <param name="image">Input image</param>
        /// <param name="board">Layout of markers in the board.</param>
        /// <param name="detectedCorners">Vector of already detected marker corners.</param>
        /// <param name="detectedIds">Vector of already detected marker identifiers.</param>
        /// <param name="rejectedCorners">Vector of rejected candidates during the marker detection process</param>
        /// <param name="cameraMatrix">Optional input 3x3 floating-point camera matrix </param>
        /// <param name="distCoeffs">Optional vector of distortion coefficients (k1,k2,p1,p2[,k3[,k4,k5,k6],[s1,s2,s3,s4]]) of 4, 5, 8 or 12 elements</param>
        /// <param name="minRepDistance">Minimum distance between the corners of the rejected candidate and the reprojected marker in order to consider it as a correspondence. (default 10)</param>
        /// <param name="errorCorrectionRate">Rate of allowed erroneous bits respect to the error correction capability of the used dictionary. -1 ignores the error correction step. (default 3)</param>
        /// <param name="checkAllOrders">Consider the four posible corner orders in the rejectedCorners array. If it set to false, only the provided corner order is considered (default true).</param>
        /// <param name="recoveredIdxs">Optional array to returns the indexes of the recovered candidates in the original rejectedCorners array.</param>
        /// <param name="parameters">marker detection parameters</param>
        public static void RefineDetectedMarkers(
           IInputArray image, IBoard board, IInputOutputArray detectedCorners,
           IInputOutputArray detectedIds, IInputOutputArray rejectedCorners,
           IInputArray cameraMatrix, IInputArray distCoeffs,
           float minRepDistance, float errorCorrectionRate,
           bool checkAllOrders,
           IOutputArray recoveredIdxs, DetectorParameters parameters)
        {
            using (InputArray iaImage = image.GetInputArray())
            using (InputOutputArray ioaDetectedCorners = detectedCorners.GetInputOutputArray())
            using (InputOutputArray ioaDetectedIds = detectedIds.GetInputOutputArray())
            using (InputOutputArray ioaRejectedCorners = rejectedCorners.GetInputOutputArray())
            using (InputArray iaCameraMatrix = cameraMatrix == null ? InputArray.GetEmpty() : cameraMatrix.GetInputArray())
            using (InputArray iaDistCoeffs = distCoeffs == null ? InputArray.GetEmpty() : distCoeffs.GetInputArray())
            using (
               OutputArray oaRecovervedIdx = recoveredIdxs == null
                  ? OutputArray.GetEmpty()
                  : recoveredIdxs.GetOutputArray())
            {
                cveArucoRefineDetectedMarkers(iaImage, board.BoardPtr, ioaDetectedCorners, ioaDetectedIds, ioaRejectedCorners,
                   iaCameraMatrix, iaDistCoeffs, minRepDistance, errorCorrectionRate, checkAllOrders, oaRecovervedIdx, ref parameters);
            }
        }
        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern void cveArucoRefineDetectedMarkers(
            IntPtr image, IntPtr board, IntPtr detectedCorners,
            IntPtr detectedIds, IntPtr rejectedCorners,
            IntPtr cameraMatrix, IntPtr distCoeffs,
            float minRepDistance, float errorCorrectionRate,
            [MarshalAs(CvInvoke.BoolMarshalType)]
            bool checkAllOrders,
            IntPtr ecoveredIdxs, ref DetectorParameters parameters);

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern void cveArucoDetectorParametersGetDefault(ref DetectorParameters parameters);

        /// <summary>
        /// Draw detected markers in image.
        /// </summary>
        /// <param name="image">Input/output image. It must have 1 or 3 channels. The number of channels is not altered.</param>
        /// <param name="corners">Positions of marker corners on input image. (e.g std::vector&lt;std::vector&lt;cv::Point2f&gt; &gt; ). For N detected markers, the dimensions of this array should be Nx4. The order of the corners should be clockwise.</param>
        /// <param name="ids">Vector of identifiers for markers in markersCorners . Optional, if not provided, ids are not painted.</param>
        /// <param name="borderColor">Color of marker borders. Rest of colors (text color and first corner color) are calculated based on this one to improve visualization.</param>
        public static void DrawDetectedMarkers(
           IInputOutputArray image, IInputArray corners, IInputArray ids,
           MCvScalar borderColor)
        {
            using (InputOutputArray ioaImage = image.GetInputOutputArray())
            using (InputArray iaCorners = corners.GetInputArray())
            using (InputArray iaIds = ids.GetInputArray())
            {
                cveArucoDrawDetectedMarkers(ioaImage, iaCorners, iaIds, ref borderColor);
            }
        }

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern void cveArucoDrawDetectedMarkers(
           IntPtr image, 
           IntPtr corners,
           IntPtr ids, 
           ref MCvScalar borderColor);

        /// <summary>
        /// Calibrate a camera using aruco markers.
        /// </summary>
        /// <param name="corners">Vector of detected marker corners in all frames. The corners should have the same format returned by detectMarkers</param>
        /// <param name="ids">List of identifiers for each marker in corners</param>
        /// <param name="counter">Number of markers in each frame so that corners and ids can be split</param>
        /// <param name="board">Marker Board layout</param>
        /// <param name="imageSize">Size of the image used only to initialize the intrinsic camera matrix.</param>
        /// <param name="cameraMatrix">Output 3x3 floating-point camera matrix. </param>
        /// <param name="distCoeffs">Output vector of distortion coefficients (k1,k2,p1,p2[,k3[,k4,k5,k6],[s1,s2,s3,s4]]) of 4, 5, 8 or 12 elements</param>
        /// <param name="rvecs">Output vector of rotation vectors (see Rodrigues ) estimated for each board view (e.g. std::vector&lt;cv::Mat&gt;). That is, each k-th rotation vector together with the corresponding k-th translation vector (see the next output parameter description) brings the board pattern from the model coordinate space (in which object points are specified) to the world coordinate space, that is, a real position of the board pattern in the k-th pattern view (k=0.. M -1).</param>
        /// <param name="tvecs">Output vector of translation vectors estimated for each pattern view.</param>
        /// <param name="flags">Flags Different flags for the calibration process</param>
        /// <param name="criteria">Termination criteria for the iterative optimization algorithm.</param>
        /// <returns>The final re-projection error.</returns>
        public static double CalibrateCameraAruco(
            IInputArrayOfArrays corners, IInputArray ids, IInputArray counter, IBoard board, Size imageSize,
            IInputOutputArray cameraMatrix, IInputOutputArray distCoeffs, IOutputArray rvecs, IOutputArray tvecs,
            CalibType flags, MCvTermCriteria criteria)
        {
            return CalibrateCameraAruco(corners, ids, counter, board, imageSize, cameraMatrix, distCoeffs, rvecs, tvecs,
                null, null, null, flags, criteria);
        }

        /// <summary>
        /// Calibrate a camera using aruco markers.
        /// </summary>
        /// <param name="corners">Vector of detected marker corners in all frames. The corners should have the same format returned by detectMarkers</param>
        /// <param name="ids">List of identifiers for each marker in corners</param>
        /// <param name="counter">Number of markers in each frame so that corners and ids can be split</param>
        /// <param name="board">Marker Board layout</param>
        /// <param name="imageSize">Size of the image used only to initialize the intrinsic camera matrix.</param>
        /// <param name="cameraMatrix">Output 3x3 floating-point camera matrix. </param>
        /// <param name="distCoeffs">Output vector of distortion coefficients (k1,k2,p1,p2[,k3[,k4,k5,k6],[s1,s2,s3,s4]]) of 4, 5, 8 or 12 elements</param>
        /// <param name="rvecs">Output vector of rotation vectors (see Rodrigues ) estimated for each board view (e.g. std::vector&lt;cv::Mat&gt;). That is, each k-th rotation vector together with the corresponding k-th translation vector (see the next output parameter description) brings the board pattern from the model coordinate space (in which object points are specified) to the world coordinate space, that is, a real position of the board pattern in the k-th pattern view (k=0.. M -1).</param>
        /// <param name="tvecs">Output vector of translation vectors estimated for each pattern view.</param>
        /// <param name="stdDeviationsIntrinsics">Output vector of standard deviations estimated for intrinsic parameters. Order of deviations values: (fx,fy,cx,cy,k1,k2,p1,p2,k3,k4,k5,k6,s1,s2,s3,s4,τx,τy) If one of parameters is not estimated, it's deviation is equals to zero.</param>
        /// <param name="stdDeviationsExtrinsics">Output vector of standard deviations estimated for extrinsic parameters. Order of deviations values: (R1,T1,…,RM,TM) where M is number of pattern views, Ri,Ti are concatenated 1x3 vectors.</param>
        /// <param name="perViewErrors">Output vector of average re-projection errors estimated for each pattern view.</param>
        /// <param name="flags">Flags Different flags for the calibration process</param>
        /// <param name="criteria">Termination criteria for the iterative optimization algorithm.</param>
        /// <returns>The final re-projection error.</returns>
        public static double CalibrateCameraAruco(
           IInputArrayOfArrays corners, IInputArray ids, IInputArray counter, IBoard board, Size imageSize,
           IInputOutputArray cameraMatrix, IInputOutputArray distCoeffs, IOutputArray rvecs, IOutputArray tvecs,
           IOutputArray stdDeviationsIntrinsics,
           IOutputArray stdDeviationsExtrinsics,
           IOutputArray perViewErrors,
           CalibType flags, MCvTermCriteria criteria)
        {
            using (InputArray iaCorners = corners.GetInputArray())
            using (InputArray iaIds = ids.GetInputArray())
            using (InputArray iaCounter = counter.GetInputArray())
            using (InputOutputArray ioaCameraMatrix = cameraMatrix.GetInputOutputArray())
            using (InputOutputArray ioaDistCoeffs = distCoeffs.GetInputOutputArray())
            using (OutputArray oaRvecs = rvecs == null ? OutputArray.GetEmpty() : rvecs.GetOutputArray())
            using (OutputArray oaTvecs = tvecs == null ? OutputArray.GetEmpty() : tvecs.GetOutputArray())
            using (OutputArray oaStdDeviationsIntrinsics = stdDeviationsIntrinsics == null ? OutputArray.GetEmpty() : stdDeviationsIntrinsics.GetOutputArray())
            using (OutputArray oaStdDeviationsExtrinsics = stdDeviationsExtrinsics == null ? OutputArray.GetEmpty() : stdDeviationsExtrinsics.GetOutputArray())
            using (OutputArray oaPerViewErrors = perViewErrors == null ? OutputArray.GetEmpty() : perViewErrors.GetOutputArray())
            {
                return cveArucoCalibrateCameraAruco(iaCorners, iaIds, iaCounter, board.BoardPtr, ref imageSize,
                   ioaCameraMatrix, ioaDistCoeffs, oaRvecs, oaTvecs,
                   oaStdDeviationsIntrinsics, oaStdDeviationsExtrinsics, oaPerViewErrors,
                   flags, ref criteria);
            }
        }

        /// <summary>
        /// Calibrate a camera using Charuco corners.
        /// </summary>
        /// <param name="charucoCorners">Vector of detected charuco corners per frame</param>
        /// <param name="charucoIds">List of identifiers for each corner in charucoCorners per frame</param>
        /// <param name="board">Marker Board layout</param>
        /// <param name="imageSize">Size of the image used only to initialize the intrinsic camera matrix.</param>
        /// <param name="cameraMatrix">Output 3x3 floating-point camera matrix. </param>
        /// <param name="distCoeffs">Output vector of distortion coefficients (k1,k2,p1,p2[,k3[,k4,k5,k6],[s1,s2,s3,s4]]) of 4, 5, 8 or 12 elements</param>
        /// <param name="rvecs">Output vector of rotation vectors (see Rodrigues ) estimated for each board view (e.g. std::vector&lt;cv::Mat&gt;). That is, each k-th rotation vector together with the corresponding k-th translation vector (see the next output parameter description) brings the board pattern from the model coordinate space (in which object points are specified) to the world coordinate space, that is, a real position of the board pattern in the k-th pattern view (k=0.. M -1).</param>
        /// <param name="tvecs">Output vector of translation vectors estimated for each pattern view.</param>
        /// <param name="flags">Flags Different flags for the calibration process</param>
        /// <param name="criteria">Termination criteria for the iterative optimization algorithm.</param>
        /// <returns>The final re-projection error.</returns>
        public static double CalibrateCameraCharuco(
            IInputArrayOfArrays charucoCorners,
            IInputArrayOfArrays charucoIds,
            CharucoBoard board,
            Size imageSize,
            IInputOutputArray cameraMatrix,
            IInputOutputArray distCoeffs,
            IOutputArray rvecs,
            IOutputArray tvecs,
            CalibType flags,
            MCvTermCriteria criteria)
        {
            return CalibrateCameraCharuco(charucoCorners, charucoIds, board, imageSize, cameraMatrix, distCoeffs,
                rvecs, tvecs, null, null, null, flags, criteria);
        }

        /// <summary>
        /// Calibrate a camera using Charuco corners.
        /// </summary>
        /// <param name="charucoCorners">Vector of detected charuco corners per frame</param>
        /// <param name="charucoIds">List of identifiers for each corner in charucoCorners per frame</param>
        /// <param name="board">Marker Board layout</param>
        /// <param name="imageSize">Size of the image used only to initialize the intrinsic camera matrix.</param>
        /// <param name="cameraMatrix">Output 3x3 floating-point camera matrix. </param>
        /// <param name="distCoeffs">Output vector of distortion coefficients (k1,k2,p1,p2[,k3[,k4,k5,k6],[s1,s2,s3,s4]]) of 4, 5, 8 or 12 elements</param>
        /// <param name="rvecs">Output vector of rotation vectors (see Rodrigues ) estimated for each board view (e.g. std::vector&lt;cv::Mat&gt;). That is, each k-th rotation vector together with the corresponding k-th translation vector (see the next output parameter description) brings the board pattern from the model coordinate space (in which object points are specified) to the world coordinate space, that is, a real position of the board pattern in the k-th pattern view (k=0.. M -1).</param>
        /// <param name="tvecs">Output vector of translation vectors estimated for each pattern view.</param>
        /// <param name="stdDeviationsIntrinsics">Output vector of standard deviations estimated for intrinsic parameters. Order of deviations values: (fx,fy,cx,cy,k1,k2,p1,p2,k3,k4,k5,k6,s1,s2,s3,s4,τx,τy) If one of parameters is not estimated, it's deviation is equals to zero.</param>
        /// <param name="stdDeviationsExtrinsics">Output vector of standard deviations estimated for extrinsic parameters. Order of deviations values: (R1,T1,…,RM,TM) where M is number of pattern views, Ri,Ti are concatenated 1x3 vectors.</param>
        /// <param name="perViewErrors">Output vector of average re-projection errors estimated for each pattern view.</param>
        /// <param name="flags">Flags Different flags for the calibration process</param>
        /// <param name="criteria">Termination criteria for the iterative optimization algorithm.</param>
        /// <returns>The final re-projection error.</returns>
        public static double CalibrateCameraCharuco(
            IInputArrayOfArrays charucoCorners,
            IInputArrayOfArrays charucoIds,
            CharucoBoard board,
            Size imageSize,
            IInputOutputArray cameraMatrix,
            IInputOutputArray distCoeffs,
            IOutputArray rvecs,
            IOutputArray tvecs,
            IOutputArray stdDeviationsIntrinsics,
            IOutputArray stdDeviationsExtrinsics,
            IOutputArray perViewErrors,
            CalibType flags,
            MCvTermCriteria criteria)
        {
            using (InputArray iaCharucoCorners = charucoCorners.GetInputArray())
            using (InputArray iaCharucoIds = charucoIds.GetInputArray())
            using (InputOutputArray ioaCameraMatrix = cameraMatrix.GetInputOutputArray())
            using (InputOutputArray ioaDistCoeffs = distCoeffs.GetInputOutputArray())
            using (OutputArray oaRvecs = rvecs == null ? OutputArray.GetEmpty() : rvecs.GetOutputArray())
            using (OutputArray oaTvecs = tvecs == null ? OutputArray.GetEmpty() : tvecs.GetOutputArray())
            using (OutputArray oaStdDeviationsIntrinsics = stdDeviationsIntrinsics == null ? OutputArray.GetEmpty() : stdDeviationsIntrinsics.GetOutputArray())
            using (OutputArray oaStdDeviationsExtrinsics = stdDeviationsExtrinsics == null ? OutputArray.GetEmpty() : stdDeviationsExtrinsics.GetOutputArray())
            using (OutputArray oaPerViewErrors = perViewErrors == null ? OutputArray.GetEmpty() : perViewErrors.GetOutputArray())
            {
                return cveArucoCalibrateCameraCharuco(
                    iaCharucoCorners, iaCharucoIds, board.BoardPtr, ref imageSize,
                   ioaCameraMatrix, ioaDistCoeffs, oaRvecs, oaTvecs,
                   oaStdDeviationsIntrinsics, oaStdDeviationsExtrinsics, oaPerViewErrors,
                   flags, ref criteria);
            }
        }

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern double cveArucoCalibrateCameraAruco(
           IntPtr corners,
           IntPtr ids,
           IntPtr counter,
           IntPtr board,
           ref Size imageSize,
           IntPtr cameraMatrix,
           IntPtr distCoeffs,
           IntPtr rvecs,
           IntPtr tvecs,
           IntPtr stdDeviationsIntrinsics,
           IntPtr stdDeviationsExtrinsics,
           IntPtr perViewErrors,
           CalibType flags,
           ref MCvTermCriteria criteria);

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern double cveArucoCalibrateCameraCharuco(
            IntPtr charucoCorners,
            IntPtr charucoIds,
            IntPtr board,
            ref Size imageSize,
            IntPtr cameraMatrix,
            IntPtr distCoeffs,
            IntPtr rvecs,
            IntPtr tvecs,
            IntPtr stdDeviationsIntrinsics,
            IntPtr stdDeviationsExtrinsics,
            IntPtr perViewErrors,
            CalibType flags,
            ref MCvTermCriteria criteria);

        /// <summary>
        /// Interpolate position of ChArUco board corners
        /// </summary>
        /// <param name="markerCorners">vector of already detected markers corners. For each marker, its four corners are provided, (e.g VectorOfVectorOfPointF ). For N detected markers, the dimensions of this array should be Nx4.The order of the corners should be clockwise.</param>
        /// <param name="markerIds">list of identifiers for each marker in corners</param>
        /// <param name="image">input image necesary for corner refinement. Note that markers are not detected and should be sent in corners and ids parameters.</param>
        /// <param name="board">layout of ChArUco board.</param>
        /// <param name="charucoCorners">interpolated chessboard corners</param>
        /// <param name="charucoIds">interpolated chessboard corners identifiers</param>
        /// <param name="cameraMatrix">optional 3x3 floating-point camera matrix</param>
        /// <param name="distCoeffs">optional vector of distortion coefficients, (k_1, k_2, p_1, p_2[, k_3[, k_4, k_5, k_6],[s_1, s_2, s_3, s_4]]) of 4, 5, 8 or 12 elements </param>
        /// <param name="minMarkers">number of adjacent markers that must be detected to return a charuco corner</param>
        /// <returns>The number of interpolated corners.</returns>
        public static int InterpolateCornersCharuco(
            IInputArrayOfArrays markerCorners,
            IInputArray markerIds,
            IInputArray image,
            CharucoBoard board,
            IOutputArray charucoCorners,
            IOutputArray charucoIds,
            IInputArray cameraMatrix = null,
            IInputArray distCoeffs = null,
            int minMarkers = 2)
        {
            using (InputArray iaMarkerCorners = markerCorners.GetInputArray())
            using (InputArray iaMarkerIds = markerIds.GetInputArray())
            using (InputArray iaImage = image.GetInputArray())
            using (OutputArray oaCharucoCorners = charucoCorners.GetOutputArray())
            using (OutputArray oaCharucoIds = charucoIds.GetOutputArray())
            using (InputArray iaCameraMatrix = cameraMatrix == null ? InputArray.GetEmpty() : cameraMatrix.GetInputArray())
            using (InputArray iaDistCoeffs = distCoeffs == null ? InputArray.GetEmpty() : distCoeffs.GetInputArray())
            {
                return cveArucoInterpolateCornersCharuco(
                    iaMarkerCorners, iaMarkerIds, iaImage, board,
                    oaCharucoCorners, oaCharucoIds,
                    iaCameraMatrix, iaDistCoeffs,
                    minMarkers);
            }
        }
        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern int cveArucoInterpolateCornersCharuco(
            IntPtr markerCorners,
            IntPtr markerIds,
            IntPtr image,
            IntPtr board,
            IntPtr charucoCorners,
            IntPtr charucoIds,
            IntPtr cameraMatrix,
            IntPtr distCoeffs,
            int minMarkers);

        /// <summary>
        /// Draws a set of Charuco corners
        /// </summary>
        /// <param name="image">image input/output image. It must have 1 or 3 channels. The number of channels is not altered.</param>
        /// <param name="charucoCorners">vector of detected charuco corners</param>
        /// <param name="charucoIds">list of identifiers for each corner in charucoCorners</param>
        /// <param name="cornerColor">color of the square surrounding each corner</param>
        public static void DrawDetectedCornersCharuco(
            IInputOutputArray image,
            IInputArray charucoCorners,
            IInputArray charucoIds,
            MCvScalar cornerColor)
        {
            using (InputOutputArray ioaImage = image.GetInputOutputArray())
            using (InputArray iaCharucoCorners = charucoCorners.GetInputArray())
            using (InputArray iaCharucoIds = charucoIds == null ? InputArray.GetEmpty() : charucoIds.GetInputArray())
            {
                cveArucoDrawDetectedCornersCharuco(ioaImage, iaCharucoCorners, iaCharucoIds, ref cornerColor);
            }
        }
        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern void cveArucoDrawDetectedCornersCharuco(
            IntPtr image,
            IntPtr charucoCorners,
            IntPtr charucoIds,
            ref MCvScalar cornerColor);

        /// <summary>
        ///  Pose estimation for a ChArUco board given some of their corners
        /// </summary>
        /// <param name="charucoCorners">vector of detected charuco corners</param>
        /// <param name="charucoIds">list of identifiers for each corner in charucoCorners</param>
        /// <param name="board">layout of ChArUco board.</param>
        /// <param name="cameraMatrix">input 3x3 floating-point camera matrix</param>
        /// <param name="distCoeffs">vector of distortion coefficients, 4, 5, 8 or 12 elements</param>
        /// <param name="rvec">Output vector (e.g. cv::Mat) corresponding to the rotation vector of the board</param>
        /// <param name="tvec">Output vector (e.g. cv::Mat) corresponding to the translation vector of the board.</param>
        /// <param name="useExtrinsicGuess">defines whether initial guess for rvec and  tvec will be used or not.</param>
        /// <returns>If pose estimation is valid, returns true, else returns false.</returns>
        public static bool EstimatePoseCharucoBoard(
            IInputArray charucoCorners,
            IInputArray charucoIds,
            CharucoBoard board,
            IInputArray cameraMatrix,
            IInputArray distCoeffs,
            IInputOutputArray rvec,
            IInputOutputArray tvec,
            bool useExtrinsicGuess = false)
        {
            using (InputArray iaCharucoCorners = charucoCorners.GetInputArray())
            using (InputArray iaCharucoIds = charucoIds.GetInputArray())
            using (InputArray iaCameraMatrix = cameraMatrix.GetInputArray())
            using (InputArray iaDistCoeffs = distCoeffs.GetInputArray())
            using (InputOutputArray ioaRvec = rvec.GetInputOutputArray())
            using (InputOutputArray ioaTvec = tvec.GetInputOutputArray())
            {
                return cveArucoEstimatePoseCharucoBoard(
                    iaCharucoCorners,
                    iaCharucoIds,
                    board,
                    iaCameraMatrix,
                    iaDistCoeffs,
                    ioaRvec,
                    ioaTvec,
                    useExtrinsicGuess);
            }
        }

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        [return: MarshalAs(CvInvoke.BoolMarshalType)]
        internal static extern bool cveArucoEstimatePoseCharucoBoard(
            IntPtr charucoCorners,
            IntPtr charucoIds,
            IntPtr board,
            IntPtr cameraMatrix,
            IntPtr distCoeffs,
            IntPtr rvec,
            IntPtr tvec,
            [MarshalAs(CvInvoke.BoolMarshalType)]
            bool useExtrinsicGuess);

        /// <summary>
        /// Detect ChArUco Diamond markers
        /// </summary>
        /// <param name="image">input image necessary for corner subpixel.</param>
        /// <param name="markerCorners">list of detected marker corners from detectMarkers function.</param>
        /// <param name="markerIds">list of marker ids in markerCorners.</param>
        /// <param name="squareMarkerLengthRate">rate between square and marker length: squareMarkerLengthRate = squareLength / markerLength.The real units are not necessary.</param>
        /// <param name="diamondCorners">output list of detected diamond corners (4 corners per diamond). The order is the same than in marker corners: top left, top right, bottom right and bottom left. Similar format than the corners returned by detectMarkers(e.g VectorOfVectorOfPointF ).</param>
        /// <param name="diamondIds">ids of the diamonds in diamondCorners. The id of each diamond is in fact of type Vec4i, so each diamond has 4 ids, which are the ids of the aruco markers composing the diamond.</param>
        /// <param name="cameraMatrix">Optional camera calibration matrix.</param>
        /// <param name="distCoeffs">Optional camera distortion coefficients.</param>
        public static void DetectCharucoDiamond(
            IInputArray image,
            IInputArray markerCorners,
            IInputArray markerIds,
            float squareMarkerLengthRate,
            IOutputArray diamondCorners,
            IOutputArray diamondIds,
            IInputArray cameraMatrix = null,
            IInputArray distCoeffs = null)
        {
            using (InputArray iaImage = image.GetInputArray())
            using (InputArray iaMarkerCorners = markerCorners.GetInputArray())
            using (InputArray iaMarkerIds = markerIds.GetInputArray())
            using (OutputArray oaDiamondCorners = diamondCorners.GetOutputArray())
            using (OutputArray oaDiamondIds = diamondIds.GetOutputArray())
            using (InputArray iaCameraMatrix = cameraMatrix == null ? InputArray.GetEmpty() : cameraMatrix.GetInputArray())
            using (InputArray iaDistCoeffs = distCoeffs == null ? InputArray.GetEmpty() : distCoeffs.GetInputArray())
            {
                cveArucoDetectCharucoDiamond(iaImage, iaMarkerCorners, iaMarkerIds, squareMarkerLengthRate, oaDiamondCorners, oaDiamondIds, iaCameraMatrix, iaDistCoeffs);
            }
        }

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern void cveArucoDetectCharucoDiamond(
            IntPtr image,
            IntPtr markerCorners,
            IntPtr markerIds,
            float squareMarkerLengthRate,
            IntPtr diamondCorners,
            IntPtr diamondIds,
            IntPtr cameraMatrix,
            IntPtr distCoeffs);

        /// <summary>
        /// Draw a set of detected ChArUco Diamond markers
        /// </summary>
        /// <param name="image">input/output image. It must have 1 or 3 channels. The number of channels is not altered.</param>
        /// <param name="diamondCorners">positions of diamond corners in the same format returned by detectCharucoDiamond(). (e.g VectorOfVectorOfPointF ). For N detected markers, the dimensions of this array should be Nx4. The order of the corners should be clockwise.</param>
        /// <param name="diamondIds">vector of identifiers for diamonds in diamondCorners, in the same format returned by detectCharucoDiamond() (e.g. VectorOfMat ). Optional, if not provided, ids are not painted. </param>
        /// <param name="borderColor">color of marker borders. Rest of colors (text color and first corner color) are calculated based on this one.</param>
        public static void DrawDetectedDiamonds(
            IInputOutputArray image,
            IInputArrayOfArrays diamondCorners,
            IInputArray diamondIds,
            MCvScalar borderColor)
        {
            using (InputOutputArray ioaImage = image.GetInputOutputArray())
            using (InputArray iaDiamondCorners = diamondCorners.GetInputArray())
            using (InputArray iaDiamondIds = diamondIds == null ? InputArray.GetEmpty() : diamondIds.GetInputArray())
            {
                cveArucoDrawDetectedDiamonds(ioaImage, iaDiamondCorners, iaDiamondIds, ref borderColor);
            }
        }

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern void cveArucoDrawDetectedDiamonds(
            IntPtr image,
            IntPtr diamondCorners,
            IntPtr diamondIds,
            ref MCvScalar borderColor);

        /// <summary>
        /// Draw a ChArUco Diamond marker
        /// </summary>
        /// <param name="dictionary">dictionary of markers indicating the type of markers.</param>
        /// <param name="ids">list of 4 ids for each ArUco marker in the ChArUco marker.</param>
        /// <param name="squareLength">size of the chessboard squares in pixels.</param>
        /// <param name="markerLength">size of the markers in pixels.</param>
        /// <param name="img">output image with the marker. The size of this image will be 3*squareLength + 2*marginSize.</param>
        /// <param name="marginSize">minimum margins (in pixels) of the marker in the output image</param>
        /// <param name="borderBits">width of the marker borders.</param>
        public static void DrawCharucoDiamond(
            Dictionary dictionary,
            int[] ids,
            int squareLength,
            int markerLength,
            IOutputArray img,
            int marginSize = 0,
            int borderBits = 1)
        {
            Debug.Assert(ids.Length == 4, "The ids should contain 4 interger values");
            GCHandle handle = GCHandle.Alloc(ids, GCHandleType.Pinned);
            using (OutputArray oaImg = img.GetOutputArray())
                cveArucoDrawCharucoDiamond(dictionary, handle.AddrOfPinnedObject(), squareLength, markerLength, oaImg, marginSize, borderBits);
            handle.Free();
        }

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern void cveArucoDrawCharucoDiamond(
            IntPtr dictionary,
            IntPtr ids,
            int squareLength,
            int markerLength,
            IntPtr img,
            int marginSize,
            int borderBits);

        /// <summary>
        /// Draw a planar board.
        /// </summary>
        /// <param name="board">Layout of the board that will be drawn. The board should be planar, z coordinate is ignored</param>
        /// <param name="outSize">Size of the output image in pixels.</param>
        /// <param name="img">Output image with the board. The size of this image will be outSize and the board will be on the center, keeping the board proportions.</param>
        /// <param name="marginSize">Minimum margins (in pixels) of the board in the output image</param>
        /// <param name="borderBits">Width of the marker borders.</param>
        public static void DrawPlanarBoard(
            IBoard board,
            Size outSize,
            IOutputArray img,
            int marginSize = 0,
            int borderBits = 1)
        {
            using (OutputArray oaImg = img.GetOutputArray())
            {
                cveArucoDrawPlanarBoard(board.BoardPtr, ref outSize, oaImg, marginSize, borderBits);
            }
        }

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern void cveArucoDrawPlanarBoard(
            IntPtr board,
            ref Size outSize,
            IntPtr img,
            int marginSize,
            int borderBits);

        /// <summary>
        /// Pose estimation for a board of markers.
        /// </summary>
        /// <param name="corners">Vector of already detected markers corners. For each marker, its four corners are provided, (e.g std::vector&gt;std::vector&gt;cv::Point2f&lt; &lt; ). For N detected markers, the dimensions of this array should be Nx4. The order of the corners should be clockwise.</param>
        /// <param name="ids">List of identifiers for each marker in corners</param>
        /// <param name="board">Layout of markers in the board. The layout is composed by the marker identifiers and the positions of each marker corner in the board reference system.</param>
        /// <param name="cameraMatrix">Input 3x3 floating-point camera matrix</param>
        /// <param name="distCoeffs">Vector of distortion coefficients (k1,k2,p1,p2[,k3[,k4,k5,k6],[s1,s2,s3,s4]]) of 4, 5, 8 or 12 elements</param>
        /// <param name="rvec">Output vector (e.g. cv::Mat) corresponding to the rotation vector of the board (see cv::Rodrigues). Used as initial guess if not empty.</param>
        /// <param name="tvec">Output vector (e.g. cv::Mat) corresponding to the translation vector of the board.</param>
        /// <param name="useExtrinsicGuess">Defines whether initial guess for rvec and tvec will be used or not. Used as initial guess if not empty.</param>
        /// <returns>The function returns the number of markers from the input employed for the board pose estimation. Note that returning a 0 means the pose has not been estimated.</returns>
        public static int EstimatePoseBoard(
            IInputArrayOfArrays corners,
            IInputArray ids,
            IBoard board,
            IInputArray cameraMatrix,
            IInputArray distCoeffs,
            IInputOutputArray rvec,
            IInputOutputArray tvec,
            bool useExtrinsicGuess = false)
        {
            using (InputArray iaCorners = corners.GetInputArray())
            using (InputArray iaIds = ids.GetInputArray())
            using (InputArray iaCameraMatrix = cameraMatrix.GetInputArray())
            using (InputArray iaDistCoeffs = distCoeffs.GetInputArray())
            using (InputOutputArray ioaRvec = rvec.GetInputOutputArray())
            using (InputOutputArray ioaTvec = tvec.GetInputOutputArray())
            {
                return cveArucoEstimatePoseBoard(iaCorners, iaIds, board.BoardPtr, iaCameraMatrix, iaDistCoeffs, ioaRvec,
                    ioaTvec, useExtrinsicGuess);
            }
        }

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern int cveArucoEstimatePoseBoard(
            IntPtr corners,
            IntPtr ids,
            IntPtr board,
            IntPtr cameraMatrix,
            IntPtr distCoeffs,
            IntPtr rvec,
            IntPtr tvec,
            [MarshalAs(CvInvoke.BoolMarshalType)]
            bool useExtrinsicGuess);

        /// <summary>
        /// Given a board configuration and a set of detected markers, returns the corresponding image points and object points to call solvePnP.
        /// </summary>
        /// <param name="board">Marker board layout.</param>
        /// <param name="detectedCorners">List of detected marker corners of the board.</param>
        /// <param name="detectedIds">List of identifiers for each marker.</param>
        /// <param name="objPoints">Vector of vectors of board marker points in the board coordinate space.</param>
        /// <param name="imgPoints">Vector of vectors of the projections of board marker corner points.</param>
        public static void GetBoardObjectAndImagePoints(
            IBoard board,
            IInputArray detectedCorners,
            IInputArray detectedIds,
            IOutputArray objPoints,
            IOutputArray imgPoints)
        {
            using (InputArray iaDetectedCorners = detectedCorners.GetInputArray())
            using (InputArray iaDetectedIds = detectedIds.GetInputArray())
            using (OutputArray oaObjPoints = objPoints.GetOutputArray())
            using (OutputArray oaImgPoints = imgPoints.GetOutputArray())
            {
                cveArucoGetBoardObjectAndImagePoints(
                    board.BoardPtr,
                    iaDetectedCorners,
                    iaDetectedIds,
                    oaObjPoints,
                    oaImgPoints);
            }
        }

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern void cveArucoGetBoardObjectAndImagePoints(
            IntPtr board,
            IntPtr detectedCorners,
            IntPtr detectedIds,
            IntPtr objPoints,
            IntPtr imgPoints);
            
    }
}