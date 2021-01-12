//----------------------------------------------------------------------------
//  Copyright (C) 2004-2021 by EMGU Corporation. All rights reserved.       
//----------------------------------------------------------------------------
/*
#if !(__UNIFIED__ || __ANDROID__ || NETSTANDARD)

using Emgu.CV.Util;
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
      : VideoCapture
   {
      /// <summary>
      /// Camera output mode
      /// </summary>
      public enum ImageGeneratorOutputMode
      {
         /// <summary>
         /// VGA resolution
         /// </summary>
         Vga30Hz = 0,
         /// <summary>
         /// SXVGA resolution
         /// </summary>
         Sxga15Hz = 1,
         /// <summary>
         /// SXVGA resolution
         /// </summary>
         Sxga30Hz = 2,
         /// <summary>
         /// QVGA resolution
         /// </summary>
         Qvga30Hz = 3,
         /// <summary>
         /// QVGA resolution
         /// </summary>
         Qvga60Hz = 4
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
      /// Kinect device type
      /// </summary>
      public enum DeviceType
      {
         /// <summary>
         /// kinect
         /// </summary>
         Kinect,
         /// <summary>
         /// Asus xtion
         /// </summary>
         Xtion
      }

      /// <summary>
      /// Create the Kinect Camera capture object
      /// </summary>
      /// <param name="type">The kinect device type</param>
      /// <param name="outputMode">The output mode</param>
      public KinectCapture(DeviceType type, ImageGeneratorOutputMode outputMode)
         : base(type == DeviceType.Kinect ? CvEnum.CaptureType.OpenNI : CvEnum.CaptureType.OpenNIAsus)
      {
         SetCaptureProperty(Emgu.CV.CvEnum.CapProp.OpenniImageGeneratorOutputMode, (double)outputMode);
      }

      /// <summary>
      /// Retrieve Gray frame from Kinect
      /// </summary>
      /// <returns>A Gray frame from Kinect</returns>
      public bool RetrieveGrayFrame(IOutputArray image)
      {
         return Retrieve(image, (int)OpenNIDataType.GrayImage);
      }

      /// <summary>
      /// Retrieve Bgr frame from Kinect
      /// </summary>
      /// <returns>A Bgr frame from Kinect</returns>
      public bool RetrieveBgrFrame(IOutputArray image)
      {
         return Retrieve(image, (int)OpenNIDataType.BgrImage);
      }

      /// <summary>
      /// Retrieve disparity map (in pixels) from Kinect
      /// </summary>
      /// <returns>The disparity map from Kinect</returns>
      public bool RetrieveDisparityMap(IOutputArray image)
      {
         return Retrieve(image, (int)OpenNIDataType.DisparityMap);
      }

      /// <summary>
      /// Retrieve disparity map (in pixels) from Kinect
      /// </summary>
      /// <returns>The disparity map from Kinect</returns>
      public bool RetrieveDisparityMap32f(IOutputArray image)
      {
         return Retrieve(image, (int)OpenNIDataType.DisparityMap32f);        
      }

      /// <summary>
      /// Retrieve the valid depth map from Kinect
      /// </summary>
      /// <returns>The valid depth map from Kinect</returns>
      public bool RetrieveValidDepthMap(IOutputArray image)
      {
         return Retrieve(image, (int)OpenNIDataType.ValidDepthMask);
      }

      /// <summary>
      /// Retrieve the depth map from Kinect (in mm)
      /// </summary>
      /// <returns>The depth map from Kinect (in mm)</returns>
      public bool RetrieveDepthMap(IOutputArray image)
      {
         return Retrieve(image, (int)OpenNIDataType.DepthMap);
      }

      /// <summary>
      /// Retrieve all the points (x, y, z position in meters) from Kinect, row by row.
      /// </summary>
      /// <returns>All the points (x, y, z position in meters) from Kinect, row by row.</returns>
      public bool RetrievePointCloudMap(IOutputArray image)
      {
         return Retrieve(image, (int) OpenNIDataType.PointCloudMap);
      }

      /// <summary>
      /// Get an enumerator of the colored points from Kinect. This function can only be called after the Grab() function.
      /// </summary>
      /// <param name="mask">The mask that controls which points should be returned. You can use the result from RetrieveValidDepthMap() function. Use null if you want all points to be returned</param>
      /// <returns>An enumerator of the colored points from Kinect</returns>
      public ColorPoint[] GetColorPoints(Image<Gray, Byte> mask)
      {
         using (VectorOfColorPoint vcp = new VectorOfColorPoint())
         {
            CvInvoke.OpenniGetColorPoints(Ptr, vcp, mask);
            return vcp.ToArray();
         }
         
      }

      /// <summary>
      /// Given the minimum distance in mm, return the maximum valid disparity value.
      /// </summary>
      /// <param name="minDistance">The minimum distance that an object is away from the camera</param>
      /// <returns>The maximum valid disparity</returns>
      public double GetMaxDisparity(double minDistance)
      {
         //baseline in mm
         double baseline = GetCaptureProperty(Emgu.CV.CvEnum.CapProp.OpenniDepthGeneratorBaseline);
         //focal length in pixels
         double f = GetCaptureProperty(Emgu.CV.CvEnum.CapProp.OpenniDepthGeneratorFocalLength);

         return baseline / f * minDistance;
      }

     

   }


   public static partial class CvInvoke
   {

      [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      internal static extern void OpenniGetColorPoints(IntPtr capture, IntPtr points, IntPtr mask);
     
   }
   
}
#endif
*/