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
//using Emgu.CV.WinForms;
using Emgu.CV.Util;
using Emgu.CV.VideoStab;
using Emgu.CV.XFeatures2D;
using Emgu.CV.XImgproc;
using Emgu.CV.Legacy;
using Emgu.Util;

using System.Threading.Tasks;

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
        public void TestBuildInformation()
        {
            String bi = CvInvoke.BuildInformation;
        }
        

        /*
        [Test]
        public void TestRotationMatrix3D()
        {
            double[] rod = new double[] { 0.2, 0.5, 0.3 };
            RotationVector3D rodVec = new RotationVector3D(rod);

            RotationVector3D rodVec2 = new RotationVector3D();
            rodVec2.RotationMatrix = rodVec.RotationMatrix;
            Matrix<double> diff = rodVec - rodVec2;
            EmguAssert.IsTrue(diff.Norm < 1.0e-8);
        }*/
        
        [Test]
        public void TestViz()
        {
            var openCVConfigDict = CvInvoke.ConfigDict;
            bool haveViz = (openCVConfigDict["HAVE_OPENCV_VIZ"] != 0);
            if (haveViz && RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                Viz3d viz = new Viz3d("show_simple_widgets");
                viz.SetBackgroundMeshLab();
                WCoordinateSystem coor = new WCoordinateSystem();
                viz.ShowWidget("coor", coor);
                WCube cube = new WCube(new MCvPoint3D64f(-.5, -.5, -.5), new MCvPoint3D64f(.5, .5, .5), true,
                    new MCvScalar(255, 255, 255));
                viz.ShowWidget("cube", cube);
                WCube cube0 = new WCube(new MCvPoint3D64f(-1, -1, -1), new MCvPoint3D64f(-.5, -.5, -.5), false,
                    new MCvScalar(123, 45, 200));
                viz.ShowWidget("cub0", cube0);
                //viz.Spin();
            }
        }
        
        [Test]
        public void TestLogLevel()
        {
            CvEnum.LogLevel level = CvInvoke.LogLevel;
            CvInvoke.LogLevel = CvEnum.LogLevel.Debug;
            level = CvInvoke.LogLevel;
        }

        /*
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
        }*/

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
              //EmguAssert.IsTrue(detected);
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

        /*
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
        }*/

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


#if !(__IOS__ || __ANDROID__ || NETFX_CORE)
        [Test]
        public void TestGrayscaleBitmapConstructor()
        {
            if (Emgu.Util.Platform.OperationSystem == Emgu.Util.Platform.OS.Windows)
            {
                String tmpFileName = "tmp.png";
                //Image<Bgra, Byte> img = new Image<Bgra, byte>(320, 240);
                Mat img = new Mat(320, 240, DepthType.Cv8U, 4);
                CvInvoke.Randu(img, new MCvScalar(), new MCvScalar(255, 255, 255, 255));
                //img.SetRandUniform(new MCvScalar(), new MCvScalar(255, 255, 255, 255));
                img.Save(tmpFileName);

                Stopwatch stopwatch = Stopwatch.StartNew();
                Mat img2 = CvInvoke.Imread(tmpFileName, ImreadModes.Unchanged);
                //Image<Bgra, Byte> img2 = new Image<Bgra, byte>("tmp.png");
                stopwatch.Stop();
                Trace.WriteLine(string.Format("Time: {0} milliseconds", stopwatch.ElapsedMilliseconds));
                //Image<Bgra, Byte> absDiff = new Image<Bgra, Byte>(320, 240);
                Mat absDiff = new Mat();
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
                using (Bitmap bmp = new Bitmap(tmpFileName))
                using (Image bmpImage = Bitmap.FromFile(tmpFileName))
                {
                    EmguAssert.AreEqual(System.Drawing.Imaging.PixelFormat.Format32bppArgb, bmpImage.PixelFormat);

                    //Image<Gray, Byte> img3 = bmp.ToImage<Gray, byte>();
                    Mat img3Bgra = bmp.ToMat();
                    Mat img3 = new Mat();
                    CvInvoke.CvtColor(img3Bgra, img3, ColorConversion.Bgra2Gray);
                    Mat imgGray = new Mat();
                    CvInvoke.CvtColor(img, imgGray, ColorConversion.Bgra2Gray);
                    stopwatch.Stop();
                    Trace.WriteLine(string.Format("Time: {0} milliseconds", stopwatch.ElapsedMilliseconds));
                    Mat diff = new Mat();
                    CvInvoke.AbsDiff(img3, imgGray, diff);
                    //Image<Gray, Byte> diff = img.Convert<Gray, Byte>().AbsDiff(img3);

                    //Test seems to failed on Linux system. Skipping test on Linux for now.
                    if (!System.Runtime.InteropServices.RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
                        EmguAssert.AreEqual(0, CvInvoke.CountNonZero(diff));

                    EmguAssert.IsTrue(imgGray.Equals(img3));
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
        */        /*
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
              //EmguAssert.IsTrue(contourD.Convex);
              //EmguAssert.AreEqual(contourD.Area, 4.0);
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

            //Emgu.CV.WinForms.ImageViewer.Show(img, String.Format("Convex Hull Computed in {0} milliseconds", watch.ElapsedMilliseconds));

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
        }

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
        */

        [Test]
        public void TestVectorOfMat()
        {
            Mat m1 = new Mat(3, 3, DepthType.Cv64F, 1);
            CvInvoke.Randn(m1, new MCvScalar(0.0), new MCvScalar(1.0));

            Mat m2 = new Mat(4, 4, DepthType.Cv32S, 1);
            CvInvoke.Randn(m1, new MCvScalar(2), new MCvScalar(2));
            

            VectorOfMat vec = new VectorOfMat(m1, m2);

            Mat tmp1 = vec[0];
            Mat tmp2 = vec[1];
            Mat n1 = new Mat();
            Mat n2 = new Mat();
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

              //Emgu.CV.WinForms.ImageViewer.Show(image, String.Format("Detection Time: {0}ms", watch.ElapsedMilliseconds));
           }
        }*/

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

        /*
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
        */


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

        /*
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
        }*/

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

        /*
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
        }*/

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
                       Emgu.CV.WinForms.ImageViewer.Show(img);

                       result.Inflate((int)(result.Width * 0.2), (int)(result.Height * 0.2));
                       using (Image<Gray, Byte> mask = img.GrabCut(result, 10))
                       using (Image<Gray, Byte> canny = img.Canny(120, 80))
                       {
                          //MCvFont f = new MCvFont(CvEnum.HersheyFonts.Complex, 2.0, 2.0);
                          using (ScalarArray ia = new ScalarArray(3))
                             CvInvoke.Compare(mask, ia, mask, CvEnum.CmpType.NotEqual);
                          //CvInvoke.Set(canny, new MCvScalar(), mask);
                          canny.SetValue(new MCvScalar(), mask);
                          canny.Draw(@"http://www.emgu.com", new Point(50, 50), CvEnum.HersheyFonts.Complex, 2.0, new Gray(255), 2);

                          CvInvoke.BitwiseNot(canny, canny, null);

                          Image<Bgr, byte> displayImg = img.ConcateHorizontal(canny.Convert<Bgr, Byte>());

                          //displayImg.Save("out_" + files[idx]);
                          Emgu.CV.WinForms.ImageViewer.Show(displayImg);
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
              //Emgu.CV.WinForms.ImageViewer.Show(image);
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
                       //Emgu.CV.WinForms.ImageViewer.Show(img);

                    }
                 }
           }
        }*/

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
            Mat img = EmguAssert.LoadMat("box.png", ImreadModes.Grayscale);
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
        public void TestTempFile()
        {
            String tempFileName = CvInvoke.TempFile(String.Empty);
        }

        [Test]
        public void TestFeature2DProperties()
        {
            using (FastFeatureDetector fast = new FastFeatureDetector())
            {
                fast.Threshold = 25;
                EmguAssert.AreEqual(25, fast.Threshold);
                fast.NonmaxSuppression = false;
                EmguAssert.IsFalse(fast.NonmaxSuppression);
                fast.Type = FastFeatureDetector.DetectorType.Type5_8;
                EmguAssert.AreEqual(FastFeatureDetector.DetectorType.Type5_8, fast.Type);
            }

            using (ORB orb = new ORB())
            {
                orb.MaxFeatures = 250;
                EmguAssert.AreEqual(250, orb.MaxFeatures);
                orb.ScaleFactor = 1.5;
                EmguAssert.IsTrue(Math.Abs(orb.ScaleFactor - 1.5) < 1e-6);
                orb.NLevels = 4;
                EmguAssert.AreEqual(4, orb.NLevels);
                orb.FastThreshold = 15;
                EmguAssert.AreEqual(15, orb.FastThreshold);
                orb.Score = ORB.ScoreType.Fast;
                EmguAssert.AreEqual(ORB.ScoreType.Fast, orb.Score);
            }
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
        public void TestSpan()
        {
#if UNSAFE_ALLOWED
            using (Mat mat1 = new Mat(1000, 1000, DepthType.Cv8U, 3))
            {
                mat1.SetTo(new MCvScalar(1,1,1));
                using (Mat mat2 = new Mat(mat1, new Rectangle(50, 50, 50, 50)))
                using (Mat mat3 = new Mat(mat1, new Rectangle(500, 50, 500, 1)))
                {

                    EmguAssert.IsFalse(mat2.IsContinuous);
                    EmguAssert.IsTrue(mat3.IsContinuous);
                    var span1 = mat1.GetSpan<Byte>();
                    int sum = 0;
                    for (int i = 0; i < span1.Length; i++)
                    {
                        sum = sum + span1[i];
                    }
                    EmguAssert.IsTrue(sum == mat1.Width * mat1.Height * mat1.NumberOfChannels);

                    var span3 = mat3.GetSpan<Byte>();
                    sum = 0;
                    for (int i = 0; i < span3.Length; i++)
                    {
                        sum = sum + span3[i];
                    }
                    EmguAssert.IsTrue(sum == mat3.Width * mat3.Height * mat1.NumberOfChannels);
                }
            }
#endif
        }

        [Test]
        public void TestFileReaderMat()
        {
            bool success;
            using (Mat m = new Mat())
                success = Emgu.CV.NativeMatFileIO.ReadFileToMat("scenetext01.jpg", m, ImreadModes.AnyColor);
        }

        #if !NETFX_CORE
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
                if (detector.Process(image))
                {
                    using (CChecker checker = detector.BestColorChecker)
                    {
                        detector.Draw(checker, image, new MCvScalar(0, 255, 0), 1);
                    }
                        /*
                    using (CCheckerDraw drawer = new CCheckerDraw(checker, new MCvScalar(0, 255, 0), 1))
                    {
                        drawer.Draw(image);
                        //image.Save("c:\\tmp.out.png");
                    }
                    //using (Mat img = new Mat(new Size(480, 320), DepthType.Cv8U, 3))
                    //{
                    //    drawer.Draw(img);
                    //}*/
                }
            }

            using (CChecker c = new CChecker())
            {
                PointF p = c.Center;
            }

        }

        [Test]
        public void TestGeoTiffWriter()
        {
            String fileName = Path.Combine(Path.GetTempPath(), "test_geotiff.tif");
            try
            {
                using (Mat image = new Mat(new Size(64, 64), DepthType.Cv8U, 3))
                {
                    image.SetTo(new MCvScalar(128, 64, 32));
                    using (TiffWriter writer = new TiffWriter(fileName))
                    {
                        // Geo tags must be written before the image data is flushed.
                        // ModelTiepoint: [i, j, k, x, y, z] — pixel (0,0) maps to lon/lat (10.0, 50.0)
                        double[] tiepoint = new double[] { 0, 0, 0, 10.0, 50.0, 0.0 };
                        // ModelPixelScale: [scaleX, scaleY, scaleZ] — 0.001 degrees/pixel
                        double[] pixelScale = new double[] { 0.001, 0.001, 0.0 };
                        writer.WriteGeoTag(tiepoint, pixelScale);
                        writer.WriteImage(image);
                    }
                }
                EmguAssert.IsTrue(File.Exists(fileName), "GeoTIFF file was not created");
                EmguAssert.IsTrue(new FileInfo(fileName).Length > 0, "GeoTIFF file is empty");
            }
            finally
            {
                if (File.Exists(fileName))
                    File.Delete(fileName);
            }
        }

        [Test]
        public void TestTileTiffWriter()
        {
            String fileName = Path.Combine(Path.GetTempPath(), "test_tile.tif");
            try
            {
                Size imageSize = new Size(64, 64);
                Size tileSize = new Size(32, 32);
                using (Mat image = new Mat(imageSize, DepthType.Cv8U, 3))
                {
                    image.SetTo(new MCvScalar(100, 150, 200));
                    using (TileTiffWriter writer = new TileTiffWriter(fileName, imageSize, tileSize))
                    {
                        writer.WriteImage(image);
                    }
                }
                EmguAssert.IsTrue(File.Exists(fileName), "Tiled TIFF file was not created");
                EmguAssert.IsTrue(new FileInfo(fileName).Length > 0, "Tiled TIFF file is empty");
            }
            finally
            {
                if (File.Exists(fileName))
                    File.Delete(fileName);
            }
        }

#endif
    }
}
