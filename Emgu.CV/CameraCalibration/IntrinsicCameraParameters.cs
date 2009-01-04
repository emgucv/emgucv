using System;
using System.Collections.Generic;
using System.Text;
using Emgu.CV.Structure;

namespace Emgu.CV
{
   /// <summary>
   /// The intrinsic camera parameters
   /// </summary>
   public class IntrinsicCameraParameters
   {
      private Matrix<double> _intrinsicMatrix;
      private Matrix<double> _distortionCoeffs;

      /// <summary>
      /// Get or Set the DistortionCoeffs ( as a 5x1 (default) or 4x1 matrix ). 
      /// The ordering of the distortion coefficients is the following:
      /// (k1, k2, p1, p2[, k3]).
      /// That is, the first 2 radial distortion coefficients are followed by 2 tangential distortion coefficients and then, optionally, by the third radial distortion coefficients. Such ordering is used to keep backward compatibility with previous versions of OpenCV
      /// </summary>
      public Matrix<double> DistortionCoeffs
      {
         get { return _distortionCoeffs; }
         set { _distortionCoeffs = value; }
      }

      /// <summary>
      /// Get or Set the intrinsic matrix (3x3)
      /// </summary>
      public Matrix<double> IntrinsicMatrix
      {
         get { return _intrinsicMatrix; }
         set { _intrinsicMatrix = value; }
      }

      /// <summary>
      /// Create the intrinsic camera parameters
      /// </summary>
      public IntrinsicCameraParameters()
      {
         _intrinsicMatrix = new Matrix<double>(3, 3);
         _distortionCoeffs = new Matrix<double>(5, 1);
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
         CvInvoke.cvInitUndistortMap(IntrinsicMatrix.Ptr, DistortionCoeffs.Ptr, mapx, mapy);
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
         CvInvoke.cvCalibrationMatrixValues(_intrinsicMatrix.Ptr, imgWidth, imgHeight, apertureWidth, apertureHeight, ref fovx, ref fovy, ref focalLength, ref principalPoint, ref pixelAspectRatio);
      }

   }
}
