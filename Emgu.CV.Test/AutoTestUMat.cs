//----------------------------------------------------------------------------
//  Copyright (C) 2004-2018 by EMGU Corporation. All rights reserved.       
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
using System.Xml.Linq;
using Emgu.CV;
using Emgu.CV.Features2D;
using Emgu.CV.Structure;
using Emgu.CV.CvEnum;
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
   public class AutoTestUMat
   {
      [Test]
      public void TestUMatCreate()
      {
         if (CvInvoke.HaveOpenCL)
         {
            CvInvoke.UseOpenCL = true;
            using (UMat m1 = new UMat())
            {
               m1.Create(10, 12, CvEnum.DepthType.Cv8U, 1);
               EmguAssert.IsTrue(m1.Data == null);
            }
            CvInvoke.UseOpenCL = false;
            using(UMat m2 = new UMat())
               m2.Create(10, 12, CvEnum.DepthType.Cv8U, 1);
               //EmguAssert.IsTrue(m2.Data != null);
            
         } else
         {
            UMat m2 = new UMat();
            m2.Create(10, 12, CvEnum.DepthType.Cv8U, 1);
            //EmguAssert.IsTrue(m2.Data != null);
         }
      }

      [Test]
      public void TestUMatConvert()
      {
         using (UMat image = new UMat(10, 10, DepthType.Cv8U, 3))
         {
            image.SetTo(new MCvScalar(3, 4, 5));
            using (Image<Bgr, byte> imageDataSameColorSameDepth = image.ToImage<Bgr, byte>())
            using (Image<Gray, byte> imageDataDifferentColorSameDepth = image.ToImage<Gray, byte>())
            using (Image<Bgr, float> imageDataSameColorDifferentDepth = image.ToImage<Bgr, float>())
            {
            }
         }
      }

#if !NETFX_CORE
      [TestAttribute]
      public void TestRuntimeSerialize()
      {
         UMat img = new UMat(100, 80, DepthType.Cv8U, 3);

         using (MemoryStream ms = new MemoryStream())
         {
            //img.SetRandNormal(new MCvScalar(100, 100, 100), new MCvScalar(50, 50, 50));
            //img.SerializationCompressionRatio = 9;
            CvInvoke.SetIdentity(img, new MCvScalar(1, 2, 3));
            System.Runtime.Serialization.Formatters.Binary.BinaryFormatter
                formatter = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
            formatter.Serialize(ms, img);
            Byte[] bytes = ms.GetBuffer();

            using (MemoryStream ms2 = new MemoryStream(bytes))
            {
               Object o = formatter.Deserialize(ms2);
               UMat img2 = (UMat)o;
               EmguAssert.IsTrue(img.Equals(img2));
            }
         }
      }
#endif
   }
}
