//----------------------------------------------------------------------------
//  Copyright (C) 2004-2011 by EMGU. All rights reserved.       
//----------------------------------------------------------------------------

﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Text;
using Emgu.CV;
using Emgu.CV.GPU;
using Emgu.CV.Structure;
using Emgu.CV.UI;
using Emgu.CV.Util;
using Emgu.CV.Features2D;
using NUnit.Framework;

namespace Emgu.CV.GPU.Test
{
   [TestFixture]
   public class TestGpuMat
   {
      [Test]
      public void TestGetCudaEnabledDeviceCount()
      {
         if (GpuInvoke.HasCuda)
         {
            int deviceCount = GpuInvoke.GetCudaEnabledDeviceCount();
            Trace.WriteLine(String.Format("Device count: {0}", deviceCount));
            if (deviceCount > 0)
            {
               GpuDeviceInfo d0 = new GpuDeviceInfo(0);
            }
         }
      }

      /*
      public int MemTest()
      {
         while (true)
         {
            using (GpuMat<Byte> m = new GpuMat<Byte>(320, 240, 1))
            {
            }
         }
      }*/

      [Test]
      public void TestGpuImageAsyncOps()
      {
         if (GpuInvoke.HasCuda)
         {
            int counter = 0;
            Stopwatch watch = Stopwatch.StartNew();
            using (GpuImage<Bgr, byte> img1 = new GpuImage<Bgr, byte>(3000, 2000))
            using (GpuImage<Bgr, Byte> img2 = new GpuImage<Bgr, Byte>(img1.Size))
            using (GpuImage<Gray, Byte> img3 = new GpuImage<Gray, byte>(img2.Size))
            using (Stream stream = new Stream())
            using (GpuMat<float> mat1 = img1.Convert<float>(stream))
            {
               while (!stream.Completed)
               {
                  if (counter <= int.MaxValue) counter++;
               }
               Trace.WriteLine(String.Format("Counter has been incremented {0} times", counter));

               counter = 0;
               GpuInvoke.CvtColor(img2, img3, CvToolbox.GetColorCvtCode(typeof(Bgr), typeof(Gray)), stream);
               while (!stream.Completed)
               {
                  if (counter <= int.MaxValue) counter++;
               }
               Trace.WriteLine(String.Format("Counter has been incremented {0} times", counter));
            }
            watch.Stop();
            Trace.WriteLine(String.Format("Total time: {0} milliseconds", watch.ElapsedMilliseconds));
         }
      }

      [Test]
      public void TestGpuMatContinuous()
      {
         GpuMat<Byte> mat = new GpuMat<byte>(1200, 640, 1, true);
         Assert.IsTrue(mat.IsContinuous);
      }

      [Test]
      public void TestGpuMatRange()
      {
         if (GpuInvoke.HasCuda)
         {
            Image<Gray, Byte> img1 = new Image<Gray, byte>(1200, 640);
            img1.SetRandUniform(new MCvScalar(0, 0, 0), new MCvScalar(255, 255, 255));
            using (GpuImage<Gray, Byte> gpuImg1 = new GpuImage<Gray, byte>(img1))
            using (GpuMat<byte> mat = new GpuMat<byte>(gpuImg1, new MCvSlice(0, 1), MCvSlice.WholeSeq))
            {
               Size s = mat.Size;
            }
         }
      }

      [Test]
      public void TestGpuMatAdd()
      {
         if (GpuInvoke.HasCuda)
         {
            int repeat = 1000;
            Image<Gray, Byte> img1 = new Image<Gray, byte>(1200, 640);
            Image<Gray, Byte> img2 = new Image<Gray, byte>(img1.Size);
            img1.SetRandUniform(new MCvScalar(0, 0, 0), new MCvScalar(255, 255, 255));
            img2.SetRandUniform(new MCvScalar(0, 0, 0), new MCvScalar(255, 255, 255));
            Image<Gray, Byte> cpuImgSum = new Image<Gray, byte>(img1.Size);
            Stopwatch watch = Stopwatch.StartNew();
            for (int i = 0; i < repeat; i++)
               CvInvoke.cvAdd(img1, img2, cpuImgSum, IntPtr.Zero);
            watch.Stop();
            Trace.WriteLine(String.Format("CPU processing time: {0}ms", (double)watch.ElapsedMilliseconds / repeat));

            watch.Reset(); watch.Start();
            GpuImage<Gray, Byte> gpuImg1 = new GpuImage<Gray, byte>(img1);
            GpuImage<Gray, Byte> gpuImg2 = new GpuImage<Gray, byte>(img2);
            GpuImage<Gray, Byte> gpuImgSum = new GpuImage<Gray, byte>(gpuImg1.Size);
            Stopwatch watch2 = Stopwatch.StartNew();
            for (int i = 0; i < repeat; i++)
               GpuInvoke.Add(gpuImg1, gpuImg2, gpuImgSum, IntPtr.Zero);
            watch2.Stop();
            Image<Gray, Byte> cpuImgSumFromGpu = gpuImgSum.ToImage();
            watch.Stop();
            Trace.WriteLine(String.Format("Core GPU processing time: {0}ms", (double)watch2.ElapsedMilliseconds / repeat));
            //Trace.WriteLine(String.Format("Total GPU processing time: {0}ms", (double)watch.ElapsedMilliseconds/repeat));

            Assert.IsTrue(cpuImgSum.Equals(cpuImgSumFromGpu));
         }
      }

      [Test]
      public void TestSplitMerge()
      {
         if (GpuInvoke.HasCuda)
         {
            using (Image<Bgr, Byte> img1 = new Image<Bgr, byte>(1200, 640))
            {
               img1.SetRandUniform(new MCvScalar(0, 0, 0), new MCvScalar(255, 255, 255));

               using (GpuImage<Bgr, Byte> gpuImg1 = new GpuImage<Bgr, byte>(img1))
               {
                  GpuImage<Gray, Byte>[] channels = gpuImg1.Split(null);

                  for (int i = 0; i < channels.Length; i++)
                  {
                     Image<Gray, Byte> imgL = channels[i].ToImage();
                     Image<Gray, Byte> imgR = img1[i];
                     Assert.IsTrue(imgL.Equals(imgR), "failed split GpuMat");
                  }

                  using (GpuImage<Bgr, Byte> gpuImg2 = new GpuImage<Bgr, byte>(channels[0].Size))
                  {
                     gpuImg2.MergeFrom(channels, null);
                     Assert.IsTrue(gpuImg2.ToImage().Equals(img1), "failed split and merge test");
                  }

                  for (int i = 0; i < channels.Length; i++)
                  {
                     channels[i].Dispose();
                  }
               }
            }
         }
      }


      [Test]
      public void TestConvolutionAndLaplace()
      {
         if (GpuInvoke.HasCuda)
         {
            Image<Gray, Byte> image = new Image<Gray, byte>(300, 400);
            image.SetRandUniform(new MCvScalar(0.0), new MCvScalar(255.0));

            float[,] k = { {0, 1, 0},
                        {1, -4, 1},
                        {0, 1, 0}};
            using (ConvolutionKernelF kernel = new ConvolutionKernelF(k))
            using (Stream s = new Stream())
            using (GpuImage<Gray, Byte> gpuImg1 = new GpuImage<Gray, byte>(image))
            using (GpuImage<Gray, Single> gpuLaplace = new GpuImage<Gray, Single>(image.Size))
            using (GpuImage<Gray, Single> gpuConv = gpuImg1.Convolution(kernel, s))
            {
               GpuInvoke.Laplacian(gpuImg1, gpuLaplace, 1, 1.0, s);
               s.WaitForCompletion();
               Assert.IsTrue(gpuLaplace.Equals(gpuConv));
            }
           
         }
      }

      [Test]
      public void TestResizeGray()
      {
         if (GpuInvoke.HasCuda)
         {
            Image<Gray, Byte> img = new Image<Gray, byte>(300, 400);
            img.SetRandUniform(new MCvScalar(0.0), new MCvScalar(255.0));

            //Image<Gray, Byte> img = new Image<Gray, byte>("airplane.jpg");

            Image<Gray, Byte> small = img.Resize(100, 200, Emgu.CV.CvEnum.INTER.CV_INTER_LINEAR);
            GpuImage<Gray, Byte> gpuImg = new GpuImage<Gray, byte>(img);
            GpuImage<Gray, byte> smallGpuImg = new GpuImage<Gray, byte>(small.Size);
            GpuInvoke.Resize(gpuImg, smallGpuImg, Emgu.CV.CvEnum.INTER.CV_INTER_LINEAR, IntPtr.Zero);
            Image<Gray, Byte> diff = smallGpuImg.ToImage().AbsDiff(small);
            //ImageViewer.Show(smallGpuImg.ToImage());
            //ImageViewer.Show(small);
            //Assert.IsTrue(smallGpuImg.ToImage().Equals(small));
         }
      }

      [Test]
      public void TestClone()
      {
         if (GpuInvoke.HasCuda)
         {
            Image<Gray, Byte> img = new Image<Gray, byte>(300, 400);
            img.SetRandUniform(new MCvScalar(0.0), new MCvScalar(255.0));

            using (GpuImage<Gray, Byte> gImg1 = new GpuImage<Gray, byte>(img))
            using (GpuImage<Gray, Byte> gImg2 = gImg1.Clone())
            using (Image<Gray, Byte> img2 = gImg2.ToImage())
            {
               Assert.IsTrue(img.Equals(img2));
            }
         }
      }

      [Test]
      public void TestColorConvert()
      {
         if (GpuInvoke.HasCuda)
         {
            Image<Bgr, Byte> img = new Image<Bgr, byte>(300, 400);
            img.SetRandUniform(new MCvScalar(0.0, 0.0, 0.0), new MCvScalar(255.0, 255.0, 255.0));
            Image<Gray, Byte> imgGray = img.Convert<Gray, Byte>();
            Image<Hsv, Byte> imgHsv = img.Convert<Hsv, Byte>();

            GpuImage<Bgr, Byte> gpuImg = new GpuImage<Bgr, Byte>(img);
            GpuImage<Gray, Byte> gpuImgGray = gpuImg.Convert<Gray, Byte>();
            GpuImage<Hsv, Byte> gpuImgHsv = gpuImg.Convert<Hsv, Byte>();

            Assert.IsTrue(gpuImgGray.Equals(new GpuImage<Gray, Byte>(imgGray)));
            Assert.IsTrue(gpuImgHsv.ToImage().Equals(imgHsv));
            Assert.IsTrue(gpuImgHsv.Equals(new GpuImage<Hsv, Byte>(imgHsv)));
         }
      }

      [Test]
      public void TestInplaceNot()
      {
         if (GpuInvoke.HasCuda)
         {
            Image<Bgr, Byte> img = new Image<Bgr, byte>(300, 400);
            GpuImage<Bgr, Byte> gpuMat = new GpuImage<Bgr, byte>(img);
            GpuInvoke.BitwiseNot(gpuMat, gpuMat, IntPtr.Zero, IntPtr.Zero);
            Assert.IsTrue(gpuMat.Equals(new GpuImage<Bgr, Byte>(img.Not())));
         }
      }


      [Test]
      public void TestResizeBgr()
      {
         if (GpuInvoke.HasCuda)
         {
            Image<Bgr, Byte> img = new Image<Bgr, byte>("pedestrian.png");
            //img.SetRandUniform(new MCvScalar(0.0, 0.0, 0.0), new MCvScalar(255.0, 255.0, 255.0));

            Size size = new Size(100, 200);

            GpuImage<Bgr, Byte> gpuImg = new GpuImage<Bgr, byte>(img);
            GpuImage<Bgr, byte> smallGpuImg = new GpuImage<Bgr, byte>(size);

            GpuInvoke.Resize(gpuImg, smallGpuImg, Emgu.CV.CvEnum.INTER.CV_INTER_LINEAR, IntPtr.Zero);
            Image<Bgr, Byte> smallCpuImg = img.Resize(size.Width, size.Height, Emgu.CV.CvEnum.INTER.CV_INTER_LINEAR);


            Image<Bgr, Byte> diff = smallGpuImg.ToImage().AbsDiff(smallCpuImg);
            //TODO: Check why they are not an excat match
            //Assert.IsTrue(diff.CountNonzero()[0] == 0);
            //ImageViewer.Show(smallGpuImg.ToImage());
            //ImageViewer.Show(small);
         }
      }

      [Test]
      public void TestCanny()
      {
         if (GpuInvoke.HasCuda)
         {
            using (Image<Bgr, Byte> image = new Image<Bgr, byte>("pedestrian.png"))
            using (GpuImage<Bgr, Byte> gpuImage = new GpuImage<Bgr, byte>(image))
            using (GpuImage<Gray, Byte> gray = gpuImage.Convert<Gray, Byte>())
            using (GpuImage<Gray, Byte> canny = new GpuImage<Gray,byte>(gray.Size))
            {
               GpuInvoke.Canny(gray, canny, 20, 100, 3, false);
               //ImageViewer.Show(canny);
            }
         }
      }

      [Test]
      public void TestHOG1()
      {
         if (GpuInvoke.HasCuda)
         {
            using (GpuHOGDescriptor hog = new GpuHOGDescriptor())
            using (Image<Bgr, Byte> image = new Image<Bgr, byte>("pedestrian.png"))
            {
               float[] pedestrianDescriptor = GpuHOGDescriptor.GetDefaultPeopleDetector();
               hog.SetSVMDetector(pedestrianDescriptor);

               Stopwatch watch = Stopwatch.StartNew();
               Rectangle[] rects;
               using (GpuImage<Bgr, Byte> gpuImage = new GpuImage<Bgr, byte>(image))
               using (GpuImage<Bgra, Byte> gpuBgra = gpuImage.Convert<Bgra, Byte>())
                  rects = hog.DetectMultiScale(gpuBgra);
               watch.Stop();

               Assert.AreEqual(1, rects.Length);

               foreach (Rectangle rect in rects)
                  image.Draw(rect, new Bgr(Color.Red), 1);
               Trace.WriteLine(String.Format("HOG detection time: {0} ms", watch.ElapsedMilliseconds));

               //ImageViewer.Show(image, String.Format("Detection Time: {0}ms", watch.ElapsedMilliseconds));
            }
         }
      }

      [Test]
      public void TestHOG2()
      {
         if (GpuInvoke.HasCuda)
         {
            using (GpuHOGDescriptor hog = new GpuHOGDescriptor(
               new Size (48, 96), //winSize
               new Size(16, 16), //blockSize
               new Size(8,8), //blockStride
               new Size(8, 8), //cellSize
               9, //nbins
               -1, //winSigma
               0.2, //L2HysThreshold
               true, //gammaCorrection
               64 //nLevels
               ))
            using (Image<Bgr, Byte> image = new Image<Bgr, byte>("pedestrian.png"))
            {
               float[] pedestrianDescriptor = GpuHOGDescriptor.GetPeopleDetector48x96();
               hog.SetSVMDetector(pedestrianDescriptor);

               Stopwatch watch = Stopwatch.StartNew();
               Rectangle[] rects;
               using (GpuImage<Bgr, Byte> gpuImage = new GpuImage<Bgr, byte>(image))
               using (GpuImage<Bgra, Byte> gpuBgra = gpuImage.Convert<Bgra, Byte>())
                  rects = hog.DetectMultiScale(gpuBgra);
               watch.Stop();

               //Assert.AreEqual(1, rects.Length);

               foreach (Rectangle rect in rects)
                  image.Draw(rect, new Bgr(Color.Red), 1);
               Trace.WriteLine(String.Format("HOG detection time: {0} ms", watch.ElapsedMilliseconds));

               //ImageViewer.Show(image, String.Format("Detection Time: {0}ms", watch.ElapsedMilliseconds));
            }
         }
      }

      [Test]
      public void TestErodeDilate()
      {
         Image<Gray, Byte> image = new Image<Gray, byte>(640, 320);
         image.Draw(new CircleF(new PointF(200, 200), 30), new Gray(255.0), 2);

         using (GpuImage<Gray, Byte> gpuImage = new GpuImage<Gray, byte>(image))
         using (GpuImage<Gray, Byte> gpuTemp = new GpuImage<Gray,byte>(gpuImage.Size))
         using (Stream stream = new Stream())
         {
            GpuInvoke.Erode(gpuImage, gpuTemp, IntPtr.Zero, new Point(-1, -1), 1, stream);
            GpuInvoke.Dilate(gpuTemp, gpuImage, IntPtr.Zero, new Point(-1, -1), 1, stream);

            using (Image<Gray, Byte> temp = new Image<Gray, byte>(image.Size))
            {
               CvInvoke.cvErode(image, temp, IntPtr.Zero, 1);
               CvInvoke.cvDilate(temp, image, IntPtr.Zero, 1);
            }
            stream.WaitForCompletion();

            Assert.IsTrue(gpuImage.ToImage().Equals(image));
         }
         
      }

      [Test]
      public void TestGPU_SURFKeypointDetection()
      {
         if (GpuInvoke.HasCuda)
         {
            Image<Gray, byte> image = new Image<Gray, byte>(200, 100);
            image.SetRandUniform(new MCvScalar(), new MCvScalar(255));
            GpuImage<Gray, Byte> gpuMat = new GpuImage<Gray, byte>(image);

            Assert.IsTrue(gpuMat.ToImage().Equals(image));

            GpuSURFDetector gpuSurf = new GpuSURFDetector(100.0f, 2, 4, false, 0.01f, false);
            GpuMat<float> gpuKpts = gpuSurf.DetectKeyPointsRaw(gpuMat, null);
            VectorOfKeyPoint kpts = new VectorOfKeyPoint();
            gpuSurf.DownloadKeypoints(gpuKpts, kpts);
         }
      }

      [Test]
      public void TestBruteForceHammingDistance()
      {
         if (GpuInvoke.HasCuda)
         {
            Image<Gray, byte> box = new Image<Gray, byte>("box.png");
            FastDetector fast = new FastDetector(100, true);
            BriefDescriptorExtractor brief = new BriefDescriptorExtractor(32);

            #region extract features from the object image
            Stopwatch stopwatch = Stopwatch.StartNew();
            VectorOfKeyPoint modelKeypoints = fast.DetectKeyPointsRaw(box, null);
            Matrix<Byte> modelDescriptors = brief.ComputeDescriptorsRaw(box, null, modelKeypoints);
            stopwatch.Stop();
            Trace.WriteLine(String.Format("Time to extract feature from model: {0} milli-sec", stopwatch.ElapsedMilliseconds));
            #endregion

            Image<Gray, Byte> observedImage = new Image<Gray, byte>("box_in_scene.png");

            #region extract features from the observed image
            stopwatch.Reset(); stopwatch.Start();
            VectorOfKeyPoint observedKeypoints = fast.DetectKeyPointsRaw(observedImage, null);
            Matrix<Byte> observedDescriptors = brief.ComputeDescriptorsRaw(observedImage, null, observedKeypoints);
            stopwatch.Stop();
            Trace.WriteLine(String.Format("Time to extract feature from image: {0} milli-sec", stopwatch.ElapsedMilliseconds));
            #endregion

            HomographyMatrix homography = null;
            using (GpuMat<Byte> gpuModelDescriptors = new GpuMat<byte>(modelDescriptors)) //initialization of GPU code might took longer time.
            {
               stopwatch.Reset(); stopwatch.Start();
               GpuBruteForceMatcher<byte> hammingMatcher = new GpuBruteForceMatcher<Byte>(DistanceType.Hamming);

               //BruteForceMatcher hammingMatcher = new BruteForceMatcher(BruteForceMatcher.DistanceType.Hamming, modelDescriptors);
               int k = 2;
               Matrix<int> trainIdx = new Matrix<int>(observedKeypoints.Size, k);
               Matrix<float> distance = new Matrix<float>(trainIdx.Size);

               using (GpuMat<Byte> gpuObservedDescriptors = new GpuMat<byte>(observedDescriptors))
               using (GpuMat<int> gpuTrainIdx = new GpuMat<int>(trainIdx))
               using (GpuMat<float> gpuDistance = new GpuMat<float>(distance))
               {
                  Stopwatch w2 = Stopwatch.StartNew();
                  hammingMatcher.KnnMatch(gpuObservedDescriptors, gpuModelDescriptors, gpuTrainIdx, gpuDistance, k, null, null);
                  w2.Stop();
                  Trace.WriteLine(String.Format("Time for feature matching (excluding data transfer): {0} milli-sec", w2.ElapsedMilliseconds));
                  gpuTrainIdx.Download(trainIdx);
                  gpuDistance.Download(distance);
               }

               Matrix<Byte> mask = new Matrix<byte>(distance.Rows, 1);
               mask.SetValue(255);
               Features2DToolbox.VoteForUniqueness(distance, 0.8, mask);

               int nonZeroCount = CvInvoke.cvCountNonZero(mask);
               if (nonZeroCount >= 4)
               {
                  nonZeroCount = Features2DToolbox.VoteForSizeAndOrientation(modelKeypoints, observedKeypoints, trainIdx, mask, 1.5, 20);
                  if (nonZeroCount >= 4)
                     homography = Features2DToolbox.GetHomographyMatrixFromMatchedFeatures(modelKeypoints, observedKeypoints, trainIdx, mask, 2);
                  nonZeroCount = CvInvoke.cvCountNonZero(mask);
               }

               stopwatch.Stop();
               Trace.WriteLine(String.Format("Time for feature matching (including data transfer): {0} milli-sec", stopwatch.ElapsedMilliseconds));
            }

            if (homography != null)
            {
               Rectangle rect = box.ROI;
               PointF[] pts = new PointF[] { 
               new PointF(rect.Left, rect.Bottom),
               new PointF(rect.Right, rect.Bottom),
               new PointF(rect.Right, rect.Top),
               new PointF(rect.Left, rect.Top)};

               PointF[] points = pts.Clone() as PointF[];
               homography.ProjectPoints(points);

               //Merge the object image and the observed image into one big image for display
               Image<Gray, Byte> res = box.ConcateVertical(observedImage);

               for (int i = 0; i < points.Length; i++)
                  points[i].Y += box.Height;
               res.DrawPolyline(Array.ConvertAll<PointF, Point>(points, Point.Round), true, new Gray(255.0), 5);
               //ImageViewer.Show(res);
            }
         }
      }
   }
}
