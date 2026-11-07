//----------------------------------------------------------------------------
//  Copyright (C) 2004-2026 by EMGU Corporation. All rights reserved.
//----------------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;
using Emgu.CV;
using Emgu.CV.Aruco;
using Emgu.CV.CvEnum;
using Emgu.CV.Features;
using Emgu.CV.Flann;
using Emgu.CV.Shape;
using Emgu.CV.Stitching;
using Emgu.CV.Text;
using Emgu.CV.Structure;
using Emgu.CV.Bioinspired;
using Emgu.CV.Dpm;
using Emgu.CV.ImgHash;
using Emgu.CV.Face;
using Emgu.CV.StructuredLight;
using Emgu.CV.Dnn;
using Emgu.CV.Cuda;
using Emgu.CV.Mcc;
using Emgu.CV.Models;
using Emgu.CV.Tiff;
using Emgu.CV.Util;
using Emgu.CV.VideoStab;
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
    public class AutoTestPtcloud
    {
        [Test]
        public void TestVolumeCreate()
        {
            foreach (VolumeType type in new VolumeType[]
                     {
                         VolumeType.Tsdf, VolumeType.HashTsdf, VolumeType.ColorTsdf, VolumeType.ColorHashTsdf
                     })
            {
                using (VolumeSettings settings = new VolumeSettings(type))
                using (Volume volume = new Volume(type, settings))
                {
                    EmguAssert.IsTrue(volume.Ptr != IntPtr.Zero, String.Format("Unable to create a {0} volume", type));
                }
            }
        }

        [Test]
        public void TestColorHashTsdfIntegrate()
        {
            //OpenCV 5.1: ColorHashTSDF. The depth-only integrate throws for the color volume types,
            //so the color overload must be used. The color image must be CV_8UC4, as in OpenCV's own tests.
            using (VolumeSettings settings = new VolumeSettings(VolumeType.ColorHashTsdf))
            using (Volume volume = new Volume(VolumeType.ColorHashTsdf, settings))
            using (Mat depth = new Mat(480, 640, DepthType.Cv32F, 1))
            using (Mat color = new Mat(480, 640, DepthType.Cv8U, 4))
            using (Mat pose = Mat.Eye(4, 4, DepthType.Cv32F, 1))
            {
                depth.SetTo(new MCvScalar(1.0));
                color.SetTo(new MCvScalar(10, 120, 200, 255));
                volume.IntegrateColor(depth, color, pose);
                CvInvoke.CheckError();

                using (Mat points = new Mat())
                using (Mat normals = new Mat())
                using (Mat colors = new Mat())
                {
                    volume.FetchPointsNormalsColors(points, normals, colors);
                    CvInvoke.CheckError();
                    EmguAssert.AreEqual(points.Rows, normals.Rows);
                    EmguAssert.AreEqual(points.Rows, colors.Rows);
                }
            }
        }

        [Test]
        public void TestRescaleDepth()
        {
            // CV_16U value 5000 represents 5000 mm; after rescale to CV_32F it should be 5.0 m
            using Mat depth16u = new Mat(4, 4, DepthType.Cv16U, 1);
            depth16u.SetTo(new MCvScalar(5000));

            using Mat depth32f = new Mat();
            CvInvoke.RescaleDepth(depth16u, DepthType.Cv32F, depth32f);

            EmguAssert.AreEqual(DepthType.Cv32F, depth32f.Depth);
            EmguAssert.AreEqual(depth16u.Size, depth32f.Size);

            float[] vals = new float[depth32f.Rows * depth32f.Cols];
            depth32f.CopyTo(vals);
            foreach (float v in vals)
                EmguAssert.IsTrue(Math.Abs(v - 5.0f) < 0.001f, $"Expected 5.0 but got {v}");
        }

        [Test]
        public void TestDepthTo3d()
        {
            int rows = 4, cols = 4;
            double f = 500.0, cx = 2.0, cy = 2.0;

            using Mat K = new Mat(3, 3, DepthType.Cv64F, 1);
            K.SetTo(new double[] { f, 0, cx, 0, f, cy, 0, 0, 1 });

            // All pixels at 1.0 m (CV_32F)
            using Mat depth = new Mat(rows, cols, DepthType.Cv32F, 1);
            depth.SetTo(new MCvScalar(1.0));

            using Mat points3d = new Mat();
            CvInvoke.DepthTo3d(depth, K, points3d);

            EmguAssert.IsFalse(points3d.IsEmpty);
            EmguAssert.AreEqual(rows, points3d.Rows);
            EmguAssert.AreEqual(cols, points3d.Cols);
            // Output should be 4-channel float
            EmguAssert.AreEqual(DepthType.Cv32F, points3d.Depth);
            EmguAssert.AreEqual(4, points3d.NumberOfChannels);
        }

        [Test]
        public void TestDepthTo3dPipelineRoundTrip()
        {
            // Full pipeline: CV_16U → RescaleDepth → DepthTo3d; verify Z values
            int rows = 3, cols = 3;
            double f = 500.0, cx = 1.0, cy = 1.0;

            using Mat K = new Mat(3, 3, DepthType.Cv64F, 1);
            K.SetTo(new double[] { f, 0, cx, 0, f, cy, 0, 0, 1 });

            // 3000 mm = 3.0 m
            using Mat depth16u = new Mat(rows, cols, DepthType.Cv16U, 1);
            depth16u.SetTo(new MCvScalar(3000));

            using Mat depthF = new Mat();
            CvInvoke.RescaleDepth(depth16u, DepthType.Cv32F, depthF);

            using Mat pts = new Mat();
            CvInvoke.DepthTo3d(depthF, K, pts);

            EmguAssert.AreEqual(rows, pts.Rows);
            EmguAssert.AreEqual(cols, pts.Cols);

            float[] flat = new float[rows * cols * 4];
            pts.CopyTo(flat);
            for (int i = 0; i < rows * cols; i++)
            {
                float z = flat[i * 4 + 2];
                EmguAssert.IsTrue(Math.Abs(z - 3.0f) < 0.01f, $"Expected Z=3.0 but got {z}");
            }
        }
    }
}
