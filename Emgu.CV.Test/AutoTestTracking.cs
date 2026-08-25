//----------------------------------------------------------------------------
//  Copyright (C) 2004-2026 by EMGU Corporation. All rights reserved.
//----------------------------------------------------------------------------
using System;
using System.Drawing;
using Emgu.CV;
using Emgu.CV.Legacy;
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
    public class AutoTestTracking
    {
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
