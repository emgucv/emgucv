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
         System.Drawing.Point m1 = new System.Drawing.Point(-1, 10);
         System.Drawing.Point m2 = new System.Drawing.Point(100, 10);
         int inside = CvInvoke.cvClipLine(new System.Drawing.Size(20, 20), ref m1, ref m2);
         Assert.AreEqual(0, m1.X);
         Assert.AreEqual(19, m2.X);
      }

      [Test]
      public void TestLookup()
      {
         float[] b = new float[4] { 0, 1, 2, 3 };
         float[] a = new float[4] { 1, 3, 2, 0 };
         PointF[] pts = new PointF[b.Length];
         for (int i = 0; i < pts.Length; i++)
            pts[i] = new PointF(b[i], a[i]);

         Assert.AreEqual(2.5f, PointCollection.FirstDegreeInterpolate(pts, 1.5f));
         Assert.AreEqual(-1f, PointCollection.FirstDegreeInterpolate(pts, 3.5f));
      }

      [Test]
      public void TestLineFitting()
      {
         List<PointF> pts = new List<PointF>();

         pts.Add(new PointF(1.0f, 1.0f));
         pts.Add(new PointF(2.0f, 2.0f));
         pts.Add(new PointF(3.0f, 3.0f));
         pts.Add(new PointF(4.0f, 4.0f));

         PointF direction, pointOnLine;
         PointCollection.Line2DFitting(pts.ToArray(),  Emgu.CV.CvEnum.DIST_TYPE.CV_DIST_L2, out direction, out pointOnLine );
         
         //check if the line is 45 degree from +x axis
         Assert.AreEqual(45.0, Math.Atan2(direction.Y, direction.X) * 180.0 / Math.PI);
      }

      [Test]
      public void TestSerialization()
      {

         MCvPoint2D64f pt2d = new MCvPoint2D64f(12.0, 5.5);
         Point ptemp = new Point(10, 10);

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
            System.Drawing.Rectangle rect = new System.Drawing.Rectangle(10, 10, 80 - 10, 50 - 10);
            img.Draw(rect, new Gray(255.0), -1);
            //ImageViewer.Show(img);
            PointF pIn = new PointF(60, 40);
            PointF pOut = new PointF(80, 100);

            using (MemStorage stor = new MemStorage())
            {
               Contour<System.Drawing.Point> cs = img.FindContours(CvEnum.CHAIN_APPROX_METHOD.CV_CHAIN_APPROX_SIMPLE, CvEnum.RETR_TYPE.CV_RETR_LIST, stor);
               Assert.AreEqual(cs.MCvContour.elem_size, Marshal.SizeOf(typeof(System.Drawing.Point)));
               Assert.AreEqual(rect.Width * rect.Height, cs.Area);

               Assert.IsTrue(cs.Convex);
               Assert.AreEqual(rect.Width * 2 + rect.Height * 2, cs.Perimeter);
               System.Drawing.Rectangle rect2 = cs.BoundingRectangle;
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
               Contour<System.Drawing.Point> c = img2.FindContours(Emgu.CV.CvEnum.CHAIN_APPROX_METHOD.CV_CHAIN_APPROX_SIMPLE, Emgu.CV.CvEnum.RETR_TYPE.CV_RETR_LIST, stor);
               Assert.AreEqual(c, null);
            }
         }

         int s1 = Marshal.SizeOf(typeof(MCvSeq));
         int s2 = Marshal.SizeOf(typeof(MCvContour));
         int sizeRect = Marshal.SizeOf(typeof(System.Drawing.Rectangle));
         Assert.AreEqual(s1 + sizeRect + 4 * Marshal.SizeOf(typeof(int)), s2);
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
      public void TestBlob()
      {
         int width = 300;
         int height = 400;
         Image<Bgr, Byte> bg = new Image<Bgr, byte>(width, height);
         bg.SetRandNormal(new MCvScalar(), new MCvScalar(100, 100, 100));

         Size size = new Size(width / 10, height / 10);
         Point topLeft = new Point((width >> 1) - (size.Width >> 1), (height >> 1) - (size.Height >> 1));

         System.Drawing.Rectangle rect = new Rectangle(topLeft, size);

         BlobTrackerAutoParam param = new BlobTrackerAutoParam();
         param.BlobDetector = new BlobDetector(Emgu.CV.CvEnum.BLOB_DETECTOR_TYPE.CC);
         param.ForgroundDetector = new ForgroundDetector(Emgu.CV.CvEnum.FORGROUND_DETECTOR_TYPE.FGD);
         param.BlobTracker = new BlobTracker(Emgu.CV.CvEnum.BLOBTRACKER_TYPE.MSFGS);
         param.FGTrainFrames = 5;
         BlobTrackerAuto tracker = new BlobTrackerAuto(param);

         ImageViewer viewer = new ImageViewer();
         //viewer.Show();
         for (int i = 0; i < 20; i++)
         {
            using (Image<Bgr, Byte> img1 = bg.Copy())
            {
               rect.Offset(5, 0); //shift the rectangle 5 pixels horizontally
               img1.Draw(rect, new Bgr(Color.Red), -1);
               tracker.Process(img1);
               viewer.Image = img1;
               viewer.Refresh();
            }
         }
         MCvBlob blob = tracker[0];
         int id = blob.ID;
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

      /*
      private float[,] ProjectPoints(float[,] points3D, Matrix<float> rotation, Matrix<float> translation, float focalLength)
      {
         float[,] imagePoint = new float[points3D.GetLength(0), 2];
         for (int i = 0; i < imagePoint.GetLength(0); i++)
         {
            MCvPoint3D32f p = new MCvPoint3D32f(points3D[i, 0], points3D[i, 1], points3D[i, 2]);
            Matrix<float> p3D = new Matrix<float>(p.Coordinate);
            Matrix<float> pProjected = rotation * p3D + translation;
            pProjected = pProjected * (focalLength / (-pProjected[2, 0]));
            imagePoint[i, 0] = pProjected[0, 0];
            imagePoint[i, 1] = pProjected[1, 0];
         }
         return imagePoint;
      }*/

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
         RotationVector3D realRotVec = new RotationVector3D(new float[3] { 0.01f, 0.02f, 0.03f });
         Matrix<float> realRotMat = realRotVec.RotationMatrix;
         Matrix<float> realTransVec = new Matrix<float>(new float[3] { 0.0f, 0.0f, -50.0f });
         float[,] imagePoint = ProjectPoints(points3D, realRotMat, realTransVec, focalLength);
         #endregion

         RotationVector3D rotVecGuess = new RotationVector3D(new float[3] { 0.03f, 0.01f, 0.02f });
         Matrix<float> rotMatGuess = rotVecGuess.RotationMatrix;
         float[] rotFVecGuess = new float[] { rotMatGuess[0, 0], rotMatGuess[1, 0], rotMatGuess[2, 0], rotMatGuess[0, 1], rotMatGuess[1, 1], rotMatGuess[2, 1], rotMatGuess[0, 2], rotMatGuess[1, 2], rotMatGuess[2, 2] };
         float[] tranFVecGuess = new float[] { 0, 0, 5 };
         CvInvoke.cvPOSIT(posit, imagePoint, 0.5, new MCvTermCriteria(200, 1.0e-5), rotFVecGuess, tranFVecGuess);
         Matrix<float> rotMatEst = new Matrix<float>(new float[,] { 
            {rotFVecGuess[0], rotFVecGuess[3], rotFVecGuess[6]},
            {rotFVecGuess[1], rotFVecGuess[4], rotFVecGuess[7]},
            {rotFVecGuess[2], rotFVecGuess[5], rotFVecGuess[8]}});
         RotationVector3D rotVecEst = new RotationVector3D();
         Matrix<float> tranMatEst = new Matrix<float>(tranFVecGuess);

         rotVecEst.RotationMatrix = rotMatEst;
         //At this point rotVecEst should be similar to realRotVec, but it is not...

         float[,] projectionFromEst = ProjectPoints(points3D, rotMatEst, tranMatEst, focalLength);

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
         XmlDocument xDoc = Toolbox.XmlSerialize<PointF>(p, new Type[] { typeof(System.Drawing.Point) });
         PointF p2 = Toolbox.XmlDeserialize<PointF>(xDoc, new Type[] { typeof(System.Drawing.Point) });
         Assert.IsTrue(p.Equals(p2));

         
         System.Drawing.Rectangle rect = new Rectangle(3, 4, 5, 3);
         XmlDocument xDoc2 = Toolbox.XmlSerialize<System.Drawing.Rectangle>(rect);
         System.Drawing.Rectangle rect2 = Toolbox.XmlDeserialize<System.Drawing.Rectangle>(xDoc2);
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
      public void GetBox2DPoints()
      {
         MCvBox2D box = new MCvBox2D(
            new System.Drawing.PointF(3.0f, 2.0f), 
            new System.Drawing.SizeF(4.0f, 6.0f), 
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
         Image<Gray, Byte> img3 = new Image<Gray, byte>("tmp.png");
         stopwatch.Stop();
         Trace.WriteLine(string.Format("Time: {0} milliseconds", stopwatch.ElapsedMilliseconds));

         Assert.IsTrue(img.Convert<Gray, Byte>().Equals(img3));
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
         Point topLeft = new Point((width >> 1) - (size.Width >>1) , (height >> 1) - (size.Height >> 1));

         System.Drawing.Rectangle rect = new Rectangle(topLeft, size);

         Image<Bgr, Byte> img1 = bg.Copy();
         img1.Draw(rect, new Bgr(Color.Red), -1);

         Image<Bgr, Byte> img2 = bg.Copy();
         rect.Offset(10, 0);
         img2.Draw(rect, new Bgr(Color.Red), -1);

         BackgroundStatisticsModel model1 = new BackgroundStatisticsModel(img1, Emgu.CV.CvEnum.BG_STAT_TYPE.GAUSSIAN_BG_MODEL);
         model1.Update(img2);

         BackgroundStatisticsModel model2 = new BackgroundStatisticsModel(img1, Emgu.CV.CvEnum.BG_STAT_TYPE.FGD_STAT_MODEL);
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

         Stopwatch watch = Stopwatch.StartNew();
         PlanarSubdivision division;

         watch.Reset(); watch.Start();
         division = new PlanarSubdivision(points, true);
         Triangle2DF[] triangles = division.GetDelaunayTriangles(false);
         watch.Stop();
         Trace.WriteLine(String.Format("{0} milli-seconds, {1} triangles", watch.ElapsedMilliseconds, triangles.Length));

         watch.Reset(); watch.Start();
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
         System.Drawing.Rectangle objectLocation = new Rectangle(  templCenter.X - (templWidth >> 1), templCenter.Y - (templHeight >> 1) , templWidth, templHeight);
         img.ROI = objectLocation;
         randomObj.Copy(img, null);
         img.ROI = System.Drawing.Rectangle.Empty;
         #endregion

         Image<Gray, Single> match = img.MatchTemplate(randomObj, Emgu.CV.CvEnum.TM_TYPE.CV_TM_SQDIFF);
         double[] minVal, maxVal;
         System.Drawing.Point[] minLoc, maxLoc;
         match.MinMax(out minVal, out maxVal, out minLoc, out maxLoc);

         Assert.AreEqual(minLoc[0].X, templCenter.X - templWidth / 2);
         Assert.AreEqual(minLoc[0].Y, templCenter.Y - templHeight / 2);
      }

      [Test]
      public void TestOpticalFlow()
      {
         #region prepare synthetic image for testing
         //Create a random object
         Image<Gray, Byte> randomObj = new Image<Gray, byte>(50, 50);
         randomObj.SetRandUniform(new MCvScalar(), new MCvScalar(255));

         //Draw the object in image1 center at (100, 100);
         Image<Gray, Byte> prevImg = new Image<Gray, byte>(300, 200);
         Rectangle objectLocation = new Rectangle(75, 75, 50, 50);
         prevImg.ROI = objectLocation;
         randomObj.Copy(prevImg, null);
         prevImg.ROI = Rectangle.Empty;

         //Draw the object in image2 center at (102, 103);
         Image<Gray, Byte> currImg = new Image<Gray, byte>(300, 200);
         objectLocation.Offset(2, 3);
         currImg.ROI = objectLocation;
         randomObj.Copy(currImg, null);
         currImg.ROI = Rectangle.Empty;
         #endregion

         PointF[] prevFeature = new PointF[] { new PointF(100f, 100f) };

         PointF[] currFeature;
         Byte[] status;
         float[] trackError;

         Stopwatch watch = Stopwatch.StartNew();

         OpticalFlow.PyrLK(
            prevImg, currImg, prevFeature, new System.Drawing.Size(10, 10), 3, new MCvTermCriteria(10, 0.01),
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
         System.Drawing.Size patternSize = new System.Drawing.Size(6, 6);

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
            new System.Drawing.Size(10, 10),
            new System.Drawing.Size(-1, -1),
            new MCvTermCriteria(0.05));

         CameraCalibration.DrawChessboardCorners(chessboardImage, patternSize, corners, patternFound);
         //Application.Run(new ImageViewer(chessboardImage));
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
      public void TestFeatureTree()
      {
         Matrix<float>[] features = new Matrix<float>[10];
         for (int i = 0; i < features.Length; i++)
            features[i] = new Matrix<float>(new float[1, 1] { { (float)i } });
         FeatureTree tree = new FeatureTree(features);

         Matrix<Int32> result;
         Matrix<double> distance;
         Matrix<float>[] features2 = new Matrix<float>[1];
         features2[0] = new Matrix<float>(new float[1,1]{{ 5.0f}});

         tree.FindFeatures(features2, out result, out distance, 1, 20);
         Assert.AreEqual(result[0, 0], 5);
         Assert.AreEqual(distance[0, 0], 0.0);
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
         foreach(Image<Gray, Byte> img in images)
         {
            rec.Recognize(img);
            //Trace.WriteLine(rec.Recognize(img));
         }
      }

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
      }

      [Test]
      public void TestContourCreate()
      {
         using (MemStorage stor = new MemStorage())
         {
            Contour<System.Drawing.Point> contour = new Contour<Point>(stor);
            contour.Push(new System.Drawing.Point(0, 0));
            contour.Push(new System.Drawing.Point(0, 2));
            contour.Push(new System.Drawing.Point(2, 2));
            contour.Push(new System.Drawing.Point(2, 0));
            Assert.IsTrue(contour.Convex);
            Assert.AreEqual(contour.Area, 4.0);
            //InContour function requires MCvContour.rect to be pre-computed
            CvInvoke.cvBoundingRect(contour, 1);
            Assert.GreaterOrEqual(contour.InContour(new Point(1, 1)), 0);
            Assert.Less(contour.InContour(new Point(3, 3)), 0);

            Contour<System.Drawing.PointF> contourF = new Contour<PointF>(stor);
            contourF.Push(new System.Drawing.PointF(0, 0));
            contourF.Push(new System.Drawing.PointF(0, 2));
            contourF.Push(new System.Drawing.PointF(2, 2));
            contourF.Push(new System.Drawing.PointF(2, 0));
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

            Seq<System.Drawing.Point> seq = new Seq<Point>(CvInvoke.CV_MAKETYPE(4, 2), stor);
            seq.Push(new System.Drawing.Point(0, 0));
            seq.Push(new System.Drawing.Point(0, 1));
            seq.Push(new System.Drawing.Point(1, 1));
            seq.Push(new System.Drawing.Point(1, 0));
         }
      }

      [Test]
      public void TestConvexHull()
      {
         #region Create some random points
         Random r = new Random();
         PointF[] pts = new PointF[40];
         for (int i = 0; i < pts.Length; i++)
         {
            pts[i] = new PointF((float)(r.NextDouble() * 600), (float)(r.NextDouble() * 600));
         }
         #endregion

         #region Find the convex hull
         PointF[] hull;
         using (MemStorage storage = new MemStorage())
         {
            Seq<PointF> seq = PointCollection.ConvexHull(pts, storage, Emgu.CV.CvEnum.ORIENTATION.CV_CLOCKWISE);
            hull = seq.ToArray();
         }
         #endregion

         #region Draw the points and the convex hull
         Image<Bgr, Byte> img = new Image<Bgr, byte>(600, 600, new Bgr(255.0, 255.0, 255.0));
         foreach (PointF p in pts)
         {
            img.Draw(new CircleF(p, 3), new Bgr(0.0, 0.0, 0.0), 1);
         }

         img.DrawPolyline(
             Array.ConvertAll<System.Drawing.PointF, System.Drawing.Point>(hull, Point.Round),
             true, new Bgr(255.0, 0.0, 0.0), 1);
         #endregion

         //ImageViewer.Show(img);
      }

      [Test]
      public void TestStereoCorrespondence()
      {
         Image<Gray, Byte> left = new Image<Gray, byte>("left.jpg");
         Image<Gray, Byte> right = new Image<Gray, byte>("right.jpg");
         Image<Gray, Int16> leftDisparity = new Image<Gray, Int16>(left.Size);
         Image<Gray, Int16> rightDisparity = new Image<Gray, Int16>(left.Size);

         /*
         StereoBM bm = new StereoBM(Emgu.CV.CvEnum.STEREO_BM_TYPE.CV_STEREO_BM_BASIC, 0);
         bm.FindStereoCorrespondence(left, right, leftDisparity);
         */

         StereoGC gc = new StereoGC(10, 5);
         Stopwatch watch = Stopwatch.StartNew();
         gc.FindStereoCorrespondence(left, right, leftDisparity, rightDisparity);
         watch.Stop();
         Trace.WriteLine(String.Format("Time used: {0} milliseconds", watch.ElapsedMilliseconds));

         Matrix<double> q = new Matrix<double>(4, 4);
         q.SetIdentity();
         MCvPoint3D32f[] points = PointCollection.ReprojectImageTo3D(leftDisparity * (-16), q);
         
         float min = (float) 1.0e10, max = 0;
         foreach (MCvPoint3D32f p in points)
         {
            if (p.z < min) min = p.z;
            else if (p.z > max) max = p.z;
         }
         Trace.WriteLine(String.Format("Min : {0}\r\nMax : {1}", min, max));
         
         //ImageViewer.Show(leftDisparity*(-16));
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
         System.Random r = new Random();
         int sampleCount = 100;

         Image<Bgr, byte> img = new Image<Bgr, byte>(400, 400);
         PointF[] pts = new PointF[sampleCount];
         for (int i = 0; i < pts.Length; i++)
         {
            int x = r.Next(100) + 20;
            int y = r.Next(300) + 50;
            img[y, x] = new Bgr(255.0, 255.0, 255.0);
            pts[i] = new PointF(x, y);
         }

         Stopwatch watch = Stopwatch.StartNew();
         Ellipse e = PointCollection.EllipseLeastSquareFitting(pts);
         watch.Stop();
         Trace.WriteLine("Time used: " + watch.ElapsedMilliseconds + "milliseconds");

         img.Draw(e, new Bgr(120.0, 120.0, 120.0), 2);
         //ImageViewer.Show(img);
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
      public void TestVideoWriter()
      {
         int numberOfFrames = 100;
         int width = 300;
         int height = 200;
         String fileName = "tmp.avi";
         using (VideoWriter writer = new VideoWriter(fileName, 2, width, height, true))
         {
            for (int i = 0; i < numberOfFrames; i++)
            {
               Image<Bgr, Byte> img1 = new Image<Bgr, byte>(width, height);
               img1.SetRandUniform(new MCvScalar(), new MCvScalar(255, 255, 255));
               writer.WriteFrame(img1);
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
               img2 = capture.QueryFrame();
               count++;
            }
            Assert.AreEqual(numberOfFrames, count);
         }
         File.Delete(fi.FullName);
      }
      
   }
}
