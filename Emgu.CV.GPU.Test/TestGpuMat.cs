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
               GpuDevice d0 = new GpuDevice(0);
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
               GpuInvoke.Add(gpuImg1, gpuImg2, gpuImgSum);
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
            Image<Bgr, Byte> img1 = new Image<Bgr, byte>(1200, 640);
            img1.SetRandUniform(new MCvScalar(0, 0, 0), new MCvScalar(255, 255, 255));

            using (GpuImage<Bgr, Byte> gpuImg1 = new GpuImage<Bgr, byte>(img1))
            {
               GpuImage<Gray, Byte>[] channels = gpuImg1.Split(null);

               for (int i = 0; i < channels.Length; i++)
               {
                  Assert.IsTrue(channels[i].ToImage().Equals(img1[i]), "failed split GpuMat");
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


      [Test]
      public void TestConvolutionAndLaplace()
      {
         if (GpuInvoke.HasCuda)
         {
            Image<Gray, Byte> image = new Image<Gray, byte>(300, 400);
            image.SetRandUniform(new MCvScalar(0.0), new MCvScalar(255.0));

            GpuImage<Gray, Byte> gpuImg1 = new GpuImage<Gray, byte>(image);
            GpuImage<Gray, Single> gpuLaplace = new GpuImage<Gray, Single>(image.Size);
            GpuInvoke.Laplacian(gpuImg1, gpuLaplace, 1, 1.0);

            float[,] k = { {0, 1, 0},
                        {1, -4, 1},
                        {0, 1, 0}};
            ConvolutionKernelF kernel = new ConvolutionKernelF(k);
            GpuImage<Gray, Single> gpuConv = gpuImg1.Convolution(kernel);

            Assert.IsTrue(gpuLaplace.Equals(gpuConv));
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
            GpuInvoke.Resize(gpuImg, smallGpuImg, Emgu.CV.CvEnum.INTER.CV_INTER_LINEAR);
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

            using (GpuImage<Gray, Byte> gImg1 = new GpuImage<Gray,byte>(img))
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
            Image<Bgr, Byte> img = new Image<Bgr, byte>(300, 400);
            img.SetRandUniform(new MCvScalar(0.0, 0.0, 0.0), new MCvScalar(255.0, 255.0, 255.0));
            //Image<Bgr, Byte> img = new Image<Bgr, byte>("airplane.jpg");

            Image<Bgr, Byte> small = img.Resize(100, 200, Emgu.CV.CvEnum.INTER.CV_INTER_LINEAR);
            GpuImage<Bgr, Byte> gpuImg = new GpuImage<Bgr, byte>(img);
            GpuImage<Bgr, byte> smallGpuImg = new GpuImage<Bgr, byte>(small.Size);
            GpuInvoke.Resize(gpuImg, smallGpuImg, Emgu.CV.CvEnum.INTER.CV_INTER_LINEAR);
            Image<Bgr, Byte> diff = smallGpuImg.ToImage().AbsDiff(small);
            //ImageViewer.Show(smallGpuImg.ToImage());
            //ImageViewer.Show(small);
         }
      }

      /*
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
               GpuInvoke.Canny(gray, canny, 20, 100, 3);
               ImageViewer.Show(canny);
            }
         }
      }*/

      [Test]
      public void TestHOG()
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
   }
}
