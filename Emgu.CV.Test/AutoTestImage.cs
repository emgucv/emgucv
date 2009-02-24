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
                  System.Drawing.Point location = new System.Drawing.Point(i, j);
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
         img1.ROI = new System.Drawing.Rectangle(10, 1, 50 - 10, 19 - 1);
         img1.SetValue(5.0);

         Image<Gray, Single> img2 = new Image<Gray, float>(50, 20);
         img2.ROI = new System.Drawing.Rectangle(0, 2, 40, 20 - 2);
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
         for (int i = 0; i <= 200; i++)
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
         img.Rotate(90, new Bgr());
         Image<Bgr, Byte> imgRotated = img.Rotate(30, new Bgr(0, 0, 0), false);
         
         //ImageViewer.Show(imgRotated);
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

         //ImageViewer.Show(image);
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

      /// <summary>
      /// A simple class that use two feature trees (postive/negative laplacian) to match SURF features. 
      /// </summary>
      public class SURFMatcher : DisposableObject
      {
         /// <summary>
         /// Features with positive laplacian
         /// </summary>
         private SURFFeature[] _positiveLaplacian;
         /// <summary>
         /// Features with negative laplacian
         /// </summary>
         private SURFFeature[] _negativeLaplacian;
         /// <summary>
         /// Feature Tree which contains features with positive laplacian
         /// </summary>
         private FeatureTree _positiveTree;
         /// <summary>
         /// Feature Tree which contains features with negative laplacian
         /// </summary>
         private FeatureTree _negativeTree;

         /// <summary>
         /// Initiate the SURF matched with the given SURF feature extracted from the model
         /// </summary>
         /// <param name="modelFeatures">The SURF feature extracted from the model</param>
         public SURFMatcher(SURFFeature[] modelFeatures)
         {
            _positiveLaplacian = Array.FindAll<SURFFeature>(modelFeatures, delegate(SURFFeature f) { return f.Point.laplacian >= 0; });
            _negativeLaplacian = Array.FindAll<SURFFeature>(modelFeatures, delegate(SURFFeature f) { return f.Point.laplacian < 0; });
            _positiveTree = new FeatureTree(
               Array.ConvertAll<SURFFeature, Matrix<float>>(
                  _positiveLaplacian,
                  delegate(SURFFeature f) { return f.Descriptor; }));
            _negativeTree = new FeatureTree(
               Array.ConvertAll<SURFFeature, Matrix<float>>(
                  _negativeLaplacian,
                  delegate(SURFFeature f) { return f.Descriptor; }));
         }

         private void SplitSURFByLaplacian(SURFFeature[] features, out List<SURFFeature> positiveLaplacian, out List<SURFFeature> negativeLaplacian)
         {
            positiveLaplacian = new List<SURFFeature>();
            negativeLaplacian = new List<SURFFeature>();
            foreach (SURFFeature f in features)
            {
               if (f.Point.laplacian >= 0)
                  positiveLaplacian.Add(f);
               else
                  negativeLaplacian.Add(f);
            }
         }

         /// <summary>
         /// 
         /// </summary>
         /// <param name="features"></param>
         /// <param name="k"></param>
         /// <param name="emax"></param>
         /// <returns></returns>
         public MatchedSURFFeature[] MatchFeature(SURFFeature[] observedFeatures, int k, int emax)
         {
            List<SURFFeature> positiveLaplacian;
            List<SURFFeature> negativeLaplacian;
            SplitSURFByLaplacian(observedFeatures, out positiveLaplacian, out negativeLaplacian);

            MatchedSURFFeature[] matchedFeatures = MatchFeatureWithModel(positiveLaplacian, _positiveLaplacian, _positiveTree, k, emax);
            MatchedSURFFeature[] negative = MatchFeatureWithModel(negativeLaplacian, _negativeLaplacian, _negativeTree, k, emax);
            int positiveSize = matchedFeatures.Length;
            Array.Resize(ref matchedFeatures, matchedFeatures.Length + negative.Length);
            Array.Copy(negative, 0, matchedFeatures, positiveSize, negative.Length);
            return matchedFeatures;
         }

         private static MatchedSURFFeature[] MatchFeatureWithModel(List<SURFFeature> observedFeatures, SURFFeature[] modelFeatures, FeatureTree modelFeatureTree, int k, int emax)
         {
            if (observedFeatures.Count == 0) return new MatchedSURFFeature[0];

            Matrix<Int32> result1;
            Matrix<double> dist1;
           
            modelFeatureTree.FindFeatures(observedFeatures.ToArray(), out result1, out dist1, k, emax);

            int[,] indexes = result1.Data;
            double[,] distances = dist1.Data;

            MatchedSURFFeature[] res = new MatchedSURFFeature[observedFeatures.Count];
            List<SURFFeature> matchedFeatures = new List<SURFFeature>();
            List<double> matchedDistances = new List<double>();
            for (int i = 0; i < res.Length; i++)
            {
               matchedFeatures.Clear();
               matchedDistances.Clear();
               for (int j = 0; j < k; j++)
               {
                  int index = indexes[i, j];
                  if (index >= 0)
                  {
                     matchedFeatures.Add(modelFeatures[index]);
                     matchedDistances.Add(distances[i, j]);
                  }
               }
               res[i].ModelFeatures = matchedFeatures.ToArray();
               res[i].ObservedFeature = observedFeatures[i];
               res[i].Distances = matchedDistances.ToArray();
            }
            result1.Dispose();
            dist1.Dispose();
            return res;
         }

         protected override void DisposeObject()
         {
            _positiveTree.Dispose();
            _negativeTree.Dispose();
         }
      }

      public struct MatchedSURFFeature
      {
         /// <summary>
         /// The observed feature
         /// </summary>
         public SURFFeature ObservedFeature;
         /// <summary>
         /// The matched model features
         /// </summary>
         public SURFFeature[] ModelFeatures;
         /// <summary>
         /// The distances to the matched model features
         /// </summary>
         public double[] Distances;

         public MatchedSURFFeature(SURFFeature observedFeature, SURFFeature[] modelFeatures, double[] dist)
         {
            ObservedFeature = observedFeature;
            ModelFeatures = modelFeatures;
            Distances = dist;
         }
      }

      /// <summary>
      /// This class use SURF and CamShift to track object
      /// </summary>
      public class SURFTracker
      {
         private SURFMatcher _matcher;
         private Image<Gray, Byte> _model;
         private Matrix<double> _originalCornerCoordinate;
         private Matrix<double> _homographyMatrix;

         public SURFTracker(Image<Gray, Byte> modelImage, ref MCvSURFParams param)
         {
            _model = modelImage;
            SURFFeature[] modelFeatures = _model.ExtractSURF(ref param);
            _matcher = new SURFMatcher(modelFeatures);

            System.Drawing.Rectangle rect = modelImage.ROI;
            _originalCornerCoordinate = new Matrix<double>(new double[,] 
            {{  rect.Left, rect.Bottom, 1.0},
               { rect.Right, rect.Bottom, 1.0},
               { rect.Right, rect.Top, 1.0},
               { rect.Left, rect.Top, 1.0}});
         }

         private Matrix<float> ProjectedCoordinatesFromHomographyMatrix()
         {
            Debug.Assert(_homographyMatrix != null, "Homography matrix must not be null.");

            using (Matrix<double> destCornerCoordinate = new Matrix<double>(_originalCornerCoordinate.Rows, 3))
            {
               Matrix<float> destCornerCoordinate2D = new Matrix<float>(_originalCornerCoordinate.Rows, 1, 2);
            
               CvInvoke.cvGEMM(_originalCornerCoordinate, _homographyMatrix, 1.0, IntPtr.Zero, 0.0, destCornerCoordinate, Emgu.CV.CvEnum.GEMM_TYPE.CV_GEMM_B_T);

               CvInvoke.cvConvertPointsHomogeneous(destCornerCoordinate, destCornerCoordinate2D);

               return destCornerCoordinate2D;
            }
         }

         /// <summary>
         /// Get a bounding rectangle of the tracked image on the obsereved image.
         /// </summary>
         /// <returns>A bounding rectangle of the tracked image on the obsereved image</returns>
         public MCvBox2D GetBoundingRectangle()
         {
            if (_homographyMatrix == null)
               return new MCvBox2D();

            using (Matrix<float> destCornerCoordinate2D = ProjectedCoordinatesFromHomographyMatrix())
            {
               return CvInvoke.cvMinAreaRect2(destCornerCoordinate2D, IntPtr.Zero);
            }
         }

         /// <summary>
         /// Get the projection of the four corners of the tracked image on the observed Image.
         /// </summary>
         /// <returns>The projection of the four corners of the tracked image on the observed Image</returns>
         public Point[] GetBoundingPoints()
         {
            if (_homographyMatrix == null) return null;

            using (Matrix<float> destCornerCoordinate2D = ProjectedCoordinatesFromHomographyMatrix())
            {
               float[,] data = destCornerCoordinate2D.Data;
               Point[] res = new Point[destCornerCoordinate2D.Rows];
               for (int i = 0; i < res.Length; i++)
               {
                  res[i] = new Point((int)data[i, 0], (int)data[i, 1]);
               }
               return res;
            }
         }

         /// <summary>
         /// Get the homography projection matrix from the model ROI to the Observed Image
         /// </summary>
         public Matrix<double> Homography
         {
            get
            {
               return _homographyMatrix;
            }
         }

         public bool CamShiftTrack(Size observedImageSize, SURFFeature[] observedFeatures, MCvBox2D initRegion)
         {
            double matchDistanceRatio = 0.8;

            using (Image<Gray, Single> matchMask = new Image<Gray, Single>(observedImageSize))
            {
               #region get the list of matched point on the observed image
               Single[, ,] matchMaskData = matchMask.Data;

               MatchedSURFFeature[] matchedFeature = _matcher.MatchFeature(observedFeatures, 2, 20);
               double ratio;
               foreach (MatchedSURFFeature f in matchedFeature)
               {
                  if (f.Distances.Length == 1 ||
                     ( ratio = f.Distances[0] / f.Distances[1]) <= matchDistanceRatio ||
                     ratio >= (1.0 / matchDistanceRatio))
                  {
                     PointF p = f.ObservedFeature.Point.pt;
                     matchMaskData[(int)p.Y, (int)p.X, 0] = 1.0f / (float)f.Distances[0];
                  }
               }
               #endregion

               Rectangle startRegion;
               if (initRegion.Equals(MCvBox2D.Empty))
                  startRegion = matchMask.ROI;
               else
               {
                  startRegion = PointCollection.BoundingRectangle(initRegion.GetVertices());
                  if (startRegion.IntersectsWith(matchMask.ROI))
                     startRegion.Intersect(matchMask.ROI);
               }

               MCvConnectedComp comp;
               MCvBox2D currentRegion;
               //Updates the current location
               CvInvoke.cvCamShift(matchMask.Ptr, startRegion, new MCvTermCriteria(10, 1.0e-8), out comp, out currentRegion);

               #region find the SURF features that belongs to the current Region
               MatchedSURFFeature[] featuesInCurrentRegion;
               using (MemStorage stor = new MemStorage())
               {
                  Contour<System.Drawing.PointF> contour = new Contour<PointF>(stor);
                  contour.PushMulti(currentRegion.GetVertices(), Emgu.CV.CvEnum.BACK_OR_FRONT.BACK);

                  CvInvoke.cvBoundingRect(contour, 1); //this is required before calling the InContour function

                  featuesInCurrentRegion = Array.FindAll(matchedFeature,
                     delegate(MatchedSURFFeature f)
                     { return contour.InContour(f.ObservedFeature.Point.pt) >= 0; });
               }
               #endregion

               return Detect(featuesInCurrentRegion, 0.8);
            }
         }

         private bool Detect(MatchedSURFFeature[] matchedFeature, double matchDistanceRatio)
         {
            List<PointF> modelPoints = new List<PointF>();
            List<PointF> observePoints = new List<PointF>();

            double ratio;
            foreach (MatchedSURFFeature f in matchedFeature)
            {
               if (f.Distances.Length == 1  //if this is the only match
                  || (ratio = f.Distances[0] / f.Distances[1]) <= matchDistanceRatio) //or the first model feature is a good match
               {  
                  modelPoints.Add(f.ModelFeatures[0].Point.pt);
                  observePoints.Add(f.ObservedFeature.Point.pt);
               }
               else if (ratio >= (1.0 / matchDistanceRatio)) //the second model feature is a good match
               {
                  modelPoints.Add(f.ModelFeatures[1].Point.pt);
                  observePoints.Add(f.ObservedFeature.Point.pt);
               }
            }

            //At leaset 7 points is required.
            if (observePoints.Count < 7)
            {
               _homographyMatrix = null;
               return false;
            }

            _homographyMatrix = CameraCalibration.FindHomography(
               modelPoints.ToArray(), //points on the object image
               observePoints.ToArray(), //points on the observed image
               CvEnum.HOMOGRAPHY_METHOD.RANSAC,
               3);
            return true;
         }

         /// <summary>
         /// Check if the there is a match of this SURF object on the image
         /// </summary>
         /// <param name="imageFeatures">The SURF feature extracted from the image</param>
         /// <returns>True if such an image is detected, false otherwise</returns>
         public bool Detect(SURFFeature[] imageFeatures)
         {
            MatchedSURFFeature[] matchedFeature = _matcher.MatchFeature(imageFeatures, 2, 20);
            return Detect(matchedFeature, 0.8);
         }
      }

      [Test]
      public void TestSURF()
      {
         for (int k = 0; k < 1; k++)
         {
            Image<Gray, Byte> modelImage = new Image<Gray, byte>("box.png");
            //modelImage = modelImage.Resize(400, 400, true);

            #region extract features from the object image
            Stopwatch stopwatch = Stopwatch.StartNew();
            MCvSURFParams param1 = new MCvSURFParams(500, false);
            SURFTracker tracker = new SURFTracker(modelImage, ref param1);
            stopwatch.Stop();
            Trace.WriteLine(String.Format("Time to extract feature from model: {0} milli-sec", stopwatch.ElapsedMilliseconds));
            #endregion

            Image<Gray, Byte> observedImage = new Image<Gray, byte>("box_in_scene.png");
            //Image<Gray, Byte> observedImage = new Image<Gray, byte>("left.jpg");
            //image = image.Resize(400, 400, true);
            #region extract features from the observed image
            stopwatch.Reset(); stopwatch.Start();
            MCvSURFParams param2 = new MCvSURFParams(500, false);
            SURFFeature[] observedFeatures = observedImage.ExtractSURF(ref param2);
            stopwatch.Stop();
            Trace.WriteLine(String.Format("Time to extract feature from image: {0} milli-sec", stopwatch.ElapsedMilliseconds));
            #endregion

            #region Merge the object image and the observed image into one big image for display
            Image<Gray, Byte> res = new Image<Gray, byte>(Math.Max(modelImage.Width, observedImage.Width), modelImage.Height + observedImage.Height);
            res.ROI = modelImage.ROI; 
            modelImage.Copy(res, null);
            res.ROI = new System.Drawing.Rectangle(0, modelImage.Height, observedImage.Width, observedImage.Height);
            observedImage.Copy(res, null);
            res.ROI = Rectangle.Empty;
            #endregion

            stopwatch.Reset(); stopwatch.Start();
            tracker.Detect(observedFeatures);
            Point[] points = tracker.GetBoundingPoints();
            stopwatch.Stop();
            Trace.WriteLine(String.Format("Time for feature matching: {0} milli-sec", stopwatch.ElapsedMilliseconds));
            for (int i = 0; i < points.Length; i++)
               points[i].Y += modelImage.Height;
            res.DrawPolyline(points, true, new Gray(255.0), 5);

            stopwatch.Reset(); stopwatch.Start();
            //set the initial region to be the whole image

            tracker.CamShiftTrack(
               observedImage.Size, //size of the image
               observedFeatures, 
               (RectangleF)observedImage.ROI); //set the initial tracking window to be the whole image

            points = tracker.GetBoundingPoints();
            Trace.WriteLine(String.Format("Time for feature tracking: {0} milli-sec", stopwatch.ElapsedMilliseconds));

            for (int i = 0; i < points.Length; i++)
               points[i].Y += modelImage.Height;
            res.DrawPolyline(points, true, new Gray(255.0), 5);
            
            //ImageViewer.Show(res.Resize(200, 200, true));
         }
      }

      [Test]
      public void TestSnake()
      {
         Image<Gray, Byte> img = new Image<Gray, Byte>(100, 100, new Gray());

         System.Drawing.Rectangle rect = new Rectangle(40, 30, 20, 40);
         img.Draw(rect, new Gray(255.0), -1);

         using (MemStorage stor = new MemStorage())
         {
            Seq<System.Drawing.Point> pts = new Seq<System.Drawing.Point>((int)CvEnum.SEQ_TYPE.CV_SEQ_POLYGON, stor);
            pts.Push(new System.Drawing.Point(20, 20));
            pts.Push(new System.Drawing.Point(20, 80));
            pts.Push(new System.Drawing.Point(80, 80));
            pts.Push(new System.Drawing.Point(80, 20));

            Image<Gray, Byte> canny = img.Canny(new Gray(100.0), new Gray(40.0));
            Seq<System.Drawing.Point> snake = canny.Snake(pts, 1.0f, 1.0f, 1.0f, new System.Drawing.Size(21, 21), new MCvTermCriteria(40, 0.0002), stor);

            img.Draw(pts, new Gray(120), 1);
            img.Draw(snake, new Gray(80), 2);

            //ImageViewer.Show(img));
         }
      }

      [Test]
      public void TestWaterShed()
      {
         Image<Bgr, Byte> image = new Image<Bgr, byte>("Stuff.jpg");
         Image<Gray, Int32> marker = new Image<Gray, Int32>(image.Width, image.Height);
         System.Drawing.Rectangle rect = image.ROI;
         marker.Draw(
            new CircleF(
               new PointF(rect.Left + rect.Width / 2.0f, rect.Top + rect.Height / 2.0f), 
               (float) (Math.Min(image.Width, image.Height) / 4.0f)),
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
         CvInvoke.cvCopyMakeBorder(matB, matBDft, new System.Drawing.Point(0, 0), Emgu.CV.CvEnum.BORDER_TYPE.CONSTANT, new MCvScalar());
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
         matB.CopyTo(dftB.GetSubRect(new System.Drawing.Rectangle(Point.Empty, matB.Size)));
         CvInvoke.cvDFT(dftB, dftB, Emgu.CV.CvEnum.CV_DXT.CV_DXT_FORWARD, matB.Rows);

         CvInvoke.cvMulSpectrums(dftA, dftB, dftA, Emgu.CV.CvEnum.MUL_SPECTRUMS_TYPE.DEFAULT);
         CvInvoke.cvDFT(dftA, dftA, Emgu.CV.CvEnum.CV_DXT.CV_DXT_INVERSE, convolvedImage.Rows);
         dftA.GetSubRect(new System.Drawing.Rectangle(Point.Empty, convolvedImage.Size)).CopyTo(convolvedImage);

         //ImageViewer.Show(convolvedImage);
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
            img.FindCornerSubPix(pts, new System.Drawing.Size(5, 5), new System.Drawing.Size(-1, -1), new MCvTermCriteria(20, 0.0001));

            foreach (PointF p in pts[0])
               img.Draw(new CircleF(p, 3.0f), new Bgr(255, 0, 0), 1);
            //ImageViewer.Show(img);
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

         Contour<System.Drawing.Point> contour = img.FindContours();

         while (contour != null)
         {
            Contour<System.Drawing.Point> approx = contour.ApproxPoly(contour.Perimeter * 0.05);

            if (approx.Convex && Math.Abs(approx.Area) > 20.0)
            {
               System.Drawing.Point[] vertices = approx.ToArray();

               LineSegment2D[] edges = PointCollection.PolyLine(vertices, true);

               res.DrawPolyline(vertices, true, new Gray(200), 1);
            }
            contour = contour.HNext;
         }
         //ImageViewer.Show(res);
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
