//----------------------------------------------------------------------------
//  Copyright (C) 2004-2026 by EMGU Corporation. All rights reserved.
//----------------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;
using Emgu.CV;
using Emgu.CV.Aruco;
using Emgu.CV.CvEnum;
using Emgu.CV.Features;
using Emgu.CV.Flann;
using Emgu.CV.Shape;
using Emgu.CV.Stitching;
using Emgu.CV.Text;
using Emgu.CV.Structure;
using Emgu.CV.Bioinspired;
using Emgu.CV.Dpm;
using Emgu.CV.ImgHash;
using Emgu.CV.Face;
using Emgu.CV.Freetype;
using Emgu.CV.StructuredLight;
using Emgu.CV.Dnn;
using Emgu.CV.Cuda;
using Emgu.CV.Mcc;
using Emgu.CV.Models;
using Emgu.CV.Tiff;
using Emgu.CV.Util;
using Emgu.CV.VideoStab;
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
    public class AutoTestRetina
    {
        [Test]
        public void TestRetina()
        {

            Mat image = EmguAssert.LoadMat("pedestrian.png", ImreadModes.ColorBgr);
            using (Retina retina = new Retina(
                       image.Size,
                       true,
                       Retina.ColorSamplingMethod.ColorBayer,
                       false,
                       1.0,
                       10.0))
            {
                Retina.RetinaParameters p = retina.Parameters;
                Retina.IplMagnoParameters iplP = p.IplMagno;
                float oldval = iplP.ParasolCellsK;
                iplP.ParasolCellsK += 0.01f;
                iplP.NormaliseOutput = false;
                p.IplMagno = iplP;
                retina.Parameters = p;
                float newval = retina.Parameters.IplMagno.ParasolCellsK;

                EmguAssert.AreEqual(newval, oldval + 0.01f);

                retina.Run(image);
                Mat out1 = new Mat();
                Mat out2 = new Mat();
                retina.GetMagno(out1);
                retina.GetParvo(out2);
            }
        }

#if !NETFX_CORE
        [Test]
        public void TestRetinaFastToneMapping()
        {
            if (Emgu.Util.Platform.OperationSystem == Emgu.Util.Platform.OS.Windows)
            {
                //TODO: Find out why this fails on Ubuntu
                Mat m = EmguAssert.LoadMat("pedestrian.png");
                Mat result = new Mat();
                using (Bioinspired.RetinaFastToneMapping tm = new Bioinspired.RetinaFastToneMapping(m.Size))
                using (Mat gray = new Mat())
                {
                    CvInvoke.CvtColor(m, gray, ColorConversion.Bgr2Gray);
                    tm.Setup(3.0f, 1.0f, 1.0f);
                    tm.ApplyFastToneMapping(gray, result);
                }

                //CvInvoke.Imshow("Tone Mapping", result);
                //CvInvoke.WaitKey();
            }
        }
#endif
    }
}
