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
using Emgu.CV.Flann;
using Emgu.CV.Stitching;
using Emgu.CV.Text;
using Emgu.CV.Structure;
using Emgu.CV.Bioinspired;
using Emgu.CV.Dpm;
using Emgu.CV.Face;
using Emgu.CV.Freetype;
using Emgu.CV.StructuredLight;
using Emgu.CV.Dnn;
using Emgu.CV.Cuda;
using Emgu.CV.Models;
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
            for (int i = 0; i < 10; i++)
            {
                bool exceptionCaught = false;
                using (Mat mat = new Mat(20, 30, DepthType.Cv8U, 1))
                {
                    try
                    {
                        double det = CvInvoke.Determinant(mat);
                    }
                    catch (CvException excpt)
                    {
                        EmguAssert.AreEqual(-215, excpt.Status);
                        exceptionCaught = true;
                    }
                }

                EmguAssert.IsTrue(exceptionCaught);
            }
        }

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
        [Test]
        public void TestCrossProduct()
        {
            MCvPoint3D32f p1 = new MCvPoint3D32f(1.0f, 0.0f, 0.0f);
            MCvPoint3D32f p2 = new MCvPoint3D32f(0.0f, 1.0f, 0.0f);
            MCvPoint3D32f p3 = p1.CrossProduct(p2);
            EmguAssert.IsTrue(new MCvPoint3D32f(0.0f, 0.0f, 1.0f).Equals(p3));
        }


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

#endif
    }
}
