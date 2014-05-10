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
using Emgu.CV.Features2D;
using Emgu.CV.Structure;
using Emgu.Util;
using NUnit.Framework;

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
   }
}
