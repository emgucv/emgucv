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
    public class AutoTestSignal
    {
        [Test]
        public static void TestResampleSignal()
        {
            int inFreq = 44100;
            int outFreq = 22050;
            int inputLength = 4410; // 0.1 seconds of samples at inFreq

            // Build a 440 Hz sine wave as a 1-row CV_32FC1 Mat
            float[] signalData = new float[inputLength];
            for (int i = 0; i < inputLength; i++)
                signalData[i] = (float)Math.Sin(2.0 * Math.PI * 440.0 * i / inFreq);

            using (Mat input = new Mat(1, inputLength, DepthType.Cv32F, 1))
            using (Mat output = new Mat())
            {
                System.Runtime.InteropServices.Marshal.Copy(signalData, 0, input.DataPointer, inputLength);

                Emgu.CV.Signal.SignalInvoke.ResampleSignal(input, output, inFreq, outFreq);

                int expectedLength = (int)Math.Floor((double)inputLength * outFreq / inFreq);
                EmguAssert.IsTrue(!output.IsEmpty, "ResampleSignal output should not be empty");
                EmguAssert.IsTrue(
                    output.Cols == expectedLength,
                    String.Format("ResampleSignal: expected {0} output samples, got {1}", expectedLength, output.Cols));
            }
        }
    }
}
