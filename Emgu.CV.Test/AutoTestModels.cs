//----------------------------------------------------------------------------
//  Copyright (C) 2004-2022 by EMGU Corporation. All rights reserved.       
//----------------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;  
using System.Runtime.Serialization;

using System.Text;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;
using Emgu.CV;
using Emgu.CV.Aruco;
using Emgu.CV.CvEnum;
using Emgu.CV.Features2D;
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

using Emgu.CV.Dnn;
using Emgu.CV.Cuda;
using Emgu.CV.DepthAI;
using Emgu.CV.Mcc;
using Emgu.CV.Models;
using Emgu.CV.Tiff;
//using Emgu.CV.UI;
using Emgu.CV.Util;
using Emgu.CV.VideoStab;
using Emgu.CV.XFeatures2D;
using Emgu.CV.XImgproc;
using Emgu.Util;

using System.Threading.Tasks;

//using Newtonsoft.Json;
using DetectorParameters = Emgu.CV.Aruco.DetectorParameters;
using DistType = Emgu.CV.CvEnum.DistType;
#if VS_TEST
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TestAttribute = Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute;
using TestFixture = Microsoft.VisualStudio.TestTools.UnitTesting.TestClassAttribute;
#elif NETFX_CORE
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
using TestAttribute = Microsoft.VisualStudio.TestPlatform.UnitTestFramework.TestMethodAttribute;
using TestFixture = Microsoft.VisualStudio.TestPlatform.UnitTestFramework.TestClassAttribute;
#else
using Emgu.CV.ML;
using NUnit.Framework;
#endif


namespace Emgu.CV.Test
{
    [TestFixture]
    public class AutoTestModels
    {
        private static void DownloadManager_OnDownloadProgressChanged(long? totalBytesToReceive, long bytesReceived, double? progressPercentage)
        {
            if (totalBytesToReceive != null) 
                Trace.WriteLine(String.Format("{0} bytes downloaded.", bytesReceived));
            else
                Trace.WriteLine(String.Format("{0} of {1} bytes downloaded ({2}%)", bytesReceived, totalBytesToReceive, progressPercentage));
        }
        
#if !TEST_MODELS
        [Ignore("Ignore from test run by default.")]
#endif
        [Test]
        public async Task TestWeChatQRCode()
        {
            using (Mat m = EmguAssert.LoadMat("link_github_ocv.jpg"))
            using (Emgu.CV.Models.WeChatQRCodeDetector detector = new WeChatQRCodeDetector())
            {
                await detector.Init(DownloadManager_OnDownloadProgressChanged);
                String text = detector.ProcessAndRender(m, m);
            }

        }

#if !TEST_MODELS
        [Ignore("Ignore from test run by default.")]
#endif
        [Test]
        public async Task TestPedestrianDetector()
        {
            using (Mat m = EmguAssert.LoadMat("pedestrian"))
            using (Emgu.CV.Models.PedestrianDetector detector = new PedestrianDetector())
            {
                await detector.Init(DownloadManager_OnDownloadProgressChanged);
                String text = detector.ProcessAndRender(m, m);
            }

        }

    }
}
