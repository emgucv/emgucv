//----------------------------------------------------------------------------
//  Copyright (C) 2004-2011 by EMGU. All rights reserved.       
//----------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Text;
using Emgu.CV.Structure;

namespace Emgu.CV
{
   /// <summary>
   /// Kinect Camera capture
   /// </summary>
   public class KinectCapture
      : Capture
   {
      #region PInvoke
      [DllImport(CvInvoke.EXTERN_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      private static extern void OpenniGetColorPoints(IntPtr capture, IntPtr points /* sequence of ColorPoint */, IntPtr mask);

      [DllImport(CvInvoke.OPENCV_HIGHGUI_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      private static extern IntPtr cvGetOpenniCaptureContext(IntPtr capture);
      #endregion

      /// <summary>
      /// Camera output mode
      /// </summary>
      public enum ImageGeneratorOutputMode
      {
         /// <summary>
         /// VGA resolution
         /// </summary>
         VGA_30HZ = 0,
         /// <summary>
         /// SXVGA resolution
         /// </summary>
         SXGA_15HZ = 1
      }

      /// <summary>
      /// Open ni data type used by the retrieve functions
      /// </summary>
      public enum OpenNIDataType
      {
         #region Data given from depth generator.
         /// <summary>
         /// Depth values in mm (CV_16UC1)
         /// </summary>
         DepthMap = 0,
         /// <summary>
         /// XYZ in meters (CV_32FC3)
         /// </summary>
         PointCloudMap = 1,
         /// <summary>
         /// Disparity in pixels (CV_8UC1)
         /// </summary>
         DisparityMap = 2,
         /// <summary>
         /// Disparity in pixels (CV_32FC1)
         /// </summary>
         DisparityMap32f = 3,
         /// <summary>
         ///  CV_8UC1
         /// </summary>
         ValidDepthMask = 4,
         #endregion

         #region Data given from RGB image generator.
         /// <summary>
         /// Bgr image 
         /// </summary>
         BgrImage = 5,
         /// <summary>
         /// Gray Image
         /// </summary>
         GrayImage = 6
         #endregion
      }

      /// <summary>
      /// Create the Kinect Camera capture object
      /// </summary>
      /// <param name="outputMode">The output mode</param>
      public KinectCapture(ImageGeneratorOutputMode outputMode)
         : base(CvEnum.CaptureType.OPENNI)
      {
         SetCaptureProperty(Emgu.CV.CvEnum.CAP_PROP.CV_CAP_OPENNI_IMAGE_GENERATOR_OUTPUT_MODE, (double)outputMode);
      }

      /// <summary>
      /// Retrieve Gray frame from Kinect
      /// </summary>
      /// <returns>A Gray frame from Kinect</returns>
      public Image<Gray, Byte> RetrieveGrayFrame()
      {
         return RetrieveGrayFrame((int)OpenNIDataType.GrayImage);
      }

      /// <summary>
      /// Retrieve Bgr frame from Kinect
      /// </summary>
      /// <returns>A Bgr frame from Kinect</returns>
      public Image<Bgr, Byte> RetrieveBgrFrame()
      {
         return RetrieveBgrFrame((int)OpenNIDataType.BgrImage);
      }

      /// <summary>
      /// Retrieve disparity map (in pixels) from Kinect
      /// </summary>
      /// <returns>The disparity map from Kinect</returns>
      public Image<Gray, Byte> RetrieveDisparityMap()
      {
         return RetrieveGrayFrame((int)OpenNIDataType.DisparityMap);
      }

      /// <summary>
      /// Retrieve disparity map (in pixels) from Kinect
      /// </summary>
      /// <returns>The disparity map from Kinect</returns>
      public Image<Gray, float> RetrieveDisparityMap32f()
      {
         IntPtr img = CvInvoke.cvRetrieveFrame(Ptr, (int)OpenNIDataType.DisparityMap32f);
         if (img == IntPtr.Zero)
            return null;
         MIplImage iplImage = (MIplImage)Marshal.PtrToStructure(img, typeof(MIplImage));

         Image<Gray, float> res = new Image<Gray, float>(iplImage.width, iplImage.height, iplImage.widthStep, iplImage.imageData);

         //inplace flip the image if necessary
         res._Flip(FlipType);

         return res;
      }

      /// <summary>
      /// Retrieve the valid depth map from Kinect
      /// </summary>
      /// <returns>The valid depth map from Kinect</returns>
      public Image<Gray, Byte> RetrieveValidDepthMap()
      {
         return RetrieveGrayFrame((int)OpenNIDataType.ValidDepthMask);
      }

      /// <summary>
      /// Retrieve the depth map from Kinect (in mm)
      /// </summary>
      /// <returns>The depth map from Kinect (in mm)</returns>
      public Image<Gray, int> RetrieveDepthMap()
      {
         IntPtr img = CvInvoke.cvRetrieveFrame(Ptr, (int)OpenNIDataType.DepthMap);
         if (img == IntPtr.Zero)
            return null;
         MIplImage iplImage = (MIplImage)Marshal.PtrToStructure(img, typeof(MIplImage));

         Image<Gray, int> res = new Image<Gray, int>(iplImage.width, iplImage.height, iplImage.widthStep, iplImage.imageData);

         //inplace flip the image if necessary
         res._Flip(FlipType);

         return res;
      }

      /// <summary>
      /// Retrieve all the points (x, y, z position in meters) from Kinect, row by row.
      /// </summary>
      /// <returns>All the points (x, y, z position in meters) from Kinect, row by row.</returns>
      public MCvPoint3D32f[] RetrievePointCloudMap()
      {
         IntPtr img = CvInvoke.cvRetrieveFrame(Ptr, (int)OpenNIDataType.PointCloudMap);
         if (img == IntPtr.Zero)
            return null;

         if (FlipType != Emgu.CV.CvEnum.FLIP.NONE)
            CvInvoke.cvFlip(img, img, FlipType);

         MIplImage iplImage = (MIplImage)Marshal.PtrToStructure(img, typeof(MIplImage));

         MCvPoint3D32f[] points = new MCvPoint3D32f[iplImage.width * iplImage.height];
         GCHandle handle = GCHandle.Alloc(points, GCHandleType.Pinned);
         using (Matrix<float> m = new Matrix<float>(iplImage.height, iplImage.width, handle.AddrOfPinnedObject()))
         {
            CvInvoke.cvCopy(img, m, IntPtr.Zero);
         }
         handle.Free();

         return points;
      }

      /// <summary>
      /// Get an enumerator of the colored points from Kinect. This function can only be called after the Grab() function.
      /// </summary>
      /// <param name="mask">The mask that controls which points should be returned. You can use the result from RetrieveValidDepthMap() function. Use null if you want all points to be returned</param>
      /// <returns>An enumerator of the colored points from Kinect</returns>
      public ColorPoint[] GetColorPoints(Image<Gray, Byte> mask)
      {
         using (MemStorage stor = new MemStorage())
         {
            Seq<ColorPoint> seq = new Seq<ColorPoint>(stor);
            OpenniGetColorPoints(Ptr, seq, mask);
            return seq.ToArray();
         }
         /*
         MCvPoint3D32f[] positions = RetrievePointCloudMap();
         if (positions == null)
            yield break;

         using (Image<Bgr, Byte> image = RetrieveBgrFrame())
         using (Image<Gray, Byte> validDepthMap = RetrieveValidDepthMap())
         {
            int imageWidth = image.Width;
            int imageHeight = image.Height;
            Byte[] mask = validDepthMap.Bytes;

            int i = 0;
            for (int h = 0; h < imageHeight; h++)
               for (int w = 0; w < imageWidth; w++, i++)
                  if (mask[i] != 0)
                  {
                     ColorPoint cp = new ColorPoint();
                     cp.Color = new Bgr(image.Data[h, w, 0], image.Data[h, w, 1], image.Data[h, w, 2]);
                     cp.Position = positions[i];
                     yield return cp;
                  }
         }*/
      }

      /// <summary>
      /// Given the minimum distance in mm, return the maximum valid disparity value.
      /// </summary>
      /// <param name="minDistance">The minimum distance that an object is away from the camera</param>
      /// <returns>The maximum valid disparity</returns>
      public double GetMaxDisparity(double minDistance)
      {
         //baseline in mm
         double baseline = GetCaptureProperty(Emgu.CV.CvEnum.CAP_PROP.CV_CAP_OPENNI_DEPTH_GENERATOR_BASELINE);
         //focal length in pixels
         double f = GetCaptureProperty(Emgu.CV.CvEnum.CAP_PROP.CV_CAP_OPENNI_DEPTH_GENERATOR_FOCAL_LENGTH);

         return baseline / f * minDistance;
      }

      /// <summary>
      /// Get the unmanaged OpenNI Context from the capture.
      /// </summary>
      /// <returns>Pointer to the OpenNI context</returns>
      /// <remarks>This function required the opencv_highgui module patched by EMGU CV, otherwise it will throw entry point not found exception.</remarks>
      public IntPtr GetOpenNIContext()
      {
         return cvGetOpenniCaptureContext(Ptr);
      }

      /// <summary>
      /// A point with Bgr color information
      /// </summary>
      public struct ColorPoint
      {
         /// <summary>
         /// The position in meters
         /// </summary>
         public MCvPoint3D32f Position;

         /// <summary>
         /// The blue color
         /// </summary>
         public Byte Blue;

         /// <summary>
         /// The green color
         /// </summary>
         public Byte Green;

         /// <summary>
         /// The red color
         /// </summary>
         public Byte Red;
      }
   }
}
