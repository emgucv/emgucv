using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using Emgu.CV;
using Emgu.CV.UI;
using Emgu.UI;
using Emgu.Util;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;
using System.Xml;
using System.IO;
using System.Runtime.Serialization;

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
            Assert.IsTrue(c1.Equals(c2));
        }

        [Test]
        public void TestCvClipLine()
        {
            MCvPoint m1 = new MCvPoint(-1, 10);
            MCvPoint m2 = new MCvPoint(100, 10);
            int inside = CvInvoke.cvClipLine(new MCvSize(20, 20), ref m1, ref m2);
            Assert.AreEqual(0, m1.x);
            Assert.AreEqual(19, m2.x);
        }

        [Test]
        public void TestLookup()
        {
            double[] b = new double[4] { 0.0, 1.0, 2.0, 3.0 };
            double[] a = new double[4] { 1.0, 3.0, 2.0, 0.0 };
            Point2D<double>[] pts = new Point2D<double>[b.Length];
            for (int i = 0; i < pts.Length; i++)
                pts[i] = new Point2D<double>(b[i], a[i]);

            Assert.AreEqual(2.5, PointCollection.FirstDegreeInterpolate(pts, 1.5));
            Assert.AreEqual(-1, PointCollection.FirstDegreeInterpolate(pts, 3.5));
        }

        [Test]
        public void TestLineFitting()
        {
            List<Point2D<float>> pts = new List<Point2D<float>>();

            pts.Add(new Point2D<float>(1.0f, 1.0f));
            pts.Add(new Point2D<float>(2.0f, 2.0f));
            pts.Add(new Point2D<float>(3.0f, 3.0f));
            pts.Add(new Point2D<float>(4.0f, 4.0f));

            Line2D<float> res = PointCollection.Line2DFitting((IEnumerable<Point<float>>)pts.ToArray(), Emgu.CV.CvEnum.DIST_TYPE.CV_DIST_L2);

            //check if the line is 45 degree from +x axis
            Assert.AreEqual(45.0, res.Direction.PointDegreeAngle);
        }

        [Test]
        public void TestSerialization()
        {
            Rectangle<int> rec = new Rectangle<int>(-10, 10, 10, -2);
            XmlDocument xdoc = Toolbox.XmlSerialize<Rectangle<int>>(rec);
            //Trace.WriteLine(xdoc.OuterXml);
            rec = Toolbox.XmlDeserialize<Rectangle<int>>(xdoc);

            Point2D<double> pt2d = new Point2D<double>(12.0, 5.5);
            xdoc = Toolbox.XmlSerialize<Point2D<double>>(pt2d);
            //Trace.WriteLine(xdoc.OuterXml);
            pt2d = Toolbox.XmlDeserialize<Point2D<double>>(xdoc);

            Circle<float> cir = new Circle<float>(new Point2D<float>(0.0f, 1.0f), 2.8f);
            xdoc = Toolbox.XmlSerialize<Circle<float>>(cir);
            //Trace.WriteLine(xdoc.OuterXml);
            cir = Toolbox.XmlDeserialize<Circle<float>>(xdoc);

            Image<Bgr, Byte> img1 = new Image<Bgr, byte>("stuff.jpg");
            xdoc = Toolbox.XmlSerialize(img1);
            //Trace.WriteLine(xdoc.OuterXml);
            Image<Bgr, Byte> img2 = Toolbox.XmlDeserialize<Image<Bgr, Byte>>(xdoc);

            Byte[] a1 = img1.Bytes;
            Byte[] a2 = img2.Bytes;
            Assert.AreEqual(a1.Length, a2.Length);
            for (int i = 0; i < a1.Length; i++)
            {
                Assert.AreEqual(a1[i], a2[i]);
            }

            img1.Dispose();
            img2.Dispose();
        }

        [Test]
        public void TestRotationMatrix3D()
        {
            float[] rod = new float[] { 0.2f, 0.5f, 0.3f };
            RotationVector3D rodVec = new RotationVector3D(rod);

            RotationVector3D rodVec2 = new RotationVector3D();
            rodVec2.RotationMatrix = rodVec.RotationMatrix;
            Assert.IsTrue(rodVec.Equals(rodVec2));
        }

        [Test]
        public void TestContour()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            using (Image<Gray, Byte> img = new Image<Gray, Byte>(100, 100, new Gray()))
            {
                Rectangle<double> rect = new Rectangle<double>(10, 80, 50, 10);
                img.Draw(rect, new Gray(255.0), -1);

                Point2D<float> pIn = new Point2D<float>(60, 40);
                Point2D<float> pOut = new Point2D<float>(80, 100);

                using (MemStorage stor = new MemStorage())
                {
                    Contour cs = img.FindContours(CvEnum.CHAIN_APPROX_METHOD.CV_CHAIN_APPROX_SIMPLE, CvEnum.RETR_TYPE.CV_RETR_LIST, stor);
                    Assert.AreEqual(rect.Area, cs.Area);
                    Assert.IsTrue(cs.Convex);
                    Assert.AreEqual(rect.Width * 2 + rect.Height * 2, cs.Perimeter);
                    Rectangle<double> rect2 = cs.BoundingRectangle;
                    rect2.Width -= 1;
                    rect2.Height -= 1;
                    rect2.Center.X -= 0.5;
                    rect2.Center.Y -= 0.5;
                    Assert.IsTrue(rect2.Equals(rect));
                    Assert.AreEqual(cs.InContour(pIn), 100);
                    Assert.AreEqual(cs.InContour(pOut), -100);
                    Assert.AreEqual(cs.Distance(pIn), 10);
                    Assert.AreEqual(cs.Distance(pOut), -50);
                    img.Draw(cs, new Gray(100), new Gray(100), 0, 1);
                }
            }
        }

        [Test]
        public void TestException()
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
                    Assert.AreEqual(-201, excpt.Status);
                    exceptionCaught = true;
                }
                Assert.IsTrue(exceptionCaught);
            }
        }

        [Test]
        public void TestRectangle()
        {
            Matrix<Byte> mat = new Matrix<Byte>(1, 4);
            mat.SetRandUniform(new MCvScalar(), new MCvScalar(255.0));

            MCvRect rect1 = new MCvRect((int)mat[0, 0], (int)mat[0, 1], (int)mat[0, 2], (int)mat[0, 3]);
            Rectangle<double> rectangle = new Rectangle<double>(rect1);
            MCvRect rect2 = rectangle.MCvRect;

            Assert.AreEqual(rect1.x, rect2.x);
            Assert.AreEqual(rect1.y, rect2.y);

        }

        [Test]
        public void TestEigenObjects()
        {
            String[] fileNames = new string[] { "stuff.jpg", "squares.gif", "lena.jpg" };
            
            int width = 100, height = 100;
            MCvTermCriteria termCrit = new MCvTermCriteria(3, 0.001);

            #region using batch method
            Image<Gray, Byte>[] imgs = Array.ConvertAll<String, Image<Gray, Byte>>(fileNames,
                delegate(String file)
                {
                    return new Image<Gray, Byte>(file).Resize(width, height);
                });

            EigenObjectRecognizer imgRecognizer1 = new EigenObjectRecognizer(imgs, ref termCrit);
            for (int i = 0; i < imgs.Length; i++)
            {
                Assert.AreEqual(i.ToString(), imgRecognizer1.Recognize(imgs[i]));
            }

            XmlDocument xDoc = Toolbox.XmlSerialize<EigenObjectRecognizer>(imgRecognizer1);
            EigenObjectRecognizer imgRecognizer2 = Toolbox.XmlDeserialize<EigenObjectRecognizer>(xDoc);

            for (int i = 0; i < imgs.Length; i++)
            {
                Assert.AreEqual(i.ToString(), imgRecognizer2.Recognize(imgs[i]));
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
                EigenObjectRecognizer imgRecognizer3 = (EigenObjectRecognizer)formatter.Deserialize(ms2);
                for (int i = 0; i < imgs.Length; i++)
                {
                    Assert.AreEqual(i.ToString(), imgRecognizer3.Recognize(imgs[i]));
                }
            }
            #endregion
        }
        [Test]
        public void TestPointInPolygon()
        {
            Triangle2D<float> tri = new Triangle2D<float>(
                new Point2D<float>(-10, -10),
                new Point2D<float>(0, 10),
                new Point2D<float>(10, -10));

            Rectangle<float> rect = new Rectangle<float>(
                new Point2D<float>(0.0f, 0.0f),
                10f, 10f);

            Point2D<float> p1 = new Point2D<float>(0, 0);
            Point2D<float> p2 = new Point2D<float>(-20, -20);
            
            Assert.IsTrue(p1.InConvexPolygon(tri));
            Assert.IsTrue(p1.InConvexPolygon(rect));
            Assert.IsFalse(p2.InConvexPolygon(tri));
            Assert.IsFalse(p2.InConvexPolygon(rect));
        }

        [Test]
        public void TestXmlSerialize()
        {
            Point2D<float> p = new Point2D<float>(0.0f, 0.0f);
            XmlDocument xDoc = Toolbox.XmlSerialize<Point2D<float>>(p);
            Point2D<float> p2 = Toolbox.XmlDeserialize<Point2D<float>>(xDoc);
            Assert.IsTrue(p.Equals(p2));
        }

        [Test]
        public void TestTriangle()
        {
            Point2D<double> p1 = new Point2D<double>(0, 0);
            Point2D<double> p2 = new Point2D<double>(1, 0);
            Point2D<double> p3 = new Point2D<double>(0, 1);
            Triangle2D<double> tri = new Triangle2D<double>(p1, p2, p3);
            double epsilon = 1e-10;
            Assert.IsTrue( Math.Abs(tri.Area - 0.5) < epsilon);
        }

        [Test]
        public void TestLine()
        {
            Point2D<double> p1 = new Point2D<double>(0, 0);
            Point2D<double> p2 = new Point2D<double>(1, 0);
            Point2D<double> p3 = new Point2D<double>(0, 1);
            LineSegment2D<double> l1 = new LineSegment2D<double>(p1, p2);
            LineSegment2D<double> l2 = new LineSegment2D<double>(p1, p3);
            double angle = l1.GetExteriorAngleDegree(l2);
            Assert.AreEqual(angle, 90.0);
        }

        [Test]
        public void GetBox2DPoints()
        {
            MCvBox2D box = new MCvBox2D();
            box.center = new MCvPoint2D32f(3.0f, 2.0f);
            box.size = new MCvSize2D32f(4.0f, 6.0f);
            Point2D<float>[] vertices = box.Vertices;
            Assert.IsTrue(vertices[0].Equals(new Point2D<float>(0.0f, 0.0f))); 
        }

        [Test]
        public void TestGrayscaleBitmapConstructor()
        {
            Image<Bgra, Byte> img = new Image<Bgra, byte>(320, 240);
            img.SetRandUniform(new MCvScalar(), new MCvScalar(255, 255, 255, 255));
            img.Save("tmp.png");

            DateTime t0 = DateTime.Now;
            Image<Bgra, Byte> img2 = new Image<Bgra, byte>("tmp.png");
            //Trace.WriteLine(string.Format("Time: {0} milliseconds", DateTime.Now.Subtract(t0).TotalMilliseconds));
            Assert.IsTrue(img.Equals(img2));

            DateTime t1 = DateTime.Now;
            Image<Gray, Byte> img3 = new Image<Gray, byte>("tmp.png");
            //Trace.WriteLine(string.Format("Time: {0} milliseconds", DateTime.Now.Subtract(t1).TotalMilliseconds));
            Assert.IsTrue(img.Convert<Gray, Byte>().Equals(img3));
        }
    }
}
