//----------------------------------------------------------------------------
//  Copyright (C) 2004-2026 by EMGU Corporation. All rights reserved.       
//----------------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Dpm;
using Emgu.CV.Mcc;
using Emgu.CV.Structure;
using Emgu.CV.Util;

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
    public class AutoTestObjDetect
    {
        [Test]
        public void TestHOG1()
        {
            using (HOGDescriptor hog = new HOGDescriptor())
            using (Mat image = EmguAssert.LoadMat("pedestrian.png", ImreadModes.ColorBgr))
            {
                float[] pedestrianDescriptor = HOGDescriptor.GetDefaultPeopleDetector();
                hog.SetSVMDetector(pedestrianDescriptor);

                Stopwatch watch = Stopwatch.StartNew();
                MCvObjectDetection[] rects = hog.DetectMultiScale(image);
                watch.Stop();

                EmguAssert.AreEqual(1, rects.Length);

                foreach (MCvObjectDetection rect in rects)
                    CvInvoke.Rectangle(image, Rectangle.Empty, new MCvScalar(0, 0, 255), 1);
                    //image.Draw(rect.Rect, new Bgr(0, 0, 255), 1);
                EmguAssert.WriteLine(String.Format("HOG detection time: {0} ms", watch.ElapsedMilliseconds));

                //Emgu.CV.WinForms.ImageViewer.Show(image, String.Format("Detection Time: {0}ms", watch.ElapsedMilliseconds));
            }
        }

        [Test]
        public void TestHOG2()
        {
            using (HOGDescriptor hog = new HOGDescriptor())
            using (Mat image = EmguAssert.LoadMat("lena.jpg", ImreadModes.ColorBgr))
            {
                float[] pedestrianDescriptor = HOGDescriptor.GetDefaultPeopleDetector();
                hog.SetSVMDetector(pedestrianDescriptor);

                Stopwatch watch = Stopwatch.StartNew();
                MCvObjectDetection[] rects = hog.DetectMultiScale(image);
                watch.Stop();

                EmguAssert.AreEqual(0, rects.Length);
                foreach (MCvObjectDetection rect in rects)
                    CvInvoke.Rectangle(image, Rectangle.Empty, new MCvScalar(0,0,255), 1);
                    //image.Draw(rect.Rect, new Bgr(0, 0, 255), 1);
                EmguAssert.WriteLine(String.Format("HOG detection time: {0} ms", watch.ElapsedMilliseconds));

                //ImageViewer.Show(image, String.Format("Detection Time: {0}ms", watch.ElapsedMilliseconds));
            }
        }

        [Test]
        public void TestDPM()
        {
            Mat m = EmguAssert.LoadMat("pedestrian.png");
            DpmDetector detector = new DpmDetector(new String[] { "inriaperson.xml" }, new string[] { "person" });
            ObjectDetection[] result = detector.Detect(m);

        }

        [Test]
        public void TestQRCode()
        {
            using (QRCodeEncoder encoder = new QRCodeEncoder())
            using (Mat qrcodeImg = new Mat())
            {
                encoder.Encode("https://www.emgu.com", qrcodeImg);
            }

            using (Mat m = EmguAssert.LoadMat("link_github_ocv.jpg"))
            using (QRCodeDetector detector = new QRCodeDetector())
            using (VectorOfPoint pts = new VectorOfPoint())
            {
                if (detector.Detect(m, pts))
                {
                    String text = detector.Decode(m, pts);
                }
            }

        }

        [Test]
        public void TestMcc()
        {
            using (Mat image = EmguAssert.LoadMat("MCC24.png"))
            using (CCheckerDetector detector = new CCheckerDetector())
            {
                if (detector.Process(image))
                {
                    using (CChecker checker = detector.BestColorChecker)
                    {
                        detector.Draw(checker, image, new MCvScalar(0, 255, 0), 1);
                    }
                        /*
                    using (CCheckerDraw drawer = new CCheckerDraw(checker, new MCvScalar(0, 255, 0), 1))
                    {
                        drawer.Draw(image);
                        //image.Save("c:\\tmp.out.png");
                    }
                    //using (Mat img = new Mat(new Size(480, 320), DepthType.Cv8U, 3))
                    //{
                    //    drawer.Draw(img);
                    //}*/
                }
            }

            using (CChecker c = new CChecker())
            {
                PointF p = c.Center;
            }

        }
    }
}
