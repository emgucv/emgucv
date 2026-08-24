//----------------------------------------------------------------------------
//  Copyright (C) 2004-2026 by EMGU Corporation. All rights reserved.
//----------------------------------------------------------------------------
using System;
using System.Drawing;
using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.ImgHash;
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
    public class AutoTestImgHash
    {
        private static double CompareHash(ImgHashBase imgHash, Mat m1, Mat m2)
        {
            Mat hash1 = new Mat();
            Mat hash2 = new Mat();
            imgHash.Compute(m1, hash1);
            imgHash.Compute(m2, hash2);
            return imgHash.Compare(hash1, hash2);
        }

        [Test]
        public void TestHash()
        {
            Mat m1 = EmguAssert.LoadMat("lena.jpg");
            Mat m2 = new Mat();
            CvInvoke.GaussianBlur(m1, m2, new Size(3, 3), 1);

            using (AverageHash averageHash = new AverageHash())
            {
                double diff = CompareHash(averageHash, m1, m2);
            }

            using (BlockMeanHash bmh = new BlockMeanHash())
            {
                double diff = CompareHash(bmh, m1, m2);
            }

            using (ColorMomentHash cmh = new ColorMomentHash())
            {
                double diff = CompareHash(cmh, m1, m2);
            }

            using (MarrHildrethHash cmh = new MarrHildrethHash())
            {
                double diff = CompareHash(cmh, m1, m2);
            }

            using (PHash cmh = new PHash())
            {
                double diff = CompareHash(cmh, m1, m2);
            }

            using (RadialVarianceHash cmh = new RadialVarianceHash())
            {
                double diff = CompareHash(cmh, m1, m2);
            }
        }
    }
}
