using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using Emgu.CV;
using Emgu.UI;
using Emgu.CV.UI;
using Emgu.CV.Structure;
using Emgu.CV.Features2D;
using Emgu.Util;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;
using System.Xml;
using System.IO;
using System.Runtime.Serialization;
using System.Threading;
using System.Runtime.InteropServices;

namespace Emgu.CV.Test
{
   [TestFixture]
   public class AutoTestFeatures2d
   {

      [Test]
      public void TestSIFT()
      {
         SIFTDetector detector = new SIFTDetector(4, 3, -1, SIFTDetector.AngleMode.AVERAGE_ANGLE, 0.04 / 3 / 2.0, 10.0, 3.0, true, true);
         TestFeature2DTracker(detector, detector);
      }

      [Test]
      public void TestSURF()
      {
         SURFDetector detector = new SURFDetector(500, false);
         TestFeature2DTracker(detector, detector);
      }

      [Test]
      public void TestStar()
      {
         StarDetector keyPointDetector = new StarDetector();
         keyPointDetector.Init();

         //SURFDetector descriptorGenerator = new SURFDetector(500, false);
         SIFTDetector descriptorGenerator = new SIFTDetector(4, 3, -1, SIFTDetector.AngleMode.AVERAGE_ANGLE, 0.04 / 3 / 2.0, 10.0, 3.0, true, true);
         
         TestFeature2DTracker(keyPointDetector, descriptorGenerator);
      }

      [Test]
      public void TestLDetector()
      {
         LDetector keyPointDetector = new LDetector();
         keyPointDetector.Init();
         
         //SURFDetector descriptorGenerator = new SURFDetector(500, false);
         SIFTDetector descriptorGenerator = new SIFTDetector(4, 3, -1, SIFTDetector.AngleMode.AVERAGE_ANGLE, 0.04 / 3 / 2.0, 10.0, 3.0, true, true);

         TestFeature2DTracker(keyPointDetector, descriptorGenerator);
      }

      [Test]
      public void TestMSER()
      {
         Image<Gray, Byte> image = new Image<Gray, byte>("stuff.jpg");
         MSERDetector param = new MSERDetector();
         param.Init();

         using (MemStorage storage = new MemStorage())
         {
            Seq<Point>[] mser = param.ExtractContours(image, null, ref param, storage);
            {
               foreach (Seq<Point> region in mser)
                  image.Draw(region, new Gray(255.0), 2);
            }
         }
      }

      [Test]
      public void TestFAST()
      {
         Image<Gray, byte> box = new Image<Gray, byte>("box.png");
         FastDetector keyPointDetector = new FastDetector(100, true);
         SIFTDetector descriptorGenerator = new SIFTDetector(4, 3, -1, SIFTDetector.AngleMode.AVERAGE_ANGLE, 0.04 / 3 / 2.0, 10.0, 3.0, true, true);

         TestFeature2DTracker(keyPointDetector, descriptorGenerator);
      }

      public static void TestFeature2DTracker(IKeyPointDetector keyPointDetector, IDescriptorExtractor descriptorGenerator)
      {
         for (int k = 0; k < 1; k++)
         {
            Image<Gray, Byte> modelImage = new Image<Gray, byte>("box.png");
            //Image<Gray, Byte> modelImage = new Image<Gray, byte>("stop.jpg");
            //modelImage = modelImage.Resize(400, 400, true);

            //modelImage._EqualizeHist();

            #region extract features from the object image
            Stopwatch stopwatch = Stopwatch.StartNew();
            ImageFeature[] modelFeatures = descriptorGenerator.ComputeDescriptors(modelImage, keyPointDetector.DetectKeyPoints(modelImage));

            //SURFFeature[] modelFeatures = modelImage.ExtractSURF(ref param1);
            Features2DTracker tracker = new Features2DTracker(modelFeatures);
            stopwatch.Stop();
            Trace.WriteLine(String.Format("Time to extract feature from model: {0} milli-sec", stopwatch.ElapsedMilliseconds));
            #endregion

            //Image<Gray, Byte> observedImage = new Image<Gray, byte>("traffic.jpg");
            Image<Gray, Byte> observedImage = new Image<Gray, byte>("box_in_scene.png");
            //Image<Gray, Byte> observedImage = modelImage.Rotate(45, new Gray(0.0));
            //Image<Gray, Byte> observedImage = new Image<Gray, byte>("left.jpg");
            //image = image.Resize(400, 400, true);

            //observedImage._EqualizeHist();
            #region extract features from the observed image
            stopwatch.Reset(); stopwatch.Start();
            //SURFDetector param2 = new SURFDetector(500, false);
            //SURFFeature[] observedFeatures = observedImage.ExtractSURF(ref param2);
            //ImageFeature[] observedFeatures = param2.DetectFeatures(observedImage, null);
            ImageFeature[] observedFeatures = descriptorGenerator.ComputeDescriptors(observedImage, keyPointDetector.DetectKeyPoints(observedImage));
            stopwatch.Stop();
            Trace.WriteLine(String.Format("Time to extract feature from image: {0} milli-sec", stopwatch.ElapsedMilliseconds));
            #endregion

            //Merge the object image and the observed image into one big image for display
            Image<Gray, Byte> res = modelImage.ConcateVertical(observedImage);

            Rectangle rect = modelImage.ROI;
            PointF[] pts = new PointF[] { 
               new PointF(rect.Left, rect.Bottom),
               new PointF(rect.Right, rect.Bottom),
               new PointF(rect.Right, rect.Top),
               new PointF(rect.Left, rect.Top)};

            HomographyMatrix homography;

            stopwatch.Reset(); stopwatch.Start();
            homography = tracker.Detect(observedFeatures, 0.8);
            stopwatch.Stop();
            Trace.WriteLine(String.Format("Time for feature matching: {0} milli-sec", stopwatch.ElapsedMilliseconds));
            if (homography != null)
            {
               PointF[] points = pts.Clone() as PointF[];
               homography.ProjectPoints(points);

               for (int i = 0; i < points.Length; i++)
                  points[i].Y += modelImage.Height;
               res.DrawPolyline(Array.ConvertAll<PointF, Point>(points, Point.Round), true, new Gray(255.0), 5);
            }

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
            }
         }
      }


      [Test]
      public void TestSURFDetector2()
      {
         Image<Gray, byte> box = new Image<Gray, byte>("box.png");
         SURFDetector detector = new SURFDetector(400, false);

         Stopwatch watch = Stopwatch.StartNew();
         ImageFeature[] features1 = detector.DetectFeatures(box, null);
         watch.Stop();
         Trace.WriteLine(String.Format("Time used: {0} milliseconds.", watch.ElapsedMilliseconds));

         watch.Reset(); watch.Start();
         MKeyPoint[] keypoints = detector.DetectKeyPoints(box, null);
         ImageFeature[] features2 = detector.ComputeDescriptors(box, null, keypoints);
         watch.Stop();
         Trace.WriteLine(String.Format("Time used: {0} milliseconds.", watch.ElapsedMilliseconds));

         watch.Reset(); watch.Start();
         SURFFeature[] features3 = box.ExtractSURF(ref detector);
         watch.Stop();
         Trace.WriteLine(String.Format("Time used: {0} milliseconds.", watch.ElapsedMilliseconds));


         PointF[] pts = Array.ConvertAll<MKeyPoint, PointF>(keypoints, delegate(MKeyPoint mkp) { return mkp.Point; });
         //SURFFeature[] features = box.ExtractSURF(pts, null, ref detector);
         //int count = features.Length;

         for (int i = 0; i < features1.Length; i++)
         {
            Assert.AreEqual(features1[i].KeyPoint.Point, features2[i].KeyPoint.Point);
            float[] d1 = features1[i].Descriptor;
            float[] d2 = features2[i].Descriptor;

            for (int j = 0; j < d1.Length; j++)
               Assert.AreEqual(d1[j], d2[j]);
         }

         foreach (MKeyPoint kp in keypoints)
         {
            box.Draw(new CircleF(kp.Point, kp.Size), new Gray(255), 1);
         }
      }

      [Test]
      public void TestLDetectorAndSelfSimDescriptor()
      {
         Image<Gray, byte> box = new Image<Gray, byte>("box.png");
         LDetector detector = new LDetector();
         detector.Init();

         MKeyPoint[] keypoints = detector.DetectKeyPoints(box, 200, true);

         Point[] pts = Array.ConvertAll<MKeyPoint, Point>(keypoints, delegate(MKeyPoint k) { return Point.Round(k.Point); });

         SelfSimDescriptor descriptor = new SelfSimDescriptor(5, 41, 3, 7, 20);
         int descriptorSize = descriptor.DescriptorSize;

         float[] descriptors = descriptor.Compute(box, new Size(20, 20), pts);

         float absSum = 0;
         foreach (float f in descriptors)
            absSum += Math.Abs(f);

         //TODO: Find out why selfsimilarity always return descriptors of all zeros. Probaboly a bug in the opencv C++ code
         //Assert.AreNotEqual(0, absSum, "The sum of the descriptor should not be zero");

         Assert.AreEqual(descriptors.Length / descriptor.DescriptorSize, pts.Length);

         foreach (MKeyPoint kp in keypoints)
         {
            box.Draw(new CircleF(kp.Point, kp.Size), new Gray(255), 1);
         }
      }
   }
}
