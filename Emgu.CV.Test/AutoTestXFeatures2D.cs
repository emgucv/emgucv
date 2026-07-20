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
using Emgu.CV.XFeatures2D;
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
    public class AutoTestXFeatures2D
    {
#if !ANDROID
        [Test]
        public void TestBrisk()
        {
            Brisk detector = new Brisk();
            EmguAssert.IsTrue(AutoTestFeatures.TestFeature2DTracker(detector, detector), "Unable to find homography matrix");
        }
#endif

        [Test]
        public void TestTEBLID()
        {
            AKAZE akaze = new AKAZE();
            TEBLID teblid = new TEBLID(5.00f, TEBLID.TeblidSize.BitSize256);
            AutoTestFeatures.TestFeature2DTracker(akaze, teblid);
        }

        [Test]
        public void TestDAISY()
        {
            AKAZE akaze = new AKAZE();
            DAISY daisy = new DAISY();
            AutoTestFeatures.TestFeature2DTracker(akaze, daisy);
        }

        [Test]
        public void TestLATCH()
        {
            AKAZE akaze = new AKAZE();
            LATCH latch = new LATCH();
            AutoTestFeatures.TestFeature2DTracker(akaze, latch);
        }

        [Test]
        public void TestAgast()
        {
            AgastFeatureDetector agast = new AgastFeatureDetector();
            SIFT sift = new SIFT();
            AutoTestFeatures.TestFeature2DTracker(agast, sift);
        }

        [Test]
        public void TestAkaze()
        {
            using (AKAZE detector = new AKAZE())
            {
                EmguAssert.IsTrue(AutoTestFeatures.TestFeature2DTracker(detector, detector), "Unable to find homography matrix");
            }
        }

        [Test]
        public void TestAkazeBlankImage()
        {
            using (AKAZE detector = new AKAZE())
            using (Mat img = new Mat(1024, 900, DepthType.Cv8U, 1))
            using (VectorOfKeyPoint vp = new VectorOfKeyPoint())
            {
                img.SetTo(new MCvScalar());
                Mat descriptors = new Mat();
                detector.DetectAndCompute(img, null, vp, descriptors, false);
            }
        }

        [Test]
        public void TestStar()
        {
            using (StarDetector keyPointDetector = new StarDetector())
            using (BriefDescriptorExtractor descriptorGenerator = new BriefDescriptorExtractor(32))
                AutoTestFeatures.TestFeature2DTracker(keyPointDetector, descriptorGenerator);
        }

        [Test]
        public void TestGFTTDetector()
        {
            using (GFTTDetector keyPointDetector = new GFTTDetector(1000, 0.01, 1, 3, false, 0.04))
            using (BriefDescriptorExtractor descriptorGenerator = new BriefDescriptorExtractor(32))
                AutoTestFeatures.TestFeature2DTracker(keyPointDetector, descriptorGenerator);
        }

        [Test]
        public void TestMSER()
        {
            using (MSER keyPointDetector = new MSER())
            using (BriefDescriptorExtractor descriptorGenerator = new BriefDescriptorExtractor(32))
                AutoTestFeatures.TestFeature2DTracker(keyPointDetector, descriptorGenerator);
        }

        [Test]
        public void TestFAST()
        {
            using (FastFeatureDetector fast = new FastFeatureDetector(10, true))
            using (BriefDescriptorExtractor brief = new BriefDescriptorExtractor(32))
            {
                EmguAssert.IsTrue(AutoTestFeatures.TestFeature2DTracker(fast, brief), "Unable to find homography matrix");
            }
        }

        [Test]
        public void TestFreak()
        {
            using (FastFeatureDetector fast = new FastFeatureDetector(10, true))
            using (Freak freak = new Freak(true, true, 22.0f, 4))
            {
                EmguAssert.IsTrue(AutoTestFeatures.TestFeature2DTracker(fast, freak), "Unable to find homography matrix");
            }
        }

        [Test]
        public void TestBOWKmeansTrainer()
        {
            Mat box = EmguAssert.LoadMat("box.png", ImreadModes.Grayscale);
            Feature2D detector = new KAZE();
            VectorOfKeyPoint kpts = new VectorOfKeyPoint();
            Mat descriptors = new Mat();
            detector.DetectAndCompute(box, null, kpts, descriptors, false);

            BOWKMeansTrainer trainer = new BOWKMeansTrainer(100, new MCvTermCriteria(), 3, CvEnum.KMeansInitType.PPCenters);
            trainer.Add(descriptors);
            Mat vocabulary = new Mat();
            trainer.Cluster(vocabulary);

            BFMatcher matcher = new BFMatcher(DistanceType.L2);

            BOWImgDescriptorExtractor extractor = new BOWImgDescriptorExtractor(detector, matcher);
            extractor.SetVocabulary(vocabulary);

            Mat descriptors2 = new Mat();
            extractor.Compute(box, kpts, descriptors2);
        }

        [Test]
        public void TestBOWKmeansTrainer2()
        {
            Mat box = EmguAssert.LoadMat("box.png", ImreadModes.Grayscale);
            Brisk detector = new Brisk(30, 3, 1.0f);
            VectorOfKeyPoint kpts = new VectorOfKeyPoint();
            Mat descriptors = new Mat();
            detector.DetectAndCompute(box, null, kpts, descriptors, false);
            Mat descriptorsF = new Mat();
            descriptors.ConvertTo(descriptorsF, CvEnum.DepthType.Cv32F);
            BOWKMeansTrainer trainer = new BOWKMeansTrainer(100, new MCvTermCriteria(), 3, CvEnum.KMeansInitType.PPCenters);
            trainer.Add(descriptorsF);
            Mat vocabulary = new Mat();
            trainer.Cluster(vocabulary);

            BFMatcher matcher = new BFMatcher(DistanceType.L2);

            BOWImgDescriptorExtractor extractor = new BOWImgDescriptorExtractor(detector, matcher);
            Mat vocabularyByte = new Mat();
            vocabulary.ConvertTo(vocabularyByte, CvEnum.DepthType.Cv8U);
            extractor.SetVocabulary(vocabularyByte);

            Mat descriptors2 = new Mat();
            extractor.Compute(box, kpts, descriptors2);
        }
    }
}
