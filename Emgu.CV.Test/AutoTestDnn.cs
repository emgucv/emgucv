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

        /*
        [Test]
        public void TestDnnPersonDetection()
        {
            string modelXmlFile = "person-detection-retail-0013.xml";
            String modelFile = "person-detection-retail-0013.bin";
            if (!File.Exists(modelFile))
            {
                //Download the model file
                String modelUrl = "https://download.01.org/opencv/2020/openvinotoolkit/2020.3/open_model_zoo/models_bin/1/person-detection-retail-0013/FP16/person-detection-retail-0013.bin";
                Trace.WriteLine("downloading file from:" + modelUrl + " to: " + modelFile);
                System.Net.WebClient downloadClient = new System.Net.WebClient();
                try
                {
                    downloadClient.DownloadFile(modelUrl, modelFile);
                }
                catch
                {
                    //Delete the file in case of failed download.
                    File.Delete(modelFile);
                    throw;
                }
            }
            if (!File.Exists(modelXmlFile))
            {
                //Download the model xml file
                String modelXmlUrl = "https://download.01.org/opencv/2020/openvinotoolkit/2020.3/open_model_zoo/models_bin/1/person-detection-retail-0013/FP16/person-detection-retail-0013.xml";
                Trace.WriteLine("downloading file from:" + modelXmlUrl + " to: " + modelXmlFile);
                System.Net.WebClient downloadClient = new System.Net.WebClient();
                try
                {
                    downloadClient.DownloadFile(modelXmlUrl, modelXmlFile);
                }
                catch
                {
                    //Delete the file in case of failed download.
                    File.Delete(modelXmlFile);
                    throw;
                }
            }

            Dnn.Net net = DnnInvoke.ReadNetFromModelOptimizer(modelXmlFile, modelFile);

            Mat img = EmguAssert.LoadMat("space_shuttle.jpg");

            //CvInvoke.Resize(img, img, new Size(224, 224));

            //Mat inputBlob = DnnInvoke.BlobFromImage(img);
            //net.SetInput(inputBlob, "data");
            //Mat probBlob = net.Forward("prob");

        }
        */

#if !TEST_MODELS
#if VS_TEST
        [Ignore()]
#else
        [Ignore("Ignore from test run by default.")]
#endif
#endif
        [Test]
        public void TestDnnTensorFlow()
        {

            //Dnn.Net net = new Dnn.Net();
            String tensorFlowFile = "tensorflow_inception_graph.pb";
            if (!File.Exists(tensorFlowFile))
            {
                //Download the tensorflow file
                String inceptionFile = "inception5h.zip";
                String googleNetUrl = "https://storage.googleapis.com/download.tensorflow.org/models/inception5h.zip";
                Trace.WriteLine("downloading file from:" + googleNetUrl + " to: " + inceptionFile);
                using (HttpClient httpClient = new HttpClient())
                using (System.IO.Stream responseStream = httpClient.GetStreamAsync(googleNetUrl).GetAwaiter().GetResult())
                using (FileStream fileStream = new FileStream(inceptionFile, FileMode.Create))
                {
                    responseStream.CopyTo(fileStream);
                }

                System.IO.Compression.ZipFile.ExtractToDirectory(inceptionFile, ".");
            }

            Dnn.Net net = DnnInvoke.ReadNetFromTensorflow(tensorFlowFile);
            //using (Dnn.Importer importer = Dnn.Importer.CreateTensorflowImporter(tensorFlowFile))
            //    importer.PopulateNet(net);

            Mat img = EmguAssert.LoadMat("space_shuttle.jpg");

            CvInvoke.Resize(img, img, new Size(224, 224));
            CvInvoke.CvtColor(img, img, ColorConversion.Bgr2Rgb);

            Mat inputBlob = DnnInvoke.BlobFromImage(img);
            net.SetInput(inputBlob, "input");
            Mat probBlob = net.Forward("softmax2");
            /*
            Dnn.Blob inputBlob = new Dnn.Blob();
            inputBlob.BatchFromImages(img);
            net.SetBlob(".input", inputBlob);
            net.Forward();
            Dnn.Blob probBlob = net.GetBlob("softmax2");
            */
            int classId;
            double classProb;
            GetMaxClass(probBlob, out classId, out classProb);
            String[] classNames = ReadClassNames("imagenet_comp_graph_label_strings.txt");

            //#if !NETFX_CORE
            Trace.WriteLine("Best class: " + classNames[classId] + ". Probability: " + classProb);
            //#endif

        }
#endif
    }
}
