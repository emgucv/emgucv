//----------------------------------------------------------------------------
//  Copyright (C) 2004-2012 by EMGU. All rights reserved.       
//----------------------------------------------------------------------------

﻿using System;
using System.Runtime.InteropServices;
using System.Drawing;
using Emgu.CV.Structure;

namespace Emgu.CV
{
   public static partial class CvInvoke
   {
      /// <summary>
      /// Finds perspective transformation H=||hij|| between the source and the destination planes
      /// </summary>
      /// <param name="srcPoints">Point coordinates in the original plane, 2xN, Nx2, 3xN or Nx3 array (the latter two are for representation in homogeneous coordinates), where N is the number of points. </param>
      /// <param name="dstPoints">Point coordinates in the destination plane, 2xN, Nx2, 3xN or Nx3 array (the latter two are for representation in homogeneous coordinates) </param>
      /// <param name="homography">Output 3x3 homography matrix. Homography matrix is determined up to a scale, thus it is normalized to make h33=1</param>
      /// <param name="method">The type of the method</param>
      /// <param name="ransacReprojThreshold">The maximum allowed reprojection error to treat a point pair as an inlier. The parameter is only used in RANSAC-based homography estimation. E.g. if dst_points coordinates are measured in pixels with pixel-accurate precision, it makes sense to set this parameter somewhere in the range ~1..3</param>
      /// <param name="mask">The optional output mask set by a robust method (RANSAC or LMEDS). </param>
      /// <returns>True if the homography matrix is found, false otherwise.</returns>
      [DllImport(OPENCV_CALIB3D_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      [return: MarshalAs(CvInvoke.BoolToIntMarshalType)]
      public static extern bool cvFindHomography(
         IntPtr srcPoints,
         IntPtr dstPoints,
         IntPtr homography,
         CvEnum.HOMOGRAPHY_METHOD method,
         double ransacReprojThreshold,
         IntPtr mask);

      /// <summary>
      /// Converts a rotation vector to rotation matrix or vice versa. Rotation vector is a compact representation of rotation matrix. Direction of the rotation vector is the rotation axis and the length of the vector is the rotation angle around the axis. 
      /// </summary>
      /// <param name="src">The input rotation vector (3x1 or 1x3) or rotation matrix (3x3). </param>
      /// <param name="dst">The output rotation matrix (3x3) or rotation vector (3x1 or 1x3), respectively</param>
      /// <param name="jacobian">Optional output Jacobian matrix, 3x9 or 9x3 - partial derivatives of the output array components w.r.t the input array components</param>
      /// <returns>True if the conversion is sucessful, false otherwise</returns>
      [DllImport(OPENCV_CALIB3D_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      [return: MarshalAs(CvInvoke.BoolToIntMarshalType)]
      public static extern bool cvRodrigues2(IntPtr src, IntPtr dst, IntPtr jacobian);


      #region Epipolar Geometry, Stereo Correspondence
      /// <summary>
      /// Calculates fundamental matrix using one of four methods listed above and returns the number of fundamental matrices found (1 or 3) and 0, if no matrix is found. 
      /// </summary>
      /// <param name="points1">Array of the first image points of 2xN, Nx2, 3xN or Nx3 size (where N is number of points). Multi-channel 1xN or Nx1 array is also acceptable. The point coordinates should be floating-point (single or double precision) </param>
      /// <param name="points2">Array of the second image points of the same size and format as points1</param>
      /// <param name="fundamentalMatrix">The output fundamental matrix or matrices. The size should be 3x3 or 9x3 (7-point method may return up to 3 matrices).</param>
      /// <param name="method">Method for computing the fundamental matrix </param>
      /// <param name="param1">Use 3.0 for default. The parameter is used for RANSAC method only. It is the maximum distance from point to epipolar line in pixels, beyond which the point is considered an outlier and is not used for computing the final fundamental matrix. Usually it is set somewhere from 1 to 3. </param>
      /// <param name="param2">Use 0.99 for default. The parameter is used for RANSAC or LMedS methods only. It denotes the desirable level of confidence of the fundamental matrix estimate. </param>
      /// <param name="status">The optional pointer to output array of N elements, every element of which is set to 0 for outliers and to 1 for the "inliers", i.e. points that comply well with the estimated epipolar geometry. The array is computed only in RANSAC and LMedS methods. For other methods it is set to all 1.</param>
      /// <returns>The number of fundamental matrices found (1 or 3) and 0, if no matrix is found. </returns>
      [DllImport(OPENCV_CALIB3D_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      public static extern int cvFindFundamentalMat(IntPtr points1,
         IntPtr points2,
         IntPtr fundamentalMatrix,
         CvEnum.CV_FM method,
         double param1,
         double param2,
         IntPtr status);

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
      [DllImport(OPENCV_CALIB3D_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      public static extern void cvComputeCorrespondEpilines(
         IntPtr points,
         int whichImage,
         IntPtr fundamentalMatrix,
         IntPtr correspondentLines);

      /// <summary>
      /// Converts 2D or 3D points from/to homogeneous coordinates, or simply copies or transposes the array. In case if the input array dimensionality is larger than the output, each point coordinates are divided by the last coordinate
      /// </summary>
      /// <param name="src">The input point array, 2xN, Nx2, 3xN, Nx3, 4xN or Nx4 (where N is the number of points). Multi-channel 1xN or Nx1 array is also acceptable</param>
      /// <param name="dst">The output point array, must contain the same number of points as the input; The dimensionality must be the same, 1 less or 1 more than the input, and also within 2..4.</param>
      [DllImport(OPENCV_CALIB3D_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      public static extern void cvConvertPointsHomogeneous(IntPtr src, IntPtr dst);

      /// <summary>
      /// Creates the stereo correspondence structure and initializes it. It is possible to override any of the parameters at any time between the calls to cvFindStereoCorrespondenceBM
      /// </summary>
      /// <param name="type">ID of one of the pre-defined parameter sets. Any of the parameters can be overridden after creating the structure.</param>
      /// <param name="numberOfDisparities">The number of disparities. If the parameter is 0, it is taken from the preset, otherwise the supplied value overrides the one from preset. </param>
      /// <returns>Pointer to the stereo correspondece structure</returns>
      [DllImport(OPENCV_CALIB3D_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      public static extern IntPtr cvCreateStereoBMState(
         CvEnum.STEREO_BM_TYPE type,
         int numberOfDisparities);

      /// <summary>
      /// Releases the stereo correspondence structure and all the associated internal buffers
      /// </summary>
      /// <param name="state">The state to be released</param>
      [DllImport(OPENCV_CALIB3D_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      public static extern void cvReleaseStereoBMState(ref IntPtr state);

      /// <summary>
      /// Computes disparity map for the input rectified stereo pair.
      /// </summary>
      /// <param name="left">The left single-channel, 8-bit image</param>
      /// <param name="right">The right image of the same size and the same type</param>
      /// <param name="disparity">The output single-channel 16-bit signed disparity map of the same size as input images. Its elements will be the computed disparities, multiplied by 16 and rounded to integer's</param>
      /// <param name="state">Stereo correspondence structure</param>
      [DllImport(OPENCV_CALIB3D_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      public static extern void cvFindStereoCorrespondenceBM(
         IntPtr left,
         IntPtr right,
         IntPtr disparity,
         IntPtr state);

      /// <summary>
      /// Computes disparity map for the input rectified stereo pair.
      /// </summary>
      /// <param name="left">The left single-channel, 8-bit image</param>
      /// <param name="right">The right image of the same size and the same type</param>
      /// <param name="disparity">The output single-channel 16-bit signed disparity map of the same size as input images. Its elements will be the computed disparities, multiplied by 16 and rounded to integer's</param>
      /// <param name="state">Stereo correspondence structure</param>
      [DllImport(OPENCV_CALIB3D_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      public static extern void cvFindStereoCorrespondenceBM(
         IntPtr left,
         IntPtr right,
         IntPtr disparity,
         ref MCvStereoBMState state);

      /// <summary>
      /// Transforms 1-channel disparity map to 3-channel image, a 3D surface.
      /// </summary>
      /// <param name="disparity">Disparity map</param>
      /// <param name="image3D">3-channel, 16-bit integer or 32-bit floating-point image - the output map of 3D points</param>
      /// <param name="Q">The reprojection 4x4 matrix, can be arbitrary, e.g. the one, computed by cvStereoRectify</param>
      [DllImport(OPENCV_CALIB3D_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      public static extern void cvReprojectImageTo3D(
         IntPtr disparity,
         IntPtr image3D,
         IntPtr Q);
      #endregion

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
      [DllImport(OPENCV_CALIB3D_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      public extern static void cvFindStereoCorrespondence(
         IntPtr leftImage, IntPtr rightImage,
         int mode, IntPtr depthImage,
         int maxDisparity,
         double param1, double param2, double param3,
         double param4, double param5);

      #region Camera Calibration
      /// <summary>
      /// Computes projections of 3D points to the image plane given intrinsic and extrinsic camera parameters. Optionally, the function computes jacobians - matrices of partial derivatives of image points as functions of all the input parameters w.r.t. the particular parameters, intrinsic and/or extrinsic. The jacobians are used during the global optimization in cvCalibrateCamera2 and cvFindExtrinsicCameraParams2. The function itself is also used to compute back-projection error for with current intrinsic and extrinsic parameters.
      /// Note, that with intrinsic and/or extrinsic parameters set to special values, the function can be used to compute just extrinsic transformation or just intrinsic transformation (i.e. distortion of a sparse set of points). 
      /// </summary>
      /// <param name="objectPoints">The array of object points, 3xN or Nx3, where N is the number of points in the view</param>
      /// <param name="rotationVector">The rotation vector, 1x3 or 3x1</param>
      /// <param name="translationVector">The translation vector, 1x3 or 3x1</param>
      /// <param name="intrinsicMatrix">The camera matrix (A) [fx 0 cx; 0 fy cy; 0 0 1]. </param>
      /// <param name="distortionCoeffs">The vector of distortion coefficients, 4x1 or 1x4 [k1, k2, p1, p2]. If it is IntPtr.Zero, all distortion coefficients are considered 0's</param>
      /// <param name="imagePoints">The output array of image points, 2xN or Nx2, where N is the total number of points in the view</param>
      /// <param name="dpdrot">Optional Nx3 matrix of derivatives of image points with respect to components of the rotation vector</param>
      /// <param name="dpdt">Optional Nx3 matrix of derivatives of image points w.r.t. components of the translation vector</param>
      /// <param name="dpdf">Optional Nx2 matrix of derivatives of image points w.r.t. fx and fy</param>
      /// <param name="dpdc">Optional Nx2 matrix of derivatives of image points w.r.t. cx and cy</param>
      /// <param name="dpddist">Optional Nx4 matrix of derivatives of image points w.r.t. distortion coefficients</param>
      /// <param name="aspectRatio">Aspect ratio</param>
      [DllImport(OPENCV_CALIB3D_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      public static extern void cvProjectPoints2(
         IntPtr objectPoints,
         IntPtr rotationVector,
         IntPtr translationVector,
         IntPtr intrinsicMatrix,
         IntPtr distortionCoeffs,
         IntPtr imagePoints,
         IntPtr dpdrot,
         IntPtr dpdt,
         IntPtr dpdf,
         IntPtr dpdc,
         IntPtr dpddist,
         double aspectRatio);

      /// <summary>
      /// Estimates intrinsic camera parameters and extrinsic parameters for each of the views
      /// </summary>
      /// <param name="objectPoints">The joint matrix of object points, 3xN or Nx3, where N is the total number of points in all views</param>
      /// <param name="imagePoints">The joint matrix of corresponding image points, 2xN or Nx2, where N is the total number of points in all views</param>
      /// <param name="pointCounts">Vector containing numbers of points in each particular view, 1xM or Mx1, where M is the number of a scene views</param>
      /// <param name="imageSize">Size of the image, used only to initialize intrinsic camera matrix</param>
      /// <param name="intrinsicMatrix">The output camera matrix (A) [fx 0 cx; 0 fy cy; 0 0 1]. If CV_CALIB_USE_INTRINSIC_GUESS and/or CV_CALIB_FIX_ASPECT_RATION are specified, some or all of fx, fy, cx, cy must be initialized</param>
      /// <param name="distortionCoeffs">The output 4x1 or 1x4 vector of distortion coefficients [k1, k2, p1, p2]</param>
      /// <param name="rotationVectors">The output 3xM or Mx3 array of rotation vectors (compact representation of rotation matrices, see cvRodrigues2). </param>
      /// <param name="translationVectors">The output 3xM or Mx3 array of translation vectors</param>
      /// <param name="flags">Different flags</param>
      /// <returns>The final reprojection error</returns>
      [DllImport(OPENCV_CALIB3D_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      public static extern double cvCalibrateCamera2(
          IntPtr objectPoints,
          IntPtr imagePoints,
          IntPtr pointCounts,
          Size imageSize,
          IntPtr intrinsicMatrix,
          IntPtr distortionCoeffs,
          IntPtr rotationVectors,
          IntPtr translationVectors,
          CvEnum.CALIB_TYPE flags);

      /// <summary>
      /// Computes various useful camera (sensor/lens) characteristics using the computed camera calibration matrix, image frame resolution in pixels and the physical aperture size
      /// </summary>
      /// <param name="calibMatr">The matrix of intrinsic parameters</param>
      /// <param name="imgWidth">Image width in pixels</param>
      /// <param name="imgHeight">Image height in pixels</param>
      /// <param name="apertureWidth">Aperture width in realworld units (optional input parameter). Set it to 0 if not used</param>
      /// <param name="apertureHeight">Aperture width in realworld units (optional input parameter). Set it to 0 if not used</param>
      /// <param name="fovx">Field of view angle in x direction in degrees</param>
      /// <param name="fovy">Field of view angle in y direction in degrees </param>
      /// <param name="focalLength">Focal length in realworld units </param>
      /// <param name="principalPoint">The principal point in realworld units </param>
      /// <param name="pixelAspectRatio">The pixel aspect ratio ~ fy/f</param>
      [DllImport(OPENCV_CALIB3D_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      public static extern void cvCalibrationMatrixValues(
         IntPtr calibMatr,
         int imgWidth,
         int imgHeight,
         double apertureWidth,
         double apertureHeight,
         ref double fovx,
         ref double fovy,
         ref double focalLength,
         ref MCvPoint2D64f principalPoint,
         ref double pixelAspectRatio);


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
      [DllImport(OPENCV_CALIB3D_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      public static extern void cvFindExtrinsicCameraParams2(
         IntPtr objectPoints,
         IntPtr imagePoints,
         IntPtr intrinsicMatrix,
         IntPtr distortionCoeffs,
         IntPtr rotationVector,
         IntPtr translationVector,
         int useExtrinsicGuess);

      /// <summary>
      /// Estimates transformation between the 2 cameras making a stereo pair. If we have a stereo camera, where the relative position and orientatation of the 2 cameras is fixed, and if we computed poses of an object relative to the fist camera and to the second camera, (R1, T1) and (R2, T2), respectively (that can be done with cvFindExtrinsicCameraParams2), obviously, those poses will relate to each other, i.e. given (R1, T1) it should be possible to compute (R2, T2) - we only need to know the position and orientation of the 2nd camera relative to the 1st camera. That's what the described function does. It computes (R, T) such that:
      /// R2=R*R1,
      /// T2=R*T1 + T
      /// </summary>
      /// <param name="objectPoints">The joint matrix of object points, 3xN or Nx3, where N is the total number of points in all views</param>
      /// <param name="imagePoints1">The joint matrix of corresponding image points in the views from the 1st camera, 2xN or Nx2, where N is the total number of points in all views</param>
      /// <param name="imagePoints2">The joint matrix of corresponding image points in the views from the 2nd camera, 2xN or Nx2, where N is the total number of points in all views</param>
      /// <param name="pointCounts">Vector containing numbers of points in each view, 1xM or Mx1, where M is the number of views</param>
      /// <param name="cameraMatrix1">The input/output camera matrices [fxk 0 cxk; 0 fyk cyk; 0 0 1]. If CV_CALIB_USE_INTRINSIC_GUESS or CV_CALIB_FIX_ASPECT_RATIO are specified, some or all of the elements of the matrices must be initialized</param>
      /// <param name="distCoeffs1">The input/output vectors of distortion coefficients for each camera, 4x1, 1x4, 5x1 or 1x5</param>
      /// <param name="cameraMatrix2">The input/output camera matrices [fxk 0 cxk; 0 fyk cyk; 0 0 1]. If CV_CALIB_USE_INTRINSIC_GUESS or CV_CALIB_FIX_ASPECT_RATIO are specified, some or all of the elements of the matrices must be initialized</param>
      /// <param name="distCoeffs2">The input/output vectors of distortion coefficients for each camera, 4x1, 1x4, 5x1 or 1x5</param>
      /// <param name="imageSize">Size of the image, used only to initialize intrinsic camera matrix</param>
      /// <param name="R">The rotation matrix between the 1st and the 2nd cameras' coordinate systems </param>
      /// <param name="T">The translation vector between the cameras' coordinate systems</param>
      /// <param name="E">The optional output essential matrix</param>
      /// <param name="F">The optional output fundamental matrix </param>
      /// <param name="termCrit">Termination criteria for the iterative optimiziation algorithm</param>
      /// <param name="flags">The calibration flags</param>
      /// <returns></returns>
      [DllImport(OPENCV_CALIB3D_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      public static extern double cvStereoCalibrate(
         IntPtr objectPoints,
         IntPtr imagePoints1,
         IntPtr imagePoints2,
         IntPtr pointCounts,
         IntPtr cameraMatrix1,
         IntPtr distCoeffs1,
         IntPtr cameraMatrix2,
         IntPtr distCoeffs2,
         Size imageSize,
         IntPtr R,
         IntPtr T,
         IntPtr E,
         IntPtr F,
         MCvTermCriteria termCrit,
         CvEnum.CALIB_TYPE flags);

      /// <summary>
      /// computes the rectification transformations without knowing intrinsic parameters of the cameras and their relative position in space, hence the suffix "Uncalibrated". Another related difference from cvStereoRectify is that the function outputs not the rectification transformations in the object (3D) space, but the planar perspective transformations, encoded by the homography matrices H1 and H2. The function implements the following algorithm [Hartley99]. 
      /// </summary>
      /// <remarks>
      /// Note that while the algorithm does not need to know the intrinsic parameters of the cameras, it heavily depends on the epipolar geometry. Therefore, if the camera lenses have significant distortion, it would better be corrected before computing the fundamental matrix and calling this function. For example, distortion coefficients can be estimated for each head of stereo camera separately by using cvCalibrateCamera2 and then the images can be corrected using cvUndistort2
      /// </remarks>
      /// <param name="points1">The array of 2D points</param>
      /// <param name="points2">The array of 2D points</param>
      /// <param name="F">Fundamental matrix. It can be computed using the same set of point pairs points1 and points2 using cvFindFundamentalMat</param>
      /// <param name="imageSize">Size of the image</param>
      /// <param name="H1">The rectification homography matrices for the first images</param>
      /// <param name="H2">The rectification homography matrices for the second images</param>
      /// <param name="threshold">If the parameter is greater than zero, then all the point pairs that do not comply the epipolar geometry well enough (that is, the points for which fabs(points2[i]T*F*points1[i])>threshold) are rejected prior to computing the homographies</param>
      /// <returns></returns>
      [DllImport(OPENCV_CALIB3D_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      public static extern int cvStereoRectifyUncalibrated(
         IntPtr points1,
         IntPtr points2,
         IntPtr F,
         Size imageSize,
         IntPtr H1,
         IntPtr H2,
         double threshold);

      /// <summary>
      /// computes the rotation matrices for each camera that (virtually) make both camera image planes the same plane. Consequently, that makes all the epipolar lines parallel and thus simplifies the dense stereo correspondence problem. On input the function takes the matrices computed by cvStereoCalibrate and on output it gives 2 rotation matrices and also 2 projection matrices in the new coordinates. The function is normally called after cvStereoCalibrate that computes both camera matrices, the distortion coefficients, R and T
      /// </summary>
      /// <param name="cameraMatrix1">The camera matrices [fx_k 0 cx_k; 0 fy_k cy_k; 0 0 1]</param>
      /// <param name="cameraMatrix2">The camera matrices [fx_k 0 cx_k; 0 fy_k cy_k; 0 0 1]</param>
      /// <param name="distCoeffs1">The vectors of distortion coefficients for first camera, 4x1, 1x4, 5x1 or 1x5</param>
      /// <param name="distCoeffs2">The vectors of distortion coefficients for second camera, 4x1, 1x4, 5x1 or 1x5</param>
      /// <param name="imageSize">Size of the image used for stereo calibration</param>
      /// <param name="R">The rotation matrix between the 1st and the 2nd cameras' coordinate systems</param>
      /// <param name="T">The translation vector between the cameras' coordinate systems</param>
      /// <param name="R1">3x3 Rectification transforms (rotation matrices) for the first camera</param>
      /// <param name="R2">3x3 Rectification transforms (rotation matrices) for the second camera</param>
      /// <param name="P1">3x4 Projection matrices in the new (rectified) coordinate systems</param>
      /// <param name="P2">3x4 Projection matrices in the new (rectified) coordinate systems</param>
      /// <param name="Q">The optional output disparity-to-depth mapping matrix, 4x4, see cvReprojectImageTo3D. </param>
      /// <param name="flags">The operation flags, use CALIB_ZERO_DISPARITY for default</param>
      /// <param name="alpha">Use -1 for default</param>
      /// <param name="newImageSize">Use Size.Empty for default</param>
      /// <param name="validPixROI1">The valid pixel ROI for image1</param>
      /// <param name="validPixROI2">The valid pixel ROI for image2</param>
      [DllImport(OPENCV_CALIB3D_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      public static extern void cvStereoRectify(
         IntPtr cameraMatrix1,
         IntPtr cameraMatrix2,
         IntPtr distCoeffs1,
         IntPtr distCoeffs2,
         Size imageSize,
         IntPtr R,
         IntPtr T,
         IntPtr R1,
         IntPtr R2,
         IntPtr P1,
         IntPtr P2,
         IntPtr Q,
         CvEnum.STEREO_RECTIFY_TYPE flags,
         double alpha,
         Size newImageSize,
         ref Rectangle validPixROI1,
         ref Rectangle validPixROI2
         );

      /// <summary>
      /// Attempts to determine whether the input image is a view of the chessboard pattern and locate internal chessboard corners
      /// </summary>
      /// <param name="image">Source chessboard view; it must be 8-bit grayscale or color image</param>
      /// <param name="patternSize">The number of inner corners per chessboard row and column</param>
      /// <param name="corners">Pointer to the output array of corners(PointF) detected</param>
      /// <param name="cornerCount">The output corner counter. If it is not IntPtr.Zero, the function stores there the number of corners found</param>
      /// <param name="flags">Various operation flags</param>
      /// <returns>Non-zero value if all the corners have been found and they have been placed in a certain order (row by row, left to right in every row), otherwise, if the function fails to find all the corners or reorder them, it returns 0</returns>
      /// <remarks>The coordinates detected are approximate, and to determine their position more accurately, the user may use the function cvFindCornerSubPix</remarks>
      [DllImport(OPENCV_CALIB3D_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      public static extern int cvFindChessboardCorners(
         IntPtr image,
         Size patternSize,
         IntPtr corners,
         ref int cornerCount,
         CvEnum.CALIB_CB_TYPE flags);

      /// <summary>
      /// Draws the individual chessboard corners detected (as red circles) in case if the board was not found (pattern_was_found=0) or the colored corners connected with lines when the board was found (pattern_was_found != 0). 
      /// </summary>
      /// <param name="image">The destination image; it must be 8-bit color image</param>
      /// <param name="patternSize">The number of inner corners per chessboard row and column</param>
      /// <param name="corners">The array of corners detected</param>
      /// <param name="count">The number of corners</param>
      /// <param name="patternWasFound">Indicates whether the complete board was found (!=0) or not (=0). One may just pass the return value cvFindChessboardCorners here. </param>
      [DllImport(OPENCV_CALIB3D_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      public static extern void cvDrawChessboardCorners(
         IntPtr image,
         Size patternSize,
         IntPtr corners,
         int count,
         int patternWasFound);

      /// <summary>
      /// Draws the individual chessboard corners detected (as red circles) in case if the board was not found (pattern_was_found=0) or the colored corners connected with lines when the board was found (pattern_was_found != 0). 
      /// </summary>
      /// <param name="image">The destination image; it must be 8-bit color image</param>
      /// <param name="patternSize">The number of inner corners per chessboard row and column</param>
      /// <param name="corners">The array of corners detected</param>
      /// <param name="count">The number of corners</param>
      /// <param name="patternWasFound">Indicates whether the complete board was found (!=0) or not (=0). One may just pass the return value cvFindChessboardCorners here. </param>
      [DllImport(OPENCV_CALIB3D_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      public static extern void cvDrawChessboardCorners(
         IntPtr image,
         Size patternSize,
         [In]
         PointF[] corners,
         int count,
         int patternWasFound);

      #endregion

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
      [DllImport(OPENCV_CALIB3D_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
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
      [DllImport(OPENCV_CALIB3D_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      public static extern void cvPOSIT(IntPtr positObject, float[,] imagePoints, double focalLength,
              MCvTermCriteria criteria, float[] rotationMatrix, float[] translationVector);

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
      [DllImport(OPENCV_CALIB3D_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      public static extern void cvPOSIT(IntPtr positObject, IntPtr imagePoints, double focalLength,
              MCvTermCriteria criteria, IntPtr rotationMatrix, IntPtr translationVector);

      /// <summary>
      /// The function cvReleasePOSITObject releases memory previously allocated by the function cvCreatePOSITObject. 
      /// </summary>
      /// <param name="positObject">pointer to CvPOSIT structure</param>
      [DllImport(OPENCV_CALIB3D_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      public static extern void cvReleasePOSITObject(ref IntPtr positObject);
      #endregion
   }
}
