//----------------------------------------------------------------------------
//  Copyright (C) 2004-2013 by EMGU. All rights reserved.       
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
using Emgu.CV.Features2D;
using Emgu.CV.Cuda;
using Emgu.CV.Stitching;
using Emgu.CV.Structure;
using Emgu.CV.Tiff;
using Emgu.CV.Util;
using Emgu.CV.VideoSurveillance;
using Emgu.CV.Nonfree;
using Emgu.CV.Softcascade;
using Emgu.Util;
using NUnit.Framework;

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
         int inside = CvInvoke.cvClipLine(new Size(20, 20), ref m1, ref m2);
         EmguAssert.AreEqual(0, m1.X);
         EmguAssert.AreEqual(19, m2.X);
      }

      [Test]
      public void TestRectangleSize()
      {
         EmguAssert.AreEqual(4 * sizeof(int), Marshal.SizeOf(typeof(Rectangle)));
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
               EmguAssert.IsTrue(hist.Equals(hist2));
            }
         }
      }

      [Test]
      public void TestLookup()
      {
         double[] b = new double[4] { 0, 1, 2, 3 };
         double[] a = new double[4] { 1, 3, 2, 0 };
         MCvPoint2D64f[] pts = new MCvPoint2D64f[b.Length];
         for (int i = 0; i < pts.Length; i++)
            pts [i] = new MCvPoint2D64f(b [i], a [i]);

         IEnumerable<MCvPoint2D64f> interPts = Toolbox.LinearInterpolate(pts, new double[2]
         {
            1.5,
            3.5
         });
         IEnumerator<MCvPoint2D64f> enumerator = interPts.GetEnumerator();
         enumerator.MoveNext();
         EmguAssert.IsTrue(1.5 == enumerator.Current.x);
         EmguAssert.IsTrue(2.5 == enumerator.Current.y);
         enumerator.MoveNext();
         EmguAssert.IsTrue(3.5 == enumerator.Current.x);
         EmguAssert.IsTrue(-1 == enumerator.Current.y);
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
         EmguAssert.AreEqual(45.0, Math.Atan2(direction.Y, direction.X) * 180.0 / Math.PI);
      }

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
            EmguAssert.AreEqual(a1 [i], a2 [i]);
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
         EmguAssert.IsTrue(diff.Norm < 1.0e-8);
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
               EmguAssert.IsTrue(cs.MCvContour.elem_size == Marshal.SizeOf(typeof(Point)));
               //EmguAssert.IsTrue(rect.Width * rect.Height == cs.Area);

               EmguAssert.IsTrue(cs.Convex);
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
               Contour<Point> c = img2.FindContours(Emgu.CV.CvEnum.CHAIN_APPROX_METHOD.CV_CHAIN_APPROX_SIMPLE, Emgu.CV.CvEnum.RETR_TYPE.CV_RETR_LIST, stor);
               EmguAssert.IsTrue(c == null);
            }
         }

         int s1 = Marshal.SizeOf(typeof(MCvSeq));
         int s2 = Marshal.SizeOf(typeof(MCvContour));
         int sizeRect = Marshal.SizeOf(typeof(Rectangle));
         EmguAssert.IsTrue(s1 + sizeRect + 4 * Marshal.SizeOf(typeof(int)) == s2);
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
            EmguAssert.IsTrue(1 == defacts.Length);
            EmguAssert.IsTrue(new Point(100, 100).Equals(defacts [0].DepthPoint));

            EmguAssert.IsTrue(contour.InContour(new PointF(90, 90)) > 0);
            EmguAssert.IsTrue(contour.InContour(new PointF(300, 300)) < 0);
            EmguAssert.IsTrue(contour.InContour(new PointF(10, 10)) == 0);
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
            } catch (CvException excpt)
            {
               EmguAssert.AreEqual(-215, excpt.Status);
               exceptionCaught = true;
            }
            EmguAssert.IsTrue(exceptionCaught);
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
      public void TestBlobDetector()
      {
         int width = 300;
         int height = 400;
         Image<Gray, Byte> img = new Image<Gray, byte>(width, height);
         //bg.SetRandNormal(new MCvScalar(), new MCvScalar(100, 100, 100));
         Size size = new Size(width / 5, height / 5);
         Point topLeft = new Point((width >> 1) - (size.Width >> 1), (height >> 1) - (size.Height >> 1));
         Rectangle rect = new Rectangle(topLeft, size);

         BlobDetector detector = new BlobDetector(CvEnum.BLOB_DETECTOR_TYPE.Simple);
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
            return EmguAssert.LoadImage<Gray, Byte>(file).Resize(width, height, CvEnum.INTER.CV_INTER_LINEAR);
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

      /*
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
            CvInvoke.cvProjectPoints2(new Matrix<float>(points3D), rotation, translation,
            new Matrix<double>(new double[,] { 
               {focalLength, 0, 0},
               {0, focalLength, 0}, 
               {0,0,1}}),
            IntPtr.Zero, imagePointMat, IntPtr.Zero, IntPtr.Zero, IntPtr.Zero, IntPtr.Zero, IntPtr.Zero, 0.0);
            return imagePointMat.Data;
         }
      }
      
      [Test]
      public void TestIntrisicParameters()
      {
         System.Diagnostics.PerformanceCounter memCounter = new PerformanceCounter("Memory", "Available MBytes");
         Trace.WriteLine(String.Format("Available mem before: {0} Mb", memCounter.NextValue()));

         IntrinsicCameraParameters[] paramArr = new IntrinsicCameraParameters[100000];
         for (int i = 0; i < paramArr.Length; i++)
         {
            paramArr[i] = new IntrinsicCameraParameters(8);
         }

         Trace.WriteLine(String.Format("Available mem after: {0} Mb", memCounter.NextValue()));
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
         XDocument xDoc = Toolbox.XmlSerialize<PointF>(p, new Type[] { typeof(Point) });
         PointF p2 = Toolbox.XmlDeserialize<PointF>(xDoc, new Type[] { typeof(Point) });
         EmguAssert.IsTrue(p.Equals(p2));


         Rectangle rect = new Rectangle(3, 4, 5, 3);
         XDocument xDoc2 = Toolbox.XmlSerialize<Rectangle>(rect);
         Rectangle rect2 = Toolbox.XmlDeserialize<Rectangle>(xDoc2);
         EmguAssert.IsTrue(rect.Equals(rect2));

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
         MCvBox2D box = new MCvBox2D(
            new PointF(3.0f, 2.0f),
            new SizeF(4.0f, 6.0f),
            0.0f);
         PointF[] vertices = box.GetVertices();
         //TODO: Find out why the following test fails. (x, y) convention changed.
         //Assert.IsTrue(vertices[0].Equals(new PointF(0.0f, 0.0f)));
         //Assert.IsTrue(vertices[1].Equals(new PointF(6.0f, 0.0f)));
      }

#if !(IOS || ANDROID)
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

            Image<Gray, Byte> img3 = new Image<Gray, byte>(bmp);
            stopwatch.Stop();
            Trace.WriteLine(string.Format("Time: {0} milliseconds", stopwatch.ElapsedMilliseconds));
            Image<Gray, Byte> diff = img.Convert<Gray, Byte>().AbsDiff(img3);
            EmguAssert.AreEqual(0, CvInvoke.CountNonZero(diff));
            EmguAssert.IsTrue(img.Convert<Gray, Byte>().Equals(img3));
         }
      }
#endif

      [Test]
      public void TestMorphEx()
      {
         Mat kernel1 = CvInvoke.GetStructuringElement(Emgu.CV.CvEnum.CV_ELEMENT_SHAPE.CV_SHAPE_CROSS, new Size(3, 3), new Point(1, 1));
         Matrix<byte> kernel2 = new Matrix<byte>(new Byte[3, 3] { { 0, 1, 0 }, { 1, 0, 1 }, { 0, 1, 0 } });
         //StructuringElementEx element2 = new StructuringElementEx(new int[3, 3] { { 0, 1, 0 }, { 1, 0, 1 }, { 0, 1, 0 } }, 1, 1);
         Image<Bgr, Byte> tmp = new Image<Bgr, byte>(100, 100);
         Image<Bgr, Byte> tmp2 = tmp.MorphologyEx(Emgu.CV.CvEnum.CV_MORPH_OP.CV_MOP_GRADIENT, kernel1, new Point(-1, -1), 1, CvEnum.BORDER_TYPE.DEFAULT, new MCvScalar());
         Image<Bgr, Byte> tmp3 = tmp.MorphologyEx(Emgu.CV.CvEnum.CV_MORPH_OP.CV_MOP_GRADIENT, kernel2, new Point(-1, -1), 1, CvEnum.BORDER_TYPE.DEFAULT, new MCvScalar());
         //Image<Bgr, Byte> tmp2 = tmp.MorphologyEx(element1, Emgu.CV.CvEnum.CV_MORPH_OP.CV_MOP_GRADIENT, 1);
         //Image<Bgr, Byte> tmp3 = tmp.MorphologyEx(element2, Emgu.CV.CvEnum.CV_MORPH_OP.CV_MOP_BLACKHAT, 1);
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

         PlanarSubdivision division;

         Stopwatch watch = Stopwatch.StartNew();
         division = new PlanarSubdivision(points, true);
         Triangle2DF[] triangles = division.GetDelaunayTriangles(false);
         watch.Stop();
         EmguAssert.WriteLine(String.Format("delaunay triangulation: {2} points, {0} milli-seconds, {1} triangles", watch.ElapsedMilliseconds, triangles.Length, points));
         watch.Reset();

         EmguAssert.IsTrue(CvInvoke.icvSubdiv2DCheck(division));

         watch.Start();
         division = new PlanarSubdivision(points);
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
            Emgu.CV.PlanarSubdivision psd = new Emgu.CV.PlanarSubdivision(points);

            // hangs here:
            Emgu.CV.Structure.Triangle2DF[] triangles = psd.GetDelaunayTriangles();

            System.Console.WriteLine("done.");
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

         Image<Gray, Single> match = img.MatchTemplate(randomObj, Emgu.CV.CvEnum.TM_TYPE.CV_TM_SQDIFF);
         double[] minVal, maxVal;
         Point[] minLoc, maxLoc;
         match.MinMax(out minVal, out maxVal, out minLoc, out maxLoc);

         EmguAssert.AreEqual(minLoc[0].X, templCenter.X - templWidth / 2);
         EmguAssert.AreEqual(minLoc[0].Y, templCenter.Y - templHeight / 2);
      }

      /// <summary>
      /// Prepare synthetic image for testing
      /// </summary>
      /// <param name="prevImg"></param>
      /// <param name="currImg"></param>
      public static void OpticalFlowImage(out Image<Gray, Byte> prevImg, out Image<Gray, Byte> currImg)
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
         OpticalFlowImage(out prevImg, out currImg);
         Image<Gray, Single> flowx = new Image<Gray, float>(prevImg.Size);
         Image<Gray, Single> flowy = new Image<Gray, float>(prevImg.Size);
         OpticalFlow.Farneback(prevImg, currImg, flowx, flowy, 0.5, 3, 5, 20, 7, 1.5, Emgu.CV.CvEnum.OPTICALFLOW_FARNEBACK_FLAG.DEFAULT);
      }

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

         OpticalFlow.BM(prevImg, currImg, blockSize, shiftSize, maxRange, false, velx, vely);

         watch.Stop();

         EmguAssert.WriteLine(String.Format(
            "Time: {0} milliseconds",
            watch.ElapsedMilliseconds));

      }

      [Test]
      public void TestOpticalFlowLK()
      {
         Image<Gray, Byte> prevImg, currImg;
         OpticalFlowImage(out prevImg, out currImg);

         PointF[] prevFeature = new PointF[] { new PointF(100f, 100f) };

         PointF[] currFeature;
         Byte[] status;
         float[] trackError;

         Stopwatch watch = Stopwatch.StartNew();

         OpticalFlow.PyrLK(
            prevImg, currImg, prevFeature, new Size(10, 10), 3, new MCvTermCriteria(10, 0.01),
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
         Image<Gray, Byte> prevImg, currImg;
         OpticalFlowImage(out prevImg, out currImg);
         Image<Gray, float> velx = new Image<Gray, float>(prevImg.Size);
         Image<Gray, float> vely = new Image<Gray, float>(prevImg.Size);

         Stopwatch watch = Stopwatch.StartNew();

         OpticalFlow.DualTVL1(prevImg, currImg, velx, vely);

         watch.Stop();
         EmguAssert.WriteLine(String.Format(
            "Time: {0} milliseconds",
            watch.ElapsedMilliseconds));
      }

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
      }

      [Test]
      public void TestFlannLinear()
      {
         float[][] features = new float[10][];
         for (int i = 0; i < features.Length; i++)
            features[i] = new float[] { (float) i };

         Flann.Index index = new Flann.Index(CvToolbox.GetMatrixFromArrays(features));

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
            features[i] = new float[] { (float) i };

         Flann.Index index = new Flann.Index(CvToolbox.GetMatrixFromArrays(features), 4);

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
            features[i] = new float[] { (float) i };

         Flann.Index index = new Flann.Index(CvToolbox.GetMatrixFromArrays(features), 4, 32, 11, Emgu.CV.Flann.CenterInitType.RANDOM, 0.2f);

         float[][] features2 = new float[1][];
         features2[0] = new float[] { 5.0f };

         Matrix<int> indices = new Matrix<int>(features2.Length, 1);
         Matrix<float> distances = new Matrix<float>(features2.Length, 1);
         index.KnnSearch(CvToolbox.GetMatrixFromArrays(features2), indices, distances, 1, 32);

         EmguAssert.IsTrue(indices[0, 0] == 5);
         EmguAssert.IsTrue(distances[0, 0] == 0.0);
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

      [Test]
      public void TestFaceRecognizer()
      {
         Image<Gray, Byte>[] images = new Image<Gray, byte>[20];
         int[] labels = new int[20];
         for (int i = 0; i < images.Length; i++)
         {
            images[i] = new Image<Gray, byte>(200, 200);
            images[i].SetRandUniform(new MCvScalar(0), new MCvScalar(255));
            labels[i] = i;
         }

         EigenFaceRecognizer eigen = new EigenFaceRecognizer(0, double.MaxValue);
         eigen.Train(images, labels);
         for (int i = 0; i < images.Length; i++)
         {
            EmguAssert.IsTrue(eigen.Predict(images[i]).Label == i);
         }
         String filePath = Path.Combine(Path.GetTempPath(), "abc.xml");
         eigen.Save(filePath);
         eigen.Load(filePath);

         FisherFaceRecognizer fisher = new FisherFaceRecognizer(0, double.MaxValue);
         fisher.Train(images, labels);
         for (int i = 0; i < images.Length; i++)
         {
            EmguAssert.IsTrue(fisher.Predict(images[i]).Label == i);
         }

         LBPHFaceRecognizer lbph = new LBPHFaceRecognizer(1, 8, 8, 8, double.MaxValue);
         lbph.Train(images, labels);
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
            pts[i] = new PointF((float) (100 + r.NextDouble() * 400), (float) (100 + r.NextDouble() * 400));
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
            if (p.z < min)
               min = p.z;
            else if (p.z > max)
               max = p.z;
         }
         EmguAssert.WriteLine(String.Format("Min : {0}\r\nMax : {1}", min, max));

         //ImageViewer.Show(leftDisparity*(-16));
      }

      //TODO: This test is failing, check when this will be fixed.
/*
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
         Image<Gray, Byte> left = EmguAssert.LoadImage<Gray, byte>("scene_l.bmp");
         Image<Gray, Byte> right = EmguAssert.LoadImage<Gray, byte>("scene_r.bmp");
         Size size = left.Size;

         Image<Gray, Int16> disparity = new Image<Gray, Int16>(size);

         StereoSGBM bm = new StereoSGBM(10, 64, 0, 0, 0, 0, 0, 0, 0, 0, StereoSGBM.Mode.SGBM);
         Stopwatch watch = Stopwatch.StartNew();
         bm.FindStereoCorrespondence(left, right, disparity);
         watch.Stop();

         EmguAssert.WriteLine(String.Format("Time used: {0} milliseconds", watch.ElapsedMilliseconds));

         Matrix<double> q = new Matrix<double>(4, 4);
         q.SetIdentity();
         MCvPoint3D32f[] points = PointCollection.ReprojectImageTo3D(disparity * (-16), q);

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
      }*/

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
                  MatND<float> mat2 = (MatND<float>) o;
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
                  VectorOfKeyPoint kpts2 = (VectorOfKeyPoint) o;
               }
            }
         }
      }

      [Test]
      public void TestVectorOfMat()
      {
         Matrix<double> m1 = new Matrix<double>(3, 3);
         m1.SetRandNormal(new MCvScalar(0.0), new MCvScalar(1.0));
         Matrix<int> m2 = new Matrix<int>(4, 4);
         m2.SetRandNormal(new MCvScalar(2), new MCvScalar(2));

         VectorOfMat vec = new VectorOfMat();
         vec.Push(m1);
         vec.Push(m2);

         Mat tmp1 = vec[0];
         Mat tmp2 = vec[1];
         Matrix<double> n1 = new Matrix<double>(tmp1.Size);
         Matrix<int> n2 = new Matrix<int>(tmp2.Size);
         tmp1.CopyTo(n1, null);
         tmp2.CopyTo(n2, null);

         EmguAssert.IsTrue(m1.Equals(n1));
         EmguAssert.IsTrue(m2.Equals(n2));
      }

      [Test]
      public void TestPyrSegmentation()
      {
         Image<Bgr, Byte> image = EmguAssert.LoadImage<Bgr, Byte>("lena.jpg");
         Image<Bgr, Byte> segImage = new Image<Bgr, byte>(image.Size);
         MemStorage storage = new MemStorage();
         IntPtr comp;
         CvInvoke.cvPyrSegmentation(image, segImage, storage, out comp, 4, 255, 30);
      }

      [Test]
      public void TestHistogram()
      {
         using (Image<Bgr, Byte> img = EmguAssert.LoadImage<Bgr, Byte>("stuff.jpg"))
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

            foreach (Image<Gray, Byte> i in HSVs)
               i.Dispose();
         }
      }

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
      }

      [Test]
      public void TestHOG1()
      {
         using (HOGDescriptor hog = new HOGDescriptor())
         using (Image<Bgr, Byte> image = EmguAssert.LoadImage<Bgr, Byte>("pedestrian.png"))
         {
            float[] pedestrianDescriptor = HOGDescriptor.GetDefaultPeopleDetector();
            hog.SetSVMDetector(pedestrianDescriptor);

            Stopwatch watch = Stopwatch.StartNew();
            Rectangle[] rects = hog.DetectMultiScale(image);
            watch.Stop();

            EmguAssert.AreEqual(1, rects.Length);

            foreach (Rectangle rect in rects)
               image.Draw(rect, new Bgr(Color.Red), 1);
            EmguAssert.WriteLine(String.Format("HOG detection time: {0} ms", watch.ElapsedMilliseconds));

            //ImageViewer.Show(image, String.Format("Detection Time: {0}ms", watch.ElapsedMilliseconds));
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
            Rectangle[] rects = hog.DetectMultiScale(image);
            watch.Stop();

            EmguAssert.AreEqual(0, rects.Length);
            foreach (Rectangle rect in rects)
               image.Draw(rect, new Bgr(Color.Red), 1);
            EmguAssert.WriteLine(String.Format("HOG detection time: {0} ms", watch.ElapsedMilliseconds));

            //ImageViewer.Show(image, String.Format("Detection Time: {0}ms", watch.ElapsedMilliseconds));
         }
      }

      [Test]
      public void TestHOGTrain64x128()
      {
         using (Image<Bgr, byte> image = EmguAssert.LoadImage<Bgr, Byte>("lena.jpg"))
         using (Image<Bgr, Byte> resize = image.Resize(64, 128, Emgu.CV.CvEnum.INTER.CV_INTER_CUBIC))
         using (HOGDescriptor hog = new HOGDescriptor(resize))
         {
            Stopwatch watch = Stopwatch.StartNew();
            Rectangle[] rects = hog.DetectMultiScale(image);
            watch.Stop();
            foreach (Rectangle rect in rects)
               image.Draw(rect, new Bgr(Color.Red), 1);

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
            Rectangle[] rects = hog.DetectMultiScale(image);
            watch.Stop();
            foreach (Rectangle rect in rects)
               image.Draw(rect, new Bgr(Color.Red), 1);

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
         Image<Bgr, Byte> img = EmguAssert.LoadImage<Bgr, Byte>("airplane.jpg");

         Rectangle rect = new Rectangle(new Point(24, 126), new Size(483, 294));
         Matrix<double> bgdModel = new Matrix<double>(1, 13 * 5);
         Matrix<double> fgdModel = new Matrix<double>(1, 13 * 5);
         Image<Gray, byte> mask = new Image<Gray, byte>(img.Size);

         CvInvoke.GrabCut(img, mask, rect, bgdModel, fgdModel, 0, Emgu.CV.CvEnum.GRABCUT_INIT_TYPE.INIT_WITH_RECT);
         CvInvoke.GrabCut(img, mask, rect, bgdModel, fgdModel, 2, Emgu.CV.CvEnum.GRABCUT_INIT_TYPE.EVAL);
         using (InputArray ia = new InputArray(3))
            CvInvoke.Compare(mask, ia, mask, CvEnum.CMP_TYPE.CV_CMP_EQ);
         //Emgu.CV.UI.ImageViewer.Show(img.ConcateHorizontal( mask.Convert<Bgr, Byte>()));
      }

      [Test]
      public void TestGrabCut2()
      {
         Image<Bgr, Byte> img = EmguAssert.LoadImage<Bgr, Byte>("pedestrian.png");
         HOGDescriptor desc = new HOGDescriptor();
         desc.SetSVMDetector(HOGDescriptor.GetDefaultPeopleDetector());

         Rectangle[] humanRegions = desc.DetectMultiScale(img);

         Image<Gray, byte> pedestrianMask = new Image<Gray, byte>(img.Size);
         foreach (Rectangle rect in humanRegions)
         {
            //generate the mask where 3 indicates foreground and 2 indicates background 
            using (Image<Gray, byte> mask = img.GrabCut(rect, 2))
            {
               //get the mask of the foreground
               using (InputArray ia = new InputArray(3))
                  CvInvoke.Compare(mask, ia, mask, Emgu.CV.CvEnum.CMP_TYPE.CV_CMP_EQ);

               pedestrianMask._Or(mask);
            }
         }
      }

      [Test]
      public void TestGraySingleImage()
      {
         Image<Gray, Single> img = new Image<Gray, float>(320, 480);
         img.SetRandUniform(new MCvScalar(), new MCvScalar(255));
         Image<Gray, Byte> mask = img.Cmp(100, CvEnum.CMP_TYPE.CV_CMP_GE);
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

         CvInvoke.cvDistTransform(img, dst, Emgu.CV.CvEnum.DIST_TYPE.CV_DIST_L2, 3, null, IntPtr.Zero);
      }

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
      }

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

      [Test]
      public void TestSeqPerformance()
      {
         Point[] pts = new Point[100];

         using (MemStorage stor = new MemStorage())
         {
            Stopwatch watch = Stopwatch.StartNew();
            Seq<Point> seq = new Seq<Point>(stor);
            seq.PushMulti(pts, Emgu.CV.CvEnum.BACK_OR_FRONT.FRONT);
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
      }

      private static String GetTempFileName()
      {
         string filename = Path.GetTempFileName();

         File.Delete(filename);

         return Path.Combine(Path.GetDirectoryName(filename), Path.GetFileNameWithoutExtension(filename));
      }

      /*
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

         using (VideoWriter writer = new VideoWriter(fileName, CvInvoke.CV_FOURCC('M', 'J', 'P', 'G'), 5, width, height, true))
         //using (VideoWriter writer = new VideoWriter(fileName, -1, 5, width, height, true))
         {
            for (int i = 0; i < numberOfFrames; i++)
            {
               writer.WriteFrame(images[i]);
            }
         }

         FileInfo fi = new FileInfo(fileName);
         EmguAssert.IsTrue(fi.Length != 0);

         using (Capture capture = new Capture(fileName))
         {
            Image<Bgr, Byte> img2 = capture.QueryFrame();
            int count = 0;
            while (img2 != null)
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
      }*/

#if !ANDROID 
      //took too long to test on android, disabling for now
      [Test]
      public void TestRTreeClassifier()
      {
         using (Image<Bgr, Byte> image = EmguAssert.LoadImage<Bgr, Byte>("box_in_scene.png"))
         using (Image<Gray, Byte> gray = image.Convert<Gray, byte>())
         using (RTreeClassifier<Bgr> classifier = new RTreeClassifier<Bgr>())
         {
            SURFDetector surf = new SURFDetector(300, false);
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

      [Test]
      public void TestIndex3D()
      {
         Random r = new Random();
         MCvPoint3D32f[] points = new MCvPoint3D32f[1000];

         for (int i = 0; i < points.Length; i++)
         {
            points[i].x = (float) r.NextDouble();
            points[i].y = (float) r.NextDouble();
            points[i].z = (float) r.NextDouble();
         }

         MCvPoint3D32f searchPoint = new MCvPoint3D32f();
         searchPoint.x = (float) r.NextDouble();
         searchPoint.y = (float) r.NextDouble();
         searchPoint.z = (float) r.NextDouble();

         int indexOfClosest1 = 0;
         double shortestDistance1 = double.MaxValue;
         for (int i = 0; i < points.Length; i++)
         {
            double dist = (searchPoint - points[i]).Norm;
            if (dist < shortestDistance1)
            {
               shortestDistance1 = dist;
               indexOfClosest1 = i;
            }
         }

         Flann.Index3D index3D = new Emgu.CV.Flann.Index3D(points);
         double shortestDistance2;
         int indexOfClosest2 = index3D.ApproximateNearestNeighbour(searchPoint, out shortestDistance2);
         shortestDistance2 = Math.Sqrt(shortestDistance2);

         EmguAssert.IsTrue(indexOfClosest1 == indexOfClosest2);
         EmguAssert.IsTrue((shortestDistance1 - shortestDistance2) <= 1.0e-3 * shortestDistance1);
      }

      [Test]
      public void TestPOSIT()
      {
         float[,] points = new float[4, 3] { { 0, 0, 0 }, { 1, 1, 0 }, { 1, 0, 1 }, { 0, 1, 1 } };
         IntPtr ptr = CvInvoke.cvCreatePOSITObject(points, 4);
         CvInvoke.cvReleasePOSITObject(ref ptr);
      }

      /*
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
                 delegate(object sender, EventArgs<string> e)
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
         using (Capture capture = new Capture(EmguAssert.GetFile( "tree.avi")))
         using (VideoWriter writer = new VideoWriter("tree_invert.avi", 10, capture.Width, capture.Height, true))
         {
            while (capture.Grab())
            {
               using (Image<Bgr, Byte> img = capture.RetrieveBgrFrame(0))
               {
                  img._Not();
                  writer.WriteFrame(img);
               }
            }
         }
      }
#endif

      [Test]
      public void TestRotationMatrix()
      {
         Size dstImageSize;
         using (RotationMatrix2D<float> rotationMatrix = RotationMatrix2D<float>.CreateRotationMatrix(new System.Drawing.PointF(320, 240), -90, new Size(640, 480), out dstImageSize))
         {
            Trace.WriteLine("emgu.cv.test", String.Format("dstSize: {0}x{1}", dstImageSize.Width, dstImageSize.Height));
            Trace.WriteLine("emgu.cv.test", String.Format("rotationMat: [ [{0}, {1}, {2}], [{3}, {4}, {5}] ]", rotationMatrix.Data[0, 0], rotationMatrix.Data[0, 1], rotationMatrix.Data[0, 2], rotationMatrix.Data[1, 0], rotationMatrix.Data[1, 1], rotationMatrix.Data[1, 2]));
         }
      }

      [Test]
      public void TestImageDecodeBuffer()
      {
         using (FileStream fs = File.OpenRead(EmguAssert.GetFile( "lena.jpg" )))
         {
            byte[] data = new byte[fs.Length];
            fs.Read(data, 0, (int) fs.Length);

            IntPtr image = CvInvoke.cvDecodeImage(data, CvEnum.LOAD_IMAGE_TYPE.CV_LOAD_IMAGE_COLOR);

            try
            {
               Image<Bgr, Byte> img = Image<Bgr, byte>.FromIplImagePtr(image);

               //Emgu.CV.UI.ImageViewer.Show(img);
            }
            finally
            {
               CvInvoke.cvReleaseImage(ref image);
            }
         }
      }

      [Test]
      public void TestRotationMatrix2D()
      {
         double angle = 32;
         Size size = new Size(960, 480);
         PointF center = new PointF(size.Width * 0.5f, size.Height * 0.5f);
         using (RotationMatrix2D<double> rotationMatrix = new RotationMatrix2D<double>(center, -angle, 1))
         {
            PointF[] corners = new PointF[] {
                  new PointF(0, 0),
                  new PointF(size.Width - 1, 0),
                  new PointF(size.Width - 1, size.Height - 1),
                  new PointF(0, size.Height - 1)};
            PointF[] oldCorners = new PointF[corners.Length];
            corners.CopyTo(oldCorners, 0);

            rotationMatrix.RotatePoints(corners);

            RotationMatrix2D<double> transformation = CameraCalibration.EstimateRigidTransform(oldCorners, corners, true);

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
      public void TestStitching()
      {
         Image<Bgr, Byte>[] images = new Image<Bgr, byte>[4];

         images[0] = EmguAssert.LoadImage<Bgr,Byte>("stitch1.jpg");
         images[1] = EmguAssert.LoadImage<Bgr, Byte>("stitch2.jpg");
         images[2] = EmguAssert.LoadImage<Bgr, Byte>("stitch3.jpg");
         images[3] = EmguAssert.LoadImage<Bgr, Byte>("stitch4.jpg");

         using (Stitcher stitcher = new Stitcher(false))
         {
            Image<Bgr, Byte> result = stitcher.Stitch(images);
            //ImageViewer.Show(result);
         }
      }

      [Test]
      public void TestEstimateAffine3D()
      {
         Random r = new Random();
         Matrix<double> affine = new Matrix<double>(3, 4);
         for(int i = 0; i < 3; i++)
            for (int j = 0; j < 4; j++)
            {
               affine.Data[i, j] = r.NextDouble() * 2 + 1; 
            }

         MCvPoint3D32f[] srcPts = new MCvPoint3D32f[4];
         for (int i = 0; i < srcPts.Length; i++)
         {
            srcPts[i] = new MCvPoint3D32f((float) (r.NextDouble() + 1), (float) (r.NextDouble() + 1), (float) (r.NextDouble() + 5));
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
               double px = r * Math.Cos(theta) + y; /*+ rng.gaussian(1)*/
               double py = 5 - y; /*+ rng.gaussian(1)*/
               double pz = r * Math.Sin(theta) + y; /*+ rng.gaussian(1)*/

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
               if ((0 <= imagePoints[i][j].x) && (imagePoints[i][j].x <= cameraRes.Height) &&
                   (0 <= imagePoints[i][j].y) && (imagePoints[i][j].y <= cameraRes.Width))
               {  // add randomness	
                  // perturbate
                  visibility[i][j] = 1;
                  imagePoints[i][j] = new MCvPoint2D64f((float)points[i].x + ran.Next(0, 3), (float)points[i].y + ran.Next(0, 3));
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
      public void TestLatenSVM()
      {
         using (LatentSvmDetector detector = new LatentSvmDetector(EmguAssert.GetFile( "car.xml" )))
         {
            String[] files = new String[] { 
               "license-plate.jpg"};

            for (int idx = 0; idx < files.Length; idx++)
               using (Image<Bgr, Byte> img = EmguAssert.LoadImage<Bgr, Byte>(files[idx]))
               {
                  MCvObjectDetection[] results = detector.Detect(img, 0.5f);
                  if (results.Length >= 0)
                  {
                     double maxScore = results[0].score;
                     Rectangle result = results[0].Rect;

                     for (int i = 1; i < results.Length; ++i)
                     {
                        if (results[i].score > maxScore)
                        {
                           maxScore = results[i].score;
                           result = results[i].Rect;
                        }
                     }

                     result.Inflate((int)(result.Width * 0.2), (int)(result.Height * 0.2));
                     using (Image<Gray, Byte> mask = img.GrabCut(result, 10))
                     using (Image<Gray, Byte> canny = img.Canny(120, 80))
                     {
                        MCvFont f = new MCvFont(CvEnum.FONT.CV_FONT_HERSHEY_COMPLEX, 2.0, 2.0);
                        using (InputArray ia = new InputArray(3))
                           CvInvoke.Compare(mask, ia, mask, CvEnum.CMP_TYPE.CV_CMP_NE);
                        CvInvoke.cvSet(canny, new MCvScalar(), mask);
                        canny.Draw(@"http://www.emgu.com", ref f, new Point(50, 50), new Gray(255));

                        CvInvoke.BitwiseNot(canny, canny, null);

                        Image<Bgr, byte> displayImg = img.ConcateHorizontal(canny.Convert<Bgr, Byte>()/*mask.Convert<Bgr, Byte>()*/);

                        //displayImg.Save("out_" + files[idx]);
                        //ImageViewer.Show(displayImg);
                     }
                  }
               }
         }
      }

      /*
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
      }*/

      [Test]
      public void TestChamferMatching()
      {
         using (Image<Gray, Byte> logo = EmguAssert.LoadImage<Gray, Byte>("logo.png"))
         using (Image<Gray, Byte> logoInClutter = EmguAssert.LoadImage<Gray, Byte>("logo_in_clutter.png"))
         using (Image<Bgr, Byte> image = logoInClutter.Convert<Bgr, Byte>())
         {
            Point[][] contours;
            float[] costs;
            int count = CvInvoke.cvChamferMatching(logoInClutter, logo, out contours, out costs, 1, 1, 1, 3, 3, 5, 0.6, 1.6, 0.5, 20);

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

      //TODO: Check why this fails again
      [Test]
      public void TestFileCapturePause()
      {
         
         int totalFrames1 = 0;

         Capture capture1 = new Capture(EmguAssert.GetFile("tree.avi"));
        
         //capture one will continute capturing all the frames.
         EventHandler captureHandle1 = delegate
         {
            Image<Bgr, Byte> img = capture1.RetrieveBgrFrame(0);
            totalFrames1++;
            Trace.WriteLine(String.Format("capture 1 frame {0}: {1}", totalFrames1, DateTime.Now.ToString()));
         };
         capture1.ImageGrabbed += captureHandle1;
         capture1.Start();

         System.Threading.Thread.Sleep(2);
         int totalFrames2 = 0;
         Capture capture2 = new Capture(EmguAssert.GetFile("tree.avi"));
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

            Image<Bgr, Byte> img = capture2.RetrieveBgrFrame(0);
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
         while (! (totalFrames1 == totalFrames2))
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

      [Test]
      public void TestGLImageView()
      {
         Emgu.CV.UI.GLView.GLImageViewer viewer = new UI.GLView.GLImageViewer();
         //viewer.ShowDialog();
      }

      [Test]
      public void TestERFilter()
      {
         CvInvoke.SanityCheck();
         bool checkInvert = false;
         using (Image<Gray, Byte> image = EmguAssert.LoadImage<Gray, Byte>("scenetext01.jpg"))
         using (ERFilterNM1 er1 = new ERFilterNM1(EmguAssert.GetFile("trained_classifierNM1.xml"), 8, 0.00025f, 0.13f, 0.4f, true, 0.1f))
         using (ERFilterNM2 er2 = new ERFilterNM2(EmguAssert.GetFile("trained_classifierNM2.xml"), 0.3f))
         {
            //using (Image<Gray, Byte> mask = new Image<Gray,byte>(image.Size.Width + 2, image.Size.Height + 2))

            Image<Gray, byte>[] channels;
            if (checkInvert)
            {
               channels = new Image<Gray, byte>[image.NumberOfChannels * 2];
               Image<Gray, Byte>[] originalChannles = image.Split();
               for (int i = 0; i < originalChannles.Length; i++)
               {
                  channels[2 * i] = originalChannles[i];
                  channels[2 * i + 1] = originalChannles[i].Not();
               }
            } else
            {
               channels = image.Split();
            }

            VectorOfERStat[] regionVecs = new VectorOfERStat[channels.Length];
            for (int i = 0; i < regionVecs.Length; i++)
               regionVecs[i] = new VectorOfERStat();

            try
            {
               for (int i = 0; i < channels.Length; i++)
               {
                  er1.Run(channels[i], regionVecs[i]);
                  er2.Run(channels[i], regionVecs[i]);
               }
               Rectangle[] regions = ERFilter.ERGrouping(channels, regionVecs, EmguAssert.GetFile("trained_classifier_erGrouping.xml"), 0.5f);

               foreach (Rectangle rect in regions)
                  image.Draw(rect, new Gray(0), 2);
               //Emgu.CV.UI.ImageViewer.Show(image);

               /*
               MCvERStat[] regions = regionVec.ToArray();
               Size size = image.Size;
               foreach (MCvERStat region in regions)
               {
                  if (region.ParentPtr != IntPtr.Zero)
                  {
                     Point p = region.GetCenter(size.Width);
                     int flags = 4 + (255<< 8) + (int)CvEnum.FLOODFILL_FLAG.FIXED_RANGE + (int)CvEnum.FLOODFILL_FLAG.MASK_ONLY;
                     MCvConnectedComp comp;
                     CvInvoke.cvFloodFill(image, p, new MCvScalar(255), new MCvScalar(region.Level), new MCvScalar(), out comp, flags, mask);
                     image.Draw(new CircleF(new PointF(p.X, p.Y), 4), new Gray(0), 2);
                  }
               }*/
               //UI.ImageViewer.Show(image.ConcateHorizontal(mask));
               //}
            }
            finally
            {
               foreach (Image<Gray, byte> tmp in channels)
                  if (tmp != null)
                     tmp.Dispose();
               foreach (VectorOfERStat tmp in regionVecs)
                  if (tmp != null)
                     tmp.Dispose();
            }
         }

      }
   }
}
