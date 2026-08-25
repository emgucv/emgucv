//----------------------------------------------------------------------------
//  Copyright (C) 2004-2026 by EMGU Corporation. All rights reserved.
//----------------------------------------------------------------------------
using System;
using System.Drawing;
using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Fuzzy;
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
    public class AutoTestFuzzy
    {
        [Test]
        public void TestCreateKernel()
        {
            using (Mat kernel = new Mat())
            {
                FuzzyInvoke.CreateKernel(FuzzyInvoke.Function.Linear, 2, kernel, 1);
                EmguAssert.IsTrue(!kernel.IsEmpty, "CreateKernel should produce a non-empty kernel");
            }
        }

        [Test]
        public void TestFilter()
        {
            using (Mat image = EmguAssert.LoadMat("lena.jpg"))
            using (Mat kernel = new Mat())
            using (Mat output = new Mat())
            {
                FuzzyInvoke.CreateKernel(FuzzyInvoke.Function.Linear, 2, kernel, image.NumberOfChannels);
                FuzzyInvoke.Filter(image, kernel, output);

                EmguAssert.IsTrue(!output.IsEmpty, "Filter output should not be empty");
                EmguAssert.AreEqual(image.Size, output.Size);
            }
        }

        [Test]
        public void TestInpaint()
        {
            using (Mat image = EmguAssert.LoadMat("lena.jpg"))
            using (Mat mask = new Mat(image.Size, DepthType.Cv8U, 1))
            using (Mat output = new Mat())
            {
                mask.SetTo(new MCvScalar(255));
                Rectangle unknownRegion = new Rectangle(
                    image.Width / 4, image.Height / 4, image.Width / 4, image.Height / 4);
                CvInvoke.Rectangle(mask, unknownRegion, new MCvScalar(0), -1);

                FuzzyInvoke.Inpaint(image, mask, output, 2, FuzzyInvoke.Function.Linear, FuzzyInvoke.InpaintAlgorithm.OneStep);

                EmguAssert.IsTrue(!output.IsEmpty, "Inpaint output should not be empty");
                EmguAssert.AreEqual(image.Size, output.Size);
            }
        }
    }
}
