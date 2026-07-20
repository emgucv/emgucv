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
using Emgu.Util;
using System.Threading.Tasks;
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
    public class AutoTestStructuredLight
    {
#if !NETFX_CORE
        [Test]
        public void TestGrayCodePattern()
        {
            using (GrayCodePattern gcp = new GrayCodePattern(1024, 768))
            using (VectorOfMat vm = new VectorOfMat())
            {
                bool generated = gcp.Generate(vm);
            }
        }

        [Test]
        public void SinusoidalPattern()
        {
            using (SinusoidalPattern sp = new SinusoidalPattern())
            using (VectorOfMat vm = new VectorOfMat())
            {
                bool generated = sp.Generate(vm);
            }
        }

#if VS_TEST
        [Ignore]
#else
        [Ignore("Ignore from test run by default to avoid downloading data.")]
#endif
        [Test]
        public async Task TestPhaseUnwrapping()
        {
            FileDownloadManager manager = new FileDownloadManager();

            manager.AddFile(new DownloadableFile(
                "https://github.com/opencv/opencv_extra/raw/4.5.3/testdata/cv/phase_unwrapping/data/wrappedpeaks.yml",
                "PhaseUnwrapping"));
            await manager.Download();
            using (FileStorage fs = new FileStorage(manager.Files[0].LocalFile, FileStorage.Mode.Read))
            using (Mat wPhaseMat = new Mat())
            using (Mat uPhaseMat = new Mat())
            using (Mat reliabilities = new Mat())
            using (FileNode pvNode = fs["phaseValues"])
            {
                pvNode.ReadMat(wPhaseMat);
                using (Emgu.CV.PhaseUnwrapping.HistogramPhaseUnwrapping phaseUnwrapping =
                    new PhaseUnwrapping.HistogramPhaseUnwrapping(
                    wPhaseMat.Cols, wPhaseMat.Rows))
                {
                    phaseUnwrapping.UnwrapPhaseMap(wPhaseMat, uPhaseMat);
                    phaseUnwrapping.GetInverseReliabilityMap(reliabilities);
                    using (Mat uPhaseMap8 = new Mat())
                    using (Mat wPhaseMap8 = new Mat())
                    using (Mat reliabilities8 = new Mat())
                    {
                        wPhaseMat.ConvertTo(wPhaseMap8, DepthType.Cv8U, 255, 128);
                        uPhaseMat.ConvertTo(uPhaseMap8, DepthType.Cv8U, 1, 128);
                        reliabilities.ConvertTo(reliabilities8, DepthType.Cv8U, 255, 128);
                    }
                }
            }
        }
#endif
    }
}
