//----------------------------------------------------------------------------
//  Copyright (C) 2004-2017 by EMGU Corporation. All rights reserved.       
//----------------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;
using System.Text;
using System.Threading;
using System.Xml;
using System.Xml.Linq;
using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Features2D;
using Emgu.CV.Structure;
#if !(__IOS__ || NETFX_CORE || __ANDROID__)
using Emgu.CV.UI;
#endif
using Emgu.CV.Util;
using Emgu.Util;

#if VS_TEST
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TestAttribute = Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute;
using TestFixture = Microsoft.VisualStudio.TestTools.UnitTesting.TestClassAttribute;
#elif NETFX_CORE
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
using TestAttribute = Microsoft.VisualStudio.TestPlatform.UnitTestFramework.TestMethodAttribute;
using TestFixture = Microsoft.VisualStudio.TestPlatform.UnitTestFramework.TestClassAttribute;
using Trace = System.Diagnostics.Debug;
using Windows.UI.Xaml.Media.Imaging;
using System.Runtime.InteropServices.WindowsRuntime;
#else
using NUnit.Framework;
#endif

namespace Emgu.CV.Test
{

    [TestFixture]
    public class AutoTestImage
    {
        [TestAttribute]
        public void TestOpenCLSetGet()
        {
            CvInvoke.UseOpenCL = false;
            EmguAssert.IsTrue(CvInvoke.UseOpenCL == false);
        }

        public static void TestOpenCL(Action test)
        {
            Trace.WriteLine("Testing without OpenCL");

            CvInvoke.UseOpenCL = false;
            test();
            if (CvInvoke.HaveOpenCL)
            {
                CvInvoke.UseOpenCL = true;
                Trace.WriteLine("Testing with OpenCL");
                test();
                CvInvoke.OclFinish();
            }
        }

        [TestAttribute]
        public void TestAccumulateWeighted()
        {
            int startValue = 50;
            Image<Gray, Single> img1 = new Image<Gray, float>(100, 40, new Gray(100));
            Image<Gray, Single> acc = new Image<Gray, float>(100, 40, new Gray(startValue));
            //IImage img = img2;
            //img1.AccumulateWeighted(acc, 0.5);
            CvInvoke.AccumulateWeighted(img1, acc, 0.3, null);
            TestOpenCL(delegate
                     {
                         UMat src = img1.ToUMat();
                         UMat result = new UMat(img1.Rows, img1.Cols, CvEnum.DepthType.Cv32F, 1);

                         result.SetTo(new MCvScalar(startValue), null);
                      //IImage img = img2;
                      //img1.AccumulateWeighted(result, 0.5);
                      CvInvoke.AccumulateWeighted(src, result, 0.3, null);
                         Image<Gray, Single> tmp = new Image<Gray, float>(img1.Size);
                         result.CopyTo(tmp, null);
                         CvInvoke.AbsDiff(acc, result, result);
                         int nonZeroCount = CvInvoke.CountNonZero(result);
                         EmguAssert.IsTrue(nonZeroCount == 0);
                     });
        }

        [TestAttribute]
        public void TestAddWeighted()
        {
            UMat img1 = new UMat(320, 480, DepthType.Cv8U, 1);
            UMat img2 = new UMat(320, 480, DepthType.Cv8U, 1);

            UMat result = new UMat();
            CvInvoke.AddWeighted(img1, 0.2, img2, 0.3, 0, result);
        }

        [TestAttribute]
        public void TestSobelScharr()
        {
            Mat img = EmguAssert.LoadMat("lena.jpg");
            Mat result = new Mat();
            CvInvoke.Sobel(img, result, CvEnum.DepthType.Cv8U, 1, 0, -1, 1.0);
            TestOpenCL(delegate
                     {
                         UMat uresult = new UMat();
                         using (UMat um = img.GetUMat(AccessType.ReadWrite))
                         {
                             Stopwatch watch = Stopwatch.StartNew();
                             CvInvoke.Sobel(img, uresult, CvEnum.DepthType.Cv8U, 1, 0, -1, 1.0, 0.0, CvEnum.BorderType.Default);
                             watch.Stop();
                             Trace.WriteLine(String.Format("Sobel completed in {0} milliseconds. (OpenCL: {1})", watch.ElapsedMilliseconds, CvInvoke.UseOpenCL));
                             uresult.CopyTo(result, null);
                         }
                      //Emgu.CV.UI.ImageViewer.Show(result);
                  });
        }

        [TestAttribute]
        public void TestSetValue()
        {
            Image<Bgr, Single> img1 = new Image<Bgr, float>(50, 20, new Bgr(8.0, 1.0, 2.0));
            for (int i = 0; i < img1.Width; i++)
                for (int j = 0; j < img1.Height; j++)
                {
                    Bgr c = img1[j, i];
                    EmguAssert.IsTrue(c.Equals(new Bgr(8.0, 1.0, 2.0)));
                }
        }

        [TestAttribute]
        public void TestIplImageSize()
        {
            IntPtr img = CvInvoke.cvCreateImageHeader(new Size(10, 10), IplDepth.IplDepth_8U, 1);
            MIplImage iplImg = (MIplImage)Marshal.PtrToStructure(img, typeof(MIplImage));

            CvInvoke.cvReleaseImageHeader(ref img);
            int sizeMIplImage = Marshal.SizeOf(typeof(MIplImage));
            CvStructSizes sizes = CvInvoke.GetCvStructSizes();
            int sizeIplImage = sizes.IplImage;
            EmguAssert.IsTrue(sizeMIplImage == iplImg.NSize);
            EmguAssert.IsTrue(sizeMIplImage == sizeIplImage);
        }

        [TestAttribute]
        public void TestImageSize()
        {
            Image<Bgr, Byte> img = new Image<Bgr, byte>(300, 200);
            Size s = img.Size;
            int tmp = s.Width + s.Height;
        }

        [TestAttribute]
        public void TestMinMax()
        {
            Image<Gray, Byte> img1 = new Image<Gray, Byte>(50, 60);
            System.Random r = new Random();

            using (Image<Gray, Byte> img2 = img1.Convert<Byte>(delegate (Byte f) { return (Byte)r.Next(255); }))
            {
                img2._Max(120.0);
                for (int i = 0; i < img2.Width; i++)
                    for (int j = 0; j < img2.Height; j++)
                        EmguAssert.IsTrue(img2[j, i].Intensity >= 120.0);
            }

            using (Image<Gray, Byte> img2 = img1.Convert<Byte>(delegate (Byte f) { return (Byte)r.Next(255); }))
            {
                img2._Min(120.0);
                for (int i = 0; i < img2.Width; i++)
                    for (int j = 0; j < img2.Height; j++)
                        EmguAssert.IsTrue(120.0 >= img2[j, i].Intensity);
            }

            using (Image<Gray, Byte> img2 = img1.Convert<Byte>(delegate (Byte f) { return (Byte)r.Next(255); }))
            using (Image<Gray, Byte> img3 = img1.Convert<Byte>(delegate (Byte f) { return (Byte)r.Next(255); }))
            using (Image<Gray, Byte> img4 = img2.Max(img3))
            {
                for (int i = 0; i < img2.Width; i++)
                    for (int j = 0; j < img2.Height; j++)
                    {
                        Point location = new Point(i, j);
                        EmguAssert.IsTrue(img4[location].Intensity >= img2[location].Intensity);
                        EmguAssert.IsTrue(img4[j, i].Intensity >= img3[j, i].Intensity);
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

        [TestAttribute]
        public void TestAvgSdv()
        {
            Image<Gray, Single> img1 = new Image<Gray, float>(50, 20);
            img1.SetRandNormal(new MCvScalar(100), new MCvScalar(30));
            Gray mean;
            MCvScalar std;
            img1.AvgSdv(out mean, out std);
        }

        [TestAttribute]
        public void TestBgrFloat()
        {
            //String fileName = "lena.jpg";
            //EmguAssert.IsTrue(File.Exists(fileName), String.Format("The specific file {0} doesn't exist", fileName));
            Image<Bgr, float> img = EmguAssert.LoadImage<Bgr, float>("lena.jpg");
            Size s = img.Size;
        }

        [TestAttribute]
        public void TestGenericOperation()
        {
            Image<Gray, Single> img1 = new Image<Gray, float>(50, 20);
            img1.ROI = new Rectangle(10, 1, 50 - 10, 19 - 1);
            img1.SetValue(5.0);

            Image<Gray, Single> img2 = new Image<Gray, float>(50, 20);
            img2.ROI = new Rectangle(0, 2, 40, 20 - 2);
            img2.SetValue(new Gray(2.0));

            EmguAssert.IsTrue(img1.Width == img2.Width);
            EmguAssert.IsTrue(img1.Height == img2.Height);

            Stopwatch watch = Stopwatch.StartNew();
            Image<Gray, Single> img3 = img1.Add(img2);
            long cvAddTime = watch.ElapsedMilliseconds;

            watch.Reset();
            watch.Start();
            Image<Gray, Single> img4 = img1.Convert<Single, Single>(img2, delegate (Single v1, Single v2)
            {
                return v1 + v2;
            });
            long genericAddTime = watch.ElapsedMilliseconds;

            Image<Gray, Single> img5 = img3.AbsDiff(img4);

            watch.Reset();
            watch.Start();
            double sum1 = img5.GetSum().Intensity;
            long cvSumTime = watch.ElapsedMilliseconds;

            watch.Reset();
            watch.Start();
            Single sum2 = 0.0f;
            img5.Action(delegate (Single v)
            {
                sum2 += v;
            });
            long genericSumTime = watch.ElapsedMilliseconds;

            EmguAssert.WriteLine(String.Format("CV Add     : {0} milliseconds", cvAddTime));
            EmguAssert.WriteLine(String.Format("Generic Add: {0} milliseconds", genericAddTime));
            EmguAssert.WriteLine(String.Format("CV Sum     : {0} milliseconds", cvSumTime));
            EmguAssert.WriteLine(String.Format("Generic Sum: {0} milliseconds", genericSumTime));
            EmguAssert.WriteLine(String.Format("Abs Diff = {0}", sum1));
            EmguAssert.WriteLine(String.Format("Abs Diff = {0}", sum2));
            EmguAssert.IsTrue(sum1 == sum2);

            img3.Dispose();
            img4.Dispose();
            img5.Dispose();

            DateTime t1 = DateTime.Now;
            img3 = img1.Mul(2.0);
            DateTime t2 = DateTime.Now;
            img4 = img1.Convert<Single>(delegate (Single v1)
            {
                return v1 * 2.0f;
            });
            DateTime t3 = DateTime.Now;

            /*
            ts1 = t2.Subtract(t1);
            ts2 = t3.Subtract(t2);
            Trace.WriteLine(String.Format("CV Mul     : {0} milliseconds", ts1.TotalMilliseconds));
            Trace.WriteLine(String.Format("Generic Mul: {0} milliseconds", ts2.TotalMilliseconds));
            */

            EmguAssert.IsTrue(img3.Equals(img4));
            img3.Dispose();
            img4.Dispose();

            t1 = DateTime.Now;
            img3 = img1.Add(img1);
            img4 = img3.Add(img1);
            t2 = DateTime.Now;
            img5 = img1.Convert<Single, Single, Single>(img1, img1, delegate (Single v1, Single v2, Single v3)
            {
                return v1 + v2 + v3;
            });
            t3 = DateTime.Now;

            /*
            ts1 = t2.Subtract(t1);
            ts2 = t3.Subtract(t2);
            Trace.WriteLine(String.Format("CV Sum (3 images)     : {0} milliseconds", ts1.TotalMilliseconds));
            Trace.WriteLine(String.Format("Generic Sum (3 images): {0} milliseconds", ts2.TotalMilliseconds));
            */
            EmguAssert.IsTrue(img5.Equals(img4));
            img3.Dispose();
            img4.Dispose();
            img5.Dispose();

            img1.Dispose();
            img2.Dispose();

            Image<Gray, Byte> gimg1 = new Image<Gray, Byte>(400, 300, new Gray(30));
            Image<Gray, Byte> gimg2 = gimg1.Convert<Byte>(delegate (Byte b)
            {
                return (Byte)(255 - b);
            });
            gimg1.Dispose();
            gimg2.Dispose();
        }

        [TestAttribute]
        public void TestConvertDepth()
        {
            Image<Gray, Byte> img1 = new Image<Gray, byte>(100, 100, new Gray(10.0));
            img1.SetRandUniform(new MCvScalar(0, 0, 0), new MCvScalar(255, 255, 255));
            Image<Gray, Single> img2 = img1.ConvertScale<Single>(2.0, 0.0);
            Image<Gray, Byte> img3 = img2.ConvertScale<Byte>(0.5, 0.0);
            EmguAssert.IsTrue(img3.Equals(img1));

            Image<Gray, Double> img4 = img1.Convert<Gray, Double>();
            Image<Gray, Byte> img5 = img4.Convert<Gray, Byte>();
            EmguAssert.IsTrue(img5.Equals(img1));
        }

#if !WINDOWS_PHONE_APP
        [TestAttribute]
        public void TestMemory()
        {
            for (int i = 0; i <= 100; i++)
            {
                Image<Bgr, Single> img = new Image<Bgr, Single>(1000, 1000);
            }
        }
#endif

        [TestAttribute]
        public void TestConversion()
        {
            Image<Bgr, Single> img1 = new Image<Bgr, Single>(100, 100);
            img1.SetRandUniform(new MCvScalar(), new MCvScalar(255.0, 255.0, 255.0));

            Image<Xyz, Single> img2 = img1.Convert<Xyz, Single>();

            Image<Gray, Byte> img3 = img1.Convert<Gray, Byte>();

            Image<Bgra, Byte> bgraImg = new Image<Bgra, byte>(640, 480);
            bgraImg.SetRandNormal(new MCvScalar(100, 100, 100, 100), new MCvScalar(50, 50, 50, 50) );
            Mat yuv = new Mat();
            CvInvoke.CvtColor(bgraImg, yuv, ColorConversion.Bgra2YuvI420);
        }

        [TestAttribute]
        public void TestGenericSetColor()
        {
            Image<Bgr, Byte> img1 = new Image<Bgr, Byte>(20, 40, new Bgr());

            int flag = 0;

            Image<Bgr, Byte> img2 = img1.Convert<Byte>(
                delegate (Byte b)
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
               delegate (Byte byteFromImg, Byte byteFromMask)
               {
                   return byteFromMask > (Byte)120 ? byteFromImg : (Byte)0;
               });
            watch.Stop();
            EmguAssert.WriteLine(String.Format("Time used: {0} milliseconds", watch.ElapsedMilliseconds));

            EmguAssert.IsTrue(img1.Equals(img2));
        }

#if !NETFX_CORE
        public static byte[] GetBytesFromFile(string fullFilePath)
        {
            using (FileStream fs = File.OpenRead(fullFilePath))
            {
                byte[] bytes = new byte[fs.Length];
                fs.Read(bytes, 0, Convert.ToInt32(fs.Length));
                return bytes;
            }
        }

        [TestAttribute]
        public void TestFromJpegData()
        {
            Byte[] data = GetBytesFromFile(EmguAssert.GetFile("lena.jpg"));
            Mat imgJpg = new Mat();
            CvInvoke.Imdecode(data, ImreadModes.Color, imgJpg);

            //Emgu.CV.UI.ImageViewer.Show(imgJpg);

            Mat imgPng = new Mat();
            data = GetBytesFromFile(EmguAssert.GetFile("pedestrian.png"));
            CvInvoke.Imdecode(data, ImreadModes.Color, imgPng);
            //Emgu.CV.UI.ImageViewer.Show(imgPng);
        }

        [TestAttribute]
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
                    Image<Bgr, Byte> img2 = (Image<Bgr, Byte>)o;
                    EmguAssert.IsTrue(img.Equals(img2));
                }
            }
        }

        [TestAttribute]
        public void TestRuntimeSerializeWithROI()
        {
            Image<Bgr, Byte> img = new Image<Bgr, byte>(100, 80);

            using (MemoryStream ms = new MemoryStream())
            {
                img.SetRandNormal(new MCvScalar(100, 100, 100), new MCvScalar(50, 50, 50));
                img.SerializationCompressionRatio = 9;
                img.ROI = new Rectangle(10, 10, 20, 30);

                System.Runtime.Serialization.Formatters.Binary.BinaryFormatter
                    formatter = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
                formatter.Serialize(ms, img);
                Byte[] bytes = ms.GetBuffer();

                using (MemoryStream ms2 = new MemoryStream(bytes))
                {
                    Object o = formatter.Deserialize(ms2);
                    Image<Bgr, Byte> img2 = (Image<Bgr, Byte>)o;
                    EmguAssert.IsTrue(img.Equals(img2));
                }
            }
        }
#endif

        [TestAttribute]
        public void TestSampleLine()
        {
            Image<Bgr, Byte> img = new Image<Bgr, byte>(101, 133);
            img.SetRandUniform(new MCvScalar(), new MCvScalar(255, 255, 255));

            Byte[,] buffer = img.Sample(new LineSegment2D(new Point(0, 0), new Point(0, 100)));
            for (int i = 0; i < 100; i++)
                EmguAssert.IsTrue(img[i, 0].Equals(new Bgr(buffer[i, 0], buffer[i, 1], buffer[i, 2])));

            buffer = img.Sample(new LineSegment2D(new Point(0, 0), new Point(100, 100)), Emgu.CV.CvEnum.Connectivity.FourConnected);
        }

        [TestAttribute]
        public void TestGetSize()
        {
            Image<Bgr, Byte> img = new Image<Bgr, byte>(10, 10, new Bgr(255, 255, 255));
            Size size = img.Size;
            EmguAssert.AreEqual(size, new Size(10, 10));
        }

#if !WINDOWS_PHONE_APP
        [TestAttribute]
        public void TestXmlSerialize()
        {
            Image<Bgr, Byte> img = new Image<Bgr, byte>(100, 80);

            img.SetRandNormal(new MCvScalar(100, 100, 100), new MCvScalar(50, 50, 50));
            img.SerializationCompressionRatio = 9;
            XDocument doc1 = Toolbox.XmlSerialize<Image<Bgr, Byte>>(img);
            String str = doc1.ToString();
            Image<Bgr, Byte> img2 = Toolbox.XmlDeserialize<Image<Bgr, Byte>>(doc1);
            EmguAssert.IsTrue(img.Equals(img2));

            img.SerializationCompressionRatio = 9;
            XDocument doc2 = Toolbox.XmlSerialize<Image<Bgr, Byte>>(img);
            Image<Bgr, Byte> img3 = Toolbox.XmlDeserialize<Image<Bgr, Byte>>(doc2);
            EmguAssert.IsTrue(img.Equals(img3));

            XDocument doc3 = XDocument.Parse(str);

            Image<Bgr, Byte> img4 = Toolbox.XmlDeserialize<Image<Bgr, Byte>>(doc3);
            EmguAssert.IsTrue(img.Equals(img4));

        }
#endif

        [TestAttribute]
        public void TestRotation()
        {
            Image<Bgr, Byte> img = new Image<Bgr, byte>(100, 80);

            img.SetRandNormal(new MCvScalar(100, 100, 100), new MCvScalar(50, 50, 50));

            Image<Bgr, Byte> imgRotated = img.Rotate(90, new Bgr(), false);
            EmguAssert.AreEqual(img.Width, imgRotated.Height);
            EmguAssert.AreEqual(img.Height, imgRotated.Width);
            imgRotated = img.Rotate(30, new Bgr(255, 255, 255), false);
            //ImageViewer.Show(imgRotated);
        }

        [TestAttribute]
        public void TestRotationSpeed()
        {
            Image<Bgr, Byte> img = new Image<Bgr, byte>(1024, 720);

            img.SetRandNormal(new MCvScalar(100, 100, 100), new MCvScalar(50, 50, 50));

            Stopwatch watch = Stopwatch.StartNew();
            Image<Bgr, Byte> imgRotated = img.Rotate(90, new Bgr(), false);
            watch.Stop();
            Trace.WriteLine(String.Format("Rotation time (wrap affine): {0}", watch.ElapsedMilliseconds));
            EmguAssert.AreEqual(img.Width, imgRotated.Height);
            EmguAssert.AreEqual(img.Height, imgRotated.Width);

            watch.Reset();
            watch.Start();
            Image<Bgr, Byte> imgRotated2 = new Image<Bgr, byte>(img.Height, img.Width);

            CvInvoke.Transpose(img, imgRotated2);
            CvInvoke.Flip(imgRotated2, imgRotated2, FlipType.Horizontal);
            watch.Stop();
            Trace.WriteLine(String.Format("Rotation time (transpose & flip): {0}", watch.ElapsedMilliseconds));
            EmguAssert.IsTrue(imgRotated.Equals(imgRotated2));
        }

        [TestAttribute]
        public void TestCascadeClassifierFaceDetect()
        {
            Image<Gray, Byte> image = EmguAssert.LoadImage<Gray, byte>("lena.jpg");
            //using (HaarCascade cascade = new HaarCascade("eye_12.xml"))
            using (CascadeClassifier cascade = new CascadeClassifier(EmguAssert.GetFile("haarcascade_eye.xml")))
            //using (HaarCascade cascade = new HaarCascade("haarcascade_frontalface_alt2.xml"))
            {
                Rectangle[] objects = cascade.DetectMultiScale(image, 1.05, 0, new Size(10, 10), Size.Empty);
                foreach (Rectangle obj in objects)
                    image.Draw(obj, new Gray(0.0), 1);
            }
            TestOpenCL(delegate
                     {
                      //using (HaarCascade cascade = new HaarCascade("eye_12.xml"))
                      using (UMat um = image.ToUMat())
                         using (CascadeClassifier cascade = new CascadeClassifier(EmguAssert.GetFile("haarcascade_eye.xml")))
                      //using (HaarCascade cascade = new HaarCascade("haarcascade_frontalface_alt2.xml"))
                      {
                             Stopwatch watch = Stopwatch.StartNew();
                             Rectangle[] objects = cascade.DetectMultiScale(um, 1.05, 0, new Size(10, 10), Size.Empty);
                             watch.Stop();
                             Trace.WriteLine(String.Format("Objects detected in {0} milliseconds (UseOpenCL: {1})", watch.ElapsedMilliseconds, CvInvoke.UseOpenCL));
                             foreach (Rectangle obj in objects)
                                 image.Draw(obj, new Gray(0.0), 1);

                         }
                     });
        }

        /*
        [TestAttribute]
        public void TestCascadeClassifierFaceDetect()
        {
           Image<Gray, Byte> image = EmguAssert.LoadImage<Gray, byte>("lena.jpg");
           //using (HaarCascade cascade = new HaarCascade("eye_12.xml"))
           using (CascadeClassifier cascade = new CascadeClassifier( EmguAssert.GetFile( "haarcascade_eye.xml" )))
           using (UMat um = image.GetUMat())
           //using (HaarCascade cascade = new HaarCascade("haarcascade_frontalface_alt2.xml"))
           {
              Rectangle[] objects = cascade.DetectMultiScale(um, 1.05, 10, new Size(10, 10), Size.Empty);
              foreach (Rectangle obj in objects)
                 image.Draw(obj, new Gray(0.0), 1);
           }
           //Emgu.CV.UI.ImageViewer.Show(image);
        }*/

#if !(__IOS__ || __ANDROID__ || NETFX_CORE)
        [TestAttribute]
        public void TestConstructor()
        {
            for (int i = 0; i < 20; i++)
            {
                Image<Gray, Byte> img = new Image<Gray, Byte>(500, 500, new Gray());
                EmguAssert.AreEqual(0, System.Convert.ToInt32(img.GetSum().Intensity));
            }

            for (int i = 0; i < 20; i++)
            {
                Image<Bgr, Single> img = new Image<Bgr, Single>(500, 500);
                EmguAssert.IsTrue(img.GetSum().Equals(new Bgr(0.0, 0.0, 0.0)));
            }

            Image<Bgr, Byte> img2 = new Image<Bgr, byte>(1, 2);
            EmguAssert.AreEqual(img2.Data.GetLength(1), 4);

            Byte[,,] data = new Byte[,,] { { { 255, 0, 0 } }, { { 0, 255, 0 } } };
            Image<Bgr, Byte> img3 = new Image<Bgr, byte>(data);

            Image<Gray, Single> img4 = new Image<Gray, float>("stuff.jpg");
            Image<Bgr, Single> img5 = new Image<Bgr, float>("stuff.jpg");

            Bitmap bmp = new Bitmap("stuff.jpg");
            Image<Bgr, Single> img6 = new Image<Bgr, float>(bmp);

            Image<Hsv, Single> img7 = new Image<Hsv, float>("stuff.jpg");
            Image<Hsv, Byte> img8 = new Image<Hsv, byte>("stuff.jpg");

        }
#endif
        [TestAttribute]
        public void TestSub()
        {
            Image<Bgr, Byte> img = new Image<Bgr, Byte>(101, 133);
            Image<Bgr, Byte> r1 = img.Not();
            Image<Bgr, Byte> r2 = 255 - img;
            EmguAssert.IsTrue(r1.Equals(r2));

            Image<Bgr, Byte> img2 = img - 10;
        }

        [TestAttribute]
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

            Image<Gray, float> absDiff = new Image<Gray, float>(convoluted.Size);
            CvInvoke.AbsDiff(laplace, convoluted, absDiff);
            //Emgu.CV.UI.ImageViewer.Show(absDiff.Convert<Gray, byte>());
            EmguAssert.IsTrue(laplace.Equals(convoluted));

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

#if !NETFX_CORE
        private static String GetTempFileName()
        {
            string filename = Path.GetTempFileName();

            File.Delete(filename);

            return Path.Combine(Path.GetDirectoryName(filename), Path.GetFileNameWithoutExtension(filename));
        }
#endif

#if (NETFX_CORE)

      public static Image<Bgr, Byte> WritableBitmapToImage(WriteableBitmap bmp0)
      {
         byte[] data = new byte[bmp0.PixelWidth * bmp0.PixelHeight * 4];
         bmp0.PixelBuffer.CopyTo(data);
         GCHandle dataHandle = GCHandle.Alloc(data, GCHandleType.Pinned);
         try
         {
            using (
               Image<Bgra, Byte> image = new Image<Bgra, byte>(bmp0.PixelWidth, bmp0.PixelHeight, bmp0.PixelWidth * 4,
                  dataHandle.AddrOfPinnedObject()))
            {
               return image.Convert<Bgr, Byte>();
            }
         }
         finally
         {
            dataHandle.Free();
         }
      }
       
#if NETFX_CORE
      [Microsoft.VisualStudio.TestPlatform.UnitTestFramework.AppContainer.UITestMethod]
#else
      [TestAttribute]
#endif
      public void TestBitmapConversion()
      {

         Stopwatch watch = Stopwatch.StartNew();
         Image<Bgr, Byte> image0 = new Image<Bgr, byte>(1200, 1080);
         image0.SetRandNormal(new MCvScalar(120, 120, 120), new MCvScalar(50, 50, 50) );
         WriteableBitmap bmp = image0.Mat.ToWritableBitmap();
         Image<Bgr, Byte> image1 = WritableBitmapToImage(bmp);
         watch.Stop();
         
         Assert.IsTrue(image0.Equals(image1));
      }
#endif

#if !(__IOS__ || __ANDROID__ || NETFX_CORE)
        [TestAttribute]
        public void TestImageSave()
        {
            TestImageSaveHelper(".bmp", System.Drawing.Imaging.ImageFormat.Bmp, 0.0);
            TestImageSaveHelper(".png", System.Drawing.Imaging.ImageFormat.Png, 0.0);
            TestImageSaveHelper(".tiff", System.Drawing.Imaging.ImageFormat.Tiff, 0.0);
            TestImageSaveHelper(".tif", System.Drawing.Imaging.ImageFormat.Tiff, 0.0);
            TestImageSaveHelper(".gif", System.Drawing.Imaging.ImageFormat.Gif, 255.0);
            TestImageSaveHelper(".jpg", System.Drawing.Imaging.ImageFormat.Jpeg, 255.0);
            TestImageSaveHelper(".jpeg", System.Drawing.Imaging.ImageFormat.Jpeg, 255.0);
            //TestImageSaveHelper(".ico", System.Drawing.Imaging.ImageFormat.Icon, 255.0);
        }

        private static void TestImageSaveHelper(String extension, System.Drawing.Imaging.ImageFormat format, double epsilon)
        {
            String fileName = GetTempFileName() + extension;
            try
            {
                using (Image<Bgr, Byte> tmp = new Image<Bgr, byte>(601, 479))
                {
                    tmp.SetRandUniform(new MCvScalar(), new MCvScalar(255, 255, 255));

                    tmp.Save(fileName);

                    using (Image i = Image.FromFile(fileName))
                    {
                        /*
                        if (System.Drawing.Imaging.ImageFormat.Jpeg.Equals(i.RawFormat))
                           Trace.WriteLine("jpeg");
                        else if (System.Drawing.Imaging.ImageFormat.Gif.Equals(i.RawFormat))
                           Trace.WriteLine("gif");
                        else if (System.Drawing.Imaging.ImageFormat.Png.Equals(i.RawFormat))
                           Trace.WriteLine("png");
                        else if (System.Drawing.Imaging.ImageFormat.Bmp.Equals(i.RawFormat))
                           Trace.WriteLine("bmp");
                        */
                        Assert.IsTrue(i.RawFormat.Equals(format));
                    }
                    if (epsilon == 0.0)
                        Assert.IsTrue(tmp.Equals(new Image<Bgr, Byte>(fileName)));
                    else
                    {
                        /*
                        using (Image<Bgr, Byte> delta = new Image<Bgr, Byte>(tmp.Size))
                        using (Image<Gray, Byte> mask = new Image<Gray, byte>(tmp.Size))
                        {
                           CvInvoke.cvAbsDiff(tmp, new Image<Bgr, Byte>(fileName), delta);
                           for (int i = 0; i < delta.NumberOfChannels; i++)
                           {
                              CvInvoke.cvCmpS(delta[i], epsilon, mask, Emgu.CV.CvEnum.CMP_TYPE.CV_CMP_GE);
                              int count = CvInvoke.cvCountNonZero(mask);
                              Assert.AreEqual(0, count);
                           }
                        }*/
                    }
                }
            }
            catch (Exception e)
            {
                throw e;
            }
            finally
            {
                File.Delete(fileName);
            }

        }

        [TestAttribute]
        public void TestBitmapConstructor()
        {
            using (Bitmap bmp0 = new Bitmap(1200, 1080, System.Drawing.Imaging.PixelFormat.Format32bppRgb))
            using (Graphics g = Graphics.FromImage(bmp0))
            {
                g.Clear(Color.Blue);

                Stopwatch watch = Stopwatch.StartNew();
                Image<Bgr, Byte> image0 = new Image<Bgr, byte>(bmp0);
                watch.Stop();
                Trace.WriteLine(String.Format("Convertsion Time: {0} milliseconds", watch.ElapsedMilliseconds));
                Image<Bgr, Byte> imageCmp0 = new Image<Bgr, byte>(image0.Size);
                imageCmp0.SetValue(new Bgr(255, 0, 0));
                Assert.IsTrue(image0.Equals(imageCmp0));
            }

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

        [TestAttribute]
        public void TestBitmapSharedDataWithImage()
        {
            Image<Bgr, Byte> img = new Image<Bgr, byte>(480, 320);
            Bitmap bmp = img.Bitmap;
            bmp.SetPixel(0, 0, Color.Red);
            Image<Bgr, Byte> img2 = new Image<Bgr, byte>(bmp);
            Assert.IsTrue(img.Equals(img2));
        }
#endif
        [TestAttribute]
        public void TestSplitMerge()
        {
            Image<Bgr, Byte> img1 = new Image<Bgr, byte>(301, 234);
            img1.SetRandUniform(new MCvScalar(), new MCvScalar(255, 255, 255));
            Image<Gray, Byte>[] channels = img1.Split();

            Image<Bgr, Byte> img2 = new Image<Bgr, byte>(channels);
            EmguAssert.IsTrue(img1.Equals(img2));
        }

        [TestAttribute]
        public void TestAccumulate()
        {
            Image<Gray, Single> img1 = new Image<Gray, Single>(300, 200);
            img1.SetRandUniform(new MCvScalar(0), new MCvScalar(255));
            Image<Gray, Single> img2 = new Image<Gray, Single>(300, 200);
            img2.SetRandUniform(new MCvScalar(0), new MCvScalar(255));

            Image<Gray, Single> img3 = img1.Copy();
            img3.Accumulate(img2);

            EmguAssert.IsTrue(img3.Equals(img1 + img2));
        }

        [TestAttribute]
        public void TestCanny()
        {
            Image<Bgr, Byte> image = EmguAssert.LoadImage<Bgr, byte>("stuff.jpg");

            //make sure canny works for multi channel image
            Image<Gray, Byte> image2 = image.Canny(200, 100);

            Size size = image2.Size;
        }

        [TestAttribute]
        public void TestInplaceFlip()
        {
            Image<Bgr, byte> image = new Image<Bgr, byte>(20, 20);
            image.SetRandUniform(new MCvScalar(), new MCvScalar(255, 255, 255));

            Image<Bgr, byte> imageOld = image.Copy();
            image._Flip(Emgu.CV.CvEnum.FlipType.Vertical);

            for (int i = 0; i < image.Rows; i++)
                for (int j = 0; j < image.Cols; j++)
                {
                    Bgr c1 = image[i, j];
                    Bgr c2 = imageOld[image.Rows - i - 1, j];
                    EmguAssert.IsTrue(c1.Equals(c2));
                }
        }

        [TestAttribute]
        public void TestFlipPerformance()
        {
            Image<Bgr, byte> image = new Image<Bgr, byte>(2048, 1024);
            image.SetRandNormal(new MCvScalar(), new MCvScalar(255, 255, 255));
            Stopwatch watch = Stopwatch.StartNew();
            image._Flip(Emgu.CV.CvEnum.FlipType.Horizontal | Emgu.CV.CvEnum.FlipType.Vertical);
            watch.Stop();
            EmguAssert.WriteLine(String.Format("Time used: {0} milliseconds", watch.ElapsedMilliseconds));
        }

        [TestAttribute]
        public void TestMoment()
        {
            Image<Gray, byte> image = new Image<Gray, byte>(100, 200);
            image.SetRandUniform(new MCvScalar(), new MCvScalar(255));
            image.ThresholdToZero(new Gray(120));
            MCvMoments moment = image.GetMoments(true);
            double[] huMoment = moment.GetHuMoment();
        }

        /*
        [TestAttribute]
        public void TestSnake()
        {
           Image<Gray, Byte> img = new Image<Gray, Byte>(100, 100, new Gray());

           Rectangle rect = new Rectangle(40, 30, 20, 40);
           img.Draw(rect, new Gray(255.0), -1);

           Point[] pts =
                 new Point[] {
                    new Point(20, 20),
                    new Point(20, 80),
                    new Point(80, 80),
                    new Point(80, 20)};

           Image<Gray, Byte> canny = img.Canny(100.0, 40.0);
           img.Draw(pts, new Gray(120), 1, LineType.EightConnected);
           canny.Snake(pts, 1.0f, 1.0f, 1.0f, new Size(21, 21), new MCvTermCriteria(40, 0.0002));

           img.Draw(pts, new Gray(80), 2, LineType.EightConnected);
           //ImageViewer.Show(img);
        }*/

        [TestAttribute]
        public void TestWaterShed()
        {
            Image<Bgr, Byte> image = EmguAssert.LoadImage<Bgr, byte>("stuff.jpg");
            Image<Gray, Int32> marker = new Image<Gray, Int32>(image.Width, image.Height);
            Rectangle rect = image.ROI;
            marker.Draw(
               new CircleF(
                  new PointF(rect.Left + rect.Width / 2.0f, rect.Top + rect.Height / 2.0f),
               /*(float)(Math.Min(image.Width, image.Height) / 20.0f)*/ 5.0f),
               new Gray(255),
               0);
            Image<Bgr, Byte> result = image.ConcateHorizontal(marker.Convert<Bgr, byte>());
            Image<Gray, Byte> mask = new Image<Gray, byte>(image.Size);
            CvInvoke.Watershed(image, marker);
            using (ScalarArray ia = new ScalarArray(0.5))
                CvInvoke.Compare(marker, ia, mask, CvEnum.CmpType.GreaterThan);

            //ImageViewer.Show(result.ConcateHorizontal(mask.Convert<Bgr, Byte>()));
        }

        [TestAttribute]
        public void TestMatrixDFT()
        {
            //The matrix to be transformed.
            Matrix<float> matB = new Matrix<float>(
               new float[,] {
            {1.0f / 16.0f, 1.0f / 16.0f, 1.0f / 16.0f},
            {1.0f / 16.0f, 8.0f / 16.0f, 1.0f / 16.0f},
            {1.0f / 16.0f, 1.0f / 16.0f, 1.0f / 16.0f}});

            Matrix<float> matBDft = new Matrix<float>(
               CvInvoke.GetOptimalDFTSize(matB.Rows),
               CvInvoke.GetOptimalDFTSize(matB.Cols));
            CvInvoke.CopyMakeBorder(matB, matBDft, 0, matBDft.Height - matB.Height, 0, matBDft.Width - matB.Width, Emgu.CV.CvEnum.BorderType.Constant, new MCvScalar());
            Matrix<float> dftIn = new Matrix<float>(matBDft.Rows, matBDft.Cols, 2);
            Matrix<float> matBDftBlank = matBDft.CopyBlank();
            using (Util.VectorOfMat mv = new Util.VectorOfMat(new Mat[] { matBDft.Mat, matBDftBlank.Mat }))
                CvInvoke.Merge(mv, dftIn);

            Matrix<float> dftOut = new Matrix<float>(dftIn.Rows, dftIn.Cols, 2);
            //perform the Fourior Transform
            CvInvoke.Dft(dftIn, dftOut, Emgu.CV.CvEnum.DxtType.Forward, matB.Rows);

            //The real part of the Fourior Transform
            Matrix<float> outReal = new Matrix<float>(matBDft.Size);
            //The imaginary part of the Fourior Transform
            Matrix<float> outIm = new Matrix<float>(matBDft.Size);
            using (Util.VectorOfMat vm = new Util.VectorOfMat())
            {
                vm.Push(outReal.Mat);
                vm.Push(outIm.Mat);
                CvInvoke.Split(dftOut, vm);
            }
        }

        [TestAttribute]
        public void TestImageDFT()
        {
            Image<Gray, float> matA = EmguAssert.LoadImage<Gray, float>("stuff.jpg");

            //The matrix to be convolved with matA, a bluring filter
            Matrix<float> matB = new Matrix<float>(
               new float[,] {
            {1.0f / 16.0f, 1.0f / 16.0f, 1.0f / 16.0f},
            {1.0f / 16.0f, 8.0f / 16.0f, 1.0f / 16.0f},
            {1.0f / 16.0f, 1.0f / 16.0f, 1.0f / 16.0f}});

            Image<Gray, float> convolvedImage = new Image<Gray, float>(new Size(matA.Width + matB.Width - 1, matA.Height + matB.Height - 1));

            Matrix<float> dftA = new Matrix<float>(
               CvInvoke.GetOptimalDFTSize(convolvedImage.Rows),
               CvInvoke.GetOptimalDFTSize(convolvedImage.Cols));
            matA.CopyTo(dftA.GetSubRect(matA.ROI));

            CvInvoke.Dft(dftA, dftA, Emgu.CV.CvEnum.DxtType.Forward, matA.Rows);

            Matrix<float> dftB = new Matrix<float>(dftA.Size);
            matB.CopyTo(dftB.GetSubRect(new Rectangle(Point.Empty, matB.Size)));
            CvInvoke.Dft(dftB, dftB, Emgu.CV.CvEnum.DxtType.Forward, matB.Rows);

            CvInvoke.MulSpectrums(dftA, dftB, dftA, Emgu.CV.CvEnum.MulSpectrumsType.Default, false);
            CvInvoke.Dft(dftA, dftA, Emgu.CV.CvEnum.DxtType.Inverse, convolvedImage.Rows);
            dftA.GetSubRect(new Rectangle(Point.Empty, convolvedImage.Size)).CopyTo(convolvedImage);
        }

        [TestAttribute]
        public void TestImageDFT2()
        {
            Image<Gray, float> image = EmguAssert.LoadImage<Gray, float>("stuff.jpg");

            Mat complexImage = new Mat(image.Size, DepthType.Cv32F, 2);
            complexImage.SetTo(new MCvScalar(0, 0));
            CvInvoke.InsertChannel(image, complexImage, 0);

            Matrix<float> dft = new Matrix<float>(image.Rows, image.Cols, 2);

            CvInvoke.Dft(complexImage, dft, Emgu.CV.CvEnum.DxtType.Forward, 0);
        }


        [TestAttribute]
        public void TestImageDFTUmat()
        {
            Image<Gray, float> image = EmguAssert.LoadImage<Gray, float>("stuff.jpg");

            UMat complexImage = new UMat(image.Size, DepthType.Cv32F, 2);
            complexImage.SetTo(new MCvScalar(0, 0));
            CvInvoke.InsertChannel(image, complexImage, 0);

            UMat dft = new UMat();

            CvInvoke.Dft(complexImage, dft, Emgu.CV.CvEnum.DxtType.Forward, 0);

            UMat[] tmp = dft.Split();

            EmguAssert.IsTrue(!tmp[0].IsEmpty);
        }

        [TestAttribute]
        public void TestEqualizeHist()
        {
            Image<Bgr, Byte> image = EmguAssert.LoadImage<Bgr, Byte>("lena.jpg");
            image._EqualizeHist();
            //Emgu.CV.UI.ImageViewer.Show(image);
        }

        [TestAttribute]
        public void TestResize()
        {
            Image<Gray, Byte> image = new Image<Gray, byte>(123, 321);
            image.SetRandUniform(new MCvScalar(), new MCvScalar(255));
            image.Resize(512, 512, CvEnum.Inter.Cubic);
        }

        [TestAttribute]
        public void TestRoi()
        {
            Image<Bgr, Byte> image = new Image<Bgr, byte>(1, 1);
            Rectangle roi = image.ROI;

            EmguAssert.AreEqual(roi.Width, image.Width);
            EmguAssert.AreEqual(roi.Height, image.Height);
        }

        [TestAttribute]
        public void TestGetSubRect()
        {
            Image<Bgr, Single> image = new Image<Bgr, float>(200, 100);
            image.SetRandUniform(new MCvScalar(), new MCvScalar(255, 255, 255, 255));
            Rectangle roi = new Rectangle(10, 20, 30, 40);
            Image<Bgr, Single> roi1 = image.Copy(roi);
            Image<Bgr, Single> roi2 = image.GetSubRect(roi);
            EmguAssert.IsTrue(roi1.Equals(roi2));
        }


        [TestAttribute]
        public void TestGoodFeature()
        {
            using (GFTTDetector detector = new GFTTDetector(1000, 0.01, 1, 3, false, 0.04))
            using (Mat img = EmguAssert.LoadMat("stuff.jpg"))
            {

                var keypoints = detector.Detect(img);
                int nanCount = 0;
                foreach (MKeyPoint p in keypoints)
                {
                    CvInvoke.Circle(img, Point.Round(p.Point), 3, new Bgr(255, 0, 0).MCvScalar, 1);
                    if (float.IsNaN(p.Point.X) || float.IsNaN(p.Point.Y))
                        nanCount++;
                }

                System.Diagnostics.Debug.WriteLine(String.Format("NanCount: {0}", nanCount));
                EmguAssert.IsTrue(nanCount == 0);
                //ImageViewer.Show(img);
            }


            using (GFTTDetector detector = new GFTTDetector())
            using (Image<Bgr, Byte> img = EmguAssert.LoadImage<Bgr, Byte>("stuff.jpg"))
            {
                Stopwatch watch = Stopwatch.StartNew();
                int runs = 10;
                for (int i = 0; i < runs; i++)
                {
                    var pts = detector.Detect(img);
                }
                watch.Stop();
                EmguAssert.WriteLine(String.Format("Avg time to extract good features from image of {0}: {1}", img.Size, watch.ElapsedMilliseconds / runs));
            }
        }

        [TestAttribute]
        public void TestContour()
        {
            Image<Gray, Byte> img = EmguAssert.LoadImage<Gray, byte>("stuff.jpg");
            img.SmoothGaussian(3);
            img = img.Canny(80, 50);
            Image<Gray, Byte> res = img.CopyBlank();
            res.SetValue(255);

            using (VectorOfVectorOfPoint contours = new VectorOfVectorOfPoint())
            using (Mat hierachy = new Mat())
            {
                CvInvoke.FindContours(img, contours, hierachy, RetrType.Tree, ChainApproxMethod.ChainApproxSimple);

            }

            using (VectorOfVectorOfPoint contours = new VectorOfVectorOfPoint())
            //using (VectorOfVectorOfInt hierarchy = new VectorOfVectorOfInt())
            {
                CvInvoke.FindContours(img, contours, null, RetrType.List, ChainApproxMethod.ChainApproxSimple);
                for (int i = 0; i < contours.Size; i++)
                {
                    using (VectorOfPoint contour = contours[i])
                    {
                        Point[] pts = contour.ToArray();
                        CvInvoke.Polylines(res, contour, true, new MCvScalar());
                    }
                }
            }
            /*
            Contour<Point> contour = img.FindContours();

            while (contour != null)
            {
               Contour<Point> approx = contour.ApproxPoly(contour.Perimeter * 0.05);

               if (approx.Convex && approx.Area > 20.0)
               {
                  Point[] vertices = approx.ToArray();

                  LineSegment2D[] edges = PointCollection.PolyLine(vertices, true);

                  res.DrawPolyline(vertices, true, new Gray(200), 1);
               }
               contour = contour.HNext;
            }*/
            //Emgu.CV.UI.ImageViewer.Show(res);
        }

        public void TestContour2()
        {
            Image<Bgr, Byte> img1 = new Image<Bgr, byte>(200, 200);
            Image<Bgr, Byte> img2 = new Image<Bgr, byte>(200, 200);

            Point[] polyline = new Point[] {
               new Point(20, 20),
               new Point(20, 30),
               new Point(30, 30),
               new Point(30, 20)};

            img1.Draw(polyline, new Bgr(255, 0, 0), 1, CvEnum.LineType.EightConnected);
            img1.Draw(polyline, new Bgr(0, 255, 0), 1, CvEnum.LineType.EightConnected);
            img1.Draw(polyline, new Bgr(0, 0, 255), 1, CvEnum.LineType.EightConnected);

            /*
            for (int i = 0; i < polyline.Length; i++)
            {
               polyline[i].X += 20;
               polyline[i].Y += 10;
            }
            img1.DrawPolyline(polyline, true, new Bgr(0, 0, 255), 1);
             */

        }

        [TestAttribute]
        public void TestBayerBG2BGR()
        {
            Image<Gray, Byte> image = new Image<Gray, byte>(200, 200);
            image.SetRandUniform(new MCvScalar(), new MCvScalar(255));
            Image<Bgr, Byte> img = new Image<Bgr, byte>(image.Size);
            CvInvoke.CvtColor(image, img, Emgu.CV.CvEnum.ColorConversion.BayerBg2Bgr);
        }

        [TestAttribute]
        public void TestGamma()
        {
            Image<Bgr, Byte> img = new Image<Bgr, byte>(320, 240);
            img.SetRandNormal(new MCvScalar(120, 120, 120), new MCvScalar(50, 50, 50));
            img._GammaCorrect(0.5);
        }

        [TestAttribute]
        public void TestImageIndexer()
        {
            using (Image<Bgr, Byte> image = new Image<Bgr, byte>(100, 500))
            {
                image.SetRandUniform(new MCvScalar(), new MCvScalar(255.0, 255.0, 255.0));
                Stopwatch watch = Stopwatch.StartNew();
                for (int i = 0; i < image.Height; i++)
                    for (int j = 0; j < image.Width; j++)
                    {
                        Bgr color = image[i, j];
                    }
                watch.Stop();
                EmguAssert.WriteLine("Time used: " + watch.ElapsedMilliseconds + ".");
                watch = Stopwatch.StartNew();
                for (int i = 0; i < image.Height; i++)
                    for (int j = 0; j < image.Width; j++)
                        for (int k = 0; k < image.NumberOfChannels; k++)
                        {
                            Byte b = image.Data[i, j, k];
                        }
                watch.Stop();
                EmguAssert.WriteLine("Time used: " + watch.ElapsedMilliseconds + ".");
            }
        }

        [TestAttribute]
        public void TestSetRandomNormal()
        {
            Image<Bgr, Byte> image = new Image<Bgr, byte>(400, 200);
            //image.SetRandUniform(new MCvScalar(), new MCvScalar(255, 255, 255));
            image.SetRandNormal(new MCvScalar(100, 100, 100), new MCvScalar(20, 20, 20));
        }

        [TestAttribute]
        public void TestGenericConvert()
        {
            Image<Gray, Single> g = new Image<Gray, Single>(80, 40);
            Image<Gray, Single> g2 = g.Convert<Single>(delegate (Single v, int x, int y)
            {
                return System.Convert.ToSingle(Math.Sqrt(0.0 + x * x + y * y));
            });
        }

        [TestAttribute]
        public void TestDrawHorizontalLine()
        {
            Point p1 = new Point(10, 10);
            Point p2 = new Point(20, 10);
            LineSegment2D l1 = new LineSegment2D(p1, p2);
            Image<Bgr, Byte> img = new Image<Bgr, byte>(200, 400, new Bgr(255, 255, 255));
            img.Draw(l1, new Bgr(0.0, 0.0, 0.0), 1);
        }

        [TestAttribute]
        public void TestMapDrawRectangle()
        {
            PointF p1 = new PointF(1.1f, 2.2f);
            SizeF p2 = new SizeF(2.2f, 4.4f);
            RectangleF rect = new RectangleF();
            rect.Location = PointF.Empty;
            rect.Size = p2;

            Map<Gray, Byte> map = new Map<Gray, Byte>(new RectangleF(PointF.Empty, new SizeF(4.0f, 8.0f)), new PointF(0.1f, 0.1f), new Gray(255.0));
            map.Draw(rect, new Gray(0.0), 1);
        }

        [TestAttribute]
        public void TestGetSubRect2()
        {
            Image<Bgr, Byte> image = new Image<Bgr, byte>(2048, 2048);
            image.SetRandUniform(new MCvScalar(), new MCvScalar(255, 255, 255));
            Rectangle rect = new Rectangle(new Point(99, 99), new Size(105, 103));
            image.ROI = rect;
            Image<Bgr, Byte> region = image.Copy();
            image.ROI = Rectangle.Empty;
            EmguAssert.IsTrue(image.GetSubRect(rect).Equals(region));

            Stopwatch watch = Stopwatch.StartNew();
            for (int i = 0; i < 100000; i++)
            {
                Image<Bgr, Byte> tmp = image.GetSubRect(rect);
            }
            watch.Stop();
            EmguAssert.WriteLine(String.Format("Time used: {0} milliseconds.", watch.ElapsedMilliseconds));

        }

        [TestAttribute]
        public void TestImageLoader()
        {
            using (Image<Bgr, Single> img = EmguAssert.LoadImage<Bgr, Single>("stuff.jpg"))
            using (Image<Bgr, Single> img2 = img.Resize(100, 100, CvEnum.Inter.Area, true))
            {
                Rectangle r = img2.ROI;
                r.Width >>= 1;
                r.Height >>= 1;
                img2.ROI = r;
            }
        }

        [TestAttribute]
        public void TestBgrSplit()
        {
            using (Image<Bgr, Byte> img = new Image<Bgr, byte>(100, 100, new Bgr(0, 100, 200)))
            {
                Image<Gray, Byte>[] channels = img.Split();
                EmguAssert.AreEqual(img.NumberOfChannels, channels.Length);
            }
        }

        [TestAttribute]
        public void TestDrawFont()
        {
            using (Image<Gray, Byte> img = new Image<Gray, Byte>(200, 300, new Gray()))
            {
                //MCvFont f = new MCvFont(CvEnum.FontFace.HersheyComplexSmall, 1.0, 1.0);
                {
                    img.Draw("h.", new Point(100, 10), CvEnum.FontFace.HersheyComplexSmall, 1.0, new Gray(255.0));
                    img.Draw("a.", new Point(100, 50), CvEnum.FontFace.HersheyComplexSmall, 1.0, new Gray(255.0));
                }
            }
        }

        [TestAttribute]
        public void TestThreshold()
        {
            using (Image<Gray, Byte> image = EmguAssert.LoadImage<Gray, byte>("stuff.jpg"))
            {
                Image<Gray, Byte> thresh1 = new Image<Gray, byte>(image.Size);
                Image<Gray, Byte> thresh2 = new Image<Gray, byte>(image.Size);
                CvInvoke.Threshold(image, thresh1, 0, 255, Emgu.CV.CvEnum.ThresholdType.Otsu | Emgu.CV.CvEnum.ThresholdType.Binary);
                CvInvoke.Threshold(image, thresh2, 255, 255, Emgu.CV.CvEnum.ThresholdType.Otsu | Emgu.CV.CvEnum.ThresholdType.Binary);

                EmguAssert.IsTrue(thresh1.Equals(thresh2));
            }
        }

        [TestAttribute]
        public void TestThreshold2()
        {
            using (Image<Gray, short> image = new Image<Gray, short>(1024, 960))
            {
                image.SetRandUniform(new MCvScalar(short.MinValue), new MCvScalar(short.MaxValue));
                image.ThresholdBinary(new Gray((short.MinValue + short.MaxValue) / 2), new Gray(short.MaxValue));
            }
        }

        [TestAttribute]
        public void TestBgra()
        {
            Image<Bgra, Byte> img = new Image<Bgra, byte>(100, 100);
            img.SetValue(new Bgra(255.0, 120.0, 0.0, 120.0));
            Image<Gray, Byte>[] channels = img.Split();
        }

        [TestAttribute]
        public void TestMixed()
        {
            using (Image<Bgr, Byte> img = EmguAssert.LoadImage<Bgr, Byte>("stuff.jpg"))
            {
                using (Image<Hsv, Byte> imgHsv = img.Convert<Hsv, Byte>())
                {
                    Image<Gray, Byte>[] imgs = imgHsv.Split();
                    using (Image<Hsv, Byte> imgHsv2 = new Image<Hsv, Byte>(imgs))
                    {
                        using (Image<Bgr, Byte> imageRGB = imgHsv2.Convert<Bgr, Byte>())
                        {
                            LineSegment2D[][] lines = imgHsv2.HoughLines(
                                50.0, 200.0,
                                1, Math.PI / 180.0, 50, 50, 10);

                            CircleF[][] circles = img.HoughCircles(
                                new Bgr(200.0, 200.0, 200.0), new Bgr(100.0, 100.0, 100.0),
                                4.0, 1.0);

                            for (int i = 0; i < lines[0].Length; i++)
                            {
                                imageRGB.Draw(lines[0][i], new Bgr(255.0, 0.0, 0.0), 1);
                            }

                            /*
                            for (int i = 0; i < lines[1].Length; i++)
                            {
                               imageRGB.Draw(lines[1][i], new Bgr(0.0, 255.0, 0.0), 1);
                            }

                            for (int i = 0; i < lines[2].Length; i++)
                            {
                               imageRGB.Draw(lines[2][i], new Bgr(0.0, 0.0, 255.0), 1);
                            }*/

                            foreach (CircleF[] cs in circles)
                                foreach (CircleF c in cs)
                                    imageRGB.Draw(c, new Bgr(0.0, 0.0, 0.0), 1);

                            //Application.Run(new ImageViewer(imageRGB));

                            bool applied = false;
                            foreach (CircleF[] cs in circles)
                                foreach (CircleF c in cs)
                                {
                                    if (!applied)
                                    {
                                        CircleF cir = c;
                                        cir.Radius += 30;
                                        using (Image<Gray, Byte> mask = new Image<Gray, Byte>(imageRGB.Width, imageRGB.Height, new Gray(0.0)))
                                        {
                                            mask.Draw(cir, new Gray(255.0), -1);

                                            using (Image<Bgr, Byte> res = imageRGB.InPaint(mask, 50))
                                            {
                                                //Emgu.CV.UI.ImageViewer.Show(res);
                                            }
                                        }
                                        applied = true;
                                    }
                                }
                        }
                    }

                    foreach (Image<Gray, Byte> i in imgs)
                        i.Dispose();
                }
            }
        }

        [TestAttribute]
        public void TestImageConvert()
        {
            try
            {
                Image<Bgr, double> img1 = EmguAssert.LoadImage<Bgr, double>("box.png");
                Image<Gray, double> img2 = img1.Convert<Gray, double>();
            }
            catch (NotSupportedException)
            {
                return;
            }
            Assert.Fail("NotSupportedException should be thrown");
        }

        /*
              [TestAttribute]
              public void TestPlanarObjectDetector()
              {
                 Image<Gray, byte> box = new Image<Gray, byte>(String.Format(_formatString, "box.png"));
                 Image<Gray, byte> scene = new Image<Gray, byte>(String.Format(_formatString, "box_in_scene.png"));
                 //Image<Gray, Byte> scene = box.Rotate(1, new Gray(), false);

                 using (PlanarObjectDetector detector = new PlanarObjectDetector())
                 {
                    Stopwatch watch = Stopwatch.StartNew();
                    LDetector keypointDetector = new LDetector();
                    keypointDetector.Init();

                    PatchGenerator pGen = new PatchGenerator();
                    pGen.SetDefaultParameters();

                    detector.Train(box, 300, 31, 50, 9, 5000, ref keypointDetector, ref pGen);
                    watch.Stop();
                    EmguAssert.WriteLine(String.Format("Training time: {0} milliseconds.", watch.ElapsedMilliseconds));

                    MKeyPoint[] modelPoints = detector.GetModelPoints();
                    int i = modelPoints.Length;

                    HomographyMatrix h = new HomographyMatrix();
                    watch = Stopwatch.StartNew();
                    PointF[] corners = detector.Detect(scene, h);
                    watch.Stop();
                    EmguAssert.WriteLine(String.Format("Detection time: {0} milliseconds.", watch.ElapsedMilliseconds));

                    foreach (PointF c in corners)
                    {
                       scene.Draw(new CircleF(c, 2), new Gray(255), 1);
                    }
                    scene.DrawPolyline(Array.ConvertAll<PointF, Point>(corners, Point.Round), true, new Gray(255), 2);

                    //ImageViewer.Show(scene);
                 }
              }
        */

#if !NETFX_CORE
        [TestAttribute]
        public void TestSaveImage()
        {
            String fileName = Path.Combine(Path.GetTempPath(), "tmp.jpg");
            DateTime t1 = DateTime.Now;
            for (int i = 0; i < 10; i++)
            {
                Image<Gray, Byte> img = EmguAssert.LoadImage<Gray, byte>("stuff.jpg");
                img.Save(fileName);
            }
            EmguAssert.WriteLine(String.Format("Time needed to save the image {0}", DateTime.Now.Subtract(t1).TotalMilliseconds / 10));
            if (File.Exists(fileName))
                File.Delete(fileName);
        }
#endif

        [TestAttribute]
        public void PerformanceComparison()
        {
            Image<Gray, Byte> img1 = new Image<Gray, byte>(1920, 1080);
            Image<Gray, Byte> img2 = new Image<Gray, byte>(img1.Size);

            img1.SetRandUniform(new MCvScalar(0), new MCvScalar(50));
            img2.SetRandUniform(new MCvScalar(0), new MCvScalar(50));

            Stopwatch w = Stopwatch.StartNew();
            Image<Gray, Byte> sum1 = img1 + img2;
            w.Stop();
            EmguAssert.WriteLine(String.Format("OpenCV Time:\t\t\t\t\t\t{0} ms", w.ElapsedMilliseconds));


            w.Reset();
            w.Start();
            Image<Gray, Byte> sum2 = new Image<Gray, byte>(img1.Size);
            Byte[,,] data1 = img1.Data;
            Byte[,,] data2 = img2.Data;
            Byte[,,] dataSum = sum2.Data;
            int rows = img1.Rows;
            int cols = img1.Cols;
            for (int i = 0; i < rows; i++)
                for (int j = 0; j < cols; j++)
                    dataSum[i, j, 0] = (Byte)(data1[i, j, 0] + data2[i, j, 0]);
            w.Stop();
            EmguAssert.WriteLine(String.Format(".NET array manipulation Time:\t\t{0} ms", w.ElapsedMilliseconds));

            EmguAssert.IsTrue(sum2.Equals(sum1));

            w.Reset();
            w.Start();
            Func<Byte, Byte, Byte> convertor = delegate (Byte b1, Byte b2)
            {
                return (Byte)(b1 + b2);
            };
            Image<Gray, Byte> sum3 = img1.Convert<Byte, Byte>(img2, convertor);
            w.Stop();
            EmguAssert.WriteLine(String.Format("Generic image manipulation Time:\t{0} ms", w.ElapsedMilliseconds));

            EmguAssert.IsTrue(sum3.Equals(sum1));

        }

#if !(__IOS__ || __ANDROID__ || NETFX_CORE)
        [TestAttribute]
        public void TestMultiThreadInMemoryWithBMP()
        {
            if (Emgu.Util.Platform.OperationSystem == Emgu.Util.TypeEnum.OS.Windows)
            {
                int threadCount = 32;

                //Create some random images and save to hard disk
                Bitmap[] imageNames = new Bitmap[threadCount];
                for (int i = 0; i < threadCount; i++)
                {
                    using (Image<Bgr, Byte> img = new Image<Bgr, byte>(2048, 1024))
                    {
                        img.SetRandNormal(new MCvScalar(100, 100, 100), new MCvScalar(50, 50, 50));
                        imageNames[i] = img.ToBitmap();
                    }
                }

                Thread[] threads = new Thread[threadCount];

                for (int i = 0; i < threadCount; i++)
                {
                    int index = i;
                    threads[i] = new Thread(delegate ()
                    {
                        Image<Gray, Byte> img = new Image<Gray, byte>(imageNames[index]);
                        Image<Gray, Byte> bmpClone = new Image<Gray, byte>(img.Bitmap);
                    });

                    threads[i].Priority = ThreadPriority.Highest;
                    threads[i].Start();
                }

                for (int i = 0; i < threadCount; i++)
                {
                    threads[i].Join();
                }
            }
        }

        [TestAttribute]
        public void TestMultiThreadWithBMP()
        {
            //TODO: find out why this test fails on unix
            if (Emgu.Util.Platform.OperationSystem == Emgu.Util.TypeEnum.OS.Windows)
            {
                int threadCount = 32;

                //Create some random images and save to hard disk
                String[] imageNames = new String[threadCount];
                for (int i = 0; i < threadCount; i++)
                {
                    using (Image<Bgr, Byte> img = new Image<Bgr, byte>(2048, 1024))
                    {
                        img.SetRandNormal(new MCvScalar(100, 100, 100), new MCvScalar(50, 50, 50));
                        imageNames[i] = String.Format("tmp{0}.bmp", i);
                        img.Save(imageNames[i]);
                    }
                }

                Thread[] threads = new Thread[threadCount];

                for (int i = 0; i < threadCount; i++)
                {
                    int index = i;
                    threads[i] = new Thread(delegate ()
                    {
                        {
                            Image<Gray, Byte> img = new Image<Gray, byte>(imageNames[index]);
                            Image<Gray, Byte> bmpClone = new Image<Gray, byte>(img.Bitmap);
                        }
                    });

                    threads[i].Priority = ThreadPriority.Highest;
                    threads[i].Start();
                }

                for (int i = 0; i < threadCount; i++)
                {
                    threads[i].Join();
                }

                //delete random images;
                foreach (string s in imageNames)
                    File.Delete(s);
            }
        }
#endif

        [TestAttribute]
        public void TestPhaseCorrelate()
        {
            Image<Gray, float> image1 = EmguAssert.LoadImage<Gray, float>("pedestrian.png");
            Image<Gray, float> image2Source = EmguAssert.LoadImage<Gray, float>("lena.jpg");
            Image<Gray, float> image2 = image2Source.Resize(image1.Width, image1.Height, CvEnum.Inter.Nearest);
            double response;
            MCvPoint2D64f pt = CvInvoke.PhaseCorrelate(image1, image2, null, out response);
        }


        [TestAttribute]
        public void TestColorMap()
        {
            Image<Bgr, Byte> image = EmguAssert.LoadImage<Bgr, Byte>("pedestrian.png");
            Image<Bgr, Byte> result = new Image<Bgr, byte>(image.Size);
            CvInvoke.ApplyColorMap(image, result, CvEnum.ColorMapType.Hot);
            //Emgu.CV.UI.ImageViewer.Show(image.ConcateHorizontal(result));
        }

        [TestAttribute]
        public void TestClahe()
        {
            Image<Gray, Byte> image = EmguAssert.LoadImage<Gray, Byte>("pedestrian.png");
            Image<Gray, Byte> result = new Image<Gray, byte>(image.Size);
            CvInvoke.CLAHE(image, 4, new Size(8, 8), result);
            //Emgu.CV.UI.ImageViewer.Show(image.ConcateHorizontal(result));
        }

        [TestAttribute]
        public void TestDenoise()
        {
            Image<Gray, Byte> image = EmguAssert.LoadImage<Gray, Byte>("pedestrian.png");
            Image<Gray, Byte> result = new Image<Gray, byte>(image.Size);
            CvInvoke.FastNlMeansDenoising(image, result, 3f, 7, 21);
            //Emgu.CV.UI.ImageViewer.Show(image.ConcateHorizontal(result));
        }

        [TestAttribute]
        public void TestDenoiseColor()
        {
            Image<Bgr, Byte> image = EmguAssert.LoadImage<Bgr, Byte>("pedestrian.png");
            Image<Bgr, Byte> result = new Image<Bgr, byte>(image.Size);
            CvInvoke.FastNlMeansDenoisingColored(image, result, 3f, 10, 7, 21);
            //Emgu.CV.UI.ImageViewer.Show(image.ConcateHorizontal(result));
        }

        [TestAttribute]
        public void TestDistor()
        {
            Image<Bgr, Byte> image = EmguAssert.LoadImage<Bgr, Byte>("pedestrian.png");
            Matrix<float> mapx, mapy;

            IntrinsicCameraParameters p = new IntrinsicCameraParameters(5);
            int centerY = image.Width >> 1;
            int centerX = image.Height >> 1;
            CvInvoke.SetIdentity(p.IntrinsicMatrix, new MCvScalar(1.0));
            p.IntrinsicMatrix.Data[0, 2] = centerY;
            p.IntrinsicMatrix.Data[1, 2] = centerX;
            p.IntrinsicMatrix.Data[2, 2] = 1;
            p.DistortionCoeffs.Data[0, 0] = -0.000003;

            p.InitUndistortMap(image.Width, image.Height, out mapx, out mapy);

            Image<Bgr, Byte> result = new Image<Bgr, byte>(image.Size);
            CvInvoke.Remap(image, result, mapx, mapy, Emgu.CV.CvEnum.Inter.Cubic);
            //Emgu.CV.UI.ImageViewer.Show(image.ConcateHorizontal(result));
        }

        [TestAttribute]
        public void TestCompare()
        {
            Matrix<float> f1 = new Matrix<float>(1, 380);
            f1.SetValue(0.8);
            Matrix<float> f2 = new Matrix<float>(f1.Size);
            f2.SetValue(1.0);
            Matrix<byte> mask1 = new Matrix<byte>(f1.Size);
            CvInvoke.Compare(f1, f2, mask1, CvEnum.CmpType.LessEqual);
            int total1 = CvInvoke.CountNonZero(mask1);

            EmguAssert.IsTrue(total1 == f1.Width * f1.Height);

            Matrix<Byte> mask2 = new Matrix<byte>(f1.Size);
            using (ScalarArray ia = new ScalarArray(1.0))
            {
                CvInvoke.Compare(f1, ia, mask2, CvEnum.CmpType.LessEqual);
                int total2 = CvInvoke.CountNonZero(mask2);
                EmguAssert.IsTrue(total1 == total2);
            }
        }

        [TestAttribute]
        public void TestSetColor()
        {
            Image<Rgb, byte> img = new Image<Rgb, Byte>(4, 2);
            img.Bytes = new byte[]
            {
                0, 125, 255,   0, 125, 255,   0, 125, 255,   0, 125, 255,
                0, 125, 255,   0, 125, 255,   0, 125, 255,   0, 125, 255,
            };
#if NETFX_CORE
            using (VectorOfByte vb = new VectorOfByte())
                using (Image<Bgr, Byte> imgBgr = img.Convert<Bgr, Byte>())
            {

                CvInvoke.Imencode(".png", img, vb);
                Mat m2 = new Mat();
                CvInvoke.Imdecode(vb, ImreadModes.AnyColor, m2);
                Assert.IsTrue(m2.Equals(imgBgr.Mat));
            }
#else
            img.Save("out.png");
            Image<Rgb, Byte> img2 = new Image<Rgb, byte>(EmguAssert.GetFile("out.png"));
            Assert.IsTrue(img.Equals(img2));
#endif
        }

        [TestAttribute]
        public void TestCountZero()
        {
            var image = new Image<Bgra, Byte>(100, 100);
            int[] count = image.CountNonzero();
        }

        [TestAttribute]
        public void TestMorphologyClosing()
        {
            //draw some blobs
            Image<Gray, Byte> img = new Image<Gray, byte>(400, 400);
            RotatedRect box1 = new RotatedRect(new PointF(100, 200), new SizeF(60, 80), 30.0f);
            RotatedRect box2 = new RotatedRect(new PointF(180, 250), new SizeF(70, 100), 0.0f);
            img.Draw(box1, new Gray(255.0), -1);
            img.Draw(box2, new Gray(255.0), -1);

            Image<Gray, Byte> result = img.ConcateHorizontal(MorphologyClosing(img, 10));
            //Emgu.CV.UI.ImageViewer.Show(result, "Left: original, Right: merged");
        }

        public static Image<Gray, Byte> MorphologyClosing(Image<Gray, Byte> img, int radius)
        {
            int kernelSize = radius * 2 + 1;
            byte[,] kernelMat = new byte[kernelSize, kernelSize];
            for (int i = 0; i < kernelSize; i++)
                for (int j = 0; j < kernelSize; j++)
                {
                    double dx = i - (radius);
                    double dy = j - (radius);
                    double dist = Math.Sqrt(dx * dx + dy * dy);
                    if (dist <= radius)
                    {
                        kernelMat[i, j] = 1;
                    }
                }

            //for definition on the close operation, see.
            //http://en.wikipedia.org/wiki/Mathematical_morphology
            using (Matrix<byte> kernel = new Matrix<byte>(kernelMat))
            {
                return img.MorphologyEx(CvEnum.MorphOp.Close, kernel, new Point(-1, -1), 1, CvEnum.BorderType.Default, new MCvScalar());
            }
            //using (StructuringElementEx e = new StructuringElementEx(kernelMat, radius, radius))
            //{
            //return img.MorphologyEx(e, CvEnum.CV_MORPH_OP.CV_MOP_CLOSE, 1);
            //}
        }
    }
}
