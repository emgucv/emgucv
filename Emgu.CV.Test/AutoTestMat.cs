//----------------------------------------------------------------------------
//  Copyright (C) 2004-2026 by EMGU Corporation. All rights reserved.       
//----------------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Xml;
using System.Xml.Linq;
using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Features;
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
        public void TestMatNewDepthTypes()
        {
            // Depth types added in OpenCV 5: CV_16BF, CV_Bool, CV_64U, CV_64S, CV_32U
            using (Mat m = new Mat(4, 5, CvEnum.DepthType.Cv32U, 1))
            {
                EmguAssert.AreEqual(CvEnum.DepthType.Cv32U, m.Depth);
                EmguAssert.AreEqual(4, m.ElementSize);
                EmguAssert.AreEqual(typeof(UInt32), CvInvoke.GetDepthType(m.Depth));
            }
            using (Mat m = new Mat(4, 5, CvEnum.DepthType.Cv64U, 1))
            {
                EmguAssert.AreEqual(CvEnum.DepthType.Cv64U, m.Depth);
                EmguAssert.AreEqual(8, m.ElementSize);
                EmguAssert.AreEqual(typeof(UInt64), CvInvoke.GetDepthType(m.Depth));
            }
            using (Mat m = new Mat(4, 5, CvEnum.DepthType.Cv64S, 1))
            {
                EmguAssert.AreEqual(CvEnum.DepthType.Cv64S, m.Depth);
                EmguAssert.AreEqual(8, m.ElementSize);
                EmguAssert.AreEqual(typeof(Int64), CvInvoke.GetDepthType(m.Depth));
            }
            using (Mat m = new Mat(4, 5, CvEnum.DepthType.CvBool, 1))
            {
                EmguAssert.AreEqual(CvEnum.DepthType.CvBool, m.Depth);
                EmguAssert.AreEqual(1, m.ElementSize);
                m.SetTo(new MCvScalar(1));
                EmguAssert.AreEqual(20, CvInvoke.CountNonZero(m));
            }
            using (Mat m = new Mat(4, 5, CvEnum.DepthType.Cv16BF, 1))
            {
                EmguAssert.AreEqual(CvEnum.DepthType.Cv16BF, m.Depth);
                EmguAssert.AreEqual(2, m.ElementSize);
            }
            foreach (CvEnum.DepthType fp8 in new CvEnum.DepthType[] { CvEnum.DepthType.Cv8FE4M3FN, CvEnum.DepthType.Cv8FE4M3FNUZ })
            {
                using (Mat m = new Mat(4, 5, fp8, 1))
                using (Mat f32 = new Mat())
                using (Mat back = new Mat())
                {
                    EmguAssert.AreEqual(fp8, m.Depth);
                    EmguAssert.AreEqual(1, m.ElementSize);
                    m.SetTo(new MCvScalar(2));
                    m.ConvertTo(f32, CvEnum.DepthType.Cv32F);
                    EmguAssert.AreEqual(2.0, CvInvoke.Mean(f32).V0);
                    f32.ConvertTo(back, fp8);
                    EmguAssert.AreEqual(fp8, back.Depth);
                }
            }

            EmguAssert.AreEqual(CvEnum.DepthType.Cv32U,CvInvoke.GetDepthType(typeof(UInt32)));
            EmguAssert.AreEqual(CvEnum.DepthType.Cv64U, CvInvoke.GetDepthType(typeof(UInt64)));
            EmguAssert.AreEqual(CvEnum.DepthType.Cv64S, CvInvoke.GetDepthType(typeof(Int64)));
            EmguAssert.AreEqual(CvEnum.DepthType.CvBool, CvInvoke.GetDepthType(typeof(bool)));
        }

        [TestAttribute]
        public void TestMatConstructorRaisesNativeErrors()
        {
            //A ROI outside of the parent matrix is rejected by OpenCV; the Mat constructor must throw it
            //instead of leaving the error pending for the next unrelated call.
            bool thrown = false;
            using (Mat parent = new Mat(10, 10, DepthType.Cv8U, 1))
            {
                try
                {
                    using (Mat roi = new Mat(parent, new Rectangle(0, 0, 100, 100)))
                    {
                    }
                }
                catch (Emgu.CV.Util.CvException)
                {
                    thrown = true;
                }
            }

            EmguAssert.IsTrue(thrown, "A Mat with an out of range ROI should throw a CvException");
            //no stale error is left behind
            using (Mat ok = new Mat(2, 2, DepthType.Cv8U, 1))
            {
                EmguAssert.AreEqual(2, ok.Rows);
            }
        }

        [TestAttribute]
        public void TestUMatAndSolveLPRaiseNativeErrors()
        {
            //These natives did not catch OpenCV errors before, so an invalid argument could crash the process.
            bool thrown = false;
            using (UMat parent = new UMat(10, 10, DepthType.Cv8U, 1))
            {
                try
                {
                    using (UMat roi = new UMat(parent, new Rectangle(0, 0, 100, 100)))
                    {
                    }
                }
                catch (Emgu.CV.Util.CvException)
                {
                    thrown = true;
                }
            }

            EmguAssert.IsTrue(thrown, "A UMat with an out of range ROI should throw a CvException");

            thrown = false;
            try
            {
                using (Mat func = new Mat())
                using (Mat constr = new Mat())
                using (Mat z = new Mat())
                    CvInvoke.SolveLP(func, constr, z);
            }
            catch (Emgu.CV.Util.CvException)
            {
                thrown = true;
            }

            EmguAssert.IsTrue(thrown, "SolveLP with empty matrices should throw a CvException");
        }

        [TestAttribute]
        public void TestReduceNewTypes()
        {
            //OpenCV 5.1 added REDUCE_PROD, REDUCE_L1, REDUCE_L2, REDUCE_LOG_SUM and REDUCE_LOG_SUM_EXP.
            //For these types dim is the axis to reduce (0: rows are reduced to 1xN, 1: columns are reduced to Mx1).
            float[,] data = new float[,] { { 1, 2, 3, 4 }, { 5, 6, 7, 8 }, { 9, 10, 11, 12 } };
            int rows = data.GetLength(0);
            int cols = data.GetLength(1);
            float[] flat = new float[rows * cols];
            for (int r = 0; r < rows; r++)
                for (int c = 0; c < cols; c++)
                    flat[r * cols + c] = data[r, c];
            using (Mat src = new Mat(rows, cols, DepthType.Cv32F, 1))
            {
                Marshal.Copy(flat, 0, src.DataPointer, flat.Length);
                foreach (ReduceType type in new ReduceType[]
                {
                    ReduceType.ReduceProd, ReduceType.ReduceL1, ReduceType.ReduceL2,
                    ReduceType.ReduceLogSum, ReduceType.ReduceLogSumExp
                })
                {
                    for (int axis = 0; axis < 2; axis++)
                    {
                        int outerCount = axis == 0 ? cols : rows;
                        int innerCount = axis == 0 ? rows : cols;
                        double[] expected = new double[outerCount];
                        for (int o = 0; o < outerCount; o++)
                        {
                            double prod = 1, l1 = 0, l2 = 0, sum = 0, sumExp = 0;
                            for (int i = 0; i < innerCount; i++)
                            {
                                double v = axis == 0 ? data[i, o] : data[o, i];
                                prod *= v;
                                l1 += Math.Abs(v);
                                l2 += v * v;
                                sum += v;
                                sumExp += Math.Exp(v);
                            }
                            switch (type)
                            {
                                case ReduceType.ReduceProd: expected[o] = prod; break;
                                case ReduceType.ReduceL1: expected[o] = l1; break;
                                case ReduceType.ReduceL2: expected[o] = Math.Sqrt(l2); break;
                                case ReduceType.ReduceLogSum: expected[o] = Math.Log(sum); break;
                                default: expected[o] = Math.Log(sumExp); break;
                            }
                        }

                        using (Mat dst = new Mat())
                        {
                            CvInvoke.Reduce(src, dst, (ReduceDimension)axis, type, DepthType.Cv32F);
                            EmguAssert.AreEqual(axis == 0 ? 1 : rows, dst.Rows);
                            EmguAssert.AreEqual(axis == 0 ? cols : 1, dst.Cols);
                            float[] result = new float[outerCount];
                            Marshal.Copy(dst.DataPointer, result, 0, outerCount);
                            for (int o = 0; o < outerCount; o++)
                                EmguAssert.IsTrue(
                                    Math.Abs(result[o] - expected[o]) <= 1e-4 * Math.Max(1.0, Math.Abs(expected[o])),
                                    String.Format("{0} axis {1} index {2}: expected {3}, got {4}", type, axis, o, expected[o], result[o]));
                        }
                    }
                }
            }
        }

        [TestAttribute]
        public void TestReduceND()
        {
            //N-D reduce with ReduceParams (OpenCV 5.1) over a 2x3x4 array holding 1..24.
            int[] sizes = new int[] { 2, 3, 4 };
            float[] values = new float[24];
            for (int i = 0; i < values.Length; i++)
                values[i] = i + 1;

            GCHandle handle = GCHandle.Alloc(values, GCHandleType.Pinned);
            try
            {
                using (Mat src = new Mat(sizes, DepthType.Cv32F, handle.AddrOfPinnedObject()))
                {
                    //sum over axes 0 and 2, keeping the reduced dimensions: result is 1x3x1
                    using (Mat dst = new Mat())
                    {
                        CvInvoke.ReduceND(src, dst, ReduceType.ReduceSum, new int[] { 0, 2 }, true, DepthType.Cv32F);
                        int[] dstSizes = dst.SizeOfDimension;
                        EmguAssert.AreEqual(3, dstSizes.Length);
                        EmguAssert.AreEqual(1, dstSizes[0]);
                        EmguAssert.AreEqual(3, dstSizes[1]);
                        EmguAssert.AreEqual(1, dstSizes[2]);
                        float[] result = new float[3];
                        Marshal.Copy(dst.DataPointer, result, 0, 3);
                        for (int j = 0; j < 3; j++)
                        {
                            double expected = 0;
                            for (int i = 0; i < 2; i++)
                                for (int k = 0; k < 4; k++)
                                    expected += values[(i * 3 + j) * 4 + k];
                            EmguAssert.IsTrue(Math.Abs(result[j] - expected) < 1e-3, String.Format("sum j={0}: expected {1}, got {2}", j, expected, result[j]));
                        }
                    }

                    //negative axis: sum over the last axis without keeping it: 2x3 values
                    using (Mat dst = new Mat())
                    {
                        CvInvoke.ReduceND(src, dst, ReduceType.ReduceSum, new int[] { -1 }, false, DepthType.Cv32F);
                        int[] dstSizes = dst.SizeOfDimension;
                        EmguAssert.AreEqual(2, dstSizes.Length);
                        EmguAssert.AreEqual(2, dstSizes[0]);
                        EmguAssert.AreEqual(3, dstSizes[1]);
                        float[] result = new float[6];
                        Marshal.Copy(dst.DataPointer, result, 0, 6);
                        for (int r = 0; r < 6; r++)
                        {
                            double expected = 0;
                            for (int k = 0; k < 4; k++)
                                expected += values[r * 4 + k];
                            EmguAssert.IsTrue(Math.Abs(result[r] - expected) < 1e-3, String.Format("sum r={0}: expected {1}, got {2}", r, expected, result[r]));
                        }
                    }

                    //product over all axes (no axes given): 24!
                    using (Mat dst = new Mat())
                    {
                        CvInvoke.ReduceND(src, dst, ReduceType.ReduceProd, null, true, DepthType.Cv32F);
                        float[] result = new float[1];
                        Marshal.Copy(dst.DataPointer, result, 0, 1);
                        double expected = 1;
                        for (int i = 1; i <= 24; i++)
                            expected *= i;
                        EmguAssert.IsTrue(Math.Abs(result[0] - expected) <= 1e-4 * expected, String.Format("prod: expected {0}, got {1}", expected, result[0]));
                    }
                }
            }
            finally
            {
                handle.Free();
            }
        }

        [TestAttribute]
        public void TestOklabRoundTrip()
        {
            using (Mat bgr = new Mat(8, 8, DepthType.Cv32F, 3))
            using (Mat lab = new Mat())
            using (Mat back = new Mat())
            using (Mat diff = new Mat())
            {
                bgr.SetTo(new MCvScalar(0.2, 0.5, 0.8));
                CvInvoke.CvtColor(bgr, lab, ColorConversion.Bgr2Oklab);
                CvInvoke.CvtColor(lab, back, ColorConversion.Oklab2Bgr);
                CvInvoke.AbsDiff(bgr, back, diff);
                double maxDiff = 0;
                double minDiff = 0;
                Point minLoc = new Point();
                Point maxLoc = new Point();
                using (Mat diff1 = diff.Reshape(1))
                    CvInvoke.MinMaxLoc(diff1, ref minDiff, ref maxDiff, ref minLoc, ref maxLoc);
                EmguAssert.IsTrue(maxDiff < 1e-3, String.Format("Oklab round trip error {0}", maxDiff));
            }
        }

        [TestAttribute]
        public void TestImwriteNewFlagValues()
        {
            EmguAssert.AreEqual(260, (int)ImwriteFlags.TiffCompressionLevel);
            EmguAssert.AreEqual(515, (int)ImwriteFlags.AvifAlphaQuality);
            EmguAssert.AreEqual(1 << 27, (int)CalibType.DisableSchurComplement);
        }

        [TestAttribute]
        public void TestMatToByteArray()
        {
            Mat m = new Mat(new Size(320, 240), DepthType.Cv8U, 3);
            byte[] bytes = new byte[m.Total.ToInt32() * m.ElementSize];
            m.CopyTo(bytes);
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

#if UNSAFE_ALLOWED
                var span = m1.GetSpan<byte>();
                for (int i = 0; i < span.Length; i++)
                {
                    span[i] = (byte)((byte)255 - (byte)(span[i]));
                }
                EmguAssert.IsTrue(m1.Equals(m2));
#endif
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
            Mat kernel = new Mat(3, 3, DepthType.Cv32F, 1);
            kernel.SetTo(new float[]
            {
                0.0f, 1.0f, 0.0f, 
                1.0f, -4.0f, 1.0f,
                0.0f, 1.0f, 0.0f
            });
            /*
            float[,] k = { {0, 1, 0},
              {1, -4, 1},
              {0, 1, 0}};
            ConvolutionKernelF kernel = new ConvolutionKernelF(k);*/
            Mat convoluted = new Mat(image.Size, DepthType.Cv8U, 1);
            CvInvoke.Filter2D(image, convoluted, kernel, new Point(1, 1));

            Mat absDiff = new Mat();

            CvInvoke.AbsDiff(laplacian, convoluted, absDiff);
            int nonZeroPixelCount = CvInvoke.CountNonZero(absDiff);

            EmguAssert.IsTrue(nonZeroPixelCount == 0);

            //Emgu.CV.WinForms.ImageViewer.Show(absDiff);
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
    }
}
