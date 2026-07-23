//----------------------------------------------------------------------------
//  Copyright (C) 2004-2026 by EMGU Corporation. All rights reserved.
//----------------------------------------------------------------------------

using System;
using System.Diagnostics;
using System.Drawing;
using System.Runtime.InteropServices;
using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Features;
using Emgu.CV.Structure;
using Emgu.CV.Util;
using Emgu.Util;

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
    public class AutoTestFeatures
    {
        [Test]
        public void TestAffineFeature()
        {
            using (SIFT sift = new SIFT())
            using (AffineFeature affine = new AffineFeature(sift))
            using (Mat img = EmguAssert.LoadMat("box.png"))
            using (VectorOfKeyPoint kpts = new VectorOfKeyPoint())
            using (Mat descriptors = new Mat())
            {
                affine.DetectAndCompute(img, null, kpts, descriptors, false);
                EmguAssert.IsTrue(kpts.Size > 0, "AffineFeature should detect keypoints");
                EmguAssert.IsTrue(descriptors.Rows == kpts.Size, "One descriptor per keypoint expected");
            }
        }

        [Test]
        public void TestANNIndex()
        {
            int dim = 16;
            int count = 100;
            using (Mat features = new Mat(count, dim, DepthType.Cv32F, 1))
            using (ANNIndex index = new ANNIndex(dim))
            {
                CvInvoke.Randu(features, new MCvScalar(0), new MCvScalar(1));
                index.AddItems(features);
                index.Build();
                EmguAssert.IsTrue(index.ItemNumber == count, "Item number should match the number of features added");
                EmguAssert.IsTrue(index.TreeNumber > 0, "Tree number should be positive after build");

                using (Mat indices = new Mat())
                using (Mat dists = new Mat())
                {
                    index.KnnSearch(features.Row(0), indices, dists, 3);
                    EmguAssert.IsTrue(!indices.IsEmpty, "KnnSearch should return indices");
                }
            }
        }

        [Test]
        public void TestSIFT()
        {
            using (SIFT detector = new SIFT())
                EmguAssert.IsTrue(TestFeature2DTracker(detector, detector), "Unable to find homography matrix");
        }

        [Test]
        public void TestORB()
        {
            using (ORB orb = new ORB(700))
            {
                EmguAssert.IsTrue(TestFeature2DTracker(orb, orb), "Unable to find homography matrix");
            }
        }

        [Test]
        public void TestSimpleBlobDetector()
        {
            using (Mat box = EmguAssert.LoadMat("box.png"))
            using (SimpleBlobDetectorParams p = new SimpleBlobDetectorParams())
            {
                p.CollectContours = true;
                using (SimpleBlobDetector detector = new SimpleBlobDetector(p))
                using (Mat mask = new Mat(box.Size, DepthType.Cv8U, 1))
                {
                    mask.SetTo(new MCvScalar(255));
                    MKeyPoint[] keypoints = detector.Detect(box, mask);
                    using (VectorOfVectorOfPoint contour = detector.GetBlobContours())
                    {
                        int count = contour.Size;
                    }
                }
            }
        }

        public static bool TestFeature2DTracker(Feature2D keyPointDetector, Feature2D descriptorGenerator)
        {
            {
                Feature2D feature2D = null;
                if (keyPointDetector == descriptorGenerator)
                {
                    feature2D = keyPointDetector as Feature2D;
                }

                Mat modelImage = EmguAssert.LoadMat("box.png", ImreadModes.Grayscale);

                #region extract features from the object image
                Stopwatch stopwatch = Stopwatch.StartNew();
                VectorOfKeyPoint modelKeypoints = new VectorOfKeyPoint();
                Mat modelDescriptors = new Mat();
                if (feature2D != null)
                {
                    feature2D.DetectAndCompute(modelImage, null, modelKeypoints, modelDescriptors, false);
                }
                else
                {
                    keyPointDetector.DetectRaw(modelImage, modelKeypoints);
                    descriptorGenerator.Compute(modelImage, modelKeypoints, modelDescriptors);
                }
                stopwatch.Stop();
                EmguAssert.WriteLine(String.Format("Time to extract feature from model: {0} milli-sec", stopwatch.ElapsedMilliseconds));
                #endregion

                Mat observedImage = EmguAssert.LoadMat("box_in_scene.png", ImreadModes.Grayscale);

                #region extract features from the observed image
                stopwatch.Reset();
                stopwatch.Start();
                VectorOfKeyPoint observedKeypoints = new VectorOfKeyPoint();
                using (Mat observedDescriptors = new Mat())
                {
                    if (feature2D != null)
                    {
                        feature2D.DetectAndCompute(observedImage, null, observedKeypoints, observedDescriptors, false);
                    }
                    else
                    {
                        keyPointDetector.DetectRaw(observedImage, observedKeypoints);
                        descriptorGenerator.Compute(observedImage, observedKeypoints, observedDescriptors);
                    }

                    stopwatch.Stop();
                    EmguAssert.WriteLine(String.Format("Time to extract feature from image: {0} milli-sec", stopwatch.ElapsedMilliseconds));
                    #endregion

                    int maxWidth = Math.Max(modelImage.Width, observedImage.Width);

                    Mat res = new Mat(
                        new Size(maxWidth, modelImage.Height + observedImage.Height),
                        DepthType.Cv8U,
                        1);
                    res.SetTo(new MCvScalar(0, 0, 0, 0));
                    using (Mat roiTop = new Mat(res, new Rectangle(Point.Empty, modelImage.Size)))
                        modelImage.CopyTo(roiTop);
                    using (Mat roiBottom = new Mat(res, new Rectangle(new Point(0, modelImage.Height), observedImage.Size)))
                        observedImage.CopyTo(roiBottom);

                    Rectangle rect = new Rectangle(Point.Empty, modelImage.Size);
                    PointF[] pts = new PointF[] {
                        new PointF(rect.Left, rect.Bottom),
                        new PointF(rect.Right, rect.Bottom),
                        new PointF(rect.Right, rect.Top),
                        new PointF(rect.Left, rect.Top)};

                    Mat homography = null;

                    stopwatch.Reset();
                    stopwatch.Start();

                    int k = 2;
                    DistanceType dt = modelDescriptors.Depth == CvEnum.DepthType.Cv8U ? DistanceType.Hamming : DistanceType.L2;
                    using (VectorOfVectorOfDMatch matches = new VectorOfVectorOfDMatch())
                    using (BFMatcher matcher = new BFMatcher(dt))
                    {
                        matcher.Add(modelDescriptors);
                        matcher.KnnMatch(observedDescriptors, matches, k, null);

                        Mat mask = new Mat(matches.Size, 1, DepthType.Cv8U, 1);
                        mask.SetTo(new MCvScalar(255));
                        FeaturesToolbox.VoteForUniqueness(matches, 0.8, mask);

                        int nonZeroCount = CvInvoke.CountNonZero(mask);
                        if (nonZeroCount >= 4)
                        {
                            nonZeroCount = FeaturesToolbox.VoteForSizeAndOrientation(modelKeypoints, observedKeypoints, matches, mask, 1.5, 20);
                            if (nonZeroCount >= 4)
                                homography = FeaturesToolbox.GetHomographyMatrixFromMatchedFeatures(modelKeypoints, observedKeypoints, matches, mask, 2);
                        }
                    }
                    stopwatch.Stop();
                    EmguAssert.WriteLine(String.Format("Time for feature matching: {0} milli-sec", stopwatch.ElapsedMilliseconds));

                    bool success = false;
                    if (homography != null)
                    {
                        PointF[] points = pts.Clone() as PointF[];
                        points = CvInvoke.PerspectiveTransform(points, homography);

                        for (int i = 0; i < points.Length; i++)
                            points[i].Y += modelImage.Height;
                        CvInvoke.Polylines(res, Array.ConvertAll<PointF, Point>(points, Point.Round), true, new MCvScalar(255), 5);
                        success = true;
                    }
                    return success;
                }
            }
        }
    }
}
