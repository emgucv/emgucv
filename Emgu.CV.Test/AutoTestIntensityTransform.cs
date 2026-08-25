//----------------------------------------------------------------------------
//  Copyright (C) 2004-2026 by EMGU Corporation. All rights reserved.
//----------------------------------------------------------------------------
using System;
using Emgu.CV;
using Emgu.CV.CvEnum;

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
    public class AutoTestIntensityTransform
    {
        [Test]
        public static void TestIntensityTransform()
        {
            Mat m = new Mat("lena.jpg", ImreadModes.ColorBgr);
            Mat bimef = new Mat();
            Mat autoScaling = new Mat();
            Mat gamma = new Mat();
            Mat contrastStretch = new Mat();
            Mat log = new Mat();
            Emgu.CV.IntensityTransform.IntensityTransformInvoke.BIMEF(m, bimef);
            Emgu.CV.IntensityTransform.IntensityTransformInvoke.Autoscaling(m, autoScaling);
            Emgu.CV.IntensityTransform.IntensityTransformInvoke.GammaCorrection(m, gamma, 2.0f);
            Emgu.CV.IntensityTransform.IntensityTransformInvoke.ContrastStretching(m, contrastStretch, 0, 0, 200, 200);
            Emgu.CV.IntensityTransform.IntensityTransformInvoke.LogTransform(m, log);
        }
    }
}
