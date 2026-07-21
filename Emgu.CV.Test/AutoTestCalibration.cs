//----------------------------------------------------------------------------
//  Copyright (C) 2004-2026 by EMGU Corporation. All rights reserved.       
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
using Emgu.CV.Features;
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
            Mat left01 = EmguAssert.LoadMat("left01.jpg", ImreadModes.Grayscale);
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
                //Matrix<double> calMatF = new Matrix<double>(calMat.Rows, calMat.Cols, calMat.NumberOfChannels);
                //calMat.CopyTo(calMatF);
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
                //Matrix<double> calMatF = new Matrix<double>(calMat.Rows, calMat.Cols, calMat.NumberOfChannels);
                //calMat.CopyTo(calMatF);
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
            Mat circlesGridImage = EmguAssert.LoadMat("circlesGrid.bmp", ImreadModes.Grayscale);
            using (SimpleBlobDetector detector = new SimpleBlobDetector())
            using (Util.VectorOfPointF centers = new Util.VectorOfPointF())
            {
                bool found = CvInvoke.FindCirclesGrid(circlesGridImage, patternSize, centers, CvEnum.CalibCgType.SymmetricGrid | CvEnum.CalibCgType.Clustering, detector);
                CvInvoke.DrawChessboardCorners(circlesGridImage, patternSize, centers, found);
                //UI.ImageViewer.Show(circlesGridImage);
            }
        }

        private static void BuildSyntheticEssentialMatPoints(out float[] raw1, out float[] raw2)
        {
            // 5×5 grid of points at Z=5, camera 2 translated 1 unit along X
            const double f = 600.0, cx = 320.0, cy = 240.0, tx = 1.0, Z = 5.0;
            const int n = 25;
            raw1 = new float[n * 2];
            raw2 = new float[n * 2];
            int idx = 0;
            for (int r = -2; r <= 2; r++)
            {
                for (int c = -2; c <= 2; c++)
                {
                    raw1[idx * 2]     = (float)(f * c / Z + cx);
                    raw1[idx * 2 + 1] = (float)(f * r / Z + cy);
                    raw2[idx * 2]     = (float)(f * (c - tx) / Z + cx);
                    raw2[idx * 2 + 1] = (float)(f * r / Z + cy);
                    idx++;
                }
            }
        }

        [Test]
        public void TestRecoverPose()
        {
            const double f = 600.0, cx = 320.0, cy = 240.0;
            using Mat K = new Mat(3, 3, DepthType.Cv64F, 1);
            K.SetTo(new double[] { f, 0, cx, 0, f, cy, 0, 0, 1 });

            BuildSyntheticEssentialMatPoints(out float[] raw1, out float[] raw2);
            int n = raw1.Length / 2;
            using Mat p1 = new Mat(n, 1, DepthType.Cv32F, 2);
            using Mat p2 = new Mat(n, 1, DepthType.Cv32F, 2);
            p1.SetTo(raw1);
            p2.SetTo(raw2);

            using Mat mask = new Mat();
            using Mat E = CvInvoke.FindEssentialMat(p1, p2, K, CvEnum.FmType.Ransac, 0.999, 1.0, 1000, mask);
            EmguAssert.IsFalse(E.IsEmpty);

            using Mat R = new Mat();
            using Mat t = new Mat();
            int inliers = CvInvoke.RecoverPose(E, p1, p2, K, R, t, mask);

            EmguAssert.IsTrue(inliers > 0);
            EmguAssert.AreEqual(3, R.Rows);
            EmguAssert.AreEqual(3, R.Cols);
            EmguAssert.AreEqual(3, t.Rows);

            // Translation vector must be unit length
            double[] tArr = new double[3];
            t.CopyTo(tArr);
            double tNorm = Math.Sqrt(tArr[0] * tArr[0] + tArr[1] * tArr[1] + tArr[2] * tArr[2]);
            EmguAssert.IsTrue(Math.Abs(tNorm - 1.0) < 0.01);
        }

        [Test]
        public void TestRecoverPoseWithDistanceThresh()
        {
            const double f = 600.0, cx = 320.0, cy = 240.0;
            using Mat K = new Mat(3, 3, DepthType.Cv64F, 1);
            K.SetTo(new double[] { f, 0, cx, 0, f, cy, 0, 0, 1 });

            BuildSyntheticEssentialMatPoints(out float[] raw1, out float[] raw2);
            int n = raw1.Length / 2;
            using Mat p1 = new Mat(n, 1, DepthType.Cv32F, 2);
            using Mat p2 = new Mat(n, 1, DepthType.Cv32F, 2);
            p1.SetTo(raw1);
            p2.SetTo(raw2);

            using Mat mask = new Mat();
            using Mat E = CvInvoke.FindEssentialMat(p1, p2, K, CvEnum.FmType.Ransac, 0.999, 1.0, 1000, mask);

            using Mat R = new Mat();
            using Mat t = new Mat();
            using Mat triangulated = new Mat();
            int inliers = CvInvoke.RecoverPoseWithDistanceThresh(
                E, p1, p2, K, R, t, 100.0, mask, triangulated);

            EmguAssert.IsTrue(inliers > 0);
            EmguAssert.IsFalse(triangulated.IsEmpty);
        }

        [Test]
        public void TestProjectPoints()
        {
            Mat cameraMatrix = new Mat(3, 3, DepthType.Cv64F, 1);
            CvInvoke.SetIdentity(cameraMatrix, new MCvScalar(1));

            Mat distortionCoeff = new Mat(8, 1, DepthType.Cv64F, 1);
            distortionCoeff.SetTo(new MCvScalar());

            VectorOfDouble rotationVector = new VectorOfDouble(new double[] { 0, 0, 0 });
            VectorOfDouble translationVector = new VectorOfDouble(new double[] { 0, 0, 0 });

            MCvPoint3D32f point = new MCvPoint3D32f(12, 32, 9);

            PointF[] points = CvInvoke.ProjectPoints(
               new MCvPoint3D32f[] { point },
               rotationVector,
               translationVector,
               cameraMatrix,
               distortionCoeff);
        }

        [Test]
        public void TestSolvePnPRansacUsac()
        {
            //3D points on a grid at two depths
            MCvPoint3D32f[] objectPts = new MCvPoint3D32f[20];
            int idx = 0;
            for (int i = 0; i < 5; i++)
                for (int j = 0; j < 4; j++)
                    objectPts[idx++] = new MCvPoint3D32f(i * 0.1f, j * 0.1f, (i + j) % 2 == 0 ? 0.0f : 0.05f);

            using (Mat cameraMatrix = new Mat(3, 3, DepthType.Cv64F, 1))
            using (VectorOfPoint3D32F vp3 = new VectorOfPoint3D32F(objectPts))
            using (Mat rvecTruth = new Mat(3, 1, DepthType.Cv64F, 1))
            using (Mat tvecTruth = new Mat(3, 1, DepthType.Cv64F, 1))
            using (VectorOfPointF imagePts = new VectorOfPointF())
            {
                double[] k = { 800, 0, 320, 0, 800, 240, 0, 0, 1 };
                cameraMatrix.SetTo(k);
                rvecTruth.SetTo(new double[] { 0.1, -0.2, 0.05 });
                tvecTruth.SetTo(new double[] { 0.05, -0.1, 1.5 });

                CvInvoke.ProjectPoints(vp3, rvecTruth, tvecTruth, cameraMatrix, null, imagePts);

                UsacParams usacParams = UsacParams.GetDefault();
                EmguAssert.IsTrue(usacParams.MaxIterations > 0, "default UsacParams should be populated");

                using (Mat rvec = new Mat())
                using (Mat tvec = new Mat())
                using (VectorOfInt inliers = new VectorOfInt())
                {
                    bool success = CvInvoke.SolvePnPRansac(
                        vp3, imagePts, cameraMatrix, null, rvec, tvec, inliers, ref usacParams);
                    EmguAssert.IsTrue(success, "SolvePnPRansac (USAC) failed");
                    EmguAssert.IsTrue(inliers.Size >= objectPts.Length - 2, "most points should be inliers");

                    double[] rv = (double[])rvec.GetData(multiDimensional: false);
                    double[] tv = (double[])tvec.GetData(multiDimensional: false);
                    EmguAssert.IsTrue(Math.Abs(rv[0] - 0.1) < 1e-3 && Math.Abs(rv[1] + 0.2) < 1e-3 && Math.Abs(rv[2] - 0.05) < 1e-3, "rvec mismatch");
                    EmguAssert.IsTrue(Math.Abs(tv[0] - 0.05) < 1e-3 && Math.Abs(tv[1] + 0.1) < 1e-3 && Math.Abs(tv[2] - 1.5) < 1e-3, "tvec mismatch");
                }
            }
        }
    }
}
