//----------------------------------------------------------------------------
//  Copyright (C) 2004-2013 by EMGU. All rights reserved.       
//----------------------------------------------------------------------------

﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Text;
using Emgu.CV;
using Emgu.CV.Cuda;
using Emgu.CV.Structure;
using Emgu.CV.UI;
using Emgu.CV.Util;
using Emgu.CV.Features2D;
using Emgu.CV.Nonfree;
using NUnit.Framework;

namespace Emgu.CV.Test
{
   [TestFixture]
   public class TestGpuMat
   {
      [Test]
      public void TestGetCudaEnabledDeviceCount()
      {
         if (CudaInvoke.HasCuda)
         {
            int deviceCount = CudaInvoke.GetCudaEnabledDeviceCount();
            Trace.WriteLine(String.Format("Device count: {0}", deviceCount));
            if (deviceCount > 0)
            {
               CudaDeviceInfo d0 = new CudaDeviceInfo(0);
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
      public void TestCudaImageAsyncOps()
      {
         if (CudaInvoke.HasCuda)
         {
            int counter = 0;
            Stopwatch watch = Stopwatch.StartNew();
            using (CudaImage<Bgr, byte> img1 = new CudaImage<Bgr, byte>(3000, 2000))
            using (CudaImage<Bgr, Byte> img2 = new CudaImage<Bgr, Byte>(img1.Size))
            using (CudaImage<Gray, Byte> img3 = new CudaImage<Gray, byte>(img2.Size))
            using (Stream stream = new Stream())
            using (GpuMat<float> mat1 = img1.Convert<float>(stream))
            {
               while (!stream.Completed)
               {
                  if (counter <= int.MaxValue) counter++;
               }
               Trace.WriteLine(String.Format("Counter has been incremented {0} times", counter));

               counter = 0;
               CudaInvoke.CvtColor(img2, img3, CvToolbox.GetColorCvtCode(typeof(Bgr), typeof(Gray)), stream);
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
         if (!CudaInvoke.HasCuda)
            return;
         GpuMat<Byte> mat = new GpuMat<byte>(1200, 640, 1, true);
         Assert.IsTrue(mat.IsContinuous);
      }

      [Test]
      public void TestGpuMatRange()
      {
         if (CudaInvoke.HasCuda)
         {
            Image<Gray, Byte> img1 = new Image<Gray, byte>(1200, 640);
            img1.SetRandUniform(new MCvScalar(0, 0, 0), new MCvScalar(255, 255, 255));
            using (CudaImage<Gray, Byte> gpuImg1 = new CudaImage<Gray, byte>(img1))
            using (GpuMat<byte> mat = new GpuMat<byte>(gpuImg1, new MCvSlice(0, 1), MCvSlice.WholeSeq))
            {
               Size s = mat.Size;
            }
         }
      }

      [Test]
      public void TestGpuMatAdd()
      {
         if (CudaInvoke.HasCuda)
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
            CudaImage<Gray, Byte> gpuImg1 = new CudaImage<Gray, byte>(img1);
            CudaImage<Gray, Byte> gpuImg2 = new CudaImage<Gray, byte>(img2);
            CudaImage<Gray, Byte> gpuImgSum = new CudaImage<Gray, byte>(gpuImg1.Size);
            Stopwatch watch2 = Stopwatch.StartNew();
            for (int i = 0; i < repeat; i++)
               CudaInvoke.Add(gpuImg1, gpuImg2, gpuImgSum, IntPtr.Zero, IntPtr.Zero);
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
         if (CudaInvoke.HasCuda)
         {
            using (Image<Bgr, Byte> img1 = new Image<Bgr, byte>(1200, 640))
            {
               img1.SetRandUniform(new MCvScalar(0, 0, 0), new MCvScalar(255, 255, 255));

               using (CudaImage<Bgr, Byte> gpuImg1 = new CudaImage<Bgr, byte>(img1))
               {
                  CudaImage<Gray, Byte>[] channels = gpuImg1.Split(null);

                  for (int i = 0; i < channels.Length; i++)
                  {
                     Image<Gray, Byte> imgL = channels[i].ToImage();
                     Image<Gray, Byte> imgR = img1[i];
                     Assert.IsTrue(imgL.Equals(imgR), "failed split GpuMat");
                  }

                  using (CudaImage<Bgr, Byte> gpuImg2 = new CudaImage<Bgr, byte>(channels[0].Size))
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
      public void TestCudaFlip()
      {
         if (CudaInvoke.HasCuda)
         {
            using (Image<Bgr, Byte> img1 = new Image<Bgr, byte>(1200, 640))
            {
               img1.SetRandUniform(new MCvScalar(0, 0, 0), new MCvScalar(255, 255, 255));
               using (Image<Bgr, Byte> img1Flip = img1.Flip(CvEnum.FLIP.HORIZONTAL | CvEnum.FLIP.VERTICAL))
               using (CudaImage<Bgr, Byte> cudaImage = new CudaImage<Bgr, byte>(img1))
               using (CudaImage<Bgr, Byte> cudaFlip = new CudaImage<Bgr,byte>(img1.Size))
               {
                  CudaInvoke.Flip(cudaImage, cudaFlip, CvEnum.FLIP.HORIZONTAL | CvEnum.FLIP.VERTICAL, null);
                  cudaFlip.Download(img1);
                  Assert.IsTrue(img1.Equals(img1Flip));
               }
            }
         }
      }

      [Test]
      public void TestConvolutionAndLaplace()
      {
         if (CudaInvoke.HasCuda)
         {
            Image<Gray, Byte> image = new Image<Gray, byte>(300, 400);
            image.SetRandUniform(new MCvScalar(0.0), new MCvScalar(255.0));

            float[,] k = { {0, 1, 0},
                        {1, -4, 1},
                        {0, 1, 0}};
            using (ConvolutionKernelF kernel = new ConvolutionKernelF(k))
            using (Stream s = new Stream())
            using (CudaImage<Gray, Byte> cudaImg = new CudaImage<Gray, byte>(image))
            using (CudaImage<Gray, Byte> cudaLaplace = new CudaImage<Gray, Byte>(image.Size))
            using (CudaImage<Gray, Byte> cudaConv = cudaImg.Convolution(kernel, s))
            using (CudaLaplacianFilter<Gray, Byte> laplacian = new CudaLaplacianFilter<Gray, byte>(1, 1.0, CvEnum.BORDER_TYPE.DEFAULT, new MCvScalar()))
            {
               laplacian.Apply(cudaImg, cudaLaplace, s);
               s.WaitForCompletion();
               Assert.IsTrue(cudaLaplace.Equals(cudaConv));
            }
         }
      }

      [Test]
      public void TestCudaFilters()
      {
         if (CudaInvoke.HasCuda)
         {
            Image<Gray, Byte> image = new Image<Gray, byte>(300, 400);
            image.SetRandUniform(new MCvScalar(0.0), new MCvScalar(255.0));
            using (CudaImage<Gray, Byte> cudaImg1 = new CudaImage<Gray, byte>(image))
            using (CudaImage<Gray, Byte> cudaImg2 = new CudaImage<Gray, Byte>(image.Size))
            using (CudaGaussianFilter<Gray, Byte> gaussian = new CudaGaussianFilter<Gray,byte>(new Size(5, 5), 0, 0, CvEnum.BORDER_TYPE.DEFAULT, CvEnum.BORDER_TYPE.DEFAULT))
            using (CudaSobelFilter<Gray, Byte> sobel = new CudaSobelFilter<Gray,byte>(1, 1, 3, 1.0, CvEnum.BORDER_TYPE.DEFAULT, CvEnum.BORDER_TYPE.DEFAULT))
            {
               gaussian.Apply(cudaImg1, cudaImg2, null);
               sobel.Apply(cudaImg1, cudaImg2, null);

            }
         }
      }

      [Test]
      public void TestResizeGray()
      {
         if (CudaInvoke.HasCuda)
         {
            Image<Gray, Byte> img = new Image<Gray, byte>(300, 400);
            img.SetRandUniform(new MCvScalar(0.0), new MCvScalar(255.0));

            //Image<Gray, Byte> img = new Image<Gray, byte>("airplane.jpg");

            Image<Gray, Byte> small = img.Resize(100, 200, Emgu.CV.CvEnum.INTER.CV_INTER_LINEAR);
            CudaImage<Gray, Byte> gpuImg = new CudaImage<Gray, byte>(img);
            CudaImage<Gray, byte> smallGpuImg = new CudaImage<Gray, byte>(small.Size);
            CudaInvoke.Resize(gpuImg, smallGpuImg, Emgu.CV.CvEnum.INTER.CV_INTER_LINEAR, IntPtr.Zero);
            Image<Gray, Byte> diff = smallGpuImg.ToImage().AbsDiff(small);
            //ImageViewer.Show(smallGpuImg.ToImage());
            //ImageViewer.Show(small);
            //Assert.IsTrue(smallGpuImg.ToImage().Equals(small));
         }
      }

      [Test]
      public void TestClone()
      {
         if (CudaInvoke.HasCuda)
         {
            Image<Gray, Byte> img = new Image<Gray, byte>(300, 400);
            img.SetRandUniform(new MCvScalar(0.0), new MCvScalar(255.0));

            using (CudaImage<Gray, Byte> gImg1 = new CudaImage<Gray, byte>(img))
            using (CudaImage<Gray, Byte> gImg2 = gImg1.Clone(null))
            using (Image<Gray, Byte> img2 = gImg2.ToImage())
            {
               Assert.IsTrue(img.Equals(img2));
            }
         }
      }

      [Test]
      public void TestColorConvert()
      {
         if (CudaInvoke.HasCuda)
         {
            Image<Bgr, Byte> img = new Image<Bgr, byte>(300, 400);
            img.SetRandUniform(new MCvScalar(0.0, 0.0, 0.0), new MCvScalar(255.0, 255.0, 255.0));
            Image<Gray, Byte> imgGray = img.Convert<Gray, Byte>();
            Image<Hsv, Byte> imgHsv = img.Convert<Hsv, Byte>();

            CudaImage<Bgr, Byte> gpuImg = new CudaImage<Bgr, Byte>(img);
            CudaImage<Gray, Byte> gpuImgGray = gpuImg.Convert<Gray, Byte>();
            CudaImage<Hsv, Byte> gpuImgHsv = gpuImg.Convert<Hsv, Byte>();

            Assert.IsTrue(gpuImgGray.Equals(new CudaImage<Gray, Byte>(imgGray)));
            Assert.IsTrue(gpuImgHsv.ToImage().Equals(imgHsv));
            Assert.IsTrue(gpuImgHsv.Equals(new CudaImage<Hsv, Byte>(imgHsv)));
         }
      }

      [Test]
      public void TestInplaceNot()
      {
         if (CudaInvoke.HasCuda)
         {
            Image<Bgr, Byte> img = new Image<Bgr, byte>(300, 400);
            CudaImage<Bgr, Byte> gpuMat = new CudaImage<Bgr, byte>(img);
            CudaInvoke.BitwiseNot(gpuMat, gpuMat, IntPtr.Zero, IntPtr.Zero);
            Assert.IsTrue(gpuMat.Equals(new CudaImage<Bgr, Byte>(img.Not())));
         }
      }


      [Test]
      public void TestResizeBgr()
      {
         if (CudaInvoke.HasCuda)
         {
            Image<Bgr, Byte> img = new Image<Bgr, byte>("pedestrian.png");
            //img.SetRandUniform(new MCvScalar(0.0, 0.0, 0.0), new MCvScalar(255.0, 255.0, 255.0));

            Size size = new Size(100, 200);

            CudaImage<Bgr, Byte> cudaImg = new CudaImage<Bgr, byte>(img);
            CudaImage<Bgr, byte> smallCudaImg = new CudaImage<Bgr, byte>(size);

            CudaInvoke.Resize(cudaImg, smallCudaImg, Emgu.CV.CvEnum.INTER.CV_INTER_LINEAR, IntPtr.Zero);
            Image<Bgr, Byte> smallCpuImg = img.Resize(size.Width, size.Height, Emgu.CV.CvEnum.INTER.CV_INTER_LINEAR);


            Image<Bgr, Byte> diff = smallCudaImg.ToImage().AbsDiff(smallCpuImg);
            //TODO: Check why they are not an excat match
            //Assert.IsTrue(diff.CountNonzero()[0] == 0);
            //ImageViewer.Show(smallGpuImg.ToImage());
            //ImageViewer.Show(small);
         }
      }

      
      [Test]
      public void TestCanny()
      {
         if (CudaInvoke.HasCuda)
         {
            using (Image<Bgr, Byte> image = new Image<Bgr, byte>("pedestrian.png"))
            using (CudaImage<Bgr, Byte> CudaImage = new CudaImage<Bgr, byte>(image))
            using (CudaImage<Gray, Byte> gray = CudaImage.Convert<Gray, Byte>())
            using (CudaImage<Gray, Byte> canny = new CudaImage<Gray,byte>(gray.Size))
            using (CudaCannyEdgeDetector detector = new CudaCannyEdgeDetector(20, 100, 3, false))
            {
               detector.Detect(gray, canny);
               //GpuInvoke.Canny(gray, canny, 20, 100, 3, false);
               //ImageViewer.Show(canny);
            }
         }
      }


      [Test]
      public void TestClahe()
      {
         if (CudaInvoke.HasCuda)
         {
            Image<Gray, Byte> image = EmguAssert.LoadImage<Gray, Byte>("pedestrian.png");
            CudaImage<Gray, Byte> cudaImage = new CudaImage<Gray, byte>(image);
            CudaImage<Gray, Byte> cudaResult = new CudaImage<Gray, byte>(cudaImage.Size);
            Image<Gray, Byte> result = new Image<Gray, byte>(cudaResult.Size);
            CudaInvoke.CLAHE(cudaImage, cudaResult, 4, new Size(8, 8), null);
            cudaResult.Download(result);
            Emgu.CV.UI.ImageViewer.Show(image.ConcateHorizontal(result));
         }
      }

      [Test]
      public void TestHOG1()
      {
         if (CudaInvoke.HasCuda)
         {
            using (CudaHOGDescriptor hog = new CudaHOGDescriptor())
            using (Image<Bgr, Byte> image = new Image<Bgr, byte>("pedestrian.png"))
            {
               float[] pedestrianDescriptor = CudaHOGDescriptor.GetDefaultPeopleDetector();
               hog.SetSVMDetector(pedestrianDescriptor);

               Stopwatch watch = Stopwatch.StartNew();
               Rectangle[] rects;
               using (CudaImage<Bgr, Byte> CudaImage = new CudaImage<Bgr, byte>(image))
               using (CudaImage<Bgra, Byte> gpuBgra = CudaImage.Convert<Bgra, Byte>())
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
         if (CudaInvoke.HasCuda)
         {
            using (CudaHOGDescriptor hog = new CudaHOGDescriptor(
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
               float[] pedestrianDescriptor = CudaHOGDescriptor.GetPeopleDetector48x96();
               hog.SetSVMDetector(pedestrianDescriptor);

               Stopwatch watch = Stopwatch.StartNew();
               Rectangle[] rects;
               using (CudaImage<Bgr, Byte> CudaImage = new CudaImage<Bgr, byte>(image))
               using (CudaImage<Bgra, Byte> gpuBgra = CudaImage.Convert<Bgra, Byte>())
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
      public void TestCudaReduce()
      {
         if (!CudaInvoke.HasCuda)
            return;
         using (Image<Bgr, Byte> img = new Image<Bgr, byte>(480, 320))
         {
            img.SetRandUniform(new MCvScalar(0, 0, 0), new MCvScalar(255, 255, 255));
            using (CudaImage<Bgr, byte> CudaImage = new CudaImage<Bgr, byte>(img))
            using (GpuMat<byte> reduced = new GpuMat<byte>(1, CudaImage.Size.Width, CudaImage.NumberOfChannels, true))
            {
               CudaInvoke.Reduce(CudaImage, reduced, CvEnum.REDUCE_DIMENSION.SINGLE_ROW, CvEnum.REDUCE_TYPE.CV_REDUCE_AVG, IntPtr.Zero);
            }
         }
      }

      [Test]
      public void TestErodeDilate()
      {
         if (!CudaInvoke.HasCuda)
            return;
         
         int morphIter = 2;
         Image<Gray, Byte> image = new Image<Gray, byte>(640, 320);
         image.Draw(new CircleF(new PointF(200, 200), 30), new Gray(255.0), 4);

         Size ksize = new Size(morphIter * 2 + 1, morphIter * 2 + 1);

         using (CudaImage<Gray, Byte> cudaImage = new CudaImage<Gray, byte>(image))
         using (CudaImage<Gray, Byte> cudaTemp = new CudaImage<Gray,byte>(cudaImage.Size))
         using (Stream stream = new Stream())
         using (CudaBoxMaxFilter<Gray> dilate = new CudaBoxMaxFilter<Gray>(ksize, new Point(-1, -1), CvEnum.BORDER_TYPE.DEFAULT, new MCvScalar()))
         using (CudaBoxMinFilter<Gray> erode = new CudaBoxMinFilter<Gray>(ksize, new Point(-1, -1), CvEnum.BORDER_TYPE.DEFAULT, new MCvScalar()))
         {
            //run the GPU version asyncrhonously with stream
            erode.Apply(cudaImage, cudaTemp, stream);
            dilate.Apply(cudaTemp, cudaImage, stream);

            //run the CPU version in parallel to the gpu version.
            using (Image<Gray, Byte> temp = new Image<Gray, byte>(image.Size))
            {
               CvInvoke.cvErode(image, temp, IntPtr.Zero, morphIter);
               CvInvoke.cvDilate(temp, image, IntPtr.Zero, morphIter);
            }

            //syncrhonize with the GPU version
            stream.WaitForCompletion();

            Assert.IsTrue(cudaImage.ToImage().Equals(image));
         }
         
      }

      [Test]
      public void TestCudaSURFKeypointDetection()
      {
         if (CudaInvoke.HasCuda)
         {
            Image<Gray, byte> image = new Image<Gray, byte>(200, 100);
            image.SetRandUniform(new MCvScalar(), new MCvScalar(255));
            CudaImage<Gray, Byte> gpuMat = new CudaImage<Gray, byte>(image);

            Assert.IsTrue(gpuMat.ToImage().Equals(image));

            CudaSURFDetector cudaSurf = new CudaSURFDetector(100.0f, 2, 4, false, 0.01f, false);
            GpuMat<float> cudaKpts = cudaSurf.DetectKeyPointsRaw(gpuMat, null);
            VectorOfKeyPoint kpts = new VectorOfKeyPoint();
            cudaSurf.DownloadKeypoints(cudaKpts, kpts);
         }
      }

      [Test]
      public void TestCudaFASTDetector()
      {
         if (!CudaInvoke.HasCuda)
            return;
         using (Image<Bgr, Byte> img = new Image<Bgr, byte>("box.png"))
         using (CudaImage<Bgr, Byte> CudaImage = new CudaImage<Bgr, byte>(img))
         using (CudaImage<Gray, Byte> grayCudaImage = CudaImage.Convert<Gray, Byte>())
         using (CudaFASTDetector detector = new CudaFASTDetector(10, true, 0.05))
         using (VectorOfKeyPoint kpts = new VectorOfKeyPoint())
         {
            GpuMat<float> keyPointsMat = detector.DetectKeyPointsRaw(grayCudaImage, null);
                        detector.DownloadKeypoints(keyPointsMat, kpts);

            foreach (MKeyPoint kpt in kpts.ToArray())
            {
               img.Draw(new CircleF(kpt.Point, 3.0f), new Bgr(0, 255, 0), 1);
            }

            //ImageViewer.Show(img);
         }
      }

      [Test]
      public void TestCudaOrbDetector()
      {
         if (!CudaInvoke.HasCuda)
            return;
         using(Image<Bgr, Byte> img = new Image<Bgr, byte>("box.png"))
         using (CudaImage<Bgr, Byte> CudaImage = new CudaImage<Bgr,byte>(img))
         using (CudaImage<Gray, Byte> grayCudaImage = CudaImage.Convert<Gray, Byte>()) 
         using (CudaORBDetector detector = new CudaORBDetector(500))
         using (VectorOfKeyPoint kpts = new VectorOfKeyPoint())
         {
            GpuMat<float> keyPointsMat;
            GpuMat<Byte> descriptorsMat;
            detector.ComputeRaw(grayCudaImage, null, out keyPointsMat, out descriptorsMat);
            detector.DownloadKeypoints(keyPointsMat, kpts);

            foreach (MKeyPoint kpt in kpts.ToArray())
            {
               img.Draw(new CircleF(kpt.Point, 3.0f), new Bgr(0, 255, 0), 1);
            }

            //ImageViewer.Show(img);
         }
      }

      [Test]
      public void TestCudaPyr()
      {
         if (!CudaInvoke.HasCuda)
            return;
         Image<Gray, Byte> img = new Image<Gray, byte>(640, 480);
         img.SetRandUniform(new MCvScalar(), new MCvScalar(255, 255, 255));
         Image<Gray, Byte> down = img.PyrDown();
         Image<Gray, Byte> up = down.PyrUp();

         CudaImage<Gray, Byte> gImg = new CudaImage<Gray, byte>(img);
         CudaImage<Gray, Byte> gDown = new CudaImage<Gray, byte>(gImg.Size.Width >> 1, gImg.Size.Height >> 1);
         CudaImage<Gray, Byte> gUp = new CudaImage<Gray, byte>(img.Size);
         CudaInvoke.PyrDown(gImg, gDown, IntPtr.Zero);
         CudaInvoke.PyrUp(gDown, gUp, IntPtr.Zero);

         CvInvoke.cvAbsDiff(down, gDown.ToImage(), down);
         CvInvoke.cvAbsDiff(up, gUp.ToImage(), up);
         double[] minVals, maxVals;
         Point[] minLocs, maxLocs;
         down.MinMax(out minVals, out maxVals, out minLocs, out maxLocs);
         double maxVal = 0.0;
         for (int i = 0; i < maxVals.Length; i++)
         {
            if (maxVals[i] > maxVal)
               maxVal = maxVals[i];
         }
         Trace.WriteLine(String.Format("Max diff: {0}", maxVal));
         Assert.LessOrEqual(maxVal, 1.0);

         up.MinMax(out minVals, out maxVals, out minLocs, out maxLocs);
         maxVal = 0.0;
         for (int i = 0; i < maxVals.Length; i++)
         {
            if (maxVals[i] > maxVal)
               maxVal = maxVals[i];
         }
         Trace.WriteLine(String.Format("Max diff: {0}", maxVal));
         Assert.LessOrEqual(maxVal, 1.0);
      }

      
      [Test]
      public void TestMatchTemplate()
      {
         if (!CudaInvoke.HasCuda)
            return;

         #region prepare synthetic image for testing
         int templWidth = 50;
         int templHeight = 50;
         Point templCenter = new Point(120, 100);

         //Create a random object
         Image<Bgr, Byte> randomObj = new Image<Bgr, byte>(templWidth, templHeight);
         randomObj.SetRandUniform(new MCvScalar(), new MCvScalar(255, 255, 255));

         //Draw the object in image1 center at templCenter;
         Image<Bgr, Byte> img = new Image<Bgr, byte>(300, 200);
         Rectangle objectLocation = new Rectangle(templCenter.X - (templWidth >> 1), templCenter.Y - (templHeight >> 1), templWidth, templHeight);
         img.ROI = objectLocation;
         randomObj.Copy(img, null);
         img.ROI = Rectangle.Empty;
         #endregion

         Image<Gray, Single> match = img.MatchTemplate(randomObj, Emgu.CV.CvEnum.TM_TYPE.CV_TM_SQDIFF);
         double[] minVal, maxVal;
         Point[] minLoc, maxLoc;
         match.MinMax(out minVal, out maxVal, out minLoc, out maxLoc);

         double gpuMinVal = 0, gpuMaxVal = 0;
         Point gpuMinLoc = Point.Empty, gpuMaxLoc = Point.Empty;
         CudaImage<Bgr, Byte> CudaImage = new CudaImage<Bgr, byte>(img);
         CudaImage<Bgr, Byte> gpuRandomObj = new CudaImage<Bgr, byte>(randomObj);
         CudaImage<Gray, Single> gpuMatch = new CudaImage<Gray, float>(match.Size);
         using (CudaTemplateMatching<Bgr, Byte> buffer = new CudaTemplateMatching<Bgr, Byte>(CvEnum.TM_TYPE.CV_TM_SQDIFF, new Size()))
         using (Stream stream = new Stream())
         {
            buffer.Match(CudaImage, gpuRandomObj, gpuMatch, stream);
            //GpuInvoke.MatchTemplate(CudaImage, gpuRandomObj, gpuMatch, CvEnum.TM_TYPE.CV_TM_SQDIFF, buffer, stream);
            stream.WaitForCompletion();
            CudaInvoke.MinMaxLoc(gpuMatch, ref gpuMinVal, ref gpuMaxVal, ref gpuMinLoc, ref gpuMaxLoc, IntPtr.Zero);
         }

         EmguAssert.AreEqual(minLoc[0].X, templCenter.X - templWidth / 2);
         EmguAssert.AreEqual(minLoc[0].Y, templCenter.Y - templHeight / 2);
         EmguAssert.IsTrue(minLoc[0].Equals(gpuMinLoc));
         EmguAssert.IsTrue(maxLoc[0].Equals(gpuMaxLoc));
         
      }

      [Test]
      public void TestCudaRemap()
      {
         if (!CudaInvoke.HasCuda)
            return;
         Image<Gray, float> xmap = new Image<Gray, float>(2, 2);
         xmap.Data[0, 0, 0] = 0; xmap.Data[0, 1, 0] = 0;
         xmap.Data[1, 0, 0] = 1; xmap.Data[1, 1, 0] = 1;
         Image<Gray, float> ymap = new Image<Gray, float>(2, 2);
         ymap.Data[0, 0, 0] = 0; ymap.Data[0, 1, 0] = 1;
         ymap.Data[1, 0, 0] = 0; ymap.Data[1, 1, 0] = 1;

         Image<Gray, Byte> image = new Image<Gray, byte>(2, 2);
         image.SetRandNormal(new MCvScalar(), new MCvScalar(255));

         using (CudaImage<Gray, Byte> CudaImage = new CudaImage<Gray, byte>(image))
         using (CudaImage<Gray, float> xCudaImage = new CudaImage<Gray, float>(xmap))
         using (CudaImage<Gray, float> yCudaImage = new CudaImage<Gray, float>(ymap))
         using (CudaImage<Gray, Byte> remapedImage = new CudaImage<Gray,byte>(CudaImage.Size))
         {
            CudaInvoke.Remap(CudaImage, remapedImage, xCudaImage, yCudaImage, CvEnum.INTER.CV_INTER_CUBIC, CvEnum.BORDER_TYPE.DEFAULT, new MCvScalar(), IntPtr.Zero);
         }
      }

      [Test]
      public void TestCudaWarpPerspective()
      {
         if (!CudaInvoke.HasCuda)
            return;
         Matrix<float> transformation = new Matrix<float>(3, 3);
         transformation.SetIdentity();

         Image<Gray, byte> image = new Image<Gray, byte>(480, 320);
         image.SetRandNormal(new MCvScalar(), new MCvScalar(255));

         using (CudaImage<Gray, byte> CudaImage = new CudaImage<Gray,byte>(image))
         using (CudaImage<Gray, Byte> resultCudaImage = new CudaImage<Gray, byte>(CudaImage.Size))
         {
            CudaInvoke.WarpPerspective(CudaImage, resultCudaImage, transformation, CvEnum.INTER.CV_INTER_CUBIC, CvEnum.BORDER_TYPE.DEFAULT, new MCvScalar(), IntPtr.Zero);
         }
      }

      [Test]
      public void TestCudaPyrLKOpticalFlow()
      {
         Image<Gray, Byte> prevImg, currImg;
         AutoTestVarious.OpticalFlowImage(out prevImg, out currImg);
         Image<Gray, Single> flowx = new Image<Gray, float>(prevImg.Size);
         Image<Gray, Single> flowy = new Image<Gray, float>(prevImg.Size);
         CudaPyrLKOpticalFlow flow = new CudaPyrLKOpticalFlow(new Size(21, 21), 3, 30, false);
         using(CudaImage<Gray, Byte> prevGpu = new CudaImage<Gray,byte>(prevImg))
         using (CudaImage<Gray, byte> currGpu = new CudaImage<Gray, byte>(currImg))
         using (CudaImage<Gray, float> flowxGpu = new CudaImage<Gray,float>(prevGpu.Size))
         using (CudaImage<Gray, float> flowyGpu = new CudaImage<Gray,float>(prevGpu.Size))
         {
            flow.Dense(prevGpu, currGpu, flowxGpu, flowyGpu);
            flowxGpu.Download(flowx);
            flowyGpu.Download(flowy);
         }  
      }

      [Test]
      public void TestBilaterialFilter()
      {
         
         if (CudaInvoke.HasCuda)
         {
            Image<Bgr, Byte> img = new Image<Bgr, byte>("pedestrian.png");
            Image<Gray, byte> gray = img.Convert<Gray, Byte>();
            CudaImage<Gray, Byte> CudaImage = new CudaImage<Gray, byte>(gray);
            
            CudaImage<Gray, Byte> gpuBilaterial = new CudaImage<Gray, byte>(CudaImage.Size);
            CudaInvoke.BilateralFilter(CudaImage, gpuBilaterial, 7, 5, 5, CvEnum.BORDER_TYPE.DEFAULT, IntPtr.Zero);

            //Emgu.CV.UI.ImageViewer.Show(gray.ConcateHorizontal(gpuBilaterial.ToImage()));
         }
      }

      [Test]
      public void TestBruteForceHammingDistance()
      {
         if (CudaInvoke.HasCuda)
         {
            Image<Gray, byte> box = new Image<Gray, byte>("box.png");
            FastDetector fast = new FastDetector(100, true);
            BriefDescriptorExtractor brief = new BriefDescriptorExtractor(32);

            #region extract features from the object image
            Stopwatch stopwatch = Stopwatch.StartNew();
            VectorOfKeyPoint modelKeypoints = fast.DetectRaw(box, null);
            Matrix<Byte> modelDescriptors = brief.Compute(box, null, modelKeypoints);
            stopwatch.Stop();
            Trace.WriteLine(String.Format("Time to extract feature from model: {0} milli-sec", stopwatch.ElapsedMilliseconds));
            #endregion

            Image<Gray, Byte> observedImage = new Image<Gray, byte>("box_in_scene.png");

            #region extract features from the observed image
            stopwatch.Reset(); stopwatch.Start();
            VectorOfKeyPoint observedKeypoints = fast.DetectRaw(observedImage, null);
            Matrix<Byte> observedDescriptors = brief.Compute(observedImage, null, observedKeypoints);
            stopwatch.Stop();
            Trace.WriteLine(String.Format("Time to extract feature from image: {0} milli-sec", stopwatch.ElapsedMilliseconds));
            #endregion

            HomographyMatrix homography = null;
            using (GpuMat<Byte> gpuModelDescriptors = new GpuMat<byte>(modelDescriptors)) //initialization of GPU code might took longer time.
            {
               stopwatch.Reset(); stopwatch.Start();
               CudaBruteForceMatcher<byte> hammingMatcher = new CudaBruteForceMatcher<Byte>(DistanceType.Hamming);

               //BruteForceMatcher hammingMatcher = new BruteForceMatcher(BruteForceMatcher.DistanceType.Hamming, modelDescriptors);
               int k = 2;
               Matrix<int> trainIdx = new Matrix<int>(observedKeypoints.Size, k);
               Matrix<float> distance = new Matrix<float>(trainIdx.Size);

               using (GpuMat<Byte> gpuObservedDescriptors = new GpuMat<byte>(observedDescriptors))
               using (GpuMat<int> gpuTrainIdx = new GpuMat<int>(trainIdx.Rows, trainIdx.Cols, 1, true))
               using (GpuMat<float> gpuDistance = new GpuMat<float>(distance.Rows, distance.Cols, 1, true))
               {
                  Stopwatch w2 = Stopwatch.StartNew();
                  hammingMatcher.KnnMatchSingle(gpuObservedDescriptors, gpuModelDescriptors, gpuTrainIdx, gpuDistance, k, null, null);
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
