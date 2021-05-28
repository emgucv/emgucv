//----------------------------------------------------------------------------
//  Copyright (C) 2004-2021 by EMGU Corporation. All rights reserved.       
//----------------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;  
using System.Runtime.Serialization;

using System.Text;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;
using Emgu.CV;
using Emgu.CV.Aruco;
using Emgu.CV.CvEnum;
using Emgu.CV.Features2D;
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

using Emgu.CV.Dnn;
using Emgu.CV.Cuda;
using Emgu.CV.DepthAI;
using Emgu.CV.Mcc;
using Emgu.CV.Tiff;
//using Emgu.CV.UI;
using Emgu.CV.Util;
using Emgu.CV.VideoStab;
using Emgu.CV.XFeatures2D;
using Emgu.CV.XImgproc;
using Emgu.Util;
//using Newtonsoft.Json;
using DetectorParameters = Emgu.CV.Aruco.DetectorParameters;
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
using Emgu.CV.ML;
using NUnit.Framework;
#endif


namespace Emgu.CV.Test
{
    [TestFixture]
    public class AutoTestVarious
    {
        [Test]
        public void TestColorEqual()
        {
            Bgr c1 = new Bgr(0.0, 0.0, 0.0);
            Bgr c2 = new Bgr(0.0, 0.0, 0.0);
            EmguAssert.IsTrue(c1.Equals(c2));
        }

        [Test]
        public void TestCvClipLine()
        {
            Point m1 = new Point(-1, 10);
            Point m2 = new Point(100, 10);
            bool inside = CvInvoke.ClipLine(new Rectangle(Point.Empty, new Size(20, 20)), ref m1, ref m2);
            EmguAssert.AreEqual(0, m1.X);
            EmguAssert.AreEqual(19, m2.X);
        }

        [Test]
        public void TestRectangleSize()
        {
            EmguAssert.AreEqual(4 * sizeof(int), Marshal.SizeOf(typeof(Rectangle)));
        }


        [Test]
        public void TestDenseHistogram()
        {
            Image<Gray, Byte> img = new Image<Gray, byte>(400, 400);
            img.SetRandUniform(new MCvScalar(), new MCvScalar(255));
            DenseHistogram hist = new DenseHistogram(256, new RangeF(0.0f, 255.0f));
            hist.Calculate<Byte>(new Image<Gray, byte>[] { img }, true, null);
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
        public void TestObjectnessBING()
        {
            using (Emgu.CV.Saliency.ObjectnessBING objectnessBING = new Saliency.ObjectnessBING())
            {
                //objectnessBING.SetTrainingPath("C:\\tmp");
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
        public void TestLookup()
        {
            double[] b = new double[4] { 0, 1, 2, 3 };
            double[] a = new double[4] { 1, 3, 2, 0 };
            MCvPoint2D64f[] pts = new MCvPoint2D64f[b.Length];
            for (int i = 0; i < pts.Length; i++)
                pts[i] = new MCvPoint2D64f(b[i], a[i]);

            IEnumerable<MCvPoint2D64f> interPts = Toolbox.LinearInterpolate(pts, new double[2]
            {
            1.5,
            3.5
            });
            IEnumerator<MCvPoint2D64f> enumerator = interPts.GetEnumerator();
            enumerator.MoveNext();
            EmguAssert.IsTrue(1.5 == enumerator.Current.X);
            EmguAssert.IsTrue(2.5 == enumerator.Current.Y);
            enumerator.MoveNext();
            EmguAssert.IsTrue(3.5 == enumerator.Current.X);
            EmguAssert.IsTrue(-1 == enumerator.Current.Y);
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
            CvInvoke.FitLine(pts.ToArray(), out direction, out pointOnLine, CvEnum.DistType.L2, 0, 0.1, 0.1);

            //check if the line is 45 degree from +x axis
            EmguAssert.AreEqual(45.0, Math.Atan2(direction.Y, direction.X) * 180.0 / Math.PI);
        }

        [Test]
        public void TestBuildInformation()
        {
            String bi = CvInvoke.BuildInformation;
        }

#if !WINDOWS_PHONE_APP
        [Test]
        public void TestXmlSerialization()
        {
            MCvPoint2D64f pt2d = new MCvPoint2D64f(12.0, 5.5);

            XDocument xdoc = Toolbox.XmlSerialize<MCvPoint2D64f>(pt2d);
            //Trace.WriteLine(xdoc.OuterXml);
            pt2d = Toolbox.XmlDeserialize<MCvPoint2D64f>(xdoc);

            CircleF cir = new CircleF(new PointF(0.0f, 1.0f), 2.8f);
            xdoc = Toolbox.XmlSerialize<CircleF>(cir);
            //Trace.WriteLine(xdoc.OuterXml);
            cir = Toolbox.XmlDeserialize<CircleF>(xdoc);

            Image<Bgr, Byte> img1 = EmguAssert.LoadImage<Bgr, byte>("stuff.jpg");
            xdoc = Toolbox.XmlSerialize(img1);
            //Trace.WriteLine(xdoc.OuterXml);
            Image<Bgr, Byte> img2 = Toolbox.XmlDeserialize<Image<Bgr, Byte>>(xdoc);

            Byte[] a1 = img1.Bytes;
            Byte[] a2 = img2.Bytes;
            EmguAssert.AreEqual(a1.Length, a2.Length);
            for (int i = 0; i < a1.Length; i++)
            {
                EmguAssert.AreEqual(a1[i], a2[i]);
            }

            img1.Dispose();
            img2.Dispose();
        }
#endif

        [Test]
        public void TestRotationMatrix3D()
        {
            double[] rod = new double[] { 0.2, 0.5, 0.3 };
            RotationVector3D rodVec = new RotationVector3D(rod);

            RotationVector3D rodVec2 = new RotationVector3D();
            rodVec2.RotationMatrix = rodVec.RotationMatrix;
            Matrix<double> diff = rodVec - rodVec2;
            EmguAssert.IsTrue(diff.Norm < 1.0e-8);
        }

#if !(NETFX_CORE || __ANDROID__ || __IOS__ || UNITY_IOS || UNITY_ANDROID)
        [Test]
        public void TestViz()
        {
            Viz3d viz = new Viz3d("show_simple_widgets");
            viz.SetBackgroundMeshLab();
            WCoordinateSystem coor = new WCoordinateSystem();
            viz.ShowWidget("coor", coor);
            WCube cube = new WCube(new MCvPoint3D64f(-.5, -.5, -.5), new MCvPoint3D64f(.5, .5, .5), true, new MCvScalar(255, 255, 255));
            viz.ShowWidget("cube", cube);
            WCube cube0 = new WCube(new MCvPoint3D64f(-1, -1, -1), new MCvPoint3D64f(-.5, -.5, -.5), false, new MCvScalar(123, 45, 200));
            viz.ShowWidget("cub0", cube0);
            //viz.Spin();
        }
#endif

        [Test]
        public void TestContour()
        {
            //Application.EnableVisualStyles();
            //Application.SetCompatibleTextRenderingDefault(false);
            using (Image<Gray, Byte> img = new Image<Gray, Byte>(100, 100, new Gray()))
            {
                Rectangle rect = new Rectangle(10, 10, 80 - 10, 50 - 10);
                img.Draw(rect, new Gray(255.0), -1);
                //ImageViewer.Show(img);
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
                /*
                using (MemStorage stor = new MemStorage())
                {
                   //Contour<Point> cs = img.FindContours(CvEnum.ChainApproxMethod.ChainApproxSimple, CvEnum.RetrType.List, stor);
                   //EmguAssert.IsTrue(cs.MCvContour.elem_size == Marshal.SizeOf(typeof(Point)));
                   //EmguAssert.IsTrue(rect.Width * rect.Height == cs.Area);

                   //EmguAssert.IsTrue(cs.Convex);
                   //EmguAssert.IsTrue(rect.Width * 2 + rect.Height * 2 == cs.Perimeter);
                   Rectangle rect2 = cs.BoundingRectangle;
                   rect2.Width -= 1;
                   rect2.Height -= 1;
                   //rect2.Center.X -= 0.5;
                   //rect2.Center.Y -= 0.5;
                   //EmguAssert.IsTrue(rect2.Equals(rect));
                   EmguAssert.IsTrue(cs.InContour(pIn) > 0);
                   EmguAssert.IsTrue(cs.InContour(pOut) < 0);
                   //EmguAssert.IsTrue(cs.Distance(pIn) == 10);
                   //EmguAssert.IsTrue(cs.Distance(pOut) == -50);
                   img.Draw(cs, new Gray(100), new Gray(100), 0, 1);

                   MCvPoint2D64f rectangleCenter = new MCvPoint2D64f(rect.X + rect.Width / 2.0, rect.Y + rect.Height / 2.0);

                   using (VectorOfPoint vp = new VectorOfPoint(cs.ToArray()))
                   {    
                      MCvMoments moment = CvInvoke.Moments(vp, false);
                      MCvPoint2D64f center = moment.GravityCenter;
                      //EmguAssert.IsTrue(center.Equals(rectangleCenter));
                   }

                }

                using (MemStorage stor = new MemStorage())
                {
                   Image<Gray, Byte> img2 = new Image<Gray, byte>(300, 200);
                   Contour<Point> c = img2.FindContours(Emgu.CV.CvEnum.ChainApproxMethod.ChainApproxSimple, Emgu.CV.CvEnum.RetrType.List, stor);
                   EmguAssert.IsTrue(c == null);
                }*/
            }

            //int s1 = Marshal.SizeOf(typeof(MCvSeq));
            //int s2 = Marshal.SizeOf(typeof(MCvContour));
            //int sizeRect = Marshal.SizeOf(typeof(Rectangle));
            //EmguAssert.IsTrue(s1 + sizeRect + 4 * Marshal.SizeOf(typeof(int)) == s2);
        }

        [Test]
        public void TestConvexityDefacts()
        {
            Image<Bgr, Byte> image = new Image<Bgr, byte>(300, 300);
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
                    //Data from Mat are not directly readable so we convert it to Matrix<>
                    Matrix<int> m = new Matrix<int>(convexityDefect.Rows, convexityDefect.Cols,
                       convexityDefect.NumberOfChannels);
                    convexityDefect.CopyTo(m);

                    for (int i = 0; i < m.Rows; i++)
                    {
                        int startIdx = m.Data[i, 0];
                        int endIdx = m.Data[i, 1];
                        Point startPoint = polyline[startIdx];
                        Point endPoint = polyline[endIdx];
                        //draw  a line connecting the convexity defect start point and end point in thin red line
                        CvInvoke.Line(image, startPoint, endPoint, new MCvScalar(0, 0, 255));
                    }
                }

                //Emgu.CV.UI.ImageViewer.Show(image);
            }
        }

        [Test]
        public void TestLogLevel()
        {
            CvEnum.LogLevel level = CvInvoke.LogLevel;
            CvInvoke.LogLevel = CvEnum.LogLevel.Debug;
            level = CvInvoke.LogLevel;
        }

        [Test]
        public void TestException()
        {
            //Test seems to crash on Linux system. Skipping test on Linux for now.
            if (!System.Runtime.InteropServices.RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
            {
                for (int i = 0; i < 10; i++)
                {
                    bool exceptionCaught = false;
                    Matrix<Byte> mat = new Matrix<byte>(20, 30);
                    try
                    {
                        double det = mat.Det;
                    }
                    catch (CvException excpt)
                    {
                        EmguAssert.AreEqual(-215, excpt.Status);
                        exceptionCaught = true;
                    }

                    EmguAssert.IsTrue(exceptionCaught);
                }
            }
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
        public void TestShapeDistanceExtractor()
        {
            using (HistogramCostExtractor comparer = new ChiHistogramCostExtractor())
            using (ThinPlateSplineShapeTransformer transformer = new ThinPlateSplineShapeTransformer())
            using (ShapeContextDistanceExtractor extractor = new ShapeContextDistanceExtractor(comparer, transformer))
            using (HausdorffDistanceExtractor extractor2 = new HausdorffDistanceExtractor())
            {
                Point[] shape1 = new Point[] { new Point(0, 0), new Point(480, 0), new Point(480, 360), new Point(0, 360) };
                Point[] shape2 = new Point[] { new Point(0, 0), new Point(480, 0), new Point(500, 240), new Point(480, 360), new Point(0, 360) };

                float distance2 = extractor2.ComputeDistance(shape1, shape2);
                float distance = extractor.ComputeDistance(shape1, shape2);
            }
        }

        /*
        [Test]
        public void TestBlobDetector()
        {
           int width = 300;
           int height = 400;
           Image<Gray, Byte> img = new Image<Gray, byte>(width, height);
           //bg.SetRandNormal(new MCvScalar(), new MCvScalar(100, 100, 100));
           Size size = new Size(width / 5, height / 5);
           Point topLeft = new Point((width >> 1) - (size.Width >> 1), (height >> 1) - (size.Height >> 1));
           Rectangle rect = new Rectangle(topLeft, size);

           BlobDetector detector = new BlobDetector(CvEnum.BlobDetectorType.Simple);
           //Image<Gray, Byte> forground = new Image<Gray,byte>(bg.Size);
           //forground.SetValue(255);
           BlobSeq newSeq = new BlobSeq();
           BlobSeq oldSeq = new BlobSeq();
           using (Image<Gray, Byte> forgroundMask = img.Copy())
           {
              rect.Offset(5, 0); //shift the rectangle 5 pixels horizontally
              forgroundMask.Draw(rect, new Gray(255), -1);
              //TODO: Find out why BlobDetector cannot detect new Blob.
              bool detected = detector.DetectNewBlob(forgroundMask, newSeq, oldSeq);
              //ImageViewer.Show(forgroundMask);
              //Assert.IsTrue(detected);
           }
        }

        [Test]
        public void TestBlobColor()
        {
           int width = 300;
           int height = 400;
           Image<Bgr, Byte> bg = new Image<Bgr, byte>(width, height);
           bg.SetRandNormal(new MCvScalar(), new MCvScalar(100, 100, 100));

           Size size = new Size(width / 10, height / 10);
           Point topLeft = new Point((width >> 1) - (size.Width >> 1), (height >> 1) - (size.Height >> 1));

           Rectangle rect = new Rectangle(topLeft, size);

           BlobTrackerAutoParam<Bgr> param = new BlobTrackerAutoParam<Bgr>();
           param.BlobDetector = new BlobDetector(Emgu.CV.CvEnum.BlobDetectorType.CC);
           //param.FGDetector = new FGDetector<Gray>(Emgu.CV.CvEnum.FORGROUND_DETECTOR_TYPE.FGD);
           param.BlobTracker = new BlobTracker(Emgu.CV.CvEnum.BlobTrackerType.MSFGS);
           param.FGTrainFrames = 5;
           BlobTrackerAuto<Bgr> tracker = new BlobTrackerAuto<Bgr>(param);

           //ImageViewer viewer = new ImageViewer();
           //viewer.Show();
           for (int i = 0; i < 20; i++)
           {
              using (Image<Bgr, Byte> img1 = bg.Copy())
              {
                 rect.Offset(5, 0); //shift the rectangle 5 pixels horizontally
                 img1.Draw(rect, new Bgr(Color.Red), -1);
                 tracker.Process(img1);
                 //viewer.Image = img1;
              }
           }

           //MCvBlob blob = tracker[0];
           //int id = blob.ID;
           //ImageViewer.Show(forground);
        }

        [Test]
        public void TestBlobGray()
        {
           int width = 300;
           int height = 400;
           Image<Bgr, Byte> bg = new Image<Bgr, byte>(width, height);
           bg.SetRandNormal(new MCvScalar(), new MCvScalar(100, 100, 100));

           Size size = new Size(width / 10, height / 10);
           Point topLeft = new Point((width >> 1) - (size.Width >> 1), (height >> 1) - (size.Height >> 1));

           Rectangle rect = new Rectangle(topLeft, size);

           BlobTrackerAutoParam<Gray> param = new BlobTrackerAutoParam<Gray>();
           param.BlobDetector = new BlobDetector(Emgu.CV.CvEnum.BlobDetectorType.CC);
           //param.FGDetector = new FGDetector<Gray>(Emgu.CV.CvEnum.FORGROUND_DETECTOR_TYPE.FGD);
           param.BlobTracker = new BlobTracker(Emgu.CV.CvEnum.BlobTrackerType.MSFGS);
           param.FGTrainFrames = 5;
           BlobTrackerAuto<Gray> tracker = new BlobTrackerAuto<Gray>(param);

           //ImageViewer viewer = new ImageViewer();
           //viewer.Show();
           for (int i = 0; i < 20; i++)
           {
              using (Image<Bgr, Byte> img1 = bg.Copy())
              {
                 rect.Offset(5, 0); //shift the rectangle 5 pixels horizontally
                 img1.Draw(rect, new Bgr(Color.Red), -1);
                 tracker.Process(img1.Convert<Gray, Byte>());
                 //viewer.Image = img1;
                 //viewer.Refresh();
              }
           }

           //MCvBlob blob = tracker[0];
           //int id = blob.ID;
           //ImageViewer.Show(foreground);
        }

        [Test]
        public void TestEigenObjects()
        {
           String[] fileNames = new string[]
           {
              "stuff.jpg",
              "squares.gif",
              "lena.jpg"
           };

           int width = 100, height = 100;
           MCvTermCriteria termCrit = new MCvTermCriteria(3, 0.001);

#region using batch method
           Image<Gray, Byte>[] imgs = Array.ConvertAll<String, Image<Gray, Byte>>(fileNames,
               delegate(String file)
           {
              return EmguAssert.LoadImage<Gray, Byte>(file).Resize(width, height, CvEnum.Inter.Linear);
           });

           EigenObjectRecognizer imgRecognizer1 = new EigenObjectRecognizer(imgs, ref termCrit);
           for (int i = 0; i < imgs.Length; i++)
           {
              EmguAssert.AreEqual(i.ToString(), imgRecognizer1.Recognize(imgs[i]).Label);
           }

           XDocument xDoc = Toolbox.XmlSerialize<EigenObjectRecognizer>(imgRecognizer1);
           EigenObjectRecognizer imgRecognizer2 = Toolbox.XmlDeserialize<EigenObjectRecognizer>(xDoc);

           for (int i = 0; i < imgs.Length; i++)
           {
              EmguAssert.AreEqual(i.ToString(), imgRecognizer2.Recognize(imgs[i]).Label);
           }

           System.Runtime.Serialization.Formatters.Binary.BinaryFormatter
               formatter = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();

           Byte[] bytes;
           using (MemoryStream ms = new MemoryStream())
           {
              formatter.Serialize(ms, imgRecognizer1);
              bytes = ms.GetBuffer();
           }
           using (MemoryStream ms2 = new MemoryStream(bytes))
           {
              EigenObjectRecognizer imgRecognizer3 = (EigenObjectRecognizer) formatter.Deserialize(ms2);
              for (int i = 0; i < imgs.Length; i++)
              {
                 EmguAssert.AreEqual(i.ToString(), imgRecognizer3.Recognize(imgs[i]).Label);
              }
           }
#endregion
        }

        [Test]
        public void StressMemoryTestHaar()
        {
           Image<Bgr, Byte> img;
           for (int i = 0; i < 10000; i++)
           {
              //HaarCascade cascade = new HaarCascade("haarcascade_frontalface_alt2.xml");
              img = new Image<Bgr, Byte>("lena.jpg");
              //MCvAvgComp[] comps = img.DetectHaarCascade(cascade)[0];
           }
        }*/

        private static float[,] ProjectPoints(float[,] points3D, RotationVector3D rotation, Matrix<double> translation, float focalLength)
        {
            using (Matrix<float> imagePointMat = new Matrix<float>(points3D.GetLength(0), 2))
            {
                CvInvoke.ProjectPoints(new Matrix<float>(points3D), rotation, translation,
                new Matrix<double>(new double[,] {
               {focalLength, 0, 0},
               {0, focalLength, 0},
               {0,0,1}}),
                null, imagePointMat);
                return imagePointMat.Data;
            }
        }

        /*
        [Test]
        public void TestIntrisicParameters()
        {
#if !(__IOS__ || __ANDROID__ || NETFX_CORE)
            System.Diagnostics.PerformanceCounter memCounter = new PerformanceCounter("Memory", "Available MBytes");
            Trace.WriteLine(String.Format("Available mem before: {0} Mb", memCounter.NextValue()));
#endif
            IntrinsicCameraParameters[] paramArr = new IntrinsicCameraParameters[100000];
            for (int i = 0; i < paramArr.Length; i++)
            {
                paramArr[i] = new IntrinsicCameraParameters(8);
            }
#if !(__IOS__ || __ANDROID__ || NETFX_CORE)
            Trace.WriteLine(String.Format("Available mem after: {0} Mb", memCounter.NextValue()));
#endif
        }

        
        [Test]
        public void TestPOSIT()
        {
           float[] rotation = new float[9];
           float[] translation = new float[3];
           float focalLength = 1.0f;

           float cubeSize = 200.0f;

           float[,] points3D = new float[,] {
              {0, 0, 0},
              {0, 0, cubeSize},
              {0, cubeSize, cubeSize}, 
              {0, cubeSize, 0},
              {cubeSize, 0, 0}, 
              {cubeSize, cubeSize, 0},
              { cubeSize, cubeSize, cubeSize }, 
              { cubeSize, 0, cubeSize } };

           IntPtr posit = CvInvoke.cvCreatePOSITObject(points3D, points3D.GetLength(0));

#region caculate the image point assuming we know the rotation and translation
           RotationVector3D realRotVec = new RotationVector3D(new double[3] { 0.1f, 0.2f, 0.3f });
           Matrix<double> realTransVec = new Matrix<double>(new double[3] { 0.0f, 0.0f, -50.0f });

           float[,] imagePoint = ProjectPoints(points3D, realRotVec, realTransVec, focalLength);
           GCHandle handle1 = GCHandle.Alloc(imagePoint, GCHandleType.Pinned);
#endregion

           RotationVector3D rotVecGuess = new RotationVector3D(new double[3] { 0.3f, 0.1f, 0.2f });
           Matrix<double> rotMatGuess = rotVecGuess.RotationMatrix;

           //float[] rotFVecGuess = new float[] { (float)rotMatGuess[0, 0], (float) rotMatGuess[1, 0], (float) rotMatGuess[2, 0], (float)rotMatGuess[0, 1], (float)rotMatGuess[1, 1], (float)rotMatGuess[2, 1], (float)rotMatGuess[0, 2], (float)rotMatGuess[1, 2], (float)rotMatGuess[2, 2] };
           Matrix<float> tranFVecGuess = new Matrix<float>( new float[] { 0, 0, 5 });
           CvInvoke.cvPOSIT(posit, handle1.AddrOfPinnedObject(), 0.5, new MCvTermCriteria(200, 1.0e-5), rotMatGuess.MCvMat.data, tranFVecGuess.MCvMat.data);

           //Matrix<double> rotMatEst = new Matrix<double>(new double[,] { 
           //   {rotFVecGuess[0], rotFVecGuess[3], rotFVecGuess[6]},
           //   {rotFVecGuess[1], rotFVecGuess[4], rotFVecGuess[7]},
           //   {rotFVecGuess[2], rotFVecGuess[5], rotFVecGuess[8]}});
           RotationVector3D rotVecEst = new RotationVector3D();
           Matrix<double> tranMatEst = tranFVecGuess.Convert<double>();

           rotVecEst.RotationMatrix = rotMatGuess;
           //At this point rotVecEst should be similar to realRotVec, but it is not...

           float[,] projectionFromEst = ProjectPoints(points3D, rotVecEst, tranMatEst, focalLength);

           for (int i = 0; i < projectionFromEst.GetLength(0); i++)
           {
              float x = imagePoint[i, 0] - projectionFromEst[i, 0];
              float y = imagePoint[i, 1] - projectionFromEst[i, 1];
              Trace.WriteLine(String.Format("Projection Distance: {0}", Math.Sqrt(x * x + y * y)));
           }

           CvInvoke.cvReleasePOSITObject(ref posit);
        }*/


        [Test]
        public void TestXmlSerialize()
        {
            PointF p = new PointF(0.0f, 0.0f);
            XDocument xDoc = Toolbox.XmlSerialize<PointF>(p, new Type[] { typeof(Point) });
            PointF p2 = Toolbox.XmlDeserialize<PointF>(xDoc, new Type[] { typeof(Point) });
            EmguAssert.IsTrue(p.Equals(p2));


            Rectangle rect = new Rectangle(3, 4, 5, 3);
            XDocument xDoc2 = Toolbox.XmlSerialize<Rectangle>(rect);
            Rectangle rect2 = Toolbox.XmlDeserialize<Rectangle>(xDoc2);
            EmguAssert.IsTrue(rect.Equals(rect2));

        }

        [Test]
        public void TestFileStorage1()
        {
            FileStorage fs = new FileStorage("haarcascade_eye.xml", FileStorage.Mode.Read);

        }

        [Test]
        public void TestFileStorage2()
        {
            Mat m = new Mat(40, 30, DepthType.Cv8U, 3);

            using (ScalarArray lower = new ScalarArray(new MCvScalar(0, 0, 0)))
            using (ScalarArray higher = new ScalarArray(new MCvScalar(255, 255, 255)))
                CvInvoke.Randu(m, lower, higher);

            int intValue = 10;
            float floatValue = 213.993f;
            double doubleValue = 32.314;

            using (FileStorage fs = new FileStorage(".xml", FileStorage.Mode.Write | FileStorage.Mode.Memory))
            {
                fs.Write(m, "m");
                fs.Write(intValue, "int");
                fs.Write(floatValue, "float");
                fs.Write(doubleValue, "double");

                string s = fs.ReleaseAndGetString();

                using (FileStorage fs2 = new FileStorage(s, FileStorage.Mode.Read | FileStorage.Mode.Memory))
                {
                    using (FileNode root = fs2.GetRoot())
                        foreach (FileNode n in root)
                        {
                            //String[] keys = n.Keys;
                            String name = n.Name;
                            n.Dispose();
                        }

                    using (FileNode node = fs2.GetFirstTopLevelNode())
                    {
                        Mat m2 = new Mat();
                        node.ReadMat(m2);
                        EmguAssert.IsTrue(m.Equals(m2));
                    }

                    using (FileNode node = fs2.GetNode("m"))
                    {
                        Mat m2 = new Mat();
                        node.ReadMat(m2);
                        EmguAssert.IsTrue(m.Equals(m2));
                    }

                    using (FileNode node = fs2.GetNode("int"))
                    {
                        EmguAssert.IsTrue(intValue.Equals(node.ReadInt()));
                    }

                    using (FileNode node = fs2.GetNode("float"))
                    {
                        EmguAssert.IsTrue(floatValue.Equals(node.ReadFloat()));
                    }

                    using (FileNode node = fs2.GetNode("double"))
                    {
                        EmguAssert.IsTrue(doubleValue.Equals(node.ReadDouble()));
                    }
                }
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
            //Trace.WriteLine(tri.Area);
            //Trace.WriteLine(((p2.X - p1.X) * (p3.Y - p1.Y) - (p2.Y - p1.Y) * (p3.X - p1.X))*0.5);
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
            //Assert.IsTrue(vertices[0].Equals(new PointF(0.0f, 0.0f)));
            //Assert.IsTrue(vertices[1].Equals(new PointF(6.0f, 0.0f)));
        }

#if !(__IOS__ || __ANDROID__ || NETFX_CORE)
        [Test]
        public void TestGrayscaleBitmapConstructor()
        {
            if (Emgu.Util.Platform.OperationSystem == Emgu.Util.Platform.OS.Windows)
            {
                Image<Bgra, Byte> img = new Image<Bgra, byte>(320, 240);
                img.SetRandUniform(new MCvScalar(), new MCvScalar(255, 255, 255, 255));
                img.Save("tmp.png");

                Stopwatch stopwatch = Stopwatch.StartNew();
                Image<Bgra, Byte> img2 = new Image<Bgra, byte>("tmp.png");
                stopwatch.Stop();
                Trace.WriteLine(string.Format("Time: {0} milliseconds", stopwatch.ElapsedMilliseconds));
                Image<Bgra, Byte> absDiff = new Image<Bgra, Byte>(320, 240);
                CvInvoke.AbsDiff(img, img2, absDiff);
                double[] min, max;
                Point[] minLoc, maxLoc;
                double eps = 1;
                absDiff.MinMax(out min, out max, out minLoc, out maxLoc); //ImageViewer.Show(absDiff);
                EmguAssert.IsTrue(max[0] < eps);
                EmguAssert.IsTrue(max[1] < eps);
                EmguAssert.IsTrue(max[2] < eps);

                stopwatch.Reset();
                stopwatch.Start();
                using (Bitmap bmp = new Bitmap("tmp.png"))
                using (Image bmpImage = Bitmap.FromFile("tmp.png"))
                {
                    EmguAssert.AreEqual(System.Drawing.Imaging.PixelFormat.Format32bppArgb, bmpImage.PixelFormat);

                    Image<Gray, Byte> img3 = bmp.ToImage<Gray, byte>();
                    stopwatch.Stop();
                    Trace.WriteLine(string.Format("Time: {0} milliseconds", stopwatch.ElapsedMilliseconds));
                    Image<Gray, Byte> diff = img.Convert<Gray, Byte>().AbsDiff(img3);

                    //Test seems to failed on Linux system. Skipping test on Linux for now.
                    if (!System.Runtime.InteropServices.RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
                        EmguAssert.AreEqual(0, CvInvoke.CountNonZero(diff));

                    EmguAssert.IsTrue(img.Convert<Gray, Byte>().Equals(img3));
                }
            }
        }
#endif
        /*
        [Test]
        public void TestAlgorithmGetList()
        {
           String[] list = AlgorithmExtensions.AlgorithmList;
        }*/

        [Test]
        public void TestMorphEx()
        {
            Mat kernel1 = CvInvoke.GetStructuringElement(Emgu.CV.CvEnum.ElementShape.Cross, new Size(3, 3), new Point(1, 1));
            Matrix<byte> kernel2 = new Matrix<byte>(new Byte[3, 3] { { 0, 1, 0 }, { 1, 0, 1 }, { 0, 1, 0 } });
            //StructuringElementEx element2 = new StructuringElementEx(new int[3, 3] { { 0, 1, 0 }, { 1, 0, 1 }, { 0, 1, 0 } }, 1, 1);
            Image<Bgr, Byte> tmp = new Image<Bgr, byte>(100, 100);
            Image<Bgr, Byte> tmp2 = tmp.MorphologyEx(Emgu.CV.CvEnum.MorphOp.Gradient, kernel1, new Point(-1, -1), 1, CvEnum.BorderType.Default, new MCvScalar());
            Image<Bgr, Byte> tmp3 = tmp.MorphologyEx(Emgu.CV.CvEnum.MorphOp.Gradient, kernel2, new Point(-1, -1), 1, CvEnum.BorderType.Default, new MCvScalar());
            //Image<Bgr, Byte> tmp2 = tmp.MorphologyEx(element1, Emgu.CV.CvEnum.CV_MORPH_OP.CV_MOP_GRADIENT, 1);
            //Image<Bgr, Byte> tmp3 = tmp.MorphologyEx(element2, Emgu.CV.CvEnum.CV_MORPH_OP.CV_MOP_BLACKHAT, 1);
        }

        /*
        [Test]
        public void TestBGModel()
        {
           int width = 300;
           int height = 400;
           Image<Bgr, Byte> bg = new Image<Bgr, byte>(width, height);
           bg.SetRandNormal(new MCvScalar(), new MCvScalar(100, 100, 100));

           Size size = new Size(width / 10, height / 10);
           Point topLeft = new Point((width >> 1) - (size.Width >> 1), (height >> 1) - (size.Height >> 1));

           Rectangle rect = new Rectangle(topLeft, size);

           Image<Bgr, Byte> img1 = bg.Copy();
           img1.Draw(rect, new Bgr(Color.Red), -1);

           Image<Bgr, Byte> img2 = bg.Copy();
           rect.Offset(10, 0);
           img2.Draw(rect, new Bgr(Color.Red), -1);

           BGStatModel<Bgr> model1 = new BGStatModel<Bgr>(img1, Emgu.CV.CvEnum.BgStatType.GaussianBgModel);
           model1.Update(img2);

           BGStatModel<Bgr> model2 = new BGStatModel<Bgr>(img1, Emgu.CV.CvEnum.BgStatType.FgdStatModel);
           model2.Update(img2);

           //ImageViewer.Show(model2.Foreground);
           //ImageViewer.Show(model1.Background);
        }*/

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

           //foreach (Triangle2DF t in triangles)
           //{
           //int equalCount = triangles.FindAll(delegate(Triangle2DF t2) { return t2.Equals(t); }).Count;
           //Assert.AreEqual(1, equalCount, "Triangle duplicates");

           //int overlapCount = triangles.FindAll(delegate(Triangle2D t2) { return Util.IsConvexPolygonInConvexPolygon(t2, t);}).Count;
           //Assert.AreEqual(1, overlapCount, "Triangle overlaps");
           //}
        }

        [Test]
        public void TestPlanarSubdivision1()
        {
           //TestPlanarSubdivisionHelper(10000);
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
                    //you might want to store the points which is not inserted here.
                    //or alternatively, add some random noise to the point and re-insert it again.
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

        /*
        [Test]
        public void TestGetModuleInfo()
        {
           Image<Bgr, Byte> img = new Image<Bgr, byte>(200, 100);
           img.Sobel(1, 0, 3);
           String plugin, module;
           CvToolbox.GetModuleInfo(out plugin, out module);
        }*/

        [Test]
        public void TestCrossProduct()
        {
            MCvPoint3D32f p1 = new MCvPoint3D32f(1.0f, 0.0f, 0.0f);
            MCvPoint3D32f p2 = new MCvPoint3D32f(0.0f, 1.0f, 0.0f);
            MCvPoint3D32f p3 = p1.CrossProduct(p2);
            EmguAssert.IsTrue(new MCvPoint3D32f(0.0f, 0.0f, 1.0f).Equals(p3));
        }

        [Test]
        public void TestMatchTemplate()
        {
            #region prepare synthetic image for testing
            int templWidth = 50;
            int templHeight = 50;
            Point templCenter = new Point(120, 100);

            //Create a random object
            Image<Bgr, Byte> randomObj = new Image<Bgr, byte>(templWidth, templHeight);
            randomObj.SetRandUniform(new MCvScalar(), new MCvScalar(255, 255, 255));

            //Draw the object in image1 center at templCenter;
            Image<Bgr, Byte> img = new Image<Bgr, byte>(300, 200);
            Rectangle objectLocation = new Rectangle(templCenter.X - (templWidth >> 1), templCenter.Y - (templHeight >> 1), templWidth, templHeight);
            img.ROI = objectLocation;
            randomObj.Copy(img, null);
            img.ROI = Rectangle.Empty;
            #endregion

            Image<Gray, Single> match = img.MatchTemplate(randomObj, Emgu.CV.CvEnum.TemplateMatchingType.Sqdiff);
            double[] minVal, maxVal;
            Point[] minLoc, maxLoc;
            match.MinMax(out minVal, out maxVal, out minLoc, out maxLoc);

            EmguAssert.AreEqual(minLoc[0].X, templCenter.X - templWidth / 2);
            EmguAssert.AreEqual(minLoc[0].Y, templCenter.Y - templHeight / 2);
        }

        /// <summary>
        /// Prepare synthetic image for testing
        /// </summary>
        public static Mat[] OpticalFlowImage()
        {
            //Create a random object
            using (Mat randomObj = new Mat(new Size(50, 50), DepthType.Cv8U, 1))
            {
                CvInvoke.Randu(randomObj, new MCvScalar(), new MCvScalar(255));

                //Draw the object in image1 center at (100, 100);
                Mat prevImg = new Mat(new Size(300, 200), DepthType.Cv8U, 1);
                Rectangle objectLocation = new Rectangle(75, 75, 50, 50);
                using (Mat roi = new Mat(prevImg, objectLocation))
                {
                    randomObj.CopyTo(roi);
                }
                
                //Draw the object in image2 center at (102, 103);
                Mat currImg = new Mat(new Size(300, 200), DepthType.Cv8U, 1);
                objectLocation.Offset(2, 3);
                using (Mat roi = new Mat(currImg, objectLocation))
                {
                    randomObj.CopyTo(roi);
                }

                return new Mat[] {prevImg, currImg};
            }
        }

        [Test]
        public void TestPCAFlow()
        {
            Mat[] images = OpticalFlowImage();
            Mat flow = new Mat();

            using (Emgu.CV.OpticalFlowPCAFlow pcaFlow = new OpticalFlowPCAFlow())
            {
                pcaFlow.Calc(images[0], images[1], flow);
            }
        }

        [Test]
        public void TestDISOpticalFlow()
        {
            Mat[] images = OpticalFlowImage();
            Mat flow = new Mat();

            using (Emgu.CV.DISOpticalFlow disFlow = new DISOpticalFlow())
            {
                disFlow.Calc(images[0], images[1], flow);
            }
        }

        [Test]
        public void TestOpticalFlowFarneback()
        {
            Mat[] images = OpticalFlowImage();
            //Image<Gray, Single> flowx = new Image<Gray, float>(images[0].Size);
            //Image<Gray, Single> flowy = new Image<Gray, float>(images[0].Size);
            Mat flow = new Mat();
            CvInvoke.CalcOpticalFlowFarneback(images[0], images[1], flow, 0.5, 3, 5, 20, 7, 1.5, Emgu.CV.CvEnum.OpticalflowFarnebackFlag.Default);
            Point pos = new Point();
            /*
            bool noNan = CvInvoke.CheckRange(flowx, true, ref pos, double.MinValue, double.MaxValue);
            EmguAssert.IsTrue(noNan, "Flowx contains nan");
            noNan = CvInvoke.CheckRange(flowy, true, ref pos, double.MinValue, double.MaxValue);
            EmguAssert.IsTrue(noNan, "Flowy contains nan");
            */
        }

        /*
        [Test]
        public void TestOpticalFlowBM()
        {
           Image<Gray, Byte> prevImg, currImg;
           OpticalFlowImage(out prevImg, out currImg);
           Size blockSize = new Size(5, 5);
           Size shiftSize = new Size(1, 1);
           Size maxRange = new Size(10, 10);
           Size velSize = new Size(
              (int) Math.Floor((prevImg.Width - blockSize.Width + shiftSize.Width) / (double) shiftSize.Width),
              (int) Math.Floor((prevImg.Height - blockSize.Height + shiftSize.Height) / (double) shiftSize.Height));
           Image<Gray, float> velx = new Image<Gray, float>(velSize);
           Image<Gray, float> vely = new Image<Gray, float>(velSize);

           Stopwatch watch = Stopwatch.StartNew();

           CvInvoke.cvCalcOpticalFlowBM(prevImg, currImg, blockSize, shiftSize, maxRange, false, velx, vely);

           watch.Stop();

           EmguAssert.WriteLine(String.Format(
              "Time: {0} milliseconds",
              watch.ElapsedMilliseconds));

        }*/

        [Test]
        public void TestOpticalFlowLK()
        {
            Mat[] images = OpticalFlowImage();

            PointF[] prevFeature = new PointF[] { new PointF(100f, 100f) };

            PointF[] currFeature;
            Byte[] status;
            float[] trackError;

            Stopwatch watch = Stopwatch.StartNew();

            CvInvoke.CalcOpticalFlowPyrLK(
               images[0], images[1], prevFeature, new Size(10, 10), 3, new MCvTermCriteria(10, 0.01),
               out currFeature, out status, out trackError);
            watch.Stop();
            EmguAssert.WriteLine(String.Format(
               "prev: ({0}, {1}); curr: ({2}, {3}); \r\nTime: {4} milliseconds",
               prevFeature[0].X, prevFeature[0].Y,
               currFeature[0].X, currFeature[0].Y,
               watch.ElapsedMilliseconds));

        }

        [Test]
        public void TestOpticalFlowDualTVL1()
        {
            Mat[] images = OpticalFlowImage();
            Mat result = new Mat();

            Stopwatch watch = Stopwatch.StartNew();
            DualTVL1OpticalFlow flow = new DualTVL1OpticalFlow();

            flow.Calc(images[0], images[1], result);

            watch.Stop();
            EmguAssert.WriteLine(String.Format(
               "Time: {0} milliseconds",
               watch.ElapsedMilliseconds));
        }

        /*
        [Test]
        public void TestKDTree()
        {
           float[][] features = new float[10][];
           for (int i = 0; i < features.Length; i++)
              features[i] = new float[] { (float) i };
           FeatureTree tree = new FeatureTree(features);

           Matrix<Int32> result;
           Matrix<double> distance;
           float[][] features2 = new float[1][];
           features2[0] = new float[] { 5.0f };

           tree.FindFeatures(features2, out result, out distance, 1, 20);
           EmguAssert.IsTrue(result[0, 0] == 5);
           EmguAssert.IsTrue(distance[0, 0] == 0.0);
        }

        [Test]
        public void TestSpillTree()
        {
           float[][] features = new float[10][];
           for (int i = 0; i < features.Length; i++)
              features[i] = new float[] { (float) i };
           FeatureTree tree = new FeatureTree(features, 50, .7, .1);

           Matrix<Int32> result;
           Matrix<double> distance;
           float[][] features2 = new float[1][];
           features2[0] = new float[] { 5.0f };

           tree.FindFeatures(features2, out result, out distance, 1, 20);
           EmguAssert.IsTrue(result[0, 0] == 5);
           EmguAssert.IsTrue(distance[0, 0] == 0.0);
        }*/

        [Test]
        public void TestFlannLinear()
        {
            float[][] features = new float[10][];
            for (int i = 0; i < features.Length; i++)
                features[i] = new float[] { (float)i };
            Flann.LinearIndexParams p = new LinearIndexParams();
            Flann.Index index = new Flann.Index(CvToolbox.GetMatrixFromArrays(features), p);

            float[][] features2 = new float[1][];
            features2[0] = new float[] { 5.0f };

            Matrix<int> indices = new Matrix<int>(features2.Length, 1);
            Matrix<float> distances = new Matrix<float>(features2.Length, 1);
            index.KnnSearch(CvToolbox.GetMatrixFromArrays(features2), indices, distances, 1, 32);

            EmguAssert.IsTrue(indices[0, 0] == 5);
            EmguAssert.IsTrue(distances[0, 0] == 0.0);
        }

        [Test]
        public void TestFlannKDTree()
        {
            float[][] features = new float[10][];
            for (int i = 0; i < features.Length; i++)
                features[i] = new float[] { (float)i };

            Flann.KdTreeIndexParams p = new KdTreeIndexParams(4);
            Flann.Index index = new Flann.Index(CvToolbox.GetMatrixFromArrays(features), p);

            float[][] features2 = new float[1][];
            features2[0] = new float[] { 5.0f };

            Matrix<int> indices = new Matrix<int>(features2.Length, 1);
            Matrix<float> distances = new Matrix<float>(features2.Length, 1);
            index.KnnSearch(CvToolbox.GetMatrixFromArrays(features2), indices, distances, 1, 32);

            EmguAssert.IsTrue(indices[0, 0] == 5);
            EmguAssert.IsTrue(distances[0, 0] == 0.0);
        }

        [Test]
        public void TestFlannCompositeTree()
        {
            float[][] features = new float[10][];
            for (int i = 0; i < features.Length; i++)
                features[i] = new float[] { (float)i };

            Flann.CompositeIndexParams p = new CompositeIndexParams(4, 32, 11, Emgu.CV.Flann.CenterInitType.Random, 0.2f);
            Flann.Index index = new Flann.Index(CvToolbox.GetMatrixFromArrays(features), p);

            float[][] features2 = new float[1][];
            features2[0] = new float[] { 5.0f };

            Matrix<int> indices = new Matrix<int>(features2.Length, 1);
            Matrix<float> distances = new Matrix<float>(features2.Length, 1);
            index.KnnSearch(CvToolbox.GetMatrixFromArrays(features2), indices, distances, 1, 32);

            EmguAssert.IsTrue(indices[0, 0] == 5);
            EmguAssert.IsTrue(distances[0, 0] == 0.0);
        }


        /*
        [Test]
        public void TestEigenObjectRecognizer()
        {
           Image<Gray, Byte>[] images = new Image<Gray, byte>[20];
           for (int i = 0; i < images.Length; i++)
           {
              images[i] = new Image<Gray, byte>(200, 200);
              images[i].SetRandUniform(new MCvScalar(0), new MCvScalar(255));
           }
           MCvTermCriteria termCrit = new MCvTermCriteria(10, 1.0e-6);

           EigenObjectRecognizer rec = new EigenObjectRecognizer(images, ref termCrit);
           foreach (Image<Gray, Byte> img in images)
           {
              rec.Recognize(img);
              //Trace.WriteLine(rec.Recognize(img));
           }
        }
        */

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
        public void TestFaceRecognizer()
        {
            int trainingImgCount = 20;
            int numComponents = trainingImgCount / 5;
            Mat[] images = new Mat[trainingImgCount];
            int[] labels = new int[trainingImgCount];
            for (int i = 0; i < images.Length; i++)
            {
                images[i] = new Mat(new Size(200, 200), DepthType.Cv8U, 1);
                CvInvoke.Randu(images[i], new MCvScalar(0), new MCvScalar(255));

                labels[i] = i;
            }

            Mat[] images2 = new Mat[trainingImgCount];
            int[] labels2 = new int[trainingImgCount];
            for (int i = 0; i < images2.Length; i++)
            {
                images2[i] = new Mat(new Size(200, 200), DepthType.Cv8U, 1);
                CvInvoke.Randu(images2[i], new MCvScalar(0), new MCvScalar(255));

                labels2[i] = i + labels.Length;
            }

            Mat sample = new Mat(new Size(200, 200), DepthType.Cv8U, 1);
            CvInvoke.Randu(sample, new MCvScalar(0), new MCvScalar(255));

            EigenFaceRecognizer eigen = new EigenFaceRecognizer(numComponents, double.MaxValue);

            eigen.Train(images, labels);
            FaceRecognizer.PredictionResult result;
            for (int i = 0; i < images.Length; i++)
            {
                result = eigen.Predict(images[i]);
                EmguAssert.IsTrue(result.Label == i);
            }

            result = eigen.Predict(sample);
            Trace.WriteLine(String.Format("Eigen distance: {0}", result.Distance));
            String filePath = Path.Combine(Path.GetTempPath(), "abc.xml");

            eigen.Write(filePath);
            using (EigenFaceRecognizer eigen2 = new EigenFaceRecognizer(numComponents, double.MaxValue))
            {
                eigen2.Read(filePath);
                for (int i = 0; i < images.Length; i++)
                {
                    result = eigen2.Predict(images[i]);
                    EmguAssert.IsTrue(result.Label == i);
                }
            }

            FisherFaceRecognizer fisher = new FisherFaceRecognizer(0, double.MaxValue);
            fisher.Train(images, labels);
            for (int i = 0; i < images.Length; i++)
            {
                result = fisher.Predict(images[i]);
                EmguAssert.IsTrue(result.Label == i);
            }
            result = fisher.Predict(sample);
            Trace.WriteLine(String.Format("Fisher distance: {0}", result.Distance));

            LBPHFaceRecognizer lbph = new LBPHFaceRecognizer(1, 8, 8, 8, double.MaxValue);
            lbph.Train(images, labels);
            lbph.Update(images2, labels2);
            
            using (VectorOfMat vm = lbph.Histograms)
            {
                EmguAssert.IsTrue(vm.Size == images.Length + images2.Length);
            }
            for (int i = 0; i < images.Length; i++)
            {
                EmguAssert.IsTrue(lbph.Predict(images[i]).Label == i);
            }

        }
        /*
        //This took ~ 60 seconds to finishes
        [Test]
        public void TestCameraCalibration()
        {
           MCvPoint3D32f[][] objectPoints = new MCvPoint3D32f[20][];
           for (int i = 0; i < objectPoints.Length; i++)
              objectPoints[i] = new MCvPoint3D32f[10];

           PointF[][] imagePoints = new PointF[20][];
           for (int i = 0; i < imagePoints.Length; i++)
              imagePoints[i] = new PointF[10];

           Size size = new Size(20, 20);
           ExtrinsicCameraParameters[] extrinsicParameters;
           CameraCalibration.CalibrateCamera(
              objectPoints,
              imagePoints,
              size,
              new IntrinsicCameraParameters(),
              Emgu.CV.CvEnum.CALIB_TYPE.DEFAULT,
              out extrinsicParameters);
        }*/

        /*
        [Test]
        public void TestContourCreate()
        {
           using (MemStorage stor = new MemStorage())
           {
              Contour<Point> contour = new Contour<Point>(stor);
              contour.Push(new Point(0, 0));
              contour.Push(new Point(0, 2));
              contour.Push(new Point(2, 2));
              contour.Push(new Point(2, 0));
              EmguAssert.IsTrue(contour.Convex);
              EmguAssert.AreEqual(contour.Area, 4.0);
              //InContour function requires MCvContour.rect to be pre-computed
              CvInvoke.cvBoundingRect(contour, 1);
              EmguAssert.IsTrue(contour.InContour(new Point(1, 1)) >= 0);
              EmguAssert.IsTrue(contour.InContour(new Point(3, 3)) < 0);

              Contour<PointF> contourF = new Contour<PointF>(stor);
              contourF.Push(new PointF(0, 0));
              contourF.Push(new PointF(0, 2));
              contourF.Push(new PointF(2, 2));
              contourF.Push(new PointF(2, 0));
              EmguAssert.IsTrue(contourF.Convex);
              EmguAssert.AreEqual(contourF.Area, 4.0);
              //InContour function requires MCvContour.rect to be pre-computed
              CvInvoke.cvBoundingRect(contourF, 1);
              EmguAssert.IsTrue(contourF.InContour(new PointF(1, 1)) >= 0);
              EmguAssert.IsTrue(contourF.InContour(new PointF(3, 3)) < 0);

              Contour<MCvPoint2D64f> contourD = new Contour<MCvPoint2D64f>(stor);
              contourD.Push(new MCvPoint2D64f(0, 0));
              contourD.Push(new MCvPoint2D64f(0, 2));
              contourD.Push(new MCvPoint2D64f(2, 2));
              contourD.Push(new MCvPoint2D64f(2, 0));
              //Assert.IsTrue(contourD.Convex);
              //Assert.AreEqual(contourD.Area, 4.0);
              //InContour function requires MCvContour.rect to be pre-computed
              //CvInvoke.cvBoundingRect(contourD, 1);
              //Assert.GreaterOrEqual(contourD.InContour(new PointF(1, 1)), 0);
              //Assert.Less(contourD.InContour(new PointF(3, 3)), 0);

              Seq<Point> seq = new Seq<Point>(CvInvoke.MakeType(CvEnum.DepthType.Cv32S, 2), stor);
              seq.Push(new Point(0, 0));
              seq.Push(new Point(0, 1));
              seq.Push(new Point(1, 1));
              seq.Push(new Point(1, 0));
           }
        }*/

        [Test]
        public void TestConvexHull()
        {
            #region Create some random points
            Random r = new Random();
            PointF[] pts = new PointF[200];
            for (int i = 0; i < pts.Length; i++)
            {
                pts[i] = new PointF((float)(100 + r.NextDouble() * 400), (float)(100 + r.NextDouble() * 400));
            }
            #endregion

            Mat img = new Mat(600, 600, DepthType.Cv8U, 3);
            img.SetTo(new MCvScalar(255.0, 255.0, 255.0));
            //Draw the points 
            foreach (PointF p in pts)
                CvInvoke.Circle(img, Point.Round(p), 3, new MCvScalar(0.0, 0.0, 0.0));

            //Find and draw the convex hull

            Stopwatch watch = Stopwatch.StartNew();
            PointF[] hull = CvInvoke.ConvexHull(pts, true);
            watch.Stop();
            CvInvoke.Polylines(
               img,
               Array.ConvertAll<PointF, Point>(hull, Point.Round),
               true, new MCvScalar(255.0, 0.0, 0.0));

            //Emgu.CV.UI.ImageViewer.Show(img, String.Format("Convex Hull Computed in {0} milliseconds", watch.ElapsedMilliseconds));

        }

        /*
        [Test]
        public void TestStereoGCCorrespondence()
        {
           Image<Gray, Byte> left = EmguAssert.LoadImage<Gray, byte>("scene_l.bmp");
           Image<Gray, Byte> right = EmguAssert.LoadImage<Gray, byte>("scene_r.bmp");
           Image<Gray, Int16> leftDisparity = new Image<Gray, Int16>(left.Size);
           Image<Gray, Int16> rightDisparity = new Image<Gray, Int16>(left.Size);

           StereoGC stereoSolver = new StereoGC(10, 5);
           Stopwatch watch = Stopwatch.StartNew();
           stereoSolver.FindStereoCorrespondence(left, right, leftDisparity, rightDisparity);
           watch.Stop();
           EmguAssert.WriteLine(String.Format("Time used: {0} milliseconds", watch.ElapsedMilliseconds));

           Matrix<double> q = new Matrix<double>(4, 4);
           q.SetIdentity();
           MCvPoint3D32f[] points = PointCollection.ReprojectImageTo3D(leftDisparity * (-16), q);

           float min = (float) 1.0e10, max = 0;
           foreach (MCvPoint3D32f p in points)
           {
              if (p.Z < min)
                 min = p.Z;
              else if (p.Z > max)
                 max = p.Z;
           }
           EmguAssert.WriteLine(String.Format("Min : {0}\r\nMax : {1}", min, max));

           //ImageViewer.Show(leftDisparity*(-16));
        }*/

        /*
        //TODO: This test is failing, check when this will be fixed.
        [Test]
        public void TestStereoBMCorrespondence()
        {
           Image<Gray, Byte> left = EmguAssert.LoadImage<Gray, byte>("scene_l.bmp");
           Image<Gray, Byte> right = EmguAssert.LoadImage<Gray, byte>("scene_r.bmp");
           Image<Gray, Int16> leftDisparity = new Image<Gray, Int16>(left.Size);
           Image<Gray, Int16> rightDisparity = new Image<Gray, Int16>(left.Size);

           StereoBM bm = new StereoBM(Emgu.CV.CvEnum.STEREO_BM_TYPE.BASIC, 0);
           Stopwatch watch = Stopwatch.StartNew();
           bm.FindStereoCorrespondence(left, right, leftDisparity);
           watch.Stop();

           EmguAssert.WriteLine(String.Format("Time used: {0} milliseconds", watch.ElapsedMilliseconds));

           Matrix<double> q = new Matrix<double>(4, 4);
           q.SetIdentity();
           MCvPoint3D32f[] points = PointCollection.ReprojectImageTo3D(leftDisparity * (-16), q);

           float min = (float) 1.0e10, max = 0;
           foreach (MCvPoint3D32f p in points)
           {
              if (p.z < min)
                 min = p.z;
              else if (p.z > max)
                 max = p.z;
           }
           EmguAssert.WriteLine(String.Format("Min : {0}\r\nMax : {1}", min, max));

        }
  */

        [Test]
        public void TestStereoSGBMCorrespondence()
        {
            Mat left = EmguAssert.LoadMat("aloeL.jpg", ImreadModes.Grayscale);
            Mat right = EmguAssert.LoadMat("aloeR.jpg", ImreadModes.Grayscale);
            Size size = left.Size;

            Mat leftDisparity = new Mat();
            Mat rightDisparity = new Mat();
            
            StereoSGBM bmLeft = new StereoSGBM(10, 64, 0, 0, 0, 0, 0, 0, 0, 0, StereoSGBM.Mode.SGBM);
            RightMatcher bmRight = new RightMatcher(bmLeft);
            DisparityWLSFilter wlsFilter = new DisparityWLSFilter(bmLeft);
            
            Stopwatch watch = Stopwatch.StartNew();
            bmLeft.Compute(left, right, leftDisparity);
            watch.Stop();
            EmguAssert.WriteLine(String.Format("Time used: {0} milliseconds", watch.ElapsedMilliseconds));
            
            bmRight.Compute(right, left, rightDisparity);

            Mat filteredDisparity = new Mat();
            wlsFilter.Filter(leftDisparity, left, filteredDisparity, rightDisparity, new Rectangle(), right);
            

            Matrix<double> q = new Matrix<double>(4, 4);
            q.SetIdentity();
            Mat disparityScaled = leftDisparity * (-16);
            MCvPoint3D32f[] points = PointCollection.ReprojectImageTo3D(disparityScaled, q);

            float min = (float)1.0e10, max = 0;
            foreach (MCvPoint3D32f p in points)
            {
                if (p.Z < min)
                    min = p.Z;
                else if (p.Z > max)
                    max = p.Z;
            }
            EmguAssert.WriteLine(String.Format("Min : {0}\r\nMax : {1}", min, max));

        }

        /*
#if !WINDOWS_PHONE_APP
        [Test]
        public void TestExtrinsicCameraParametersRuntimeSerialize()
        {
           ExtrinsicCameraParameters param = new ExtrinsicCameraParameters();

           CvInvoke.Randu(param.RotationVector, new MCvScalar(), new MCvScalar(1.0));
           CvInvoke.Randu(param.TranslationVector, new MCvScalar(), new MCvScalar(100)  );

           System.Runtime.Serialization.Formatters.Binary.BinaryFormatter
               formatter = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();

           Byte[] bytes;
           using (MemoryStream ms = new MemoryStream())
           {
              formatter.Serialize(ms, param);
              bytes = ms.GetBuffer();
           }
           using (MemoryStream ms2 = new MemoryStream(bytes))
           {
              ExtrinsicCameraParameters param2 = (ExtrinsicCameraParameters) formatter.Deserialize(ms2);

              EmguAssert.IsTrue(param.Equals(param2));
           }
        }

        [Test]
        public void TestIntrinsicCameraParametersRuntimeSerialize()
        {
           IntrinsicCameraParameters param = new IntrinsicCameraParameters();

           param.DistortionCoeffs.SetRandUniform(new MCvScalar(), new MCvScalar(1.0));
           param.IntrinsicMatrix.SetRandUniform(new MCvScalar(), new MCvScalar(100));

           System.Runtime.Serialization.Formatters.Binary.BinaryFormatter
               formatter = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();

           Byte[] bytes;
           using (MemoryStream ms = new MemoryStream())
           {
              formatter.Serialize(ms, param);
              bytes = ms.GetBuffer();
           }
           using (MemoryStream ms2 = new MemoryStream(bytes))
           {
              IntrinsicCameraParameters param2 = (IntrinsicCameraParameters) formatter.Deserialize(ms2);

              EmguAssert.IsTrue(param.Equals(param2));
           }
        }
#endif
  */
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

            #region draw the points and the fitted ellips
            Mat img = new Mat(400, 400, DepthType.Cv8U, 3);
            img.SetTo(new MCvScalar(255, 255, 255));
            foreach (PointF p in pts)
                CvInvoke.Circle(img, Point.Round(p), 2, new MCvScalar(0, 255, 0), 1);
            RotatedRect rect = fittedEllipse.RotatedRect;
            rect.Angle += 90; //the detected ellipse was off by 90 degree
            CvInvoke.Ellipse(img, rect, new MCvScalar(0, 0, 255), 2);
            #endregion

            //Emgu.CV.UI.ImageViewer.Show(img, String.Format("Time used: {0} milliseconds", watch.ElapsedMilliseconds));
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

            //Emgu.CV.UI.ImageViewer.Show(img, String.Format("Time used: {0} milliseconds", watch.ElapsedMilliseconds));
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

            //Emgu.CV.UI.ImageViewer.Show(img, String.Format("Time used: {0} milliseconds", watch.ElapsedMilliseconds));
        }

        /*

        [Test]
        public void TestMatND()
        {
           using (MatND<float> mat = new MatND<float>(3, 5, 1))
           {
              mat.SetRandNormal(new MCvScalar(), new MCvScalar(255));
              MatND<double> matD = mat.Convert<double>();
              MCvMatND matND = matD.MCvMatND;
              int rows = matND.dim[0].Size;
              int cols = matND.dims >= 2 ? matND.dim[1].Size : 1;
              int channels = matND.dims >= 3 ? matND.dim[2].Size : 1;
              Matrix<double> matrix = new Matrix<double>(rows, cols, channels);
              CvInvoke.cvCopy(matD, matrix, IntPtr.Zero);
              //using (MatrixViewer viewer = new MatrixViewer())
              {
                 //viewer.Matrix = matrix;
                 //viewer.ShowDialog();
              }
           }
        }

        [Test]
        public void TestSURFFeatureRuntimeSerialization()
        {
           MCvSURFPoint p = new MCvSURFPoint();
           float[] desc = new float[36];
           SURFFeature sf = new SURFFeature(ref p, desc);
           using (MemoryStream ms = new MemoryStream())
           {
              System.Runtime.Serialization.Formatters.Binary.BinaryFormatter
                     formatter = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
              formatter.Serialize(ms, sf);

              Byte[] bytes = ms.GetBuffer();

              using (MemoryStream ms2 = new MemoryStream(bytes))
              {
                 Object o = formatter.Deserialize(ms2);
                 SURFFeature sf2 = (SURFFeature) o;
              }
           }
        }*/

#if !NETFX_CORE
        [Test]
        public void TestMatNDRuntimeSerialization()
        {
            using (MatND<float> mat = new MatND<float>(2, 3, 4, 5))
            {
                using (MemoryStream ms = new MemoryStream())
                {
                    mat.SetRandNormal(new MCvScalar(100), new MCvScalar(50));

                    mat.SerializationCompressionRatio = 6;
                    System.Runtime.Serialization.Formatters.Binary.BinaryFormatter
                        formatter = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
                    formatter.Serialize(ms, mat);
                    Byte[] bytes = ms.GetBuffer();

                    using (MemoryStream ms2 = new MemoryStream(bytes))
                    {
                        Object o = formatter.Deserialize(ms2);
                        MatND<float> mat2 = (MatND<float>)o;
                        EmguAssert.IsTrue(mat.Equals(mat2));
                    }
                }
            }
        }

        [Test]
        public void TestVectorOfKeyPointSerialization()
        {
            using (VectorOfKeyPoint kpts = new VectorOfKeyPoint())
            {

                using (MemoryStream ms = new MemoryStream())
                {
                    kpts.Push(new MKeyPoint[] { new MKeyPoint() });
                    System.Runtime.Serialization.Formatters.Binary.BinaryFormatter
                        formatter = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
                    formatter.Serialize(ms, kpts);
                    Byte[] bytes = ms.GetBuffer();

                    using (MemoryStream ms2 = new MemoryStream(bytes))
                    {
                        Object o = formatter.Deserialize(ms2);
                        VectorOfKeyPoint kpts2 = (VectorOfKeyPoint)o;
                    }
                }
            }
        }
#endif

        [Test]
        public void TestVectorOfMat()
        {
            Matrix<double> m1 = new Matrix<double>(3, 3);
            m1.SetRandNormal(new MCvScalar(0.0), new MCvScalar(1.0));
            Matrix<int> m2 = new Matrix<int>(4, 4);
            m2.SetRandNormal(new MCvScalar(2), new MCvScalar(2));

            VectorOfMat vec = new VectorOfMat(m1.Mat, m2.Mat);

            Mat tmp1 = vec[0];
            Mat tmp2 = vec[1];
            Matrix<double> n1 = new Matrix<double>(tmp1.Size);
            Matrix<int> n2 = new Matrix<int>(tmp2.Size);
            tmp1.CopyTo(n1, null);
            tmp2.CopyTo(n2, null);

            EmguAssert.IsTrue(m1.Equals(n1));
            EmguAssert.IsTrue(m2.Equals(n2));
        }

        /*
        [Test]
        public void TestPyrSegmentation()
        {
           Image<Bgr, Byte> image = EmguAssert.LoadImage<Bgr, Byte>("lena.jpg");
           Image<Bgr, Byte> segImage = new Image<Bgr, byte>(image.Size);
           MemStorage storage = new MemStorage();
           IntPtr comp;
           CvInvoke.cvPyrSegmentation(image, segImage, storage, out comp, 4, 255, 30);
        }*/

        [Test]
        public void TestHistogram()
        {
            using (Image<Bgr, Byte> img = EmguAssert.LoadImage<Bgr, Byte>("stuff.jpg"))
            using (Image<Hsv, Byte> img2 = img.Convert<Hsv, Byte>())
            {
                Image<Gray, Byte>[] HSVs = img2.Split();

                using (Mat h = new Mat())
                using (Mat bpj = new Mat())
                using (VectorOfMat vm = new VectorOfMat())
                {
                    vm.Push(HSVs[0]);
                    CvInvoke.CalcHist(vm, new int[] { 0 }, null, h, new int[] { 20 }, new float[] { 0, 180 }, false);
                    CvInvoke.CalcBackProject(vm, new int[] { 0 }, h, bpj, new float[] { 0, 180 }, 0.1);

                    //Emgu.CV.UI.HistogramViewer.Show(bpj);
                    //Emgu.CV.UI.ImageViewer.Show(bpj);
                    //h.Calculate(new Image<Gray, Byte>[1] { HSVs[0] }, true, null);
                    //using (Image<Gray, Byte> bpj = h.BackProject(new Image<Gray, Byte>[1] { HSVs[0] }))
                    //{
                    //   Size sz = bpj.Size;
                    //}

                    //using (Image<Gray, Single> patchBpj = h.BackProjectPatch(
                    //   new Image<Gray, Byte>[1] { HSVs[0] },
                    //   new Size(5, 5),
                    //   Emgu.CV.CvEnum.HISTOGRAM_COMP_METHOD.CV_COMP_CHISQR,
                    //   1.0))
                    //{
                    //   Size sz = patchBpj.Size;
                    //}
                }

                foreach (Image<Gray, Byte> i in HSVs)
                    i.Dispose();
            }
        }

        /*
        [Test]
        public void TestSoftcascade()
        {
           using (SoftCascadeDetector detector = new SoftCascadeDetector(EmguAssert.GetFile("inria_caltech-17.01.2013.xml"), 0.4, 5.0, 55, SoftCascadeDetector.RejectionCriteria.Default))
           using (Image<Bgr, Byte> image = EmguAssert.LoadImage<Bgr, Byte>("pedestrian.png"))
           {
              Stopwatch watch = Stopwatch.StartNew();
              SoftCascadeDetector.Detection[] detections = detector.Detect(image, null);
              watch.Stop();
              foreach (SoftCascadeDetector.Detection detection in detections)
                 image.Draw(detection.BoundingBox, new Bgr(Color.Red), 1);

              //Emgu.CV.UI.ImageViewer.Show(image, String.Format("Detection Time: {0}ms", watch.ElapsedMilliseconds));
           }
        }*/

        [Test]
        public void TestHOG1()
        {
            using (HOGDescriptor hog = new HOGDescriptor())
            using (Image<Bgr, Byte> image = EmguAssert.LoadImage<Bgr, Byte>("pedestrian.png"))
            {
                float[] pedestrianDescriptor = HOGDescriptor.GetDefaultPeopleDetector();
                hog.SetSVMDetector(pedestrianDescriptor);

                Stopwatch watch = Stopwatch.StartNew();
                MCvObjectDetection[] rects = hog.DetectMultiScale(image);
                watch.Stop();

                EmguAssert.AreEqual(1, rects.Length);

                foreach (MCvObjectDetection rect in rects)
                    image.Draw(rect.Rect, new Bgr(0, 0, 255), 1);
                EmguAssert.WriteLine(String.Format("HOG detection time: {0} ms", watch.ElapsedMilliseconds));

                //Emgu.CV.UI.ImageViewer.Show(image, String.Format("Detection Time: {0}ms", watch.ElapsedMilliseconds));
            }
        }

        [Test]
        public void TestHOG2()
        {
            using (HOGDescriptor hog = new HOGDescriptor())
            using (Image<Bgr, Byte> image = EmguAssert.LoadImage<Bgr, Byte>("lena.jpg"))
            {
                float[] pedestrianDescriptor = HOGDescriptor.GetDefaultPeopleDetector();
                hog.SetSVMDetector(pedestrianDescriptor);

                Stopwatch watch = Stopwatch.StartNew();
                MCvObjectDetection[] rects = hog.DetectMultiScale(image);
                watch.Stop();

                EmguAssert.AreEqual(0, rects.Length);
                foreach (MCvObjectDetection rect in rects)
                    image.Draw(rect.Rect, new Bgr(0, 0, 255), 1);
                EmguAssert.WriteLine(String.Format("HOG detection time: {0} ms", watch.ElapsedMilliseconds));

                //ImageViewer.Show(image, String.Format("Detection Time: {0}ms", watch.ElapsedMilliseconds));
            }
        }

        /*
        [Test]
        public void TestHOGTrain64x128()
        {
            using (Image<Bgr, byte> image = EmguAssert.LoadImage<Bgr, Byte>("lena.jpg"))
            using (Image<Bgr, Byte> resize = image.Resize(64, 128, Emgu.CV.CvEnum.Inter.Cubic))
            using (HOGDescriptor hog = new HOGDescriptor(resize))
            {
                Stopwatch watch = Stopwatch.StartNew();
                MCvObjectDetection[] rects = hog.DetectMultiScale(image);
                watch.Stop();
                foreach (MCvObjectDetection rect in rects)
                    image.Draw(rect.Rect, new Bgr(0, 0, 255), 1);

                //ImageViewer.Show(image, String.Format("Detection Time: {0}ms", watch.ElapsedMilliseconds));
            }
        }

        [Test]
        public void TestHOGTrainAnySize()
        {
            using (Image<Bgr, byte> image = EmguAssert.LoadImage<Bgr, Byte>("lena.jpg"))
            using (HOGDescriptor hog = new HOGDescriptor(image))
            {

                Stopwatch watch = Stopwatch.StartNew();
                MCvObjectDetection[] rects = hog.DetectMultiScale(image);
                watch.Stop();
                foreach (MCvObjectDetection rect in rects)
                    image.Draw(rect.Rect, new Bgr(0, 0, 255), 1);

                EmguAssert.WriteLine(String.Format("Detection Time: {0}ms", watch.ElapsedMilliseconds));
                //ImageViewer.Show(image, String.Format("Detection Time: {0}ms", watch.ElapsedMilliseconds));
            }
        }

        
        [Test]
        public void TestOctTree()
        {
           MCvPoint3D32f[] pts = new MCvPoint3D32f[] 
           {
              new MCvPoint3D32f(1, 2, 3),
              new MCvPoint3D32f(2, 3, 4),
              new MCvPoint3D32f(4, 5, 6),
              new MCvPoint3D32f(2, 2, 2)
           };

           using (Octree tree = new Octree(pts, 10, 10))
           {
              MCvPoint3D32f[] p = tree.GetPointsWithinSphere(new MCvPoint3D32f(0, 0, 0), 5);
              int i = p.Length;
           }
        }*/

        /*
        [Test]
        public void TestVectorOfFloat()
        {
           int k = 0;
           for (int i = 0; i < 1000000; i++)
           {
              using (VectorOfFloat v = new VectorOfFloat(1000))
              {
                 k += v.Size;
              }
           }
        }*/

        [Test]
        public void TestGrabCut1()
        {
            Image<Bgr, Byte> img = EmguAssert.LoadImage<Bgr, Byte>("lena.jpg");

            Rectangle rect = new Rectangle(new Point(50, 50), new Size(400, 400));
            Matrix<double> bgdModel = new Matrix<double>(1, 13 * 5);
            Matrix<double> fgdModel = new Matrix<double>(1, 13 * 5);
            Image<Gray, byte> mask = new Image<Gray, byte>(img.Size);

            CvInvoke.GrabCut(img, mask, rect, bgdModel, fgdModel, 0, Emgu.CV.CvEnum.GrabcutInitType.InitWithRect);
            CvInvoke.GrabCut(img, mask, rect, bgdModel, fgdModel, 2, Emgu.CV.CvEnum.GrabcutInitType.Eval);
            using (ScalarArray ia = new ScalarArray(3))
                CvInvoke.Compare(mask, ia, mask, CvEnum.CmpType.Equal);
            //Emgu.CV.UI.ImageViewer.Show(img.ConcateHorizontal( mask.Convert<Bgr, Byte>()));
        }

        [Test]
        public void TestGrabCut2()
        {
            Image<Bgr, Byte> img = EmguAssert.LoadImage<Bgr, Byte>("pedestrian.png");
            HOGDescriptor desc = new HOGDescriptor();
            desc.SetSVMDetector(HOGDescriptor.GetDefaultPeopleDetector());

            MCvObjectDetection[] humanRegions = desc.DetectMultiScale(img);

            Image<Gray, byte> pedestrianMask = new Image<Gray, byte>(img.Size);
            foreach (MCvObjectDetection rect in humanRegions)
            {
                //generate the mask where 3 indicates foreground and 2 indicates background 
                using (Image<Gray, byte> mask = img.GrabCut(rect.Rect, 2))
                {
                    //get the mask of the foreground
                    using (ScalarArray ia = new ScalarArray(3))
                        CvInvoke.Compare(mask, ia, mask, Emgu.CV.CvEnum.CmpType.Equal);

                    pedestrianMask._Or(mask);
                }
            }
        }

        [Test]
        public void TestGraySingleImage()
        {
            Image<Gray, Single> img = new Image<Gray, float>(320, 480);
            img.SetRandUniform(new MCvScalar(), new MCvScalar(255));
            Image<Gray, Byte> mask = img.Cmp(100, CvEnum.CmpType.GreaterEqual);
            int[] count = mask.CountNonzero();
            int c = count[0];
        }

        [Test]
        public void TestDiatanceTransform()
        {
            Image<Gray, Byte> img = new Image<Gray, byte>(480, 320);
            img.Draw(new Rectangle(200, 100, 160, 90), new Gray(255), 1);
            img._Not();
            Image<Gray, Single> dst = new Image<Gray, Single>(img.Size);

            CvInvoke.DistanceTransform(img, dst, null, Emgu.CV.CvEnum.DistType.L2, 3);
        }

        /*
        [Test]
        public void TestAdaptiveSkinDetector()
        {
           Image<Bgr, Byte> image = EmguAssert.LoadImage<Bgr, Byte>("lena.jpg");
           using (AdaptiveSkinDetector detector = new AdaptiveSkinDetector(1, AdaptiveSkinDetector.MorphingMethod.ErodeDilate))
           {
              Image<Gray, Byte> mask = new Image<Gray, byte>(image.Size);
              detector.Process(image, mask);
              //mask._EqualizeHist();
              //ImageViewer.Show(mask);
           }
        }*/

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


#if !NETFX_CORE
        [Test]
        public void TestBinaryStorage()
        {
            //generate some randome points
            PointF[] pts = new PointF[120];
            GCHandle handle = GCHandle.Alloc(pts, GCHandleType.Pinned);
            using (Matrix<float> ptsMat = new Matrix<float>(pts.Length, 2, handle.AddrOfPinnedObject(), Marshal.SizeOf(typeof(float)) * 2))
            {
                ptsMat.SetRandNormal(new MCvScalar(), new MCvScalar(100));
            }
            handle.Free();

            String fileName = Path.Combine(Path.GetTempPath(), "tmp.dat");
            Stopwatch watch = Stopwatch.StartNew();
            BinaryFileStorage<PointF> stor = new BinaryFileStorage<PointF>(fileName, pts);
            //BinaryFileStorage<PointF> stor = new BinaryFileStorage<PointF>("abc.data", pts);
            watch.Stop();
            EmguAssert.WriteLine(String.Format("Time for writing {0} points: {1} milliseconds", pts.Length, watch.ElapsedMilliseconds));
            int estimatedSize = stor.EstimateSize();

            //EmguAssert.IsTrue(pts.Length == estimatedSize);

            watch.Reset();
            watch.Start();
            PointF[] pts2 = stor.ToArray();
            watch.Stop();
            EmguAssert.WriteLine(String.Format("Time for reading {0} points: {1} milliseconds", pts.Length, watch.ElapsedMilliseconds));

            if (File.Exists(fileName))
                File.Delete(fileName);

            //EmguAssert.IsTrue(pts.Length == pts2.Length);

            //Check for equality
            for (int i = 0; i < pts.Length; i++)
            {
                //EmguAssert.IsTrue(pts[i] == pts2[i]);
            }
        }
#endif

        /*
        [Test]
        public void TestSeqPerformance()
        {
           Point[] pts = new Point[100];

           using (MemStorage stor = new MemStorage())
           {
              Stopwatch watch = Stopwatch.StartNew();
              Seq<Point> seq = new Seq<Point>(stor);
              seq.PushMulti(pts, Emgu.CV.CvEnum.BackOrFront.Front);
              watch.Stop();
              EmguAssert.WriteLine(String.Format("Time for storing {0} points: {1} milliseconds", pts.Length, watch.ElapsedMilliseconds));

              watch.Reset();
              watch.Start();
              int counter = 0;
              foreach (Point p in seq)
              {
                 counter++;
              }
              watch.Stop();
              EmguAssert.WriteLine(String.Format("Time for reading {0} points: {1} milliseconds", pts.Length, watch.ElapsedMilliseconds));

           }
        }


        [Test]
        public void TestCondensation()
        {
           IntPtr conden = CvInvoke.cvCreateConDensation(5, 5, 100);
           CvInvoke.cvReleaseConDensation(ref conden);
        }*/

#if !NETFX_CORE
        public static String GetTempFileName()
        {
            string filename = Path.GetTempFileName();

            File.Delete(filename);

            return Path.Combine(Path.GetDirectoryName(filename), Path.GetFileNameWithoutExtension(filename));
        }

        [Test]
        public void TestVideoWriter()
        {
            int numberOfFrames = 10;
            int width = 300;
            int height = 200;
            String fileName = GetTempFileName() + ".avi";

            Image<Bgr, Byte>[] images = new Image<Bgr, byte>[numberOfFrames];
            for (int i = 0; i < images.Length; i++)
            {
                images[i] = new Image<Bgr, byte>(width, height);
                images[i].SetRandUniform(new MCvScalar(), new MCvScalar(255, 255, 255));
            }

            //using (VideoWriter writer = new VideoWriter(fileName, VideoWriter.Fourcc('H', '2', '6', '4'), 5, new Size(width, height), true))
            using (VideoWriter writer = new VideoWriter(fileName, VideoWriter.Fourcc('M', 'J', 'P', 'G'), 5, new Size(width, height), true))
            //using (VideoWriter writer = new VideoWriter(fileName, VideoWriter.Fourcc('X', '2', '6', '4'), 5, new Size(width, height), true))
            //using (VideoWriter writer = new VideoWriter(fileName, -1, 5, new Size( width, height ), true))
            {
                EmguAssert.IsTrue(writer.IsOpened);
                for (int i = 0; i < numberOfFrames; i++)
                {
                    writer.Write(images[i].Mat);
                }
            }

            FileInfo fi = new FileInfo(fileName);
            EmguAssert.IsTrue(fi.Exists && fi.Length != 0, "File should not be empty");

            using (VideoCapture capture = new VideoCapture(fileName))
            {
                Mat img2 = capture.QueryFrame();
                int count = 0;
                while (img2 != null && !img2.IsEmpty)
                {
                    EmguAssert.IsTrue(img2.Width == width);
                    EmguAssert.IsTrue(img2.Height == height);
                    //Assert.IsTrue(img2.Equals( images[count]) );
                    img2 = capture.QueryFrame();
                    count++;
                }
                EmguAssert.IsTrue(numberOfFrames == count);
            }
            File.Delete(fi.FullName);
        }

        [Test]
        public void TestVideoWriterMSMF()
        {
            Backend[] backends = CvInvoke.WriterBackends;
            int backend_idx = 0; //any backend;
            foreach (Backend be in backends)
            {
                if (be.Name.Equals("MSMF"))
                {
                    backend_idx = be.ID;
                    break;
                }
            }

            if (backend_idx > 0) //if MSMF back-end is found
            {
                int numberOfFrames = 100;
                int width = 640;
                int height = 480;
                String fileName = GetTempFileName() + ".mp4";
                //String fileName = "C:\\tmp\\out.mp4";
                Image<Bgr, Byte>[] images = new Image<Bgr, byte>[numberOfFrames];
                for (int i = 0; i < images.Length; i++)
                {
                    images[i] = new Image<Bgr, byte>(width, height);
                    images[i].SetRandUniform(new MCvScalar(), new MCvScalar(255, 255, 255));
                }

                using (VideoWriter writer = new VideoWriter(
                        fileName,
                        backend_idx,
                        VideoWriter.Fourcc('H', '2', '6', '4'),
                        29.97,
                        new Size(width, height),
                        true))
                {
                    double quality = writer.Get(VideoWriter.WriterProperty.Quality);
                    bool writeSucess = writer.Set(VideoWriter.WriterProperty.Quality, 100);
                    EmguAssert.IsTrue(writer.IsOpened);
                    for (int i = 0; i < numberOfFrames; i++)
                    {
                        writer.Write(images[i].Mat);
                    }
                }

                FileInfo fi = new FileInfo(fileName);
                EmguAssert.IsTrue(fi.Exists && fi.Length != 0, "File should not be empty");

                using (VideoCapture capture = new VideoCapture(fileName))
                {
                    Mat img2 = capture.QueryFrame();
                    int count = 0;
                    while (img2 != null && !img2.IsEmpty)
                    {
                        EmguAssert.IsTrue(img2.Width == width);
                        EmguAssert.IsTrue(img2.Height == height);
                        //Assert.IsTrue(img2.Equals( images[count]) );
                        img2 = capture.QueryFrame();
                        count++;
                    }

                    EmguAssert.IsTrue(numberOfFrames == count);
                }
                Trace.WriteLine(String.Format("File {0} size: {1}", fi.FullName, fi.Length));
                File.Delete(fi.FullName);
            }
        }
#endif

        /*
#if !ANDROID
        //took too long to test on android, disabling for now
        [Test]
        public void TestRTreeClassifier()
        {
           using (Image<Bgr, Byte> image = EmguAssert.LoadImage<Bgr, Byte>("box_in_scene.png"))
           using (Image<Gray, Byte> gray = image.Convert<Gray, byte>())
           using (RTreeClassifier<Bgr> classifier = new RTreeClassifier<Bgr>())
           {
              SURF surf = new SURF(300);
              MKeyPoint[] keypoints = surf.Detect(gray, null);
              Point[] points = Array.ConvertAll<MKeyPoint, Point>(keypoints, delegate(MKeyPoint kp) {
                 return Point.Round(kp.Point); });
              Stopwatch watch = Stopwatch.StartNew();
              classifier.Train(image, points, 48, 9, 50, 176, 4);
              watch.Stop();
              EmguAssert.WriteLine(String.Format("Training time: {0} milliseconds", watch.ElapsedMilliseconds));
              float[] signiture = classifier.GetSigniture(image, points[0], 15);
              EmguAssert.IsTrue(signiture.Length == classifier.NumberOfClasses);
           }
        }
#endif
        */

        [Test]
        public void TestIndex3D()
        {
            Random r = new Random();
            MCvPoint3D32f[] points = GetRandom(10000, 0, 100, r);

            MCvPoint3D32f[] searchPoints = GetRandom(10, 0, 100, r);

            int indexOfClosest1 = 0;
            double shortestDistance1 = double.MaxValue;
            for (int i = 0; i < points.Length; i++)
            {
                double dist = (searchPoints[0] - points[i]).Norm;
                if (dist < shortestDistance1)
                {
                    shortestDistance1 = dist;
                    indexOfClosest1 = i;
                }
            }
            using (Flann.KdTreeIndexParams p = new KdTreeIndexParams())
            using (Flann.Index3D index3D = new Emgu.CV.Flann.Index3D(points, p))
            {

                double shortestDistance2;
                Index3D.Neighbor n = index3D.NearestNeighbor(searchPoints[0]);
                shortestDistance2 = Math.Sqrt(n.SquareDist);

                //EmguAssert.IsTrue(indexOfClosest1 == n.Index);
                //EmguAssert.IsTrue((shortestDistance1 - shortestDistance2) <= 1.0e-3 * shortestDistance1);

                var neighbors = index3D.RadiusSearch(searchPoints[0], 100, 10);
            }
        }

        private static MCvPoint3D32f[] GetRandom(int count, float min, float max, Random r = null)
        {
            if (r == null)
                r = new Random();

            MCvPoint3D32f[] features = new MCvPoint3D32f[count];
            for (int i = 0; i < features.Length; i++)
            {
                MCvPoint3D32f p = new MCvPoint3D32f();

                p.X = (float)(r.NextDouble() * (max - min)) + min;
                p.Y = (float)(r.NextDouble() * (max - min)) + min;
                p.Z = (float)(r.NextDouble() * (max - min)) + min;

                features[i] = p;
            }

            return features;
        }



        /*
        [Test]
        public void TestPOSIT()
        {
           float[,] points = new float[4, 3] { { 0, 0, 0 }, { 1, 1, 0 }, { 1, 0, 1 }, { 0, 1, 1 } };
           IntPtr ptr = CvInvoke.cvCreatePOSITObject(points, 4);
           CvInvoke.cvReleasePOSITObject(ref ptr);
        }


        [Test]
        public void TestTiffWriter()
        {
           Image<Bgr, Byte> image = new Image<Bgr, byte>(320, 240);
           Size tileSize = new Size(32, 32);
           String fileName = Path.Combine(Path.GetTempPath(), "temp.tiff");
           using (TileTiffWriter<Bgr, Byte> writer = new TileTiffWriter<Bgr, byte>(fileName, image.Size, tileSize))
           {
              EmguAssert.IsTrue(tileSize == writer.TileSize, "Tile size not equals");
              writer.WriteImage(image);
           }
           if (File.Exists(fileName))
              File.Delete(fileName);
        }*/

        [Test]
        public void TestDataLogger()
        {
            bool dataLogged = false;

            using (DataLogger<String> logger = new DataLogger<String>(1))
            {
                logger.OnDataReceived +=
                     delegate (object sender, EventArgs<string> e)
                {
                    EmguAssert.AreEqual(e.Value, "Test");
                    dataLogged = true;
                };

                logger.Log("Test", 0);
                EmguAssert.IsFalse(dataLogged);

                logger.Log("Test", 1);

                EmguAssert.IsTrue(dataLogged);
            }
        }

        [Test]
        public void Test_VectorOfFloat()
        {
            VectorOfFloat vf = new VectorOfFloat();
            float[] values = new float[20];
            for (int i = 0; i < values.Length; i++)
                values[i] = i;
            vf.Push(values);
            float[] valuesCopy = vf.ToArray();
            for (int i = 0; i < values.Length; i++)
                EmguAssert.AreEqual(values[i], valuesCopy[i]);
        }

#if !ANDROID
        [Test]
        public void TestCaptureFromFile()
        {
            using (VideoCapture capture = new VideoCapture(EmguAssert.GetFile("tree.avi")))
            using (VideoWriter writer = new VideoWriter("tree_invert.avi", 10, new Size(capture.Width, capture.Height), true))
            {
                int maxCount = 10;
                Mat img = new Mat();
                while (capture.Grab() && maxCount > 0)
                {
                    capture.Retrieve(img);
                    CvInvoke.BitwiseNot(img, img);
                    writer.Write(img);
                    maxCount--;
                }
            }
        }
#endif

        [Test]
        public void TestDrawMarker()
        {
            Mat m = new Mat(new Size(640, 480), DepthType.Cv8U, 3);
            m.SetTo(new MCvScalar(0, 0, 0));
            CvInvoke.DrawMarker(m, new Point(200, 200), new MCvScalar(255.0, 255.0, 0), MarkerTypes.Diamond);
            //Emgu.CV.UI.ImageViewer.Show(m);
        }

        [Test]
        public void TestRotationMatrix()
        {
            Size dstImageSize;
            using (RotationMatrix2D rotationMatrix = RotationMatrix2D.CreateRotationMatrix(new System.Drawing.PointF(320, 240), -90, new Size(640, 480), out dstImageSize))
            using (Matrix<double> m = new Matrix<double>(2, 3))
            {
                rotationMatrix.CopyTo(m);
                System.Diagnostics.Debug.WriteLine("emgu.cv.test", String.Format("dstSize: {0}x{1}", dstImageSize.Width, dstImageSize.Height));
                System.Diagnostics.Debug.WriteLine("emgu.cv.test", String.Format("rotationMat: [ [{0}, {1}, {2}], [{3}, {4}, {5}] ]", m.Data[0, 0], m.Data[0, 1], m.Data[0, 2], m.Data[1, 0], m.Data[1, 1], m.Data[1, 2]));
            }
        }

#if !NETFX_CORE
        [Test]
        public void TestImageDecodeBuffer()
        {
            using (FileStream fs = File.OpenRead(EmguAssert.GetFile("lena.jpg")))
            {
                byte[] data = new byte[fs.Length];
                fs.Read(data, 0, (int)fs.Length);

                Mat image = new Mat();

                CvInvoke.Imdecode(data, ImreadModes.Color, image);
                //Emgu.CV.UI.ImageViewer.Show(image);
            }
        }

        [Test]
        public void TestImdecodeGray()
        {

            using (FileStream fs = File.OpenRead(EmguAssert.GetFile("lena.jpg")))
            {
                byte[] data = new byte[fs.Length];
                fs.Read(data, 0, (int)fs.Length);

                Mat image = new Mat();

                CvInvoke.Imdecode(data, ImreadModes.Grayscale, image);
                EmguAssert.IsTrue(image.NumberOfChannels == 1);
            }
        }

        [TestAttribute]
        public void TestImreadmulti()
        {
            Mat[] mats = CvInvoke.Imreadmulti(EmguAssert.GetFile("multipage.tif"));
        }

#endif

        [Test]
        public void TestVectorOfVector()
        {
            int[][] v0 = new int[][]
            {
            new int[] {1, 2, 3},
            new int[] {4, 5},
            new int[] {6}
            };

            using (VectorOfVectorOfInt v = new VectorOfVectorOfInt(v0))
            {
                int[][] v1 = v.ToArrayOfArray();

                for (int i = 0; i < v0.Length; i++)
                {
                    int[] a = v0[i];
                    int[] b = v1[i];
                    for (int j = 0; j < a.Length; j++)
                    {
                        EmguAssert.IsTrue(a[j] == b[j]);
                    }
                }
            }

        }

        [Test]
        public void TestRotationMatrix2D()
        {
            double angle = 32;
            Size size = new Size(960, 480);
            PointF center = new PointF(size.Width * 0.5f, size.Height * 0.5f);
            using (RotationMatrix2D rotationMatrix = new RotationMatrix2D(center, -angle, 1))
            {
                PointF[] corners = new PointF[] {
                  new PointF(0, 0),
                  new PointF(size.Width - 1, 0),
                  new PointF(size.Width - 1, size.Height - 1),
                  new PointF(0, size.Height - 1)};
                PointF[] oldCorners = new PointF[corners.Length];
                corners.CopyTo(oldCorners, 0);

                rotationMatrix.RotatePoints(corners);

                //Mat transformation = CvInvoke.EstimateRigidTransform(oldCorners, corners, true);
                Mat transformation = CvInvoke.EstimateAffine2D(oldCorners, corners);

                Matrix<double> delta = new Matrix<double>(transformation.Size);
                CvInvoke.AbsDiff(rotationMatrix, transformation, delta);
                double min = 0, max = 0;
                Point minLoc = new Point(), maxLoc = new Point();
                CvInvoke.MinMaxLoc(delta, ref min, ref max, ref minLoc, ref maxLoc, null);

                double min2, max2;
                int[] minLoc2 = new int[2], maxLoc2 = new int[2];
                CvInvoke.MinMaxIdx(delta, out min2, out max2, minLoc2, maxLoc2, null);
                EmguAssert.IsTrue(min == min2);
                EmguAssert.IsTrue(max == max2);
                EmguAssert.IsTrue(minLoc.X == minLoc2[1]);
                EmguAssert.IsTrue(minLoc.Y == minLoc2[0]);
                EmguAssert.IsTrue(maxLoc.X == maxLoc2[1]);
                EmguAssert.IsTrue(maxLoc.Y == maxLoc2[0]);

                EmguAssert.IsTrue(max < 1.0e-4, String.Format("Error {0} is too large. Expected to be less than 1.0e-4", max));
            }
        }

        /*
        [Test]
        public void Test_MPEG_4_2_Codec()
        {
           if (IntPtr.Size == 4) //Only perform the test in 32bit mode
           {
              String fileName = Path.Combine(Path.GetTempPath(), "tmp.avi");
              using (Image<Gray, Byte> img = new Image<Gray, byte>(480, 320))
              using (VideoWriter writer = new VideoWriter(fileName, CvInvoke.CV_FOURCC('M', 'P', '4', '2'), 10, 480, 320, false))
              {
                 writer.WriteFrame(img);
              }
              if (File.Exists(fileName))
                 File.Delete(fileName);
           }
        }*/

        [Test]
        public void TestRetina()
        {

            Image<Bgr, byte> image = EmguAssert.LoadImage<Bgr, Byte>("pedestrian.png");
            using (Retina retina = new Retina(
                new Size(image.Width, image.Height),
                true,
                Retina.ColorSamplingMethod.ColorBayer, false, 1.0, 10.0))
            {
                Retina.RetinaParameters p = retina.Parameters;
                Retina.IplMagnoParameters iplP = p.IplMagno;
                float oldval = iplP.ParasolCellsK;
                iplP.ParasolCellsK += 0.01f;
                iplP.NormaliseOutput = false;
                p.IplMagno = iplP;
                retina.Parameters = p;
                float newval = retina.Parameters.IplMagno.ParasolCellsK;

                Assert.AreEqual(newval, oldval + 0.01f);

                retina.Run(image);
                Mat out1 = new Mat();
                Mat out2 = new Mat();
                retina.GetMagno(out1);
                retina.GetParvo(out2);
            }
        }

        [Test]
        public void TestPyrMeanshiftSegmentation()
        {
            Image<Bgr, byte> image = EmguAssert.LoadImage<Bgr, Byte>("pedestrian.png");
            Image<Bgr, Byte> result = new Image<Bgr, byte>(image.Size);
            CvInvoke.PyrMeanShiftFiltering(image, result, 10, 20, 1, new MCvTermCriteria(5, 1));
            //Image<Gray, Byte> hue = result.Convert<Hsv, Byte>()[0];
            Image<Gray, Byte> hue = result.Convert<Gray, Byte>().Canny(30, 20);

            //ImageViewer.Show(image.ConcateHorizontal( result ).ConcateVertical(hue.Convert<Bgr, Byte>()));
        }

        [Test]
        public void TestStitching1()
        {
            Image<Bgr, Byte>[] images = new Image<Bgr, byte>[4];

            images[0] = EmguAssert.LoadImage<Bgr, Byte>("stitch1.jpg");
            images[1] = EmguAssert.LoadImage<Bgr, Byte>("stitch2.jpg");
            images[2] = EmguAssert.LoadImage<Bgr, Byte>("stitch3.jpg");
            images[3] = EmguAssert.LoadImage<Bgr, Byte>("stitch4.jpg");

            using (Stitcher stitcher = new Stitcher())
            {
                Mat result = new Mat();
                using (VectorOfMat vm = new VectorOfMat())
                {
                    vm.Push(images);
                    stitcher.Stitch(vm, result);
                }
                //Emgu.CV.UI.ImageViewer.Show(result);
            }
        }

        [Test]
        public void TestStitching2()
        {
            Image<Bgr, Byte>[] images = new Image<Bgr, byte>[4];

            images[0] = EmguAssert.LoadImage<Bgr, Byte>("stitch1.jpg");
            images[1] = EmguAssert.LoadImage<Bgr, Byte>("stitch2.jpg");
            images[2] = EmguAssert.LoadImage<Bgr, Byte>("stitch3.jpg");
            images[3] = EmguAssert.LoadImage<Bgr, Byte>("stitch4.jpg");

            using (Stitcher stitcher = new Stitcher())
            using (ORBDetector finder = new ORBDetector())
            {
                stitcher.SetFeaturesFinder(finder);
                Mat result = new Mat();
                using (VectorOfMat vm = new VectorOfMat())
                {
                    vm.Push(images);
                    stitcher.Stitch(vm, result);
                }
                //Emgu.CV.UI.ImageViewer.Show(result);
            }
        }

        [Test]
        public void TestStitching3()
        {
            Mat[] images = new Mat[4];

            images[0] = EmguAssert.LoadMat("stitch1.jpg");
            images[1] = EmguAssert.LoadMat("lena.jpg");
            images[2] = EmguAssert.LoadMat("dog416.png");
            images[3] = EmguAssert.LoadMat("pedestrian.png");

            using (Stitcher stitcher = new Stitcher())
            //using (OrbFeaturesFinder finder = new OrbFeaturesFinder(new Size(3, 1)))
            {
                //stitcher.SetFeaturesFinder(finder);
                Mat result = new Mat();
                using (VectorOfMat vm = new VectorOfMat())
                {
                    vm.Push(images);
                    stitcher.Stitch(vm, result);
                }
                //Emgu.CV.UI.ImageViewer.Show(result);
            }
        }

        [Test]
        public void TestStitching4()
        {
            Mat[] images = new Mat[1];

            images[0] = EmguAssert.LoadMat("stitch1.jpg");


            using (Stitcher stitcher = new Stitcher())
            //using (OrbFeaturesFinder finder = new OrbFeaturesFinder(new Size(3, 1)))
            {
                //stitcher.SetFeaturesFinder(finder);
                Mat result = new Mat();
                using (VectorOfMat vm = new VectorOfMat())
                {
                    vm.Push(images);
                    stitcher.Stitch(vm, result);
                }
                //Emgu.CV.UI.ImageViewer.Show(result);
            }
        }

        [Test]
        public void TestStitching5()
        {
            Mat[] images = new Mat[4];

            images[0] = EmguAssert.LoadMat("stitch1.jpg");
            images[1] = EmguAssert.LoadMat("stitch2.jpg");
            images[2] = EmguAssert.LoadMat("stitch3.jpg");
            images[3] = EmguAssert.LoadMat("stitch4.jpg");

            using (Stitcher stitcher = new Stitcher(Stitcher.Mode.Panorama))
            using (ORBDetector detector = new ORBDetector())
            using (SphericalWarper warper = new SphericalWarper())
            using (SeamFinder finder = new GraphCutSeamFinder())
            using (BlocksChannelsCompensator compensator = new BlocksChannelsCompensator())
            using (FeatherBlender blender = new FeatherBlender())
            {
                stitcher.SetFeaturesFinder(detector);
                stitcher.SetWarper(warper);
                stitcher.SetSeamFinder(finder);
                stitcher.SetExposureCompensator(compensator);
                stitcher.SetBlender(blender);

                Mat result = new Mat();
                using (VectorOfMat vm = new VectorOfMat())
                {
                    vm.Push(images);
                    stitcher.Stitch(vm, result);
                }

                //Emgu.CV.UI.ImageViewer.Show(result);
            }
        }

        /*
        [Test]
        public void TestStitching5()
        {
           Mat[] images = new Mat[2];

           images[0] = new Mat();
           images[1] = new Mat();

           using (Stitcher stitcher = new Stitcher(false))
           //using (OrbFeaturesFinder finder = new OrbFeaturesFinder(new Size(3, 1)))
           {
              //stitcher.SetFeaturesFinder(finder);
              Mat result = new Mat();
              using (VectorOfMat vm = new VectorOfMat())
              {
                 vm.Push(images);
                 stitcher.Stitch(vm, result);
              }
              //Emgu.CV.UI.ImageViewer.Show(result);
           }
        }*/

        [Test]
        public void TestEstimateAffine3D()
        {
            Random r = new Random();
            Matrix<double> affine = new Matrix<double>(3, 4);
            for (int i = 0; i < 3; i++)
                for (int j = 0; j < 4; j++)
                {
                    affine.Data[i, j] = r.NextDouble() * 2 + 1;
                }

            MCvPoint3D32f[] srcPts = new MCvPoint3D32f[4];
            for (int i = 0; i < srcPts.Length; i++)
            {
                srcPts[i] = new MCvPoint3D32f((float)(r.NextDouble() + 1), (float)(r.NextDouble() + 1), (float)(r.NextDouble() + 5));
            }
            MCvPoint3D32f[] dstPts = new MCvPoint3D32f[srcPts.Length];

            GCHandle srcHandle = GCHandle.Alloc(srcPts, GCHandleType.Pinned);
            GCHandle dstHandle = GCHandle.Alloc(dstPts, GCHandleType.Pinned);
            using (Matrix<float> srcMat = new Matrix<float>(srcPts.Length, 1, 3, srcHandle.AddrOfPinnedObject(), Marshal.SizeOf(typeof(MCvPoint3D32f))))
            using (Matrix<float> dstMat = new Matrix<float>(dstPts.Length, 1, 3, dstHandle.AddrOfPinnedObject(), Marshal.SizeOf(typeof(MCvPoint3D32f))))
            {
                CvInvoke.Transform(srcMat, dstMat, affine);
            }
            srcHandle.Free();
            dstHandle.Free();

            byte[] inlier;
            Matrix<double> estimate;
            CvInvoke.EstimateAffine3D(srcPts, dstPts, out estimate, out inlier, 3, 0.99);
        }

        /*
#region Test code contributed by Daniel Bell, modified by Canming
        [Test]
        public void TestLevMarqSparse()
        {
           int N = 11;
           int pN = 20;
           MCvPoint3D64f[] points = new MCvPoint3D64f[pN];

           MCvPoint2D64f[][] imagePoints = new MCvPoint2D64f[N][];
           int[][] visibility = new int[N][];
           List<Matrix<double>> cameraMatrix = new List<Matrix<double>>();
           List<Matrix<double>> R = new List<Matrix<double>>();
           List<Matrix<double>> T = new List<Matrix<double>>();
           List<Matrix<double>> distcoeff = new List<Matrix<double>>();
           MCvTermCriteria termCrit = new MCvTermCriteria(30, 1.0e-12);

           Size cameraRes = new Size(640, 480);
           Matrix<float> pMatrix = new Matrix<float>(3, pN);
           Matrix<float> imMatrix = new Matrix<float>(2, pN);
           Matrix<int> visMatrix = new Matrix<int>(1, pN);
           Matrix<double> RMatrix = new Matrix<double>(3, 3);
           Matrix<double> TMatrix = new Matrix<double>(3, 1);
           Matrix<double> dcMatrix = new Matrix<double>(4, 1);
           Matrix<double> camMatrix = new Matrix<double>(new double[,] { { cameraRes.Width / 4, 0, cameraRes.Width / 2 }, { 0, cameraRes.Height / 4, cameraRes.Height / 2 }, { 0, 0, 1 } });
           dcMatrix.SetZero();

           for (int y = -pN / 2; y < pN / 2; y += 1)
           {
              double r = 4 + Math.Sin(3 * Math.PI * y / 10);
              for (double theta = 0; theta <= 2 * Math.PI; theta += Math.PI / 4)
              {
                 double px = r * Math.Cos(theta) + y; //+ rng.gaussian(1)
                 double py = 5 - y; //+ rng.gaussian(1)
                 double pz = r * Math.Sin(theta) + y; //+ rng.gaussian(1)

                 points[y + pN / 2] = new MCvPoint3D64f(px, py, pz);
              }
           }
           for (int i = 0; i < N; i++)
           {
              visMatrix = new Matrix<int>(1, pN);
              cameraMatrix.Add(camMatrix);
              double[,] r = new double[,] { { Math.Cos(i * 2 * Math.PI / N), 0, Math.Sin(i * 2 * Math.PI / N) }, { 0, 1, 0 }, { -Math.Sin(i * 2 * Math.PI / N), 0, Math.Cos(i * 2 * Math.PI / N) } };
              double[] t = new double[] { 0, 0, 30 };
              R.Add(new Matrix<double>(r));
              T.Add(new Matrix<double>(t));
              distcoeff.Add(dcMatrix.Clone());
           }

           Random ran = new Random();
           for (int i = 0; i < N; i++)
           {
              imagePoints[i] = new MCvPoint2D64f[pN];
              visibility[i] = new int[pN];
              // check if the point is in cameras
              for (int j = 0; j < pN; j++)
              {
                 // if the image point is within camera resolution then the point is visible
                 if ((0 <= imagePoints[i][j].X) && (imagePoints[i][j].X <= cameraRes.Height) &&
                     (0 <= imagePoints[i][j].Y) && (imagePoints[i][j].Y <= cameraRes.Width))
                 {  // add randomness	
                    // perturbate
                    visibility[i][j] = 1;
                    imagePoints[i][j] = new MCvPoint2D64f((float)points[i].X + ran.Next(0, 3), (float)points[i].Y + ran.Next(0, 3));
                 }
                 // else, the point is not visible 
                 else
                 {
                    visibility[i][j] = 0;
                    imagePoints[i][j] = new MCvPoint2D64f(-1, -1);
                 }
              }
           }

           MCvPoint3D64f[] oldPoints = new MCvPoint3D64f[points.Length];
           Array.Copy(points, oldPoints, points.Length);
           Matrix<double> cameraMatOld = cameraMatrix[0].Clone();
           Matrix<double> rotationMatOld = R[0].Clone();
           Matrix<double> translationMatOld = T[0].Clone();
           Matrix<double> distcoeffOld = distcoeff[0].Clone();
           LevMarqSparse.BundleAdjust(points, imagePoints, visibility, cameraMatrix.ToArray(), R.ToArray(), T.ToArray(), distcoeff.ToArray(), termCrit);

        }
#endregion

        [Test]
        public void TestLatenSVM2()
        {
           using (LatentSvmDetector detector = new LatentSvmDetector(new string[] {EmguAssert.GetFile( "car.xml" )}))
           {
              String[] files = new String[] { 
                 "license-plate.jpg"};

              for (int idx = 0; idx < files.Length; idx++)
                 using (Mat img = EmguAssert.LoadMat(files[idx]))
                 {
                    MCvObjectDetection[] results = detector.Detect(img, 0.5f);
                    if (results.Length >= 0)
                    {
                       double maxScore = results[0].Score;
                       Rectangle result = results[0].Rect;

                       for (int i = 1; i < results.Length; ++i)
                       {
                          if (results[i].Score > maxScore)
                          {
                             maxScore = results[i].Score;
                             result = results[i].Rect;
                          }
                       }
                       CvInvoke.Rectangle(img, result, new MCvScalar(0, 0, 255));
                       Emgu.CV.UI.ImageViewer.Show(img);

                       result.Inflate((int)(result.Width * 0.2), (int)(result.Height * 0.2));
                       using (Image<Gray, Byte> mask = img.GrabCut(result, 10))
                       using (Image<Gray, Byte> canny = img.Canny(120, 80))
                       {
                          //MCvFont f = new MCvFont(CvEnum.FontFace.HersheyComplex, 2.0, 2.0);
                          using (ScalarArray ia = new ScalarArray(3))
                             CvInvoke.Compare(mask, ia, mask, CvEnum.CmpType.NotEqual);
                          //CvInvoke.Set(canny, new MCvScalar(), mask);
                          canny.SetValue(new MCvScalar(), mask);
                          canny.Draw(@"http://www.emgu.com", new Point(50, 50), CvEnum.FontFace.HersheyComplex, 2.0, new Gray(255), 2);

                          CvInvoke.BitwiseNot(canny, canny, null);

                          Image<Bgr, byte> displayImg = img.ConcateHorizontal(canny.Convert<Bgr, Byte>());

                          //displayImg.Save("out_" + files[idx]);
                          Emgu.CV.UI.ImageViewer.Show(displayImg);
                       }
                    }
                 }
           }
        }

        [Test]
        public void TestDataMatrix()
        {
           using (Image<Gray, Byte> img = new Image<Gray, byte>("qrcode.png"))
           using (Emgu.CV.Util.VectorOfDataMatrixCode v = new VectorOfDataMatrixCode())
           {
              //ImageViewer.Show(img);
              v.Find(img);
              v.Draw(img);
              //ImageViewer.Show(img);
           }
        }

        [Test]
        public void TestChamferMatching()
        {
           using (Image<Gray, Byte> logo = EmguAssert.LoadImage<Gray, Byte>("logo.png"))
           using (Image<Gray, Byte> logoInClutter = EmguAssert.LoadImage<Gray, Byte>("logo_in_clutter.png"))
           using (Image<Bgr, Byte> image = logoInClutter.Convert<Bgr, Byte>())
           {
              Point[][] contours;
              float[] costs;
              int count = CvInvoke.ChamferMatching(logoInClutter.Mat, logo.Mat, out contours, out costs, 1, 1, 1, 3, 3, 5, 0.6, 1.6, 0.5, 20);

              foreach (Point[] contour in contours)
              {
                 foreach (Point p in contour)
                 {
                    image[p] = new Bgr(Color.Red);
                 }
              }
              //Emgu.CV.UI.ImageViewer.Show(image);
           }
        }

        [Test]
        public void TestLatenSVM()
        {
           using (LatentSvmDetector detector = new LatentSvmDetector(new string[] { EmguAssert.GetFile("cat.xml") }))
           {
              String[] files = new String[] { "cat.png"};

              for (int idx = 0; idx < files.Length; idx++)
                 using (Mat img = EmguAssert.LoadMat(files[idx]))
                 {
                    MCvObjectDetection[] results = detector.Detect(img, 0.5f);
                    if (results.Length >= 0)
                    {
                       double maxScore = results[0].Score;
                       Rectangle result = results[0].Rect;

                       for (int i = 1; i < results.Length; ++i)
                       {
                          if (results[i].Score > maxScore)
                          {
                             maxScore = results[i].Score;
                             result = results[i].Rect;
                          }
                       }
                       CvInvoke.Rectangle(img, result, new MCvScalar(0, 0, 255));
                       //Emgu.CV.UI.ImageViewer.Show(img);

                    }
                 }
           }
        }*/

#if !NETFX_CORE
        [Test]
        public void TestFileCaptureNonAscii()
        {
            String fileName = EmguAssert.GetFile("tree.avi");
            String newName = fileName.Replace("tree.avi", "树.avi");
            File.Copy(fileName, newName, true);
            VideoCapture capture = new VideoCapture(EmguAssert.GetFile(newName));
            int counter = 0;
            using (Mat m = new Mat())
                while (capture.Grab())
                {
                    capture.Retrieve(m);
                    counter++;
                }

            Trace.WriteLine(String.Format("{0} frames found in file {1}", counter, newName));
        }

        //TODO: Check why this fails again
        [Test]
        public void TestFileCapturePause()
        {

            int totalFrames1 = 0;
            String fileName = EmguAssert.GetFile("tree.avi");
            String fileName2 = fileName.Replace("tree.avi", "tree2.avi");
            File.Copy(fileName, fileName2, true);

            VideoCapture capture1 = new VideoCapture(fileName);

            //capture one will continute capturing all the frames.
            EventHandler captureHandle1 = delegate
            {
                Mat img = new Mat();
                capture1.Retrieve(img);
                totalFrames1++;
                Trace.WriteLine(String.Format("capture 1 frame {0}: {1}", totalFrames1, DateTime.Now.ToString()));
            };
            capture1.ImageGrabbed += captureHandle1;
            capture1.Start();

            System.Threading.Thread.Sleep(2);



            int totalFrames2 = 0;
            VideoCapture capture2 = new VideoCapture(fileName2);
            int counter = 0;
            //capture 2 will capture 2 frames, pause for 1 seconds, then continute;
            EventHandler captureHandle = delegate
            {
                counter++;
                totalFrames2++;

                bool needPause = (counter >= 2);
                if (needPause)
                {
                    capture2.Pause();
                    counter = 0;
                }

                Mat img = new Mat();
                capture2.Retrieve(img);
                Trace.WriteLine(String.Format("capture 2 frame {0}: {1}", totalFrames2, DateTime.Now.ToString()));

                if (needPause)
                {
                    System.Threading.ThreadPool.QueueUserWorkItem(delegate
                    {
                        Trace.WriteLine("Sleep for 1 sec");
                        System.Threading.Thread.Sleep(1000);
                        capture2.Start();
                    });
                }

            };

            capture2.ImageGrabbed += captureHandle;
            capture2.Start();


            //int totalFrames = 69;
            Stopwatch s = Stopwatch.StartNew();
            while (!(totalFrames1 == totalFrames2))
            {
                System.Threading.Thread.Sleep(1000);

                if (s.ElapsedMilliseconds > 120 * 1000)
                {
                    EmguAssert.IsTrue(false, "Unable to finished reading frames in 2 mins");
                    break;
                }
            }
            capture1.Dispose();
            capture2.Dispose();
        }

        /*
#if !(__IOS__ || __ANDROID__)
        [Test]
        public void TestGLImageView()
        {
           Emgu.CV.UI.GLView.GLImageViewer viewer = new UI.GLView.GLImageViewer();
           //viewer.ShowDialog();
        }
#endif
        */
#endif
        [Test]
        public void TestCvString()
        {
            string s = "From ? to ?";

            using (CvString str = new CvString(s))
            {
                string s2 = str.ToString();
                EmguAssert.IsTrue(s.Equals(s2));
            }
        }

        [Test]
        public void TestFloodFill()
        {
            Image<Gray, byte> img = EmguAssert.LoadImage<Bgr, Byte>("box.png").Convert<Gray, Byte>();
            Rectangle boundingRect;
            int area = CvInvoke.FloodFill(img,
                null,
                new Point(0, 0),
                new MCvScalar(255),
                out boundingRect,
                new MCvScalar(255),
                new MCvScalar(255),
                Connectivity.EightConnected,
                FloodFillType.Default);
            int bRectArea = boundingRect.Size.Width * boundingRect.Size.Height;
#if !NETFX_CORE
            Trace.WriteLine("Flooded area: " + area + ". Bounding rectangle area: " + bRectArea);
#endif
            EmguAssert.IsTrue(bRectArea != 0, "Area should not be 0");

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
            Mat frame = EmguAssert.LoadMat("lena.jpg");
            using (VectorOfVectorOfPoint contours = new VectorOfVectorOfPoint())
            using (Image<Gray, byte> canny = frame.ToImage<Gray, byte>())
            {
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
                            using (Matrix<int> value = new Matrix<int>(defects.Rows, defects.Cols, defects.NumberOfChannels))
                            {
                                defects.CopyTo(value);
                                //you can iterate through the defect here:
                                for (int j = 0; j < value.Rows; j++)
                                {
                                    int startIdx = value.Data[j, 0];
                                    int endIdx = value.Data[j, 1];
                                    int farthestPtIdx = value.Data[j, 2];
                                    double fixPtDepth = value.Data[j, 3] / 256.0;

                                }
                            }
                        }
                    }
                }
            }
        }

#if !(__IOS__ || NETFX_CORE)

        private static String[] ReadClassNames(String fileName)
        {
            return File.ReadAllLines(fileName);
        }

        /* Find best class for the blob (i. e. class with maximal probability) */
        private static void GetMaxClass(Mat probBlob, out int classId, out double classProb)
        {
            //Mat matRef = probBlob.MatRef();
            Mat probMat = probBlob.Reshape(1, 1); //reshape the blob to 1x1000 matrix
            Point minLoc = new Point(), maxLoc = new Point();
            double minVal = 0, maxVal = 0;
            CvInvoke.MinMaxLoc(probMat, ref minVal, ref maxVal, ref minLoc, ref maxLoc);
            classId = maxLoc.X;
            classProb = maxVal;
        }

        [Test]
        public void TestDnnFcn8sHeavyPascal()
        {
            //Dnn.Net net = new Dnn.Net();
            String caffeModelFile = "fcn8s-heavy-pascal.caffemodel";
            if (!File.Exists(caffeModelFile))
            {
                //Download the bvlc googlenet file
                String CaffeModelUrl = "http://dl.caffe.berkeleyvision.org/fcn8s-heavy-pascal.caffemodel";
                Trace.WriteLine("downloading file from:" + CaffeModelUrl + " to: " + caffeModelFile);
                System.Net.WebClient downloadClient = new System.Net.WebClient();
                try
                {
                    downloadClient.DownloadFile(CaffeModelUrl, caffeModelFile);
                }
                catch
                {
                    //delete file in case of failed download
                    File.Delete(caffeModelFile);
                    throw;
                }
            }
            Dnn.Net net = DnnInvoke.ReadNetFromCaffe("fcn8s-heavy-pascal.prototxt", caffeModelFile);


            Mat img = EmguAssert.LoadMat("street.png");
            //FCN accepts 500x500 RGB-images
            CvInvoke.Resize(img, img, new Size(500, 500));

            Mat inputBlob = DnnInvoke.BlobFromImage(img);
            net.SetInput(inputBlob, "data");
            Mat probBlob = net.Forward("score");
            IntPtr dataPtr = probBlob.DataPointer;

            int channels = probBlob.SizeOfDimension[1];
            int rows = probBlob.SizeOfDimension[2];
            int cols = probBlob.SizeOfDimension[3];
            //should be a 1 x 21 x 500 x 500 Mat, where 21 is the number of classes.
            float[,,] labels = new float[channels, rows, cols];
            GCHandle handle = GCHandle.Alloc(labels, GCHandleType.Pinned);
            Emgu.CV.Util.CvToolbox.Memcpy(dataPtr, handle.AddrOfPinnedObject(), sizeof(float) * probBlob.NumberOfChannels * probBlob.Rows * probBlob.Cols);
            handle.Free();
            //Marshal.Copy(dataPtr, labels, 0, labels.Length);
            byte[] imageData = new byte[3 * rows * cols];
            for (int r = 0; r < rows; r++)
                for (int c = 0; c < cols; c++)
                {
                    int l = 0;
                    float maxScore = 0;
                    for (int n = 0; n < channels; n++)
                    {
                        if (maxScore < labels[n, r, c])
                        {
                            maxScore = labels[n, r, c];
                            l = n;
                        }
                        int idx = 3 * (r * cols + c);
                        imageData[idx] = (byte)l;
                        imageData[idx + 1] = (byte)l;
                        imageData[idx + 2] = (byte)l;
                    }
                }

#if !NETFX_CORE
            //Trace.WriteLine("Best class: " + classNames[classId] + ". Probability: " + classProb);
#endif

        }

        /*
        [Test]
        public void TestDnnPersonDetection()
        {
            string modelXmlFile = "person-detection-retail-0013.xml";
            String modelFile = "person-detection-retail-0013.bin";
            if (!File.Exists(modelFile))
            {
                //Download the model file
                String modelUrl = "https://download.01.org/opencv/2020/openvinotoolkit/2020.3/open_model_zoo/models_bin/1/person-detection-retail-0013/FP16/person-detection-retail-0013.bin";
                Trace.WriteLine("downloading file from:" + modelUrl + " to: " + modelFile);
                System.Net.WebClient downloadClient = new System.Net.WebClient();
                try
                {
                    downloadClient.DownloadFile(modelUrl, modelFile);
                }
                catch
                {
                    //Delete the file in case of failed download.
                    File.Delete(modelFile);
                    throw;
                }
            }
            if (!File.Exists(modelXmlFile))
            {
                //Download the model xml file
                String modelXmlUrl = "https://download.01.org/opencv/2020/openvinotoolkit/2020.3/open_model_zoo/models_bin/1/person-detection-retail-0013/FP16/person-detection-retail-0013.xml";
                Trace.WriteLine("downloading file from:" + modelXmlUrl + " to: " + modelXmlFile);
                System.Net.WebClient downloadClient = new System.Net.WebClient();
                try
                {
                    downloadClient.DownloadFile(modelXmlUrl, modelXmlFile);
                }
                catch
                {
                    //Delete the file in case of failed download.
                    File.Delete(modelXmlFile);
                    throw;
                }
            }

            Dnn.Net net = DnnInvoke.ReadNetFromModelOptimizer(modelXmlFile, modelFile);

            Mat img = EmguAssert.LoadMat("space_shuttle.jpg");

            
            //CvInvoke.Resize(img, img, new Size(224, 224));

            //Mat inputBlob = DnnInvoke.BlobFromImage(img);
            //net.SetInput(inputBlob, "data");
            //Mat probBlob = net.Forward("prob");
        
        }
        */

        [Test]
        public void TestDnnBvlcGoogleNet()
        {

            //Dnn.Net net = new Dnn.Net();
            String googleNetFile = "bvlc_googlenet.caffemodel";
            if (!File.Exists(googleNetFile))
            {
                //Download the bvlc googlenet file
                String googleNetUrl = "http://dl.caffe.berkeleyvision.org/bvlc_googlenet.caffemodel";
                Trace.WriteLine("downloading file from:" + googleNetUrl + " to: " + googleNetFile);
                System.Net.WebClient downloadClient = new System.Net.WebClient();
                try
                {
                    downloadClient.DownloadFile(googleNetUrl, googleNetFile);
                }
                catch
                {
                    //Delete the file in case of failed download.
                    File.Delete(googleNetFile);
                    throw;
                }
            }
            //using (Dnn.Importer importer = Dnn.Importer.CreateCaffeImporter("bvlc_googlenet.prototxt", googleNetFile))
            //    importer.PopulateNet(net);
            Dnn.Net net = DnnInvoke.ReadNetFromCaffe("bvlc_googlenet.prototxt", googleNetFile);

            Mat img = EmguAssert.LoadMat("space_shuttle.jpg");

            CvInvoke.Resize(img, img, new Size(224, 224));

            Mat inputBlob = DnnInvoke.BlobFromImage(img);
            net.SetInput(inputBlob, "data");
            Mat probBlob = net.Forward("prob");

            /*
            int classId;
            double classProb;
            GetMaxClass(probBlob, out classId, out classProb);
            String[] classNames = ReadClassNames("synset_words.txt");

            //#if !NETFX_CORE
            Trace.WriteLine("Best class: " + classNames[classId] + ". Probability: " + classProb);
            //#endif
            */
        }

        private static void CheckAndDownloadFile(String fileName, String url)
        {
            //String emguS3Base = "https://s3.amazonaws.com/emgu-public/";
            if (!File.Exists(fileName))
            {
                //Download the ssd file    
                String fileUrl = url + fileName;
                Trace.WriteLine("downloading file from:" + fileUrl + " to: " + fileName);
                System.Net.WebClient downloadClient = new System.Net.WebClient();
                try
                {
                    downloadClient.DownloadFile(fileUrl, fileName);
                }
                catch
                {
                    File.Delete(fileName);
                    throw;
                }
            }
        }

        [Test]
        public void TestDnnSSD()
        {

            int imgDim = 300;

            String ssdFile = "VGG_VOC0712_SSD_300x300_iter_120000.caffemodel";
            String ssdProtoFile = "VGG_VOC0712_SSD_300x300_iter_120000.prototxt";
            String emguS3Base = "https://s3.amazonaws.com/emgu-public/";
            CheckAndDownloadFile(ssdFile, emguS3Base);
            CheckAndDownloadFile(ssdProtoFile, emguS3Base);

            Dnn.Net net = DnnInvoke.ReadNetFromCaffe(ssdProtoFile, ssdFile);
            //Layer inputLayer = net.GetLayer(0); //Layer with id = 0 is the input layer
            int[] outputLayerIds = net.UnconnectedOutLayers;
            Layer[] outputLayer = Array.ConvertAll(outputLayerIds, id => net.GetLayer(id));


            Mat img = EmguAssert.LoadMat("dog416.png");

            Mat inputBlob = DnnInvoke.BlobFromImage(img, 1.0, new Size(imgDim, imgDim), new MCvScalar(104, 117, 123), true, false);
            net.SetInput(inputBlob);
            Mat detection = net.Forward(outputLayer[0].Name);

            Int64 ticks = net.GetPerfProfile();
            Trace.WriteLine(String.Format("SSD processing time: {0} milliseconds", ticks / 10000));

            float confidenceThreshold = 0.5f;
            String[] labelsLines = File.ReadAllLines("pascal-classes.txt");
            String[] labels = new String[labelsLines.Length];
            for (int i = 0; i < labels.Length; i++)
            {
                labels[i] = labelsLines[i].Split(' ')[0].Trim();
            }

            int[] dim = detection.SizeOfDimension;
            int step = dim[3] * sizeof(float);
            IntPtr start = detection.DataPointer;
            for (int i = 0; i < dim[2]; i++)
            {
                float[] values = new float[dim[3]];
                Marshal.Copy(new IntPtr(start.ToInt64() + step * i), values, 0, dim[3]);
                float confident = values[2];

                if (confident > confidenceThreshold)
                {
                    float objectClass = values[1];

                    float xLeftBottom = values[3] * img.Cols;
                    float yLeftBottom = values[4] * img.Rows;
                    float xRightTop = values[5] * img.Cols;
                    float yRightTop = values[6] * img.Rows;
                    RectangleF objectRegion = new RectangleF(xLeftBottom, yLeftBottom, xRightTop - xLeftBottom, yRightTop - yLeftBottom);

                    CvInvoke.Rectangle(img, Rectangle.Round(objectRegion), new MCvScalar(0, 255, 0));
                    CvInvoke.PutText(img, labels[(int)objectClass - 1], Point.Round(objectRegion.Location), FontFace.HersheyPlain, 1.0, new MCvScalar(0, 0, 255));
                }
            }

            CvInvoke.Imwrite("rgb_ssd_result.jpg", img);

            String netText = net.Dump();
            net.DumpToFile("rgb_ssd.dot");
        }

        [Test]
        public void TestDnnSSDFaceDetect()
        {
            int imgDim = 300;
            MCvScalar meanVal = new MCvScalar(104, 177, 123);
            String ssdFile = "res10_300x300_ssd_iter_140000.caffemodel";
            String ssdProtoFile = "deploy.prototxt";


            CheckAndDownloadFile(ssdFile, "https://github.com/opencv/opencv_3rdparty/raw/dnn_samples_face_detector_20170830/");
            CheckAndDownloadFile(ssdProtoFile, "https://raw.githubusercontent.com/opencv/opencv/master/samples/dnn/face_detector/");

            Dnn.Net net = DnnInvoke.ReadNetFromCaffe(ssdProtoFile, ssdFile);

            Mat img = EmguAssert.LoadMat("lena.jpg");

            Mat inputBlob = DnnInvoke.BlobFromImage(img, 1.0, new Size(imgDim, imgDim), meanVal, false, false);
            net.SetInput(inputBlob, "data");
            Mat detection = net.Forward("detection_out");

            float confidenceThreshold = 0.5f;

            List<Rectangle> faceRegions = new List<Rectangle>();

            int[] dim = detection.SizeOfDimension;
            int step = dim[3] * sizeof(float);
            IntPtr start = detection.DataPointer;
            for (int i = 0; i < dim[2]; i++)
            {
                float[] values = new float[dim[3]];
                Marshal.Copy(new IntPtr(start.ToInt64() + step * i), values, 0, dim[3]);
                float confident = values[2];

                if (confident > confidenceThreshold)
                {
                    float xLeftBottom = values[3] * img.Cols;
                    float yLeftBottom = values[4] * img.Rows;
                    float xRightTop = values[5] * img.Cols;
                    float yRightTop = values[6] * img.Rows;
                    RectangleF objectRegion = new RectangleF(xLeftBottom, yLeftBottom, xRightTop - xLeftBottom, yRightTop - yLeftBottom);
                    Rectangle faceRegion = Rectangle.Round(objectRegion);
                    faceRegions.Add(faceRegion);

                }
            }

            String facemarkFileName = "lbfmodel.yaml";
            String facemarkFileUrl = "https://raw.githubusercontent.com/kurnianggoro/GSOC2017/master/data/";
            CheckAndDownloadFile(facemarkFileName, facemarkFileUrl);

            using (FacemarkLBFParams facemarkParam = new CV.Face.FacemarkLBFParams())
            using (FacemarkLBF facemark = new CV.Face.FacemarkLBF(facemarkParam))
            using (VectorOfRect vr = new VectorOfRect(faceRegions.ToArray()))
            using (VectorOfVectorOfPointF landmarks = new VectorOfVectorOfPointF())
            {
                facemark.LoadModel(facemarkFileName);
                facemark.Fit(img, vr, landmarks);

                foreach (Rectangle face in faceRegions)
                {
                    CvInvoke.Rectangle(img, face, new MCvScalar(0, 255, 0));
                }

                int len = landmarks.Size;
                for (int i = 0; i < landmarks.Size; i++)
                {
                    using (VectorOfPointF vpf = landmarks[i])
                        FaceInvoke.DrawFacemarks(img, vpf, new MCvScalar(255, 0, 0));
                }

            }

            CvInvoke.Imwrite("rgb_ssd_facedetect.jpg", img);

        }

        [Test]
        public void TestDnnTensorFlow()
        {

            //Dnn.Net net = new Dnn.Net();
            String tensorFlowFile = "tensorflow_inception_graph.pb";
            if (!File.Exists(tensorFlowFile))
            {
                //Download the tensorflow file
                String inceptionFile = "inception5h.zip";
                String googleNetUrl = "https://storage.googleapis.com/download.tensorflow.org/models/inception5h.zip";
                Trace.WriteLine("downloading file from:" + googleNetUrl + " to: " + inceptionFile);
                System.Net.WebClient downloadClient = new System.Net.WebClient();
                downloadClient.DownloadFile(googleNetUrl, inceptionFile);

                System.IO.Compression.ZipFile.ExtractToDirectory(inceptionFile, ".");
            }

            Dnn.Net net = DnnInvoke.ReadNetFromTensorflow(tensorFlowFile);
            //using (Dnn.Importer importer = Dnn.Importer.CreateTensorflowImporter(tensorFlowFile))
            //    importer.PopulateNet(net);

            Mat img = EmguAssert.LoadMat("space_shuttle.jpg");

            CvInvoke.Resize(img, img, new Size(224, 224));
            CvInvoke.CvtColor(img, img, ColorConversion.Bgr2Rgb);

            Mat inputBlob = DnnInvoke.BlobFromImage(img);
            net.SetInput(inputBlob, "input");
            Mat probBlob = net.Forward("softmax2");
            /*
            Dnn.Blob inputBlob = new Dnn.Blob();
            inputBlob.BatchFromImages(img);
            net.SetBlob(".input", inputBlob);
            net.Forward();
            Dnn.Blob probBlob = net.GetBlob("softmax2");
            */
            int classId;
            double classProb;
            GetMaxClass(probBlob, out classId, out classProb);
            String[] classNames = ReadClassNames("imagenet_comp_graph_label_strings.txt");

            //#if !NETFX_CORE
            Trace.WriteLine("Best class: " + classNames[classId] + ". Probability: " + classProb);
            //#endif

        }
#endif

#if !(__ANDROID__ || __IOS__ || NETFX_CORE)
        [Test]
        public void TestUnicodeImgFileIO()
        {
            Mat m = EmguAssert.LoadMat("lena.jpg");
            EmguAssert.IsTrue(CvInvoke.Imwrite("测试.jpg", m));
            Bitmap bmp = new Bitmap("测试.jpg");
            UMat m2 = bmp.ToImage<Bgr, Byte>().ToUMat();
            Mat m3 = EmguAssert.LoadMat("测试.jpg");
            //Emgu.CV.UI.ImageViewer.Show(m2);
        }

        [Test]
        public void TestUnicodeCvString()
        {
            String target = "测试.jpg";
            using (CvString s = new CvString(target))
            {
                String s2 = s.ToString();
                EmguAssert.IsTrue(s2.Equals(target));
            }
        }

        [Test]
        public void TestFreetype()
        {
            using (Mat m = new Mat(new Size(640, 480), DepthType.Cv8U, 3))
            using (Freetype.Freetype2 freetype = new Freetype2())
            {
                m.SetTo(new MCvScalar(0, 0, 0, 0));
                freetype.LoadFontData("NotoSansCJK-Regular.ttc", 0);

                freetype.PutText(m, "测试", new Point(100, 100), 36, new MCvScalar(255, 255, 0), 1, LineType.EightConnected, false);
                //CvInvoke.NamedWindow("test");
                //CvInvoke.Imshow("test", m);
                //CvInvoke.WaitKey();
            }
        }

        [Test]
        public void TestHDR()
        {
            String[] lines = File.ReadAllLines("list.txt");

            List<Mat> imageList = new List<Mat>();
            List<float> timeList = new List<float>();
            foreach (var line in lines)
            {
                string[] words = line.Split(' ');
                String fileName = words[0];
                Mat m = CvInvoke.Imread(fileName);
                float time = 1.0f / float.Parse(words[1]);
                imageList.Add(m);
                timeList.Add(time);
            }

            using (VectorOfMat images = new VectorOfMat(imageList.ToArray()))
            using (VectorOfFloat times = new VectorOfFloat(timeList.ToArray()))
            {
                Mat response = new Mat();
                CalibrateDebevec calibrate = new CalibrateDebevec();
                calibrate.Process(images, response, times);

                Mat hdr = new Mat();
                MergeDebevec mergeDebevec = new MergeDebevec();
                mergeDebevec.Process(images, hdr, times, response);

                Mat ldr = new Mat();
                Tonemap tonemap = new Tonemap(2.2f);
                tonemap.Process(hdr, ldr);

                Mat fusion = new Mat();
                MergeMertens mergeMertens = new MergeMertens();
                mergeMertens.Process(images, fusion, times, response);
                //mergeMertens.Process(images, fusion);

                using (Mat fusionScaled = new Mat())
                using (Mat ldrScaled = new Mat())
                using (ScalarArray sa = new ScalarArray(new MCvScalar(255.0, 255.0, 255.0)))
                {
                    fusion.ConvertTo(fusionScaled, DepthType.Cv8U, 255);
                    CvInvoke.Imwrite("fusion.png", fusionScaled);

                    ldr.ConvertTo(ldrScaled, DepthType.Cv8U, 255);
                    CvInvoke.Imwrite("ldr.png", ldrScaled);

                    CvInvoke.Imwrite("hdr.hdr", hdr);
                }

            }
            imageList.Clear();

        }
#endif
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
            Size s = CvInvoke.GetTextSize("Hello world", FontFace.HersheyPlain, 16, 1, ref baseline);
        }

        [Test]
        public void TestArucoCreateBoard()
        {
            Emgu.CV.Aruco.DetectorParameters p = DetectorParameters.GetDefault();

            Size imageSize = new Size();
            int markersX = 4;
            int markersY = 4;
            int markersLength = 80;
            int markersSeparation = 30;
            int margins = markersSeparation;
            imageSize.Width = markersX * (markersLength + markersSeparation) - markersSeparation + 2 * margins;
            imageSize.Height = markersY * (markersLength + markersSeparation) - markersSeparation + 2 * margins;
            int borderBits = 1;

            Aruco.Dictionary dictionary = new Dictionary(Dictionary.PredefinedDictionaryName.Dict4X4_100);
            Aruco.GridBoard board = new GridBoard(markersX, markersY, markersLength, markersSeparation, dictionary);
            Mat boardImage = new Mat();
            board.Draw(imageSize, boardImage, margins, borderBits);
            CvInvoke.Imwrite("board.png", boardImage);
        }

        [Test]
        public void TestDPM()
        {
            Mat m = EmguAssert.LoadMat("pedestrian.png");
            DpmDetector detector = new DpmDetector(new String[] { "inriaperson.xml" }, new string[] { "person" });
            ObjectDetection[] result = detector.Detect(m);

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

        private static double CompareHash(ImgHashBase imgHash, Mat m1, Mat m2)
        {
            Mat hash1 = new Mat();
            Mat hash2 = new Mat();
            imgHash.Compute(m1, hash1);
            imgHash.Compute(m2, hash2);
            return imgHash.Compare(hash1, hash2);
        }

        [Test]
        public void TestRng()
        {
            Emgu.CV.RNG rng = new Emgu.CV.RNG();
            Mat m = new Mat(new Size(480, 320), DepthType.Cv8U, 3);
            rng.Fill(m, RNG.DistType.Uniform, new MCvScalar(0, 0, 0, 0), new MCvScalar(255, 255, 255, 255));
            var data1 = m.GetData();
            var data2 = m.GetData(false);
            UMat um = new UMat(new Size(480, 320), DepthType.Cv8U, 3);
            rng.Fill(um, RNG.DistType.Uniform, new MCvScalar(0, 0, 0, 0), new MCvScalar(255, 255, 255, 255));
            data1 = um.GetData();
            data2 = um.GetData(false);
        }

        [Test]
        public void TestHash()
        {
            Mat m1 = EmguAssert.LoadMat("lena.jpg");
            Mat m2 = new Mat();
            CvInvoke.GaussianBlur(m1, m2, new Size(3, 3), 1);

            using (AverageHash averageHash = new AverageHash())
            {
                double diff = CompareHash(averageHash, m1, m2);
            }

            using (BlockMeanHash bmh = new BlockMeanHash())
            {
                double diff = CompareHash(bmh, m1, m2);
            }

            using (ColorMomentHash cmh = new ColorMomentHash())
            {
                double diff = CompareHash(cmh, m1, m2);
            }

            using (MarrHildrethHash cmh = new MarrHildrethHash())
            {
                double diff = CompareHash(cmh, m1, m2);
            }

            using (PHash cmh = new PHash())
            {
                double diff = CompareHash(cmh, m1, m2);
            }

            using (RadialVarianceHash cmh = new RadialVarianceHash())
            {
                double diff = CompareHash(cmh, m1, m2);
            }
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
            Mat source = new Mat("lena.jpg", ImreadModes.Color);
            Mat img1 = new Mat();
            CvInvoke.Resize(source, img1, new System.Drawing.Size(size, size));
            //Mat img1 = new Mat(height, width, DepthType.Cv8U, 3); 
            //CvInvoke.Randn(img1, new MCvScalar(122, 122, 122), new MCvScalar(60, 60, 60));

            // create a smaller 3 channel image
            Mat img2 = new Mat();
            CvInvoke.Resize(source, img2, new System.Drawing.Size(size / 2, size / 2));
            //CvInvoke.Randn(img2, new MCvScalar(122, 122, 122), new MCvScalar(60, 60, 60));

            //use a circular region as the clone mask
            Mat mask = new Mat(img2.Size, DepthType.Cv8U, 1);
            int radius = Math.Min(mask.Width, mask.Height) / 2;
            CvInvoke.Circle(mask, new System.Drawing.Point(mask.Height / 2, mask.Width / 2), radius, new MCvScalar(255), -1);

            Mat blend = new Mat(img1.Size, DepthType.Cv8U, 3);
            CvInvoke.SeamlessClone(img2, img1, mask, new System.Drawing.Point(img1.Height / 2, img1.Width / 2), blend, CloningMethod.Normal);
            return blend;
        }

#if !NETFX_CORE

        public void CreateUnityIcons(String productName = null)
        {
            //128x128
            Image<Bgra, Byte> imgSmall = GenerateLogo(128, 128, productName);

            //200x258
            Image<Bgra, Byte> imgMedium = GenerateLogo(200, 120, productName).ConcateVertical(new Image<Bgra, byte>(200, 138));

            //860x389
            int screenShotWidth = 400;
            int rightPadding = 40;
            Image<Bgra, Byte> screenShot =
               new Image<Bgr, byte>("unity_screenshot.png").Resize(screenShotWidth, 209, Inter.Linear,
                  true).Convert<Bgra, Byte>();
            if (screenShot.Width < screenShotWidth)
                screenShot = new Image<Bgra, byte>((screenShotWidth - screenShot.Width) / 2, screenShot.Height).ConcateHorizontal(screenShot);
            Image<Bgra, Byte> imgLarge =
               new Image<Bgra, byte>(860 - (screenShotWidth + rightPadding), 389, new Bgra(255, 255, 255, 0)).ConcateHorizontal(
               GenerateLogo(screenShotWidth, 389 - screenShot.Height).ConcateVertical(screenShot)).ConcateHorizontal(
               new Image<Bgra, byte>(rightPadding, 389, new Bgra(255, 255, 255, 0)));

            imgSmall.Save(String.Format("Emgu{0}Logo_128x128.png", productName == null ? String.Empty : productName));
            imgMedium.Save(String.Format("Emgu{0}Logo_200x258.png", productName == null ? String.Empty : productName));
            imgLarge.Save(String.Format("Emgu{0}Logo_860x389.png", productName == null ? String.Empty : productName));


            //Image<Bgra, Byte> result = imgSmall.ConcateVertical(imgMedium).ConcateVertical(imgLarge);
            //result.Draw(new LineSegment2D(new Point(0, imgSmall.Height), new Point(result.Width, imgSmall.Height) ), new Bgra(0, 0, 0, 255), 1  );
            //result.Draw(new LineSegment2D(new Point(0, imgSmall.Height + imgMedium.Height), new Point(result.Width, imgSmall.Height + imgMedium.Height)), new Bgra(0, 0, 0, 255), 1);
            //ImageViewer.Show(result);
        }

        /*
        [Test]
        public void TestLSDDetector()
        {
            Mat mat = EmguAssert.LoadMat("box.png");
            using (Emgu.CV.LineDescriptor.LSDDetector lsdDetector = new LineDescriptor.LSDDetector())
            using (Emgu.CV.LineDescriptor.VectorOfKeyLine lines = new LineDescriptor.VectorOfKeyLine())
            using (Mat mask = Mat.Ones(mat.Rows, mat.Cols, DepthType.Cv8U, 1))
            using (Mat output = mat.Clone())
            {
                if (output.NumberOfChannels == 1)
                    CvInvoke.CvtColor(output, output, ColorConversion.Gray2Bgr);
                for (int i = 0; i < 10; i++)
                    lsdDetector.Detect(mat, lines, 2, 1, mask);

                Emgu.CV.LineDescriptor.MKeyLine[] keylines = lines.ToArray();
                foreach (Emgu.CV.LineDescriptor.MKeyLine line in keylines)
                {
                    if (line.Octave == 0)
                        CvInvoke.Line(output,
                            Point.Round(new PointF(line.StartPointX, line.StartPointX)),
                            Point.Round(new PointF(line.EndPointX, line.EndPointY)),
                            new MCvScalar(0, 0, 255, 0));
                }

                output.Save("line_detected.png");
            }
        }*/

        [Test]
        public void TestCvException()
        {
            //Test seems to crash on Linux system. Skipping test on Linux for now.
            if (!System.Runtime.InteropServices.RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
            {
                try
                {
                    // Do something to cause a CvException, canny do not work on 4 channel images, will throw CvException
                    Mat m = new Mat(new Size(480, 320), DepthType.Cv32F, 4);
                    Mat edges = new Mat();
                    CvInvoke.Canny(m, edges, 100, 80);
                }
                catch (CvException e)
                {
                    string str = e.ErrorStr;
                }
            }
        }

        [Test]
        public void TestRetinaFastToneMapping()
        {
            if (Emgu.Util.Platform.OperationSystem == Emgu.Util.Platform.OS.Windows)
            {
                //TODO: Find out why this fails on Ubuntu
                Mat m = EmguAssert.LoadMat("pedestrian.png");
                Mat result = new Mat();
                using (Bioinspired.RetinaFastToneMapping tm = new Bioinspired.RetinaFastToneMapping(m.Size))
                using (Mat gray = new Mat())
                {
                    CvInvoke.CvtColor(m, gray, ColorConversion.Bgr2Gray);
                    tm.Setup(3.0f, 1.0f, 1.0f);
                    tm.ApplyFastToneMapping(gray, result);
                }

                //CvInvoke.Imshow("Tone Mapping", result);
                //CvInvoke.WaitKey();
            }
        }

        [Test]
        public void TestOilPainting()
        {
            Mat m = EmguAssert.LoadMat("pedestrian.png");
            Mat result = new Mat();
            Emgu.CV.XPhoto.XPhotoInvoke.OilPainting(m, result, 10, 1, ColorConversion.Bgr2Lab);
            //CvInvoke.Imshow("oil painting", result);
            //CvInvoke.WaitKey();
        }

        [Test]
        public void TestGenerateLogo()
        {
            String productName = null;
            Image<Bgra, Byte> logo = GenerateLogo(800, 800, productName);
            logo.Save(String.Format("Emgu{0}Logo.png", productName == null ? String.Empty : productName));
        }

        /*
        public void GenerateLogo(String productName = null)
        {
            Image<Bgra, Byte> logo = GenerateLogo(860, 389, productName);
            logo.Save(String.Format("Emgu{0}Logo.png", productName == null ? String.Empty : productName));
        }*/

        public Image<Bgra, byte> GenerateLogo(int width, int height = -1, String productName = null)
        {
            int heightShift = 0;
            int textHeight = (int)(width / 160.0 * 72.0);
            if (height <= 0)
                height = textHeight;
            else
            {
                heightShift = Math.Max((height - textHeight) / 2, 0);
            }
            double scale = width / 160.0;
            Image<Bgr, Byte> semgu = new Image<Bgr, byte>(width, height, new Bgr(0, 0, 0));
            Image<Bgr, Byte> scv = new Image<Bgr, byte>(width, height, new Bgr(0, 0, 0));
            //MCvFont f1 = new MCvFont(CvEnum.FontFace.HersheyTriplex, 1.5 * scale, 1.5 * scale);
            //MCvFont f2 = new MCvFont(CvEnum.FontFace.HersheyComplex, 1.6 * scale, 2.2 * scale);
            semgu.Draw("Emgu", Point.Round(new PointF((float)(6 * scale), (float)(50 * scale + heightShift))), CvEnum.FontFace.HersheyTriplex, 1.5 * scale, new Bgr(55, 155, 255), (int)Math.Round(1.5 * scale));
            semgu._Dilate((int)(1 * scale));
            if (productName != null)
                scv.Draw(productName, Point.Round(new PointF((float)(50 * scale), (float)(60 * scale + heightShift))), CvEnum.FontFace.HersheySimplex, 1.6 * scale, new Bgr(255, 55, 255), (int)Math.Round(2.2 * scale));

            scv._Dilate((int)(2 * scale));
            Image<Bgr, Byte> logoBgr = semgu.Or(scv);
            Image<Gray, Byte> logoA = new Image<Gray, byte>(logoBgr.Size);
            logoA.SetValue(255, logoBgr.Convert<Gray, Byte>());
            logoBgr._Not();
            logoA._Not();
            Image<Gray, Byte>[] channels = logoBgr.Split();
            channels = new Image<Gray, byte>[] { channels[0], channels[1], channels[2], new Image<Gray, Byte>(channels[0].Width, channels[0].Height, new Gray(255.0)) };
            Image<Bgra, Byte> logoBgra = new Image<Bgra, byte>(channels);
            logoBgra.SetValue(new Bgra(0.0, 0.0, 0.0, 0.0), logoA);
            //logoBgra.Save("EmguCVLogo.png");
            return logoBgra;
            /*
            Image<Bgr, Byte> bg_header = new Image<Bgr, byte>(1, 92);
            for (int i = 0; i < 92; i++)
               bg_header[i, 0] = new Bgr(210, 210 - i * 0.4, 210 - i * 0.9);
            bg_header.Save("bg_header.gif");*/
        }

        [Test]
        public void TestVectorOfCvERStat()
        {
            //CvInvoke.CheckLibraryLoaded();
            int sizeOfElement = Emgu.Util.Toolbox.SizeOf<MCvERStat>();
            using (VectorOfERStat v = new VectorOfERStat())
            {

            }
        }

        [Test]
        public static void TestBackgroundSubtractorMOG2()
        {
            //ImageViewer viewer = new ImageViewer();
            using (VideoCapture capture = new VideoCapture("tree.avi"))
            using (BackgroundSubtractorMOG2 subtractor = new BackgroundSubtractorMOG2())
            using (Mat frame = new Mat())
            using (Mat fgMask = new Mat())
            {
                int frameCount = 0;
                while (capture.Grab())
                {
                    capture.Retrieve(frame);
                    subtractor.Apply(frame, fgMask);
                    frameCount++;
                }
                EmguAssert.IsTrue(frameCount > 0, "BackgroundSubtractorMOG2 did not return any frames");
            }
        }

        [Test]
        public static void TestIntensityTransform()
        {
            Mat m = new Mat("lena.jpg", ImreadModes.Color);
            Mat bimef = new Mat();
            Mat autoScaling = new Mat();
            Mat gamma = new Mat();
            Mat contrastStretch = new Mat();
            Mat log = new Mat();
            Emgu.CV.IntensityTransform.IntensityTransformInvoke.BIMEF(m, bimef);
            Emgu.CV.IntensityTransform.IntensityTransformInvoke.Autoscaling(m, autoScaling);
            Emgu.CV.IntensityTransform.IntensityTransformInvoke.GammaCorrection(m, gamma, 2.0f);
            Emgu.CV.IntensityTransform.IntensityTransformInvoke.ContrastStretching(m, contrastStretch, 0, 0, 200, 200);
            Emgu.CV.IntensityTransform.IntensityTransformInvoke.LogTransform(m, log);
        }

        [Test]
        public static void TestOnePassVideoStabilizer()
        {
            //ImageViewer viewer = new ImageViewer();
            using (VideoCapture capture = new VideoCapture("tree.avi"))
            using (Emgu.CV.VideoStab.CaptureFrameSource framesource = new CaptureFrameSource(capture))
            using (Emgu.CV.VideoStab.GaussianMotionFilter motionFilter = new Emgu.CV.VideoStab.GaussianMotionFilter())
            //using (Features2D.FastDetector detector = new Features2D.FastDetector(10, true))
            //using (Features2D.SURF detector = new Features2D.SURF(500, false))
            //using (Features2D.ORBDetector detector = new Features2D.ORBDetector(500))
            using (Emgu.CV.VideoStab.OnePassStabilizer stabilizer = new Emgu.CV.VideoStab.OnePassStabilizer(framesource))
            using (Mat frame = new Mat())
            {
                stabilizer.SetMotionFilter(motionFilter);
                int frameCount = 0;
                while (stabilizer.NextFrame(frame))
                {
                    frameCount++;
                }
                EmguAssert.IsTrue(frameCount > 0, "VideoStabilizer did not return any frames");
            }
        }

        /*
        [Test]
        public void TestCreateImageHeader()
        {
            int width = 480;
            int height = 320;
            IntPtr iplImageHeader = CvInvoke.cvCreateImageHeader(new Size(width, height), IplDepth.IplDepth_8U, 1);
            Size s = CvInvoke.cvGetSize(iplImageHeader);
            CvInvoke.cvReleaseImageHeader(ref iplImageHeader);
            Assert.AreEqual(s.Width, width);
            Assert.AreEqual(s.Height, height);
        }*/

        [Test]
        public void TestFileReaderMat()
        {
            bool success;
            using (Mat m = new Mat())
                success = Emgu.CV.NativeMatFileIO.ReadFileToMat("scenetext01.jpg", m, ImreadModes.AnyColor);
        }

        [Test]
        public void TestLoadLibrary()
        {
            bool loaded = (IntPtr.Zero != Emgu.Util.Toolbox.LoadLibrary("not_exist"));
            EmguAssert.IsFalse(loaded);
        }

        [Test]
        public void TestMcc()
        {
            using (Mat image = EmguAssert.LoadMat("MCC24.png"))
            using (CCheckerDetector detector = new CCheckerDetector())
            {
                if (detector.Process(image, CChecker.TypeChart.MCC24))
                {
                    using (CChecker checker = detector.BestColorChecker)
                    using (CCheckerDraw drawer = new CCheckerDraw(checker, new MCvScalar(0, 255, 0), 1))
                    {
                        drawer.Draw(image);
                        //image.Save("c:\\tmp.out.png");
                    }
                    //using (Mat img = new Mat(new Size(480, 320), DepthType.Cv8U, 3))
                    //{
                    //    drawer.Draw(img);
                    //}
                }
            }

            using (CChecker c = new CChecker())
            {
                PointF p = c.Center;
            }

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
        public void TestERFilter()
        {
            CvInvoke.SanityCheck();
            bool checkInvert = true;
            using (Image<Bgr, Byte> image = EmguAssert.LoadImage<Bgr, Byte>("scenetext01.jpg"))
            using (ERFilterNM1 er1 = new ERFilterNM1(EmguAssert.GetFile("trained_classifierNM1.xml"), 8, 0.00025f, 0.13f, 0.4f, true, 0.1f))
            using (ERFilterNM2 er2 = new ERFilterNM2(EmguAssert.GetFile("trained_classifierNM2.xml"), 0.3f))
            {
                //using (Image<Gray, Byte> mask = new Image<Gray,byte>(image.Size.Width + 2, image.Size.Height + 2))
                int channelCount = image.NumberOfChannels;
                UMat[] channels = new UMat[checkInvert ? channelCount * 2 : channelCount];

                for (int i = 0; i < channelCount; i++)
                {
                    UMat c = new UMat();
                    CvInvoke.ExtractChannel(image.Mat, c, i);
                    channels[i] = c;
                }

                if (checkInvert)
                {
                    for (int i = 0; i < channelCount; i++)
                    {
                        UMat c = new UMat();
                        CvInvoke.BitwiseNot(channels[i], c);
                        channels[i + channelCount] = c;
                    }
                }

                VectorOfERStat[] regionVecs = new VectorOfERStat[channels.Length];


                for (int i = 0; i < regionVecs.Length; i++)
                    regionVecs[i] = new VectorOfERStat();

                /*
                for (int i = 0; i < channels.Length; i++)
                {
                   Emgu.CV.UI.ImageViewer.Show(channels[i]);
                }*/

                try
                {
                    for (int i = 0; i < channels.Length; i++)
                    {
                        er1.Run(channels[i], regionVecs[i]);
                        er2.Run(channels[i], regionVecs[i]);
                    }
                    using (VectorOfUMat vm = new VectorOfUMat(channels))
                    {
                        Rectangle[] regions = ERFilter.ERGrouping(image, vm, regionVecs, ERFilter.GroupingMethod.OrientationHoriz, EmguAssert.GetFile("trained_classifier_erGrouping.xml"), 0.5f);

                        /*
                        System.Runtime.Serialization.Formatters.Binary.BinaryFormatter formatter = new BinaryFormatter();
                        using (MemoryStream ms = new MemoryStream())
                        {
                            formatter.Serialize(ms, regionVecs);

                            VectorOfERStat rv2 = formatter.Deserialize(ms) as VectorOfERStat;
                        }*/

                        foreach (Rectangle rect in regions)
                            image.Draw(rect, new Bgr(0, 0, 255), 2);

                    }
                }
                finally
                {
                    foreach (UMat tmp in channels)
                        if (tmp != null)
                            tmp.Dispose();
                    foreach (VectorOfERStat tmp in regionVecs)
                        if (tmp != null)
                            tmp.Dispose();
                }
                //Emgu.CV.UI.ImageViewer.Show(image);

            }

        }
#endif
    }
}
