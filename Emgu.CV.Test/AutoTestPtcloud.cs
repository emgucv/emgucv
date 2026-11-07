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
        //Helpers for the point cloud processing tests. A cloud is an Nx1 CV_32FC3 Mat.
        private static Mat MakeCloudMat(System.Collections.Generic.List<float[]> points)
        {
            float[] flat = new float[points.Count * 3];
            for (int i = 0; i < points.Count; i++)
            {
                flat[3 * i] = points[i][0];
                flat[3 * i + 1] = points[i][1];
                flat[3 * i + 2] = points[i][2];
            }
            Mat m = new Mat(points.Count, 1, DepthType.Cv32F, 3);
            if (points.Count > 0)
                Marshal.Copy(flat, 0, m.DataPointer, flat.Length);
            return m;
        }

        //A 1xN CV_32FC3 Mat, the layout OpenCV 5.1's own tests use for the triangle rasterizer inputs: an Nx1 column of
        //3-channel elements fails an in-place transpose assertion inside triangleRasterize.
        private static Mat MakeRowMat(System.Collections.Generic.List<float[]> points)
        {
            float[] flat = new float[points.Count * 3];
            for (int i = 0; i < points.Count; i++)
            {
                flat[3 * i] = points[i][0];
                flat[3 * i + 1] = points[i][1];
                flat[3 * i + 2] = points[i][2];
            }
            Mat m = new Mat(1, points.Count, DepthType.Cv32F, 3);
            Marshal.Copy(flat, 0, m.DataPointer, flat.Length);
            return m;
        }

        private static float[] ReadFloats(Mat m, int count)
        {
            float[] result = new float[count];
            Marshal.Copy(m.DataPointer, result, 0, count);
            return result;
        }

        //Evenly sampled unit sphere (Fibonacci lattice).
        private static System.Collections.Generic.List<float[]> MakeSpherePoints(int n)
        {
            var pts = new System.Collections.Generic.List<float[]>();
            float ga = (float)(Math.PI * (3.0 - Math.Sqrt(5.0)));
            for (int i = 0; i < n; i++)
            {
                float z = 1f - 2f * (i + 0.5f) / n;
                float r = (float)Math.Sqrt(Math.Max(0f, 1f - z * z));
                float t = ga * i;
                pts.Add(new float[] { r * (float)Math.Cos(t), r * (float)Math.Sin(t), z });
            }
            return pts;
        }

        //A 10x10x10 grid (spacing 0.1) followed by five far away isolated points.
        private static System.Collections.Generic.List<float[]> MakeGridWithOutliers(out int numInliers)
        {
            var pts = new System.Collections.Generic.List<float[]>();
            for (int x = 0; x < 10; x++)
                for (int y = 0; y < 10; y++)
                    for (int z = 0; z < 10; z++)
                        pts.Add(new float[] { x * 0.1f, y * 0.1f, z * 0.1f });
            numInliers = pts.Count;
            pts.Add(new float[] { 5f, 5f, 5f });
            pts.Add(new float[] { -4f, 2f, 1f });
            pts.Add(new float[] { 3f, -6f, 2f });
            pts.Add(new float[] { 8f, 8f, -3f });
            pts.Add(new float[] { -5f, -5f, 7f });
            return pts;
        }

        [Test]
        public void TestRemoveStatisticalOutliers()
        {
            int numInliers;
            using (Mat cloud = MakeCloudMat(MakeGridWithOutliers(out numInliers)))
            using (Mat filtered = new Mat())
            using (Mat kept = new Mat())
            {
                CvInvoke.RemoveStatisticalOutliers(cloud, filtered, 20, 2.0, kept);
                EmguAssert.IsTrue(numInliers == filtered.Total.ToInt32(), String.Format("Expected {0} points, got {1}", numInliers, filtered.Total.ToInt32()));
                EmguAssert.AreEqual(numInliers, kept.Total.ToInt32());
                int[] indices = new int[numInliers];
                Marshal.Copy(kept.DataPointer, indices, 0, numInliers);
                foreach (int idx in indices)
                    EmguAssert.IsTrue(idx >= 0 && idx < numInliers, "A far away outlier was kept, index " + idx);

                //the optional kept indices output can be omitted
                using (Mat filtered2 = new Mat())
                {
                    CvInvoke.RemoveStatisticalOutliers(cloud, filtered2);
                    EmguAssert.AreEqual(numInliers, filtered2.Total.ToInt32());
                }
            }
        }

        [Test]
        public void TestRemoveRadiusOutliers()
        {
            int numInliers;
            using (Mat cloud = MakeCloudMat(MakeGridWithOutliers(out numInliers)))
            using (Mat filtered = new Mat())
            using (Mat kept = new Mat())
            {
                CvInvoke.RemoveRadiusOutliers(cloud, filtered, 0.25, 5, kept);
                EmguAssert.IsTrue(numInliers == filtered.Total.ToInt32(), String.Format("Expected {0} points, got {1}", numInliers, filtered.Total.ToInt32()));
                EmguAssert.AreEqual(numInliers, kept.Total.ToInt32());
            }
        }

        [Test]
        public void TestOrientNormals()
        {
            //A planar grid on z = 0 whose normals all point down must be flipped to face a viewpoint above the plane.
            var pts = new System.Collections.Generic.List<float[]>();
            for (int x = 0; x < 20; x++)
                for (int y = 0; y < 20; y++)
                    pts.Add(new float[] { x * 0.1f, y * 0.1f, 0f });
            int n = pts.Count;
            using (Mat cloud = MakeCloudMat(pts))
            using (Mat normals = new Mat(n, 1, DepthType.Cv32F, 3))
            {
                float[] down = new float[n * 3];
                for (int i = 0; i < n; i++)
                    down[3 * i + 2] = -1f;
                Marshal.Copy(down, 0, normals.DataPointer, down.Length);

                CvInvoke.OrientNormals(cloud, normals, new MCvPoint3D32f(1f, 1f, 5f));
                float[] result = ReadFloats(normals, n * 3);
                for (int i = 0; i < n; i++)
                    EmguAssert.IsTrue(result[3 * i + 2] > 0.99f, "The normal does not face the viewpoint, index " + i);
            }
        }

        [Test]
        public void TestOrientNormalsConsistent()
        {
            //A unit sphere with exact outward normals, every third one flipped. Consistent orientation must make
            //(almost) all of them point away from the center again.
            var pts = MakeSpherePoints(1000);
            int n = pts.Count;
            using (Mat cloud = MakeCloudMat(pts))
            using (Mat normals = new Mat(n, 1, DepthType.Cv32F, 3))
            {
                float[] nv = new float[n * 3];
                for (int i = 0; i < n; i++)
                {
                    float sign = (i % 3 == 0) ? -1f : 1f;
                    nv[3 * i] = sign * pts[i][0];
                    nv[3 * i + 1] = sign * pts[i][1];
                    nv[3 * i + 2] = sign * pts[i][2];
                }
                Marshal.Copy(nv, 0, normals.DataPointer, nv.Length);

                CvInvoke.OrientNormalsConsistent(cloud, normals, 12);
                float[] result = ReadFloats(normals, n * 3);
                int outward = 0;
                for (int i = 0; i < n; i++)
                {
                    float dot = result[3 * i] * pts[i][0] + result[3 * i + 1] * pts[i][1] + result[3 * i + 2] * pts[i][2];
                    if (dot > 0)
                        outward++;
                }
                EmguAssert.IsTrue(outward >= (int)(0.98 * n), "Only " + outward + " of " + n + " normals point outward");
            }
        }

        [Test]
        public void TestEstimateMedianSpacingAndMeshBPA()
        {
            var pts = MakeSpherePoints(500);
            int n = pts.Count;
            using (Mat cloud = MakeCloudMat(pts))
            using (Mat normals = MakeCloudMat(pts)) //the exact normals of a unit sphere are the points themselves
            using (Mat vertices = new Mat())
            using (Mat triangles = new Mat())
            {
                float spacing = CvInvoke.EstimateMedianSpacing(cloud);
                EmguAssert.IsTrue(spacing > 0.01f && spacing < 0.5f, "Unexpected median spacing " + spacing);

                CvInvoke.CreateMeshBPA(cloud, normals, vertices, triangles);
                EmguAssert.IsTrue(n == vertices.Total.ToInt32(), String.Format("Expected {0} vertices, got {1}", n, vertices.Total.ToInt32()));
                //the triangles are stored as 3 vertex indices each; the Mat may be Mx3 or a 1-D array of 3-channel elements
                int valueCount = triangles.Total.ToInt32() * triangles.NumberOfChannels;
                EmguAssert.AreEqual(DepthType.Cv32S, triangles.Depth);
                EmguAssert.IsTrue(valueCount > 0 && valueCount % 3 == 0, "Ball pivoting should produce triangles, got " + valueCount + " values");
                int[] idx = new int[valueCount];
                Marshal.Copy(triangles.DataPointer, idx, 0, idx.Length);
                foreach (int i in idx)
                    EmguAssert.IsTrue(i >= 0 && i < n, "Triangle index out of range: " + i);

                //explicit radii
                using (Mat radii = new Mat(1, 2, DepthType.Cv32F, 1))
                using (Mat vertices2 = new Mat())
                using (Mat triangles2 = new Mat())
                {
                    Marshal.Copy(new float[] { spacing * 1.5f, spacing * 3f }, 0, radii.DataPointer, 2);
                    CvInvoke.CreateMeshBPA(cloud, normals, vertices2, triangles2, radii);
                    EmguAssert.AreEqual(n, vertices2.Total.ToInt32());
                    EmguAssert.IsTrue(triangles2.Total.ToInt32() * triangles2.NumberOfChannels % 3 == 0, "Unexpected triangle layout");
                }
            }
        }

        [Test]
        public void TestBoundingVolumes3D()
        {
            //An axis aligned box grid of 20 x 10 x 5 points with 0.1 spacing: x in [0, 1.9], y in [0, 0.9], z in [0, 0.4].
            var pts = new System.Collections.Generic.List<float[]>();
            for (int x = 0; x < 20; x++)
                for (int y = 0; y < 10; y++)
                    for (int z = 0; z < 5; z++)
                        pts.Add(new float[] { x * 0.1f, y * 0.1f, z * 0.1f });
            using (Mat cloud = MakeCloudMat(pts))
            using (Mat minB = new Mat())
            using (Mat maxB = new Mat())
            using (Mat center = new Mat())
            using (Mat axes = new Mat())
            using (Mat half = new Mat())
            using (Mat sphereCenter = new Mat())
            {
                CvInvoke.BoundingBox3D(cloud, minB, maxB);
                float[] mn = ReadFloats(minB, 3);
                float[] mx = ReadFloats(maxB, 3);
                float[] expectedMax = new float[] { 1.9f, 0.9f, 0.4f };
                for (int i = 0; i < 3; i++)
                {
                    EmguAssert.IsTrue(Math.Abs(mn[i]) < 1e-5f, "AABB min " + i + " = " + mn[i]);
                    EmguAssert.IsTrue(Math.Abs(mx[i] - expectedMax[i]) < 1e-5f, "AABB max " + i + " = " + mx[i]);
                }

                CvInvoke.OrientedBoundingBox3D(cloud, center, axes, half);
                float[] c = ReadFloats(center, 3);
                float[] a = ReadFloats(axes, 9);
                float[] h = ReadFloats(half, 3);
                //every point must be inside the oriented box: abs(axes * (p - center)) <= halfExtents
                foreach (float[] p in pts)
                    for (int r = 0; r < 3; r++)
                    {
                        float d = a[3 * r] * (p[0] - c[0]) + a[3 * r + 1] * (p[1] - c[1]) + a[3 * r + 2] * (p[2] - c[2]);
                        EmguAssert.IsTrue(Math.Abs(d) <= h[r] + 1e-3f, "Point outside of the oriented box");
                    }
                //the PCA axes of this box are the coordinate axes, so the sorted half extents are known
                Array.Sort(h);
                EmguAssert.IsTrue(Math.Abs(h[0] - 0.2f) < 1e-3f && Math.Abs(h[1] - 0.45f) < 1e-3f && Math.Abs(h[2] - 0.95f) < 1e-3f,
                    "Unexpected half extents " + h[0] + ", " + h[1] + ", " + h[2]);

                double radius = CvInvoke.ApproxEnclosingSphere3D(cloud, sphereCenter);
                float[] sc = ReadFloats(sphereCenter, 3);
                EmguAssert.IsTrue(radius > 0, "The enclosing sphere radius should be positive");
                foreach (float[] p in pts)
                {
                    double dist = Math.Sqrt((p[0] - sc[0]) * (p[0] - sc[0]) + (p[1] - sc[1]) * (p[1] - sc[1]) + (p[2] - sc[2]) * (p[2] - sc[2]));
                    EmguAssert.IsTrue(dist <= radius + 1e-3, "A point is outside of the enclosing sphere");
                }
            }
        }

        [Test]
        public void TestPointCloudProcessingEmptyInput()
        {
            using (Mat empty = new Mat())
            using (Mat single = MakeCloudMat(new System.Collections.Generic.List<float[]> { new float[] { 1f, 2f, 3f } }))
            using (Mat center = new Mat())
            {
                EmguAssert.AreEqual(0.0f, CvInvoke.EstimateMedianSpacing(single));
                EmguAssert.AreEqual(0.0, CvInvoke.ApproxEnclosingSphere3D(empty, center));
                using (Mat normals = new Mat())
                {
                    CvInvoke.OrientNormals(empty, normals, new MCvPoint3D32f(0f, 0f, 1f));
                    CvInvoke.OrientNormalsConsistent(empty, normals, 8);
                }
            }
        }

        [Test]
        public void TestGaussianSplatPlyPropNames()
        {
            String[] names = CvInvoke.GetGaussianSplatPlyPropNames();
            EmguAssert.IsTrue(names.Length >= 14, "Expected at least x, y, z, f_dc_0..2, opacity, scale_0..2 and rot_0..3, got " + names.Length);
            EmguAssert.IsTrue(Array.IndexOf(names, "x") >= 0 && Array.IndexOf(names, "opacity") >= 0 && Array.IndexOf(names, "rot_0") >= 0,
                "The default property names should include x, opacity and rot_0");
        }

        [Test]
        public void TestDecodeGaussianSplats()
        {
            //Two Gaussians with identity rotation, scale exp(log(0.1)) = 0.1, zero color harmonics and zero opacity logit.
            //Decoding gives color 0.5, opacity sigmoid(0) = 0.5 and a diagonal covariance of 0.01.
            String[] names = CvInvoke.GetGaussianSplatPlyPropNames();
            int columns = names.Length;
            int gaussians = 2;
            float[] attributes = new float[gaussians * columns];
            for (int g = 0; g < gaussians; g++)
            {
                attributes[g * columns + Array.IndexOf(names, "x")] = 1f + g;
                attributes[g * columns + Array.IndexOf(names, "y")] = 2f + g;
                attributes[g * columns + Array.IndexOf(names, "z")] = 3f + g;
                for (int s = 0; s < 3; s++)
                    attributes[g * columns + Array.IndexOf(names, "scale_" + s)] = (float)Math.Log(0.1);
                attributes[g * columns + Array.IndexOf(names, "rot_0")] = 1f;
            }

            using (Mat attr = new Mat(gaussians, columns, DepthType.Cv32F, 1))
            using (Mat splats = new Mat())
            {
                Marshal.Copy(attributes, 0, attr.DataPointer, attributes.Length);
                CvInvoke.DecodeGaussianSplats(attr, splats);
                int stride = (int)GaussianSplatLayout.Stride;
                EmguAssert.AreEqual(gaussians * stride, splats.Total.ToInt32() * splats.NumberOfChannels);
                float[] v = ReadFloats(splats, gaussians * stride);
                for (int g = 0; g < gaussians; g++)
                {
                    int o = g * stride;
                    int pos = (int)GaussianSplatLayout.Pos, cov = (int)GaussianSplatLayout.Cov;
                    int rgb = (int)GaussianSplatLayout.Rgb, alpha = (int)GaussianSplatLayout.Alpha;
                    EmguAssert.IsTrue(Math.Abs(v[o + pos] - (1f + g)) < 1e-5f && Math.Abs(v[o + pos + 1] - (2f + g)) < 1e-5f && Math.Abs(v[o + pos + 2] - (3f + g)) < 1e-5f, "position");
                    for (int c = 0; c < 3; c++)
                        EmguAssert.IsTrue(Math.Abs(v[o + rgb + c] - 0.5f) < 1e-3f, "color " + c + " = " + v[o + rgb + c]);
                    EmguAssert.IsTrue(Math.Abs(v[o + alpha] - 0.5f) < 1e-3f, "opacity = " + v[o + alpha]);
                    //upper triangle order: xx, xy, xz, yy, yz, zz
                    EmguAssert.IsTrue(Math.Abs(v[o + cov] - 0.01f) < 1e-4f, "cov xx = " + v[o + cov]);
                    EmguAssert.IsTrue(Math.Abs(v[o + cov + 3] - 0.01f) < 1e-4f, "cov yy = " + v[o + cov + 3]);
                    EmguAssert.IsTrue(Math.Abs(v[o + cov + 5] - 0.01f) < 1e-4f, "cov zz = " + v[o + cov + 5]);
                    EmguAssert.IsTrue(Math.Abs(v[o + cov + 1]) < 1e-4f && Math.Abs(v[o + cov + 2]) < 1e-4f && Math.Abs(v[o + cov + 4]) < 1e-4f, "off-diagonal covariance");
                }
            }
        }

        [Test]
        public void TestDecodeGaussianSplatsPacked()
        {
            //One 32 byte .splat record: position (3 floats), scale (3 floats, already activated), RGBA (4 bytes),
            //rotation quaternion (4 bytes, (b - 128) / 128).
            byte[] record = new byte[32];
            Buffer.BlockCopy(new float[] { 1f, 2f, 3f, 0.1f, 0.1f, 0.1f }, 0, record, 0, 24);
            record[24] = 255; record[25] = 128; record[26] = 0; record[27] = 255;
            record[28] = 255; record[29] = 128; record[30] = 128; record[31] = 128;
            using (Mat buf = new Mat(1, record.Length, DepthType.Cv8U, 1))
            using (Mat splats = new Mat())
            {
                Marshal.Copy(record, 0, buf.DataPointer, record.Length);
                CvInvoke.DecodeGaussianSplatsPacked(buf, splats);
                int stride = (int)GaussianSplatLayout.Stride;
                EmguAssert.AreEqual(stride, splats.Total.ToInt32() * splats.NumberOfChannels);
                float[] v = ReadFloats(splats, stride);
                int pos = (int)GaussianSplatLayout.Pos, cov = (int)GaussianSplatLayout.Cov;
                int rgb = (int)GaussianSplatLayout.Rgb, alpha = (int)GaussianSplatLayout.Alpha;
                EmguAssert.IsTrue(Math.Abs(v[pos] - 1f) < 1e-5f && Math.Abs(v[pos + 1] - 2f) < 1e-5f && Math.Abs(v[pos + 2] - 3f) < 1e-5f, "position");
                EmguAssert.IsTrue(Math.Abs(v[rgb] - 1f) < 1e-2f && Math.Abs(v[rgb + 1] - 128f / 255f) < 1e-2f && Math.Abs(v[rgb + 2]) < 1e-2f,
                    "color " + v[rgb] + ", " + v[rgb + 1] + ", " + v[rgb + 2]);
                EmguAssert.IsTrue(Math.Abs(v[alpha] - 1f) < 1e-2f, "opacity = " + v[alpha]);
                //an almost identity rotation of an isotropic scale keeps the covariance close to 0.01 * I
                EmguAssert.IsTrue(Math.Abs(v[cov] - 0.01f) < 1e-3f && Math.Abs(v[cov + 3] - 0.01f) < 1e-3f && Math.Abs(v[cov + 5] - 0.01f) < 1e-3f,
                    "covariance diagonal " + v[cov] + ", " + v[cov + 3] + ", " + v[cov + 5]);
            }
        }

        [Test]
        public void TestTriangleRasterize()
        {
            //A triangle 2 units in front of the camera (the camera looks down -Z), seen with a 90 degree vertical field of view.
            int width = 100;
            int height = 100;
            double zNear = 0.1;
            double zFar = 10.0;
            using (Mat vertices = MakeRowMat(new System.Collections.Generic.List<float[]>
                   {
                       new float[] { -1f, -1f, -2f },
                       new float[] { 1f, -1f, -2f },
                       new float[] { 0f, 1f, -2f }
                   }))
            using (Mat indices = new Mat(1, 1, DepthType.Cv32S, 3))
            using (Mat colors = MakeRowMat(new System.Collections.Generic.List<float[]>
                   {
                       new float[] { 1f, 0f, 0f },
                       new float[] { 0f, 1f, 0f },
                       new float[] { 0f, 0f, 1f }
                   }))
            using (Mat world2cam = Mat.Eye(4, 4, DepthType.Cv32F, 1))
            using (Mat colorBuf = new Mat(height, width, DepthType.Cv32F, 3))
            using (Mat depthBuf = new Mat(height, width, DepthType.Cv32F, 1))
            using (Mat depthOnly = new Mat(height, width, DepthType.Cv32F, 1))
            using (Mat colorOnly = new Mat(height, width, DepthType.Cv32F, 3))
            {
                Marshal.Copy(new int[] { 0, 1, 2 }, 0, indices.DataPointer, 3);
                colorBuf.SetTo(new MCvScalar(0, 0, 0));
                colorOnly.SetTo(new MCvScalar(0, 0, 0));
                depthBuf.SetTo(new MCvScalar(zFar));
                depthOnly.SetTo(new MCvScalar(zFar));

                CvInvoke.TriangleRasterize(
                    vertices, indices, colors, colorBuf, depthBuf, world2cam, Math.PI / 2, zNear, zFar,
                    TriangleShadingType.Shaded, TriangleCullingMode.None);
                CvInvoke.TriangleRasterizeDepth(
                    vertices, indices, depthOnly, world2cam, Math.PI / 2, zNear, zFar,
                    TriangleShadingType.Shaded, TriangleCullingMode.None);
                CvInvoke.TriangleRasterizeColor(
                    vertices, indices, colors, colorOnly, world2cam, Math.PI / 2, zNear, zFar,
                    TriangleShadingType.Shaded, TriangleCullingMode.None);

                float[] depth = ReadFloats(depthBuf, width * height);
                float[] depth2 = ReadFloats(depthOnly, width * height);
                float[] color = ReadFloats(colorBuf, width * height * 3);
                float[] color2 = ReadFloats(colorOnly, width * height * 3);

                //the image center is inside the triangle, the top-left corner is outside of it
                int center = (height / 2) * width + width / 2;
                EmguAssert.IsTrue(Math.Abs(depth[center] - 2f) < 0.05f, "center depth = " + depth[center]);
                EmguAssert.IsTrue(Math.Abs(depth[0] - zFar) < 1e-4, "corner depth = " + depth[0]);
                EmguAssert.IsTrue(color[3 * center] + color[3 * center + 1] + color[3 * center + 2] > 0.5f, "The triangle center should be colored");
                EmguAssert.IsTrue(color[0] == 0f && color[1] == 0f && color[2] == 0f, "The corner should stay black");

                //the depth-only and color-only variants must agree with the combined one
                for (int i = 0; i < depth.Length; i++)
                    EmguAssert.IsTrue(Math.Abs(depth[i] - depth2[i]) < 1e-4f, "depth mismatch at " + i);
                for (int i = 0; i < color.Length; i++)
                    EmguAssert.IsTrue(Math.Abs(color[i] - color2[i]) < 1e-4f, "color mismatch at " + i);
            }
        }

        [Test]
        public void TestPtcloudNativeErrorsAreRaised()
        {
            //native errors in the ptcloud functions must surface as a CvException from the call that caused them.
            //depthTo3d asserts that the camera matrix K is 3x3.
            bool thrown = false;
            try
            {
                using (Mat depth = new Mat(4, 4, DepthType.Cv32F, 1))
                using (Mat badK = new Mat(2, 2, DepthType.Cv64F, 1))
                using (Mat points = new Mat())
                    CvInvoke.DepthTo3d(depth, badK, points);
            }
            catch (Emgu.CV.Util.CvException)
            {
                thrown = true;
            }

            EmguAssert.IsTrue(thrown, "DepthTo3d with an invalid camera matrix should throw a CvException");
        }

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
