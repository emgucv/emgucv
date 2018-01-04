//----------------------------------------------------------------------------
//  Copyright (C) 2004-2018 by EMGU Corporation. All rights reserved.       
//----------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Text;
using Emgu.CV.CvEnum;
using Emgu.CV.OCR;
using Emgu.CV.Structure;
using Emgu.CV.Util;
using Emgu.CV.Features2D;
using Emgu.CV.XFeatures2D;


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
   public class AutoTestOpenCL
   {
      [Test]
      public void TestOclInfo()
      {
         Trace.WriteLine(CvInvoke.OclGetPlatformsSummary());
      }

      [Test]
      public void TestOclKernel()
      {
         if (CvInvoke.HaveOpenCL && CvInvoke.UseOpenCL)
         {

            Ocl.Device defaultDevice = Ocl.Device.Default;

            Mat img = EmguAssert.LoadMat("lena.jpg");
            Mat imgGray = new Mat();
            CvInvoke.CvtColor(img, imgGray, ColorConversion.Bgr2Gray);
            Mat imgFloat = new Mat();
            imgGray.ConvertTo(imgFloat, DepthType.Cv32F, 1.0/255);
            UMat umat = imgFloat.GetUMat(AccessType.Read, UMat.Usage.AllocateDeviceMemory);
            UMat umatDst = new UMat();
            umatDst.Create(umat.Rows, umat.Cols, DepthType.Cv32F, umat.NumberOfChannels, UMat.Usage.AllocateDeviceMemory);
            
            String buildOpts = String.Format("-D dstT={0}", Ocl.OclInvoke.TypeToString(umat.Depth));
    
            String sourceStr = @"
__kernel void magnutude_filter_8u(
       __global const uchar* src, int src_step, int src_offset,
       __global uchar* dst, int dst_step, int dst_offset, int dst_rows, int dst_cols,
       float scale)
{
   int x = get_global_id(0);
   int y = get_global_id(1);
   if (x < dst_cols && y < dst_rows)
   {
       int dst_idx = y * dst_step + x + dst_offset;
       if (x > 0 && x < dst_cols - 1 && y > 0 && y < dst_rows - 2)
       {
           int src_idx = y * src_step + x + src_offset;
           int dx = (int)src[src_idx]*2 - src[src_idx - 1]          - src[src_idx + 1];
           int dy = (int)src[src_idx]*2 - src[src_idx - 1*src_step] - src[src_idx + 1*src_step];
           dst[dst_idx] = convert_uchar_sat(sqrt((float)(dx*dx + dy*dy)) * scale);
       }
       else
       {
           dst[dst_idx] = 0;
       }
   }
}";

            using (CvString errorMsg = new CvString())
            using (Ocl.ProgramSource ps = new Ocl.ProgramSource(sourceStr))
            using (Ocl.Kernel kernel = new Ocl.Kernel())
            using (Ocl.Image2D image2d = new Ocl.Image2D(umat))
            using (Ocl.KernelArg ka = new Ocl.KernelArg(Ocl.KernelArg.Flags.ReadWrite, umatDst))
            {
               float shiftX = 100.5f;
               float shiftY = -50.0f;

               bool success = kernel.Create("shift", ps, buildOpts, errorMsg);
               EmguAssert.IsTrue(success, errorMsg.ToString());
               int idx = 0;
               idx = kernel.Set(idx, image2d);
               idx = kernel.Set(idx, ref shiftX);
               idx = kernel.Set(idx, ref shiftY);
               idx = kernel.Set(idx, ka);
               IntPtr[] globalThreads = new IntPtr[] {new IntPtr(umat.Cols), new IntPtr(umat.Rows), new IntPtr(1) };
               success = kernel.Run(globalThreads, null, true);
               EmguAssert.IsTrue(success, "Failed to run the kernel");
               using (Mat matDst = umatDst.GetMat(AccessType.Read))
               using (Mat saveMat = new Mat())
               {
                  matDst.ConvertTo(saveMat, DepthType.Cv8U, 255.0);
                  saveMat.Save("tmp.jpg");
               }
            }
         }
      }

      [Test]
      public void TestOclChangeDefaultDevice()
      {
         if (CvInvoke.HaveOpenCL && CvInvoke.UseOpenCL)
         {
            using (VectorOfOclPlatformInfo oclPlatformInfos = Ocl.OclInvoke.GetPlatformsInfo())
            {
               if (oclPlatformInfos.Size > 0)
               {
                  for (int i = 0; i < oclPlatformInfos.Size; i++)
                  {
                     Ocl.PlatformInfo platformInfo = oclPlatformInfos[i];

                     for (int j = 0; j < platformInfo.DeviceNumber; j++)
                     {
                        Ocl.Device device = platformInfo.GetDevice(j);

                        Trace.WriteLine(String.Format("{0}Setting device to {1}", Environment.NewLine, device.Name));
                        //OclDevice d = new OclDevice();
                        //d.Set(device.NativeDevicePointer);


                        Ocl.Device defaultDevice = Ocl.Device.Default;
                        defaultDevice.Set(device.NativeDevicePointer);

                        Trace.WriteLine(String.Format("Current OpenCL default device: {0}", defaultDevice.Name));

                        UMat m = new UMat(2048, 2048, DepthType.Cv8U, 3);
                        m.SetTo(new MCvScalar(100, 100, 100));
                        CvInvoke.GaussianBlur(m, m, new Size(3, 3), 3);

                        Stopwatch watch = Stopwatch.StartNew();
                        m.SetTo(new MCvScalar(100, 100, 100));
                        CvInvoke.GaussianBlur(m, m, new Size(3, 3), 3);
                        watch.Stop();
                        Trace.WriteLine(String.Format("Device '{0}' time: {1} milliseconds", defaultDevice.Name,
                           watch.ElapsedMilliseconds));
                        CvInvoke.OclFinish();
                     }
                  }
               }

               Trace.WriteLine(Environment.NewLine + "Disable OpenCL");
               CvInvoke.UseOpenCL = false;
               UMat m2 = new UMat(2048, 2048, DepthType.Cv8U, 3);
               m2.SetTo(new MCvScalar(100, 100, 100));
               CvInvoke.GaussianBlur(m2, m2, new Size(3, 3), 3);

               Stopwatch watch2 = Stopwatch.StartNew();
               m2.SetTo(new MCvScalar(100, 100, 100));
               CvInvoke.GaussianBlur(m2, m2, new Size(3, 3), 3);
               watch2.Stop();
               Trace.WriteLine(String.Format("CPU time: {0} milliseconds", watch2.ElapsedMilliseconds));
               CvInvoke.UseOpenCL = true;
            }
         }
      }

 
   }
}
