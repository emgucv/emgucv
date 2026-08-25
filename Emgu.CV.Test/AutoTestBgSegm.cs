//----------------------------------------------------------------------------
//  Copyright (C) 2004-2026 by EMGU Corporation. All rights reserved.
//----------------------------------------------------------------------------
using System;
using Emgu.CV;
using Emgu.CV.BgSegm;

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
    public class AutoTestBgSegm
    {
        private static void RunBackgroundSubtractor(IBackgroundSubtractor subtractor)
        {
            using (VideoCapture capture = new VideoCapture("tree.avi"))
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
                EmguAssert.IsTrue(frameCount > 0, "Background subtractor did not process any frames");
            }
        }

        [Test]
        public void TestBackgroundSubtractorCNT()
        {
            using (BackgroundSubtractorCNT subtractor = new BackgroundSubtractorCNT())
                RunBackgroundSubtractor(subtractor);
        }

        [Test]
        public void TestBackgroundSubtractorGMG()
        {
            using (BackgroundSubtractorGMG subtractor = new BackgroundSubtractorGMG(120, 0.8))
                RunBackgroundSubtractor(subtractor);
        }

        [Test]
        public void TestBackgroundSubtractorGSOC()
        {
            using (BackgroundSubtractorGSOC subtractor = new BackgroundSubtractorGSOC())
                RunBackgroundSubtractor(subtractor);
        }

        [Test]
        public void TestBackgroundSubtractorLSBP()
        {
            using (BackgroundSubtractorLSBP subtractor = new BackgroundSubtractorLSBP())
                RunBackgroundSubtractor(subtractor);
        }

        [Test]
        public void TestBackgroundSubtractorMOG()
        {
            using (BackgroundSubtractorMOG subtractor = new BackgroundSubtractorMOG())
                RunBackgroundSubtractor(subtractor);
        }
    }
}
