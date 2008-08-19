using System;
using System.Collections.Generic;
using System.Text;

namespace Emgu.CV
{
   /// <summary>
   /// The intrinsic camera parameters
   /// </summary>
   public class IntrinsicCameraParameters
   {
      private Matrix<float> _intrinsicMatrix;
      private Matrix<float> _distortionCoeffs;

      /// <summary>
      /// Get or Set the DistortionCoeffs ( as a 3x1 matrix )
      /// </summary>
      public Matrix<float> DistortionCoeffs
      {
         get { return _distortionCoeffs; }
         set { _distortionCoeffs = value; }
      }

      /// <summary>
      /// Get or Set the intrinsic matrix
      /// </summary>
      public Matrix<float> IntrinsicMatrix
      {
         get { return _intrinsicMatrix; }
         set { _intrinsicMatrix = value; }
      }

      /// <summary>
      /// Create the intrinsic camera parameters
      /// </summary>
      public IntrinsicCameraParameters()
      {
         _intrinsicMatrix = new Matrix<float>(3, 3);
         _distortionCoeffs = new Matrix<float>(3, 1);
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
   }
}
