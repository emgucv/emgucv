//----------------------------------------------------------------------------
//  Copyright (C) 2004-2026 by EMGU Corporation. All rights reserved.
//----------------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;
using Emgu.CV;
using Emgu.CV.Aruco;
using Emgu.CV.CvEnum;
using Emgu.CV.Features;
using Emgu.CV.Flann;
using Emgu.CV.Shape;
using Emgu.CV.Stitching;
using Emgu.CV.Text;
using Emgu.CV.Structure;
using Emgu.CV.Bioinspired;
using Emgu.CV.Dpm;
using Emgu.CV.ImgHash;
using Emgu.CV.Face;
using Emgu.CV.Freetype;
using Emgu.CV.StructuredLight;
using Emgu.CV.Dnn;
using Emgu.CV.Cuda;
using Emgu.CV.Mcc;
using Emgu.CV.Models;
using Emgu.CV.Tiff;
using Emgu.CV.Util;
using Emgu.CV.VideoStab;
using Emgu.CV.XImgproc;
using DistType = Emgu.CV.CvEnum.DistType;
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
    public class AutoTestImgproc
    {
        [Test]
        public void TestDenseHistogram()
        {
            Mat img = new Mat(400, 400, DepthType.Cv8U, 1);
            CvInvoke.Randu(img, new MCvScalar(0), new MCvScalar(255) );
            DenseHistogram hist = new DenseHistogram(256, new RangeF(0.0f, 255.0f));
            hist.Calculate(new Mat[] {img}, true, null);
            float[] binValues = hist.GetBinValues();
            /*
            using (MemoryStream ms = new MemoryStream())
            {
               System.Runtime.Serialization.Formatters.Binary.BinaryFormatter
               formatter = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
               formatter.Serialize(ms, hist);
               Byte[] bytes = ms.GetBuffer();

               using (MemoryStream ms2 = new MemoryStream(bytes))
               {
                  Object o = formatter.Deserialize(ms2);
                  DenseHistogram hist2 = (DenseHistogram)o;
                  EmguAssert.IsTrue(hist.Equals(hist2));
               }
            }*/
        }

        [Test]
        public void TestDenseHistogram2()
        {
            Mat img = new Mat(400, 400, DepthType.Cv8U, 3);
            CvInvoke.Randu(img, new MCvScalar(), new MCvScalar(255, 255, 255));
            Mat hist = new Mat();
            using (VectorOfMat vms = new VectorOfMat(img))
            {
                CvInvoke.CalcHist(vms, new int[] { 0, 1, 2 }, null, hist, new int[] { 20, 20, 20 },
                   new float[] { 0, 255, 0, 255, 0, 255 }, true);
                byte[] bytes = hist.GetRawData();
                hist.SetTo(bytes);

                float[] bins = new float[20 * 20 * 20];
                hist.CopyTo(bins);
            }
        }

        [Test]
        public void TestDenseHistogram3()
        {
            UMat img = new UMat(400, 400, DepthType.Cv8U, 3);
            CvInvoke.Randu(img, new MCvScalar(), new MCvScalar(255, 255, 255));
            UMat hist = new UMat();
            using (VectorOfUMat vms = new VectorOfUMat(img))
            {
                CvInvoke.CalcHist(vms, new int[] { 0, 1, 2 }, null, hist, new int[] { 20, 20, 20 },
                   new float[] { 0, 255, 0, 255, 0, 255 }, true);
                byte[] bytes = hist.Bytes;
                hist.SetTo(bytes);

                float[] bins = new float[20 * 20 * 20];
                hist.CopyTo(bins);
            }
        }

        [Test]
        public void TestDenseHistogram4()
        {
            Mat img = new Mat(400, 400, DepthType.Cv8U, 3);
            CvInvoke.Randu(img, new MCvScalar(), new MCvScalar(255, 255, 255));
            Mat hist = new Mat();
            using (VectorOfMat vms = new VectorOfMat(img))
            {
                CvInvoke.CalcHist(vms, new int[] { 0, 1, 2 }, null, hist, new int[] { 20, 20, 20 },
                   new float[] { 0, 255, 0, 255, 0, 255 }, true);
                byte[] bytes = hist.GetRawData();
            }
        }

        [Test]
        public void TestLineFitting1()
        {
            List<PointF> pts = new List<PointF>();

            pts.Add(new PointF(1.0f, 1.0f));
            pts.Add(new PointF(2.0f, 2.0f));
            pts.Add(new PointF(3.0f, 3.0f));
            pts.Add(new PointF(4.0f, 4.0f));

            PointF direction, pointOnLine;
            CvInvoke.FitLine(pts.ToArray(), out direction, out pointOnLine, DistType.L2, 0, 0.1, 0.1);

            //check if the line is 45 degree from +x axis
            EmguAssert.AreEqual(45.0, Math.Atan2(direction.Y, direction.X) * 180.0 / Math.PI);
        }

        [Test]
        public void TestContour()
        {
            using (Mat img = new Mat(100, 100, DepthType.Cv8U, 1))
            {
                img.SetTo(new MCvScalar());
                Rectangle rect = new Rectangle(10, 10, 80 - 10, 50 - 10);
                CvInvoke.Rectangle(img, rect, new MCvScalar(255), -1);
                PointF pIn = new PointF(60, 40);
                PointF pOut = new PointF(80, 100);

                using (VectorOfVectorOfPoint contours = new VectorOfVectorOfPoint())
                {
                    CvInvoke.FindContours(img, contours, null, RetrType.List, ChainApproxMethod.ChainApproxSimple);
                    using (VectorOfPoint firstContour = contours[0])
                    {
                        EmguAssert.IsTrue(CvInvoke.IsContourConvex(firstContour));
                    }
                }
            }
        }

        [Test]
        public void TestConvexityDefacts()
        {
            Mat image = new Mat(300, 300, DepthType.Cv8U, 3);
            Point[] polyline = new Point[] {
            new Point(10, 10),
            new Point(10, 250),
            new Point(100, 100),
            new Point(250, 250),
            new Point(250, 10)};
            using (VectorOfPoint vp = new VectorOfPoint(polyline))
            using (VectorOfVectorOfPoint contours = new VectorOfVectorOfPoint(vp))
            using (VectorOfInt convexHull = new VectorOfInt())
            using (Mat convexityDefect = new Mat())
            {
                //Draw the contour in white thick line
                CvInvoke.DrawContours(image, contours, -1, new MCvScalar(255, 255, 255), 3);
                CvInvoke.ConvexHull(vp, convexHull);
                CvInvoke.ConvexityDefects(vp, convexHull, convexityDefect);

                //convexity defect is a four channel mat, when k rows and 1 cols, where k = the number of convexity defects.
                if (!convexityDefect.IsEmpty)
                {
                    int[,] mData = convexityDefect.GetData(true) as int[,];
                    for (int i = 0; i < convexityDefect.Rows; i++)
                    {
                        int startIdx = mData[i, 0];
                        int endIdx = mData[i, 1];
                        Point startPoint = polyline[startIdx];
                        Point endPoint = polyline[endIdx];
                        //draw  a line connecting the convexity defect start point and end point in thin red line
                        CvInvoke.Line(image, startPoint, endPoint, new MCvScalar(0, 0, 255));
                    }
                }

                //Emgu.CV.WinForms.ImageViewer.Show(image);
            }
        }

        [Test]
        public void TestTriangle()
        {
            PointF p1 = new PointF(0, 0);
            PointF p2 = new PointF(1, 0);
            PointF p3 = new PointF(0, 1);
            Triangle2DF tri = new Triangle2DF(p1, p2, p3);
            double epsilon = 1e-12;
            EmguAssert.IsTrue(Math.Abs(tri.Area - 0.5) < epsilon);
        }

        [Test]
        public void TestLine()
        {
            PointF p1 = new PointF(0, 0);
            PointF p2 = new PointF(1, 0);
            PointF p3 = new PointF(0, 1);
            LineSegment2DF l1 = new LineSegment2DF(p1, p2);
            LineSegment2DF l2 = new LineSegment2DF(p1, p3);
            double angle = l1.GetExteriorAngleDegree(l2);
            EmguAssert.AreEqual(angle, 90.0);
        }

        [Test]
        public void TestGetBox2DPoints()
        {
            RotatedRect box = new RotatedRect(
               new PointF(3.0f, 2.0f),
               new SizeF(4.0f, 6.0f),
               0.0f);
            PointF[] vertices = box.GetVertices();
            //TODO: Find out why the following test fails. (x, y) convention changed.
            //EmguAssert.IsTrue(vertices[0].Equals(new PointF(0.0f, 0.0f)));
            //EmguAssert.IsTrue(vertices[1].Equals(new PointF(6.0f, 0.0f)));
        }

        [Test]
        public void TestMorphEx()
        {
            Mat kernel1 = CvInvoke.GetStructuringElement(Emgu.CV.CvEnum.MorphShapes.Cross, new Size(3, 3), new Point(1, 1));
            Mat kernel2 = new Mat(3, 3, DepthType.Cv8U, 1);
            kernel2.SetTo(new byte[]
            {
                0, 1, 0,
                1, 0, 1,
                0, 1, 0
            });
            Mat tmp = new Mat(100, 100, DepthType.Cv8U, 3);
            Mat tmp2 = new Mat();
            CvInvoke.MorphologyEx(tmp, tmp2, MorphOp.Gradient, kernel1, new Point(-1, -1), 1, CvEnum.BorderType.Default, new MCvScalar());
            Mat tmp3 = new Mat();
            CvInvoke.MorphologyEx(tmp, tmp3, MorphOp.Gradient, kernel2, new Point(-1, -1), 1, CvEnum.BorderType.Default, new MCvScalar());
        }

        /*
        public void TestPlanarSubdivisionHelper(int pointCount)
        {
#region generate random points
           PointF[] points = new PointF[pointCount];
           Random r = new Random((int) DateTime.Now.Ticks);
           for (int i = 0; i < points.Length; i++)
           {
              points[i] = new PointF((float) (r.NextDouble() * 20), (float) (r.NextDouble() * 20));
           }
#endregion

           Subdiv2D division;

           Stopwatch watch = Stopwatch.StartNew();
           division = new Subdiv2D(points, true);
           Triangle2DF[] triangles = division.GetDelaunayTriangles(false);
           watch.Stop();
           EmguAssert.WriteLine(String.Format("delaunay triangulation: {2} points, {0} milli-seconds, {1} triangles", watch.ElapsedMilliseconds, triangles.Length, points));
           watch.Reset();

           EmguAssert.IsTrue(CvInvoke.icvSubdiv2DCheck(division));

           watch.Start();
           division = new Subdiv2D(points);
           VoronoiFacet[] facets = division.GetVoronoiFacets();
           watch.Stop();
           EmguAssert.WriteLine(String.Format("Voronoi facets: {2} points, {0} milli-seconds, {1} facets", watch.ElapsedMilliseconds, facets.Length, points));
        }

        [Test]
        public void TestPlanarSubdivision1()
        {
           TestPlanarSubdivisionHelper(27);
           TestPlanarSubdivisionHelper(83);
           TestPlanarSubdivisionHelper(139);
           TestPlanarSubdivisionHelper(363);
           TestPlanarSubdivisionHelper(13);
           TestPlanarSubdivisionHelper(41);
           TestPlanarSubdivisionHelper(69);
        }*/

        [Test]
        public void TestPlanarSubdivision2()
        {
            PointF[] pts = new PointF[33];

            pts[0] = new PointF(224, 432);
            pts[1] = new PointF(368, 596);
            pts[2] = new PointF(316, 428);
            pts[3] = new PointF(244, 596);
            pts[4] = new PointF(224, 436);
            pts[5] = new PointF(224, 552);
            pts[6] = new PointF(276, 568);
            pts[7] = new PointF(308, 472);
            pts[8] = new PointF(316, 588);
            pts[9] = new PointF(368, 536);
            pts[10] = new PointF(332, 428);
            pts[11] = new PointF(124, 380);
            pts[12] = new PointF(180, 400);
            pts[13] = new PointF(148, 360);
            pts[14] = new PointF(148, 416);
            pts[15] = new PointF(128, 372);
            pts[16] = new PointF(124, 392);
            pts[17] = new PointF(136, 412);
            pts[18] = new PointF(156, 416);
            pts[19] = new PointF(176, 404);
            pts[20] = new PointF(180, 384);
            pts[21] = new PointF(168, 364);
            pts[22] = new PointF(260, 104);
            pts[23] = new PointF(428, 124);
            pts[24] = new PointF(328, 32);
            pts[25] = new PointF(320, 200);
            pts[26] = new PointF(268, 76);
            pts[27] = new PointF(264, 144);
            pts[28] = new PointF(316, 196);
            pts[29] = new PointF(384, 196);
            pts[30] = new PointF(424, 136);
            pts[31] = new PointF(412, 68);
            pts[32] = new PointF(348, 32);

            Subdiv2D subdiv = new Subdiv2D(pts);
            for (int i = 0; i < pts.Length; i++)
            {
                int edge;
                int point;
                CvEnum.Subdiv2DPointLocationType location = subdiv.Locate(pts[i], out edge, out point);
                if (location == Emgu.CV.CvEnum.Subdiv2DPointLocationType.OnEdge)
                {
                    continue;
                }
                subdiv.Insert(pts[i]);
            }

            VoronoiFacet[] facets = subdiv.GetVoronoiFacets();
        }

        #region Test code from Bug 36, thanks to Bart

        [Test]
        public void TestPlanarSubdivision3()
        {
            testCrashPSD(13, 5);
            testCrashPSD(41, 5);
            testCrashPSD(69, 5);
        }

        public static void testCrashPSD(int nPoints, int maxIter)
        {
            if (maxIter < 0)
            {
                // some ridiculously large number
                maxIter = 1000000000;
            }
            else if (maxIter == 0)
            {
                maxIter = 1;
            }

            int iter = 0;

            do
            {
                iter++;
                EmguAssert.WriteLine(String.Format("Running iteration {0} ... ", iter));

                PointF[] points = getPoints(nPoints);

                /*
                // write points to file
                TextWriter writer = new StreamWriter("points.txt");
                writer.WriteLine("// {0} generated points:", points.Length);
                writer.WriteLine("PointF[] points = new PointF[]");
                writer.WriteLine("{");
                foreach (PointF p in points)
                {
                   writer.WriteLine("\tnew PointF({0}f, {1}f),", p.X, p.Y);
                }
                writer.WriteLine("};");
                writer.Close();
                */
                Emgu.CV.Subdiv2D psd = new Emgu.CV.Subdiv2D(points);

                // hangs here:
                Emgu.CV.Structure.Triangle2DF[] triangles = psd.GetDelaunayTriangles();

                //System.Console.WriteLine("done.");
            }
            while (iter < maxIter);
        }

        public static PointF[] getPoints(int amount)
        {
            Random rand = new Random();
            PointF[] points = new PointF[amount];

            for (int i = 0; i < amount; i++)
            {
                // ensure unique points within [(0f,0f)..(800f,600f)]
                float x = (int)(rand.NextDouble() * 799) + (float)i / amount;
                float y = (int)(rand.NextDouble() * 599) + (float)i / amount;
                points[i] = new PointF(x, y);
            }

            return points;
        }
        #endregion

        [Test]
        public void TestMatchTemplate()
        {
            #region prepare synthetic image for testing
            int templWidth = 50;
            int templHeight = 50;
            Point templCenter = new Point(120, 100);

            //Create a random object
            Mat randomObj = new Mat(new Size(templWidth, templHeight), DepthType.Cv8U, 3);
            CvInvoke.Randu(randomObj, new MCvScalar(), new MCvScalar(255, 255, 255));

            //Draw the object in image1 center at templCenter;
            Mat img = new Mat(300, 200, DepthType.Cv8U, 3);
            Rectangle objectLocation = new Rectangle(templCenter.X - (templWidth >> 1), templCenter.Y - (templHeight >> 1), templWidth, templHeight);
            using (Mat tmp = new Mat(img, objectLocation))
            {
                randomObj.CopyTo(tmp);
            }
            #endregion

            Mat match = new Mat();
            CvInvoke.MatchTemplate(img, randomObj, match, TemplateMatchingType.Sqdiff);
            double[] minVal, maxVal;
            Point[] minLoc, maxLoc;
            match.MinMax(out minVal, out maxVal, out minLoc, out maxLoc);

            EmguAssert.AreEqual(minLoc[0].X, templCenter.X - templWidth / 2);
            EmguAssert.AreEqual(minLoc[0].Y, templCenter.Y - templHeight / 2);
        }

        [Test]
        public void TestSplit()
        {
            using (Mat m = new Mat(480, 640, DepthType.Cv8U, 3))
            {
                using (RNG rng = new RNG())
                    rng.Fill(m, RNG.DistType.Uniform, new MCvScalar(), new MCvScalar(255, 255, 255));

                using (VectorOfMat vm = new VectorOfMat())
                {
                    CvInvoke.Split(m, vm);
                }
            }
        }

        [Test]
        public void TestEllipseFitting()
        {
            #region generate random points
            System.Random r = new Random();
            int sampleCount = 100;
            Ellipse modelEllipse = new Ellipse(new PointF(200, 200), new SizeF(150, 60), 90);
            PointF[] pts = PointCollection.GeneratePointCloud(modelEllipse, sampleCount);
            #endregion

            Stopwatch watch = Stopwatch.StartNew();
            Ellipse fittedEllipse = PointCollection.EllipseLeastSquareFitting(pts);
            watch.Stop();

            #region draw the points and the fitted ellipse
            Mat img = new Mat(400, 400, DepthType.Cv8U, 3);
            img.SetTo(new MCvScalar(255, 255, 255));

            foreach (PointF p in pts)
                CvInvoke.Circle(img, Point.Round(p), 2, new MCvScalar(0, 255, 0), 1);

            CvInvoke.Ellipse(img, fittedEllipse.RotatedRect, new MCvScalar(0, 0, 255), 2);
            #endregion
            //Emgu.CV.WinForms.ImageViewer.Show(img, String.Format("Time used: {0} milliseconds", watch.ElapsedMilliseconds));
        }

        [Test]
        public void TestMinAreaRect()
        {
            #region generate random points
            System.Random r = new Random();
            int sampleCount = 100;
            Ellipse modelEllipse = new Ellipse(new PointF(200, 200), new SizeF(90, 60), -60);
            PointF[] pts = PointCollection.GeneratePointCloud(modelEllipse, sampleCount);
            #endregion

            Stopwatch watch = Stopwatch.StartNew();
            RotatedRect box = CvInvoke.MinAreaRect(pts);
            watch.Stop();

            #region draw the points and the box
            Mat img = new Mat(400, 400, DepthType.Cv8U, 3);
            img.SetTo(new MCvScalar(255, 255, 255));

            Point[] vertices = Array.ConvertAll(box.GetVertices(), Point.Round);

            CvInvoke.Polylines(img, vertices, true, new MCvScalar(0, 0, 255), 1);
            foreach (PointF p in pts)
                CvInvoke.Circle(img, Point.Round(p), 2, new MCvScalar(0, 255, 0), 1);
            #endregion

            //Emgu.CV.WinForms.ImageViewer.Show(img, String.Format("Time used: {0} milliseconds", watch.ElapsedMilliseconds));
        }

        [Test]
        public void TestMinEnclosingCircle()
        {
            #region generate random points
            System.Random r = new Random();
            int sampleCount = 100;
            Ellipse modelEllipse = new Ellipse(new PointF(200, 200), new SizeF(90, 60), -60);
            PointF[] pts = PointCollection.GeneratePointCloud(modelEllipse, sampleCount);
            #endregion

            Stopwatch watch = Stopwatch.StartNew();
            CircleF circle = CvInvoke.MinEnclosingCircle(pts);
            watch.Stop();

            #region draw the points and the circle
            Mat img = new Mat(400, 400, DepthType.Cv8U, 3);
            img.SetTo(new MCvScalar(255, 255, 255));
            foreach (PointF p in pts)
                CvInvoke.Circle(img, Point.Round(p), 2, new MCvScalar(0, 255, 0), 1);
            #endregion

            //Emgu.CV.WinForms.ImageViewer.Show(img, String.Format("Time used: {0} milliseconds", watch.ElapsedMilliseconds));
        }

        [Test]
        public void TestHistogram()
        {
            using (Mat img = EmguAssert.LoadMat("stuff.jpg", ImreadModes.ColorBgr))
            using (Mat img2 = new Mat())
            {
                CvInvoke.CvtColor(img, img2, ColorConversion.Bgr2Hsv);

                using (Mat h = new Mat())
                using (Mat bpj = new Mat())
                using (VectorOfMat vm = new VectorOfMat())
                {
                    CvInvoke.Split(img2, vm);
                    CvInvoke.CalcHist(vm, new int[] { 0 }, null, h, new int[] { 20 }, new float[] { 0, 180 }, false);
                    CvInvoke.CalcBackProject(vm, new int[] { 0 }, h, bpj, new float[] { 0, 180 }, 0.1);
                }

                /*
                foreach (Image<Gray, Byte> i in HSVs)
                    i.Dispose();
                */
            }
        }

        [Test]
        public void TestGrabCut1()
        {
            Mat img = EmguAssert.LoadMat("lena.jpg", ImreadModes.ColorBgr);

            Rectangle rect = new Rectangle(new Point(50, 50), new Size(400, 400));
            Mat bgdModel = new Mat();
            Mat fgdModel = new Mat();
            Mat mask = new Mat();

            CvInvoke.GrabCut(img, mask, rect, bgdModel, fgdModel, 0, Emgu.CV.CvEnum.GrabcutInitType.InitWithRect);
            CvInvoke.GrabCut(img, mask, rect, bgdModel, fgdModel, 2, Emgu.CV.CvEnum.GrabcutInitType.Eval);
            using (ScalarArray ia = new ScalarArray(3))
                CvInvoke.Compare(mask, ia, mask, CvEnum.CmpType.Equal);
            //Emgu.CV.WinForms.ImageViewer.Show(img.ConcateHorizontal( mask.Convert<Bgr, Byte>()));
        }

        [Test]
        public void TestGrabCut2()
        {
            Mat img = EmguAssert.LoadMat("pedestrian.png", ImreadModes.ColorBgr);
            HOGDescriptor desc = new HOGDescriptor();
            desc.SetSVMDetector(HOGDescriptor.GetDefaultPeopleDetector());

            MCvObjectDetection[] humanRegions = desc.DetectMultiScale(img);

            Mat pedestrianMask = new Mat(img.Size, DepthType.Cv8U, 1);
            pedestrianMask.SetTo(new MCvScalar());
            foreach (MCvObjectDetection rect in humanRegions)
            {
                //generate the mask where 3 indicates foreground and 2 indicates background
                using (Mat mask = new Mat()) //img.GrabCut(rect.Rect, 2))
                using (Mat bgdModel = new Mat())
                using(Mat fgdModel = new Mat())
                {
                    CvInvoke.GrabCut(img, mask, rect.Rect, bgdModel, fgdModel, 2, GrabcutInitType.InitWithRect);
                    //get the mask of the foreground
                    using (ScalarArray ia = new ScalarArray(3))
                        CvInvoke.Compare(mask, ia, mask, Emgu.CV.CvEnum.CmpType.Equal);

                    CvInvoke.BitwiseOr(pedestrianMask, mask, pedestrianMask);
                }
            }
        }

        [Test]
        public void TestGraySingleImage()
        {
            Mat img = new Mat(320, 480, DepthType.Cv32F, 1);
            CvInvoke.Randu(img, new MCvScalar(), new MCvScalar(255));
            Mat mask = new Mat();
            CvInvoke.Compare(img, new ScalarArray(100), mask, CmpType.GreaterEqual);
            int count = CvInvoke.CountNonZero(mask);
        }

        [Test]
        public void TestDiatanceTransform()
        {
            Mat img = new Mat(480, 320, DepthType.Cv8U, 1);
            img.SetTo(new MCvScalar());
            CvInvoke.Rectangle(img, new Rectangle(200, 100, 160, 90), new MCvScalar(255), 1);
            CvInvoke.BitwiseNot(img, img);
            Mat dst = new Mat();

            CvInvoke.DistanceTransform(img, dst, null, DistType.L2, 3);
        }

        [Test]
        public void TestLineIterator()
        {
            Mat img = EmguAssert.LoadMat("pedestrian.png");


            Mat line = LineIterator.SampleLine(img, new Point(0, 0), new Point(img.Width, img.Height));
            byte[,,] pixelData = line.GetData(true) as byte[,,];

            LineIterator li = new LineIterator(img, new Point(0, 0), new Point(img.Width, img.Height), 8, false);
            int count = li.Count;
            List<Point> points = new List<Point>();
            List<byte[]> sample = new List<byte[]>();
            for (int i = 0; i < count; i++)
            {
                points.Add(li.Pos);
                byte[] data = li.Data as byte[];
                sample.Add(data);

                //invert the pixel
                for (int j = 0; j < data.Length; j++)
                {
                    data[j] = (byte)(255 - data[j]);
                }

                li.Data = data;
                li.MoveNext();
            }

            //CvInvoke.Imshow("hello", img);
            //CvInvoke.WaitKey();
        }

        [Test]
        public void TestDrawMarker()
        {
            Mat m = new Mat(new Size(640, 480), DepthType.Cv8U, 3);
            m.SetTo(new MCvScalar(0, 0, 0));
            CvInvoke.DrawMarker(m, new Point(200, 200), new MCvScalar(255.0, 255.0, 0), MarkerTypes.Diamond);
            //Emgu.CV.WinForms.ImageViewer.Show(m);
        }

        [Test]
        public void TestHoughLine()
        {
            Mat img = EmguAssert.LoadMat("box.png");

            using (Mat imgGray = new Mat())
            using (VectorOfPointF vp = new VectorOfPointF())
            {
                if (img.NumberOfChannels == 1)
                    img.CopyTo(imgGray);
                else
                    CvInvoke.CvtColor(img, imgGray, ColorConversion.Bgr2Gray);
                CvInvoke.HoughLines(imgGray, vp, 10, Math.PI / 30, 5);
                PointF[] pts = vp.ToArray();
            }
        }

        [Test]
        public void TestHoughLinesPBlankImage()
        {
            using (Mat blank = new Mat(100, 100, DepthType.Cv8U, 1))
            {
                blank.SetTo(new MCvScalar(0));
                LineSegment2D[] lines = CvInvoke.HoughLinesP(blank, 1, Math.PI / 180, 10);
                EmguAssert.IsTrue(0 == lines.Length);
            }
        }

        [Test]
        public void TestHoughLinesBlankImage()
        {
            using (Mat blank = new Mat(100, 100, DepthType.Cv8U, 1))
            using (VectorOfPointF lines = new VectorOfPointF())
            {
                blank.SetTo(new MCvScalar(0));
                CvInvoke.HoughLines(blank, lines, 1, Math.PI / 180, 10);
                EmguAssert.IsTrue(0 == lines.Size);
            }
        }

        [Test]
        public void TestHoughCirclesBlankImage()
        {
            using (Mat blank = new Mat(100, 100, DepthType.Cv8U, 1))
            {
                blank.SetTo(new MCvScalar(0));
                CircleF[] circles = CvInvoke.HoughCircles(blank, CvEnum.HoughModes.Gradient, 1, 10);
                EmguAssert.IsTrue(0 == circles.Length);
            }
        }

        [Test]
        public void TestFastLineDetector()
        {
            using (Mat img = new Mat("box.png", ImreadModes.Grayscale))
            using (Mat drawImg = new Mat())
            using (FastLineDetector fld = new FastLineDetector())
            {
                LineSegment2DF[] lineSegments = fld.Detect(img);
                CvInvoke.CvtColor(img, drawImg, ColorConversion.Gray2Bgr);
                fld.DrawSegments(drawImg, lineSegments);
                EmguAssert.IsTrue(lineSegments.Length > 0);
            }
        }

        [Test]
        public void TestConvecityDefect()
        {
            Mat frame = EmguAssert.LoadMat("lena.jpg", ImreadModes.ColorBgr);
            using (VectorOfVectorOfPoint contours = new VectorOfVectorOfPoint())
            using (Mat canny = new Mat())
            {
                CvInvoke.CvtColor(frame, canny, ColorConversion.Bgr2Gray);
                IOutputArray hierarchy = null;
                CvInvoke.FindContours(canny, contours, hierarchy, RetrType.List, ChainApproxMethod.ChainApproxSimple);

                for (int i = 0; i < contours.Size; i++)
                {
                    CvInvoke.ApproxPolyDP(contours[i], contours[i], 5, false);
                    using (VectorOfInt hull = new VectorOfInt())
                    using (Mat defects = new Mat())
                    using (VectorOfPoint c = contours[i])
                    {
                        CvInvoke.ConvexHull(c, hull, false, false);
                        CvInvoke.ConvexityDefects(c, hull, defects);
                        if (!defects.IsEmpty)
                        {
                            int[,] valueData = defects.GetData(true) as int[,];
                            {
                                for (int j = 0; j < defects.Rows; j++)
                                {
                                    int startIdx = valueData[j, 0];
                                    int endIdx = valueData[j, 1];
                                    int farthestPtIdx = valueData[j, 2];
                                    double fixPtDepth = valueData[j, 3] / 256.0;

                                }
                            }
                        }
                    }
                }
            }
        }

        [Test]
        public void TestVectorOfString()
        {
            String[] s = new String[100];
            for (int i = 0; i < s.Length; i++)
            {
                s[i] = i.ToString();
            }

            using (VectorOfCvString vs = new VectorOfCvString(s))
            {
                String[] sClone = vs.ToArray();
                for (int i = 0; i < sClone.Length; i++)
                {
                    EmguAssert.IsTrue(i.ToString().Equals(sClone[i]));
                }
            }
        }

        [Test]
        public void TestGetTextSize()
        {
            int baseline = 0;
            Size s = CvInvoke.GetTextSize("Hello world", HersheyFonts.Plain, 16, 1, ref baseline);
        }

        [Test]
        public void TestFontFace()
        {
            using (FontFace font = new FontFace())
            {
                String name = font.Name;
                EmguAssert.IsTrue(!String.IsNullOrEmpty(name));

                Rectangle rect = CvInvoke.GetTextSize(new Size(640, 480), "Hello world", new Point(100, 100), font, 32);
                EmguAssert.IsTrue(rect.Width > 0);
                EmguAssert.IsTrue(rect.Height > 0);

                using (Mat m = new Mat(new Size(640, 480), DepthType.Cv8U, 3))
                {
                    m.SetTo(new MCvScalar(0, 0, 0, 0));
                    Point continuation = CvInvoke.PutText(m, "Hello world", new Point(100, 100), new MCvScalar(255, 255, 0), font, 32);
                    EmguAssert.IsTrue(continuation.X > 100);
                    EmguAssert.IsTrue(CvInvoke.CountNonZero(m.Reshape(1)) > 0);
                }

                EmguAssert.IsTrue(font.Set("italic"));
            }
        }

        [Test]
        public void TestNiBlack()
        {
            Mat m = EmguAssert.LoadMat("lena.jpg");
            Mat gray = new Mat();
            CvInvoke.CvtColor(m, gray, ColorConversion.Bgr2Gray);
            Mat result = new Mat();
            XImgproc.XImgprocInvoke.NiBlackThreshold(gray, result, 120, XImgproc.LocalBinarizationMethods.Niblack, 7, 0.5);
        }

        [Test]
        public void TestSeamlessClone()
        {
            Stopwatch watch = Stopwatch.StartNew();
            Mat blend = TestSeamlessCloneHelper(3840);
            watch.Stop();
#if NETFX_CORE
            Debug.WriteLine("Seamless clone complete in {0} milliseconds", watch.ElapsedMilliseconds.ToString());
#else
            Trace.WriteLine("Seamless clone complete in {0} milliseconds", watch.ElapsedMilliseconds.ToString());
#endif
        }

        private static Mat TestSeamlessCloneHelper(int size)
        {
            // create a 3 channel dst image
            Mat source = new Mat("lena.jpg", ImreadModes.ColorBgr);
            Mat img1 = new Mat();
            CvInvoke.Resize(source, img1, new System.Drawing.Size(size, size));

            // create a smaller 3 channel image
            Mat img2 = new Mat();
            CvInvoke.Resize(source, img2, new System.Drawing.Size(size / 2, size / 2));

            //use a circular region as the clone mask
            Mat mask = new Mat(img2.Size, DepthType.Cv8U, 1);
            int radius = Math.Min(mask.Width, mask.Height) / 2;
            CvInvoke.Circle(mask, new System.Drawing.Point(mask.Height / 2, mask.Width / 2), radius, new MCvScalar(255), -1);

            Mat blend = new Mat(img1.Size, DepthType.Cv8U, 3);
            CvInvoke.SeamlessClone(img2, img1, mask, new System.Drawing.Point(img1.Height / 2, img1.Width / 2), blend, CloningMethod.Normal);
            return blend;
        }

        [Test]
        public void TestEMD()
        {
            using (Mat signature1 = new Mat(new Size(100, 1), DepthType.Cv32F, 1))
            using (Mat signature2 = new Mat(new Size(100, 1), DepthType.Cv32F, 1))
            {
                CvInvoke.Randu(signature1, new MCvScalar(), new MCvScalar(1.0));
                CvInvoke.Randu(signature2, new MCvScalar(), new MCvScalar(1.0));
                double dist = CvInvoke.EMD(signature1, signature2, DistType.L2);
            }
        }

        [Test]
        public void TestScanSegment()
        {
            using (Mat image = EmguAssert.LoadMat("lena.jpg"))
            using (ScanSegment ss = new ScanSegment(image.Width, image.Height, 1))
            using (Mat contours = new Mat())
            using (Mat contoursBgr = new Mat())
            {
                ss.Iterate(image);
                ss.GetLabelContourMask(contours);
                CvInvoke.CvtColor(contours, contoursBgr, ColorConversion.Gray2Bgr);
                CvInvoke.BitwiseOr(image, contoursBgr, image);
                //Emgu.CV.WinForms.ImageViewer.Show(image);
            }
        }

        [Test]
        public void TestPyrMeanshiftSegmentation()
        {
            Mat image = EmguAssert.LoadMat("pedestrian.png", ImreadModes.ColorBgr);
            Mat result = new Mat();
            CvInvoke.PyrMeanShiftFiltering(image, result, 10, 20, 1, new MCvTermCriteria(5, 1));
            Mat resultGray = new Mat();
            CvInvoke.CvtColor(result, resultGray, ColorConversion.Bgr2Gray);
            Mat hue = new Mat();
            CvInvoke.Canny(resultGray, hue, 30, 20);
        }
    }
}
