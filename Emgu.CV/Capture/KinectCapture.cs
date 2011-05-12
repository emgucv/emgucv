//----------------------------------------------------------------------------
//  Copyright (C) 2004-2011 by EMGU. All rights reserved.       
//----------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Drawing;
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
      /// Create the Kinet Camera capture object
      /// </summary>
      /// <param name="outputMode"></param>
      public KinectCapture(ImageGeneratorOutputMode outputMode)
         : base(CvEnum.CaptureType.OPENNI)
      {
         SetCaptureProperty(Emgu.CV.CvEnum.CAP_PROP.CV_CAP_OPENNI_IMAGE_GENERATOR_OUTPUT_MODE, (double)outputMode);
      }

      /// <summary>
      /// Retrieve Gray frame from Kinet
      /// </summary>
      /// <returns>A Gray frame from Kinet</returns>
      public Image<Gray, Byte> RetrieveGrayFrame()
      {
         return RetrieveGrayFrame((int)OpenNIDataType.GrayImage);
      }

      /// <summary>
      /// Retrieve Bgr frame from Kinet
      /// </summary>
      /// <returns>A Bgr frame from Kinet</returns>
      public Image<Bgr, Byte> RetrieveBgrFrame()
      {
         return RetrieveBgrFrame((int)OpenNIDataType.BgrImage);
      }

      /// <summary>
      /// Retrieve disparity map from Kinet
      /// </summary>
      /// <returns>The disparity map from Kinet</returns>
      public Image<Gray, Byte> RetrieveDisparityMap()
      {
         return RetrieveGrayFrame((int)OpenNIDataType.DisparityMap);
      }

      /// <summary>
      /// Retrieve the valid depth map from Kinet
      /// </summary>
      /// <returns>The valid depth map from Kinet</returns>
      public Image<Gray, Byte> RetrieveValidDepthMap()
      {
         return RetrieveGrayFrame((int)OpenNIDataType.ValidDepthMask);
      }

   }
}
