using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using Emgu.CV;
using Emgu.UI;
using Emgu.CV.UI;
using Emgu.CV.Structure;
using Emgu.Util;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;
using System.Xml;
using System.IO;
using System.Runtime.Serialization;
using System.Threading;
using System.Runtime.InteropServices;

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
         IImage img = img2;
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
                  Point location = new Point(i, j);
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
         img1.ROI = new Rectangle(10, 1, 50 - 10, 19 - 1);
         img1.SetValue(5.0);

         Image<Gray, Single> img2 = new Image<Gray, float>(50, 20);
         img2.ROI = new Rectangle(0, 2, 40, 20 - 2);
         img2.SetValue(new Gray(2.0));

         Assert.AreEqual(img1.Width, img2.Width);
         Assert.AreEqual(img1.Height, img2.Height);

         Stopwatch watch = Stopwatch.StartNew();
         Image<Gray, Single> img3 = img1.Add(img2);
         long cvAddTime = watch.ElapsedMilliseconds;

         watch.Reset(); watch.Start();
         Image<Gray, Single> img4 = img1.Convert<Single, Single>(img2, delegate(Single v1, Single v2) { return v1 + v2; });
         long genericAddTime = watch.ElapsedMilliseconds;

         Image<Gray, Single> img5 = img3.AbsDiff(img4);

         watch.Reset(); watch.Start();
         double sum1 = img5.GetSum().Intensity;
         long cvSumTime = watch.ElapsedMilliseconds;

         watch.Reset(); watch.Start();
         Single sum2 = 0.0f;
         img5.Action(delegate(Single v) { sum2 += v; });
         long genericSumTime = watch.ElapsedMilliseconds;

         Trace.WriteLine(String.Format("CV Add     : {0} milliseconds", cvAddTime));
         Trace.WriteLine(String.Format("Generic Add: {0} milliseconds", genericAddTime));
         Trace.WriteLine(String.Format("CV Sum     : {0} milliseconds", cvSumTime));
         Trace.WriteLine(String.Format("Generic Sum: {0} milliseconds", genericSumTime));
         Trace.WriteLine(String.Format("Abs Diff = {0}", sum1));
         Trace.WriteLine(String.Format("Abs Diff = {0}", sum2));
         Assert.AreEqual(sum1, sum2);

         img3.Dispose();
         img4.Dispose();
         img5.Dispose();

         DateTime t1 = DateTime.Now;
         img3 = img1.Mul(2.0);
         DateTime t2 = DateTime.Now;
         img4 = img1.Convert<Single>(delegate(Single v1) { return v1 * 2.0f; });
         DateTime t3 = DateTime.Now;

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
         for (int i = 0; i <= 100; i++)
         {
            Image<Bgr, Single> img = new Image<Bgr, Single>(1000, 1000);
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

         Stopwatch watch = Stopwatch.StartNew();
         Image<Bgr, Byte> imgMasked = img.Convert<Byte, Byte>(mask,
            delegate(Byte byteFromImg, Byte byteFromMask)
            {
               return byteFromMask > (Byte)120 ? byteFromImg : (Byte)0;
            });
         watch.Stop();
         Trace.WriteLine(String.Format("Time used: {0} milliseconds", watch.ElapsedMilliseconds));

         Assert.IsTrue(img1.Equals(img2));
      }

      [Test]
      public void TestRuntimeSerialize()
      {
         Image<Bgr, Byte> img = new Image<Bgr, byte>(100, 80);

         using (MemoryStream ms = new MemoryStream())
         {
            img.SetRandNormal(new MCvScalar(100, 100, 100), new MCvScalar(50, 50, 50));
            img.SerializationCompressionRatio = 9;
            System.Runtime.Serialization.Formatters.Binary.BinaryFormatter
                formatter = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
            formatter.Serialize(ms, img);
            Byte[] bytes = ms.GetBuffer();

            using (MemoryStream ms2 = new MemoryStream(bytes))
            {
               Object o = formatter.Deserialize(ms2);
               Image<Bgr, Byte> img2 = (Image<Bgr, Byte>) o;
               Assert.IsTrue(img.Equals(img2));
            }
         }
      }

      [Test]
      public void TestSampleLine()
      {
         Image<Bgr, Byte> img = new Image<Bgr, byte>(101, 133);
         img.SetRandUniform(new MCvScalar(), new MCvScalar(255, 255, 255));

         Byte[,] buffer = img.Sample(new LineSegment2D(new Point(0, 0), new Point(0, 100)));
         for (int i = 0; i < 100; i++)
            Assert.IsTrue(img[i, 0].Equals(new Bgr(buffer[i, 0], buffer[i, 1], buffer[i, 2])));

         buffer = img.Sample(new LineSegment2D(new Point(0, 0), new Point(100, 100)), Emgu.CV.CvEnum.CONNECTIVITY.FOUR_CONNECTED);
      }


      [Test]
      public void TestGetSize()
      {
         Image<Bgr, Byte> img = new Image<Bgr, byte>(10, 10, new Bgr(255, 255, 255));
         Size size = img.Size;
         Assert.AreEqual(size, new Size(10, 10));
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
         
         Image<Bgr, Byte> imgRotated = img.Rotate(90, new Bgr(), false);
         Assert.AreEqual(img.Width, imgRotated.Height);
         Assert.AreEqual(img.Height, imgRotated.Width);
         imgRotated = img.Rotate(30, new Bgr(255, 255, 255), false);
         
      }

      [Test]
      public void TestFaceDetect()
      {
         Image<Gray, Byte> image = new Image<Gray, byte>("lena.jpg");
         //using (HaarCascade cascade = new HaarCascade(@".\haarcascades\eye_12.xml"))
         using (HaarCascade cascade = new HaarCascade(@".\haarcascades\haarcascade_eye.xml"))
         //using (HaarCascade cascade = new HaarCascade(@".\haarcascades\haarcascade_frontalface_alt2.xml"))
         {
            MCvAvgComp[][] objects = image.DetectHaarCascade(cascade, 1.05, 0, Emgu.CV.CvEnum.HAAR_DETECTION_TYPE.DO_CANNY_PRUNING, new Size(10, 10));

            foreach (MCvAvgComp obj in objects[0])
               image.Draw(obj.rect, new Gray(0.0), 1);

            using (MemStorage stor = new MemStorage())
            {
               IntPtr objs = CvInvoke.cvHaarDetectObjects(
                             image.Ptr,
                             cascade.Ptr,
                             stor.Ptr,
                             1.05,
                             0,
                             Emgu.CV.CvEnum.HAAR_DETECTION_TYPE.DO_CANNY_PRUNING,
                             new Size(10, 10));

               if (objs != IntPtr.Zero)
               {
                  Seq<MCvAvgComp> rects = new Seq<MCvAvgComp>(objs, stor);

                  MCvAvgComp[] rect = rects.ToArray();
                  for (int i = 0; i < rects.Total; i++)
                  {
                     Assert.AreEqual(rect[i].rect, rects[i].rect);
                  }
               }
            }
         }
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
         try
         {
            Matrix<float> kernel1D = new Matrix<float>(new float[] { 1.0f, -2.0f, 1.0f });
            Image<Gray, float> result = new Image<Gray, float>(image.Width, image.Height);
            CvInvoke.cvFilter2D(image, result, kernel1D, new MCvPoint(0, 1));
         }
         catch (Exception e)
         {
            throw e;
         }*/
      }

      [Test]
      public void TestBitmapConstructor()
      {
         #region test byte images
         Image<Bgr, Byte> image1 = new Image<Bgr, byte>(201, 401);
         image1.SetRandUniform(new MCvScalar(), new MCvScalar(255.0, 255.0, 255.0));
         Assert.IsTrue(image1.Equals(new Image<Bgr, byte>(image1.ToBitmap())));
         Assert.IsTrue(image1.Equals(new Image<Bgr, byte>(image1.Bitmap)));

         Image<Gray, Byte> image3 = new Image<Gray, byte>(11, 7);
         image3.SetRandUniform(new MCvScalar(), new MCvScalar(255.0, 255.0, 255.0));
         Assert.IsTrue(image3.Equals(new Image<Gray, byte>(image3.ToBitmap())));
         Assert.IsTrue(image3.Equals(new Image<Gray, byte>(image3.Bitmap)));

         Image<Bgra, Byte> image5 = new Image<Bgra, byte>(201, 401);
         image5.SetRandUniform(new MCvScalar(), new MCvScalar(255.0, 255.0, 255.0, 255.0));
         Assert.IsTrue(image5.Equals(new Image<Bgra, byte>(image5.ToBitmap())));
         Assert.IsTrue(image5.Equals(new Image<Bgra, byte>(image5.Bitmap)));
         #endregion

         #region test single images
         Image<Bgr, Single> image7 = new Image<Bgr, Single>(201, 401);
         image7.SetRandUniform(new MCvScalar(), new MCvScalar(255.0, 255.0, 255.0));
         Bitmap bmp = image7.ToBitmap();
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
         for (int k = 0; k < 1; k++)
         {
            Image<Gray, Byte> modelImage = new Image<Gray, byte>("box.png");
            //Image<Gray, Byte> modelImage = new Image<Gray, byte>("stop.jpg");
            //modelImage = modelImage.Resize(400, 400, true);

            //modelImage._EqualizeHist();

            #region extract features from the object image
            Stopwatch stopwatch = Stopwatch.StartNew();
            MCvSURFParams param1 = new MCvSURFParams(500, false);
            SURFFeature[] modelFeatures = modelImage.ExtractSURF(ref param1);
            SURFTracker tracker = new SURFTracker(modelFeatures);
            stopwatch.Stop();
            Trace.WriteLine(String.Format("Time to extract feature from model: {0} milli-sec", stopwatch.ElapsedMilliseconds));
            #endregion

            //Image<Gray, Byte> observedImage = new Image<Gray, byte>("traffic.jpg");
            Image<Gray, Byte> observedImage = new Image<Gray, byte>("box_in_scene.png");
            //Image<Gray, Byte> observedImage = modelImage.Rotate(45, new Gray(0.0));
            //Image<Gray, Byte> observedImage = new Image<Gray, byte>("left.jpg");
            //image = image.Resize(400, 400, true);

            //observedImage._EqualizeHist();
            #region extract features from the observed image
            stopwatch.Reset(); stopwatch.Start();
            MCvSURFParams param2 = new MCvSURFParams(500, false);
            SURFFeature[] observedFeatures = observedImage.ExtractSURF(ref param2);
            stopwatch.Stop();
            Trace.WriteLine(String.Format("Time to extract feature from image: {0} milli-sec", stopwatch.ElapsedMilliseconds));
            #endregion

            //Merge the object image and the observed image into one big image for display
            Image<Gray, Byte> res = modelImage.ConcateVertical(observedImage); 

            Rectangle rect = modelImage.ROI;
            PointF[] pts = new PointF[] { 
               new PointF(rect.Left, rect.Bottom),
               new PointF(rect.Right, rect.Bottom),
               new PointF(rect.Right, rect.Top),
               new PointF(rect.Left, rect.Top)};

            HomographyMatrix homography;

            stopwatch.Reset(); stopwatch.Start();
            homography = tracker.Detect(observedFeatures, 0.8);
            stopwatch.Stop();
            Trace.WriteLine(String.Format("Time for feature matching: {0} milli-sec", stopwatch.ElapsedMilliseconds));
            if (homography != null)
            {
               PointF[] points = pts.Clone() as PointF[];
               homography.ProjectPoints(points);
               
               for (int i = 0; i < points.Length; i++)
                  points[i].Y += modelImage.Height;
               res.DrawPolyline(Array.ConvertAll<PointF, Point>(points, Point.Round), true, new Gray(255.0), 5);
            }

            stopwatch.Reset(); stopwatch.Start();
            //set the initial region to be the whole image
            using (Image<Gray, Single> priorMask = new Image<Gray, float>(observedImage.Size))
            {
               priorMask.SetValue(1.0);
               homography = tracker.CamShiftTrack(
                  observedFeatures,
                  (RectangleF)observedImage.ROI,
                  priorMask);
            }
            Trace.WriteLine(String.Format("Time for feature tracking: {0} milli-sec", stopwatch.ElapsedMilliseconds));

            if (homography != null) //set the initial tracking window to be the whole image
            {
               PointF[] points = pts.Clone() as PointF[];
               homography.ProjectPoints(points);

               for (int i = 0; i < points.Length; i++)
                  points[i].Y += modelImage.Height;
               res.DrawPolyline(Array.ConvertAll<PointF, Point>(points, Point.Round), true, new Gray(255.0), 5);
            }
         }
      }

      [Test]
      public void TestSnake()
      {
         Image<Gray, Byte> img = new Image<Gray, Byte>(100, 100, new Gray());

         Rectangle rect = new Rectangle(40, 30, 20, 40);
         img.Draw(rect, new Gray(255.0), -1);

         using (MemStorage stor = new MemStorage())
         {
            Seq<Point> pts = new Seq<Point>((int)CvEnum.SEQ_TYPE.CV_SEQ_POLYGON, stor);
            pts.Push(new Point(20, 20));
            pts.Push(new Point(20, 80));
            pts.Push(new Point(80, 80));
            pts.Push(new Point(80, 20));

            Image<Gray, Byte> canny = img.Canny(new Gray(100.0), new Gray(40.0));
            Seq<Point> snake = canny.Snake(pts, 1.0f, 1.0f, 1.0f, new Size(21, 21), new MCvTermCriteria(40, 0.0002), stor);

            img.Draw(pts, new Gray(120), 1);
            img.Draw(snake, new Gray(80), 2);
         }
      }

      [Test]
      public void TestWaterShed()
      {
         Image<Bgr, Byte> image = new Image<Bgr, byte>("Stuff.jpg");
         Image<Gray, Int32> marker = new Image<Gray, Int32>(image.Width, image.Height);
         Rectangle rect = image.ROI;
         marker.Draw(
            new CircleF(
               new PointF(rect.Left + rect.Width / 2.0f, rect.Top + rect.Height / 2.0f),
               (float)(Math.Min(image.Width, image.Height) / 4.0f)),
            new Gray(255),
            0);
         CvInvoke.cvWatershed(image, marker);
      }

      [Test]
      public void TestMatrixDFT()
      {
         //The matrix to be transformed.
         Matrix<float> matB = new Matrix<float>(
            new float[,] { 
            {1.0f/16.0f, 1.0f/16.0f, 1.0f/16.0f}, 
            {1.0f/16.0f, 8.0f/16.0f, 1.0f/16.0f}, 
            {1.0f/16.0f, 1.0f/16.0f, 1.0f/16.0f}});

         Matrix<float> matBDft = new Matrix<float>(
            CvInvoke.cvGetOptimalDFTSize(matB.Rows),
            CvInvoke.cvGetOptimalDFTSize(matB.Cols));
         CvInvoke.cvCopyMakeBorder(matB, matBDft, new Point(0, 0), Emgu.CV.CvEnum.BORDER_TYPE.CONSTANT, new MCvScalar());
         Matrix<float> dftIn = new Matrix<float>(matBDft.Rows, matBDft.Cols, 2);
         CvInvoke.cvMerge(matBDft, IntPtr.Zero, IntPtr.Zero, IntPtr.Zero, dftIn);

         Matrix<float> dftOut = new Matrix<float>(dftIn.Rows, dftIn.Cols, 2);
         //perform the Fourior Transform
         CvInvoke.cvDFT(dftIn, dftOut, Emgu.CV.CvEnum.CV_DXT.CV_DXT_FORWARD, matB.Rows);

         //The real part of the Fourior Transform
         Matrix<float> outReal = new Matrix<float>(matBDft.Size);
         //The imaginary part of the Fourior Transform
         Matrix<float> outIm = new Matrix<float>(matBDft.Size);
         CvInvoke.cvSplit(dftOut, outReal, outIm, IntPtr.Zero, IntPtr.Zero);
      }

      [Test]
      public void TestImageDFT()
      {
         Image<Gray, float> matA = new Image<Gray, float>("stuff.jpg");

         //The matrix to be convoled with matA, a bluring filter
         Matrix<float> matB = new Matrix<float>(
            new float[,] { 
            {1.0f/16.0f, 1.0f/16.0f, 1.0f/16.0f}, 
            {1.0f/16.0f, 8.0f/16.0f, 1.0f/16.0f}, 
            {1.0f/16.0f, 1.0f/16.0f, 1.0f/16.0f}});

         Image<Gray, float> convolvedImage = new Image<Gray, float>(matA.Size + matB.Size - new Size(1, 1));

         Matrix<float> dftA = new Matrix<float>(
            CvInvoke.cvGetOptimalDFTSize(convolvedImage.Rows),
            CvInvoke.cvGetOptimalDFTSize(convolvedImage.Cols));
         matA.CopyTo(dftA.GetSubRect(matA.ROI));

         CvInvoke.cvDFT(dftA, dftA, Emgu.CV.CvEnum.CV_DXT.CV_DXT_FORWARD, matA.Rows);

         Matrix<float> dftB = new Matrix<float>(dftA.Size);
         matB.CopyTo(dftB.GetSubRect(new Rectangle(Point.Empty, matB.Size)));
         CvInvoke.cvDFT(dftB, dftB, Emgu.CV.CvEnum.CV_DXT.CV_DXT_FORWARD, matB.Rows);

         CvInvoke.cvMulSpectrums(dftA, dftB, dftA, Emgu.CV.CvEnum.MUL_SPECTRUMS_TYPE.DEFAULT);
         CvInvoke.cvDFT(dftA, dftA, Emgu.CV.CvEnum.CV_DXT.CV_DXT_INVERSE, convolvedImage.Rows);
         dftA.GetSubRect(new Rectangle(Point.Empty, convolvedImage.Size)).CopyTo(convolvedImage);
      }

      [Test]
      public void TestImageDFT2()
      {
         Image<Gray, float> image = new Image<Gray, float>("stuff.jpg");
         IntPtr complexImage = CvInvoke.cvCreateImage(image.Size, Emgu.CV.CvEnum.IPL_DEPTH.IPL_DEPTH_32F, 2);
         CvInvoke.cvSetImageCOI(complexImage, 1);
         CvInvoke.cvCopy(image, complexImage, IntPtr.Zero);
         CvInvoke.cvSetImageCOI(complexImage, 0);

         Matrix<float> dft = new Matrix<float>(image.Rows, image.Cols, 2);

         CvInvoke.cvDFT(complexImage, dft, Emgu.CV.CvEnum.CV_DXT.CV_DXT_FORWARD, 0);
      }

      [Test]
      public void TestResize()
      {
         Image<Gray, Byte> image = new Image<Gray, byte>(123, 321);
         image.SetRandUniform(new MCvScalar(), new MCvScalar(255));
         image.Resize(512, 512, CvEnum.INTER.CV_INTER_CUBIC);
      }

      [Test]
      public void TestRoi()
      {
         Image<Bgr, Byte> image = new Image<Bgr, byte>(1, 1);
         Rectangle roi = image.ROI;

         Assert.AreEqual(roi.Width, image.Width);
         Assert.AreEqual(roi.Height, image.Height);
      }

      [Test]
      public void TestGetSubRect()
      {
         Image<Bgr, Single> image = new Image<Bgr, float>(200, 100);
         image.SetRandUniform(new MCvScalar(), new MCvScalar(255, 255, 255, 255));
         Rectangle roi = new Rectangle(10, 20, 30, 40);
         Image<Bgr, Single> roi1 = image.Copy(roi);
         Image<Bgr, Single> roi2 = image.GetSubRect(roi);
         Assert.IsTrue(roi1.Equals( roi2 ));
      }

      [Test]
      public void TestGoodFeature()
      {
         using (Image<Bgr, Byte> img = new Image<Bgr, Byte>("stuff.jpg"))
         {
            PointF[][] pts = img.GoodFeaturesToTrack(100, 0.1, 10, 5);
            img.FindCornerSubPix(pts, new Size(5, 5), new Size(-1, -1), new MCvTermCriteria(20, 0.0001));

            foreach (PointF p in pts[0])
               img.Draw(new CircleF(p, 3.0f), new Bgr(255, 0, 0), 1);
         }
      }

      [Test]
      public void TestContour()
      {
         Image<Gray, Byte> img = new Image<Gray, byte>("stuff.jpg");
         img.SmoothGaussian(3);
         img = img.Canny(new Gray(80), new Gray(50));
         Image<Gray, Byte> res = img.CopyBlank();
         res.SetValue(255);

         Contour<Point> contour = img.FindContours();

         while (contour != null)
         {
            Contour<Point> approx = contour.ApproxPoly(contour.Perimeter * 0.05);

            if (approx.Convex && Math.Abs(approx.Area) > 20.0)
            {
               Point[] vertices = approx.ToArray();

               LineSegment2D[] edges = PointCollection.PolyLine(vertices, true);

               res.DrawPolyline(vertices, true, new Gray(200), 1);
            }
            contour = contour.HNext;
         }
      }

      [Test]
      public void TestGamma()
      {
         Image<Bgr, Byte> img = new Image<Bgr, byte>(320, 240);
         img.SetRandNormal(new MCvScalar(120, 120, 120), new MCvScalar(50, 50, 50));
         img._GammaCorrect(0.5);
      }

      [Test]
      public void TestImageIndexer()
      {
         Image<Bgr, Byte> image = new Image<Bgr, byte>(1000, 5000);
         image.SetRandUniform(new MCvScalar(), new MCvScalar(255.0, 255.0, 255.0));
         Stopwatch watch = Stopwatch.StartNew();
         for (int i = 0; i < image.Height; i++)
            for (int j = 0; j < image.Width; j++)
            {
               Bgr color = image[i, j];
            }
         watch.Stop();
         Trace.WriteLine("Time used: " + watch.ElapsedMilliseconds + ".");
         watch = Stopwatch.StartNew();
         for (int i = 0; i < image.Height; i++)
            for (int j = 0; j < image.Width; j++)
               for (int k = 0; k < image.NumberOfChannels; k++)
               {
                  Byte b = image.Data[i, j, k];
               }
         watch.Stop();
         Trace.WriteLine("Time used: " + watch.ElapsedMilliseconds + ".");
      }

      [Test]
      public void TestSetRandome()
      {
         Image<Bgr, Byte> image = new Image<Bgr, byte>(400, 200);
         //image.SetRandUniform(new MCvScalar(), new MCvScalar(255, 255, 255));
         //TODO: Find out why Random normal no longer works for multi channel images
         image.SetRandNormal(new MCvScalar(100, 100, 100), new MCvScalar(20, 20, 20));
      }

      [Test]
      public void TestMSER()
      {
         Image<Gray, Byte> image = new Image<Gray, byte>("stuff.jpg");
         MCvMSERParams param = MCvMSERParams.GetDefaultParameter();
         using (MemStorage storage = new MemStorage())
         {
            Seq<Point>[] mser = image.ExtractMSER(null, ref param, storage);
            {
               foreach(Seq<Point> region in mser)
                  image.Draw(region, new Gray(255.0), 2);
            }
         }
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
