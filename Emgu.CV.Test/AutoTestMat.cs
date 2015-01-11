//----------------------------------------------------------------------------
//  Copyright (C) 2004-2014 by EMGU Corporation. All rights reserved.       
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
using Emgu.CV.CvEnum;
using Emgu.CV.Features2D;
using Emgu.CV.Structure;
using Emgu.Util;
#if NETFX_CORE
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
using TestAttribute = Microsoft.VisualStudio.TestPlatform.UnitTestFramework.TestMethodAttribute;
using TestFixture = Microsoft.VisualStudio.TestPlatform.UnitTestFramework.TestClassAttribute;
#else
using NUnit.Framework;
#endif
namespace Emgu.CV.Test
{
   [TestFixture]
   public class AutoTestMat
   {
      [TestAttribute]
      public void TestMatCreate()
      {
         Mat m = new Mat();
         m.Create(10, 12, CvEnum.DepthType.Cv8U, 1);
         m.Create(18, 22, CvEnum.DepthType.Cv64F, 3);
      }

      [TestAttribute]
      public void TestArrToMat()
      {
         Matrix<float> m = new Matrix<float>(320, 240);
         Mat mat = new Mat();
         m.Mat.CopyTo(mat);
         EmguAssert.IsTrue(m.Mat.Depth == DepthType.Cv32F);
         EmguAssert.IsTrue(mat.Depth == DepthType.Cv32F);
      }

      [TestAttribute]
      public void TestMatEquals()
      {
         Mat m1 = new Mat(640, 320, DepthType.Cv8U, 3);
         m1.SetTo(new MCvScalar(1, 2, 3));
         Mat m2 = new Mat(640, 320, DepthType.Cv8U, 3);
         m2.SetTo(new MCvScalar(1, 2, 3));
         
         EmguAssert.IsTrue(m1.Equals(m2));

      }

#if !NETFX_CORE
      [TestAttribute]
      public void TestRuntimeSerialize()
      {
         Mat img = new Mat(100, 80, DepthType.Cv8U, 3);

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
               Mat img2 = (Mat)o;
               EmguAssert.IsTrue(img.Equals(img2));
            }
         }
      }
#endif
   }
}
