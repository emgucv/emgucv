//----------------------------------------------------------------------------
//  Copyright (C) 2004-2026 by EMGU Corporation. All rights reserved.
//----------------------------------------------------------------------------
using System;
using System.Drawing;
using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Reg;
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
    public class AutoTestReg
    {
        [Test]
        public void TestMapShiftWarp()
        {
            using (Mat image = EmguAssert.LoadMat("lena.jpg", ImreadModes.Grayscale))
            using (Mat warped = new Mat())
            using (MapShift mapShift = new MapShift(new MCvPoint2D64f(3.0, 5.0)))
            {
                mapShift.Warp(image, warped);

                EmguAssert.IsTrue(!warped.IsEmpty, "MapShift.Warp output should not be empty");
                EmguAssert.AreEqual(image.Size, warped.Size);
            }
        }

        [Test]
        public void TestMapperGradShiftRecovery()
        {
            using (Mat image = EmguAssert.LoadMat("lena.jpg", ImreadModes.Grayscale))
            using (Mat shifted = new Mat())
            using (Mat translationMatrix = new Mat(2, 3, DepthType.Cv64F, 1))
            {
                double[] translationData = new double[] { 1, 0, 4, 0, 1, 2 };
                System.Runtime.InteropServices.Marshal.Copy(translationData, 0, translationMatrix.DataPointer, translationData.Length);
                CvInvoke.WarpAffine(image, shifted, translationMatrix, image.Size);

                using (MapperGradShift baseMapper = new MapperGradShift())
                using (MapperPyramid pyramidMapper = new MapperPyramid(baseMapper))
                using (Map map = pyramidMapper.Calculate(image, shifted))
                using (Mat recovered = new Mat())
                {
                    map.Warp(image, recovered);

                    EmguAssert.IsTrue(!recovered.IsEmpty, "MapperGradShift recovery output should not be empty");
                    EmguAssert.AreEqual(image.Size, recovered.Size);
                }
            }
        }
    }
}
