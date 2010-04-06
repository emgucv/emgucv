using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using Emgu.CV;
using Emgu.CV.UI;
using Emgu.CV.Structure;
using Emgu.CV.VideoSurveillance;
using Emgu.UI;
using Emgu.Util;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Serialization;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.InteropServices;
using System.Linq;

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
         Point m1 = new Point(-1, 10);
         Point m2 = new Point(100, 10);
         int inside = CvInvoke.cvClipLine(new Size(20, 20), ref m1, ref m2);
         Assert.AreEqual(0, m1.X);
         Assert.AreEqual(19, m2.X);
      }

      [Test]
      public void TestDenseHistogramRuntimeSerialization()
      {
         Image<Gray, Byte> img = new Image<Gray, byte>(400, 400);
         img.SetRandUniform(new MCvScalar(), new MCvScalar(255));
         DenseHistogram hist = new DenseHistogram(256, new RangeF(0.0f, 255.0f));
         hist.Calculate<Byte>(new Image<Gray, byte>[] { img }, true, null);

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
               Assert.IsTrue(hist.Equals(hist2));
            }
         }
      }

      [Test]
      public void TestLookup()
      {
         double[] b = new double[4] { 0, 1, 2, 3 };
         double[] a = new double[4] { 1, 3, 2, 0 };
         MCvPoint2D64f [] pts = new MCvPoint2D64f[b.Length];
         for (int i = 0; i < pts.Length; i++)
            pts[i] = new MCvPoint2D64f(b[i], a[i]);

         IEnumerable<MCvPoint2D64f> interPts = Toolbox.LinearInterpolate(pts, new double[2] { 1.5, 3.5 });
         IEnumerator<MCvPoint2D64f> enumerator = interPts.GetEnumerator();
         enumerator.MoveNext();
         Assert.AreEqual(1.5, enumerator.Current.x);
         Assert.AreEqual(2.5, enumerator.Current.y);
         enumerator.MoveNext();
         Assert.AreEqual(3.5, enumerator.Current.x);
         Assert.AreEqual(-1, enumerator.Current.y);
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
         PointCollection.Line2DFitting(pts.ToArray(), Emgu.CV.CvEnum.DIST_TYPE.CV_DIST_L2, out direction, out pointOnLine);

         //check if the line is 45 degree from +x axis
         Assert.AreEqual(45.0, Math.Atan2(direction.Y, direction.X) * 180.0 / Math.PI);
      }

      [Test]
      public void TestXmlSerialization()
      {
         MCvPoint2D64f pt2d = new MCvPoint2D64f(12.0, 5.5);

         XmlDocument xdoc = Toolbox.XmlSerialize<MCvPoint2D64f>(pt2d);
         //Trace.WriteLine(xdoc.OuterXml);
         pt2d = Toolbox.XmlDeserialize<MCvPoint2D64f>(xdoc);

         CircleF cir = new CircleF(new PointF(0.0f, 1.0f), 2.8f);
         xdoc = Toolbox.XmlSerialize<CircleF>(cir);
         //Trace.WriteLine(xdoc.OuterXml);
         cir = Toolbox.XmlDeserialize<CircleF>(xdoc);

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
         double[] rod = new double[] { 0.2, 0.5, 0.3 };
         RotationVector3D rodVec = new RotationVector3D(rod);

         RotationVector3D rodVec2 = new RotationVector3D();
         rodVec2.RotationMatrix = rodVec.RotationMatrix;
         Matrix<double> diff = rodVec - rodVec2;
         Assert.IsTrue(diff.Norm < 1.0e-8);
      }

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

            using (MemStorage stor = new MemStorage())
            {
               Contour<Point> cs = img.FindContours(CvEnum.CHAIN_APPROX_METHOD.CV_CHAIN_APPROX_SIMPLE, CvEnum.RETR_TYPE.CV_RETR_LIST, stor);
               Assert.AreEqual(cs.MCvContour.elem_size, Marshal.SizeOf(typeof(Point)));
               Assert.AreEqual(rect.Width * rect.Height, cs.Area);

               Assert.IsTrue(cs.Convex);
               Assert.AreEqual(rect.Width * 2 + rect.Height * 2, cs.Perimeter);
               Rectangle rect2 = cs.BoundingRectangle;
               rect2.Width -= 1;
               rect2.Height -= 1;
               //rect2.Center.X -= 0.5;
               //rect2.Center.Y -= 0.5;
               Assert.IsTrue(rect2.Equals(rect));
               Assert.AreEqual(cs.InContour(pIn), 100);
               Assert.AreEqual(cs.InContour(pOut), -100);
               Assert.AreEqual(cs.Distance(pIn), 10);
               Assert.AreEqual(cs.Distance(pOut), -50);
               img.Draw(cs, new Gray(100), new Gray(100), 0, 1);

               MCvPoint2D64f rectangleCenter = new MCvPoint2D64f(rect.X + rect.Width / 2.0, rect.Y + rect.Height / 2.0);
               MCvMoments moment = cs.GetMoments();
               MCvPoint2D64f center = moment.GravityCenter;
               Assert.AreEqual(center, rectangleCenter);
            }

            using (MemStorage stor = new MemStorage())
            {
               Image<Gray, Byte> img2 = new Image<Gray, byte>(300, 200);
               Contour<Point> c = img2.FindContours(Emgu.CV.CvEnum.CHAIN_APPROX_METHOD.CV_CHAIN_APPROX_SIMPLE, Emgu.CV.CvEnum.RETR_TYPE.CV_RETR_LIST, stor);
               Assert.AreEqual(c, null);
            }
         }

         int s1 = Marshal.SizeOf(typeof(MCvSeq));
         int s2 = Marshal.SizeOf(typeof(MCvContour));
         int sizeRect = Marshal.SizeOf(typeof(Rectangle));
         Assert.AreEqual(s1 + sizeRect + 4 * Marshal.SizeOf(typeof(int)), s2);
      }

      [Test]
      public void TestConvexityDefacts()
      {
         Image<Gray, Byte> image = new Image<Gray, byte>(300, 300);
         Point[] polyline = new Point[] {
            new Point(10, 10),
            new Point(10, 250),
            new Point(100, 100),
            new Point(250, 250),
            new Point(250, 10)};

         using (MemStorage stor = new MemStorage())
         {
            Seq<Point> contour = new Seq<Point>(stor);
            contour.PushMulti(polyline, Emgu.CV.CvEnum.BACK_OR_FRONT.FRONT);
            image.Draw(contour, new Gray(255), 1);
            Seq<MCvConvexityDefect> defactSeq =
               contour.GetConvexityDefacts(
                  stor,
                  Emgu.CV.CvEnum.ORIENTATION.CV_CLOCKWISE);
            MCvConvexityDefect[] defacts = defactSeq.ToArray();
            Assert.AreEqual(1, defacts.Length);
            Assert.AreEqual(new Point(100, 100), defacts[0].DepthPoint);

            Assert.IsTrue(contour.InContour(new PointF(90, 90)) > 0);
            Assert.IsTrue(contour.InContour(new PointF(300, 300)) < 0);
            Assert.IsTrue(contour.InContour(new PointF(10, 10)) == 0);
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
               Assert.AreEqual(-215, excpt.Status);
               exceptionCaught = true;
            }
            Assert.IsTrue(exceptionCaught);
         }
      }

      [Test]
      public void TestProjectPoints()
      {
         IntrinsicCameraParameters intrin = new IntrinsicCameraParameters();
         intrin.IntrinsicMatrix.SetIdentity();
         ExtrinsicCameraParameters extrin = new ExtrinsicCameraParameters();
         MCvPoint3D32f point = new MCvPoint3D32f();

         PointF[] points = CameraCalibration.ProjectPoints(new MCvPoint3D32f[] { point }, extrin, intrin);
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
         param.BlobDetector = new BlobDetector(Emgu.CV.CvEnum.BLOB_DETECTOR_TYPE.CC);
         //param.FGDetector = new FGDetector<Gray>(Emgu.CV.CvEnum.FORGROUND_DETECTOR_TYPE.FGD);
         param.BlobTracker = new BlobTracker(Emgu.CV.CvEnum.BLOBTRACKER_TYPE.MSFGS);
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
         param.BlobDetector = new BlobDetector(Emgu.CV.CvEnum.BLOB_DETECTOR_TYPE.CC);
         //param.FGDetector = new FGDetector<Gray>(Emgu.CV.CvEnum.FORGROUND_DETECTOR_TYPE.FGD);
         param.BlobTracker = new BlobTracker(Emgu.CV.CvEnum.BLOBTRACKER_TYPE.MSFGS);
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
         //ImageViewer.Show(forground);
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
                return new Image<Gray, Byte>(file).Resize(width, height, CvEnum.INTER.CV_INTER_LINEAR);
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

      /*
      [Test]
      public void TestPointInPolygon()
      {
         Triangle2D tri = new Triangle2D(
             new Point2D<float>(-10, -10),
             new Point2D<float>(0, 10),
             new Point2D<float>(10, -10));

         Rectangle<float> rect = new Rectangle(
             new Point2D<float>(0.0f, 0.0f),
             10f, 10f);

         Point2D<float> p1 = new Point2D<float>(0, 0);
         Point2D<float> p2 = new Point2D<float>(-20, -20);

         Assert.IsTrue(p1.InConvexPolygon(tri));
         Assert.IsTrue(p1.InConvexPolygon(rect));
         Assert.IsFalse(p2.InConvexPolygon(tri));
         Assert.IsFalse(p2.InConvexPolygon(rect));
      }*/



      private static float[,] ProjectPoints(float[,] points3D, RotationVector3D rotation, Matrix<double> translation, float focalLength)
      {
         using (Matrix<float> imagePointMat = new Matrix<float>(points3D.GetLength(0), 2))
         {
            CvInvoke.cvProjectPoints2(new Matrix<float>(points3D), rotation, translation,
            new Matrix<double>(new double[,] { 
               {focalLength, 0, 0},
               {0, focalLength, 0}, 
               {0,0,1}}),
            IntPtr.Zero, imagePointMat, IntPtr.Zero, IntPtr.Zero, IntPtr.Zero, IntPtr.Zero, IntPtr.Zero);
            return imagePointMat.Data;
         }
      }

      /*
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
         XmlDocument xDoc = Toolbox.XmlSerialize<PointF>(p, new Type[] { typeof(Point) });
         PointF p2 = Toolbox.XmlDeserialize<PointF>(xDoc, new Type[] { typeof(Point) });
         Assert.IsTrue(p.Equals(p2));


         Rectangle rect = new Rectangle(3, 4, 5, 3);
         XmlDocument xDoc2 = Toolbox.XmlSerialize<Rectangle>(rect);
         Rectangle rect2 = Toolbox.XmlDeserialize<Rectangle>(xDoc2);
         Assert.IsTrue(rect.Equals(rect2));

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
         Assert.IsTrue(Math.Abs(tri.Area - 0.5) < epsilon);
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
         Assert.AreEqual(angle, 90.0);
      }

      [Test]
      public void TestGetBox2DPoints()
      {
         MCvBox2D box = new MCvBox2D(
            new PointF(3.0f, 2.0f),
            new SizeF(4.0f, 6.0f),
            0.0f);
         PointF[] vertices = box.GetVertices();
         Assert.IsTrue(vertices[0].Equals(new PointF(0.0f, 0.0f)));
         Assert.IsTrue(vertices[1].Equals(new PointF(6.0f, 0.0f)));
      }

      [Test]
      public void TestGrayscaleBitmapConstructor()
      {
         Image<Bgra, Byte> img = new Image<Bgra, byte>(320, 240);
         img.SetRandUniform(new MCvScalar(), new MCvScalar(255, 255, 255, 255));
         img.Save("tmp.png");

         Stopwatch stopwatch = Stopwatch.StartNew();
         Image<Bgra, Byte> img2 = new Image<Bgra, byte>("tmp.png");
         stopwatch.Stop();
         Trace.WriteLine(string.Format("Time: {0} milliseconds", stopwatch.ElapsedMilliseconds));
         Assert.IsTrue(img.Equals(img2));

         stopwatch.Reset(); stopwatch.Start();
         using (Bitmap bmp = new Bitmap("tmp.png"))
         using (Image bmpImage = Bitmap.FromFile("tmp.png"))
         {
            Assert.AreEqual(System.Drawing.Imaging.PixelFormat.Format32bppArgb, bmpImage.PixelFormat);

            Image<Gray, Byte> img3 = new Image<Gray, byte>(bmp);
            stopwatch.Stop();
            Trace.WriteLine(string.Format("Time: {0} milliseconds", stopwatch.ElapsedMilliseconds));
            Image<Gray, Byte> diff = img.Convert<Gray, Byte>().AbsDiff(img3);
            Assert.AreEqual(0, CvInvoke.cvCountNonZero(diff));
            Assert.IsTrue(img.Convert<Gray, Byte>().Equals(img3));
         }
      }

      [Test]
      public void TestMorphEx()
      {
         StructuringElementEx element1 = new StructuringElementEx(3, 3, 1, 1, Emgu.CV.CvEnum.CV_ELEMENT_SHAPE.CV_SHAPE_CROSS);
         StructuringElementEx element2 = new StructuringElementEx(new int[3, 3] { { 0, 1, 0 }, { 1, 0, 1 }, { 0, 1, 0 } }, 1, 1);
         Image<Bgr, Byte> tmp = new Image<Bgr, byte>(100, 100);
         Image<Bgr, Byte> tmp2 = tmp.MorphologyEx(element1, Emgu.CV.CvEnum.CV_MORPH_OP.CV_MOP_GRADIENT, 1);
         Image<Bgr, Byte> tmp3 = tmp.MorphologyEx(element2, Emgu.CV.CvEnum.CV_MORPH_OP.CV_MOP_BLACKHAT, 1);
      }

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

         BGStatModel<Bgr> model1 = new BGStatModel<Bgr>(img1, Emgu.CV.CvEnum.BG_STAT_TYPE.GAUSSIAN_BG_MODEL);
         model1.Update(img2);

         BGStatModel<Bgr> model2 = new BGStatModel<Bgr>(img1, Emgu.CV.CvEnum.BG_STAT_TYPE.FGD_STAT_MODEL);
         model2.Update(img2);

         //ImageViewer.Show(model2.Foreground);
         //ImageViewer.Show(model1.Background);
      }


      [Test]
      public void TestPlanarSubdivision1()
      {
         int pointCount = 10000;

         #region generate random points
         PointF[] points = new PointF[pointCount];
         Random r = new Random((int)DateTime.Now.Ticks);
         for (int i = 0; i < points.Length; i++)
         {
            points[i] = new PointF((float)(r.NextDouble() * 20), (float)(r.NextDouble() * 20));
         }
         #endregion

         PlanarSubdivision division;

         Stopwatch watch = Stopwatch.StartNew();
         division = new PlanarSubdivision(points, true);
         Triangle2DF[] triangles = division.GetDelaunayTriangles(false);
         watch.Stop();
         Trace.WriteLine(String.Format("{0} milli-seconds, {1} triangles", watch.ElapsedMilliseconds, triangles.Length));
         watch.Reset();

         Assert.IsTrue(CvInvoke.icvSubdiv2DCheck(division) == 1);
         
         watch.Start();
         division = new PlanarSubdivision(points);
         VoronoiFacet[] facets = division.GetVoronoiFacets();
         watch.Stop();
         Trace.WriteLine(String.Format("{0} milli-seconds, {1} facets", watch.ElapsedMilliseconds, facets.Length));

         //foreach (Triangle2DF t in triangles)
         //{
         //int equalCount = triangles.FindAll(delegate(Triangle2DF t2) { return t2.Equals(t); }).Count;
         //Assert.AreEqual(1, equalCount, "Triangle duplicates");

         //int overlapCount = triangles.FindAll(delegate(Triangle2D t2) { return Util.IsConvexPolygonInConvexPolygon(t2, t);}).Count;
         //Assert.AreEqual(1, overlapCount, "Triangle overlaps");
         //}
      }

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

         PlanarSubdivision subdiv = new PlanarSubdivision(pts);
         for (int i = 0; i < pts.Length; i++)
         {
            MCvSubdiv2DEdge? edge;
            MCvSubdiv2DPoint? point;
            CvEnum.Subdiv2DPointLocationType location = subdiv.Locate(ref pts[i], out edge, out point);
            if (location == Emgu.CV.CvEnum.Subdiv2DPointLocationType.ON_EDGE)
            {
               //you might want to store the points which is not inserted here.
               //or alternatively, add some random noise to the point and re-insert it again.
               continue;
            }
            subdiv.Insert(pts[i]);
         }

         VoronoiFacet[] facets = subdiv.GetVoronoiFacets();
      }

      [Test]
      public void TestGetModuleInfo()
      {
         Image<Bgr, Byte> img = new Image<Bgr, byte>(200, 100);
         img.Sobel(1, 0, 3);
         String plugin, module;
         Util.GetModuleInfo(out plugin, out module);
      }

      [Test]
      public void TestCrossProduct()
      {
         MCvPoint3D32f p1 = new MCvPoint3D32f(1.0f, 0.0f, 0.0f);
         MCvPoint3D32f p2 = new MCvPoint3D32f(0.0f, 1.0f, 0.0f);
         MCvPoint3D32f p3 = p1.CrossProduct(p2);
         Assert.IsTrue(new MCvPoint3D32f(0.0f, 0.0f, 1.0f).Equals(p3));
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

         Image<Gray, Single> match = img.MatchTemplate(randomObj, Emgu.CV.CvEnum.TM_TYPE.CV_TM_SQDIFF);
         double[] minVal, maxVal;
         Point[] minLoc, maxLoc;
         match.MinMax(out minVal, out maxVal, out minLoc, out maxLoc);

         Assert.AreEqual(minLoc[0].X, templCenter.X - templWidth / 2);
         Assert.AreEqual(minLoc[0].Y, templCenter.Y - templHeight / 2);
      }

      /// <summary>
      /// Prepare synthetic image for testing
      /// </summary>
      /// <param name="prevImg"></param>
      /// <param name="currImg"></param>
      private static void OptiocalFlowImage(out Image<Gray, Byte> prevImg, out Image<Gray, Byte> currImg)
      {
         //Create a random object
         Image<Gray, Byte> randomObj = new Image<Gray, byte>(50, 50);
         randomObj.SetRandUniform(new MCvScalar(), new MCvScalar(255));

         //Draw the object in image1 center at (100, 100);
         prevImg = new Image<Gray, byte>(300, 200);
         Rectangle objectLocation = new Rectangle(75, 75, 50, 50);
         prevImg.ROI = objectLocation;
         randomObj.Copy(prevImg, null);
         prevImg.ROI = Rectangle.Empty;

         //Draw the object in image2 center at (102, 103);
         currImg = new Image<Gray, byte>(300, 200);
         objectLocation.Offset(2, 3);
         currImg.ROI = objectLocation;
         randomObj.Copy(currImg, null);
         currImg.ROI = Rectangle.Empty;
      }

      [Test]
      public void TestOpticalFlowFarneback()
      {
         Image<Gray, Byte> prevImg, currImg;
         OptiocalFlowImage(out prevImg, out currImg);
         Image<Gray, Single> flowx = new Image<Gray, float>(prevImg.Size);
         Image<Gray, Single> flowy = new Image<Gray, float>(prevImg.Size);
         OpticalFlow.Farneback(prevImg, currImg, flowx, flowy, 0.5, 3, 5, 20, 7, 1.5, Emgu.CV.CvEnum.OPTICALFLOW_FARNEBACK_FLAG.DEFAULT);
      }

      [Test]
      public void TestOpticalFlowBM()
      {
         Image<Gray, Byte> prevImg, currImg;
         OptiocalFlowImage(out prevImg, out currImg);
         Size blockSize = new Size(5, 5);
         Size shiftSize = new Size(1, 1);
         Size maxRange = new Size(10, 10);
         Size velSize = new Size(
            (int)Math.Floor((prevImg.Width - blockSize.Width) / (double)shiftSize.Width),
            (int)Math.Floor((prevImg.Height - blockSize.Height) / (double)shiftSize.Height));
         Image<Gray, float> velx = new Image<Gray, float>(velSize);
         Image<Gray, float> vely = new Image<Gray, float>(velSize);

         Stopwatch watch = Stopwatch.StartNew();

         OpticalFlow.BM(prevImg, currImg, blockSize, shiftSize, maxRange, false, velx, vely);

         watch.Stop();

         Trace.WriteLine(String.Format(
            "Time: {0} milliseconds",
            watch.ElapsedMilliseconds));

      }

      [Test]
      public void TestOpticalFlowLK()
      {
         Image<Gray, Byte> prevImg, currImg;
         OptiocalFlowImage(out prevImg, out currImg);

         PointF[] prevFeature = new PointF[] { new PointF(100f, 100f) };

         PointF[] currFeature;
         Byte[] status;
         float[] trackError;

         Stopwatch watch = Stopwatch.StartNew();

         OpticalFlow.PyrLK(
            prevImg, currImg, prevFeature, new Size(10, 10), 3, new MCvTermCriteria(10, 0.01),
            out currFeature, out status, out trackError);
         watch.Stop();
         Trace.WriteLine(String.Format(
            "prev: ({0}, {1}); curr: ({2}, {3}); \r\nTime: {4} milliseconds",
            prevFeature[0].X, prevFeature[0].Y,
            currFeature[0].X, currFeature[0].Y,
            watch.ElapsedMilliseconds));

      }

      [Test]
      public void TestChessboardCalibration()
      {
         Size patternSize = new Size(6, 6);

         Image<Gray, Byte> chessboardImage = new Image<Gray, byte>("chessBoard.jpg");
         PointF[] corners;
         bool patternFound =
            CameraCalibration.FindChessboardCorners(
            chessboardImage,
            patternSize,
            Emgu.CV.CvEnum.CALIB_CB_TYPE.ADAPTIVE_THRESH | Emgu.CV.CvEnum.CALIB_CB_TYPE.NORMALIZE_IMAGE | Emgu.CV.CvEnum.CALIB_CB_TYPE.FILTER_QUADS,
            out corners);

         chessboardImage.FindCornerSubPix(
            new PointF[][] { corners },
            new Size(10, 10),
            new Size(-1, -1),
            new MCvTermCriteria(0.05));

         CameraCalibration.DrawChessboardCorners(chessboardImage, patternSize, corners, patternFound);
      }

      /*
      [Test]
      public void TestFillConvexPolygon()
      {
         Image<Bgr, Byte> img = new Image<Bgr, byte>(200, 200);
         Rectangle<double> rect = new Rectangle<double>(new Point2D<double>(100, 100), 50, 50);
         img.Draw((IConvexPolygon<double>)rect, new Bgr(Color.Blue), 0);
         //Application.Run(new ImageViewer(img));
      }*/

      [Test]
      public void TestKDTree()
      {
         float[][] features = new float[10][];
         for (int i = 0; i < features.Length; i++)
            features[i] = new float[] { (float)i };
         FeatureTree tree = new FeatureTree(features);

         Matrix<Int32> result;
         Matrix<double> distance;
         float[][] features2 = new float[1][];
         features2[0] = new float[] { 5.0f };

         tree.FindFeatures(features2, out result, out distance, 1, 20);
         Assert.AreEqual(result[0, 0], 5);
         Assert.AreEqual(distance[0, 0], 0.0);
      }

      [Test]
      public void TestSpillTree()
      {
         float[][] features = new float[10][];
         for (int i = 0; i < features.Length; i++)
            features[i] = new float[] { (float)i };
         FeatureTree tree = new FeatureTree(features, 50, .7, .1);

         Matrix<Int32> result;
         Matrix<double> distance;
         float[][] features2 = new float[1][];
         features2[0] = new float[] { 5.0f };

         tree.FindFeatures(features2, out result, out distance, 1, 20);
         Assert.AreEqual(result[0, 0], 5);
         Assert.AreEqual(distance[0, 0], 0.0);
      }

      [Test]
      public void TestFlannLinear()
      {
         float[][] features = new float[10][];
         for (int i = 0; i < features.Length; i++)
            features[i] = new float[] { (float)i };

         Flann.Index index = new Flann.Index(Util.GetMatrixFromDescriptors(features));

         float[][] features2 = new float[1][];
         features2[0] = new float[] { 5.0f };

         Matrix<int> indices = new Matrix<int>(features2.Length, 1);
         Matrix<float> distances = new Matrix<float>(features2.Length, 1);
         index.KnnSearch(Util.GetMatrixFromDescriptors(features2), indices, distances, 1, 32);

         Assert.AreEqual(indices[0, 0], 5);
         Assert.AreEqual(distances[0, 0], 0.0);
      }

      [Test]
      public void TestFlannKDTree()
      {
         float[][] features = new float[10][];
         for (int i = 0; i < features.Length; i++)
            features[i] = new float[] { (float)i };

         Flann.Index index = new Flann.Index(Util.GetMatrixFromDescriptors(features), 4);

         float[][] features2 = new float[1][];
         features2[0] = new float[] { 5.0f };

         Matrix<int> indices = new Matrix<int>(features2.Length, 1);
         Matrix<float> distances = new Matrix<float>(features2.Length, 1);
         index.KnnSearch(Util.GetMatrixFromDescriptors(features2), indices, distances, 1, 32);

         Assert.AreEqual(indices[0, 0], 5);
         Assert.AreEqual(distances[0, 0], 0.0);
      }

      [Test]
      public void TestFlannCompositeTree()
      {
         float[][] features = new float[10][];
         for (int i = 0; i < features.Length; i++)
            features[i] = new float[] { (float)i };

         Flann.Index index = new Flann.Index(Util.GetMatrixFromDescriptors(features), 4, 32, 11, Emgu.CV.Flann.CenterInitType.RANDOM, 0.2f);

         float[][] features2 = new float[1][];
         features2[0] = new float[] { 5.0f };

         Matrix<int> indices = new Matrix<int>(features2.Length, 1);
         Matrix<float> distances = new Matrix<float>(features2.Length, 1);
         index.KnnSearch(Util.GetMatrixFromDescriptors(features2), indices, distances, 1, 32);

         Assert.AreEqual(indices[0, 0], 5);
         Assert.AreEqual(distances[0, 0], 0.0);
      }

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

      //TODO: find out why CameraCalibration test go to infinite loop since svn 1611
      /*
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
            Assert.IsTrue(contour.Convex);
            Assert.AreEqual(contour.Area, 4.0);
            //InContour function requires MCvContour.rect to be pre-computed
            CvInvoke.cvBoundingRect(contour, 1);
            Assert.GreaterOrEqual(contour.InContour(new Point(1, 1)), 0);
            Assert.Less(contour.InContour(new Point(3, 3)), 0);

            Contour<PointF> contourF = new Contour<PointF>(stor);
            contourF.Push(new PointF(0, 0));
            contourF.Push(new PointF(0, 2));
            contourF.Push(new PointF(2, 2));
            contourF.Push(new PointF(2, 0));
            Assert.IsTrue(contourF.Convex);
            Assert.AreEqual(contourF.Area, 4.0);
            //InContour function requires MCvContour.rect to be pre-computed
            CvInvoke.cvBoundingRect(contourF, 1);
            Assert.GreaterOrEqual(contourF.InContour(new PointF(1, 1)), 0);
            Assert.Less(contourF.InContour(new PointF(3, 3)), 0);

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

            Seq<Point> seq = new Seq<Point>(CvInvoke.CV_MAKETYPE(4, 2), stor);
            seq.Push(new Point(0, 0));
            seq.Push(new Point(0, 1));
            seq.Push(new Point(1, 1));
            seq.Push(new Point(1, 0));
         }
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

         Image<Bgr, Byte> img = new Image<Bgr, byte>(600, 600, new Bgr(255.0, 255.0, 255.0));
         //Draw the points 
         foreach (PointF p in pts)
            img.Draw(new CircleF(p, 3), new Bgr(0.0, 0.0, 0.0), 1);

         //Find and draw the convex hull
         using (MemStorage storage = new MemStorage())
         {
            Stopwatch watch = Stopwatch.StartNew();
            PointF[] hull = PointCollection.ConvexHull(pts, storage, Emgu.CV.CvEnum.ORIENTATION.CV_CLOCKWISE).ToArray();
            watch.Stop();
            img.DrawPolyline(
                Array.ConvertAll<PointF, Point>(hull, Point.Round),
                true, new Bgr(255.0, 0.0, 0.0), 1);

            //ImageViewer.Show(img, String.Format("Convex Hull Computed in {0} milliseconds", watch.ElapsedMilliseconds));
         }
      }

      [Test]
      public void TestStereoGCCorrespondence()
      {
         Image<Gray, Byte> left = new Image<Gray, byte>("left.jpg");
         Image<Gray, Byte> right = new Image<Gray, byte>("right.jpg");
         Image<Gray, Int16> leftDisparity = new Image<Gray, Int16>(left.Size);
         Image<Gray, Int16> rightDisparity = new Image<Gray, Int16>(left.Size);

         StereoGC stereoSolver = new StereoGC(10, 5);
         Stopwatch watch = Stopwatch.StartNew();
         stereoSolver.FindStereoCorrespondence(left, right, leftDisparity, rightDisparity);
         watch.Stop();
         Trace.WriteLine(String.Format("Time used: {0} milliseconds", watch.ElapsedMilliseconds));

         Matrix<double> q = new Matrix<double>(4, 4);
         q.SetIdentity();
         MCvPoint3D32f[] points = PointCollection.ReprojectImageTo3D(leftDisparity * (-16), q);

         float min = (float)1.0e10, max = 0;
         foreach (MCvPoint3D32f p in points)
         {
            if (p.z < min) min = p.z;
            else if (p.z > max) max = p.z;
         }
         Trace.WriteLine(String.Format("Min : {0}\r\nMax : {1}", min, max));

         //ImageViewer.Show(leftDisparity*(-16));
      }

      [Test]
      public void TestStereoBMCorrespondence()
      {
         Image<Gray, Byte> left = new Image<Gray, byte>("left.jpg");
         Image<Gray, Byte> right = new Image<Gray, byte>("right.jpg");
         Image<Gray, Int16> leftDisparity = new Image<Gray, Int16>(left.Size);
         Image<Gray, Int16> rightDisparity = new Image<Gray, Int16>(left.Size);

         StereoBM bm = new StereoBM(Emgu.CV.CvEnum.STEREO_BM_TYPE.BASIC, 0);
         Stopwatch watch = Stopwatch.StartNew();
         bm.FindStereoCorrespondence(left, right, leftDisparity);
         watch.Stop();

         Trace.WriteLine(String.Format("Time used: {0} milliseconds", watch.ElapsedMilliseconds));

         Matrix<double> q = new Matrix<double>(4, 4);
         q.SetIdentity();
         MCvPoint3D32f[] points = PointCollection.ReprojectImageTo3D(leftDisparity * (-16), q);

         float min = (float)1.0e10, max = 0;
         foreach (MCvPoint3D32f p in points)
         {
            if (p.z < min) min = p.z;
            else if (p.z > max) max = p.z;
         }
         Trace.WriteLine(String.Format("Min : {0}\r\nMax : {1}", min, max));

      }

      [Test]
      public void TestStereoSGBMCorrespondence()
      {
         Image<Gray, Byte> left = new Image<Gray, byte>("left.jpg");
         Image<Gray, Byte> right = new Image<Gray, byte>("right.jpg");
         Size size = left.Size;

         Image<Gray, Int16> disparity = new Image<Gray, Int16>(size);

         StereoSGBM bm = new StereoSGBM(10, 64, 0, 0, 0, 0, 0, 0, 0, 0, false);
         Stopwatch watch = Stopwatch.StartNew();
         bm.FindStereoCorrespondence(left, right, disparity);
         watch.Stop();

         Trace.WriteLine(String.Format("Time used: {0} milliseconds", watch.ElapsedMilliseconds));

         Matrix<double> q = new Matrix<double>(4, 4);
         q.SetIdentity();
         MCvPoint3D32f[] points = PointCollection.ReprojectImageTo3D(disparity * (-16), q);

         float min = (float)1.0e10, max = 0;
         foreach (MCvPoint3D32f p in points)
         {
            if (p.z < min) min = p.z;
            else if (p.z > max) max = p.z;
         }
         Trace.WriteLine(String.Format("Min : {0}\r\nMax : {1}", min, max));

      }

      [Test]
      public void TestExtrinsicCameraParametersRuntimeSerialize()
      {
         ExtrinsicCameraParameters param = new ExtrinsicCameraParameters();

         param.RotationVector.SetRandUniform(new MCvScalar(), new MCvScalar(1.0));
         param.TranslationVector.SetRandUniform(new MCvScalar(), new MCvScalar(100));

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
            ExtrinsicCameraParameters param2 = (ExtrinsicCameraParameters)formatter.Deserialize(ms2);

            Assert.IsTrue(param.Equals(param2));
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
            IntrinsicCameraParameters param2 = (IntrinsicCameraParameters)formatter.Deserialize(ms2);

            Assert.IsTrue(param.Equals(param2));
         }
      }

      [Test]
      public void TestEllipseFitting()
      {
         #region generate random points
         System.Random r = new Random();
         int sampleCount = 100;
         Ellipse modelEllipse = new Ellipse(new PointF(200, 200), new SizeF(150, 60), 30);
         PointF[] pts = PointCollection.GeneratePointCloud(modelEllipse, sampleCount);
         #endregion

         Stopwatch watch = Stopwatch.StartNew();
         Ellipse fittedEllipse = PointCollection.EllipseLeastSquareFitting(pts);
         watch.Stop();

         #region draw the points and the fitted ellipse
         Image<Bgr, byte> img = new Image<Bgr, byte>(400, 400, new Bgr(Color.White));
         foreach (PointF p in pts)
            img.Draw(new CircleF(p, 2), new Bgr(Color.Green), 1);
         img.Draw(fittedEllipse, new Bgr(Color.Red), 2);
         #endregion

         //ImageViewer.Show(img, String.Format("Time used: {0} milliseconds", watch.ElapsedMilliseconds));
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
         MCvBox2D box = PointCollection.MinAreaRect(pts);
         watch.Stop();

         #region draw the points and the box
         Image<Bgr, byte> img = new Image<Bgr, byte>(400, 400, new Bgr(Color.White));
         img.Draw(box, new Bgr(Color.Red), 1);
         foreach (PointF p in pts)
            img.Draw(new CircleF(p, 2), new Bgr(Color.Green), 1);
         #endregion

         //ImageViewer.Show(img, String.Format("Time used: {0} milliseconds", watch.ElapsedMilliseconds));
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
         CircleF circle = PointCollection.MinEnclosingCircle(pts);
         watch.Stop();

         #region draw the points and the circle
         Image<Bgr, byte> img = new Image<Bgr, byte>(400, 400, new Bgr(Color.White));
         img.Draw(circle, new Bgr(Color.Red), 1);
         foreach (PointF p in pts)
            img.Draw(new CircleF(p, 2), new Bgr(Color.Green), 1);
         #endregion

         //ImageViewer.Show(img, String.Format("Time used: {0} milliseconds", watch.ElapsedMilliseconds));
      }


      [Test]
      public void TestMemstorage()
      {
         for (int i = 0; i < 100000; i++)
         {
            using (MemStorage stor = new MemStorage())
            {
               Seq<PointF> seq = new Seq<PointF>(stor);
            }
         }
      }

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
               SURFFeature sf2 = (SURFFeature)o;
            }
         }
      }


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
                  Assert.IsTrue(mat.Equals(mat2));
               }
            }
         }
      }

      [Test]
      public void TestPyrSegmentation()
      {
         Image<Bgr, Byte> image = new Image<Bgr, byte>("lena.jpg");
         Image<Bgr, Byte> segImage = new Image<Bgr, byte>(image.Size);
         MemStorage storage = new MemStorage();
         IntPtr comp;
         CvInvoke.cvPyrSegmentation(image, segImage, storage, out comp, 4, 255, 30);
      }

      [Test]
      public void TestHistogram()
      {
         using (Image<Bgr, Byte> img = new Image<Bgr, byte>("stuff.jpg"))
         using (Image<Hsv, Byte> img2 = img.Convert<Hsv, Byte>())
         {
            Image<Gray, Byte>[] HSVs = img2.Split();

            using (DenseHistogram h = new DenseHistogram(20, new RangeF(0, 180)))
            {
               h.Calculate(new Image<Gray, Byte>[1] { HSVs[0] }, true, null);
               using (Image<Gray, Byte> bpj = h.BackProject(new Image<Gray, Byte>[1] { HSVs[0] }))
               {
                  Size sz = bpj.Size;
               }
               using (Image<Gray, Single> patchBpj = h.BackProjectPatch(
                  new Image<Gray, Byte>[1] { HSVs[0] }, 
                  new Size(5, 5), 
                  Emgu.CV.CvEnum.HISTOGRAM_COMP_METHOD.CV_COMP_CHISQR,
                  1.0))
               {
                  Size sz = patchBpj.Size;
               }
            }

            foreach (Image<Gray, Byte> i in HSVs) i.Dispose();
         }
      }

      [Test]
      public void TestHOG1()
      {
         using (HOGDescriptor hog = new HOGDescriptor())
         using (Image<Bgr, Byte> image = new Image<Bgr, byte>("pedestrian.png"))
         {
            float[] pedestrianDescriptor = HOGDescriptor.GetDefaultPeopleDetector();
            hog.SetSVMDetector(pedestrianDescriptor);

            Stopwatch watch = Stopwatch.StartNew();
            Rectangle[] rects = hog.DetectMultiScale(image);
            watch.Stop();

            Assert.AreEqual(1, rects.Length);

            foreach (Rectangle rect in rects)
               image.Draw(rect, new Bgr(Color.Red), 1);
            Trace.WriteLine(String.Format("HOG detection time: {0} ms", watch.ElapsedMilliseconds));

            //ImageViewer.Show(image, String.Format("Detection Time: {0}ms", watch.ElapsedMilliseconds));
         }
      }

      [Test]
      public void TestHOG2()
      {
         using (HOGDescriptor hog = new HOGDescriptor())
         using (Image<Bgr, Byte> image = new Image<Bgr, byte>("lena.jpg"))
         {
            float[] pedestrianDescriptor = HOGDescriptor.GetDefaultPeopleDetector();
            hog.SetSVMDetector(pedestrianDescriptor);

            Stopwatch watch = Stopwatch.StartNew();
            Rectangle[] rects = hog.DetectMultiScale(image);
            watch.Stop();

            Assert.AreEqual(0, rects.Length);
            foreach (Rectangle rect in rects)
               image.Draw(rect, new Bgr(Color.Red), 1);
            Trace.WriteLine(String.Format("HOG detection time: {0} ms", watch.ElapsedMilliseconds));

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
      public void TestGrabCut1()
      {
         Image<Bgr, Byte> img = new Image<Bgr, byte>("airplane.jpg");

         Rectangle rect = new Rectangle(new Point(24, 126), new Size(483, 294));
         Matrix<Single> bgdModel = new Matrix<float>(1, 13 * 5);
         Matrix<Single> fgdModel = new Matrix<float>(1, 13 * 5);
         Image<Gray, byte> mask = new Image<Gray, byte>(img.Size);

         CvInvoke.CvGrabCut(img, mask, ref rect, bgdModel, fgdModel, 0, Emgu.CV.CvEnum.GRABCUT_INIT_TYPE.INIT_WITH_RECT);
         CvInvoke.CvGrabCut(img, mask, ref rect, bgdModel, fgdModel, 2, Emgu.CV.CvEnum.GRABCUT_INIT_TYPE.EVAL);

      }

      [Test]
      public void TestGrabCut2()
      {
         Image<Bgr, Byte> img = new Image<Bgr, byte>("pedestrian.png");
         HOGDescriptor desc = new HOGDescriptor();
         desc.SetSVMDetector(HOGDescriptor.GetDefaultPeopleDetector());

         Rectangle[] humanRegions = desc.DetectMultiScale(img);

         Image<Gray, byte> pedestrianMask = new Image<Gray, byte>(img.Size);
         foreach (Rectangle rect in humanRegions)
         {
            //generate the mask where 3 indicates forground and 2 indicates background 
            using (Image<Gray, byte> mask = img.GrabCut(rect, 2))
            {
               //get the mask of the forground
               CvInvoke.cvCmpS(mask, 3, mask, Emgu.CV.CvEnum.CMP_TYPE.CV_CMP_EQ);

               pedestrianMask._Or(mask);
            }
         }
      }

      [Test]
      public void TestQuaternions1()
      {
         Quaternions q = new Quaternions();
         double epsilon = 1.0e-10;

         Matrix<double> point = new Matrix<double>(3, 1);
         point.SetRandNormal(new MCvScalar(), new MCvScalar(20));
         using (Matrix<double> pt1 = new Matrix<double>(3, 1))
         using (Matrix<double> pt2 = new Matrix<double>(3, 1))
         using (Matrix<double> pt3 = new Matrix<double>(3, 1))
         {
            double x1 = 1.0, y1 = 0.2, z1 = 0.1;
            double x2 = 0.0, y2 = 0.0, z2 = 0.0;

            q.SetEuler(x1, y1, z1);
            q.GetEuler(ref x2, ref y2, ref z2);

            Assert.IsTrue(
               Math.Abs(x2 - x1) < epsilon &&
               Math.Abs(y2 - y1) < epsilon &&
               Math.Abs(z2 - z1) < epsilon);

            q.RotatePoints(point, pt1);

            Matrix<double> rMat = new Matrix<double>(3, 3);
            q.GetRotationMatrix(rMat);
            CvInvoke.cvGEMM(rMat, point, 1.0, IntPtr.Zero, 0.0, pt2, Emgu.CV.CvEnum.GEMM_TYPE.CV_GEMM_DEFAULT);

            CvInvoke.cvAbsDiff(pt1, pt2, pt3);

            Assert.IsTrue(
               pt3[0, 0] < epsilon &&
               pt3[1, 0] < epsilon &&
               pt3[2, 0] < epsilon);

         }

         double rotationAngle = 0.2;
         q.SetEuler(rotationAngle, 0.0, 0.0);
         Assert.IsTrue(Math.Abs(q.RotationAngle - rotationAngle) < epsilon);
         q.SetEuler(0.0, rotationAngle, 0.0);
         Assert.IsTrue(Math.Abs(q.RotationAngle - rotationAngle) < epsilon);
         q.SetEuler(0.0, 0.0, rotationAngle);
         Assert.IsTrue(Math.Abs(q.RotationAngle - rotationAngle) < epsilon);

         q = q * q;
         Assert.IsTrue(Math.Abs(q.RotationAngle / 2.0 - rotationAngle) < epsilon);

         q.SetEuler(0.2, 0.1, 0.05);
         double t = q.RotationAngle;
         q = q*q;
         Assert.IsTrue(Math.Abs(q.RotationAngle / 2.0 - t) < epsilon);

      }

      public void TestQuaternionsPerformance()
      {
         Quaternions q = new Quaternions();
         Random r = new Random();
         q.SetEuler(r.NextDouble(), r.NextDouble(), r.NextDouble());

         Stopwatch watch = Stopwatch.StartNew();
         double counter = 0.0;
         for (int i = 0; i < 1000000; i++)
         {
            Quaternions q2 = q * q;
            counter += q2.W;
         }
         watch.Stop();
         Trace.WriteLine(String.Format("Time used: {0} milliseconds", watch.ElapsedMilliseconds));

      }

      [Test]
      public void TestQuaternion2()
      {
         Random r = new Random();
         Quaternions q1 = new Quaternions();
         q1.SetEuler(r.NextDouble(), r.NextDouble(), r.NextDouble());

         Quaternions q2 = new Quaternions();
         q2.SetEuler(r.NextDouble(), r.NextDouble(), r.NextDouble());

         MCvPoint3D64f p = new MCvPoint3D64f(r.NextDouble() * 10, r.NextDouble() * 10, r.NextDouble() * 10);

         MCvPoint3D64f delta = (q1 * q2).RotatePoint(p) - q1.RotatePoint(q2.RotatePoint(p));
         double epsilon = 1.0e-8;
         Assert.Less(delta.x, epsilon);
         Assert.Less(delta.y, epsilon);
         Assert.Less(delta.z, epsilon);

      }

      [Test]
      public void TestQuaternion3()
      {
         Random r = new Random();
         Quaternions q1 = new Quaternions();
         q1.AxisAngle = new MCvPoint3D64f(r.NextDouble(), r.NextDouble(), r.NextDouble());

         Quaternions q2 = new Quaternions();
         q2.AxisAngle = q1.AxisAngle;

         double epsilon = 1.0e-12;
         Assert.Less(Math.Abs(q1.W - q2.W), epsilon);
         Assert.Less(Math.Abs(q1.X - q2.X), epsilon);
         Assert.Less(Math.Abs(q1.Y - q2.Y), epsilon);
         Assert.Less(Math.Abs(q1.Z - q2.Z), epsilon);

         RotationVector3D rVec = new RotationVector3D(new double[] { q1.AxisAngle.x, q1.AxisAngle.y, q1.AxisAngle.z });
         Matrix<double> m1 = rVec.RotationMatrix;
         Matrix<double> m2 = new Matrix<double>(3, 3);
         q1.GetRotationMatrix(m2);
         Matrix<double> diff = new Matrix<double>(3, 3);
         CvInvoke.cvAbsDiff(m1, m2, diff);
         double norm = CvInvoke.cvNorm(diff, IntPtr.Zero, Emgu.CV.CvEnum.NORM_TYPE.CV_C, IntPtr.Zero);
         Assert.Less(norm, epsilon);
      }


      [Test]
      public void TestDiatanceTransform()
      {
         Image<Gray, Byte> img = new Image<Gray, byte>(480, 320);
         img.Draw(new Rectangle(200, 100, 160, 90), new Gray(255), 1);
         img._Not();
         Image<Gray, Single> dst = new Image<Gray, Single>(img.Size);

         CvInvoke.cvDistTransform(img, dst, Emgu.CV.CvEnum.DIST_TYPE.CV_DIST_L2, 3, null, IntPtr.Zero);
      }


      [Test]
      public void TestAdaptiveSkinDetector()
      {
         Image<Bgr, Byte> image = new Image<Bgr, byte>("lena.jpg");
         using (AdaptiveSkinDetector detector = new AdaptiveSkinDetector(1, AdaptiveSkinDetector.MorphingMethod.ERODE_DILATE))
         {
            Image<Gray, Byte> mask = new Image<Gray, byte>(image.Size);
            detector.Process(image, mask);
            //mask._EqualizeHist();
            //ImageViewer.Show(mask);
         }
      }

      [Test]
      public void TestBinaryStorage()
      {
         Point[] pts = new Point[10000000];
         String fileName = GetTempFileName();
         Stopwatch watch = Stopwatch.StartNew();
         BinaryFileStorage<Point> stor = new BinaryFileStorage<Point>(fileName, pts);
         watch.Stop();
         Trace.WriteLine(String.Format("Time for writing {0} points: {1} milliseconds", pts.Length, watch.ElapsedMilliseconds));

         watch.Reset(); watch.Start();
         int counter = 0;
         foreach (Point p in stor)
         {
            counter++;
         }
         watch.Stop();
         Trace.WriteLine(String.Format("Time for reading {0} points: {1} milliseconds", pts.Length, watch.ElapsedMilliseconds));
         File.Delete(fileName);
      }

      [Test]
      public void TestSeqPerformance()
      {
         Point[] pts = new Point[1000000];
         
         using (MemStorage stor = new MemStorage())
         {
            Stopwatch watch = Stopwatch.StartNew();
            Seq<Point> seq = new Seq<Point>(stor);
            seq.PushMulti(pts, Emgu.CV.CvEnum.BACK_OR_FRONT.FRONT);
            watch.Stop();
            Trace.WriteLine(String.Format("Time for storing {0} points: {1} milliseconds", pts.Length, watch.ElapsedMilliseconds));

            watch.Reset(); watch.Start();
            int counter = 0;
            foreach (Point p in seq)
            {
               counter++;
            }
            watch.Stop();
            Trace.WriteLine(String.Format("Time for reading {0} points: {1} milliseconds", pts.Length, watch.ElapsedMilliseconds));
            
         }
      }

      [Test]
      public void TestCondensation()
      {
         IntPtr conden = CvInvoke.cvCreateConDensation(5, 5, 100);
         CvInvoke.cvReleaseConDensation(ref conden);
      }

      private static String GetTempFileName()
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
         String fileName = GetTempFileName() + ".mpeg";

         Image<Bgr, Byte>[] images = new Image<Bgr, byte>[numberOfFrames];
         for (int i = 0; i < images.Length; i++)
         {
            images[i] = new Image<Bgr, byte>(width, height);
            images[i].SetRandUniform(new MCvScalar(), new MCvScalar(255, 255, 255));
         }

         using (VideoWriter writer = new VideoWriter(fileName, 5, width, height, true))
         {
            for (int i = 0; i < numberOfFrames; i++)
            {
               writer.WriteFrame(images[i]);
            }
         }

         FileInfo fi = new FileInfo(fileName);
         Assert.AreNotEqual(fi.Length, 0);

         using (Capture capture = new Capture(fileName))
         {
            Image<Bgr, Byte> img2 = capture.QueryFrame();
            int count = 0;
            while (img2 != null)
            {
               Assert.AreEqual(img2.Width, width);
               Assert.AreEqual(img2.Height, height);
               //Assert.IsTrue(img2.Equals( images[count]) );
               img2 = capture.QueryFrame();
               count++;
            }
            Assert.AreEqual(numberOfFrames, count);
         }
         File.Delete(fi.FullName);
      }

      [Test]
      public void TestRTreeClassifier()
      {
         using(Image<Bgr, Byte> image = new Image<Bgr, byte>("box_in_scene.png"))
         using(Image<Gray, Byte> gray = image.Convert<Gray, byte>())
         using (RTreeClassifier<Bgr> classifier = new RTreeClassifier<Bgr>())
         {
            MCvSURFParams surf = new MCvSURFParams(300, false);
            MKeyPoint[] keypoints = surf.DetectKeyPoints(gray, null);
            Point[] points = Array.ConvertAll<MKeyPoint, Point>(keypoints, delegate(MKeyPoint kp) { return Point.Round(kp.Point); });
            Stopwatch watch = Stopwatch.StartNew();
            classifier.Train(image, points, 48, 9, 50, 176, 4);
            watch.Stop();
            Trace.WriteLine(String.Format("Training time: {0} milliseconds", watch.ElapsedMilliseconds));
            float[] signiture = classifier.GetSigniture(image, points[0], 15);
            Assert.AreEqual(signiture.Length, classifier.NumberOfClasses);
         }
      }
   }
}
