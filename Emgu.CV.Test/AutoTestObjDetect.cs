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
using Emgu.CV.Legacy;
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
        public void TestObjectnessBING()
        {
            using (Emgu.CV.Saliency.ObjectnessBING objectnessBING = new Saliency.ObjectnessBING())
            {
                //objectnessBING.SetTrainingPath("C:\\tmp");
            }
        }

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
        public void TestTrackerMedianFlow()
        {
            using (Mat box = EmguAssert.LoadMat("box.png"))
            using (MultiTracker multiTracker = new MultiTracker())
            using (TrackerMedianFlow medianFlowTracker = new TrackerMedianFlow(
                10,
                new Size(3, 3),
                5,
                new MCvTermCriteria(20, 0.3),
                new Size(30, 30), 10.0))
            {
                bool success = multiTracker.Add(medianFlowTracker, box, new Rectangle(new Point(), box.Size));
            }
                
        }
    }
}
