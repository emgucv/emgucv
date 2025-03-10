﻿//----------------------------------------------------------------------------
//  Copyright (C) 2004-2025 by EMGU Corporation. All rights reserved.       
//----------------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Xml;
using System.Xml.Linq;
using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Features2D;
using Emgu.CV.Quality;
using Emgu.CV.Structure;
using Emgu.CV.Util;
using Emgu.Util;

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
    public class AutoTestMat
    {
        [TestAttribute]
        public void TestMatCreate()
        {
            Mat m = new Mat();
            var data = m.GetData();
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
        public void TestMatToByteArray()
        {
            Mat m = new Mat(new Size(320, 240), DepthType.Cv8U, 3);
            byte[] bytes = new byte[m.Total.ToInt32() * m.ElementSize];
            m.CopyTo(bytes);
        }

        [TestAttribute]
        public void TestMatToArr()
        {

            Mat mat = new Mat(new Size(320, 240), DepthType.Cv32F, 1);

            Matrix<float> m = new Matrix<float>(mat.Rows, mat.Cols, mat.NumberOfChannels);
            mat.CopyTo(m);

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

        [TestAttribute]
        public void TestMatMultiply()
        {
            Mat m1 = new Mat(640, 320, DepthType.Cv8U, 3);
            m1.SetTo(new MCvScalar(100, 100, 100));
            Mat m2 = new Mat();
            using (ScalarArray sa = new ScalarArray(new MCvScalar(.5, 0.5, 0.5)))
            {
                CvInvoke.Multiply(m1, sa, m2);
                MCvScalar mean = new MCvScalar();
                MCvScalar std = new MCvScalar();
                CvInvoke.MeanStdDev(m2, ref mean, ref std);
            }
            
        }

        [TestAttribute]
        public void TestMatPixelAccess()
        {
            Mat m1 = EmguAssert.LoadMat("lena.jpg");
            byte[] data = new byte[m1.Width * m1.Height * 3]; //3 channel bgr image data
            GCHandle handle = GCHandle.Alloc(data, GCHandleType.Pinned);
            using (Mat m2 = new Mat(m1.Size, DepthType.Cv8U, 3, handle.AddrOfPinnedObject(), m1.Width * 3))
            {
                CvInvoke.BitwiseNot(m1, m2);

                var span = m1.GetSpan<byte>();
                for (int i = 0; i < span.Length; i++)
                {
                    span[i] = (byte)((byte)255 - (byte)(span[i]));
                }
                EmguAssert.IsTrue(m1.Equals(m2));
            }

            handle.Free();
            //now the data array contains the pixel data of the inverted lena image.
            //note that if the m2 Mat was allocated with the wrong size, data[] array will contains all 0s, and no exception will be thrown
            //so be really careful when performing the above operations.

        }


        [TestAttribute]
        public void TestConvolutionAndLaplace()
        {
            Mat image = new Mat(new Size(300, 400), DepthType.Cv8U, 1);
            CvInvoke.Randu(image, new MCvScalar(0.0), new MCvScalar(255.0));
            Mat laplacian = new Mat();
            CvInvoke.Laplacian(image, laplacian, DepthType.Cv8U);

            float[,] k = { {0, 1, 0},
              {1, -4, 1},
              {0, 1, 0}};
            ConvolutionKernelF kernel = new ConvolutionKernelF(k);
            Mat convoluted = new Mat(image.Size, DepthType.Cv8U, 1);
            CvInvoke.Filter2D(image, convoluted, kernel, kernel.Center);

            Mat absDiff = new Mat();

            CvInvoke.AbsDiff(laplacian, convoluted, absDiff);
            int nonZeroPixelCount = CvInvoke.CountNonZero(absDiff);

            EmguAssert.IsTrue(nonZeroPixelCount == 0);

            //Emgu.CV.UI.ImageViewer.Show(absDiff);
        }

        [TestAttribute]
        public void TestGetRowCol()
        {
            Mat m = new Mat(new Size(2, 2), DepthType.Cv64F, 1);
            double[] value = new double[]
            {
            1, 2,
            3, 4
            };
            m.SetTo(value);

            Mat secondRow = m.Row(1);
            double[] secondRowValue = new double[2];
            secondRow.CopyTo(secondRowValue);
            EmguAssert.IsTrue(value[2] == secondRowValue[0]);
            EmguAssert.IsTrue(value[3] == secondRowValue[1]);

            Mat secondCol = m.Col(1);
            double[] secondColValue = new double[2];
            secondCol.CopyTo(secondColValue);
            EmguAssert.IsTrue(value[1] == secondColValue[0]);
            EmguAssert.IsTrue(value[3] == secondColValue[1]);
        }

        [TestAttribute]
        public void TestMatToFileStorage()
        {
            //create a matrix m with random values
            Mat m = new Mat(120, 240, DepthType.Cv8U, 1);
            using (ScalarArray low = new ScalarArray(0))
            using (ScalarArray high = new ScalarArray(255))
                CvInvoke.Randu(m, low, high);

            //Convert the random matrix m to yml format, good for matrix that contains values such as calibration, homography etc.
            String mStr;
            using (FileStorage fs = new FileStorage(".yml", FileStorage.Mode.Write | FileStorage.Mode.Memory))
            {
                fs.Write(m, "m");
                mStr = fs.ReleaseAndGetString();
            }

            using (FileStorage fs = new FileStorage(".xml", FileStorage.Mode.FormatXml | FileStorage.Mode.Write | FileStorage.Mode.Memory))
            {
                fs.Write(m, "m");
                mStr = fs.ReleaseAndGetString();
            }

            //Treat the Mat as image data and convert it to png format.
            using (VectorOfByte bytes = new VectorOfByte())
            {
                CvInvoke.Imencode(".png", m, bytes);

                byte[] rawData = bytes.ToArray();
            }
        }

        [TestAttribute]
        public void TestVectorOfVectorOfMat()
        {
            using (Mat m = new Mat())
            using (VectorOfMat vm = new VectorOfMat())
            {
                vm.Push(m);
                using (VectorOfVectorOfMat vvm = new VectorOfVectorOfMat())
                {
                    vvm.Push(vm);
                }
            }
        }

        [TestAttribute]
        public void TestQuality()
        {
            Mat m1 = EmguAssert.LoadMat("lena.jpg");
            Mat m2 = new Mat();
            CvInvoke.GaussianBlur(m1, m2, new Size(5, 5), 3);
            using (Emgu.CV.Quality.QualityMSE q = new QualityMSE(m1))
            {
                MCvScalar quality = q.Compute(m2);
            }

        }

        [Test]
        public void TestJsonSerializeAndDeserialize()
        {
            using (Mat mat = new Mat(new Size(50, 60), DepthType.Cv8U, 3))
            {
                String jsonString = JsonSerializer.Serialize(mat);

                using (Mat mat2 = JsonSerializer.Deserialize<Mat>(jsonString))
                    EmguAssert.IsTrue(mat.Equals(mat2));

            }
        }

        /*
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
        */
    }
}
