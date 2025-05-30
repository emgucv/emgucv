﻿//----------------------------------------------------------------------------
//  Copyright (C) 2004-2025 by EMGU Corporation. All rights reserved.       
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
using Emgu.CV.CvEnum;
using Emgu.CV.Features2D;
using Emgu.CV.Structure;
using Emgu.CV.Util;
using Emgu.Util;
#if VS_TEST
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TestAttribute = Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute;
using TestFixture = Microsoft.VisualStudio.TestTools.UnitTesting.TestClassAttribute;
#elif NETFX_CORE
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
using TestAttribute = Microsoft.VisualStudio.TestPlatform.UnitTestFramework.TestMethodAttribute;
using TestFixture = Microsoft.VisualStudio.TestPlatform.UnitTestFramework.TestClassAttribute;
#else
using NUnit.Framework;
#endif

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
        public void TestChessboardCalibrationSolvePnPRansac()
        {
            Size patternSize = new Size(9, 6);

            Mat chessboardImage = EmguAssert.LoadMat("left01.jpg", ImreadModes.Grayscale);
            Util.VectorOfPointF corners = new Util.VectorOfPointF();
            bool patternWasFound = CvInvoke.FindChessboardCorners(chessboardImage, patternSize, corners);
            CvInvoke.CornerSubPix(
                chessboardImage,
                corners,
                new Size(10, 10),
                new Size(-1, -1),
                new MCvTermCriteria(0.05));

            MCvPoint3D32f[] objectPts = CalcChessboardCorners(patternSize, 1.0f);

            using (VectorOfVectorOfPoint3D32F ptsVec = new VectorOfVectorOfPoint3D32F(new MCvPoint3D32f[][] { objectPts }))
            using (VectorOfVectorOfPointF imgPtsVec = new VectorOfVectorOfPointF(corners))
            using (Mat cameraMatrix = new Mat())
            using (Mat distortionCoeff = new Mat())
            using (VectorOfMat rotations = new VectorOfMat())
            using (VectorOfMat translations = new VectorOfMat())
            {
                Mat calMat = CvInvoke.InitCameraMatrix2D(ptsVec, imgPtsVec, chessboardImage.Size, 0);
                Matrix<double> calMatF = new Matrix<double>(calMat.Rows, calMat.Cols, calMat.NumberOfChannels);
                calMat.CopyTo(calMatF);
                double error = CvInvoke.CalibrateCamera(ptsVec, imgPtsVec, chessboardImage.Size, cameraMatrix,
                    distortionCoeff,
                    rotations, translations, CalibType.Default, new MCvTermCriteria(30, 1.0e-10));
                using (Mat rotation = new Mat())
                using (Mat translation = new Mat())
                using (VectorOfPoint3D32F vpObject = new VectorOfPoint3D32F(objectPts))
                {
                    CvInvoke.SolvePnPRansac(
                        vpObject,
                        corners,
                        cameraMatrix,
                        distortionCoeff,
                        rotation,
                        translation);
                }

                CvInvoke.DrawChessboardCorners(chessboardImage, patternSize, corners, patternWasFound);
                using (Mat undistorted = new Mat())
                {
                    CvInvoke.Undistort(chessboardImage, undistorted, cameraMatrix, distortionCoeff);
                    String title = String.Format("Reprojection error: {0}", error);
                    //CvInvoke.NamedWindow(title);
                    //CvInvoke.Imshow(title, undistorted);
                    //CvInvoke.WaitKey();
                    //UI.ImageViewer.Show(undistorted, String.Format("Reprojection error: {0}", error));
                }
            }
        }

        [Test]
        public void TestChessboardCalibrationSolvePnP()
        {
            Size patternSize = new Size(9, 6);

            Mat chessboardImage = EmguAssert.LoadMat("left01.jpg", ImreadModes.Grayscale);
            Util.VectorOfPointF corners = new Util.VectorOfPointF();
            bool patternWasFound = CvInvoke.FindChessboardCorners(chessboardImage, patternSize, corners);
            CvInvoke.CornerSubPix(
                chessboardImage,
                corners,
                new Size(10, 10),
                new Size(-1, -1),
                new MCvTermCriteria(0.05));

            MCvPoint3D32f[] objectPts = CalcChessboardCorners(patternSize, 1.0f);

            using (VectorOfVectorOfPoint3D32F ptsVec = new VectorOfVectorOfPoint3D32F(new MCvPoint3D32f[][] { objectPts }))
            using (VectorOfVectorOfPointF imgPtsVec = new VectorOfVectorOfPointF(corners))
            using (Mat cameraMatrix = new Mat())
            using (Mat distortionCoeff = new Mat())
            using (VectorOfMat rotations = new VectorOfMat())
            using (VectorOfMat translations = new VectorOfMat())
            {
                Mat calMat = CvInvoke.InitCameraMatrix2D(ptsVec, imgPtsVec, chessboardImage.Size, 0);
                Matrix<double> calMatF = new Matrix<double>(calMat.Rows, calMat.Cols, calMat.NumberOfChannels);
                calMat.CopyTo(calMatF);
                double error = CvInvoke.CalibrateCamera(ptsVec, imgPtsVec, chessboardImage.Size, cameraMatrix,
                    distortionCoeff,
                    rotations, translations, CalibType.Default, new MCvTermCriteria(30, 1.0e-10));
                using (Mat rotation = new Mat())
                using (Mat translation = new Mat())
                using (VectorOfPoint3D32F vpObject = new VectorOfPoint3D32F(objectPts))
                {
                    CvInvoke.SolvePnP(
                        vpObject,
                        corners,
                        cameraMatrix,
                        distortionCoeff,
                        rotation,
                        translation);
                }

                CvInvoke.DrawChessboardCorners(chessboardImage, patternSize, corners, patternWasFound);
                using (Mat undistorted = new Mat())
                {
                    CvInvoke.Undistort(chessboardImage, undistorted, cameraMatrix, distortionCoeff);
                    String title = String.Format("Reprojection error: {0}", error);
                    //CvInvoke.NamedWindow(title);
                    //CvInvoke.Imshow(title, undistorted);
                    //CvInvoke.WaitKey();
                    //UI.ImageViewer.Show(undistorted, String.Format("Reprojection error: {0}", error));
                }
            }
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
