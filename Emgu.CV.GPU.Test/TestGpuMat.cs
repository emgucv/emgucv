using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using Emgu.CV.GPU;

namespace Emgu.CV.GPU.Test
{
   [TestFixture]
   public class TestGpuMat
   {
      public int TestGetCudaEnabledDeviceCount()
      {
         return GpuInvoke.GetCudaEnabledDeviceCount();
      }
   }
}
