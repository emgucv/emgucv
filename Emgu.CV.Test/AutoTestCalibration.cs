//----------------------------------------------------------------------------
//  Copyright (C) 2004-2011 by EMGU. All rights reserved.       
//----------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using Emgu.CV;
using Emgu.UI;
using Emgu.CV.UI;
using Emgu.CV.Structure;
using Emgu.CV.Features2D;
using Emgu.Util;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;
using System.Xml;
using System.IO;
using System.Runtime.Serialization;
using System.Threading;
using System.Runtime.InteropServices;

namespace Emgu.CV.Test
{
   [TestFixture]
   public class AutoTestCalibration
   {
      [Test]
      public void TestCalibration()
      {
         Size patternSize = new Size(9, 6);
         Image<Gray, Byte> left01 = new Image<Gray, byte>("left01.jpg");
         PointF[] corners = CameraCalibration.FindChessboardCorners(left01, patternSize, CvEnum.CALIB_CB_TYPE.DEFAULT);
      }

      [Test]
      public void TestChessboardCalibration()
      {
         Size patternSize = new Size(9, 6);

         Image<Gray, Byte> chessboardImage = new Image<Gray, byte>("left01.jpg");
         PointF[] corners =
            CameraCalibration.FindChessboardCorners(
            chessboardImage,
            patternSize,
            Emgu.CV.CvEnum.CALIB_CB_TYPE.ADAPTIVE_THRESH | Emgu.CV.CvEnum.CALIB_CB_TYPE.NORMALIZE_IMAGE | Emgu.CV.CvEnum.CALIB_CB_TYPE.FILTER_QUADS);

         chessboardImage.FindCornerSubPix(
            new PointF[][] { corners },
            new Size(10, 10),
            new Size(-1, -1),
            new MCvTermCriteria(0.05));

         MCvPoint3D32f[] objectPts = CameraCalibration.CalcChessboardCorners(patternSize, 1.0f);
         IntrinsicCameraParameters intrisic = new IntrinsicCameraParameters();
         ExtrinsicCameraParameters[] extrinsic;
         double error = CameraCalibration.CalibrateCamera(new MCvPoint3D32f[][] { objectPts }, new PointF[][] { corners },
            chessboardImage.Size, intrisic, CvEnum.CALIB_TYPE.DEFAULT, out extrinsic);

         CameraCalibration.DrawChessboardCorners(chessboardImage, patternSize, corners);
         Image<Gray, Byte> undistorted = intrisic.Undistort(chessboardImage);
         //ImageViewer.Show(undistorted, String.Format("Reprojection error: {0}", error));
      }
   }
}
