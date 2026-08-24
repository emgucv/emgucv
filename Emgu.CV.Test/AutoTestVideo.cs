//----------------------------------------------------------------------------
//  Copyright (C) 2004-2026 by EMGU Corporation. All rights reserved.
//----------------------------------------------------------------------------
using System;
using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Structure;

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
    public class AutoTestVideo
    {
        [Test]
        public static void TestBackgroundSubtractorMOG2()
        {
            //ImageViewer viewer = new ImageViewer();
            using (VideoCapture capture = new VideoCapture("tree.avi"))
            using (BackgroundSubtractorMOG2 subtractor = new BackgroundSubtractorMOG2())
            using (Mat frame = new Mat())
            using (Mat fgMask = new Mat())
            {
                int frameCount = 0;
                while (capture.Grab())
                {
                    capture.Retrieve(frame);
                    subtractor.Apply(frame, fgMask);
                    frameCount++;
                }
                EmguAssert.IsTrue(frameCount > 0, "BackgroundSubtractorMOG2 did not return any frames");
            }
        }
    }
}
