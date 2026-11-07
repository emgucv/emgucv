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
    public class AutoTestDnn
    {
#if !(__IOS__ || NETFX_CORE)

        private static String[] ReadClassNames(String fileName)
        {
            return File.ReadAllLines(fileName);
        }

        /* Find best class for the blob (i. e. class with maximal probability) */
        private static void GetMaxClass(Mat probBlob, out int classId, out double classProb)
        {
            //Mat matRef = probBlob.MatRef();
            Mat probMat = probBlob.Reshape(1, 1); //reshape the blob to 1x1000 matrix
            Point minLoc = new Point(), maxLoc = new Point();
            double minVal = 0, maxVal = 0;
            CvInvoke.MinMaxLoc(probMat, ref minVal, ref maxVal, ref minLoc, ref maxLoc);
            classId = maxLoc.X;
            classProb = maxVal;
        }

#endif

        [Test]
        public void TestBlobFromImageInterpolation()
        {
            using (Mat img = new Mat(new Size(100, 80), DepthType.Cv8U, 3))
            {
                CvInvoke.Randu(img, new MCvScalar(), new MCvScalar(255, 255, 255));
                using (Mat blobLinear = DnnInvoke.BlobFromImage(img, 1.0 / 255, new Size(64, 64)))
                using (Mat blobExact = DnnInvoke.BlobFromImage(img, 1.0 / 255, new Size(64, 64), new MCvScalar(), false, false, DepthType.Cv32F, Inter.LinearExact))
                using (Mat blobNearest = DnnInvoke.BlobFromImage(img, 1.0 / 255, new Size(64, 64), new MCvScalar(), false, false, DepthType.Cv32F, Inter.Nearest))
                {
                    EmguAssert.AreEqual(4, blobExact.Dims);
                    EmguAssert.AreEqual(64, blobExact.SizeOfDimension[3]);
                    EmguAssert.AreEqual(64, blobNearest.SizeOfDimension[3]);
                    EmguAssert.AreEqual(64, blobLinear.SizeOfDimension[3]);
                }
            }
        }
    }
}
