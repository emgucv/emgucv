//----------------------------------------------------------------------------
//  Copyright (C) 2004-2015 by EMGU Corporation. All rights reserved.       
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
   [Obsolete("This class will be removed in the next release. Please uses the corresponding CvInvoke function instead.")]
   public static class CameraCalibration
   {
      /// <summary>
      /// Estimates intrinsic camera parameters and extrinsic parameters for each of the views
      /// </summary>
      /// <param name="objectPoints">The 3D location of the object points. The first index is the index of image, second index is the index of the point</param>
      /// <param name="imagePoints">The 2D image location of the points. The first index is the index of the image, second index is the index of the point</param>
      /// <param name="imageSize">The size of the image, used only to initialize intrinsic camera matrix</param>
      /// <param name="intrinsicParam">The intrisinc parameters, might contains some initial values. The values will be modified by this function.</param>
      /// <param name="calibrationType">cCalibration type</param>
      /// <param name="termCriteria">The termination criteria</param>
      /// <param name="extrinsicParams">The output array of extrinsic parameters.</param>
      /// <returns>The final reprojection error</returns>
      public static double CalibrateCamera(
         MCvPoint3D32f[][] objectPoints,
         PointF[][] imagePoints,
         Size imageSize,
         IntrinsicCameraParameters intrinsicParam,
         CvEnum.CalibType calibrationType,
         MCvTermCriteria termCriteria,
         out ExtrinsicCameraParameters[] extrinsicParams)
      {
         Debug.Assert(objectPoints.Length == imagePoints.Length, "The number of images for objects points should be equal to the number of images for image points");
         int imageCount = objectPoints.Length;

         using (VectorOfVectorOfPoint3D32F vvObjPts = new VectorOfVectorOfPoint3D32F(objectPoints))
         using (VectorOfVectorOfPointF vvImgPts = new VectorOfVectorOfPointF(imagePoints))
         {
            double reprojectionError = -1;
            using (VectorOfMat rotationVectors = new VectorOfMat())
            using (VectorOfMat translationVectors = new VectorOfMat())
            {
               Mat cameraMat = new Mat();
               Mat distorCoeff = new Mat();
               reprojectionError = CvInvoke.CalibrateCamera(
                   vvObjPts,
                   vvImgPts,
                   imageSize,
                   intrinsicParam.IntrinsicMatrix,
                   intrinsicParam.DistortionCoeffs,
                   rotationVectors,
                   translationVectors,
                   calibrationType,
                   termCriteria);

               extrinsicParams = new ExtrinsicCameraParameters[imageCount];
               for (int i = 0; i < imageCount; i++)
               {
                  ExtrinsicCameraParameters p = new ExtrinsicCameraParameters();
                  using (Mat matR = rotationVectors[i])
                     matR.CopyTo(p.RotationVector);
                  using (Mat matT = translationVectors[i])
                     matT.CopyTo( p.TranslationVector);
                  extrinsicParams[i] = p;
               }
            }
            return reprojectionError;
         }
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
         CvEnum.CalibType flags,
         MCvTermCriteria termCrit,
         out ExtrinsicCameraParameters extrinsicParams,
         out Matrix<double> foundamentalMatrix,
         out Matrix<double> essentialMatrix)
      {
         Debug.Assert(objectPoints.Length == imagePoints1.Length && objectPoints.Length == imagePoints2.Length, "The number of images for objects points should be equal to the number of images for image points");

         using (VectorOfVectorOfPoint3D32F objectPointVec = new VectorOfVectorOfPoint3D32F(objectPoints))
         using (VectorOfVectorOfPointF imagePoints1Vec = new VectorOfVectorOfPointF(imagePoints1))
         using (VectorOfVectorOfPointF imagePoints2Vec = new VectorOfVectorOfPointF(imagePoints2))
         {
            extrinsicParams = new ExtrinsicCameraParameters();
            essentialMatrix = new Matrix<double>(3, 3);
            foundamentalMatrix = new Matrix<double>(3, 3);

            CvInvoke.StereoCalibrate(
               objectPointVec,
               imagePoints1Vec,
               imagePoints2Vec,
               
               intrinsicParam1.IntrinsicMatrix,
               intrinsicParam1.DistortionCoeffs,
               intrinsicParam2.IntrinsicMatrix,
               intrinsicParam2.DistortionCoeffs,
               imageSize,
               extrinsicParams.RotationVector,
               extrinsicParams.TranslationVector,
               essentialMatrix,
               foundamentalMatrix,
               flags,
               termCrit);
         }
      }

      /// <summary>
      /// Estimates extrinsic camera parameters using known intrinsic parameters and extrinsic parameters for each view. The coordinates of 3D object points and their correspondent 2D projections must be specified. This function also minimizes back-projection error. 
      /// </summary>
      /// <param name="objectPoints">The array of object points</param>
      /// <param name="imagePoints">The array of corresponding image points</param>
      /// <param name="intrin">The intrinsic parameters</param>
      /// <param name="method">Method for solving a PnP problem</param>
      /// <returns>The extrinsic parameters</returns>
      public static ExtrinsicCameraParameters SolvePnP(
         MCvPoint3D32f[] objectPoints,
         PointF[] imagePoints,
         IntrinsicCameraParameters intrin, 
         CvEnum.SolvePnpMethod method = CvEnum.SolvePnpMethod.Iterative)
      {
         ExtrinsicCameraParameters p = new ExtrinsicCameraParameters();
         using (VectorOfPoint3D32F objPtVec = new VectorOfPoint3D32F(objectPoints))
         using (VectorOfPointF imgPtVec = new VectorOfPointF(imagePoints))
            CvInvoke.SolvePnP(objPtVec, imgPtVec, intrin.IntrinsicMatrix, intrin.DistortionCoeffs, p.RotationVector, p.TranslationVector, false, method);
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
            CvInvoke.ProjectPoints(
                  pointMatrix,
                  extrin.RotationVector,
                  extrin.TranslationVector,
                  intrin.IntrinsicMatrix,
                  intrin.DistortionCoeffs,
                  imagePointMatrix);
         handle1.Free();
         handle2.Free();
         return imagePoints;
      }

      /*
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
         CvEnum.HomographyMethod method,
         double ransacReprojThreshold = 3,
         )
      {
         HomographyMatrix homography = new HomographyMatrix();
         Mat h = CvInvoke.FindHomography(srcPoints.Ptr, dstPoints.Ptr, method, ransacReprojThreshold);
         if ( !)
         {
            homography.Dispose();
            return null;
         }
         return homography;
      }*/




      /// <summary>
      /// Calculates the matrix of an affine transform such that:
      /// (x'_i,y'_i)^T=map_matrix (x_i,y_i,1)^T
      /// where dst(i)=(x'_i,y'_i), src(i)=(x_i,y_i), i=0..2.
      /// </summary>
      /// <param name="src">Coordinates of 3 triangle vertices in the source image. If the array contains more than 3 points, only the first 3 will be used</param>
      /// <param name="dest">Coordinates of the 3 corresponding triangle vertices in the destination image. If the array contains more than 3 points, only the first 3 will be used</param>
      /// <returns>The 2x3 rotation matrix that defines the Affine transform</returns>
      public static Mat GetAffineTransform(PointF[] src, PointF[] dest)
      {
         Debug.Assert(src.Length >= 3, "The source should contain at least 3 points");
         Debug.Assert(dest.Length >= 3, "The destination should contain at least 3 points");

         
         using (VectorOfPointF ptSrc = src.Length == 3 ? new VectorOfPointF(src) : new VectorOfPointF(new PointF[] {src[0], src[1], src[2]}))
         using (VectorOfPointF ptDest = dest.Length == 3 ? new VectorOfPointF(dest) : new VectorOfPointF(new PointF[]{dest[0], dest[1], dest[2]}))
            return CvInvoke.GetAffineTransform(ptSrc, ptDest);
      }

      /// <summary>
      /// Estimate rigid transformation between 2 point sets.
      /// </summary>
      /// <param name="sourcePoints">The points from the source image</param>
      /// <param name="destinationPoints">The corresponding points from the destination image</param>
      /// <param name="fullAffine">Indicates if full affine should be performed</param>
      /// <returns>If success, the 2x3 rotation matrix that defines the Affine transform. Otherwise null is returned.</returns>
      public static Mat EstimateRigidTransform(PointF[] sourcePoints, PointF[] destinationPoints, bool fullAffine)
      {
         using (VectorOfPointF srcVec = new VectorOfPointF(sourcePoints))
         using (VectorOfPointF dstVec = new VectorOfPointF(destinationPoints))
            return CvInvoke.EstimateRigidTransform(srcVec, dstVec, fullAffine);
      }



      #region helper methods
      private static Matrix<float> ToMatrix<T>(T[][] data)
         where T: struct
      {
         int elementCount = 0;

#if NETFX_CORE
         int structureSize = Marshal.SizeOf<T>();
         int floatValueInStructure = structureSize / Marshal.SizeOf<float>();
#else
         int structureSize = Marshal.SizeOf(typeof(T));
         int floatValueInStructure = structureSize / Marshal.SizeOf(typeof(float));
#endif

         foreach (T[] d in data) 
            elementCount += d.Length;

         Matrix<float> res = new Matrix<float>(elementCount, floatValueInStructure);

         Int64 address = res.MCvMat.Data.ToInt64();

         foreach (T[] d in data)
         {
            int lengthInBytes = d.Length * structureSize;
            GCHandle handle = GCHandle.Alloc(d, GCHandleType.Pinned);
            CvToolbox.Memcpy(new IntPtr(address), handle.AddrOfPinnedObject(), lengthInBytes);
            handle.Free();
            address += lengthInBytes;
         }

         return res;
      }
      #endregion
   }
}
