//----------------------------------------------------------------------------
//  Copyright (C) 2004-2013 by EMGU. All rights reserved.       
//----------------------------------------------------------------------------
﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Text;
using Emgu.CV.OCR;
using Emgu.CV.Structure;
using Emgu.CV.OpenCL;
using NUnit.Framework;

namespace Emgu.CV.Test
{
   [TestFixture]
   public class AutoTestOpenCL
   {
      [Test]
      public void TestSplitMerge()
      {
         int device = OclInvoke.GetDevice(OclDeviceType.All);
         if (device > 0)
         {
            using (Image<Bgr, Byte> img1 = new Image<Bgr, byte>(1200, 640))
            {
               img1.SetRandUniform(new MCvScalar(0, 0, 0), new MCvScalar(255, 255, 255));

               using (OclImage<Bgr, Byte> gpuImg1 = new OclImage<Bgr, byte>(img1))
               {
                  OclImage<Gray, Byte>[] channels = gpuImg1.Split();

                  for (int i = 0; i < channels.Length; i++)
                  {
                     Image<Gray, Byte> imgL = channels[i].ToImage();
                     Image<Gray, Byte> imgR = img1[i];
                     Assert.IsTrue(imgL.Equals(imgR), "failed split GpuMat");
                  }

                  using (OclImage<Bgr, Byte> gpuImg2 = new OclImage<Bgr, byte>(channels[0].Size))
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
      }
   }
}
