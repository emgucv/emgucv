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
using NUnit.Framework;

namespace Emgu.CV.Test
{
   [TestFixture]
   public class AutoTestMat
   {
      [Test]
      public void TestMatCreate()
      {
         Mat m = new Mat();
         m.Create(10, 12, CvEnum.DepthType.Cv8U, 1);
         m.Create(18, 22, CvEnum.DepthType.Cv64F, 3);
      }

      [Test]
      public void TestArrToMat()
      {
         Matrix<float> m = new Matrix<float>(320, 240);
         Mat mat = new Mat();
         m.Mat.CopyTo(mat);
         EmguAssert.IsTrue(m.Mat.Depth == DepthType.Cv32F);
         EmguAssert.IsTrue(mat.Depth == DepthType.Cv32F);
      }

      [Test]
      public void TestMatEquals()
      {
         Mat m1 = new Mat(640, 320, DepthType.Cv8U, 3);
         m1.SetTo(new MCvScalar(1, 2, 3));
         Mat m2 = new Mat(640, 320, DepthType.Cv8U, 3);
         m2.SetTo(new MCvScalar(1, 2, 3));
         
         EmguAssert.IsTrue(m1.Equals(m2));

      }
   }
}
