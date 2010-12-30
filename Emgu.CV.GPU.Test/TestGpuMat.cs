using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using Emgu.CV.GPU;
using Emgu.CV;
using Emgu.CV.Structure;
using System.Diagnostics;

namespace Emgu.CV.GPU.Test
{
   [TestFixture]
   public class TestGpuMat
   {
      [Test]
      public void TestGetCudaEnabledDeviceCount()
      {
         int deviceCount = GpuInvoke.GetCudaEnabledDeviceCount();
         Trace.WriteLine(String.Format("Device count: {0}", deviceCount));
         if (deviceCount > 0)
         {
            GpuDevice d0 = new GpuDevice(0);
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
               GpuInvoke.gpuMatAdd(gpuImg1, gpuImg2, gpuImgSum);
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
               GpuImage<Gray, Byte>[] channels = gpuImg1.Split();
               
               for (int i = 0; i < channels.Length; i++)
               {
                  Assert.IsTrue(channels[i].ToImage().Equals(img1[i]), "failed split GpuMat");
               }

               using (GpuImage<Bgr, Byte> gpuImg2 = new GpuImage<Bgr, byte>(channels[0].Size))
               {
                  gpuImg2.MergeFrom(channels);
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
            GpuInvoke.gpuMatLaplacian(gpuImg1, gpuLaplace, 1, 1.0);

            float[,] k = { {0, 1, 0},
                        {1, -4, 1},
                        {0, 1, 0}};
            ConvolutionKernelF kernel = new ConvolutionKernelF(k);
            GpuImage<Gray, Single> gpuConv = gpuImg1.Convolution(kernel);

            Assert.IsTrue(gpuLaplace.Equals(gpuConv));
         }
      }
   }
}
