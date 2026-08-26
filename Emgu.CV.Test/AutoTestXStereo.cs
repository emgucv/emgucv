//----------------------------------------------------------------------------
//  Copyright (C) 2004-2026 by EMGU Corporation. All rights reserved.
//----------------------------------------------------------------------------
using System;
using System.Drawing;
using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Stereo;
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
    public class AutoTestXStereo
    {
        [Test]
        public void TestQuasiDenseStereo()
        {
            Size imgSize = new Size(320, 240);

            // Synthetic stereo pair: a random-noise "left" texture and a "right" image
            // that is the same texture shifted horizontally, simulating disparity.
            using (Mat left = new Mat(imgSize, DepthType.Cv8U, 1))
            using (Mat right = new Mat(imgSize, DepthType.Cv8U, 1))
            {
                CvInvoke.Randu(left, new MCvScalar(0), new MCvScalar(255));

                using (Mat warpMat = new Mat(2, 3, DepthType.Cv64F, 1))
                {
                    double[] warpData = new double[] { 1, 0, 8, 0, 1, 0 };
                    System.Runtime.InteropServices.Marshal.Copy(warpData, 0, warpMat.DataPointer, warpData.Length);
                    CvInvoke.WarpAffine(left, right, warpMat, imgSize, Inter.Nearest, Warp.Default, BorderType.Replicate);
                }

                using (QuasiDenseStereo stereo = new QuasiDenseStereo(imgSize))
                {
                    stereo.Process(left, right);

                    using (Mat disparity = stereo.GetDisparity())
                    {
                        EmguAssert.IsTrue(!disparity.IsEmpty, "GetDisparity output should not be empty");
                        EmguAssert.AreEqual(imgSize, disparity.Size);
                        EmguAssert.AreEqual(DepthType.Cv32F, disparity.Depth);
                    }
                }
            }
        }
    }
}
