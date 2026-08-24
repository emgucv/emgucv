//----------------------------------------------------------------------------
//  Copyright (C) 2004-2026 by EMGU Corporation. All rights reserved.
//----------------------------------------------------------------------------
using System;
using System.Drawing;
using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Structure;
using Emgu.CV.Util;

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
    public class AutoTestGapi
    {
        /// <summary>
        /// Some Emgu CV runtime builds (e.g. Ubuntu builds without the optional "ade" dependency
        /// available at configure time) are compiled without G-API support; calling into any
        /// G-API function on such a build throws a CvException. Skip the G-API tests in that case,
        /// mirroring the CudaInvoke.HasCuda pattern used for the optional Cuda module.
        /// </summary>
        private static bool IsGapiSupported()
        {
            try
            {
                using (GMat m = new GMat())
                {
                }
                return true;
            }
            catch (CvException)
            {
                return false;
            }
        }

        [Test]
        public void TestGComputationUnaryMat()
        {
            if (!IsGapiSupported())
                return;

            using (GMat gIn = new GMat())
            using (GMat gOut = GapiInvoke.BitwiseNot(gIn))
            using (GComputation comp = new GComputation(gIn, gOut))
            using (Mat input = new Mat(new Size(64, 48), DepthType.Cv8U, 1))
            using (Mat output = new Mat())
            using (Mat expected = new Mat())
            {
                CvInvoke.Randu(input, new MCvScalar(0), new MCvScalar(255));

                comp.Apply(input, output);

                CvInvoke.BitwiseNot(input, expected);

                EmguAssert.IsFalse(output.IsEmpty);
                EmguAssert.AreEqual(input.Size, output.Size);

                using (Mat diff = new Mat())
                {
                    CvInvoke.AbsDiff(expected, output, diff);
                    EmguAssert.AreEqual(0, CvInvoke.CountNonZero(diff));
                }
            }
        }

        [Test]
        public void TestGComputationBinaryMat()
        {
            if (!IsGapiSupported())
                return;

            using (GMat gIn1 = new GMat())
            using (GMat gIn2 = new GMat())
            using (GMat gOut = GapiInvoke.Add(gIn1, gIn2))
            using (GComputation comp = new GComputation(gIn1, gIn2, gOut))
            using (Mat input1 = new Mat(new Size(64, 48), DepthType.Cv8U, 1))
            using (Mat input2 = new Mat(new Size(64, 48), DepthType.Cv8U, 1))
            using (Mat output = new Mat())
            using (Mat expected = new Mat())
            {
                CvInvoke.Randu(input1, new MCvScalar(0), new MCvScalar(128));
                CvInvoke.Randu(input2, new MCvScalar(0), new MCvScalar(128));

                comp.Apply(input1, input2, output);

                CvInvoke.Add(input1, input2, expected);

                using (Mat diff = new Mat())
                {
                    CvInvoke.AbsDiff(expected, output, diff);
                    EmguAssert.AreEqual(0, CvInvoke.CountNonZero(diff));
                }
            }
        }

        [Test]
        public void TestGComputationUnaryScalar()
        {
            if (!IsGapiSupported())
                return;

            using (GMat gIn = new GMat())
            using (GScalar gOut = GapiInvoke.Mean(gIn))
            using (GComputation comp = new GComputation(gIn, gOut))
            using (Mat input = new Mat(new Size(64, 48), DepthType.Cv8U, 1))
            {
                input.SetTo(new MCvScalar(100));

                MCvScalar result = comp.ApplyS(input);

                EmguAssert.IsTrue(
                    Math.Abs(result.V0 - 100.0) < 1.0,
                    String.Format("Expected mean close to 100.0, got {0}", result.V0));
            }
        }

        [Test]
        public void TestGComputationBinaryScalar()
        {
            if (!IsGapiSupported())
                return;

            using (GMat gIn1 = new GMat())
            using (GMat gIn2 = new GMat())
            using (GMat gAdd = GapiInvoke.Add(gIn1, gIn2))
            using (GScalar gOut = GapiInvoke.Mean(gAdd))
            using (GComputation comp = new GComputation(gIn1, gIn2, gOut))
            using (Mat input1 = new Mat(new Size(64, 48), DepthType.Cv8U, 1))
            using (Mat input2 = new Mat(new Size(64, 48), DepthType.Cv8U, 1))
            {
                input1.SetTo(new MCvScalar(40));
                input2.SetTo(new MCvScalar(60));

                MCvScalar result = comp.ApplyS(input1, input2);

                EmguAssert.IsTrue(
                    Math.Abs(result.V0 - 100.0) < 1.0,
                    String.Format("Expected mean close to 100.0, got {0}", result.V0));
            }
        }

        [Test]
        public void TestGComputationMultipleIO()
        {
            if (!IsGapiSupported())
                return;

            using (GMat gIn1 = new GMat())
            using (GMat gIn2 = new GMat())
            using (GMat gAdd = GapiInvoke.Add(gIn1, gIn2))
            using (GMat gSub = GapiInvoke.Sub(gIn1, gIn2))
            using (VectorOfGMat ins = new VectorOfGMat(gIn1, gIn2))
            using (VectorOfGMat outs = new VectorOfGMat(gAdd, gSub))
            using (GComputation comp = new GComputation(ins, outs))
            using (Mat input1 = new Mat(new Size(64, 48), DepthType.Cv8U, 1))
            using (Mat input2 = new Mat(new Size(64, 48), DepthType.Cv8U, 1))
            using (VectorOfMat inputVec = new VectorOfMat(input1, input2))
            using (VectorOfMat outputVec = new VectorOfMat())
            using (Mat expectedAdd = new Mat())
            using (Mat expectedSub = new Mat())
            {
                CvInvoke.Randu(input1, new MCvScalar(0), new MCvScalar(128));
                CvInvoke.Randu(input2, new MCvScalar(0), new MCvScalar(128));

                comp.Apply(inputVec, outputVec);

                EmguAssert.AreEqual(2, outputVec.Size);

                CvInvoke.Add(input1, input2, expectedAdd);
                CvInvoke.Subtract(input1, input2, expectedSub);

                using (Mat diffAdd = new Mat())
                using (Mat diffSub = new Mat())
                using (Mat outAdd = outputVec[0])
                using (Mat outSub = outputVec[1])
                {
                    CvInvoke.AbsDiff(expectedAdd, outAdd, diffAdd);
                    EmguAssert.AreEqual(0, CvInvoke.CountNonZero(diffAdd));

                    CvInvoke.AbsDiff(expectedSub, outSub, diffSub);
                    EmguAssert.AreEqual(0, CvInvoke.CountNonZero(diffSub));
                }
            }
        }
    }
}
