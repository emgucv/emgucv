//----------------------------------------------------------------------------
//  Copyright (C) 2004-2026 by EMGU Corporation. All rights reserved.
//----------------------------------------------------------------------------
using System;
using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.PpfMatch3d;
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
    public class AutoTestSurfaceMatching
    {
        // Generate points evenly distributed on a unit sphere (Fibonacci sphere), as a
        // synthetic stand-in for the module's own .ply sample models.
        private static Mat CreateSpherePointCloud(int count)
        {
            float[] data = new float[count * 3];
            double goldenAngle = Math.PI * (3.0 - Math.Sqrt(5.0));
            for (int i = 0; i < count; i++)
            {
                double y = 1.0 - (i / (double)(count - 1)) * 2.0;
                double radius = Math.Sqrt(Math.Max(0.0, 1.0 - y * y));
                double theta = goldenAngle * i;
                double x = Math.Cos(theta) * radius;
                double z = Math.Sin(theta) * radius;

                data[i * 3] = (float)x;
                data[i * 3 + 1] = (float)y;
                data[i * 3 + 2] = (float)z;
            }

            Mat pc = new Mat(count, 3, DepthType.Cv32F, 1);
            System.Runtime.InteropServices.Marshal.Copy(data, 0, pc.DataPointer, data.Length);
            return pc;
        }

        [Test]
        public void TestComputeNormalsAndTransform()
        {
            using (Mat pc = CreateSpherePointCloud(100))
            using (Mat pcWithNormals = new Mat())
            {
                int result = PpfMatch3dInvoke.ComputeNormalsPC3d(
                    pc, pcWithNormals, 6, true, new float[] { 0, 0, 0 });
                EmguAssert.AreEqual(1, result);
                EmguAssert.IsTrue(!pcWithNormals.IsEmpty, "ComputeNormalsPC3d output should not be empty");
                EmguAssert.AreEqual(6, pcWithNormals.Cols);
                EmguAssert.AreEqual(pc.Rows, pcWithNormals.Rows);

                using (Mat identityPose = Mat.Eye(4, 4, DepthType.Cv64F, 1))
                using (Mat transformed = new Mat())
                {
                    PpfMatch3dInvoke.TransformPCPose(pcWithNormals, identityPose, transformed);
                    EmguAssert.IsTrue(!transformed.IsEmpty, "TransformPCPose output should not be empty");
                    EmguAssert.AreEqual(pcWithNormals.Size, transformed.Size);
                }
            }
        }

        [Test]
        public void TestICPRegisterModelToScene()
        {
            using (Mat pc = CreateSpherePointCloud(100))
            using (Mat model = new Mat())
            {
                PpfMatch3dInvoke.ComputeNormalsPC3d(pc, model, 6, true, new float[] { 0, 0, 0 });

                using (ICP icp = new ICP(10))
                using (Mat pose = new Mat())
                {
                    double residual = 0;
                    // Register the model against itself; ICP should converge trivially.
                    int result = icp.RegisterModelToScene(model, model, ref residual, pose);

                    EmguAssert.AreEqual(0, result);
                    EmguAssert.IsTrue(!pose.IsEmpty, "ICP.RegisterModelToScene pose output should not be empty");
                    EmguAssert.AreEqual(4, pose.Rows);
                    EmguAssert.AreEqual(4, pose.Cols);
                }
            }
        }

        [Test]
        public void TestPPF3DDetectorTrainAndMatch()
        {
            using (Mat pc = CreateSpherePointCloud(100))
            using (Mat model = new Mat())
            {
                PpfMatch3dInvoke.ComputeNormalsPC3d(pc, model, 6, true, new float[] { 0, 0, 0 });

                using (PPF3DDetector detector = new PPF3DDetector(0.05, 0.05, 30))
                using (VectorOfPose3D results = new VectorOfPose3D())
                {
                    detector.TrainModel(model);
                    detector.Match(model, results, 0.2, 0.2);

                    EmguAssert.IsTrue(results.Size > 0, "PPF3DDetector.Match should find at least one pose");
                }
            }
        }

        [Test]
        public void TestPose3DProperties()
        {
            using (Pose3D pose = new Pose3D())
            {
                MCvPoint3D64f t = new MCvPoint3D64f(1.0, 2.0, 3.0);
                pose.T = t;
                MCvPoint3D64f actualT = pose.T;
                EmguAssert.AreEqual(t.X, actualT.X);
                EmguAssert.AreEqual(t.Y, actualT.Y);
                EmguAssert.AreEqual(t.Z, actualT.Z);

                MCvScalar q = new MCvScalar(1, 0, 0, 0);
                pose.Q = q;
                MCvScalar actualQ = pose.Q;
                EmguAssert.AreEqual(q.V0, actualQ.V0);
            }
        }
    }
}
