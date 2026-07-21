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
using Emgu.CV.Freetype;
using Emgu.CV.StructuredLight;
using Emgu.CV.Dnn;
using Emgu.CV.Cuda;
using Emgu.CV.Mcc;
using Emgu.CV.Models;
using Emgu.CV.Tiff;
using Emgu.CV.Util;
using Emgu.CV.VideoStab;
using Emgu.CV.XImgproc;
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
    public class AutoTestStereo
    {
        /*
        [Test]
        public void TestStereoGCCorrespondence()
        {
           Image<Gray, Byte> left = EmguAssert.LoadImage<Gray, byte>("scene_l.bmp");
           Image<Gray, Byte> right = EmguAssert.LoadImage<Gray, byte>("scene_r.bmp");
           Image<Gray, Int16> leftDisparity = new Image<Gray, Int16>(left.Size);
           Image<Gray, Int16> rightDisparity = new Image<Gray, Int16>(left.Size);

           StereoGC stereoSolver = new StereoGC(10, 5);
           Stopwatch watch = Stopwatch.StartNew();
           stereoSolver.FindStereoCorrespondence(left, right, leftDisparity, rightDisparity);
           watch.Stop();
           EmguAssert.WriteLine(String.Format("Time used: {0} milliseconds", watch.ElapsedMilliseconds));

           Matrix<double> q = new Matrix<double>(4, 4);
           q.SetIdentity();
           MCvPoint3D32f[] points = PointCollection.ReprojectImageTo3D(leftDisparity * (-16), q);

           float min = (float) 1.0e10, max = 0;
           foreach (MCvPoint3D32f p in points)
           {
              if (p.Z < min)
                 min = p.Z;
              else if (p.Z > max)
                 max = p.Z;
           }
           EmguAssert.WriteLine(String.Format("Min : {0}\r\nMax : {1}", min, max));

           //ImageViewer.Show(leftDisparity*(-16));
        }*/

        /*
        //TODO: This test is failing, check when this will be fixed.
        [Test]
        public void TestStereoBMCorrespondence()
        {
           Image<Gray, Byte> left = EmguAssert.LoadImage<Gray, byte>("scene_l.bmp");
           Image<Gray, Byte> right = EmguAssert.LoadImage<Gray, byte>("scene_r.bmp");
           Image<Gray, Int16> leftDisparity = new Image<Gray, Int16>(left.Size);
           Image<Gray, Int16> rightDisparity = new Image<Gray, Int16>(left.Size);

           StereoBM bm = new StereoBM(Emgu.CV.CvEnum.STEREO_BM_TYPE.BASIC, 0);
           Stopwatch watch = Stopwatch.StartNew();
           bm.FindStereoCorrespondence(left, right, leftDisparity);
           watch.Stop();

           EmguAssert.WriteLine(String.Format("Time used: {0} milliseconds", watch.ElapsedMilliseconds));

           Matrix<double> q = new Matrix<double>(4, 4);
           q.SetIdentity();
           MCvPoint3D32f[] points = PointCollection.ReprojectImageTo3D(leftDisparity * (-16), q);

           float min = (float) 1.0e10, max = 0;
           foreach (MCvPoint3D32f p in points)
           {
              if (p.z < min)
                 min = p.z;
              else if (p.z > max)
                 max = p.z;
           }
           EmguAssert.WriteLine(String.Format("Min : {0}\r\nMax : {1}", min, max));

        }
  */

        [Test]
        public void TestStereoSGBMCorrespondence()
        {
            Mat left = EmguAssert.LoadMat("aloeL.jpg", ImreadModes.Grayscale);
            Mat right = EmguAssert.LoadMat("aloeR.jpg", ImreadModes.Grayscale);
            Size size = left.Size;

            Mat leftDisparity = new Mat();
            Mat rightDisparity = new Mat();

            StereoSGBM bmLeft = new StereoSGBM(10, 64, 0, 0, 0, 0, 0, 0, 0, 0, StereoSGBM.Mode.SGBM);
            RightMatcher bmRight = new RightMatcher(bmLeft);
            DisparityWLSFilter wlsFilter = new DisparityWLSFilter(bmLeft);

            Stopwatch watch = Stopwatch.StartNew();
            bmLeft.Compute(left, right, leftDisparity);
            watch.Stop();
            EmguAssert.WriteLine(String.Format("Time used: {0} milliseconds", watch.ElapsedMilliseconds));

            bmRight.Compute(right, left, rightDisparity);

            Mat filteredDisparity = new Mat();
            wlsFilter.Filter(leftDisparity, left, filteredDisparity, rightDisparity, new Rectangle(), right);


            Mat q = new Mat(4, 4, DepthType.Cv64F, 1);
            q.SetTo( new double[]
            {
                1.0, 0.0, 0.0, 0.0,
                0.0, 1.0, 0.0, 0.0,
                0.0, 0.0, 1.0, 0.0,
                0.0, 0.0, 0.0, 1.0
            });
            //q.SetIdentity();
            Mat disparityScaled = leftDisparity * (-16);
            MCvPoint3D32f[] points = PointCollection.ReprojectImageTo3D(disparityScaled, q);

            float min = (float)1.0e10, max = 0;
            foreach (MCvPoint3D32f p in points)
            {
                if (p.Z < min)
                    min = p.Z;
                else if (p.Z > max)
                    max = p.Z;
            }
            EmguAssert.WriteLine(String.Format("Min : {0}\r\nMax : {1}", min, max));

        }

        [Test]
        public void TestEstimateAffine3D()
        {
            Random r = new Random();
            //Matrix<double> affine = new Matrix<double>(3, 4);
            Mat affine = new Mat(3, 4, DepthType.Cv64F, 1);
            double[] affineData = new double[3 * 4];
            for (int i = 0; i < 3; i++)
                for (int j = 0; j < 4; j++)
                {
                    affineData[i*4+j] = r.NextDouble() * 2 + 1;
                }
            affine.SetTo(affineData);

            MCvPoint3D32f[] srcPts = new MCvPoint3D32f[4];
            for (int i = 0; i < srcPts.Length; i++)
            {
                srcPts[i] = new MCvPoint3D32f((float)(r.NextDouble() + 1), (float)(r.NextDouble() + 1), (float)(r.NextDouble() + 5));
            }
            MCvPoint3D32f[] dstPts = new MCvPoint3D32f[srcPts.Length];

            GCHandle srcHandle = GCHandle.Alloc(srcPts, GCHandleType.Pinned);
            GCHandle dstHandle = GCHandle.Alloc(dstPts, GCHandleType.Pinned);
            using (Mat srcMat = new Mat(srcPts.Length, 1, DepthType.Cv32F, 3, srcHandle.AddrOfPinnedObject(), Marshal.SizeOf(typeof(MCvPoint3D32f))))
            using (Mat dstMat = new Mat(dstPts.Length, 1, DepthType.Cv32F, 3, dstHandle.AddrOfPinnedObject(), Marshal.SizeOf(typeof(MCvPoint3D32f))))
            {
                CvInvoke.Transform(srcMat, dstMat, affine);
            }
            srcHandle.Free();
            dstHandle.Free();

            byte[] inlier;
            Mat estimate;
            CvInvoke.EstimateAffine3D(srcPts, dstPts, out estimate, out inlier, 3, 0.99);
        }

        /*
#region Test code contributed by Daniel Bell, modified by Canming
        [Test]
        public void TestLevMarqSparse()
        {
           int N = 11;
           int pN = 20;
           MCvPoint3D64f[] points = new MCvPoint3D64f[pN];

           MCvPoint2D64f[][] imagePoints = new MCvPoint2D64f[N][];
           int[][] visibility = new int[N][];
           List<Matrix<double>> cameraMatrix = new List<Matrix<double>>();
           List<Matrix<double>> R = new List<Matrix<double>>();
           List<Matrix<double>> T = new List<Matrix<double>>();
           List<Matrix<double>> distcoeff = new List<Matrix<double>>();
           MCvTermCriteria termCrit = new MCvTermCriteria(30, 1.0e-12);

           Size cameraRes = new Size(640, 480);
           // ... (full body omitted)
           LevMarqSparse.BundleAdjust(points, imagePoints, visibility, cameraMatrix.ToArray(), R.ToArray(), T.ToArray(), distcoeff.ToArray(), termCrit);

        }
#endregion
        */

        /*
        [Test]
        public void TestOctTree()
        {
           MCvPoint3D32f[] pts = new MCvPoint3D32f[] 
           {
              new MCvPoint3D32f(1, 2, 3),
              new MCvPoint3D32f(2, 3, 4),
              new MCvPoint3D32f(4, 5, 6),
              new MCvPoint3D32f(2, 2, 2)
           };

           using (Octree tree = new Octree(pts, 10, 10))
           {
              MCvPoint3D32f[] p = tree.GetPointsWithinSphere(new MCvPoint3D32f(0, 0, 0), 5);
              int i = p.Length;
           }
        }

        [Test]
        public void TestIndex3D()
        {
            Random r = new Random();
            MCvPoint3D32f[] points = GetRandom(10000, 0, 100, r);

            MCvPoint3D32f[] searchPoints = GetRandom(10, 0, 100, r);

            int indexOfClosest1 = 0;
            double shortestDistance1 = double.MaxValue;
            for (int i = 0; i < points.Length; i++)
            {
                double dist = (searchPoints[0] - points[i]).Norm;
                if (dist < shortestDistance1)
                {
                    shortestDistance1 = dist;
                    indexOfClosest1 = i;
                }
            }
            using (Flann.KdTreeIndexParams p = new KdTreeIndexParams())
            using (Flann.Index3D index3D = new Emgu.CV.Flann.Index3D(points, p))
            {

                double shortestDistance2;
                Index3D.Neighbor n = index3D.NearestNeighbor(searchPoints[0]);
                shortestDistance2 = Math.Sqrt(n.SquareDist);

                //EmguAssert.IsTrue(indexOfClosest1 == n.Index);
                //EmguAssert.IsTrue((shortestDistance1 - shortestDistance2) <= 1.0e-3 * shortestDistance1);

                var neighbors = index3D.RadiusSearch(searchPoints[0], 100, 10);
            }
        }

        private static MCvPoint3D32f[] GetRandom(int count, float min, float max, Random r = null)
        {
            if (r == null)
                r = new Random();

            MCvPoint3D32f[] features = new MCvPoint3D32f[count];
            for (int i = 0; i < features.Length; i++)
            {
                MCvPoint3D32f p = new MCvPoint3D32f();

                p.X = (float)(r.NextDouble() * (max - min)) + min;
                p.Y = (float)(r.NextDouble() * (max - min)) + min;
                p.Z = (float)(r.NextDouble() * (max - min)) + min;

                features[i] = p;
            }

            return features;
        }
        */
    }
}