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
using Emgu.CV.Features;
using Emgu.CV.Flann;
using Emgu.CV.Shape;
using Emgu.CV.Stitching;
using Emgu.CV.Text;
using Emgu.CV.Structure;
using Emgu.CV.Bioinspired;
using Emgu.CV.Dpm;
using Emgu.CV.ImgHash;
using Emgu.CV.Face;
using Emgu.CV.Freetype;
using Emgu.CV.StructuredLight;
using Emgu.CV.Dnn;
using Emgu.CV.Cuda;
using Emgu.CV.Mcc;
using Emgu.CV.Models;
using Emgu.CV.Tiff;
using Emgu.CV.Util;
using Emgu.CV.VideoStab;
using Emgu.Util;
#if VS_TEST
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TestAttribute = Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute;
using TestFixture = Microsoft.VisualStudio.TestTools.UnitTesting.TestClassAttribute;
#elif NETFX_CORE
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
using TestAttribute = Microsoft.VisualStudio.TestPlatform.UnitTestFramework.TestMethodAttribute;
using TestFixture = Microsoft.VisualStudio.TestPlatform.UnitTestFramework.TestClassAttribute;
#else
using NUnit.Framework;
#endif

namespace Emgu.CV.Test
{
    [TestFixture]
    public class AutoTestVideoIO
    {
#if !NETFX_CORE
        public static String GetTempFileName()
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
            String fileName = GetTempFileName() + ".avi";

            Mat[] images = new Mat[numberOfFrames];
            for (int i = 0; i < images.Length; i++)
            {
                images[i] = new Mat(height, width, DepthType.Cv8U, 3);
                CvInvoke.Randu(images[i], new MCvScalar(), new MCvScalar(255, 255, 255));
            }

            //using (VideoWriter writer = new VideoWriter(fileName, VideoWriter.Fourcc('H', '2', '6', '4'), 5, new Size(width, height), true))
            using (VideoWriter writer = new VideoWriter(fileName, VideoWriter.Fourcc('M', 'J', 'P', 'G'), 5, new Size(width, height), true))
            //using (VideoWriter writer = new VideoWriter(fileName, VideoWriter.Fourcc('X', '2', '6', '4'), 5, new Size(width, height), true))
            //using (VideoWriter writer = new VideoWriter(fileName, -1, 5, new Size( width, height ), true))
            {
                EmguAssert.IsTrue(writer.IsOpened);
                for (int i = 0; i < numberOfFrames; i++)
                {
                    writer.Write(images[i]);
                }
            }

            FileInfo fi = new FileInfo(fileName);
            EmguAssert.IsTrue(fi.Exists && fi.Length != 0, "File should not be empty");

            using (VideoCapture capture = new VideoCapture(fileName))
            {
                Mat img2 = capture.QueryFrame();
                int count = 0;
                while (img2 != null && !img2.IsEmpty)
                {
                    EmguAssert.IsTrue(img2.Width == width);
                    EmguAssert.IsTrue(img2.Height == height);
                    //EmguAssert.IsTrue(img2.Equals( images[count]) );
                    img2 = capture.QueryFrame();
                    count++;
                }
                EmguAssert.IsTrue(numberOfFrames == count);
            }
            File.Delete(fi.FullName);
        }

        [Test]
        public void TestVideoWriterMSMF()
        {
            Backend[] backends = CvInvoke.WriterBackends;
            int backend_idx = 0; //any backend;
            foreach (Backend be in backends)
            {
                if (be.Name.Equals("MSMF"))
                {
                    backend_idx = be.ID;
                    break;
                }
            }

            if (backend_idx > 0) //if MSMF back-end is found
            {
                int numberOfFrames = 100;
                int width = 640;
                int height = 480;
                String fileName = GetTempFileName() + ".mp4";
                //String fileName = "C:\\tmp\\out.mp4";
                Mat[] images = new Mat[numberOfFrames];
                for (int i = 0; i < images.Length; i++)
                {
                    images[i] = new Mat(width, height, DepthType.Cv8U, 3);
                    CvInvoke.Randu(images[i], new MCvScalar(), new MCvScalar(255, 255, 255));
                    //images[i].SetRandUniform(new MCvScalar(), new MCvScalar(255, 255, 255));
                }

                using (VideoWriter writer = new VideoWriter(
                        fileName,
                        backend_idx,
                        VideoWriter.Fourcc('H', '2', '6', '4'),
                        29.97,
                        new Size(width, height),
                        true))
                {
                    double quality = writer.Get(VideoWriter.WriterProperty.Quality);
                    bool writeSucess = writer.Set(VideoWriter.WriterProperty.Quality, 100);
                    EmguAssert.IsTrue(writer.IsOpened);
                    for (int i = 0; i < numberOfFrames; i++)
                    {
                        writer.Write(images[i]);
                    }
                }

                FileInfo fi = new FileInfo(fileName);
                EmguAssert.IsTrue(fi.Exists && fi.Length != 0, "File should not be empty");

                using (VideoCapture capture = new VideoCapture(
                    fileName,
                    VideoCapture.API.Msmf,
                    new Tuple<CapProp, int>(CapProp.HwAcceleration, (int) VideoAccelerationType.Any)))
                {
                    Mat img2 = capture.QueryFrame();
                    int count = 0;
                    while (img2 != null && !img2.IsEmpty)
                    {
                        EmguAssert.IsTrue(img2.Width == width);
                        EmguAssert.IsTrue(img2.Height == height);
                        //EmguAssert.IsTrue(img2.Equals( images[count]) );
                        img2 = capture.QueryFrame();
                        count++;
                    }

                    EmguAssert.IsTrue(numberOfFrames == count);
                }
                Trace.WriteLine(String.Format("File {0} size: {1}", fi.FullName, fi.Length));
                File.Delete(fi.FullName);
            }
        }
#endif

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
                     delegate (object sender, EventArgs<string> e)
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

#if !ANDROID
        [Test]
        public void TestCaptureFromFile()
        {
            using (VideoCapture capture = new VideoCapture(EmguAssert.GetFile("tree.avi")))
            using (VideoWriter writer = new VideoWriter("tree_invert.avi", 10, new Size(capture.Width, capture.Height), true))
            {
                int maxCount = 10;
                Mat img = new Mat();
                while (capture.Grab() && maxCount > 0)
                {
                    capture.Retrieve(img);
                    CvInvoke.BitwiseNot(img, img);
                    writer.Write(img);
                    maxCount--;
                }
            }
        }
#endif

#if !NETFX_CORE
        [Test]
        public void TestImageDecodeBuffer()
        {
            using (FileStream fs = File.OpenRead(EmguAssert.GetFile("lena.jpg")))
            {
                byte[] data = new byte[fs.Length];
                fs.Read(data, 0, (int)fs.Length);

                Mat image = new Mat();

                CvInvoke.Imdecode(data, ImreadModes.ColorBgr, image);
                //Emgu.CV.WinForms.ImageViewer.Show(image);
            }
        }

        [Test]
        public void TestImdecodeGray()
        {

            using (FileStream fs = File.OpenRead(EmguAssert.GetFile("lena.jpg")))
            {
                byte[] data = new byte[fs.Length];
                fs.Read(data, 0, (int)fs.Length);

                Mat image = new Mat();

                CvInvoke.Imdecode(data, ImreadModes.Grayscale, image);
                EmguAssert.IsTrue(image.NumberOfChannels == 1);
            }
        }

        [TestAttribute]
        public void TestImreadmulti()
        {
            Mat[] mats = CvInvoke.Imreadmulti(EmguAssert.GetFile("multipage.tif"));
        }

#endif

#if !NETFX_CORE
        [Test]
        public void TestFileCaptureNonAscii()
        {
            String fileName = EmguAssert.GetFile("tree.avi");
            String newName = fileName.Replace("tree.avi", "树.avi");
            File.Copy(fileName, newName, true);
            VideoCapture capture = new VideoCapture(EmguAssert.GetFile(newName));
            int counter = 0;
            using (Mat m = new Mat())
                while (capture.Grab())
                {
                    capture.Retrieve(m);
                    counter++;
                }

            Trace.WriteLine(String.Format("{0} frames found in file {1}", counter, newName));
        }

        //TODO: Check why this fails again
        [Test]
        public void TestFileCapturePause()
        {
            int totalFrames1 = 0;
            String fileName = EmguAssert.GetFile("tree.avi");
            String fileName2 = fileName.Replace("tree.avi", "tree2.avi");
            File.Copy(fileName, fileName2, true);

            VideoCapture capture1 = new VideoCapture(fileName);

            //capture one will continute capturing all the frames.
            EventHandler captureHandle1 = delegate
            {
                Mat img = new Mat();
                capture1.Retrieve(img);
                totalFrames1++;
                Trace.WriteLine(String.Format("capture 1 frame {0}: {1}", totalFrames1, DateTime.Now.ToString()));
            };
            capture1.ImageGrabbed += captureHandle1;
            capture1.Start();

            System.Threading.Thread.Sleep(2);

            int totalFrames2 = 0;
            VideoCapture capture2 = new VideoCapture(fileName2);
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

                Mat img = new Mat();
                capture2.Retrieve(img);
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
            while (!(totalFrames1 == totalFrames2))
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

        /*
#if !(__IOS__ || __ANDROID__)
        [Test]
        public void TestGLImageView()
        {
           Emgu.CV.WinForms.GLView.GLImageViewer viewer = new UI.GLView.GLImageViewer();
           //viewer.ShowDialog();
        }
#endif
        */
#endif

#if !(__ANDROID__ || __IOS__ || NETFX_CORE)
        [Test]
        public void TestUnicodeImgFileIO()
        {
            //Bitmap is only available on windows
            if (Platform.OperationSystem == Emgu.Util.Platform.OS.Windows)
            {
                Mat m = EmguAssert.LoadMat("lena.jpg");
                EmguAssert.IsTrue(CvInvoke.Imwrite("测试.jpg", m));
                Bitmap bmp = new Bitmap("测试.jpg");
                Mat tmp = bmp.ToMat();
                UMat m2 = new UMat();
                tmp.CopyTo(m2);
                Mat m3 = EmguAssert.LoadMat("测试.jpg");
                //Emgu.CV.WinForms.ImageViewer.Show(m2);
            }
        }

        [Test]
        public void TestUnicodeCvString()
        {
            String target = "测试.jpg";
            using (CvString s = new CvString(target))
            {
                String s2 = s.ToString();
                EmguAssert.IsTrue(s2.Equals(target));
            }
        }
#endif

#if !NETFX_CORE
        [Test]
        public static void TestOnePassVideoStabilizer()
        {
            //ImageViewer viewer = new ImageViewer();
            using (VideoCapture capture = new VideoCapture("tree.avi"))
            using (Emgu.CV.VideoStab.CaptureFrameSource framesource = new CaptureFrameSource(capture))
            using (Emgu.CV.VideoStab.GaussianMotionFilter motionFilter = new Emgu.CV.VideoStab.GaussianMotionFilter())
            //using (Features2D.FastDetector detector = new Features2D.FastDetector(10, true))
            //using (Features2D.SURF detector = new Features2D.SURF(500, false))
            //using (Features2D.ORBDetector detector = new Features2D.ORBDetector(500))
            using (Emgu.CV.VideoStab.OnePassStabilizer stabilizer = new Emgu.CV.VideoStab.OnePassStabilizer(framesource))
            using (Mat frame = new Mat())
            {
                stabilizer.SetMotionFilter(motionFilter);
                int frameCount = 0;
                while (stabilizer.NextFrame(frame))
                {
                    frameCount++;
                }
                EmguAssert.IsTrue(frameCount > 0, "VideoStabilizer did not return any frames");
            }
        }
#endif
    }
}