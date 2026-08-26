//----------------------------------------------------------------------------
//  Copyright (C) 2004-2026 by EMGU Corporation. All rights reserved.
//----------------------------------------------------------------------------
using System;
using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Plot;
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
    public class AutoTestPlot
    {
        [Test]
        public void TestPlot2dSingleAxis()
        {
            double[] data = new double[100];
            for (int i = 0; i < data.Length; i++)
                data[i] = Math.Sin(2.0 * Math.PI * i / data.Length);

            using (Mat dataMat = new Mat(1, data.Length, DepthType.Cv64F, 1))
            using (Mat result = new Mat())
            {
                System.Runtime.InteropServices.Marshal.Copy(data, 0, dataMat.DataPointer, data.Length);

                using (Plot2d plot = new Plot2d(dataMat))
                {
                    plot.SetPlotSize(300, 300);
                    plot.SetPlotLineColor(new MCvScalar(0, 0, 255));
                    plot.SetPlotBackgroundColor(new MCvScalar(255, 255, 255));
                    plot.SetPlotAxisColor(new MCvScalar(0, 0, 0));
                    plot.SetPlotGridColor(new MCvScalar(200, 200, 200));
                    plot.SetPlotTextColor(new MCvScalar(0, 0, 0));

                    plot.Render(result);
                }

                EmguAssert.IsTrue(!result.IsEmpty, "Plot2d.Render output should not be empty");
            }
        }

        [Test]
        public void TestPlot2dXY()
        {
            int count = 50;
            double[] xData = new double[count];
            double[] yData = new double[count];
            for (int i = 0; i < count; i++)
            {
                xData[i] = i;
                yData[i] = i * i;
            }

            using (Mat xMat = new Mat(1, count, DepthType.Cv64F, 1))
            using (Mat yMat = new Mat(1, count, DepthType.Cv64F, 1))
            using (Mat result = new Mat())
            {
                System.Runtime.InteropServices.Marshal.Copy(xData, 0, xMat.DataPointer, count);
                System.Runtime.InteropServices.Marshal.Copy(yData, 0, yMat.DataPointer, count);

                using (Plot2d plot = new Plot2d(xMat, yMat))
                {
                    plot.Render(result);
                }

                EmguAssert.IsTrue(!result.IsEmpty, "Plot2d.Render (X/Y) output should not be empty");
            }
        }
    }
}
