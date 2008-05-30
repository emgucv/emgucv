using System;
using System.Collections.Generic;
using System.Text;

namespace Emgu.CV.CameraCalibration
{
    /// <summary>
    /// Functions used form camera calibration
    /// </summary>
    public static class CameraCalibration
    {
        /// <summary>
        /// The function cvFindExtrinsicCameraParams2 estimates extrinsic camera parameters using known intrinsic parameters and and extrinsic parameters for each view. The coordinates of 3D object points and their correspondent 2D projections must be specified. This function also minimizes back-projection error. 
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
            RotationVector rotation = new RotationVector();

            Matrix<float> objectPointMatrix = PointCollection<float>.ToMatrix((IEnumerable<Point<float>>)objectPoints);
            Matrix<float> imagePointMatrix = PointCollection<float>.ToMatrix((IEnumerable<Point<float>>)imagePoints);

            CvInvoke.cvFindExtrinsicCameraParams2(objectPointMatrix.Ptr, imagePointMatrix.Ptr, intrin.IntrinsicMatrix.Ptr, intrin.DistortionCoeffs.Ptr, rotation.Ptr, translation.Ptr);

            res.TranslationVector = translation;
            res.RotationVector = rotation;

            return res;
        }

        /// <summary>
        /// The function Undistort2 transforms the image to compensate radial and tangential lens distortion. The camera matrix and distortion parameters can be determined using cvCalibrateCamera2. For every pixel in the output image the function computes coordinates of the corresponding location in the input image using the formulae in the section beginning. Then, the pixel value is computed using bilinear interpolation. If the resolution of images is different from what was used at the calibration stage, fx, fy, cx and cy need to be adjusted appropriately, while the distortion coefficients remain the same
        /// </summary>
        /// <typeparam name="C">The color type of the image</typeparam>
        /// <typeparam name="D">The depth of the image</typeparam>
        /// <param name="src">the distorted image</param>
        /// <param name="intrin">the intrinsic camera parameters</param>
        /// <returns>the corected image</returns>
        public static Image<C, D> Undistort2<C, D>(Image<C, D> src, IntrinsicCameraParameters intrin) 
            where C: ColorType, new()
            where D: IComparable, new()
        {
            Image<C, D> res = src.BlankClone();
            CvInvoke.cvUndistort2(src.Ptr, res.Ptr, intrin.IntrinsicMatrix.Ptr, intrin.DistortionCoeffs.Ptr);
            return res;
        }

        /// <summary>
        /// <para>The function cvProjectPoints2 computes projections of 3D points to the image plane given intrinsic and extrinsic camera parameters. Optionally, the function computes jacobians - matrices of partial derivatives of image points as functions of all the input parameters w.r.t. the particular parameters, intrinsic and/or extrinsic. The jacobians are used during the global optimization in cvCalibrateCamera2 and cvFindExtrinsicCameraParams2. The function itself is also used to compute back-projection error for with current intrinsic and extrinsic parameters.</para>
        /// <para>Note, that with intrinsic and/or extrinsic parameters set to special values, the function can be used to compute just extrinsic transformation or just intrinsic transformation (i.e. distortion of a sparse set of points) </para> 
        /// </summary>
        /// <param name="objectPoints">The array of object points, 3xN or Nx3, where N is the number of points in the view</param>
        /// <param name="extrin">extrinsic parameters</param>
        /// <param name="intrin">intrinsic parameters</param>
        /// <param name="mats">Optional matrix supplied in the following order: dpdrot, dpdt, dpdf, dpdc, dpddist</param>
        /// <returns>A Nx2 matrix of 2D points</returns>
        public static Matrix<float> ProjectPoints2(
            Point3D<float>[] objectPoints,
            ExtrinsicCameraParameters extrin,
            IntrinsicCameraParameters intrin,
            params Matrix<float>[] mats)
        {
            Matrix<float> res = new Matrix<float>(objectPoints.Length, 2);
            Matrix<float> pointMatrix = PointCollection<float>.ToMatrix( (IEnumerable<Point<float>>) objectPoints );
            IntPtr dpdrot = mats.Length > 0 ? mats[0].Ptr : IntPtr.Zero;
            IntPtr dpdt = mats.Length > 1 ? mats[1].Ptr : IntPtr.Zero;
            IntPtr dpdf = mats.Length > 2 ? mats[2].Ptr : IntPtr.Zero;
            IntPtr dpdc = mats.Length > 3 ? mats[3].Ptr : IntPtr.Zero;
            IntPtr dpddist = mats.Length > 4 ? mats[4].Ptr : IntPtr.Zero;

            CvInvoke.cvProjectPoints2(pointMatrix.Ptr, extrin.RotationVector.Ptr, extrin.TranslationVector.Ptr, intrin.IntrinsicMatrix.Ptr, intrin.DistortionCoeffs.Ptr, res.Ptr, dpdrot, dpdt, dpdf, dpdc, dpddist);
            return res;
        }

        /// <summary>
        /// The function cvFindHomography finds perspective transformation H=||hij|| between the source and the destination planes
        /// </summary>
        /// <param name="srcPoints">Point coordinates in the original plane, 2xN, Nx2, 3xN or Nx3 array (the latter two are for representation in homogenious coordinates), where N is the number of points</param>
        /// <param name="dstPoints">Point coordinates in the destination plane, 2xN, Nx2, 3xN or Nx3 array (the latter two are for representation in homogenious coordinates) </param>
        /// <returns>Output 3x3 homography matrix. </returns>
        public static Matrix<float> FindHomography( Matrix<float> srcPoints, Matrix<float> dstPoints)
        {
            Matrix<float> homography = new Matrix<float>(3, 3);
            CvInvoke.cvFindHomography(srcPoints.Ptr, dstPoints.Ptr, homography.Ptr);
            return homography;
        }
    }
}
