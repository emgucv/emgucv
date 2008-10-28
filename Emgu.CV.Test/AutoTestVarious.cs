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
               Contour<MCvPoint> cs = img.FindContours(CvEnum.CHAIN_APPROX_METHOD.CV_CHAIN_APPROX_SIMPLE, CvEnum.RETR_TYPE.CV_RETR_LIST, stor);
               Assert.AreEqual(cs.MCvContour.elem_size, Marshal.SizeOf(typeof(MCvPoint)));
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

               MCvMoments moment = cs.GetMoments();
               Assert.IsTrue(moment.GravityCenter.Equals(rect2.Center));
            }

            using (MemStorage stor = new MemStorage())
            {
               Image<Gray, Byte> img2 = new Image<Gray, byte>(300, 200);
               Contour<MCvPoint> c = img2.FindContours(Emgu.CV.CvEnum.CHAIN_APPROX_METHOD.CV_CHAIN_APPROX_SIMPLE, Emgu.CV.CvEnum.RETR_TYPE.CV_RETR_LIST, stor);
               Assert.AreEqual(c, null);
            }
         }

         int s1 = Marshal.SizeOf(typeof(MCvSeq));
         int s2 = Marshal.SizeOf(typeof(MCvContour));
         int sizeRect = Marshal.SizeOf(typeof(MCvRect));
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

         /*
         using (MemStorage stor = new MemStorage())
         {
            Seq<MCvPoint2D32f> contour = new Seq<MCvPoint2D32f>(
               CvEnum.SEQ_ELTYPE.CV_SEQ_ELTYPE_GENERIC,
               CvEnum.SEQ_KIND.CV_SEQ_KIND_CURVE, 
               CvEnum.SEQ_FLAG.CV_SEQ_FLAG_CLOSED, 
               stor);
            foreach (Point2D<float> p in tri.Vertices)
            {
               contour.Push(p.MCvPoint2D32f);
            }
            Assert.IsTrue(contour.InContour(p1) > 0);
         }*/
      }

      private float[,] ProjectPoints(float[,] points3D, Matrix<float> rotation, Matrix<float> translation, float focalLength)
      {
         float[,] imagePoint = new float[points3D.GetLength(0), 2];
         for (int i = 0; i < imagePoint.GetLength(0); i++)
         {
            Point3D<float> p = new Point3D<float>(points3D[i, 0], points3D[i, 1], points3D[i, 2]);
            Matrix<float> p3D = new Matrix<float>(p.Coordinate);
            Matrix<float> pProjected = rotation * p3D + translation;
            pProjected = pProjected * (focalLength / (-pProjected[2, 0]));
            imagePoint[i, 0] = pProjected[0, 0];
            imagePoint[i, 1] = pProjected[1, 0];
         }
         return imagePoint;
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
         Assert.IsTrue(Math.Abs(tri.Area - 0.5) < epsilon);
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

         Image<Bgr, Byte> img1 = bg.Copy();
         img1.Draw(new Rectangle<double>(new Point2D<double>(width >> 1, height >> 1), width / 10, height / 10), new Bgr(Color.Red), -1);

         Image<Bgr, Byte> img2 = bg.Copy();
         img2.Draw(new Rectangle<double>(new Point2D<double>(width >> 1 + 10, height >> 1), width / 10, height / 10), new Bgr(Color.Red), -1);

         BackgroundStatisticsModel model1 = new BackgroundStatisticsModel(img1, Emgu.CV.CvEnum.BG_STAT_TYPE.GAUSSIAN_BG_MODEL);
         model1.Update(img2);

         BackgroundStatisticsModel model2 = new BackgroundStatisticsModel(img1, Emgu.CV.CvEnum.BG_STAT_TYPE.FGD_STAT_MODEL);
         model2.Update(img2);

         //Application.Run(new ImageViewer(model2.Foreground));
         //Application.Run(new ImageViewer(model.BackGround));
      }

      [Test]
      public void TestPlannarSubdivision1()
      {
         int pointCount = 1000;

         #region generate random points
         Point2D<float>[] points = new Point2D<float>[pointCount];
         Random r = new Random((int)DateTime.Now.Ticks);
         for (int i = 0; i < points.Length; i++)
         {
            points[i] = new Point2D<float>((float)(r.NextDouble() * 20), (float)(r.NextDouble() * 20));
         }
         #endregion

         DateTime t1 = DateTime.Now;
         PlanarSubdivision division = new PlanarSubdivision(points);

         List<Triangle2D<float>> triangles = division.GetDelaunayTriangles(false);
         Trace.WriteLine(String.Format("{0} milli-seconds, {1} triangles", DateTime.Now.Subtract(t1).TotalMilliseconds, triangles.Count));

         t1 = DateTime.Now;
         division = new PlanarSubdivision(points);

         List<VoronoiFacet> facets = division.GetVoronoiFacets();
         Trace.WriteLine(String.Format("{0} milli-seconds, {1} facets", DateTime.Now.Subtract(t1).TotalMilliseconds, facets.Count));

         foreach (Triangle2D<float> t in triangles)
         {
            int equalCount = triangles.FindAll(delegate(Triangle2D<float> t2) { return t2.Equals(t); }).Count;
            Assert.AreEqual(1, equalCount, "Triangle duplicates");

            int overlapCount = triangles.FindAll(delegate(Triangle2D<float> t2) { return Utils.IsConvexPolygonInConvexPolygon(t2, t);}).Count;
            Assert.AreEqual(1, overlapCount, "Triangle overlaps");
         }
      }

      [Test]
      public void TestPlannarSubdivision2()
      {
         Point2D<float>[] pts = new Point2D<float>[33];

         pts[0] = new Point2D<float>(224, 432);
         pts[1] = new Point2D<float>(368, 596);
         pts[2] = new Point2D<float>(316, 428);
         pts[3] = new Point2D<float>(244, 596);
         pts[4] = new Point2D<float>(224, 436);
         pts[5] = new Point2D<float>(224, 552);
         pts[6] = new Point2D<float>(276, 568);
         pts[7] = new Point2D<float>(308, 472);
         pts[8] = new Point2D<float>(316, 588);
         pts[9] = new Point2D<float>(368, 536);
         pts[10] = new Point2D<float>(332, 428);
         pts[11] = new Point2D<float>(124, 380);
         pts[12] = new Point2D<float>(180, 400);
         pts[13] = new Point2D<float>(148, 360);
         pts[14] = new Point2D<float>(148, 416);
         pts[15] = new Point2D<float>(128, 372);
         pts[16] = new Point2D<float>(124, 392);
         pts[17] = new Point2D<float>(136, 412);
         pts[18] = new Point2D<float>(156, 416);
         pts[19] = new Point2D<float>(176, 404);
         pts[20] = new Point2D<float>(180, 384);
         pts[21] = new Point2D<float>(168, 364);
         pts[22] = new Point2D<float>(260, 104);
         pts[23] = new Point2D<float>(428, 124);
         pts[24] = new Point2D<float>(328, 32);
         pts[25] = new Point2D<float>(320, 200);
         pts[26] = new Point2D<float>(268, 76);
         pts[27] = new Point2D<float>(264, 144);
         pts[28] = new Point2D<float>(316, 196);
         pts[29] = new Point2D<float>(384, 196);
         pts[30] = new Point2D<float>(424, 136);
         pts[31] = new Point2D<float>(412, 68);
         pts[32] = new Point2D<float>(348, 32);

         #region Find the region of interest
         Rectangle<float> roi;
         using (MemStorage storage = new MemStorage())
         using (Seq<MCvPoint2D32f> seq = PointCollection.To2D32fSequence(storage, Emgu.Util.Toolbox.IEnumConvertor<Point2D<float>, Point<float>>(pts, delegate(Point2D<float> p) { return (Point<float>)p; })))
         {
            MCvRect cvRect = CvInvoke.cvBoundingRect(seq.Ptr, true);
            roi = new Rectangle<float>(cvRect);
         }
         #endregion

         PlanarSubdivision subdiv = new PlanarSubdivision(roi);
         for (int i = 0; i < pts.Length; i++)
         {
            MCvPoint2D32f ptToInsert = pts[i].MCvPoint2D32f;

            {
               MCvSubdiv2DEdge? edge;
               MCvSubdiv2DPoint? point;
               CvEnum.Subdiv2DPointLocationType location = subdiv.Locate(ref ptToInsert, out edge, out point);
               if (location == Emgu.CV.CvEnum.Subdiv2DPointLocationType.ON_EDGE)
               {
                  //you might want to store the points which is not inserted here.
                  //or alternatively, add some random noise to the point and re-insert it again.
                  continue;
               }
            }

            subdiv.Insert(ref ptToInsert);
         }

         List<VoronoiFacet> facets = subdiv.GetVoronoiFacets();

      }

      [Test]
      public void TestGetModuleInfo()
      {
         Image<Bgr, Byte> img = new Image<Bgr, byte>(200, 100);
         img.Sobel(1, 0, 3);
         String plugin, module;
         Utils.GetModuleInfo(out plugin, out module);
      }

      [Test]
      public void TestCrossProduct()
      {
         Point3D<float> p1 = new Point3D<float>(1.0f, 0.0f, 0.0f);
         Point3D<float> p2 = new Point3D<float>(0.0f, 1.0f, 0.0f);
         Point3D<float> p3 = p1.CrossProduct(p2);
         Assert.IsTrue(new Point3D<float>(0.0f, 0.0f, 1.0f).Equals(p3));
      }

      [Test]
      public void TestMatchTemplate()
      {
         #region prepare synthetic image for testing
         int templWidth = 50;
         int templHeight = 50;
         Point2D<double> templCenter = new Point2D<double>(120, 100);

         //Create a random object
         Image<Bgr, Byte> randomObj = new Image<Bgr, byte>(templWidth, templHeight);
         randomObj.SetRandUniform(new MCvScalar(), new MCvScalar(255, 255, 255));

         //Draw the object in image1 center at templCenter;
         Image<Bgr, Byte> img = new Image<Bgr, byte>(300, 200);
         Rectangle<double> objectLocation = new Rectangle<double>(templCenter, 50, 50);
         img.ROI = objectLocation;
         randomObj.Copy(img, null);
         img.ROI = null;
         #endregion

         Image<Gray, Single> match = img.MatchTemplate(randomObj, Emgu.CV.CvEnum.TM_TYPE.CV_TM_SQDIFF);
         double[] minVal, maxVal;
         MCvPoint[] minLoc, maxLoc;
         match.MinMax(out minVal, out maxVal, out minLoc, out maxLoc);

         Assert.AreEqual(minLoc[0].x, templCenter.X - templWidth / 2);
         Assert.AreEqual(minLoc[0].y, templCenter.Y - templHeight / 2);
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
         Rectangle<double> objectLocation = new Rectangle<double>(new Point2D<double>(100, 100), 50, 50);
         prevImg.ROI = objectLocation;
         randomObj.Copy(prevImg, null);
         prevImg.ROI = null;

         //Draw the object in image2 center at (102, 103);
         Image<Gray, Byte> currImg = new Image<Gray, byte>(300, 200);
         objectLocation.Center += new Point2D<double>(2, 3);
         currImg.ROI = objectLocation;
         randomObj.Copy(currImg, null);
         currImg.ROI = null;
         #endregion

         Point2D<float>[] prevFeature = new Point2D<float>[] { new Point2D<float>(100f, 100f) };

         Point2D<float>[] currFeature;
         Byte[] status;
         float[] trackError;

         DateTime t = DateTime.Now;

         OpticalFlow.PyrLK(
            prevImg, currImg, prevFeature, new MCvSize(10, 10), 3, new MCvTermCriteria(10, 0.01),
            out currFeature, out status, out trackError);

         Trace.WriteLine(String.Format(
            "prev: ({0}, {1}); curr: ({2}, {3}); Time: {4} milliseconds",
            prevFeature[0].X, prevFeature[0].Y,
            currFeature[0].X, currFeature[0].Y,
            DateTime.Now.Subtract(t).TotalMilliseconds));

      }

      [Test]
      public void TestChessboardCalibration()
      {
         MCvSize patternSize = new MCvSize(6, 6);

         Image<Gray, Byte> chessboardImage = new Image<Gray, byte>("chessBoard.jpg");
         Point2D<float>[] corners;
         bool patternFound =
            CameraCalibration.FindChessboardCorners(
            chessboardImage,
            patternSize,
            Emgu.CV.CvEnum.CALIB_CB_TYPE.ADAPTIVE_THRESH | Emgu.CV.CvEnum.CALIB_CB_TYPE.NORMALIZE_IMAGE | Emgu.CV.CvEnum.CALIB_CB_TYPE.FILTER_QUADS,
            out corners);

         chessboardImage.FindCornerSubPix(
            new Point2D<float>[][] { corners },
            new MCvSize(10, 10),
            new MCvSize(-1, -1),
            new MCvTermCriteria(0.05));

         CameraCalibration.DrawChessboardCorners(chessboardImage, patternSize, corners, patternFound);
         //Application.Run(new ImageViewer(chessboardImage));
      }

      [Test]
      public void TestFillConvexPolygon()
      {
         Image<Bgr, Byte> img = new Image<Bgr, byte>(200, 200);
         Rectangle<double> rect = new Rectangle<double>(new Point2D<double>(100, 100), 50, 50);
         img.Draw((IConvexPolygon<double>)rect, new Bgr(Color.Blue), 0);
         //Application.Run(new ImageViewer(img));
      }

      //TODO: Figure out why the following test case cannot passed
      /*
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
      }*/
   }
}
