//----------------------------------------------------------------------------
//  Copyright (C) 2004-2026 by EMGU Corporation. All rights reserved.
//----------------------------------------------------------------------------
using System;
using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Hfs;

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
    public class AutoTestHfs
    {
        [Test]
        public void TestHfsSegmentCpu()
        {
            using (Mat image = EmguAssert.LoadMat("lena.jpg", ImreadModes.ColorBgr))
            using (HfsSegment hfs = new HfsSegment(image.Height, image.Width))
            using (Mat segmented = hfs.PerformSegmentCpu(image, true))
            {
                EmguAssert.IsTrue(!segmented.IsEmpty, "HfsSegment.PerformSegmentCpu output should not be empty");
                EmguAssert.AreEqual(image.Size, segmented.Size);
            }
        }

        [Test]
        public void TestHfsSegmentCpuIndexMap()
        {
            using (Mat image = EmguAssert.LoadMat("lena.jpg", ImreadModes.ColorBgr))
            using (HfsSegment hfs = new HfsSegment(image.Height, image.Width))
            using (Mat indexMap = hfs.PerformSegmentCpu(image, false))
            {
                EmguAssert.IsTrue(!indexMap.IsEmpty, "HfsSegment.PerformSegmentCpu index-map output should not be empty");
                EmguAssert.AreEqual(image.Size, indexMap.Size);
                EmguAssert.AreEqual(DepthType.Cv16U, indexMap.Depth);
            }
        }
    }
}
