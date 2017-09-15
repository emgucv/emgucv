//----------------------------------------------------------------------------
//  Copyright (C) 2004-2017 by EMGU Corporation. All rights reserved.       
//----------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;
using System.Text;
using System.Threading;
using System.Xml;
using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Features2D;
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
   public class AutoTestFeatures2d
   {
#if !ANDROID
      [Test]
      public void TestBrisk()
      {
         Brisk detector = new Brisk();
         EmguAssert.IsTrue(TestFeature2DTracker(detector, detector), "Unable to find homography matrix");
      }
#endif

      [Test]
      public void TestDAISY()
      {
         SURF surf = new SURF(300);
         DAISY  daisy = new DAISY();
         EmguAssert.IsTrue(TestFeature2DTracker(surf, daisy), "Unable to find homography matrix");
      }

      [Test]
      public void TestLATCH()
      {
         SURF surf = new SURF(300);
         LATCH latch = new LATCH();
         EmguAssert.IsTrue(TestFeature2DTracker(surf, latch), "Unable to find homography matrix");
      }

      /*
      [Test]
      public void TestLUCID()
      {
         SURF surf = new SURF(300);
         LUCID lucid = new LUCID();
         EmguAssert.IsTrue(TestFeature2DTracker(surf, lucid ), "Unable to find homography matrix");
      }*/

      [Test]
      public void TestSIFT()
      {
         SIFT detector = new SIFT();
         EmguAssert.IsTrue(TestFeature2DTracker(detector, detector), "Unable to find homography matrix");
      }

      /*
      [Test]
      public void TestDense()
      {
         DenseFeatureDetector detector = new DenseFeatureDetector(1.0f, 1, 0.1f, 6, 0, true, false); 
         SIFT extractor = new SIFT();
         EmguAssert.IsTrue(TestFeature2DTracker(detector, extractor), "Unable to find homography matrix");
      
      }*/

      [Test]
      public void TestSURF()
      {
         SURF detector = new SURF(500);
         //ParamDef[] parameters = detector.GetParams();
         EmguAssert.IsTrue(TestFeature2DTracker(detector, detector), "Unable to find homography matrix");
      }

      [Test]
      public void TestSURFBlankImage()
      {
         SURF detector = new SURF(500);
         Image<Gray, Byte> img = new Image<Gray, byte>(1024, 900);
         VectorOfKeyPoint vp = new VectorOfKeyPoint();
         Mat descriptors = new Mat();
         detector.DetectAndCompute(img, null, vp, descriptors, false);
      }

      [Test]
      public void TestStar()
      {
         StarDetector keyPointDetector = new StarDetector();

         //SURF descriptorGenerator = new SURF(500, false);
         SIFT descriptorGenerator = new SIFT();
         //ParamDef[] parameters = keyPointDetector.GetParams();
         TestFeature2DTracker(keyPointDetector, descriptorGenerator);
      }

      [Test]
      public void TestGFTTDetector()
      {
         GFTTDetector keyPointDetector = new GFTTDetector(1000, 0.01, 1, 3, false, 0.04);
         SIFT descriptorGenerator = new SIFT();
         //ParamDef[] parameters = keyPointDetector.GetParams();
         TestFeature2DTracker(keyPointDetector, descriptorGenerator);
      }

      /*
      [Test]
      public void TestDenseFeatureDetector()
      {
         DenseFeatureDetector keyPointDetector = new DenseFeatureDetector(1, 1, 0.1f, 6, 0, true, false);
         SIFT descriptorGenerator = new SIFT();
         TestFeature2DTracker(keyPointDetector, descriptorGenerator);
      }*/

      /*
      [Test]
      public void TestLDetector()
      {
         LDetector keyPointDetector = new LDetector();
         keyPointDetector.Init();
         
         //SURF descriptorGenerator = new SURF(500, false);
         SIFT descriptorGenerator = new SIFT(4, 3, -1, SIFT.AngleMode.AVERAGE_ANGLE, 0.04 / 3 / 2.0, 10.0, 3.0, true, true);

         TestFeature2DTracker(keyPointDetector, descriptorGenerator);
      }*/


      [Test]
      public void TestMSER()
      {
         MSERDetector keyPointDetector = new MSERDetector();
         SIFT descriptorGenerator = new SIFT();
         //ParamDef[] parameters = keyPointDetector.GetParams();
         TestFeature2DTracker(keyPointDetector, descriptorGenerator);
      }

      /*
      [Test]
      public void TestMSERContour()
      {
         Image<Gray, Byte> image = new Image<Gray, byte>("stuff.jpg");
         MSERDetector param = new MSERDetector();

         using (MemStorage storage = new MemStorage())
         {
            Seq<Point>[] mser = param.ExtractContours(image, null, storage);
            {
               foreach (Seq<Point> region in mser)
                  image.Draw(region, new Gray(255.0), 2);
            }
         }
      }*/

      [Test]
      public void TestFAST()
      {
         FastDetector fast = new FastDetector(10, true);
         //GridAdaptedFeatureDetector fastGrid = new GridAdaptedFeatureDetector(fast, 2000, 4, 4);
         BriefDescriptorExtractor brief = new BriefDescriptorExtractor(32);
         //ParamDef[] parameters = fastGrid.GetParams();
         EmguAssert.IsTrue(TestFeature2DTracker(fast, brief), "Unable to find homography matrix");
      }

      [Test]
      public void TestORB()
      {
         ORBDetector orb = new ORBDetector(700);
         //String[] parameters = orb.GetParamNames();
         EmguAssert.IsTrue(TestFeature2DTracker(orb, orb), "Unable to find homography matrix");
      }

      [Test]
      public void TestFreak()
      {
         FastDetector fast = new FastDetector(10, true);
         Freak freak = new Freak(true, true, 22.0f, 4);
         //ParamDef[] parameters = freak.GetParams();
         //int nOctaves = freak.GetInt("nbOctave");
         EmguAssert.IsTrue(TestFeature2DTracker(fast, freak), "Unable to find homography matrix");
      }

      public static bool TestFeature2DTracker(Feature2D keyPointDetector, Feature2D descriptorGenerator)
      {
         //for (int k = 0; k < 1; k++)
         {
            Feature2D feature2D = null;
            if (keyPointDetector == descriptorGenerator)
            {
               feature2D = keyPointDetector as Feature2D;
            }

            Mat modelImage = EmguAssert.LoadMat("box.png");
            //Image<Gray, Byte> modelImage = new Image<Gray, byte>("stop.jpg");
            //modelImage = modelImage.Resize(400, 400, true);

            //modelImage._EqualizeHist();

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

            //Image<Gray, Byte> observedImage = new Image<Gray, byte>("traffic.jpg");
            Image<Gray, Byte> observedImage = EmguAssert.LoadImage<Gray, byte>("box_in_scene.png");
            //Image<Gray, Byte> observedImage = modelImage.Rotate(45, new Gray(0.0));
            //image = image.Resize(400, 400, true);

            //observedImage._EqualizeHist();
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

               //Merge the object image and the observed image into one big image for display
               Image<Gray, Byte> res = modelImage.ToImage<Gray, Byte>().ConcateVertical(observedImage);

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
               //using (Matrix<int> indices = new Matrix<int>(observedDescriptors.Rows, k))
               //using (Matrix<float> dist = new Matrix<float>(observedDescriptors.Rows, k))
               using (VectorOfVectorOfDMatch matches = new VectorOfVectorOfDMatch())
               using (BFMatcher matcher = new BFMatcher(dt))
               {
                  //ParamDef[] parameterDefs = matcher.GetParams();
                  matcher.Add(modelDescriptors);
                  matcher.KnnMatch(observedDescriptors, matches, k, null);

                  Mat mask = new Mat(matches.Size, 1, DepthType.Cv8U, 1);
                  mask.SetTo(new MCvScalar(255));
                  //mask.SetValue(255);
                  Features2DToolbox.VoteForUniqueness(matches, 0.8, mask);

                  int nonZeroCount = CvInvoke.CountNonZero(mask);
                  if (nonZeroCount >= 4)
                  {
                     nonZeroCount = Features2DToolbox.VoteForSizeAndOrientation(modelKeypoints, observedKeypoints, matches, mask, 1.5, 20);
                     if (nonZeroCount >= 4)
                        homography = Features2DToolbox.GetHomographyMatrixFromMatchedFeatures(modelKeypoints, observedKeypoints, matches, mask, 2);
                  }
               }
               stopwatch.Stop();
               EmguAssert.WriteLine(String.Format("Time for feature matching: {0} milli-sec", stopwatch.ElapsedMilliseconds));

               bool success = false;
               if (homography != null)
               {
                  PointF[] points = pts.Clone() as PointF[];
                  points = CvInvoke.PerspectiveTransform(points, homography);
                  //homography.ProjectPoints(points);

                  for (int i = 0; i < points.Length; i++)
                     points[i].Y += modelImage.Height;
                  
                  res.DrawPolyline(
#if NETFX_CORE
                     Extensions.
#else
                     Array.
#endif
                     ConvertAll<PointF, Point>(points, Point.Round), true, new Gray(255.0), 5);

                  success = true;
               }
               //Emgu.CV.UI.ImageViewer.Show(res);
               return success;
            }

            

            /*
            stopwatch.Reset(); stopwatch.Start();
            //set the initial region to be the whole image
            using (Image<Gray, Single> priorMask = new Image<Gray, float>(observedImage.Size))
            {
               priorMask.SetValue(1.0);
               homography = tracker.CamShiftTrack(
                  observedFeatures,
                  (RectangleF)observedImage.ROI,
                  priorMask);
            }
            Trace.WriteLine(String.Format("Time for feature tracking: {0} milli-sec", stopwatch.ElapsedMilliseconds));
            
            if (homography != null) //set the initial tracking window to be the whole image
            {
               PointF[] points = pts.Clone() as PointF[];
               homography.ProjectPoints(points);

               for (int i = 0; i < points.Length; i++)
                  points[i].Y += modelImage.Height;
               res.DrawPolyline(Array.ConvertAll<PointF, Point>(points, Point.Round), true, new Gray(255.0), 5);
               return true;
            }
            else
            {
               return false;
            }*/

         }
      }

      /*
      [Test]
      public void TestDetectorColor()
      {
         Image<Bgr, byte> box = EmguAssert.LoadImage<Bgr, byte>("box.png");
         Image<Gray, byte> gray = box.Convert<Gray, Byte>();

         SURF surf = new SURF(400);
         OpponentColorDescriptorExtractor opponentSurf = new OpponentColorDescriptorExtractor(surf);
         SIFT sift = new SIFT();
         OpponentColorDescriptorExtractor opponentSift = new OpponentColorDescriptorExtractor(sift);
         //using (Util.VectorOfKeyPoint kpts = surf.DetectKeyPointsRaw(gray, null))
         using (Util.VectorOfKeyPoint kpts = new VectorOfKeyPoint() )
         {
            sift.DetectRaw(gray, kpts);
            for (int i = 1; i < 2; i++)
            {
               using (Mat surfDescriptors =  new Mat())
               {
                  opponentSurf.Compute(box, kpts, surfDescriptors);
                  //EmguAssert.IsTrue(surfDescriptors.Width == (surf.SURFParams.Extended == 0 ? 64 : 128) * 3);
               }

               //TODO: Find out why the following test fails
               using (Mat siftDescriptors = new Mat())
               {
                  sift.Compute(gray, kpts, siftDescriptors);
                  EmguAssert.IsTrue(siftDescriptors.Cols == sift.GetDescriptorSize());
               }

               int siftDescriptorSize = sift.GetDescriptorSize();
               using (Mat siftDescriptors = new Mat())
               {
                  opponentSift.Compute(box, kpts, siftDescriptors);
                  EmguAssert.IsTrue(siftDescriptors.Cols == siftDescriptorSize * 3);
               }
            }
         }
      }*/

      [Test]
      public void TestSURFDetector2()
      {
         //Trace.WriteLine("Size of MCvSURFParams: " + Marshal.SizeOf(typeof(MCvSURFParams)));
         Image<Gray, byte> box = EmguAssert.LoadImage<Gray, byte>("box.png");
         SURF detector = new SURF(400);

         Stopwatch watch = Stopwatch.StartNew();
         VectorOfKeyPoint vp1 = new VectorOfKeyPoint();
         Mat descriptors1 = new Mat();
         detector.DetectAndCompute(box, null, vp1, descriptors1, false);
         watch.Stop();
         EmguAssert.WriteLine(String.Format("Time used: {0} milliseconds.", watch.ElapsedMilliseconds));

         watch.Reset();
         watch.Start();
         MKeyPoint[] keypoints = detector.Detect(box, null);
         //ImageFeature<float>[] features2 = detector.Compute(box, keypoints);
         watch.Stop();
         EmguAssert.WriteLine(String.Format("Time used: {0} milliseconds.", watch.ElapsedMilliseconds));

         watch.Reset();
         watch.Start();
         //MCvSURFParams p = detector.SURFParams;
        
         //SURFFeature[] features3 = box.ExtractSURF(ref p);
         //watch.Stop();
         //EmguAssert.WriteLine(String.Format("Time used: {0} milliseconds.", watch.ElapsedMilliseconds));

        // EmguAssert.IsTrue(features1.Length == features2.Length);
         //EmguAssert.IsTrue(features2.Length == features3.Length);

         PointF[] pts =
#if NETFX_CORE
            Extensions.
#else
            Array.
#endif
            ConvertAll<MKeyPoint, PointF>(keypoints, delegate(MKeyPoint mkp)
         {
            return mkp.Point;
         });
         //SURFFeature[] features = box.ExtractSURF(pts, null, ref detector);
         //int count = features.Length;

         /*
         for (int i = 0; i < features1.Length; i++)
         {
            Assert.AreEqual(features1[i].KeyPoint.Point, features2[i].KeyPoint.Point);
            float[] d1 = features1[i].Descriptor;
            float[] d2 = features2[i].Descriptor;

            for (int j = 0; j < d1.Length; j++)
               Assert.AreEqual(d1[j], d2[j]);
         }*/

         foreach (MKeyPoint kp in keypoints)
         {
            box.Draw(new CircleF(kp.Point, kp.Size), new Gray(255), 1);
         }
      }

      /*
      [Test]
      public void TestGridAdaptedFeatureDetectorRepeatedRun()
      {
         Image<Gray, byte> box = EmguAssert.LoadImage<Gray, byte>("box.png");
         SURF surfdetector = new SURF(400);

         GridAdaptedFeatureDetector detector = new GridAdaptedFeatureDetector(surfdetector, 1000, 2, 2);
         VectorOfKeyPoint kpts1 = new VectorOfKeyPoint();
         detector.DetectRaw(box, kpts1);
         VectorOfKeyPoint kpts2 = new VectorOfKeyPoint();
         detector.DetectRaw(box, kpts2);
         EmguAssert.IsTrue(kpts1.Size == kpts2.Size);
      }*/

      /*
      [Test]
      public void TestSURFDetectorRepeatedRun()
      {
         Image<Gray, byte> box = EmguAssert.LoadImage<Gray, byte>("box.png");
         SURF detector = new SURF(400);
         Image<Gray, Byte> boxInScene = EmguAssert.LoadImage<Gray, byte>("box_in_scene.png");
         ImageFeature<float>[] features1 = detector.DetectAndCompute(box, null);
         Features2DTracker<float> tracker = new Features2DTracker<float>(features1);

         ImageFeature<float>[] imageFeatures = detector.DetectAndCompute(boxInScene, null);
         Features2DTracker<float>.MatchedImageFeature[] matchedFeatures = tracker.MatchFeature(imageFeatures, 2);
         int length1 = matchedFeatures.Length;
         matchedFeatures = Features2DTracker<float>.VoteForUniqueness(matchedFeatures, 0.8);
         int length2 = matchedFeatures.Length;
         matchedFeatures = Features2DTracker<float>.VoteForSizeAndOrientation(matchedFeatures, 1.5, 20);
         int length3 = matchedFeatures.Length;

         for (int i = 0; i < 100; i++)
         {
            Features2DTracker<float>.MatchedImageFeature[] matchedFeaturesNew = tracker.MatchFeature(imageFeatures, 2);
            EmguAssert.IsTrue(length1 == matchedFeaturesNew.Length, String.Format("Failed in iteration {0}", i));
            
            //for (int j = 0; j < length1; j++)
            //{
            //   Features2DTracker.MatchedImageFeature oldMF = matchedFeatures[j];
            //   Features2DTracker.MatchedImageFeature newMF = matchedFeaturesNew[j];
            //   for (int k = 0; k < oldMF.SimilarFeatures.Length; k++)
            //   {
            //      Assert.AreEqual(oldMF.SimilarFeatures[k].Distance, newMF.SimilarFeatures[k].Distance, String.Format("Failed in iteration {0}", i)); 
            //   }
            //}
            matchedFeaturesNew = Features2DTracker<float>.VoteForUniqueness(matchedFeaturesNew, 0.8);
            EmguAssert.IsTrue(length2 == matchedFeaturesNew.Length, String.Format("Failed in iteration {0}", i));
            matchedFeaturesNew = Features2DTracker<float>.VoteForSizeAndOrientation(matchedFeaturesNew, 1.5, 20);
            EmguAssert.IsTrue(length3 == matchedFeaturesNew.Length, String.Format("Failed in iteration {0}", i));
         }
      }
      
      [Test]
      public void TestSelfMatch()
      {
         Image<Gray, byte> box = EmguAssert.LoadImage<Gray, byte>("box.png");
         SURF surfDetector = new SURF(300);
         ImageFeature<float>[] features1 = surfDetector.DetectAndCompute(box, null);
         Features2DTracker<float> tracker = new Features2DTracker<float>(features1);
         HomographyMatrix m = tracker.Detect(features1, 0.8);
      }

      [Test]
      public void TestLDetectorAndSelfSimDescriptor()
      {
         Image<Gray, byte> box = EmguAssert.LoadImage<Gray, byte>("box.png");
         LDetector detector = new LDetector();
         detector.Init();

         MKeyPoint[] keypoints = detector.DetectKeyPoints(box, 200, true);

         Point[] pts = Array.ConvertAll<MKeyPoint, Point>(keypoints, delegate(MKeyPoint k)
         {
            return Point.Round(k.Point);
         });

         SelfSimDescriptor descriptor = new SelfSimDescriptor(5, 41, 3, 7, 20);
         int descriptorSize = descriptor.DescriptorSize;

         float[] descriptors = descriptor.Compute(box.Mat, new Size(20, 20), pts);

         float absSum = 0;
         foreach (float f in descriptors)
            absSum += Math.Abs(f);

         EmguAssert.IsTrue(0 != absSum, "The sum of the descriptor should not be zero");

         EmguAssert.IsTrue(descriptors.Length / descriptor.DescriptorSize == pts.Length);

         foreach (MKeyPoint kp in keypoints)
         {
            box.Draw(new CircleF(kp.Point, kp.Size), new Gray(255), 1);
         }
      }*/

      [Test]
      public void TestBOWKmeansTrainer()
      {
         Image<Gray, byte> box = EmguAssert.LoadImage<Gray, byte>("box.png");
         SURF detector = new SURF(500);
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
         Image<Gray, byte> box = EmguAssert.LoadImage<Gray, byte>("box.png");
         Brisk detector = new Brisk(30, 3, 1.0f);
         VectorOfKeyPoint kpts = new VectorOfKeyPoint();
         Mat descriptors = new Mat();
         detector.DetectAndCompute(box, null, kpts, descriptors, false);
         Mat descriptorsF = new Mat();
         descriptors.ConvertTo(descriptorsF, CvEnum.DepthType.Cv32F);
         //Matrix<float> descriptorsF = descriptors.Convert<float>();
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

      [Test]
      public void TestSimpleBlobDetector()
      {
         Mat box = EmguAssert.LoadMat("box.png");
         SimpleBlobDetectorParams p = new SimpleBlobDetectorParams();
         SimpleBlobDetector detector = new SimpleBlobDetector(p);
         MKeyPoint[] keypoints = detector.Detect(box);
      }
   }
}
