//----------------------------------------------------------------------------
//  Copyright (C) 2004-2013 by EMGU. All rights reserved.       
//----------------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;
using System.Text;
using System.Threading;
using System.Xml;
using Emgu.CV;
using Emgu.CV.Features2D;
using Emgu.CV.Structure;
using Emgu.Util;
using NUnit.Framework;

namespace Emgu.CV.Test
{
   [TestFixture]
   public class AutoTestCalibration
   {
      [Test]
      public void TestCalibration()
      {
         Size patternSize = new Size(9, 6);
         Image<Gray, Byte> left01 = EmguAssert.LoadImage<Gray, byte>("left01.jpg");
         using (Util.VectorOfPointF vec = new Util.VectorOfPointF())
         {
            CvInvoke.FindChessboardCorners(left01, patternSize, vec);
            PointF[] corners = vec.ToArray();
         } 
      }

      public static MCvPoint3D32f[] CalcChessboardCorners(Size boardSize, float squareSize)
      {
         List<MCvPoint3D32f> points = new List<MCvPoint3D32f>();
         for (int i = 0; i < boardSize.Height; ++i)
            for (int j = 0; j < boardSize.Width; ++j)
               points.Add(new MCvPoint3D32f(j * squareSize, i * squareSize, 0.0f));
         return points.ToArray();
      }

      [Test]
      public void TestChessboardCalibration()
      {
         Size patternSize = new Size(9, 6);

         Image<Gray, Byte> chessboardImage = EmguAssert.LoadImage<Gray, byte>("left01.jpg");
         Util.VectorOfPointF corners = new Util.VectorOfPointF();
         bool patternWasFound = CvInvoke.FindChessboardCorners(chessboardImage, patternSize, corners);

         chessboardImage.FindCornerSubPix(
            new PointF[][] {corners.ToArray()},
            new Size(10, 10),
            new Size(-1, -1),
            new MCvTermCriteria(0.05));

         MCvPoint3D32f[] objectPts = CalcChessboardCorners(patternSize, 1.0f);
         IntrinsicCameraParameters intrisic = new IntrinsicCameraParameters(8);
         ExtrinsicCameraParameters[] extrinsic;
         double error = CameraCalibration.CalibrateCamera(new MCvPoint3D32f[][] { objectPts }, new PointF[][] { corners.ToArray() },
            chessboardImage.Size, intrisic, CvEnum.CalibType.Default, new MCvTermCriteria(30, 1.0e-10),  out extrinsic);
         CvInvoke.DrawChessboardCorners(chessboardImage, patternSize, corners, patternWasFound);
         //CameraCalibration.DrawChessboardCorners(chessboardImage, patternSize, corners);
         Image<Gray, Byte> undistorted = intrisic.Undistort(chessboardImage);
         //UI.ImageViewer.Show(undistorted, String.Format("Reprojection error: {0}", error));
      }

      [Test]
      public void TestCirclesGrid()
      {
         Size patternSize = new Size(4, 3);
         Image<Gray, Byte> circlesGridImage = EmguAssert.LoadImage<Gray, byte>("circlesGrid.bmp");
         using (SimpleBlobDetector detector = new SimpleBlobDetector())
         using (Util.VectorOfPointF centers = new Util.VectorOfPointF())
         {
            bool found = CvInvoke.FindCirclesGrid(circlesGridImage, patternSize, centers, CvEnum.CalibCgType.SymmetricGrid | CvEnum.CalibCgType.Clustering, detector);
            CvInvoke.DrawChessboardCorners(circlesGridImage, patternSize, centers, found);
            //UI.ImageViewer.Show(circlesGridImage);
         }
      }
   }
}
