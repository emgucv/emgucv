//----------------------------------------------------------------------------
//  Copyright (C) 2004-2026 by EMGU Corporation. All rights reserved.
//----------------------------------------------------------------------------
using System.Drawing;
using Emgu.CV;
using Emgu.CV.Shape;
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
    public class AutoTestShape
    {
        [Test]
        public void TestShapeDistanceExtractor()
        {
            using (HistogramCostExtractor comparer = new ChiHistogramCostExtractor())
            using (ThinPlateSplineShapeTransformer transformer = new ThinPlateSplineShapeTransformer())
            using (ShapeContextDistanceExtractor extractor = new ShapeContextDistanceExtractor(comparer, transformer))
            using (HausdorffDistanceExtractor extractor2 = new HausdorffDistanceExtractor())
            {
                Point[] shape1 = new Point[] { new Point(0, 0), new Point(480, 0), new Point(480, 360), new Point(0, 360) };
                Point[] shape2 = new Point[] { new Point(0, 0), new Point(480, 0), new Point(500, 240), new Point(480, 360), new Point(0, 360) };

                float distance2 = extractor2.ComputeDistance(shape1, shape2);
                float distance = extractor.ComputeDistance(shape1, shape2);
            }
        }
    }
}
