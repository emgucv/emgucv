//----------------------------------------------------------------------------
//  Copyright (C) 2004-2026 by EMGU Corporation. All rights reserved.
//----------------------------------------------------------------------------
using System;
using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.LineDescriptor;

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
    public class AutoTestLineDescriptor
    {
        [Test]
        public void TestBinaryDescriptor()
        {
            using (Mat image = EmguAssert.LoadMat("lena.jpg", ImreadModes.Grayscale))
            using (BinaryDescriptor bd = new BinaryDescriptor())
            using (VectorOfKeyLine keylines = new VectorOfKeyLine())
            using (Mat descriptors = new Mat())
            {
                bd.Detect(image, keylines);
                EmguAssert.IsTrue(keylines.Size > 0, "BinaryDescriptor.Detect should find at least one line");

                bd.Compute(image, keylines, descriptors);
                EmguAssert.IsTrue(!descriptors.IsEmpty, "BinaryDescriptor.Compute output should not be empty");
            }
        }

        [Test]
        public void TestLSDDetector()
        {
            using (Mat image = EmguAssert.LoadMat("lena.jpg", ImreadModes.Grayscale))
            using (LSDDetector detector = new LSDDetector())
            using (VectorOfKeyLine keylines = new VectorOfKeyLine())
            {
                detector.Detect(image, keylines, 2, 1);
                EmguAssert.IsTrue(keylines.Size > 0, "LSDDetector.Detect should find at least one line");
            }
        }
    }
}
