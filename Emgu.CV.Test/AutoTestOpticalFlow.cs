//----------------------------------------------------------------------------
//  Copyright (C) 2004-2026 by EMGU Corporation. All rights reserved.
//----------------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Runtime.InteropServices;
using Emgu.CV;
using Emgu.CV.CvEnum;
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
    public class AutoTestOpticalFlow
    {
        /// <summary>
        /// Prepare synthetic image pair for optical flow testing.
        /// </summary>
        public static Mat[] OpticalFlowImage()
        {
            using (Mat randomObj = new Mat(new Size(50, 50), DepthType.Cv8U, 1))
            {
                CvInvoke.Randu(randomObj, new MCvScalar(), new MCvScalar(255));

                Mat prevImg = new Mat(new Size(300, 200), DepthType.Cv8U, 1);
                Rectangle objectLocation = new Rectangle(75, 75, 50, 50);
                using (Mat roi = new Mat(prevImg, objectLocation))
                {
                    randomObj.CopyTo(roi);
                }

                Mat currImg = new Mat(new Size(300, 200), DepthType.Cv8U, 1);
                objectLocation.Offset(2, 3);
                using (Mat roi = new Mat(currImg, objectLocation))
                {
                    randomObj.CopyTo(roi);
                }

                return new Mat[] { prevImg, currImg };
            }
        }

        [Test]
        public void TestPCAFlow()
        {
            Mat[] images = OpticalFlowImage();
            using (Mat flow = new Mat())
            using (Emgu.CV.OpticalFlowPCAFlow pcaFlow = new OpticalFlowPCAFlow())
            {
                pcaFlow.Calc(images[0], images[1], flow);
                foreach (Mat image in images)
                {
                    image.Dispose();
                }
            }
        }

        [Test]
        public void TestDISOpticalFlow()
        {
            Mat[] images = OpticalFlowImage();
            using (Mat flow = new Mat())
            using (Emgu.CV.DISOpticalFlow disFlow = new DISOpticalFlow())
            {
                disFlow.Calc(images[0], images[1], flow);
                foreach (Mat image in images)
                {
                    image.Dispose();
                }
            }
        }

        [Test]
        public void TestOpticalFlowFarneback()
        {
            Mat[] images = OpticalFlowImage();
            Mat flow = new Mat();
            CvInvoke.CalcOpticalFlowFarneback(images[0], images[1], flow, 0.5, 3, 5, 20, 7, 1.5, Emgu.CV.CvEnum.OpticalflowFarnebackFlag.Default);
        }

        [Test]
        public void TestOpticalFlowLK()
        {
            Mat[] images = OpticalFlowImage();

            PointF[] prevFeature = new PointF[] { new PointF(100f, 100f) };

            PointF[] currFeature;
            Byte[] status;
            float[] trackError;

            Stopwatch watch = Stopwatch.StartNew();

            CvInvoke.CalcOpticalFlowPyrLK(
               images[0], images[1], prevFeature, new Size(10, 10), 3, new MCvTermCriteria(10, 0.01),
               out currFeature, out status, out trackError);
            watch.Stop();
            EmguAssert.WriteLine(String.Format(
               "prev: ({0}, {1}); curr: ({2}, {3}); \r\nTime: {4} milliseconds",
               prevFeature[0].X, prevFeature[0].Y,
               currFeature[0].X, currFeature[0].Y,
               watch.ElapsedMilliseconds));
        }

        [Test]
        public void TestOpticalFlowDualTVL1()
        {
            Mat[] images = OpticalFlowImage();
            Mat result = new Mat();

            Stopwatch watch = Stopwatch.StartNew();
            DualTVL1OpticalFlow flow = new DualTVL1OpticalFlow();

            flow.Calc(images[0], images[1], result);

            watch.Stop();
            EmguAssert.WriteLine(String.Format(
               "Time: {0} milliseconds",
               watch.ElapsedMilliseconds));
        }
    }
}
