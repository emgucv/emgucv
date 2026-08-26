//----------------------------------------------------------------------------
//  Copyright (C) 2004-2026 by EMGU Corporation. All rights reserved.
//----------------------------------------------------------------------------
using System;
using System.Drawing;
using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Rapid;
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
    public class AutoTestRapid
    {
        // A flat square mesh (matching the layout used by the rapid module's own
        // track_marker.py sample): 4 corners in the Z=0 plane, 2 triangles in CCW order.
        private static VectorOfPoint3D32F CreateSquareMeshPoints()
        {
            return new VectorOfPoint3D32F(new MCvPoint3D32f[]
            {
                new MCvPoint3D32f(-5, 5, 0),
                new MCvPoint3D32f(5, 5, 0),
                new MCvPoint3D32f(5, -5, 0),
                new MCvPoint3D32f(-5, -5, 0),
            });
        }

        private static Mat CreateSquareMeshTriangles()
        {
            Mat tris = new Mat(2, 3, DepthType.Cv32S, 1);
            int[] triData = new int[] { 0, 1, 2, 0, 2, 3 };
            System.Runtime.InteropServices.Marshal.Copy(triData, 0, tris.DataPointer, triData.Length);
            return tris;
        }

        [Test]
        public void TestTrackerConstruction()
        {
            using (VectorOfPoint3D32F pts3d = CreateSquareMeshPoints())
            using (Mat tris = CreateSquareMeshTriangles())
            using (Emgu.CV.Rapid.Rapid rapid = new Emgu.CV.Rapid.Rapid(pts3d, tris))
            using (OLSTracker olsTracker = new OLSTracker(pts3d, tris))
            {
                EmguAssert.IsTrue(rapid.TrackerPtr != IntPtr.Zero, "Rapid.TrackerPtr should not be null");
                EmguAssert.IsTrue(rapid.AlgorithmPtr != IntPtr.Zero, "Rapid.AlgorithmPtr should not be null");
                EmguAssert.IsTrue(olsTracker.TrackerPtr != IntPtr.Zero, "OLSTracker.TrackerPtr should not be null");
                EmguAssert.IsTrue(olsTracker.AlgorithmPtr != IntPtr.Zero, "OLSTracker.AlgorithmPtr should not be null");

                rapid.ClearState();
                olsTracker.ClearState();
            }
        }

        [Test]
        public void TestDrawWireframeAndExtractControlPoints()
        {
            Size imsize = new Size(800, 600);
            using (VectorOfPoint3D32F pts3d = CreateSquareMeshPoints())
            using (Mat tris = CreateSquareMeshTriangles())
            using (Mat rvec = new Mat(3, 1, DepthType.Cv64F, 1))
            using (Mat tvec = new Mat(3, 1, DepthType.Cv64F, 1))
            using (Mat cameraMatrix = new Mat(3, 3, DepthType.Cv64F, 1))
            using (VectorOfPointF pts2d = new VectorOfPointF())
            using (Mat img = new Mat(imsize, DepthType.Cv8U, 1))
            {
                double[] rvecData = new double[] { 0, 0, 0 };
                System.Runtime.InteropServices.Marshal.Copy(rvecData, 0, rvec.DataPointer, rvecData.Length);

                double[] tvecData = new double[] { 0, 0, 50 };
                System.Runtime.InteropServices.Marshal.Copy(tvecData, 0, tvec.DataPointer, tvecData.Length);

                double[] cameraMatrixData = new double[] { 800, 0, 400, 0, 800, 300, 0, 0, 1 };
                System.Runtime.InteropServices.Marshal.Copy(cameraMatrixData, 0, cameraMatrix.DataPointer, cameraMatrixData.Length);

                img.SetTo(new MCvScalar(0));

                CvInvoke.ProjectPoints(pts3d, rvec, tvec, cameraMatrix, null, pts2d);
                EmguAssert.AreEqual(4, pts2d.Size);

                RapidInvoke.DrawWireframe(img, pts2d, tris, new MCvScalar(255), LineType.EightConnected, true);
                EmguAssert.IsTrue(CvInvoke.CountNonZero(img) > 0, "DrawWireframe should have drawn non-zero pixels");

                using (VectorOfPointF ctl2d = new VectorOfPointF())
                using (VectorOfPoint3D32F ctl3d = new VectorOfPoint3D32F())
                {
                    RapidInvoke.ExtractControlPoints(
                        4, 10, pts3d, rvec, tvec, cameraMatrix, imsize, tris, ctl2d, ctl3d);

                    EmguAssert.IsTrue(ctl2d.Size > 0, "ExtractControlPoints should produce at least one 2D control point");
                    EmguAssert.AreEqual(ctl2d.Size, ctl3d.Size);
                }
            }
        }
    }
}
