//----------------------------------------------------------------------------
//  Copyright (C) 2004-2026 by EMGU Corporation. All rights reserved.       
//----------------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Structure;
using Emgu.CV.Util;

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
    public class AutoTestPhoto
    {
#if !(__ANDROID__ || __IOS__ || NETFX_CORE)
        [Test]
        public void TestHDR()
        {
            String[] lines = File.ReadAllLines("list.txt");

            List<Mat> imageList = new List<Mat>();
            List<float> timeList = new List<float>();
            foreach (var line in lines)
            {
                string[] words = line.Split(' ');
                String fileName = words[0];
                Mat m = CvInvoke.Imread(fileName);
                float time = 1.0f / float.Parse(words[1]);
                imageList.Add(m);
                timeList.Add(time);
            }

            using (VectorOfMat images = new VectorOfMat(imageList.ToArray()))
            using (VectorOfFloat times = new VectorOfFloat(timeList.ToArray()))
            {
                Mat response = new Mat();
                CalibrateDebevec calibrate = new CalibrateDebevec();
                calibrate.Process(images, response, times);

                Mat hdr = new Mat();
                MergeDebevec mergeDebevec = new MergeDebevec();
                mergeDebevec.Process(images, hdr, times, response);

                Mat ldr = new Mat();
                Tonemap tonemap = new Tonemap(2.2f);
                tonemap.Process(hdr, ldr);

                Mat fusion = new Mat();
                MergeMertens mergeMertens = new MergeMertens();
                mergeMertens.Process(images, fusion, times, response);
                //mergeMertens.Process(images, fusion);

                using (Mat fusionScaled = new Mat())
                using (Mat ldrScaled = new Mat())
                using (ScalarArray sa = new ScalarArray(new MCvScalar(255.0, 255.0, 255.0)))
                {
                    fusion.ConvertTo(fusionScaled, DepthType.Cv8U, 255);
                    CvInvoke.Imwrite("fusion.png", fusionScaled);

                    ldr.ConvertTo(ldrScaled, DepthType.Cv8U, 255);
                    CvInvoke.Imwrite("ldr.png", ldrScaled);

                    CvInvoke.Imwrite("hdr.hdr", hdr);
                }

            }
            imageList.Clear();

        }
#endif

        [Test]
        public void TestOilPainting()
        {
            Mat m = EmguAssert.LoadMat("pedestrian.png");
            Mat result = new Mat();
            Emgu.CV.XPhoto.XPhotoInvoke.OilPainting(m, result, 10, 1, ColorConversion.Bgr2Lab);
            //CvInvoke.Imshow("oil painting", result);
            //CvInvoke.WaitKey();
        }
    }
}
