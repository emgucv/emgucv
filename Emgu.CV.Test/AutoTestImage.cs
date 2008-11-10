using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using Emgu.CV;
using Emgu.UI;
using Emgu.CV.UI;
using Emgu.Util;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;
using System.Xml;
using System.IO;
using System.Runtime.Serialization;
using System.Threading;

namespace Emgu.CV.Test
{
   [TestFixture]
   public class AutoTestImage
   {
      [Test]
      public void TestRunningAvg()
      {
         Image<Gray, Single> img1 = new Image<Gray, float>(100, 40, new Gray(100));
         Image<Gray, Single> img2 = new Image<Gray, float>(100, 40, new Gray(50));
         img1.RunningAvg(img2, 0.5);
      }

      [Test]
      public void TestSetValue()
      {
         Image<Bgr, Single> img1 = new Image<Bgr, float>(50, 20, new Bgr(8.0, 1.0, 2.0));
         for (int i = 0; i < img1.Width; i++)
            for (int j = 0; j < img1.Height; j++)
            {
               Bgr c = img1[j, i];
               Assert.IsTrue(c.Equals(new Bgr(8.0, 1.0, 2.0)));
            }
      }

      [Test]
      public void TestMinMax()
      {
         Image<Gray, Byte> img1 = new Image<Gray, Byte>(50, 60);
         System.Random r = new Random();

         using (Image<Gray, Byte> img2 = img1.Convert<Byte>(delegate(Byte f) { return (Byte)r.Next(255); }))
         {
            img2._Max(120.0);
            for (int i = 0; i < img2.Width; i++)
               for (int j = 0; j < img2.Height; j++)
                  Assert.GreaterOrEqual(img2[j, i].Intensity, 120.0);
         }

         using (Image<Gray, Byte> img2 = img1.Convert<Byte>(delegate(Byte f) { return (Byte)r.Next(255); }))
         {
            img2._Min(120.0);
            for (int i = 0; i < img2.Width; i++)
               for (int j = 0; j < img2.Height; j++)
                  Assert.GreaterOrEqual(120.0, img2[j, i].Intensity);
         }

         using (Image<Gray, Byte> img2 = img1.Convert<Byte>(delegate(Byte f) { return (Byte)r.Next(255); }))
         using (Image<Gray, Byte> img3 = img1.Convert<Byte>(delegate(Byte f) { return (Byte)r.Next(255); }))
         using (Image<Gray, Byte> img4 = img2.Max(img3))
         {
            for (int i = 0; i < img2.Width; i++)
               for (int j = 0; j < img2.Height; j++)
               {
                  Point2D<int> location = new Point2D<int>(i, j);
                  Assert.GreaterOrEqual(img4[location].Intensity, img2[location].Intensity);
                  Assert.GreaterOrEqual(img4[j, i].Intensity, img3[j, i].Intensity);
               }
         }

         /*
         using (Image<Gray, Byte> img2 = img1.Convert<Byte>(delegate(Byte f) { return (Byte)r.Next(255); }))
         using (Image<Gray, Byte> img3 = img1.Convert<Byte>(delegate(Byte f) { return (Byte)r.Next(255); }))
         using (Image<Gray, Byte> img4 = img2.Min(img3))
         {
             for (int i = 0; i < img2.Width; i++)
                 for (int j = 0; j < img2.Height; j++)
                 {
                     Assert.GreaterOrEqual(img2.GetPixel(new Point2D<int>(i, j)).Intensity, img4.GetPixel(new Point2D<int>(i, j)).Intensity);
                     Assert.GreaterOrEqual(img3.GetPixel(new Point2D<int>(i, j)).Intensity, img4.GetPixel(new Point2D<int>(i, j)).Intensity);
                 }
         }*/
      }

      [Test]
      public void TestAvgSdv()
      {
         Image<Gray, Single> img1 = new Image<Gray, float>(50, 20);
         img1.SetRandNormal(new MCvScalar(100), new MCvScalar(30));
         Gray mean;
         MCvScalar std;
         img1.AvgSdv(out mean, out std);
      }

      [Test]
      public void TestGenericOperation()
      {
         Image<Gray, Single> img1 = new Image<Gray, float>(50, 20);
         img1.ROI = new Rectangle<double>(new MCvRect(10, 1, 50 - 10, 19 - 1));
         img1.SetValue(5.0);

         Image<Gray, Single> img2 = new Image<Gray, float>(50, 20);
         img2.ROI = new Rectangle<double>(new MCvRect(0, 2, 40, 20 - 2));
         img2.SetValue(new Gray(2.0));

         Assert.AreEqual(img1.Width, img2.Width);
         Assert.AreEqual(img1.Height, img2.Height);

         DateTime t1 = DateTime.Now;
         Image<Gray, Single> img3 = img1.Add(img2);
         DateTime t2 = DateTime.Now;
         Image<Gray, Single> img4 = img1.Convert<Single, Single>(img2, delegate(Single v1, Single v2) { return v1 + v2; });
         DateTime t3 = DateTime.Now;

         Image<Gray, Single> img5 = img3.AbsDiff(img4);
         DateTime t4 = DateTime.Now;
         double sum1 = img5.GetSum().Intensity;
         DateTime t5 = DateTime.Now;
         Single sum2 = 0.0f;
         img5.Action(delegate(Single v) { sum2 += v; });
         DateTime t6 = DateTime.Now;

         /*
         TimeSpan ts1 = t2.Subtract(t1);
         TimeSpan ts2 = t3.Subtract(t2);
         TimeSpan ts3 = t5.Subtract(t4);
         TimeSpan ts4 = t6.Subtract(t5);
         Trace.WriteLine(String.Format("CV Add     : {0} milliseconds", ts1.TotalMilliseconds));
         Trace.WriteLine(String.Format("Generic Add: {0} milliseconds", ts2.TotalMilliseconds));
         Trace.WriteLine(String.Format("CV Sum     : {0} milliseconds", ts3.TotalMilliseconds));
         Trace.WriteLine(String.Format("Generic Sum: {0} milliseconds", ts4.TotalMilliseconds));
         Trace.WriteLine(String.Format("Abs Diff = {0}", sum1));
         Trace.WriteLine(String.Format("Abs Diff = {0}", sum2));*/
         Assert.AreEqual(sum1, sum2);

         img3.Dispose();
         img4.Dispose();
         img5.Dispose();

         t1 = DateTime.Now;
         img3 = img1.Mul(2.0);
         t2 = DateTime.Now;
         img4 = img1.Convert<Single>(delegate(Single v1) { return v1 * 2.0f; });
         t3 = DateTime.Now;

         /*
         ts1 = t2.Subtract(t1);
         ts2 = t3.Subtract(t2);
         Trace.WriteLine(String.Format("CV Mul     : {0} milliseconds", ts1.TotalMilliseconds));
         Trace.WriteLine(String.Format("Generic Mul: {0} milliseconds", ts2.TotalMilliseconds));
         */
         Assert.IsTrue(img3.Equals(img4));
         img3.Dispose();
         img4.Dispose();

         t1 = DateTime.Now;
         img3 = img1.Add(img1);
         img4 = img3.Add(img1);
         t2 = DateTime.Now;
         img5 = img1.Convert<Single, Single, Single>(img1, img1, delegate(Single v1, Single v2, Single v3) { return v1 + v2 + v3; });
         t3 = DateTime.Now;
         /*
         ts1 = t2.Subtract(t1);
         ts2 = t3.Subtract(t2);
         Trace.WriteLine(String.Format("CV Sum (3 images)     : {0} milliseconds", ts1.TotalMilliseconds));
         Trace.WriteLine(String.Format("Generic Sum (3 images): {0} milliseconds", ts2.TotalMilliseconds));
         */
         Assert.IsTrue(img5.Equals(img4));
         img3.Dispose();
         img4.Dispose();
         img5.Dispose();

         img1.Dispose();
         img2.Dispose();

         Image<Gray, Byte> gimg1 = new Image<Gray, Byte>(400, 300, new Gray(30));
         Image<Gray, Byte> gimg2 = gimg1.Convert<Byte>(delegate(Byte b) { return (Byte)(255 - b); });
         gimg1.Dispose();
         gimg2.Dispose();
      }

      [Test]
      public void TestConvertDepth()
      {
         Image<Gray, Byte> img1 = new Image<Gray, byte>(100, 100, new Gray(10.0));
         img1.SetRandUniform(new MCvScalar(0, 0, 0), new MCvScalar(255, 255, 255));
         Image<Gray, Single> img2 = img1.ConvertScale<Single>(2.0, 0.0);
         Image<Gray, Byte> img3 = img2.ConvertScale<Byte>(0.5, 0.0);
         Assert.IsTrue(img3.Equals(img1));

         Image<Gray, Double> img4 = img1.Convert<Gray, Double>();
         Image<Gray, Byte> img5 = img4.Convert<Gray, Byte>();
         Assert.IsTrue(img5.Equals(img1));
      }

      [Test]
      public void TestMemory()
      {
         for (int i = 0; i <= 500; i++)
         {
            Image<Bgr, Single> img = new Image<Bgr, Single>(1000, 1000, new Bgr());
         }
      }

      [Test]
      public void TestConversion()
      {
         Image<Bgr, Single> img1 = new Image<Bgr, Single>(100, 100);
         img1.SetRandUniform(new MCvScalar(), new MCvScalar(255.0, 255.0, 255.0));

         Image<Xyz, Single> img2 = img1.Convert<Xyz, Single>();

         Image<Gray, Byte> img3 = img1.Convert<Gray, Byte>();

      }

      [Test]
      public void TestGenericSetColor()
      {
         Image<Bgr, Byte> img1 = new Image<Bgr, Byte>(20, 40, new Bgr());

         int flag = 0;

         Image<Bgr, Byte> img2 = img1.Convert<Byte>(
             delegate(Byte b)
             {
                return ((flag++ % 3) == 0) ? (Byte)255 : (Byte)0;
             });

         img1.SetValue(new Bgr(255, 0, 0));

         Image<Bgr, Byte> img = new Image<Bgr, byte>(800, 800);
         img.SetValue(255);
         Image<Bgr, Byte> mask = new Image<Bgr, byte>(img.Width, img.Height);
         mask.SetRandUniform(new MCvScalar(0, 0, 0), new MCvScalar(255, 255, 255)); //file the mask with random color

         DateTime startTime = DateTime.Now;
         Image<Bgr, Byte> imgMasked = img.Convert<Byte, Byte>(mask,
            delegate(Byte byteFromImg, Byte byteFromMask)
            {
               return byteFromMask > (Byte)120 ? byteFromImg : (Byte)0;
            });
         Trace.WriteLine(String.Format("Time used: {0} milliseconds", DateTime.Now.Subtract(startTime).TotalMilliseconds));

         Assert.IsTrue(img1.Equals(img2));
      }

      [Test]
      public void TestRuntimeSerialize()
      {
         Image<Bgr, Byte> img = new Image<Bgr, byte>(100, 80);

         using (MemoryStream ms = new MemoryStream())
         {
            img.SetRandNormal(new MCvScalar(100, 100, 100), new MCvScalar(50, 50, 50));

            System.Runtime.Serialization.Formatters.Binary.BinaryFormatter
                formatter = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
            formatter.Serialize(ms, img);
            Byte[] bytes = ms.GetBuffer();

            using (MemoryStream ms2 = new MemoryStream(bytes))
            {
               Image<Bgr, Byte> img2 = (Image<Bgr, Byte>)formatter.Deserialize(ms2);
               Assert.IsTrue(img.Equals(img2));
            }
         }
      }

      [Test]
      public void TestSampleLine()
      {
         Image<Bgr, Byte> img = new Image<Bgr, byte>(101, 133);
         img.SetRandUniform(new MCvScalar(), new MCvScalar(255, 255, 255));

         Byte[,] buffer = img.Sample(new LineSegment2D<int>(new Point2D<int>(0, 0), new Point2D<int>(0, 100)));
         for (int i = 0; i < 100; i++)
            Assert.IsTrue(img[i, 0].Equals(new Bgr(buffer[i, 0], buffer[i, 1], buffer[i, 2])));

         buffer = img.Sample(new LineSegment2D<int>(new Point2D<int>(0, 0), new Point2D<int>(100, 100)), Emgu.CV.CvEnum.LINE_SAMPLE_TYPE.FOUR_CONNECTED);
      }


      [Test]
      public void TestGetSize()
      {
         Image<Bgr, Byte> img = new Image<Bgr, byte>(10, 10, new Bgr(255, 255, 255));

         Point2D<int> size = img.Size;

      }

      [Test]
      public void TestXmlSerialize()
      {
         Image<Bgr, Byte> img = new Image<Bgr, byte>(100, 80);

         img.SetRandNormal(new MCvScalar(100, 100, 100), new MCvScalar(50, 50, 50));
         img.SerializationCompressionRatio = 9;
         XmlDocument doc1 = Toolbox.XmlSerialize<Image<Bgr, Byte>>(img);
         String str = doc1.OuterXml;
         Image<Bgr, Byte> img2 = Toolbox.XmlDeserialize<Image<Bgr, Byte>>(doc1);
         Assert.IsTrue(img.Equals(img2));

         img.SerializationCompressionRatio = 9;
         XmlDocument doc2 = Toolbox.XmlSerialize<Image<Bgr, Byte>>(img);
         Image<Bgr, Byte> img3 = Toolbox.XmlDeserialize<Image<Bgr, Byte>>(doc2);
         Assert.IsTrue(img.Equals(img3));

         XmlDocument doc3 = new XmlDocument();
         doc3.LoadXml(str);
         Image<Bgr, Byte> img4 = Toolbox.XmlDeserialize<Image<Bgr, Byte>>(doc3);
         Assert.IsTrue(img.Equals(img4));

      }

      [Test]
      public void TestRotation()
      {
         Image<Bgr, Byte> img = new Image<Bgr, byte>(100, 80);

         img.SetRandNormal(new MCvScalar(100, 100, 100), new MCvScalar(50, 50, 50));
         img.Rotate(90, new Bgr());
      }

      [Test]
      public void TestConstructor()
      {
         for (int i = 0; i < 20; i++)
         {
            Image<Gray, Byte> img = new Image<Gray, Byte>(500, 500, new Gray());
            Assert.AreEqual(0, System.Convert.ToInt32(img.GetSum().Intensity));
         }

         for (int i = 0; i < 20; i++)
         {
            Image<Bgr, Single> img = new Image<Bgr, Single>(500, 500);
            Assert.IsTrue(img.GetSum().Equals(new Bgr(0.0, 0.0, 0.0)));
         }

         Image<Bgr, Byte> img2 = new Image<Bgr, byte>(1, 2);
         Assert.AreEqual(img2.Data.GetLength(1), 4);

         Byte[, ,] data = new Byte[,,] { { { 255, 0, 0 } }, { { 0, 255, 0 } } };
         Image<Bgr, Byte> img3 = new Image<Bgr, byte>(data);

         Image<Gray, Single> img4 = new Image<Gray, float>("stuff.jpg");
         Image<Bgr, Single> img5 = new Image<Bgr, float>("stuff.jpg");

         Bitmap bmp = new Bitmap("stuff.jpg");
         Image<Bgr, Single> img6 = new Image<Bgr, float>(bmp);

         Image<Hsv, Single> img7 = new Image<Hsv, float>("stuff.jpg");
         Image<Hsv, Byte> img8 = new Image<Hsv, byte>("stuff.jpg");

      }

      [Test]
      public void TestSub()
      {
         Image<Bgr, Byte> img = new Image<Bgr, Byte>(101, 133);
         Assert.IsTrue(img.Not().Equals(255 - img));

         Image<Bgr, Byte> img2 = img - 10;
      }

      [Test]
      public void TestConvolutionAndLaplace()
      {
         Image<Gray, Byte> image = new Image<Gray, byte>(300, 400);
         image.SetRandUniform(new MCvScalar(0.0), new MCvScalar(255.0));

         Image<Gray, float> laplace = image.Laplace(1);

         float[,] k = { {0, 1, 0},
                        {1, -4, 1},
                        {0, 1, 0}};
         ConvolutionKernelF kernel = new ConvolutionKernelF(k);

         Image<Gray, float> convoluted = image * kernel;
         Assert.IsTrue(laplace.Equals(convoluted));

         /*
         Matrix<float> kernel1D = new Matrix<float>(new float[] { 1.0f, -2.0f, 1.0f });
         Image<Gray, float> result = new Image<Gray, float>(image.Width, image.Height);
         CvInvoke.cvFilter2D(image, result, kernel1D, new MCvPoint(-1, -1));
         */
      }

      [Test]
      public void TestBitmapConstructor()
      {
         #region test byte images
         Image<Bgr, Byte> image1 = new Image<Bgr, byte>(201, 401);
         image1.SetRandUniform(new MCvScalar(), new MCvScalar(255.0, 255.0, 255.0));
         Bitmap bmp = image1.ToBitmap();

         Image<Bgr, Byte> image2 = new Image<Bgr, byte>(bmp);
         Assert.IsTrue(image1.Equals(image2));

         Image<Gray, Byte> image3 = new Image<Gray, byte>(11, 7);
         image3.SetRandUniform(new MCvScalar(), new MCvScalar(255.0, 255.0, 255.0));
         bmp = image3.ToBitmap();
         DateTime t1 = DateTime.Now;
         Image<Gray, Byte> image4 = new Image<Gray, byte>(bmp);
         Trace.WriteLine(DateTime.Now.Subtract(t1).TotalMilliseconds);
         Assert.IsTrue(image3.Equals(image4));
         #endregion

         #region test single images
         Image<Bgr, Single> image5 = new Image<Bgr, Single>(201, 401);
         image5.SetRandUniform(new MCvScalar(), new MCvScalar(255.0, 255.0, 255.0));
         Bitmap bmp2 = image5.ToBitmap();
         #endregion
      }

      [Test]
      public void TestSplitMerge()
      {
         Image<Bgr, Byte> img1 = new Image<Bgr, byte>(301, 234);
         img1.SetRandUniform(new MCvScalar(), new MCvScalar(255, 255, 255));
         Image<Gray, Byte>[] channels = img1.Split();

         Image<Bgr, Byte> img2 = new Image<Bgr, byte>(channels);
         Assert.IsTrue(img1.Equals(img2));
      }

      [Test]
      public void TestAcc()
      {
         Image<Gray, Single> img1 = new Image<Gray, Single>(300, 200);
         img1.SetRandUniform(new MCvScalar(0), new MCvScalar(255));
         Image<Gray, Single> img2 = new Image<Gray, Single>(300, 200);
         img2.SetRandUniform(new MCvScalar(0), new MCvScalar(255));

         Image<Gray, Single> img3 = img1.Copy();
         img3.Acc(img2);

         Assert.IsTrue(img3.Equals(img1 + img2));
      }

      [Test]
      public void TestCanny()
      {
         Image<Bgr, Byte> image = new Image<Bgr, byte>("stuff.jpg");

         //make sure canny works for multi channel image
         Image<Bgr, Byte> image2 = image.Canny(new Bgr(200, 200, 200), new Bgr(100, 100, 100));
      }

      [Test]
      public void TestInplaceFlip()
      {
         Image<Bgr, byte> image = new Image<Bgr, byte>(20, 20);
         image.SetRandUniform(new MCvScalar(), new MCvScalar(255, 255, 255));

         Image<Bgr, byte> imageOld = image.Copy();
         image._Flip(Emgu.CV.CvEnum.FLIP.VERTICAL);

         for (int i = 0; i < image.Rows; i++)
            for (int j = 0; j < image.Cols; j++)
            {
               Bgr c1 = image[i, j];
               Bgr c2 = imageOld[image.Rows - i - 1, j];
               Assert.IsTrue(c1.Equals(c2));
            }
      }

      [Test]
      public void TestMoment()
      {
         Image<Gray, byte> image = new Image<Gray, byte>(100, 200);
         image.SetRandUniform(new MCvScalar(), new MCvScalar(255));
         image.ThresholdToZero(new Gray(120));
         MCvMoments moment = image.GetMoments(true);
      }

      [Test]
      public void TestSURF()
      {
         Image<Gray, Byte> objectImage = new Image<Gray, byte>("box.png");
         objectImage = objectImage.Resize(400, 400, true);
         DateTime t1 = DateTime.Now;
         MCvSURFParams param1 = new MCvSURFParams(500, false);
         SURFFeature[] objectFeatures = objectImage.ExtractSURF(ref param1);
         Trace.WriteLine(String.Format("{0} milli-sec", DateTime.Now.Subtract(t1).TotalMilliseconds));

         Image<Gray, Byte> image = new Image<Gray, byte>("box_in_scene.png");
         image = image.Resize(400, 400, true);
         t1 = DateTime.Now;
         MCvSURFParams param2 = new MCvSURFParams(500, false);
         SURFFeature[] imageFeatures = image.ExtractSURF(ref param2);
         Trace.WriteLine(String.Format("{0} milli-sec", DateTime.Now.Subtract(t1).TotalMilliseconds));

         Image<Gray, Byte> res = new Image<Gray, byte>(Math.Max(objectImage.Width, image.Width), objectImage.Height + image.Height);
         res.ROI = new Rectangle<double>( new MCvRect( 0, 0, objectImage.Width, objectImage.Height));
         objectImage.Copy(res, null);
         res.ROI = new Rectangle<double>( new MCvRect( 0, objectImage.Height, image.Width, image.Height ));
         image.Copy(res, null);
         res.ROI = null;

         t1 = DateTime.Now;
         List<Point2D<float>> list1 = new List<Point2D<float>>();
         List<Point2D<float>> list2 = new List<Point2D<float>>();
         foreach (SURFFeature f in objectFeatures)
         {
            double[] distance = Array.ConvertAll<SURFFeature, double>(imageFeatures,
               delegate(SURFFeature imgFeature)
               {
                  if (imgFeature.Point.laplacian != f.Point.laplacian)
                     return -1;
                  return CvInvoke.cvNorm(imgFeature.Descriptor, f.Descriptor, Emgu.CV.CvEnum.NORM_TYPE.CV_L2, IntPtr.Zero);
               });

            int closestIndex = 0;
            int secondClosestIndex = 0;

            for (int i = 0; i < distance.Length; i++)
            {
               if (distance[i] >= 0)
               {
                  if (distance[i] < distance[closestIndex] || distance[closestIndex] == -1)
                  {
                     secondClosestIndex = closestIndex;
                     closestIndex = i;
                  }
               }
            }
            if (distance[closestIndex] < 0.6 * distance[secondClosestIndex])
            { //If this is almost a unique match
               Point2D<float> p1 = new Point2D<float>((float)f.Point.pt.x, (float)f.Point.pt.y);
               SURFFeature match = imageFeatures[closestIndex];
               Point2D<float> p2 = new Point2D<float>((float)match.Point.pt.x, (float)match.Point.pt.y);
               list1.Add(p1);
               list2.Add(p2);

               Point2D<float> p = p2.Convert<float>();
               p.Y += objectImage.Height;
               res.Draw(new LineSegment2D<int>(p1.Convert<int>(), p.Convert<int>()), new Gray(0), 1);
            }
         }

         Matrix<float> homographyMatrix = CameraCalibration.FindHomography(list1.ToArray(), list2.ToArray(), CvEnum.HOMOGRAPHY_METHOD.RANSAC, 3);
         Rectangle<double> rect = objectImage.ROI;

         Point2D<double>[] pts = new Point2D<double>[]
         {
            HomographyTransform( rect.BottomLeft, homographyMatrix ),
            HomographyTransform( rect.BottomRight, homographyMatrix ),
            HomographyTransform( rect.TopRight, homographyMatrix ),
            HomographyTransform( rect.TopLeft, homographyMatrix )
         };

         foreach (Point2D<double> p in pts)
         {
            p.Y += objectImage.Height;
         }

         res.DrawPolyline(pts, true, new Gray(255.0), 5);

         Trace.WriteLine(String.Format("{0} milli-sec", DateTime.Now.Subtract(t1).TotalMilliseconds));

         //Application.Run(new ImageViewer(res.Resize(200, 200, true)));
      }

      private Point2D<double> HomographyTransform(Point2D<double> p, Matrix<float> homographyMatrix)
      {
         Matrix<float> pMat = new Matrix<float>(p.Convert<float>().Resize(3).Coordinate);
         pMat[2, 0] = 1.0f;
         pMat = homographyMatrix * pMat;
         pMat = pMat / (double)pMat[2, 0];
         return new Point2D<double>((double)pMat[0, 0], (double)pMat[1, 0]);
      }

      [Test]
      public void TestSnake()
      {
         Image<Gray, Byte> img = new Image<Gray, Byte>(100, 100, new Gray());

         Point2D<double> center = new Point2D<double>(50, 50);
         double width = 20;
         double height = 40;
         Rectangle<double> rect = new Rectangle<double>(center, width, height);
         img.Draw(rect, new Gray(255.0), -1);

         using (MemStorage stor = new MemStorage())
         {
            Seq<MCvPoint> pts = new Seq<MCvPoint>((int)CvEnum.SEQ_TYPE.CV_SEQ_POLYGON, stor);
            pts.Push(new MCvPoint(20, 20));
            pts.Push(new MCvPoint(20, 80));
            pts.Push(new MCvPoint(80, 80));
            pts.Push(new MCvPoint(80, 20));

            Image<Gray, Byte> canny = img.Canny(new Gray(100.0), new Gray(40.0));
            Seq<MCvPoint> snake = canny.Snake(pts, 1.0f, 1.0f, 1.0f, new MCvSize(21, 21), new MCvTermCriteria(40, 0.0002), stor);

            img.Draw(pts, new Gray(120), 1);
            img.Draw(snake, new Gray(80), 2);
            //Application.Run(new ImageViewer(img));
         }
      }

      [Test]
      public void TestDFT()
      {
         Image<Gray, float> matA = new Image<Gray, float>("stuff.jpg");
         
         Matrix<float> matB = new Matrix<float>(
            new float[,] { 
            {0, 1, 0}, 
            {1, -4, 1}, 
            {0, 1, 0}}); //a simpel laplace filter

         Image<Gray, float> convResult1 = new Image<Gray, float>(matA.Cols + matB.Cols - 1, matA.Rows + matB.Rows - 1);
         int dft_rows = CvInvoke.cvGetOptimalDFTSize(convResult1.Rows);
         int dft_cols = CvInvoke.cvGetOptimalDFTSize(convResult1.Cols);

         Matrix<float> dftA = new Matrix<float>(dft_rows, dft_cols);

         matA.CopyTo(dftA.GetSubMatrix(new Rectangle<double>(new MCvRect(0, 0, matA.Width, matA.Height))));

         CvInvoke.cvDFT(dftA, dftA, Emgu.CV.CvEnum.CV_DXT.CV_DXT_FORWARD, matA.Rows);

         Matrix<float> dftB = new Matrix<float>(dft_rows, dft_cols);
         matB.CopyTo(dftB.GetSubMatrix(new Rectangle<double>(new MCvRect(0, 0, matB.Width, matB.Height))));
         CvInvoke.cvDFT(dftB, dftB, Emgu.CV.CvEnum.CV_DXT.CV_DXT_FORWARD, matB.Rows);

         CvInvoke.cvMulSpectrums(dftA, dftB, dftA, Emgu.CV.CvEnum.MUL_SPECTRUMS_TYPE.DEFAULT);
         CvInvoke.cvDFT(dftA, dftA, Emgu.CV.CvEnum.CV_DXT.CV_DXT_INVERSE, convResult1.Rows);
         dftA.GetSubMatrix(new Rectangle<double>(new MCvRect(0, 0, convResult1.Width, convResult1.Height))).CopyTo(convResult1);

         //ImageViewer.Show(convResult1);
      }

      /*
      [Test]
      public void T()
      {
         DateTime t1 = DateTime.Now;
         for (int i = 0; i < 100; i++)
         {
            Image<Gray, Byte> img = new Image<Gray, byte>("01E01002.TIF");
         }
         Trace.WriteLine(DateTime.Now.Subtract(t1).TotalMilliseconds / 100);
      }*/
   }
}
