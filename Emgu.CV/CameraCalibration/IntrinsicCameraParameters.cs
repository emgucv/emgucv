//----------------------------------------------------------------------------
//  Copyright (C) 2004-2015 by EMGU Corporation. All rights reserved.       
//----------------------------------------------------------------------------

using System;
using Emgu.CV.Structure;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Diagnostics;

namespace Emgu.CV
{
   /// <summary>
   /// Intrinsic camera parameters
   /// </summary>
#if !NETFX_CORE
   [Serializable]
#endif
   [Obsolete("This class will be removed in the next release, please use separate camera matrix and distortion coefficient with the CvInvoke function instead.")]
   public class IntrinsicCameraParameters : IEquatable<IntrinsicCameraParameters>
   {
      private Matrix<double> _intrinsicMatrix;
      private Matrix<double> _distortionCoeffs;

      /// <summary>
      /// Get or Set the DistortionCoeffs ( as a 5x1 (default), 4x1 or 8x1 matrix ). 
      /// The ordering of the distortion coefficients is the following:
      /// (k1, k2, p1, p2[, k3 [,k4, k5, k6]]).
      /// That is, the first 2 radial distortion coefficients are followed by 2 tangential distortion coefficients and then, optionally, by the third radial distortion coefficients. Such ordering is used to keep backward compatibility with previous versions of OpenCV
      /// </summary>
      public Matrix<double> DistortionCoeffs
      {
         get { return _distortionCoeffs; }
         set 
         {
            Debug.Assert((value.Rows == 4 || value.Rows == 5 || value.Rows == 8) && value.Cols == 1, "The distortion coefficient should be either 4x1, 5x1 or 8x1");
            _distortionCoeffs = value; 
         }
      }

      /// <summary>
      /// Get or Set the intrinsic matrix (3x3)
      /// </summary>
      public Matrix<double> IntrinsicMatrix
      {
         get { return _intrinsicMatrix; }
         set 
         {
            Debug.Assert(value.Rows == 3 && value.Cols == 3, "The intrinsic matrix should be 3x3");
            _intrinsicMatrix = value; 
         }
      }

      /// <summary>
      /// Create the intrinsic camera parameters
      /// </summary>
      public IntrinsicCameraParameters()
         : this(8)
      {
      }

      /// <summary>
      /// Create the intrinsic camera parameters 
      /// </summary>
      /// <param name="distortionCoeffsCount">The number of distortion coefficients. Should be either 4, 5 or 8.</param>
      public IntrinsicCameraParameters(int distortionCoeffsCount)
      {
         IntrinsicMatrix = new Matrix<double>(3, 3);
         DistortionCoeffs = new Matrix<double>(distortionCoeffsCount, 1);
      }

      /// <summary>
      /// Pre-computes the undistortion map - coordinates of the corresponding pixel in the distorted image for every pixel in the corrected image. Then, the map (together with input and output images) can be passed to cvRemap function.
      /// </summary>
      /// <param name="width">The width of the image</param>
      /// <param name="height">The height of the image</param>
      /// <param name="mapx">The output array of x-coordinates of the map</param>
      /// <param name="mapy">The output array of y-coordinates of the map</param>
      public void InitUndistortMap(int width, int height, out Matrix<float> mapx, out Matrix<float> mapy)
      {
         mapx = new Matrix<float>(height, width);
         mapy = new Matrix<float>(height, width);
         CvInvoke.InitUndistortRectifyMap(IntrinsicMatrix, DistortionCoeffs, null, IntrinsicMatrix, new Size(width, height), CvEnum.DepthType.Cv32F, mapx, mapy);
      }

      /// <summary>
      /// computes various useful camera (sensor/lens) characteristics using the computed camera calibration matrix, image frame resolution in pixels and the physical aperture size
      /// </summary>
      /// <param name="imgWidth">Image width in pixels</param>
      /// <param name="imgHeight">Image height in pixels</param>
      /// <param name="apertureWidth">Aperture width in realworld units (optional input parameter). Set it to 0 if not used</param>
      /// <param name="apertureHeight">Aperture width in realworld units (optional input parameter). Set it to 0 if not used</param>
      /// <param name="fovx">Field of view angle in x direction in degrees</param>
      /// <param name="fovy">Field of view angle in y direction in degrees </param>
      /// <param name="focalLength">Focal length in realworld units </param>
      /// <param name="principalPoint">The principal point in realworld units </param>
      /// <param name="pixelAspectRatio">The pixel aspect ratio ~ fy/f</param>
      public void GetIntrinsicMatrixValues(
         int imgWidth,
         int imgHeight,
         double apertureWidth,
         double apertureHeight,
         out double fovx,
         out double fovy,
         out double focalLength,
         out MCvPoint2D64f principalPoint,
         out double pixelAspectRatio)
      {
         fovx = 0;
         fovy = 0;
         focalLength = 0;
         principalPoint = new MCvPoint2D64f();
         pixelAspectRatio = 0;
         CvInvoke.CalibrationMatrixValues(_intrinsicMatrix, new Size(imgWidth, imgHeight), apertureWidth, apertureHeight, ref fovx, ref fovy, ref focalLength, ref principalPoint, ref pixelAspectRatio);
      }

      /// <summary>
      /// Similar to cvInitUndistortRectifyMap and is opposite to it at the same time.
      /// The functions are similar in that they both are used to correct lens distortion and to perform the optional perspective (rectification) transformation.
      /// They are opposite because the function cvInitUndistortRectifyMap does actually perform the reverse transformation in order to initialize the maps properly, while this function does the forward transformation.
      /// </summary>
      /// <param name="src">The observed point coordinates</param>
      /// <param name="R">Optional rectification transformation in object space (3x3 matrix). R1 or R2, computed by cvStereoRectify can be passed here. If null, the identity matrix is used.</param>
      /// <param name="P">Optional new camera matrix (3x3) or the new projection matrix (3x4). P1 or P2, computed by cvStereoRectify can be passed here. If null, the identity matrix is used.</param>
      public PointF[] Undistort(PointF[] src, Matrix<double> R = null, Matrix<double> P = null)
      {
         PointF[] dst = new PointF[src.Length]; 
         GCHandle srcHandle = GCHandle.Alloc(src, GCHandleType.Pinned);
         GCHandle dstHandle = GCHandle.Alloc(dst, GCHandleType.Pinned);
         using (Matrix<float> srcPointMatrix = new Matrix<float>(src.Length, 1, 2, srcHandle.AddrOfPinnedObject(), 2 * sizeof(float)))
         using (Matrix<float> dstPointMatrix = new Matrix<float>(dst.Length, 1, 2, dstHandle.AddrOfPinnedObject(), 2 * sizeof(float)))
         {
            CvInvoke.UndistortPoints(
                srcPointMatrix, dstPointMatrix,
                _intrinsicMatrix,
                _distortionCoeffs,
                R,
                P);
         }
         srcHandle.Free();
         dstHandle.Free();
         return dst;
      }

      /// <summary>
      /// Transforms the image to compensate radial and tangential lens distortion. 
      /// The camera matrix and distortion parameters can be determined using cvCalibrateCamera2. For every pixel in the output image the function computes coordinates of the corresponding location in the input image using the formulae in the section beginning. Then, the pixel value is computed using bilinear interpolation. If the resolution of images is different from what was used at the calibration stage, fx, fy, cx and cy need to be adjusted appropriately, while the distortion coefficients remain the same
      /// </summary>
      /// <typeparam name="TColor">The color type of the image</typeparam>
      /// <typeparam name="TDepth">The depth of the image</typeparam>
      /// <param name="src">The distorted image</param>
      /// <returns>The corrected image</returns>
      public Image<TColor, TDepth> Undistort<TColor, TDepth>(Image<TColor, TDepth> src)
         where TColor : struct, IColor
         where TDepth : new()
      {
         Image<TColor, TDepth> res = src.CopyBlank();
         CvInvoke.Undistort(
            src, 
            res, 
            _intrinsicMatrix, 
            _distortionCoeffs);
         return res;
      }

      #region IEquatable<IntrinsicCameraParameters> Members

      /// <summary>
      /// Return true if the two intrinsic camera parameters are equal
      /// </summary>
      /// <param name="other">The other intrinsic camera parameters to compare with</param>
      /// <returns>True if the two intrinsic camera parameters are equal</returns>
      public bool Equals(IntrinsicCameraParameters other)
      {
         return _intrinsicMatrix.Equals(other.IntrinsicMatrix)
            && _distortionCoeffs.Equals(other.DistortionCoeffs);
      }

      #endregion
   }
}
