//----------------------------------------------------------------------------
//  Copyright (C) 2004-2012 by EMGU. All rights reserved.       
//----------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Runtime.InteropServices;
using Emgu.CV.Structure;
using Emgu.CV.Util;

namespace Emgu.CV
{
   /// <summary>
   /// Camera calibration functions
   /// </summary>
   public static class CameraCalibration
   {
      /// <summary>
      /// Estimates intrinsic camera parameters and extrinsic parameters for each of the views
      /// </summary>
      /// <param name="objectPoints">The 3D location of the object points. The first index is the index of image, second index is the index of the point</param>
      /// <param name="imagePoints">The 2D image location of the points. The first index is the index of the image, second index is the index of the point</param>
      /// <param name="imageSize">The size of the image, used only to initialize intrinsic camera matrix</param>
      /// <param name="intrinsicParam">The intrisinc parameters, might contains some initial values. The values will be modified by this function.</param>
      /// <param name="flags">Flags</param>
      /// <param name="extrinsicParams">The output array of extrinsic parameters.</param>
      /// <returns>The final reprojection error</returns>
      public static double CalibrateCamera(
         MCvPoint3D32f[][] objectPoints,
         PointF[][] imagePoints,
         Size imageSize,
         IntrinsicCameraParameters intrinsicParam,
         CvEnum.CALIB_TYPE flags,
         out ExtrinsicCameraParameters[] extrinsicParams)
      {
         Debug.Assert(objectPoints.Length == imagePoints.Length, "The number of images for objects points should be equal to the number of images for image points");
         int imageCount = objectPoints.Length;

         #region get the array that represent the point counts
         int[] pointCounts = new int[objectPoints.Length];
         for (int i = 0; i < objectPoints.Length; i++)
         {
            Debug.Assert(objectPoints[i].Length == imagePoints[i].Length, String.Format("Number of 3D points and image points should be equal in the {0}th image", i));
            pointCounts[i] = objectPoints[i].Length;
         }
         #endregion

         double reprojectionError = -1;
         using (Matrix<float> objectPointMatrix = ToMatrix(objectPoints))
         using (Matrix<float> imagePointMatrix = ToMatrix(imagePoints))
         using (Matrix<int> pointCountsMatrix = new Matrix<int>(pointCounts))
         using (Matrix<double> rotationVectors = new Matrix<double>(imageCount, 3))
         using (Matrix<double> translationVectors = new Matrix<double>(imageCount, 3))
         {
            reprojectionError = CvInvoke.cvCalibrateCamera2(
                objectPointMatrix.Ptr,
                imagePointMatrix.Ptr,
                pointCountsMatrix.Ptr,
                imageSize,
                intrinsicParam.IntrinsicMatrix,
                intrinsicParam.DistortionCoeffs,
                rotationVectors,
                translationVectors,
                flags);

            extrinsicParams = new ExtrinsicCameraParameters[imageCount];
            IntPtr matPtr = Marshal.AllocHGlobal(StructSize.MCvMat);
            for (int i = 0; i < imageCount; i++)
            {
               ExtrinsicCameraParameters p = new ExtrinsicCameraParameters();
               CvInvoke.cvGetRow(rotationVectors.Ptr, matPtr, i);
               CvInvoke.cvTranspose(matPtr, p.RotationVector.Ptr);
               CvInvoke.cvGetRow(translationVectors.Ptr, matPtr, i);
               CvInvoke.cvTranspose(matPtr, p.TranslationVector.Ptr);
               extrinsicParams[i] = p;
            }
            Marshal.FreeHGlobal(matPtr);
         }
         return reprojectionError;
      }

      /// <summary>
      /// Estimates transformation between the 2 cameras making a stereo pair. If we have a stereo camera, where the relative position and orientatation of the 2 cameras is fixed, and if we computed poses of an object relative to the fist camera and to the second camera, (R1, T1) and (R2, T2), respectively (that can be done with cvFindExtrinsicCameraParams2), obviously, those poses will relate to each other, i.e. given (R1, T1) it should be possible to compute (R2, T2) - we only need to know the position and orientation of the 2nd camera relative to the 1st camera. That's what the described function does. It computes (R, T) such that:
      /// R2=R*R1,
      /// T2=R*T1 + T
      /// </summary>
      /// <param name="objectPoints">The 3D location of the object points. The first index is the index of image, second index is the index of the point</param>
      /// <param name="imagePoints1">The 2D image location of the points for camera 1. The first index is the index of the image, second index is the index of the point</param>
      /// <param name="imagePoints2">The 2D image location of the points for camera 2. The first index is the index of the image, second index is the index of the point</param>
      /// <param name="intrinsicParam1">The intrisinc parameters for camera 1, might contains some initial values. The values will be modified by this function.</param>
      /// <param name="intrinsicParam2">The intrisinc parameters for camera 2, might contains some initial values. The values will be modified by this function.</param>
      /// <param name="imageSize">Size of the image, used only to initialize intrinsic camera matrix</param>
      /// <param name="flags">Different flags</param>
      /// <param name="extrinsicParams">The extrinsic parameters which contains:
      /// R - The rotation matrix between the 1st and the 2nd cameras' coordinate systems; 
      /// T - The translation vector between the cameras' coordinate systems. </param>
      /// <param name="essentialMatrix">The essential matrix</param>
      /// <param name="termCrit">Termination criteria for the iterative optimiziation algorithm </param>
      /// <param name="foundamentalMatrix">The fundamental matrix</param>
      public static void StereoCalibrate(
         MCvPoint3D32f[][] objectPoints,
         PointF[][] imagePoints1,
         PointF[][] imagePoints2,
         IntrinsicCameraParameters intrinsicParam1,
         IntrinsicCameraParameters intrinsicParam2,
         Size imageSize,
         CvEnum.CALIB_TYPE flags,
         MCvTermCriteria termCrit,
         out ExtrinsicCameraParameters extrinsicParams,
         out Matrix<double> foundamentalMatrix,
         out Matrix<double> essentialMatrix)
      {
         Debug.Assert(objectPoints.Length == imagePoints1.Length && objectPoints.Length == imagePoints2.Length, "The number of images for objects points should be equal to the number of images for image points");

         #region get the matrix that represent the point counts
         int[,] pointCounts = new int[objectPoints.Length, 1];
         for (int i = 0; i < objectPoints.Length; i++)
         {
            Debug.Assert(objectPoints[i].Length == imagePoints1[i].Length && objectPoints[i].Length == imagePoints2[i].Length, String.Format("Number of 3D points and image points should be equal in the {0}th image", i));
            pointCounts[i, 0] = objectPoints[i].Length;
         }
         #endregion

         using (Matrix<float> objectPointMatrix = ToMatrix(objectPoints))
         using (Matrix<float> imagePointMatrix1 = ToMatrix(imagePoints1))
         using (Matrix<float> imagePointMatrix2 = ToMatrix(imagePoints2))
         using (Matrix<int> pointCountsMatrix = new Matrix<int>(pointCounts))
         {
            extrinsicParams = new ExtrinsicCameraParameters();
            essentialMatrix = new Matrix<double>(3, 3);
            foundamentalMatrix = new Matrix<double>(3, 3);

            CvInvoke.cvStereoCalibrate(
               objectPointMatrix.Ptr,
               imagePointMatrix1.Ptr,
               imagePointMatrix2.Ptr,
               pointCountsMatrix.Ptr,
               intrinsicParam1.IntrinsicMatrix,
               intrinsicParam1.DistortionCoeffs,
               intrinsicParam2.IntrinsicMatrix,
               intrinsicParam2.DistortionCoeffs,
               imageSize,
               extrinsicParams.RotationVector,
               extrinsicParams.TranslationVector,
               essentialMatrix.Ptr,
               foundamentalMatrix.Ptr,
               termCrit,
               flags);
         }
      }

      /// <summary>
      /// Estimates extrinsic camera parameters using known intrinsic parameters and extrinsic parameters for each view. The coordinates of 3D object points and their correspondent 2D projections must be specified. This function also minimizes back-projection error. 
      /// </summary>
      /// <param name="objectPoints">The array of object points</param>
      /// <param name="imagePoints">The array of corresponding image points</param>
      /// <param name="intrin">The intrinsic parameters</param>
      /// <returns>The extrinsic parameters</returns>
      public static ExtrinsicCameraParameters FindExtrinsicCameraParams2(
          MCvPoint3D32f[] objectPoints,
          PointF[] imagePoints,
          IntrinsicCameraParameters intrin)
      {
         ExtrinsicCameraParameters p = new ExtrinsicCameraParameters();

         GCHandle handle1 = GCHandle.Alloc(objectPoints, GCHandleType.Pinned);
         GCHandle handle2 = GCHandle.Alloc(imagePoints, GCHandleType.Pinned);
         using (Matrix<float> objectPointMatrix = new Matrix<float>(objectPoints.Length, 3, handle1.AddrOfPinnedObject()))
         using (Matrix<float> imagePointMatrix = new Matrix<float>(imagePoints.Length, 2, handle2.AddrOfPinnedObject()))
            CvInvoke.cvFindExtrinsicCameraParams2(objectPointMatrix, imagePointMatrix, intrin.IntrinsicMatrix.Ptr, intrin.DistortionCoeffs.Ptr, p.RotationVector.Ptr, p.TranslationVector.Ptr, 0);
         handle1.Free();
         handle2.Free();

         return p;
      }

      /// <summary>
      /// Computes projections of 3D points to the image plane given intrinsic and extrinsic camera parameters. 
      /// Optionally, the function computes jacobians - matrices of partial derivatives of image points as functions of all the input parameters w.r.t. the particular parameters, intrinsic and/or extrinsic. 
      /// The jacobians are used during the global optimization in cvCalibrateCamera2 and cvFindExtrinsicCameraParams2. 
      /// The function itself is also used to compute back-projection error for with current intrinsic and extrinsic parameters. 
      /// </summary>
      /// <remarks>Note, that with intrinsic and/or extrinsic parameters set to special values, the function can be used to compute just extrinsic transformation or just intrinsic transformation (i.e. distortion of a sparse set of points) </remarks>
      /// <param name="objectPoints">The array of object points.</param>
      /// <param name="extrin">Extrinsic parameters</param>
      /// <param name="intrin">Intrinsic parameters</param>
      /// <param name="mats">Optional matrix supplied in the following order: dpdrot, dpdt, dpdf, dpdc, dpddist</param>
      /// <returns>The array of image points which is the projection of <paramref name="objectPoints"/></returns>
      public static PointF[] ProjectPoints(
          MCvPoint3D32f[] objectPoints,
          ExtrinsicCameraParameters extrin,
          IntrinsicCameraParameters intrin,
          params Matrix<float>[] mats)
      {
         PointF[] imagePoints = new PointF[objectPoints.Length];

         int matsLength = mats.Length;
         GCHandle handle1 = GCHandle.Alloc(objectPoints, GCHandleType.Pinned);
         GCHandle handle2 = GCHandle.Alloc(imagePoints, GCHandleType.Pinned);
         using (Matrix<float> pointMatrix = new Matrix<float>(objectPoints.Length, 1, 3, handle1.AddrOfPinnedObject(), 3 * sizeof(float)))
         using (Matrix<float> imagePointMatrix = new Matrix<float>(imagePoints.Length, 1, 2, handle2.AddrOfPinnedObject(), 2 * sizeof(float)))
            CvInvoke.cvProjectPoints2(
                  pointMatrix,
                  extrin.RotationVector.Ptr,
                  extrin.TranslationVector.Ptr,
                  intrin.IntrinsicMatrix.Ptr,
                  intrin.DistortionCoeffs.Ptr,
                  imagePointMatrix,
                  matsLength > 0 ? mats[0] : IntPtr.Zero,
                  matsLength > 1 ? mats[1] : IntPtr.Zero,
                  matsLength > 2 ? mats[2] : IntPtr.Zero,
                  matsLength > 3 ? mats[3] : IntPtr.Zero,
                  matsLength > 4 ? mats[4] : IntPtr.Zero, 
                  0.0);
         handle1.Free();
         handle2.Free();
         return imagePoints;
      }

      /// <summary>
      /// Use the specific method to find perspective transformation H=||h_ij|| between the source and the destination planes 
      /// </summary>
      /// <param name="srcPoints">Point coordinates in the original plane, 2xN, Nx2, 3xN or Nx3 array (the latter two are for representation in homogeneous coordinates), where N is the number of points</param>
      /// <param name="dstPoints">Point coordinates in the destination plane, 2xN, Nx2, 3xN or Nx3 array (the latter two are for representation in homogeneous coordinates) </param>
      /// <param name="method">FindHomography method</param>
      /// <param name="ransacReprojThreshold">The maximum allowed reprojection error to treat a point pair as an inlier. The parameter is only used in RANSAC-based homography estimation. E.g. if dst_points coordinates are measured in pixels with pixel-accurate precision, it makes sense to set this parameter somewhere in the range ~1..3</param>
      /// <returns>The 3x3 homography matrix if found. Null if not found.</returns>
      public static HomographyMatrix FindHomography(
         Matrix<float> srcPoints,
         Matrix<float> dstPoints,
         CvEnum.HOMOGRAPHY_METHOD method,
         double ransacReprojThreshold)
      {
         HomographyMatrix homography = new HomographyMatrix();
         if (!CvInvoke.cvFindHomography(srcPoints.Ptr, dstPoints.Ptr, homography.Ptr, method, ransacReprojThreshold, IntPtr.Zero))
         {
            homography.Dispose();
            return null;
         }
         return homography;
      }

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
      /// <returns>The 3x3 homography matrix if found. Null if not found.</returns>
      public static HomographyMatrix FindHomography(
         PointF[] srcPoints,
         PointF[] dstPoints,
         CvEnum.HOMOGRAPHY_METHOD method,
         double ransacReprojThreshold)
      {
         HomographyMatrix homography;

         GCHandle srcHandle = GCHandle.Alloc(srcPoints, GCHandleType.Pinned);
         GCHandle dstHandle = GCHandle.Alloc(dstPoints, GCHandleType.Pinned);
         using (Matrix<float> srcPointMatrix = new Matrix<float>(srcPoints.Length, 2, srcHandle.AddrOfPinnedObject()))
         using (Matrix<float> dstPointMatrix = new Matrix<float>(dstPoints.Length, 2, dstHandle.AddrOfPinnedObject()))
            homography = FindHomography(srcPointMatrix, dstPointMatrix, method, ransacReprojThreshold);
         srcHandle.Free();
         dstHandle.Free();
         return homography;
      }

      /// <summary>
      /// Attempts to determine whether the input image is a view of the chessboard pattern and locate internal chessboard corners
      /// </summary>
      /// <param name="image">Source chessboard view</param>
      /// <param name="patternSize">The number of inner corners per chessboard row and column</param>
      /// <param name="flags">Various operation flags</param>
      /// <returns>The corners detected if the chess board pattern is found, otherwise null is returned</returns>
      public static PointF[] FindChessboardCorners(
         Image<Gray, Byte> image,
         Size patternSize,
         CvEnum.CALIB_CB_TYPE flags)
      {
         int cornerCount = 0;

         PointF[] corners = new PointF[patternSize.Width * patternSize.Height];
         GCHandle handle = GCHandle.Alloc(corners, GCHandleType.Pinned);

         bool patternFound =
            CvInvoke.cvFindChessboardCorners(
               image.Ptr,
               patternSize,
               handle.AddrOfPinnedObject(),
               ref cornerCount,
               flags) != 0;

         handle.Free();

         if (cornerCount != corners.Length)
            Array.Resize(ref corners, cornerCount);

         return patternFound ? corners : null;
      }

      /// <summary>
      /// Draws the individual chessboard corners detected (as red circles) in case if the board was not found (patternWasFound== false) or the colored corners connected with lines when the board was found (patternWasFound == true). 
      /// </summary>
      /// <param name="image">The destination image</param>
      /// <param name="patternSize">The number of inner corners per chessboard row and column</param>
      /// <param name="corners">The array of corners detected. Can be null if no corners were found</param>
      public static void DrawChessboardCorners(
         Image<Gray, Byte> image,
         Size patternSize,
         PointF[] corners)
      {
         CvInvoke.cvDrawChessboardCorners(
            image.Ptr,
            patternSize,
            corners,
            corners.Length,
            corners != null ? 1 : 0);
      }

      /// <summary>
      /// Calculates the matrix of an affine transform such that:
      /// (x'_i,y'_i)^T=map_matrix (x_i,y_i,1)^T
      /// where dst(i)=(x'_i,y'_i), src(i)=(x_i,y_i), i=0..2.
      /// </summary>
      /// <param name="src">Coordinates of 3 triangle vertices in the source image. If the array contains more than 3 points, only the first 3 will be used</param>
      /// <param name="dest">Coordinates of the 3 corresponding triangle vertices in the destination image. If the array contains more than 3 points, only the first 3 will be used</param>
      /// <returns>The 2x3 rotation matrix that defines the Affine transform</returns>
      public static RotationMatrix2D<double> GetAffineTransform(PointF[] src, PointF[] dest)
      {
         Debug.Assert(src.Length >= 3, "The source should contain at least 3 points");
         Debug.Assert(dest.Length >= 3, "The destination should contain at least 3 points");

         RotationMatrix2D<double> rot = new RotationMatrix2D<double>();
         CvInvoke.cvGetAffineTransform(src, dest, rot);
         return rot;
      }

      /// <summary>
      /// Estimate rigid transformation between 2 point sets.
      /// </summary>
      /// <param name="src">The points from the source image</param>
      /// <param name="dest">The corresponding points from the destination image</param>
      /// <param name="fullAffine">Indicates if full affine should be performed</param>
      /// <returns>If success, the 2x3 rotation matrix that defines the Affine transform. Otherwise null is returned.</returns>
      public static RotationMatrix2D<double> EstimateRigidTransform(PointF[] src, PointF[] dest, bool fullAffine)
      {
         RotationMatrix2D<double> result = new RotationMatrix2D<double>();
         GCHandle handleA = GCHandle.Alloc(src, GCHandleType.Pinned);
         GCHandle handleB = GCHandle.Alloc(dest, GCHandleType.Pinned);
         bool success;
         using (Matrix<float> a = new Matrix<float>(src.Length, 1, 2, handleA.AddrOfPinnedObject(), 2 * sizeof(float)))
         using (Matrix<float> b = new Matrix<float>(dest.Length, 1, 2, handleB.AddrOfPinnedObject(), 2 * sizeof(float)))
         {
            success = CvInvoke.cvEstimateRigidTransform(a, b, result, fullAffine);
         }
         handleA.Free();
         handleB.Free();

         if (success)
         {
            return result;
         }
         else
         {
            result.Dispose();
            return null;
         }
      }

      /// <summary>
      /// calculates matrix of perspective transform such that:
      /// (t_i x'_i,t_i y'_i,t_i)^T=map_matrix (x_i,y_i,1)^T
      /// where dst(i)=(x'_i,y'_i), src(i)=(x_i,y_i), i=0..3.
      /// </summary>
      /// <param name="src">Coordinates of 4 quadrangle vertices in the source image</param>
      /// <param name="dest">Coordinates of the 4 corresponding quadrangle vertices in the destination image</param>
      /// <returns>The 3x3 Homography matrix</returns>
      public static HomographyMatrix GetPerspectiveTransform(PointF[] src, PointF[] dest)
      {
         Debug.Assert(src.Length >= 4, "The source should contain at least 4 points");
         Debug.Assert(dest.Length >= 4, "The destination should contain at least 4 points");

         HomographyMatrix rot = new HomographyMatrix();
         CvInvoke.cvGetPerspectiveTransform(src, dest, rot);
         return rot;
      }

      #region helper methods
      private static Matrix<float> ToMatrix(MCvPoint3D32f[][] data)
      {
         int elementCount = 0;
         foreach (MCvPoint3D32f[] d in data) elementCount += d.Length;

         Matrix<float> res = new Matrix<float>(elementCount, 3);

         Int64 address = res.MCvMat.data.ToInt64();

         foreach (MCvPoint3D32f[] d in data)
         {
            int lengthInBytes = d.Length * StructSize.MCvPoint3D32f;
            GCHandle handle = GCHandle.Alloc(d, GCHandleType.Pinned);
            Emgu.Util.Toolbox.memcpy(new IntPtr(address), handle.AddrOfPinnedObject(), lengthInBytes);
            handle.Free();
            address += lengthInBytes;
         }

         return res;
      }

      private static Matrix<float> ToMatrix(PointF[][] data)
      {
         int elementCount = 0;
         foreach (PointF[] d in data) elementCount += d.Length;

         Matrix<float> res = new Matrix<float>(elementCount, 2);
         Int64 address = res.MCvMat.data.ToInt64();

         foreach (PointF[] d in data)
         {
            int lengthInBytes = d.Length * StructSize.PointF;
            GCHandle handle = GCHandle.Alloc(d, GCHandleType.Pinned);
            Emgu.Util.Toolbox.memcpy(new IntPtr(address), handle.AddrOfPinnedObject(), lengthInBytes);
            handle.Free();
            address += lengthInBytes;
         }

         return res;
      }
      #endregion
   }
}
