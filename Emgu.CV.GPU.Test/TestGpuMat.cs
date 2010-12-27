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
      public int TestGetCudaEnabledDeviceCount()
      {
         return GpuInvoke.GetCudaEnabledDeviceCount();
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

      public void TestGpuMatAdd()
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
         Trace.WriteLine(String.Format( "CPU processing time: {0}ms", (double)watch.ElapsedMilliseconds / repeat));

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
         Trace.WriteLine(String.Format("Core GPU processing time: {0}ms", (double)watch2.ElapsedMilliseconds/repeat));
         //Trace.WriteLine(String.Format("Total GPU processing time: {0}ms", (double)watch.ElapsedMilliseconds/repeat));

         Assert.IsTrue(cpuImgSum.Equals( cpuImgSumFromGpu ));
      }
   }
}
