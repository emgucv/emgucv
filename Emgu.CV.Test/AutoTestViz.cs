//----------------------------------------------------------------------------
//  Copyright (C) 2004-2026 by EMGU Corporation. All rights reserved.
//----------------------------------------------------------------------------
using System;
using System.Runtime.InteropServices;
using Emgu.CV;
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
    public class AutoTestViz
    {
        [Test]
        public void TestViz()
        {
            var openCVConfigDict = CvInvoke.ConfigDict;
            bool haveViz = (openCVConfigDict["HAVE_OPENCV_VIZ"] != 0);
            if (haveViz && RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                Viz3d viz = new Viz3d("show_simple_widgets");
                viz.SetBackgroundMeshLab();
                WCoordinateSystem coor = new WCoordinateSystem();
                viz.ShowWidget("coor", coor);
                WCube cube = new WCube(new MCvPoint3D64f(-.5, -.5, -.5), new MCvPoint3D64f(.5, .5, .5), true,
                    new MCvScalar(255, 255, 255));
                viz.ShowWidget("cube", cube);
                WCube cube0 = new WCube(new MCvPoint3D64f(-1, -1, -1), new MCvPoint3D64f(-.5, -.5, -.5), false,
                    new MCvScalar(123, 45, 200));
                viz.ShowWidget("cub0", cube0);
                //viz.Spin();
            }
        }
    }
}
