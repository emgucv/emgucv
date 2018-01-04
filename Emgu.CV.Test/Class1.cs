//----------------------------------------------------------------------------
//  Copyright (C) 2004-2018 by EMGU Corporation. All rights reserved.       
//----------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Linq;
using Emgu.CV;
using Emgu.CV.Cvb;
using Emgu.CV.Cuda;
using Emgu.CV.CvEnum;
using Emgu.CV.Structure;
using Emgu.CV.UI;
using Emgu.CV.Util;

//using Emgu.UI;
using Emgu.Util;
using Emgu.CV.VideoStab;

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
    public class Tester
    {
        public void TestRotationMatrix2D()
        {
            RotationMatrix2D mat = new RotationMatrix2D(new PointF(1, 2), 30, 1);
            RotationMatrix2D mat2 = new RotationMatrix2D(new PointF(1, 2), 30, 1);
            //Trace.WriteLine(Emgu.Toolbox.MatrixToString<float>(mat.Data, ", ", ";\r\n"));
        }

        public void TestCapture()
        {
            VideoCapture capture = new VideoCapture("abc.efg");
            Mat image = capture.QueryFrame();
        }

        public void TestShowMat()
        {
            Mat m = new Mat(323, 241, CvEnum.DepthType.Cv8U, 3);
            m.SetTo(new MCvScalar());
            Emgu.CV.UI.ImageViewer.Show(m);
        }

        /*
        public void TestKinect()
        {
            using (KinectCapture capture = new KinectCapture(KinectCapture.DeviceType.Kinect, KinectCapture.ImageGeneratorOutputMode.Vga30Hz))
            {
                ImageViewer viewer = new ImageViewer();
                Application.Idle += delegate (Object sender, EventArgs e)
                {
                //Image<Bgr, Byte> img = capture.RetrieveBgrFrame();
                capture.Grab();
                    Mat img = new Mat();
                    capture.RetrieveDisparityMap(img);
                    viewer.Image = img;
                };

                viewer.ShowDialog();
            }
        }*/

        public void CreateUnityIcons()
        {
            //128x128
            Image<Bgra, Byte> imgSmall = GenerateLogo(128, 128);

            //200x258
            Image<Bgra, Byte> imgMedium = GenerateLogo(200, 120).ConcateVertical(new Image<Bgra, byte>(200, 138));



            //860x389
            int screenShotWidth = 400;
            int rightPadding = 40;
            Image<Bgra, Byte> screenShot =
               new Image<Bgr, byte>(@"..\Emgu.CV.Unity\unityStoreIcons\unity_screenshot.png").Resize(screenShotWidth, 209, Inter.Linear,
                  true).Convert<Bgra, Byte>();
            if (screenShot.Width < screenShotWidth)
                screenShot = new Image<Bgra, byte>((screenShotWidth - screenShot.Width) / 2, screenShot.Height).ConcateHorizontal(screenShot);
            Image<Bgra, Byte> imgLarge =
               new Image<Bgra, byte>(860 - (screenShotWidth + rightPadding), 389, new Bgra(255, 255, 255, 0)).ConcateHorizontal(
               GenerateLogo(screenShotWidth, 389 - screenShot.Height).ConcateVertical(screenShot)).ConcateHorizontal(
               new Image<Bgra, byte>(rightPadding, 389, new Bgra(255, 255, 255, 0)));

            imgSmall.Save(@"..\Emgu.CV.Unity\unityStoreIcons\EmguCVLogo_128x128.png");
            imgMedium.Save(@"..\Emgu.CV.Unity\unityStoreIcons\EmguCVLogo_200x258.png");
            imgLarge.Save(@"..\Emgu.CV.Unity\unityStoreIcons\EmguCVLogo_860x389.png");
            //Image<Bgra, Byte> result = imgSmall.ConcateVertical(imgMedium).ConcateVertical(imgLarge);
            //result.Draw(new LineSegment2D(new Point(0, imgSmall.Height), new Point(result.Width, imgSmall.Height) ), new Bgra(0, 0, 0, 255), 1  );
            //result.Draw(new LineSegment2D(new Point(0, imgSmall.Height + imgMedium.Height), new Point(result.Width, imgSmall.Height + imgMedium.Height)), new Bgra(0, 0, 0, 255), 1);
            //ImageViewer.Show(result);
        }

        public void GenerateLogo()
        {
            Image<Bgra, Byte> logo = GenerateLogo(860, 389);
            logo.Save("EmguCVLogo.png");
        }

        public Image<Bgra, byte> GenerateLogo(int width, int height = -1)
        {
            int heightShift = 0;
            int textHeight = (int)(width / 160.0 * 72.0);
            if (height <= 0)
                height = textHeight;
            else
            {
                heightShift = Math.Max((height - textHeight) / 2, 0);
            }
            double scale = width / 160.0;
            Image<Bgr, Byte> semgu = new Image<Bgr, byte>(width, height, new Bgr(0, 0, 0));
            Image<Bgr, Byte> scv = new Image<Bgr, byte>(width, height, new Bgr(0, 0, 0));
            //MCvFont f1 = new MCvFont(CvEnum.FontFace.HersheyTriplex, 1.5 * scale, 1.5 * scale);
            //MCvFont f2 = new MCvFont(CvEnum.FontFace.HersheyComplex, 1.6 * scale, 2.2 * scale);
            semgu.Draw("Emgu", Point.Round(new PointF((float)(6 * scale), (float)(50 * scale + heightShift))), CvEnum.FontFace.HersheyTriplex, 1.5 * scale, new Bgr(55, 155, 255), (int)Math.Round(1.5 * scale));
            semgu._Dilate((int)(1 * scale));
            scv.Draw("CV", Point.Round(new PointF((float)(50 * scale), (float)(60 * scale + heightShift))), CvEnum.FontFace.HersheySimplex, 1.6 * scale, new Bgr(255, 55, 255), (int)Math.Round(2.2 * scale));
            scv._Dilate((int)(2 * scale));
            Image<Bgr, Byte> logoBgr = semgu.Or(scv);
            Image<Gray, Byte> logoA = new Image<Gray, byte>(logoBgr.Size);
            logoA.SetValue(255, logoBgr.Convert<Gray, Byte>());
            logoBgr._Not();
            logoA._Not();
            Image<Gray, Byte>[] channels = logoBgr.Split();
            channels = new Image<Gray, byte>[] { channels[0], channels[1], channels[2], new Image<Gray, Byte>(channels[0].Width, channels[0].Height, new Gray(255.0)) };
            Image<Bgra, Byte> logoBgra = new Image<Bgra, byte>(channels);
            logoBgra.SetValue(new Bgra(0.0, 0.0, 0.0, 0.0), logoA);
            //logoBgra.Save("EmguCVLogo.png");
            return logoBgra;
            /*
            Image<Bgr, Byte> bg_header = new Image<Bgr, byte>(1, 92);
            for (int i = 0; i < 92; i++)
               bg_header[i, 0] = new Bgr(210, 210 - i * 0.4, 210 - i * 0.9);
            bg_header.Save("bg_header.gif");*/
        }

        public void TestImage()
        {

            ImageViewer viewer = new ImageViewer();
            Application.Idle += delegate (Object sender, EventArgs e)
            {
                Image<Bgr, Byte> image = new Image<Bgr, byte>(400, 400);
                image.SetRandUniform(new MCvScalar(), new MCvScalar(255, 255, 255));
                image.Save("temp.jpeg");
                Image<Bgr, Byte> img = new Image<Bgr, byte>("temp.jpeg");
                viewer.Image = img;
            };

            viewer.ShowDialog();
            /*
            for (int i = 0; i < 10000; i++)
            {
               Image<Bgr, Byte> img = new Image<Bgr, byte>("temp.jpeg");
               viewer.Image = img;
            }*/


        }

        /*
        public void TestPointPerformance()
        {
           #region test point constructors
           int numberOfPoints = 100000;
           Stopwatch stopwatch = Stopwatch.StartNew();
           Point2D<int>[] pts = new Point2D<int>[numberOfPoints];
           for (int i = 0; i < pts.Length; i++)
              pts[i] = new Point2D<int>();
           stopwatch.Stop();
           Trace.WriteLine("Point2D creation: " + stopwatch.ElapsedMilliseconds + " milliseconds.");
           stopwatch.Reset(); stopwatch.Start();
           System.Drawing.Point[] mpts = new System.Drawing.Point[numberOfPoints];
           stopwatch.Stop();
           Trace.WriteLine("System.Drawing.Point creation: " + stopwatch.ElapsedMilliseconds + " milliseconds.");
           #endregion

           #region System.Drawing.Point example
           System.Drawing.Point p1 = new System.Drawing.Point(0, 0); 

           System.Drawing.Point[] ptsArray = new System.Drawing.Point[1]; //structure (value type) in array are initialized, ptsArray[0] has been allocated to a default point (0,0)
           ptsArray[0] = p1; //ptsArray[0] now contains a copy of p1 (0, 0)
           p1.X = 1; //change the value on p1, now p1 is (1, 0)
           Trace.WriteLine("difference in X: " + (p1.X - ptsArray[0].X)); //ptsArray[0] returns a copy of the point (0,0)
           #endregion

           int numberOfPointsInArray = 1000000;
           int numberOfReadyAccess = 1000;

           stopwatch.Reset(); stopwatch.Start();
           //initialize Point2D array
           Point2D<int>[] pointArray = new Point2D<int>[numberOfPointsInArray];
           for (int i = 0; i < pointArray.Length; i++)
              pointArray[i] = new Point2D<int>();

           for (int j = 0; j < numberOfReadyAccess; j++)
              for (int i = 0; i < pointArray.Length; i++)
              {
                 pointArray[i].X = 2;
              }
           stopwatch.Stop();
           Trace.WriteLine("Time to access elements in Point2D<int> array: " + stopwatch.ElapsedMilliseconds);

           stopwatch.Reset(); stopwatch.Start();
           //initialize System.Drawing.Point array
           System.Drawing.Point[] pointStructures = new System.Drawing.Point[numberOfPointsInArray];

           for (int j = 0; j < numberOfReadyAccess; j++)
              for (int i = 0; i < pointStructures.Length; i++)
              {
                 pointStructures[i].X = 2;
              }
           stopwatch.Stop();
           Trace.WriteLine("Time to access elements in System.Drawing.Point array: " + stopwatch.ElapsedMilliseconds);

        }*/

        public void TestCvNamedWindow1()
        {
            //The name of the window
            String win1 = "Test Window";

            //Create the window using the specific name
            CvInvoke.NamedWindow(win1);

            //Create an image of 400x200 of Blue color
            using (Image<Bgr, Byte> img = new Image<Bgr, byte>(400, 200, new Bgr(255, 0, 0)))
            {
                //Create the font
                //MCvFont f = new MCvFont(CvEnum.FontFace.HersheyComplex, 1.0, 1.0);
                //Draw "Hello, world." on the image using the specific font
                img.Draw("Hello, world", new Point(10, 80), CvEnum.FontFace.HersheyComplex, 1.0, new Bgr(0, 255, 0));

                //Show the image
                CvInvoke.Imshow(win1, img);
                //Wait for the key pressing event
                CvInvoke.WaitKey(0);
                //Destroy the window
                CvInvoke.DestroyWindow(win1);
            }
        }

        public void TestCvNamedWindow2()
        {
            //Create an image of 400x200 of Blue color
            using (Image<Bgr, Byte> img = new Image<Bgr, byte>(400, 200, new Bgr(255, 0, 0)))
            {
                //Create the font
                //MCvFont f = new MCvFont(CvEnum.FontFace.HersheyComplex, 1.0, 1.0);

                //Draw "Hello, world." on the image using the specific font
                img.Draw("Hello, world", new Point(10, 80), CvEnum.FontFace.HersheyComplex, 1.0, new Bgr(0, 255, 0));

                //Show the image using ImageViewer from Emgu.CV.UI
                ImageViewer.Show(img, "Test Window");
            }
        }

        /*
        public void TestModuleInfo()
        {
           string pluginName;
           string versionName;
           CvToolbox.GetModuleInfo(out pluginName, out versionName);
           Trace.WriteLine(String.Format("Plugin: {0}\r\nVersion: {1}", pluginName, versionName ));
        }*/

        public void TestRandom()
        {
            using (Image<Bgr, byte> img = new Image<Bgr, byte>(200, 200))
            {
                img.SetRandNormal(new MCvScalar(0.0, 0.0, 0.0), new MCvScalar(50.0, 50.0, 50.0));
                ImageViewer.Show(img);
            }
        }

        /*
        public void TestGpuVibe()
        {
           int warmUpFrames = 20;

           GpuVibe<Gray> vibe = null;
           Image<Gray, Byte> mask = null;
           using (ImageViewer viewer = new ImageViewer()) //create an image viewer
           using (Capture capture = new Capture()) //create a camera captue
           {
              capture.ImageGrabbed += delegate(object sender, EventArgs e)
              {  
                 //run this until application closed (close button click on image viewer)

                 using(Image<Bgr, byte> frame = capture.RetrieveBgrFrame(0))
                 using (CudaImage<Bgr, byte> gpuFrame = new CudaImage<Bgr, byte>(frame))
                 using (CudaImage<Gray, Byte> gpuGray = gpuFrame.Convert<Gray, Byte>())
                 {
                    if (warmUpFrames > 0)
                    {
                       warmUpFrames--;
                       return;
                    }

                    if (vibe == null)
                    {
                       vibe = new GpuVibe<Gray>(1234567, gpuGray, null);
                       return;
                    }
                    else
                    {
                       vibe.Apply(gpuGray, null);
                       if (mask == null)
                          mask = new Image<Gray, byte>(vibe.ForgroundMask.Size);

                       vibe.ForgroundMask.Download(mask);
                       viewer.Image = frame.ConcateHorizontal(mask.Convert<Bgr, Byte>()); //draw the image obtained from camera

                    }
                 }
              };
              capture.Start();
              viewer.ShowDialog(); //show the image viewer
           }
        }

        public void TestGpuBackgroundModel()
        {
           int warmUpFrames = 20;
           int totalFrames = 0;

           //CudaBackgroundSubtractorMOG2<Bgr>  bgModel = null;
           //CudaBackgroundSubtractorMOG<Bgr> bgModel = null;
           CudaBackgroundSubtractorGMG<Bgr> bgModel = null;
           //CudaBackgroundSubtractorFGD<Bgr> bgModel = null;

           Image<Gray, Byte> mask = null;
           using (ImageViewer viewer = new ImageViewer()) //create an image viewer
           using (Capture capture = new Capture()) //create a camera captue
           {
              capture.ImageGrabbed += delegate(object sender, EventArgs e)
              {
                 //run this until application closed (close button click on image viewer)
                 totalFrames++;

                 if (viewer != null && !viewer.IsDisposed)
                 {
                    if (viewer.InvokeRequired)
                    {
                       viewer.Invoke((Action)delegate { viewer.Text = String.Format("Processing {0}th frame.", totalFrames); });
                    }
                    else
                    {
                       viewer.Text = String.Format("Processing {0}th frame.", totalFrames); 
                    }
                 }

                 using (Image<Bgr, byte> frame = capture.RetrieveBgrFrame(0))
                 using (CudaImage<Bgr, byte> gpuFrame = new CudaImage<Bgr, byte>(frame))
                 {
                    if (warmUpFrames > 0)
                    {
                       warmUpFrames--;
                       return;
                    }

                    if (bgModel == null)
                    {
                       //bgModel = new CudaBackgroundSubtractorMOG2<Bgr>(500, 16, true);
                       //bgModel = new CudaBackgroundSubtractorMOG<Bgr>(200, 5, 0.7, 0);
                       bgModel = new CudaBackgroundSubtractorGMG<Bgr>(120, 0.8);
                       bgModel.Apply(gpuFrame, -1.0f, null);
                       //bgModel = new CudaBackgroundSubtractorFGD<Bgr>(128, 15, 25, 64, 25, 40, true, 1, 0.1f, 0.005f, 0.1f, 2.0f, 0.9f, 15.0f);
                       //bgModel.Apply(gpuFrame, -1.0f);

                       return;
                    }
                    else
                    {
                       bgModel.Apply(gpuFrame, -1.0f, null);
                       //bgModel.Apply(gpuFrame, -1.0f);

                       if (mask == null)
                          mask = new Image<Gray, byte>(bgModel.ForgroundMask.Size);

                       bgModel.ForgroundMask.Download(mask);
                       Image<Bgr, Byte> result = frame.ConcateHorizontal(mask.Convert<Bgr, Byte>());
                       if (viewer != null && !viewer.IsDisposed)
                       {
                          if (viewer.InvokeRequired)
                          {
                             viewer.Invoke((Action)delegate { viewer.Image = result; });
                          }
                          else
                          {
                             viewer.Image = result; //draw the image obtained from camera
                          }
                       }

                    }
                 }
              };
              capture.Start();
              viewer.ShowDialog(); //show the image viewer
           }
        }*/

        public void CameraTest()
        {
            using (ImageViewer viewer = new ImageViewer()) //create an image viewer
            using (VideoCapture capture = new VideoCapture()) //create a camera captue
            {
                capture.ImageGrabbed += delegate (object sender, EventArgs e)
                {  //run this until application closed (close button click on image viewer)
                Mat m = new Mat();
                    capture.Retrieve(m);
                    viewer.Image = m; //draw the image obtained from camera
            };
                capture.Start();
                viewer.ShowDialog(); //show the image viewer
            }
        }

        /*
        public void CameraTest2()
        {
           using (ImageViewer viewer = new ImageViewer())
           using (Capture capture = new Capture())
           {
              capture.ImageGrabbed += delegate(object sender, EventArgs e)
              {
                 Image<Bgr, Byte> img = capture.RetrieveBgrFrame(0);
                 img = img.Resize(0.8, Emgu.CV.CvEnum.Inter.Linear);
                 Image<Gray, Byte> gray = img.Convert<Gray, Byte>();
                 gray._EqualizeHist();
                 viewer.Image = gray;

                 capture.Pause();
                 System.Threading.ThreadPool.QueueUserWorkItem(delegate
                 {
                    Thread.Sleep(1000);
                    capture.Start();
                 });
              };
              capture.Start();
              viewer.ShowDialog();
           }
        }*/

        public void CameraTest3()
        {
            ImageViewer viewer = new ImageViewer();
            using (VideoCapture capture = new VideoCapture())
            {
                Application.Idle += delegate (object sender, EventArgs e)
                {
                    Mat frame = capture.QueryFrame();
                    if (frame != null)
                    {
                        Bitmap bmp = frame.ToImage<Bgr, Byte>().ToBitmap();

                        viewer.Image = new Image<Bgr, Byte>(bmp);

                    }
                };
                viewer.ShowDialog();
            }
        }

        public void TestHaarPerformance()
        {
            CascadeClassifier face = new CascadeClassifier("haarcascade_frontalface_alt2.xml");
            Image<Gray, Byte> img = new Image<Gray, byte>("lena.jpg");
            Stopwatch watch = Stopwatch.StartNew();
            face.DetectMultiScale(img, 1.1, 3, Size.Empty, Size.Empty);
            watch.Stop();
            Trace.WriteLine(String.Format("Detecting face from {0}x{1} image took: {2} milliseconds.", img.Width, img.Height, watch.ElapsedMilliseconds));
        }

        public void TestFaceDetect()
        {
            using (Image<Bgr, Byte> image = new Image<Bgr, byte>("lena.jpg"))
            using (Image<Bgr, Byte> smooth = image.SmoothGaussian(7))
            {
                DateTime t1 = DateTime.Now;

                FaceDetector fd = new FaceDetector();
                Face f = fd.Detect(smooth)[0];
                TimeSpan ts = DateTime.Now.Subtract(t1);
                Trace.WriteLine(ts.TotalMilliseconds);

                Eye e = f.DetectEye()[0];

                //Application.Run(new ImageViewer(e.RGB));

                /*
                Image<Rgb, Byte> res = f.RGB.BlankClone();
                res.Draw(f.SkinContour, new Rgb(255.0, 255.0, 255.0), new Rgb(255.0, 255.0, 255.0), -1);
                Application.Run(new ImageViewer(res.ToBitmap()));
                */
            }
        }

        public void TestCompression()
        {
            Image<Bgr, Byte> image = new Image<Bgr, byte>("lena.jpg");
            DateTime t1 = DateTime.Now;
            int l1 = image.Bytes.Length;
            DateTime t2 = DateTime.Now;
            image.SerializationCompressionRatio = 9;
            int l2 = image.Bytes.Length;
            DateTime t3 = DateTime.Now;
            TimeSpan ts1 = t2.Subtract(t1);
            TimeSpan ts2 = t3.Subtract(t2);
            Trace.WriteLine(String.Format("Image Size: {0} x {1}", image.Width, image.Height));
            Trace.WriteLine(String.Format("T1: {0}, T2: {1}, Delta: {2}", ts1.TotalMilliseconds, ts2.TotalMilliseconds, ts2.TotalMilliseconds - ts1.TotalMilliseconds));

            Trace.WriteLine(
                String.Format(
                "Original size: {0}; Compressed Size: {1}, Compression Ratio: {2}%",
                l1,
                l2,
                l2 * 100.0 / l1));
        }

        public void TestMarshalIplImage()
        {
            Image<Bgr, Single> image = new Image<Bgr, float>(2041, 1023);
            DateTime timeStart = DateTime.Now;
            for (int i = 0; i < 10000; i++)
            {
                MIplImage img = image.MIplImage;
            }
            TimeSpan timeSpan = DateTime.Now.Subtract(timeStart);
            Trace.WriteLine(String.Format("Time: {0} milliseconds", timeSpan.TotalMilliseconds));
        }

        public void TestImageViewer()
        {
            //System.Threading.Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo("zh-CN");

            ImageViewer viewer = new ImageViewer(null);
            //viewer.Image = new Image<Bgr, Byte>(50, 50);
            //viewer.Image = null;
            //viewer.ImageBox.FunctionalMode = ImageBox.FunctionalModeOption.RightClickMenu;
            viewer.ShowDialog();

            //Application.Run(new ImageViewer(null));

        }

        //[TestAttribute]
        public void TestImageViewerUMat()
        {
            UMat umat = new UMat(new Size(640, 320), DepthType.Cv8U, 1);

            ImageViewer viewer = new ImageViewer(umat);
            //viewer.Image = new Image<Bgr, Byte>(50, 50);
            //viewer.Image = null;
            //viewer.ImageBox.FunctionalMode = ImageBox.FunctionalModeOption.RightClickMenu;
            viewer.ShowDialog();
            
            //Application.Run(new ImageViewer(null));

        }

        //[TestAttribute]
        public void TestImageViewerGpuMat()
        {
            GpuMat gpumat = new GpuMat(320, 640,  DepthType.Cv8U, 1);

            ImageViewer viewer = new ImageViewer(gpumat);
            //viewer.Image = new Image<Bgr, Byte>(50, 50);
            //viewer.Image = null;
            //viewer.ImageBox.FunctionalMode = ImageBox.FunctionalModeOption.RightClickMenu;
            viewer.ShowDialog();

            //Application.Run(new ImageViewer(null));

        }


        public void TestImageViewerFrameRate()
        {
            ImageViewer viewer = new ImageViewer(null);
            Image<Bgr, Byte> img = new Image<Bgr, Byte>(1024, 1024);

            Application.Idle += delegate (Object sender, EventArgs e)
            {
                double v = DateTime.Now.Ticks % 30;
                img.SetValue(new Bgr(v, v, v));

                viewer.Image = img;
            };
            viewer.ShowDialog();

        }

        public void TestPlayVideo()
        {
            VideoCapture capture = new VideoCapture("car.avi");
            ImageViewer viewer = new ImageViewer(null);

            Application.Idle += delegate (Object sender, EventArgs e)
            {
                Mat m = capture.QueryFrame();
                if (m != null && !m.IsEmpty)
                {
                    viewer.Image = m;
                    Thread.Sleep(300);
                }
            };
            viewer.ShowDialog();
        }

        /*
        public static void TestOnePassVideoStabilizerCamera()
        {
           ImageViewer viewer = new ImageViewer();
           using (Capture capture = new Capture())
           using (GaussianMotionFilter motionFilter = new GaussianMotionFilter())
           //using (Features2D.FastDetector detector = new Features2D.FastDetector(10, true))
           using (Features2D.SURF detector = new Features2D.SURF(500, false))
           //using (Features2D.ORBDetector detector = new Features2D.ORBDetector(500))
           using (OnePassStabilizer stabilizer = new OnePassStabilizer(capture))
           {
              stabilizer.SetMotionFilter(motionFilter);
              //motionEstimator.SetDetector(detector);

              //stabilizer.SetMotionEstimator(motionEstimator);
              Application.Idle += delegate(object sender, EventArgs e)
              {
                 Image<Bgr, byte> frame = stabilizer.NextFrame();
                 if (frame != null)
                    viewer.Image = frame;
              };
              viewer.ShowDialog();
           }
        }*/

        public static void TestOnePassVideoStabilizer()
        {
            ImageViewer viewer = new ImageViewer();
            using (VideoCapture capture = new VideoCapture("tree.avi"))
            using (CaptureFrameSource frameSource = new CaptureFrameSource(capture))
            using (OnePassStabilizer stabilizer = new OnePassStabilizer(frameSource))
            {
                Stopwatch watch = new Stopwatch();
                //stabilizer.SetMotionEstimator(motionEstimator);
                Application.Idle += delegate (object sender, EventArgs e)
                {
                    watch.Reset();
                    watch.Start();
                    Mat frame = stabilizer.NextFrame();
                    watch.Stop();
                    if (watch.ElapsedMilliseconds < 200)
                    {
                        Thread.Sleep(200 - (int)watch.ElapsedMilliseconds);
                    }
                    if (frame != null)
                        viewer.Image = frame;
                };
                viewer.ShowDialog();
            }
        }

        public static void TestTwoPassVideoStabilizer()
        {
            ImageViewer viewer = new ImageViewer();
            using (VideoCapture capture = new VideoCapture("tree.avi"))
            using (GaussianMotionFilter motionFilter = new GaussianMotionFilter(15, -1.0f))
            //using (Features2D.FastDetector detector = new Features2D.FastDetector(10, true))
            //using (Features2D.SURF detector = new Features2D.SURF(500, false))
            //using (Features2D.ORBDetector detector = new Features2D.ORBDetector(500))
            using (CaptureFrameSource frameSource = new CaptureFrameSource(capture))
            using (TwoPassStabilizer stabilizer = new TwoPassStabilizer(frameSource))
            {
                Stopwatch watch = new Stopwatch();
                //stabilizer.SetMotionEstimator(motionEstimator);
                Application.Idle += delegate (object sender, EventArgs e)
                {
                    watch.Reset();
                    watch.Start();
                    Mat frame = stabilizer.NextFrame();
                    watch.Stop();
                    if (watch.ElapsedMilliseconds < 200)
                    {
                        Thread.Sleep(200 - (int)watch.ElapsedMilliseconds);
                    }
                    if (frame != null)
                        viewer.Image = frame;
                };
                viewer.ShowDialog();
            }
        }

        public static void TestSuperres()
        {
            ImageViewer viewer = new ImageViewer();
            //using (Capture capture = new Capture("car.avi"))
            using (Superres.FrameSource frameSource = new Superres.FrameSource("car.avi", false))
            using (Superres.SuperResolution sr = new Superres.SuperResolution(Superres.SuperResolution.OpticalFlowType.Btvl, frameSource))
            //using (Superres.SuperResolution sr = new Superres.SuperResolution(Superres.SuperResolution.OpticalFlowType.BTVL1_OCL, frameSource))
            {
                Stopwatch watch = new Stopwatch();
                int counter = 0;
                Application.Idle += delegate (object sender, EventArgs e)
                {
                    watch.Reset();
                    watch.Start();

                //Image<Bgr, byte> frame = frameSource.NextFrame();
                Mat frame = new Mat();
                    sr.NextFrame(frame);
                //Image<Gray, Byte> frame = capture.QueryGrayFrame();
                watch.Stop();
                    if (watch.ElapsedMilliseconds < 200)
                    {
                        Thread.Sleep(200 - (int)watch.ElapsedMilliseconds);
                    }
                    if (!frame.IsEmpty)
                    {
                        viewer.Image = frame;
                        viewer.Text = String.Format("Frame {0}: {1} milliseconds.", counter++, watch.ElapsedMilliseconds);
                    }
                    else
                    {
                        viewer.Text = String.Format("{0} frames processed", counter);
                    }
                };
                viewer.ShowDialog();
            }
        }

        public static void TestImageViewMat()
        {
            Mat m = CvInvoke.Imread(EmguAssert.GetFile("box.png"), ImreadModes.AnyColor);
            Mat m2 = new Mat();
            CvInvoke.CvtColor(m, m2, ColorConversion.Gray2Rgb);
            Mat m3 = new Mat();
            m2.ConvertTo(m3, DepthType.Cv16U);

            ImageViewer.Show(m3);
        }

        public static void TestCaptureFrameSource()
        {
            ImageViewer viewer = new ImageViewer();
            using (VideoCapture capture = new VideoCapture())
            using (CaptureFrameSource frameSource = new CaptureFrameSource(capture))
            {
                Application.Idle += delegate (object sender, EventArgs e)
                {
                    Mat frame = frameSource.NextFrame();
                    if (frame != null)
                        viewer.Image = frame;
                };
                viewer.ShowDialog();
            }
        }

        /*
        public static void TestCodeBook()
        {
           int learningFrames = 40;
           using (BlobDetector detector = new BlobDetector(CvEnum.BlobDetectorType.Simple))
           using (BlobSeq blobs = new BlobSeq())
           using (Capture capture = new Capture("tree.avi"))
           using (BGCodeBookModel<Ycc> bgmodel = new BGCodeBookModel<Ycc>())
           {

              #region Set color thresholds values
              //bgmodel.MCvBGCodeBookModel.ModMin0 = bgmodel.MCvBGCodeBookModel.ModMin1 = bgmodel.MCvBGCodeBookModel.ModMin2 = 3;
              //bgmodel.MCvBGCodeBookModel.ModMax0 = bgmodel.MCvBGCodeBookModel.ModMax1 = bgmodel.MCvBGCodeBookModel.ModMax2 = 10;
              //bgmodel.MCvBGCodeBookModel.CbBounds0 = bgmodel.MCvBGCodeBookModel.CbBounds1 = bgmodel.MCvBGCodeBookModel.CbBounds2 = 10;
              #endregion


              ImageViewer viewer = new ImageViewer();
              int count = 0;
              EventHandler processFrame = delegate(Object sender, EventArgs e)
              {
                 Image<Bgr, Byte> img = capture.RetrieveBgrFrame();
                 if (img == null)
                 {
                    return;
                 }

                 viewer.Text = String.Format("Processing {0}th image. {1}", count++, learningFrames > 0 ? "(Learning)" : String.Empty);

                 using (Image<Ycc, Byte> ycc = img.Convert<Ycc, Byte>()) //using YCC color space for BGCodeBook
                 {
                    if (learningFrames == 0) //training is completed
                       bgmodel.ClearStale(bgmodel.MCvBGCodeBookModel.T / 2, Rectangle.Empty, null);

                    if (learningFrames > 0)
                       bgmodel.Apply(ycc); 
                    else if (learningFrames <= 0)
                    {
                       bgmodel.Diff(ycc, Rectangle.Empty);
                       Image<Gray, Byte> m = bgmodel.ForegroundMask.Clone();
                       blobs.Clear();
                       if (detector.DetectNewBlob(m, blobs, null))
                       {
                          foreach (MCvBlob b in blobs)
                             m.Draw((Rectangle) b, new Gray(100), 2);
                       }
                       viewer.Image = m;
                    }
                    learningFrames--;
                    System.Threading.Thread.Sleep(100);
                 }

                 img.Dispose();
              };
              capture.ImageGrabbed += processFrame;
              capture.Start();

              viewer.ShowDialog();
           }
        }

        public void TestArrayChangeDimension()
        {
           float[] vec = new float[10];
           for (int i = 0; i < vec.Length; i++)
              vec[i] = (float)i;

           float[,] vec2 = new float[vec.Length, 1];

           GCHandle hdl1 = GCHandle.Alloc(vec, GCHandleType.Pinned);
           GCHandle hdl2 = GCHandle.Alloc(vec2, GCHandleType.Pinned);
           Emgu.Util.Toolbox.memcpy(hdl1.AddrOfPinnedObject(), hdl2.AddrOfPinnedObject(), vec.Length * Marshal.SizeOf(typeof(float)));
           hdl1.Free();
           hdl2.Free();
           //Array.Copy(vec, vec2, vec.Length);

           for (int i = 0; i < vec.Length; i++)
           {
              Assert.AreEqual(vec[i], vec2[i, 0]);
           }
        }*/


        private void TestMap()
        {
            PointF center = new PointF(-110032, -110032);
            float width = 10000, height = 12000;


            Map<Gray, Byte> map = new Map<Gray, byte>(new RectangleF(center.X - width / 2, center.Y - height / 2, width, height), new PointF(100, 100));
            PointF[] pts = new PointF[]
               {
                new PointF( (float)center.X + 3120,(float) center.Y + 2310),
                new PointF((float)center.X -220, (float) center.Y-4120)
               };
            map.DrawPolyline(pts, false, new Gray(255.0), 1);
            Triangle2DF tri = new Triangle2DF(
                new PointF((float)center.X - 1000.0f, (float)center.Y + 200.0f),
                new PointF((float)center.X - 3000.0f, (float)center.Y + 200.0f),
                new PointF((float)center.X - 700f, (float)center.Y + 800.0f));
            map.Draw(tri, new Gray(80), 0);
            map.Draw(tri, new Gray(255), 1);
            ImageViewer.Show(map);
        }

        private class SyntheticData
        {
            private Matrix<float> _state;
            private Matrix<float> _transitionMatrix;
            private Matrix<float> _measurementNoise;
            private Matrix<float> _processNoise;
            private Matrix<float> _errorCovariancePost;
            private Matrix<float> _measurementMatrix;
            public Matrix<float> MeasurementMatrix
            {
                get
                {
                    return _measurementMatrix;
                }
            }
            public Matrix<float> TransitionMatrix
            {
                get
                {
                    return _transitionMatrix;
                }
            }

            public Matrix<float> State
            {
                get
                {
                    return _state;
                }
            }
            public Matrix<float> MeasurementNoise
            {
                get
                {
                    return _measurementNoise;
                }
            }
            public Matrix<float> ProcessNoise
            {
                get
                {
                    return _processNoise;
                }
            }
            public Matrix<float> ErrorCovariancePost
            {
                get
                {
                    return _errorCovariancePost;
                }
            }

            public SyntheticData()
            {
                _state = new Matrix<float>(2, 1);
                // start with random position and velocity
                //_state.SetRandNormal(new MCvScalar(0.0), new MCvScalar(1.0));
                _state[0, 0] = 0.0f;
                _state[1, 0] = 0.05f;

                _measurementNoise = new Matrix<float>(1, 1);
                _measurementNoise.SetIdentity(new MCvScalar(1.0e-2));
                _processNoise = new Matrix<float>(2, 2);
                _processNoise.SetIdentity(new MCvScalar(1.0e-5));
                _errorCovariancePost = new Matrix<float>(2, 2);
                _errorCovariancePost.SetIdentity();
                _transitionMatrix = new Matrix<float>(new float[,] { { 1, 1 }, { 0, 1 } }); // phi_t = phi_{t-1} + delta_phi
                _measurementMatrix = new Matrix<float>(new float[,] { { 1, 0 } });
                _measurementMatrix.SetIdentity(); //the measurement is [ phi ]
            }

            public Matrix<float> GetMeasurement()
            {
                Matrix<float> measurementNoise = new Matrix<float>(1, 1);
                measurementNoise.SetRandNormal(new MCvScalar(), new MCvScalar(Math.Sqrt(MeasurementNoise[0, 0])));
                return MeasurementMatrix * _state + measurementNoise;
            }

            public void GoToNextState()
            {
                Matrix<float> processNoise = new Matrix<float>(2, 1);
                processNoise.SetRandNormal(new MCvScalar(), new MCvScalar(Math.Sqrt(ProcessNoise[0, 0])));
                _state = TransitionMatrix * _state + processNoise;
            }
        }

        public void TestPoint()
        {
            Point p = new Point(1, 2);
            XDocument d = Emgu.Util.Toolbox.XmlSerialize<Point>(p);
            Point p2 = Emgu.Util.Toolbox.XmlDeserialize<Point>(d);
            Assert.AreEqual(p, p2);

            Size s = new Size(1, 2);
            d = Emgu.Util.Toolbox.XmlSerialize<Size>(s);
            Trace.WriteLine(d.ToString());

            Rectangle r = new Rectangle(1, 2, 3, 4);
            d = Emgu.Util.Toolbox.XmlSerialize<Rectangle>(r);
            Trace.WriteLine(d.ToString());
        }

        public void TestNegate()
        {
            Assert.AreEqual(~1, -2);
            Assert.AreEqual(~(-1), 0);
            //Assert.AreEqual(~3, 2);
        }

        public void TestReflection()
        {
            MethodInfo[] members = typeof(Emgu.CV.CvInvoke).GetMethods();
            foreach (MethodInfo member in members)
            {
                object[] pinvokeAtts = member.GetCustomAttributes(typeof(DllImportAttribute), false);
                if (pinvokeAtts.Length > 0) // if this is a PInvoke call
                {
                    DllImportAttribute att = pinvokeAtts[0] as DllImportAttribute;
                    Type[] types = Array.ConvertAll<ParameterInfo, Type>(member.GetParameters(), delegate (ParameterInfo p) { return p.ParameterType; });
                    //System.Reflection.Emit.DynamicMethod m = new System.Reflection.Emit.DynamicMethod("_"+member.Name, member.Attributes, member.CallingConvention, member.ReturnType, types, typeof(CvInvoke), true);
                }
            }
        }
        /*
        private class CustomDllImportAttribute : DllImportAttribute
        {
        }*/

        public void TestDiv2()
        {
            int total = 1000000000;
            int b = 0;
            Stopwatch watch = Stopwatch.StartNew();
            for (int i = 0; i < total; i++)
            {
                b += (i / 4) * 4;
                b /= 2;
            }
            watch.Stop();
            Trace.WriteLine(watch.ElapsedMilliseconds);

            b = 0;
            watch.Reset(); watch.Start();
            for (int i = 0; i < total; i++)
            {
                b += (i >> 2) << 2;
                b >>= 1;
            }
            Trace.WriteLine(watch.ElapsedMilliseconds);

        }

        /*
        public void TestCodeBookBGModel()
        {
           using (Capture capture = new Capture())
           using (BGCodeBookModel<Bgr> model = new BGCodeBookModel<Bgr>())
           {
              ImageViewer viewer = new ImageViewer();
              Image<Gray, byte> fgMask = capture.QueryFrame().Convert<Gray, Byte>();

              Application.Idle += delegate(Object sender, EventArgs args)
              {
                 Mat frame = capture.QueryFrame();
                 model.Apply(frame);
                 viewer.Image = model.ForegroundMask; 
              };
              viewer.ShowDialog();
           }
        }

        public void TestBlobTracking()
        {
           MCvFGDStatModelParams fgparam = new MCvFGDStatModelParams();
           fgparam.alpha1 = 0.1f;
           fgparam.alpha2 = 0.005f;
           fgparam.alpha3 = 0.1f;
           fgparam.delta = 2;
           fgparam.is_obj_without_holes = 1;
           fgparam.Lc = 32;
           fgparam.Lcc = 16;
           fgparam.minArea = 15;
           fgparam.N1c = 15;
           fgparam.N1cc = 25;
           fgparam.N2c = 25;
           fgparam.N2cc = 35;
           fgparam.perform_morphing = 0;
           fgparam.T = 0.9f;

           BlobTrackerAutoParam<Bgr> param = new BlobTrackerAutoParam<Bgr>();
           param.BlobDetector = new BlobDetector(Emgu.CV.CvEnum.BlobDetectorType.CC);
           param.FGDetector = new FGDetector<Bgr>(Emgu.CV.CvEnum.ForgroundDetectorType.Fgd, fgparam);
           param.BlobTracker = new BlobTracker(Emgu.CV.CvEnum.BLOBTRACKER_TYPE.MSFG);
           param.FGTrainFrames = 10;
           BlobTrackerAuto<Bgr> tracker = new BlobTrackerAuto<Bgr>(param);

           //MCvFont font = new MCvFont(Emgu.CV.CvEnum.FontFace.HersheySimplex, 1.0, 1.0);

           using(ImageViewer viewer = new ImageViewer())
           using (Capture capture = new Capture())
           {
              capture.ImageGrabbed += delegate(object sender, EventArgs e)
              {
                 tracker.Process(capture.RetrieveBgrFrame());

                 //Image<Bgr, Byte> img = capture.RetrieveBgrFrame();

                 Image<Bgr, Byte> img = tracker.ForegroundMask.Convert<Bgr, Byte>();
                 foreach (MCvBlob blob in tracker)
                 {
                    img.Draw((Rectangle)blob, new Bgr(255.0, 255.0, 255.0), 2);
                    img.Draw(blob.ID.ToString(), Point.Round(blob.Center), CvEnum.FontFace.HersheySimplex, 1.0, new Bgr(255.0, 255.0, 255.0));
                 }
                 viewer.Image = img;
              };
              capture.Start();
              viewer.ShowDialog();
           }
        }*/

        public void TestCvBlob()
        {
            //MCvFont font = new MCvFont(Emgu.CV.CvEnum.FontFace.HersheySimplex, 0.5, 0.5);
            using (CvTracks tracks = new CvTracks())
            using (ImageViewer viewer = new ImageViewer())
            using (VideoCapture capture = new VideoCapture())
            using (Mat fgMask = new Mat())
            {
                //BGStatModel<Bgr> bgModel = new BGStatModel<Bgr>(capture.QueryFrame(), Emgu.CV.CvEnum.BG_STAT_TYPE.GAUSSIAN_BG_MODEL);
                BackgroundSubtractorMOG2 bgModel = new BackgroundSubtractorMOG2(0, 0, true);
                //BackgroundSubstractorMOG bgModel = new BackgroundSubstractorMOG(0, 0, 0, 0);

                capture.ImageGrabbed += delegate (object sender, EventArgs e)
                {
                    Mat frame = new Mat();
                    capture.Retrieve(frame);
                    bgModel.Apply(frame, fgMask);

                    using (CvBlobDetector detector = new CvBlobDetector())
                    using (CvBlobs blobs = new CvBlobs())
                    {
                        detector.Detect(fgMask.ToImage<Gray, Byte>(), blobs);
                        blobs.FilterByArea(100, int.MaxValue);

                        tracks.Update(blobs, 20.0, 10, 0);

                        Image<Bgr, Byte> result = new Image<Bgr, byte>(frame.Size);

                        using (Image<Gray, Byte> blobMask = detector.DrawBlobsMask(blobs))
                        {
                            frame.CopyTo(result, blobMask);
                        }
                    //CvInvoke.cvCopy(frame, result, blobMask);

                    foreach (KeyValuePair<uint, CvTrack> pair in tracks)
                        {
                            if (pair.Value.Inactive == 0) //only draw the active tracks.
                        {
                                CvBlob b = blobs[pair.Value.BlobLabel];
                                Bgr color = detector.MeanColor(b, frame.ToImage<Bgr, Byte>());
                                result.Draw(pair.Key.ToString(), pair.Value.BoundingBox.Location, CvEnum.FontFace.HersheySimplex, 0.5, color);
                                result.Draw(pair.Value.BoundingBox, color, 2);
                                Point[] contour = b.GetContour();
                                result.Draw(contour, new Bgr(0, 0, 255), 1);
                            }
                        }

                        viewer.Image = frame.ToImage<Bgr, Byte>().ConcateVertical(fgMask.ToImage<Bgr, Byte>().ConcateHorizontal(result));
                    }
                };
                capture.Start();
                viewer.ShowDialog();
            }
        }

        /*
        public void TestPyrLK()
        {
           const int MAX_CORNERS = 500;
           Capture c = new Capture();
           ImageViewer viewer = new ImageViewer();
           Image<Gray, Byte> oldImage = null;
           Image<Gray, Byte> currentImage = null;
           Application.Idle += new EventHandler(delegate(object sender, EventArgs e)
           {
              if (oldImage == null)
              {
                 oldImage = c.QueryGrayFrame();
              }

              currentImage = c.QueryGrayFrame();
              Features2D.GFTTDetector detector = new Features2D.GFTTDetector(MAX_CORNERS, 0.05, 3, 3);

              //PointF[] features = oldImage.GoodFeaturesToTrack(MAX_CORNERS, 0.05, 3.0, 3, false, 0.04)[0];
              PointF[] shiftedFeatures;
              Byte[] status;
              float[] trackErrors;
              CvInvoke.CalcOpticalFlowPyrLK(oldImage, currentImage, features, new Size(9, 9), 3, new MCvTermCriteria(20, 0.05),
                 out shiftedFeatures, out status, out trackErrors);

              Image<Gray, Byte> displayImage = currentImage.Clone();
              for (int i = 0; i < features.Length; i++)
                 displayImage.Draw(new LineSegment2DF(features[i], shiftedFeatures[i]), new Gray(), 2);

              oldImage = currentImage;
              viewer.Image = displayImage;
           });
           viewer.ShowDialog();
        }*/


        public void TestPyrLKGPU()
        {
            if (!CudaInvoke.HasCuda)
                return;

            const int MAX_CORNERS = 500;
            VideoCapture c = new VideoCapture();
            ImageViewer viewer = new ImageViewer();
            GpuMat oldImage = null;
            GpuMat currentImage = null;
            using (CudaGoodFeaturesToTrackDetector detector = new CudaGoodFeaturesToTrackDetector(DepthType.Cv8U, 1, MAX_CORNERS, 0.05, 3.0, 3, false, 0.04))
            using (CudaDensePyrLKOpticalFlow flow = new CudaDensePyrLKOpticalFlow(new Size(21, 21), 3, 30, false))
            {
                Application.Idle += new EventHandler(delegate (object sender, EventArgs e)
                {
                    if (oldImage == null)
                    {
                        Mat bgrFrame = c.QueryFrame();
                        using (GpuMat oldBgrImage = new GpuMat(bgrFrame))
                        {
                            oldImage = new GpuMat();
                            CudaInvoke.CvtColor(oldBgrImage, oldImage, ColorConversion.Bgr2Gray);
                        }
                    }

                    using (Mat tmpFrame = c.QueryFrame())
                    using (GpuMat tmp = new GpuMat(tmpFrame))
                    {
                        currentImage = new GpuMat();
                        CudaInvoke.CvtColor(tmp, currentImage, ColorConversion.Bgr2Gray);
                    }
                    using (GpuMat f = new GpuMat())

                    using (GpuMat vertex = new GpuMat())
                    using (GpuMat colors = new GpuMat())
                    using (GpuMat corners = new GpuMat())
                    {
                        flow.Calc(oldImage, currentImage, f);

                    //CudaInvoke.CreateOpticalFlowNeedleMap(u, v, vertex, colors);
                    detector.Detect(oldImage, corners, null);
                    //GpuMat<float> detector.Detect(oldImage, null);
                    /*
                    //PointF[] features = oldImage.GoodFeaturesToTrack(MAX_CORNERS, 0.05, 3.0, 3, false, 0.04)[0];
                    PointF[] shiftedFeatures;
                    Byte[] status;
                    float[] trackErrors;
                    OpticalFlow.PyrLK(oldImage, currentImage, features, new Size(9, 9), 3, new MCvTermCriteria(20, 0.05),
                       out shiftedFeatures, out status, out trackErrors);
                    */

                        Mat displayImage = new Mat();
                        currentImage.Download(displayImage);

                    /*
                    for (int i = 0; i < features.Length; i++)
                       displayImage.Draw(new LineSegment2DF(features[i], shiftedFeatures[i]), new Gray(), 2);
                    */
                        oldImage = currentImage;
                        viewer.Image = displayImage;
                    }
                });
                viewer.ShowDialog();
            }
        }

        //[Test]
        public void TestKalman()
        {
            Image<Bgr, Byte> img = new Image<Bgr, byte>(400, 400);

            SyntheticData syntheticData = new SyntheticData();

            //Matrix<float> state = new Matrix<float>(new float[] { 0.0f, 0.0f}); //initial guess

            #region initialize Kalman filter
            KalmanFilter tracker = new KalmanFilter(2, 1, 0);
            syntheticData.TransitionMatrix.Mat.CopyTo(tracker.TransitionMatrix);
            syntheticData.MeasurementMatrix.Mat.CopyTo(tracker.MeasurementMatrix);

            syntheticData.ProcessNoise.Mat.CopyTo(tracker.ProcessNoiseCov);
            syntheticData.MeasurementNoise.Mat.CopyTo(tracker.MeasurementNoiseCov);
            syntheticData.ErrorCovariancePost.Mat.CopyTo(tracker.ErrorCovPost);
            tracker.StatePost.SetTo(new float[] { 0.0f, 0.0f });
            #endregion

            System.Converter<double, PointF> angleToPoint =
               delegate (double radianAngle)
               {
                   return new PointF(
                   (float)(img.Width / 2 + img.Width / 3 * Math.Cos(radianAngle)),
                   (float)(img.Height / 2 - img.Width / 3 * Math.Sin(radianAngle)));
               };

            Action<PointF, Bgr> drawCross =
              delegate (PointF point, Bgr color)
              {
                  img.Draw(new Cross2DF(point, 15, 15), color, 1);
              };

            ImageViewer viewer = new ImageViewer();
            System.Windows.Forms.Timer timer = new System.Windows.Forms.Timer();
            timer.Interval = 200;
            timer.Tick += new EventHandler(delegate (object sender, EventArgs e)
            {
                Matrix<float> measurement = syntheticData.GetMeasurement();
             // adjust Kalman filter state 
             tracker.Correct(measurement.Mat);

                tracker.Predict();

             #region draw the state, prediction and the measurement

             float[] correctedState = new float[2];
                float[] predictedState = new float[2];
                tracker.StatePost.CopyTo(correctedState);
                tracker.StatePre.CopyTo(predictedState);
                PointF statePoint = angleToPoint(correctedState[0]);
                PointF predictPoint = angleToPoint(predictedState[0]);
                PointF measurementPoint = angleToPoint(measurement[0, 0]);

                img.SetZero(); //clear the image
             drawCross(statePoint, new Bgr(Color.White)); //draw current state in White
             drawCross(measurementPoint, new Bgr(Color.Red)); //draw the measurement in Red
             drawCross(predictPoint, new Bgr(Color.Green)); //draw the prediction (the next state) in green 
             img.Draw(new LineSegment2DF(statePoint, predictPoint), new Bgr(Color.Magenta), 1); //Draw a line between the current position and prediction of next position 

             //Trace.WriteLine(String.Format("Velocity: {0}", tracker.CorrectedState[1, 0]));
             #endregion

             syntheticData.GoToNextState();

                viewer.Image = img;
            });
            timer.Start();
            viewer.Disposed += delegate (Object sender, EventArgs e) { timer.Stop(); };
            viewer.Text = "Actual State: White; Measurement: Red; Prediction: Green";
            viewer.ShowDialog();
        }

        //TODO: Fix this
        /*
        [Test]
        public void VideoRetina()
        {
           ImageViewer v = new ImageViewer();

           using (VideoCapture capture = new VideoCapture(0))
           using (Bioinspired.Retina retina = new Bioinspired.Retina(new Size(capture.Width, capture.Height), true, Retina.ColorSamplingMethod.ColorBayer, false, 1.0, 10.0))
           {
              Bioinspired.Retina.RetinaParameters p = retina.Parameters;
              Bioinspired.Retina.IplMagnoParameters iplP = p.IplMagno;
              float oldval = iplP.ParasolCells_k;
              iplP.ParasolCells_k += 0.01f;
              iplP.NormaliseOutput = false;
              p.IplMagno = iplP;
              retina.Parameters = p;
              float newval = retina.Parameters.IplMagno.ParasolCells_k;

              Assert.AreEqual(newval, oldval + 0.01f);

              Application.Idle += delegate(Object sender, EventArgs e)
              {
                 Image<Bgr, byte> img = capture.QueryFrame();
                 retina.Run(img);

                 v.Image = img.ConcateVertical(retina.GetParvo().ConcateHorizontal(retina.GetMagno().Convert<Bgr, byte>()));

              };
              v.ShowDialog();

           }
        }
        
        public void TestStereo()
        {
           using (ImageViewer v = new ImageViewer())
           using (Capture cl = new Capture(0))
           using (Capture cr = new Capture(1))
           //using (GpuStereoConstantSpaceBP stereo = new GpuStereoConstantSpaceBP(128, 8, 4, 4))
           using (CudaStereoBM stereo = new CudaStereoBM(64, 19))
           using (CudaImage<Bgr, byte> leftGpu = new CudaImage<Bgr, byte>(new Size(cl.Width, cl.Height)))
           using (CudaImage<Bgr, byte> rightGpu = new CudaImage<Bgr, byte>(new Size(cr.Width, cr.Height)))
           using (CudaImage<Gray, byte> gpuDisparity = new CudaImage<Gray, byte>(leftGpu.Size))
           using (Image<Gray, Byte> disparity = new Image<Gray, byte>(gpuDisparity.Size))
           {
              Application.Idle +=
              ((Object sender, EventArgs e) =>
              {
                 cl.Grab();
                 cr.Grab();
                 using (Image<Bgr, Byte> left = cl.RetrieveBgrFrame())
                 using (Image<Bgr, Byte> right = cr.RetrieveBgrFrame())
                 {
                    leftGpu.Upload(left);
                    rightGpu.Upload(right);
                    using (CudaImage<Gray, byte> leftGray = leftGpu.Convert<Gray, byte>())
                    using (CudaImage<Gray, byte> rightGray = rightGpu.Convert<Gray, byte>())
                    {
                       stereo.FindStereoCorrespondence(leftGray, rightGray, gpuDisparity, null);
                       gpuDisparity.Download(disparity);
                       Image<Bgr, Byte> img = left.ConcateHorizontal(right);
                       img = img.ConcateVertical(disparity.Convert<Bgr, byte>());
                       //Image<Bgr, Byte> img = c0.RetrieveBgrFrame();
                       v.Image = img;
                    }
                 }
              });

              v.ShowDialog();
           }
        }*/
    }
}
