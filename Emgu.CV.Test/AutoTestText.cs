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
    public class AutoTestText
    {
#if !NETFX_CORE

        [Test]
        public void TestVectorOfCvERStat()
        {
            //CvInvoke.CheckLibraryLoaded();
            int sizeOfElement = Emgu.Util.Toolbox.SizeOf<MCvERStat>();
            using (VectorOfERStat v = new VectorOfERStat())
            {

            }
        }

        [Test]
        public static void TestBackgroundSubtractorMOG2()
        {
            //ImageViewer viewer = new ImageViewer();
            using (VideoCapture capture = new VideoCapture("tree.avi"))
            using (BackgroundSubtractorMOG2 subtractor = new BackgroundSubtractorMOG2())
            using (Mat frame = new Mat())
            using (Mat fgMask = new Mat())
            {
                int frameCount = 0;
                while (capture.Grab())
                {
                    capture.Retrieve(frame);
                    subtractor.Apply(frame, fgMask);
                    frameCount++;
                }
                EmguAssert.IsTrue(frameCount > 0, "BackgroundSubtractorMOG2 did not return any frames");
            }
        }

        [Test]
        public static void TestIntensityTransform()
        {
            Mat m = new Mat("lena.jpg", ImreadModes.ColorBgr);
            Mat bimef = new Mat();
            Mat autoScaling = new Mat();
            Mat gamma = new Mat();
            Mat contrastStretch = new Mat();
            Mat log = new Mat();
            Emgu.CV.IntensityTransform.IntensityTransformInvoke.BIMEF(m, bimef);
            Emgu.CV.IntensityTransform.IntensityTransformInvoke.Autoscaling(m, autoScaling);
            Emgu.CV.IntensityTransform.IntensityTransformInvoke.GammaCorrection(m, gamma, 2.0f);
            Emgu.CV.IntensityTransform.IntensityTransformInvoke.ContrastStretching(m, contrastStretch, 0, 0, 200, 200);
            Emgu.CV.IntensityTransform.IntensityTransformInvoke.LogTransform(m, log);
        }

        [Test]
        public static void TestResampleSignal()
        {
            int inFreq = 44100;
            int outFreq = 22050;
            int inputLength = 4410; // 0.1 seconds of samples at inFreq

            // Build a 440 Hz sine wave as a 1-row CV_32FC1 Mat
            float[] signalData = new float[inputLength];
            for (int i = 0; i < inputLength; i++)
                signalData[i] = (float)Math.Sin(2.0 * Math.PI * 440.0 * i / inFreq);

            using (Mat input = new Mat(1, inputLength, DepthType.Cv32F, 1))
            using (Mat output = new Mat())
            {
                System.Runtime.InteropServices.Marshal.Copy(signalData, 0, input.DataPointer, inputLength);

                Emgu.CV.Signal.SignalInvoke.ResampleSignal(input, output, inFreq, outFreq);

                int expectedLength = (int)Math.Floor((double)inputLength * outFreq / inFreq);
                EmguAssert.IsTrue(!output.IsEmpty, "ResampleSignal output should not be empty");
                EmguAssert.IsTrue(
                    output.Cols == expectedLength,
                    String.Format("ResampleSignal: expected {0} output samples, got {1}", expectedLength, output.Cols));
            }
        }

        [Test]
        public void TestERFilter()
        {
            CvInvoke.SanityCheck();
            bool checkInvert = true;
            using (Mat image = EmguAssert.LoadMat("scenetext01.jpg", ImreadModes.ColorBgr))
            using (ERFilterNM1 er1 = new ERFilterNM1(EmguAssert.GetFile("trained_classifierNM1.xml"), 8, 0.00025f, 0.13f, 0.4f, true, 0.1f))
            using (ERFilterNM2 er2 = new ERFilterNM2(EmguAssert.GetFile("trained_classifierNM2.xml"), 0.3f))
            {
                //using (Image<Gray, Byte> mask = new Image<Gray,byte>(image.Size.Width + 2, image.Size.Height + 2))
                int channelCount = image.NumberOfChannels;
                UMat[] channels = new UMat[checkInvert ? channelCount * 2 : channelCount];

                for (int i = 0; i < channelCount; i++)
                {
                    UMat c = new UMat();
                    CvInvoke.ExtractChannel(image, c, i);
                    channels[i] = c;
                }

                if (checkInvert)
                {
                    for (int i = 0; i < channelCount; i++)
                    {
                        UMat c = new UMat();
                        CvInvoke.BitwiseNot(channels[i], c);
                        channels[i + channelCount] = c;
                    }
                }

                VectorOfERStat[] regionVecs = new VectorOfERStat[channels.Length];


                for (int i = 0; i < regionVecs.Length; i++)
                    regionVecs[i] = new VectorOfERStat();

                /*
                for (int i = 0; i < channels.Length; i++)
                {
                   Emgu.CV.WinForms.ImageViewer.Show(channels[i]);
                }*/

                try
                {
                    for (int i = 0; i < channels.Length; i++)
                    {
                        er1.Run(channels[i], regionVecs[i]);
                        er2.Run(channels[i], regionVecs[i]);
                    }
                    using (VectorOfUMat vm = new VectorOfUMat(channels))
                    {
                        Rectangle[] regions = ERFilter.ERGrouping(image, vm, regionVecs, ERFilter.GroupingMethod.OrientationHoriz, EmguAssert.GetFile("trained_classifier_erGrouping.xml"), 0.5f);

                        /*
                        System.Runtime.Serialization.Formatters.Binary.BinaryFormatter formatter = new BinaryFormatter();
                        using (MemoryStream ms = new MemoryStream())
                        {
                            formatter.Serialize(ms, regionVecs);

                            VectorOfERStat rv2 = formatter.Deserialize(ms) as VectorOfERStat;
                        }*/

                        foreach (Rectangle rect in regions)
                            CvInvoke.Rectangle(image, rect, new MCvScalar(0,0,255), 2);
                            //image.Draw(rect, new Bgr(0, 0, 255), 2);

                    }
                }
                finally
                {
                    foreach (UMat tmp in channels)
                        if (tmp != null)
                            tmp.Dispose();
                    foreach (VectorOfERStat tmp in regionVecs)
                        if (tmp != null)
                            tmp.Dispose();
                }
                //Emgu.CV.WinForms.ImageViewer.Show(image);

            }

        }
#endif
    }
}
