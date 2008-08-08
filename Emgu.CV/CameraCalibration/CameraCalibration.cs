using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;

namespace Emgu.CV
{
    /// <summary>
    /// Functions used form camera calibration
    /// </summary>
    public static class CameraCalibration
    {
        /// <summary>
        /// Estimates intrinsic camera parameters and extrinsic parameters for each of the views
        /// </summary>
        /// <param name="objectPoints">The 3D location of the object points. The first index is the index of image, second index is the index of the point</param>
        /// <param name="imagePoints">The 2D image location of the points. The first index is the index of the image, second index is the index of the point</param>
        /// <param name="imageSize">The size of the image, used only to initialize intrinsic camera matrix</param>
        /// <param name="intrinsicParam">The intrisinc parameters, might contains some initial value. The value is modified by this function.</param>
        /// <param name="flags">Flags</param>
        /// <param name="extrinsicParams">The output array of extrinsic parameters.</param>
        public static void CalibrateCamera(Point3D<float>[][] objectPoints, Point2D<float>[][] imagePoints, ref MCvSize imageSize, IntrinsicCameraParameters intrinsicParam, int flags, out ExtrinsicCameraParameters[] extrinsicParams)
        {
            Debug.Assert(objectPoints.Length == imagePoints.Length, "The number of images for objects points should be equal to the number of images for image points");
            int imageCount = objectPoints.Length;

            #region get the matrix that represent the object points
            List<Point<float>> objectPointList = new List<Point<float>>();
            foreach(Point3D<float>[] pts in objectPoints)
                objectPointList.AddRange(pts);
            Matrix<float> objectPointMatrix = PointCollection.ToMatrix<float>(objectPointList);
            #endregion

            #region get the matrix that represent the image points
            List<Point<float>> imagePointList = new List<Point<float>>();
            foreach (Point2D<float>[] pts in imagePoints)
                imagePointList.AddRange(pts);
            Matrix<float> imagePointMatrix = PointCollection.ToMatrix<float>(imagePointList);
            #endregion

            #region get the matrix that represent the point counts
            int[] pointCounts = new int[objectPoints.Length];
            for (int i = 0; i < objectPoints.Length; i++)
            {
                Debug.Assert(objectPoints[i].Length == imagePoints[i].Length, String.Format("Number of 3D points and image points should be equal in the {0}th image", i));
                pointCounts[i] = objectPoints[i].Length;
            }
            Matrix<int> pointCountsMatrix = new Matrix<int>(pointCounts);
            #endregion

            Matrix<float> rotationVectors = new Matrix<float>(3, imageCount-1);
            Matrix<float> translationVectors = new Matrix<float>(3, imageCount-1);

            CvInvoke.cvCalibrateCamera2(
                objectPointMatrix.Ptr, 
                imagePointMatrix.Ptr, 
                pointCountsMatrix.Ptr,
                imageSize, 
                intrinsicParam.IntrinsicMatrix, 
                intrinsicParam.DistortionCoeffs,
                rotationVectors, 
                translationVectors, 
                flags);

            extrinsicParams = new ExtrinsicCameraParameters[imageCount - 1];
            for (int i = 0; i < imageCount - 1; i++)
            {
                RotationVector3D rot = new RotationVector3D( new float[]{ rotationVectors[0, i], rotationVectors[1, i], rotationVectors[2,i]});
                extrinsicParams[i] = new ExtrinsicCameraParameters(rot, translationVectors.GetCol(i).Transpose());
            }
        }

        /// <summary>
        /// Estimates extrinsic camera parameters using known intrinsic parameters and and extrinsic parameters for each view. The coordinates of 3D object points and their correspondent 2D projections must be specified. This function also minimizes back-projection error. 
        /// </summary>
        /// <param name="objectPoints">The array of object points</param>
        /// <param name="imagePoints">The array of corresponding image points</param>
        /// <param name="intrin">The intrinsic parameters</param>
        /// <returns>the extrinsic parameters</returns>
        public static ExtrinsicCameraParameters FindExtrinsicCameraParams2(
            Point3D<float>[] objectPoints,
            Point2D<float>[] imagePoints,
            IntrinsicCameraParameters intrin)
        {
            ExtrinsicCameraParameters res = new ExtrinsicCameraParameters();
            Matrix<float> translation = new Matrix<float>(3, 1);
            RotationVector3D rotation = new RotationVector3D();

            Matrix<float> objectPointMatrix = PointCollection.ToMatrix(Emgu.Utils.IEnumConvertor<Point2D<float>, Point<float>>(objectPoints, delegate(Point2D<float> p) { return (Point<float>)p; }));
            Matrix<float> imagePointMatrix = PointCollection.ToMatrix(Emgu.Utils.IEnumConvertor<Point2D<float>, Point<float>>(imagePoints, delegate(Point2D<float> p) { return (Point<float>)p; }));

            CvInvoke.cvFindExtrinsicCameraParams2(objectPointMatrix.Ptr, imagePointMatrix.Ptr, intrin.IntrinsicMatrix.Ptr, intrin.DistortionCoeffs.Ptr, rotation.Ptr, translation.Ptr);

            res.TranslationVector = translation;
            res.RotationVector = rotation;

            return res;
        }

        /// <summary>
        /// Transforms the image to compensate radial and tangential lens distortion. 
        /// The camera matrix and distortion parameters can be determined using cvCalibrateCamera2. For every pixel in the output image the function computes coordinates of the corresponding location in the input image using the formulae in the section beginning. Then, the pixel value is computed using bilinear interpolation. If the resolution of images is different from what was used at the calibration stage, fx, fy, cx and cy need to be adjusted appropriately, while the distortion coefficients remain the same
        /// </summary>
        /// <typeparam name="C">The color type of the image</typeparam>
        /// <typeparam name="D">The depth of the image</typeparam>
        /// <param name="src">The distorted image</param>
        /// <param name="intrin">The intrinsic camera parameters</param>
        /// <returns>The corrected image</returns>
        public static Image<C, D> Undistort2<C, D>(Image<C, D> src, IntrinsicCameraParameters intrin) 
            where C: ColorType, new()
            where D: IComparable, new()
        {
            Image<C, D> res = src.CopyBlank();
            CvInvoke.cvUndistort2(src.Ptr, res.Ptr, intrin.IntrinsicMatrix.Ptr, intrin.DistortionCoeffs.Ptr);
            return res;
        }

        /// <summary>
        /// Computes projections of 3D points to the image plane given intrinsic and extrinsic camera parameters. 
        /// Optionally, the function computes jacobians - matrices of partial derivatives of image points as functions of all the input parameters w.r.t. the particular parameters, intrinsic and/or extrinsic. 
        /// The jacobians are used during the global optimization in cvCalibrateCamera2 and cvFindExtrinsicCameraParams2. 
        /// The function itself is also used to compute back-projection error for with current intrinsic and extrinsic parameters. 
        /// </summary>
        /// <remarks>Note, that with intrinsic and/or extrinsic parameters set to special values, the function can be used to compute just extrinsic transformation or just intrinsic transformation (i.e. distortion of a sparse set of points) </remarks>
        /// <param name="objectPoints">The array of object points, 3xN or Nx3, where N is the number of points in the view</param>
        /// <param name="extrin">extrinsic parameters</param>
        /// <param name="intrin">intrinsic parameters</param>
        /// <param name="mats">Optional matrix supplied in the following order: dpdrot, dpdt, dpdf, dpdc, dpddist</param>
        /// <returns>The array of image points which is the projection of <paramref name="objectPoints"/></returns>
        public static Point2D<float>[] ProjectPoints2(
            Point3D<float>[] objectPoints,
            ExtrinsicCameraParameters extrin,
            IntrinsicCameraParameters intrin,
            params Matrix<float>[] mats)
        {
            Matrix<float> pointMatrix = PointCollection.ToMatrix( Emgu.Utils.IEnumConvertor<Point2D<float>, Point<float>>(objectPoints, delegate(Point2D<float> p) { return (Point<float>)p; })  );
            IntPtr dpdrot = mats.Length > 0 ? mats[0].Ptr : IntPtr.Zero;
            IntPtr dpdt = mats.Length > 1 ? mats[1].Ptr : IntPtr.Zero;
            IntPtr dpdf = mats.Length > 2 ? mats[2].Ptr : IntPtr.Zero;
            IntPtr dpdc = mats.Length > 3 ? mats[3].Ptr : IntPtr.Zero;
            IntPtr dpddist = mats.Length > 4 ? mats[4].Ptr : IntPtr.Zero;

            Matrix<float> point2DMatrix = new Matrix<float>(objectPoints.Length, 2);
            CvInvoke.cvProjectPoints2(pointMatrix.Ptr, extrin.RotationVector.Ptr, extrin.TranslationVector.Ptr, intrin.IntrinsicMatrix.Ptr, intrin.DistortionCoeffs.Ptr, point2DMatrix.Ptr, dpdrot, dpdt, dpdf, dpdc, dpddist);
            Point2D<float>[] imagePoints = new Point2D<float>[objectPoints.Length];
            for (int i = 0; i < imagePoints.Length; i++)
                imagePoints[i] = new Point2D<float>(point2DMatrix[i, 0], point2DMatrix[i, 1]);
            return imagePoints;
        }

        /// <summary>
        /// Finds perspective transformation H=||h_ij|| between the source and the destination planes
        /// </summary>
        /// <param name="srcPoints">Point coordinates in the original plane, 2xN, Nx2, 3xN or Nx3 array (the latter two are for representation in homogenious coordinates), where N is the number of points</param>
        /// <param name="dstPoints">Point coordinates in the destination plane, 2xN, Nx2, 3xN or Nx3 array (the latter two are for representation in homogenious coordinates) </param>
        /// <returns>The 3x3 homography matrix. </returns>
        public static Matrix<float> FindHomography( Matrix<float> srcPoints, Matrix<float> dstPoints)
        {
            Matrix<float> homography = new Matrix<float>(3, 3);
            CvInvoke.cvFindHomography(srcPoints.Ptr, dstPoints.Ptr, homography.Ptr);
            return homography;
        }

        /// <summary>
        /// Finds perspective transformation H=||h_ij|| between the source and the destination planes
        /// </summary>
        /// <param name="srcPoints">Point coordinates in the original plane</param>
        /// <param name="dstPoints">Point coordinates in the destination plane</param>
        /// <returns>The 3x3 homography matrix. </returns>
        public static Matrix<float> FindHomography(Point2D<float>[] srcPoints, Point2D<float>[] dstPoints)
        {
            Matrix<float> srcPointMatrix = PointCollection.ToMatrix<float>(Emgu.Utils.IEnumConvertor<Point2D<float>, Point<float>>(srcPoints, delegate(Point2D<float> p) { return (Point<float>)p; }));
            Matrix<float> dstPointMatrix = PointCollection.ToMatrix<float>(Emgu.Utils.IEnumConvertor<Point2D<float>, Point<float>>(dstPoints, delegate(Point2D<float> p) { return (Point<float>)p; }));
            return FindHomography(srcPointMatrix, dstPointMatrix);
        }
    }
}
