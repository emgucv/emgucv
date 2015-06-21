//----------------------------------------------------------------------------
//  Copyright (C) 2004-2015 by EMGU Corporation. All rights reserved.       
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
      /// <param name="homography">The output homography matrix</param>
      /// <param name="method">FindHomography method</param>
      /// <param name="ransacReprojThreshold">
      /// The maximum allowed reprojection error to treat a point pair as an inlier. 
      /// The parameter is only used in RANSAC-based homography estimation. 
      /// E.g. if dst_points coordinates are measured in pixels with pixel-accurate precision, it makes sense to set this parameter somewhere in the range ~1..3
      /// </param>
      /// <param name="mask">Optional output mask set by a robust method ( CV_RANSAC or CV_LMEDS ). Note that the input mask values are ignored.</param>
      /// <returns>The 3x3 homography matrix if found. Null if not found.</returns>
      public static void FindHomography(
         PointF[] srcPoints,
         PointF[] dstPoints,
         IOutputArray homography,
         CvEnum.HomographyMethod method,
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
               CvInvoke.FindHomography(srcPointMatrix, dstPointMatrix, homography, method, ransacReprojThreshold, mask);
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
      /// <param name="homography">Output 3x3 homography matrix. Homography matrix is determined up to a scale, thus it is normalized to make h33=1</param>
      public static void FindHomography(
         IInputArray srcPoints,
         IInputArray dstPoints,
         IOutputArray homography,
         CvEnum.HomographyMethod method = CvEnum.HomographyMethod.Default,
         double ransacReprojThreshold = 3,
         IOutputArray mask = null)
      {
         using (InputArray iaSrcPoints = srcPoints.GetInputArray())
         using (InputArray iaDstPoints = dstPoints.GetInputArray())
         using (OutputArray oaHomography = homography.GetOutputArray())
         using (OutputArray oaMask = mask == null ? OutputArray.GetEmpty() : mask.GetOutputArray())
            cveFindHomography(iaSrcPoints, iaDstPoints, oaHomography, method, ransacReprojThreshold, oaMask);
      }
      [DllImport(ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      private static extern void cveFindHomography(IntPtr srcPoints, IntPtr dstPoints, IntPtr homography, CvEnum.HomographyMethod method, double ransacReprojThreshold, IntPtr mask);

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
      /// Calculates fundamental matrix using one of four methods listed above and returns the number of fundamental matrices found (1 or 3) and 0, if no matrix is found. 
      /// </summary>
      /// <param name="points1">Array of N points from the first image. The point coordinates should be floating-point (single or double precision).</param>
      /// <param name="points2">Array of the second image points of the same size and format as points1 </param>
      /// <param name="method">Method for computing the fundamental matrix </param>
      /// <param name="param1">Parameter used for RANSAC. It is the maximum distance from a point to an epipolar line in pixels, beyond which the point is considered an outlier and is not used for computing the final fundamental matrix. It can be set to something like 1-3, depending on the accuracy of the point localization, image resolution, and the image noise.</param>
      /// <param name="param2">Parameter used for the RANSAC or LMedS methods only. It specifies a desirable level of confidence (probability) that the estimated matrix is correct.</param>
      /// <param name="mask">The optional pointer to output array of N elements, every element of which is set to 0 for outliers and to 1 for the "inliers", i.e. points that comply well with the estimated epipolar geometry. The array is computed only in RANSAC and LMedS methods. For other methods it is set to all 1.</param>
      /// <param name="f">The calculated fundamental matrix</param>
      public static void FindFundamentalMat(IInputArray points1, IInputArray points2, IOutputArray f, CvEnum.FmType method = CvEnum.FmType.Ransac, double param1 = 3, double param2 = 0.99, IOutputArray mask = null)
      {
         using (InputArray iaPoints1 = points1.GetInputArray())
         using (InputArray iaPoints2 = points2.GetInputArray())
         using (OutputArray oaF = f.GetOutputArray())
         using (OutputArray oaMask = mask == null ? OutputArray.GetEmpty() : mask.GetOutputArray())
            cveFindFundamentalMat(iaPoints1, iaPoints2, oaF, method, param1, param2, oaMask);
      }
      [DllImport(ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      private static extern void cveFindFundamentalMat(IntPtr points1, IntPtr points2, IntPtr dst, CvEnum.FmType method, double param1, double param2, IntPtr mask);


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
      public static void ComputeCorrespondEpilines(IInputArray points, int whichImage, IInputArray fundamentalMatrix, IOutputArray correspondentLines)
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
      public static void ReprojectImageTo3D(IInputArray disparity, IOutputArray image3D, IInputArray q, bool handleMissingValues = false, CvEnum.DepthType ddepth = CvEnum.DepthType.Default)
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
         [MarshalAs(CvInvoke.BoolMarshalType)]
         bool handleMissingValues,
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
          IInputArray rvec, IInputArray tvec, IInputArray cameraMatrix, IInputArray distCoeffs, IOutputArray jacobian = null, double aspectRatio = 0)
      {
         PointF[] imagePoints = new PointF[objectPoints.Length];

         GCHandle handle1 = GCHandle.Alloc(objectPoints, GCHandleType.Pinned);
         GCHandle handle2 = GCHandle.Alloc(imagePoints, GCHandleType.Pinned);
         using (Mat pointMatrix = new Mat(objectPoints.Length, 1, DepthType.Cv32F, 3, handle1.AddrOfPinnedObject(), 3 * sizeof(float)))
         using (Mat imagePointMatrix = new Mat(imagePoints.Length, 1, DepthType.Cv32F, 2, handle2.AddrOfPinnedObject(), 2 * sizeof(float)))
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
      public static void ProjectPoints(IInputArray objectPoints, IInputArray rvec, IInputArray tvec, IInputArray cameraMatrix, IInputArray distCoeffs, IOutputArray imagePoints, IOutputArray jacobian = null, double aspectRatio = 0)
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
      private static extern void cveProjectPoints(IntPtr objPoints, IntPtr rvec, IntPtr tvec, IntPtr cameraMatrix, IntPtr distCoeffs, IntPtr imagePoints, IntPtr jacobian, double aspectRatio);


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
         System.Diagnostics.Debug.Assert(objectPoints.Length == imagePoints.Length, "The number of images for objects points should be equal to the number of images for image points");
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
                  using (Mat matR = rotationVectors[i])
                     matR.CopyTo(rotationVectors[i]);
                  translationVectors[i] = new Mat();
                  using (Mat matT = translationVectors[i])
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
         using(InputArray iaObjectPoints = objectPoints.GetInputArray())
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
         ref double fovx, ref double fovy, ref double focalLength, ref MCvPoint2D64f principalPoint, ref double aspectRatio)
      {
         using (InputArray iaCameraMatrix = cameraMatrix.GetInputArray())
         cveCalibrationMatrixValues(
            iaCameraMatrix, ref imageSize, apertureWidth, apertureHeight, ref fovx, ref fovy, ref focalLength, ref principalPoint, ref aspectRatio);
      }
      [DllImport(ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      private static extern void cveCalibrationMatrixValues(
         IntPtr cameraMatrix, ref Size imageSize, double apertureWidth, double apertureHeight,
         ref double fovx, ref double fovy, ref double focalLength, ref MCvPoint2D64f principalPoint, ref double aspectRatio);


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
      /// <returns>The extrinsic parameters</returns>
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
            return CvInvoke.SolvePnP(objPtVec, imgPtVec, intrinsicMatrix, distortionCoeffs, rotationVector, translationVector, false, method);
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
         using (InputArray iaImagePoints = imagePoints.GetInputArray() )
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
      /// <param name="minInliersCount">Number of inliers. If the algorithm at some stage finds more inliers than minInliersCount, it finishes.</param>
      /// <param name="inliers">Output vector that contains indices of inliers in objectPoints and imagePoints .</param>
      /// <param name="flags">Method for solving a PnP problem </param>
      public static void SolvePnPRansac(
         IInputArray objectPoints, IInputArray imagePoints, IInputArray cameraMatrix, IInputArray distCoeffs,
         IOutputArray rvec, IOutputArray tvec,
         bool useExtrinsicGuess, int iterationsCount, float reprojectionError, int minInliersCount,
         IOutputArray inliers, CvEnum.SolvePnpMethod flags)
      {
         using (InputArray iaObjectPoints = objectPoints.GetInputArray())
         using (InputArray iaImagePoints = imagePoints.GetInputArray())
         using (InputArray iaCameraMatrix = cameraMatrix.GetInputArray())
         using (InputArray iaDistortionCoeffs = distCoeffs == null ? InputArray.GetEmpty() : distCoeffs.GetInputArray())
         using (OutputArray oaRotationVector = rvec.GetOutputArray())
         using (OutputArray oaTranslationVector = tvec.GetOutputArray())
         using (OutputArray oaInliers = inliers == null ? OutputArray.GetEmpty() : inliers.GetOutputArray())
         cveSolvePnPRansac(
            iaObjectPoints, iaImagePoints, iaCameraMatrix, iaDistortionCoeffs,
            oaRotationVector, oaTranslationVector,
            useExtrinsicGuess, iterationsCount, reprojectionError, minInliersCount,
            oaInliers, flags);
      }
      [DllImport(ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      private static extern void cveSolvePnPRansac(
         IntPtr objectPoints, IntPtr imagePoints, IntPtr cameraMatrix, IntPtr distCoeffs,
         IntPtr rvec, IntPtr tvec,
         [MarshalAs(CvInvoke.BoolMarshalType)]
         bool useExtrinsicGuess,
         int iterationsCount, float reprojectionError, int minInliersCount,
         IntPtr inliers, CvEnum.SolvePnpMethod flags);


      /// <summary>
      /// Estimates transformation between the 2 cameras making a stereo pair. If we have a stereo camera, where the relative position and orientatation of the 2 cameras is fixed, and if we computed poses of an object relative to the fist camera and to the second camera, (R1, T1) and (R2, T2), respectively (that can be done with cvFindExtrinsicCameraParams2), obviously, those poses will relate to each other, i.e. given (R1, T1) it should be possible to compute (R2, T2) - we only need to know the position and orientation of the 2nd camera relative to the 1st camera. That's what the described function does. It computes (R, T) such that:
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
      public static void StereoCalibrate(
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
         System.Diagnostics.Debug.Assert(objectPoints.Length == imagePoints1.Length && objectPoints.Length == imagePoints2.Length, "The number of images for objects points should be equal to the number of images for image points");

         using (VectorOfVectorOfPoint3D32F objectPointVec = new VectorOfVectorOfPoint3D32F(objectPoints))
         using (VectorOfVectorOfPointF imagePoints1Vec = new VectorOfVectorOfPointF(imagePoints1))
         using (VectorOfVectorOfPointF imagePoints2Vec = new VectorOfVectorOfPointF(imagePoints2))
         {
            CvInvoke.StereoCalibrate(
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
      /// <returns></returns>
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
      /// computes the rectification transformations without knowing intrinsic parameters of the cameras and their relative position in space, hence the suffix "Uncalibrated". Another related difference from cvStereoRectify is that the function outputs not the rectification transformations in the object (3D) space, but the planar perspective transformations, encoded by the homography matrices H1 and H2. The function implements the following algorithm [Hartley99]. 
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
      /// <param name="threshold">If the parameter is greater than zero, then all the point pairs that do not comply the epipolar geometry well enough (that is, the points for which fabs(points2[i]T*F*points1[i])>threshold) are rejected prior to computing the homographies</param>
      /// <returns></returns>
      public static bool StereoRectifyUncalibrated(IInputArray points1, IInputArray points2, IInputArray f, Size imgSize, IOutputArray h1, IOutputArray h2, double threshold = 5)
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
      private static extern bool cveStereoRectifyUncalibrated(IntPtr points1, IntPtr points2, IntPtr f, ref Size imgSize, IntPtr h1, IntPtr h2, double threshold);


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
      /// <param name="flags">The operation flags, use CALIB_ZERO_DISPARITY for default</param>
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
         using (InputArray iaT = t.GetInputArray() )
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
      /// Attempts to determine whether the input image is a view of the chessboard pattern and locate internal chessboard corners
      /// </summary>
      /// <param name="image">Source chessboard view; it must be 8-bit grayscale or color image</param>
      /// <param name="patternSize">The number of inner corners per chessboard row and column</param>
      /// <param name="corners">Pointer to the output array of corners(PointF) detected</param>
      /// <param name="flags">Various operation flags</param>
      /// <returns>True if all the corners have been found and they have been placed in a certain order (row by row, left to right in every row), otherwise, if the function fails to find all the corners or reorder them, it returns 0</returns>
      /// <remarks>The coordinates detected are approximate, and to determine their position more accurately, the user may use the function cvFindCornerSubPix</remarks>
      public static bool FindChessboardCorners(IInputArray image, Size patternSize, IOutputArray corners, CvEnum.CalibCbType flags = CvEnum.CalibCbType.AdaptiveThresh | CvEnum.CalibCbType.NormalizeImage)
      {
         using (InputArray iaImage = image.GetInputArray())
         using (OutputArray oaCorners = corners.GetOutputArray())
         return cveFindChessboardCorners(iaImage, ref patternSize, oaCorners, flags);
      }
      [DllImport(ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      private static extern bool cveFindChessboardCorners(IntPtr image, ref Size patternSize, IntPtr corners, CvEnum.CalibCbType flags);


      /// <summary>
      /// Draws the individual chessboard corners detected (as red circles) in case if the board was not found (pattern_was_found=0) or the colored corners connected with lines when the board was found (pattern_was_found != 0). 
      /// </summary>
      /// <param name="image">The destination image; it must be 8-bit color image</param>
      /// <param name="patternSize">The number of inner corners per chessboard row and column</param>
      /// <param name="corners">The array of corners detected</param>
      /// <param name="patternWasFound">Indicates whether the complete board was found (!=0) or not (=0). One may just pass the return value cvFindChessboardCorners here. </param>
      public static void DrawChessboardCorners(IInputOutputArray image, Size patternSize, IInputArray corners, bool patternWasFound)
      {
         using (InputOutputArray ioaImage = image.GetInputOutputArray())
         using (InputArray iaCorners = corners.GetInputArray())
         cveDrawChessboardCorners(ioaImage, ref patternSize, iaCorners, patternWasFound);
      }
      [DllImport(ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      private static extern void cveDrawChessboardCorners(IntPtr image, ref Size patternSize, IntPtr corners, [MarshalAs(CvInvoke.BoolMarshalType)] bool patternWasFound);
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
      public static void TriangulatePoints(IInputArray projMat1, IInputArray projMat2, IInputArray projPoints1, IInputArray projPoints2, IOutputArray points4D)
      {
         using (InputArray iaProjMat1 = projMat1.GetInputArray())
         using (InputArray iaProjMat2 = projMat2.GetInputArray())
         using (InputArray iaProjPoints1 = projPoints1.GetInputArray())
         using (InputArray iaProjPoints2 = projPoints2.GetInputArray())
         using (OutputArray oaPoints4D = points4D.GetOutputArray())
            cveTriangulatePoints(iaProjMat1, iaProjMat2, iaProjPoints1, iaProjPoints2, oaPoints4D);
      }
      [DllImport(ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      private extern static void cveTriangulatePoints(IntPtr projMat1, IntPtr projMat2, IntPtr projPoints1, IntPtr projPoints2, IntPtr points4D);

      /// <summary>
      /// Refines coordinates of corresponding points.
      /// </summary>
      /// <param name="f">3x3 fundamental matrix.</param>
      /// <param name="points1">1xN array containing the first set of points.</param>
      /// <param name="points2">1xN array containing the second set of points.</param>
      /// <param name="newPoints1">The optimized points1.</param>
      /// <param name="newPoints2">The optimized points2.</param>
      public static void CorrectMatches(IInputArray f, IInputArray points1, IInputArray points2, IOutputArray newPoints1, IOutputArray newPoints2)
      {
         using (InputArray iaF = f.GetInputArray())
         using (InputArray iaPoints1 = points1.GetInputArray())
         using (InputArray iaPoints2 = points2.GetInputArray())
         using (OutputArray oaNewPoints1 = newPoints1.GetOutputArray())
         using (OutputArray oaNewPoints2 = newPoints2.GetOutputArray())
            cveCorrectMatches(iaF, iaPoints1, iaPoints2, oaNewPoints1, oaNewPoints2);
      }
      [DllImport(ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      private extern static void cveCorrectMatches(IntPtr f, IntPtr points1, IntPtr points2, IntPtr newPoints1, IntPtr newPoints2);

   }
}
