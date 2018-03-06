//----------------------------------------------------------------------------
//  Copyright (C) 2004-2018 by EMGU Corporation. All rights reserved.       
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
using Emgu.CV.Structure;
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
    public class AutoTestMatrix
    {
        [Test]
        public void TestInvert()
        {
            Matrix<Single> m = new Matrix<Single>(3, 3);
            Matrix<Single> mInvert = new Matrix<Single>(3, 3);

            m.SetIdentity();

            CvInvoke.Invert(m, mInvert, Emgu.CV.CvEnum.DecompMethod.LU);

            EmguAssert.IsTrue(m.Equals(mInvert));
        }

        [Test]
        public void TestGetData()
        {
            float[,] data = new float[2, 3];
            data[0, 0] = 1; data[0, 1] = 2; data[0, 2] = 3;
            data[1, 0] = 4; data[1, 1] = 5; data[1, 2] = 6;
            GCHandle dataHandle = GCHandle.Alloc(data, GCHandleType.Pinned);
            using (Matrix<float> m = new Matrix<float>(data.GetLength(0), data.GetLength(1), dataHandle.AddrOfPinnedObject()))
            {
                float[,] data2 = m.Data;
                for (int i = 0; i < data2.GetLength(0); i++)
                    for (int j = 0; j < data.GetLength(1); j++)
                        EmguAssert.IsTrue(data2[i, j] == data[i, j]);
            }

            dataHandle.Free();
        }


        [Test]
        public void TestSolve()
        {
            Matrix<Single> lhs = new Matrix<Single>(3, 3);
            lhs.SetIdentity();
            Matrix<Single> rhs = new Matrix<Single>(new float[,] { { 0.1f }, { 0.2f }, { 0.5f } });
            Matrix<Single> result = new Matrix<float>(3, 1);
            CvInvoke.Solve(lhs, rhs, result, CvEnum.DecompMethod.LU);

            EmguAssert.AreEqual(rhs[0, 0], result[0, 0]);
            EmguAssert.AreEqual(rhs[1, 0], result[1, 0]);
            EmguAssert.AreEqual(rhs[2, 0], result[2, 0]);
        }

        [Test]
        public void TestNot()
        {
            Matrix<byte> m = new Matrix<byte>(10, 8);
            m.SetValue(1.0);
            m._Not();
            byte[,] d2 = m.Data;

            foreach (byte v in d2)
                EmguAssert.IsTrue(254.0 == v);
        }

        [Test]
        public void TestArithmatic()
        {
            Matrix<byte> m = new Matrix<byte>(10, 8);
            m.SetRandNormal(new MCvScalar(), new MCvScalar(30));
            Matrix<byte> mMultiplied = m.Mul(2.0);

            for (int i = 0; i < m.Rows; i++)
                for (int j = 0; j < m.Cols; j++)
                    EmguAssert.IsTrue(m[i, j] * 2 == mMultiplied[i, j]);
        }

        [Test]
        public void TestCvInvoke()
        {
            IntPtr mat = CvInvoke.cvCreateMat(10, 10, Emgu.CV.CvEnum.DepthType.Cv32F);
            CvInvoke.cvReleaseMat(ref mat);
            mat = CvInvoke.cvCreateMat(10, 10, Emgu.CV.CvEnum.DepthType.Cv32S);
            CvInvoke.cvReleaseMat(ref mat);
        }

        /// <summary>
        /// Test the matrix constructor that accepts a two dimensional array as input
        /// </summary>
        [Test]
        public void TestData()
        {
            Byte[,] data = new Byte[20, 30];
            Random r = new Random();
            Byte[] bytes = new Byte[data.Length];
            r.NextBytes(bytes);
            for (int i = 0; i < data.GetLength(0); i++)
                for (int j = 0; j < data.GetLength(1); j++)
                    data[i, j] = bytes[i * data.GetLength(1) + j];

            Matrix<Byte> m = new Matrix<byte>(data);
            Byte[,] data2 = m.Data;

            for (int i = 0; i < data.GetLength(0); i++)
                for (int j = 0; j < data.GetLength(1); j++)
                {
                    EmguAssert.AreEqual(data[i, j], data2[i, j]);
                    EmguAssert.AreEqual(data[i, j], m[i, j]);
                }
        }

        /// <summary>
        /// Test the matrix transpose function for matrix of Byte
        /// </summary>
        [Test]
        public void TestTransposeByteMatrix()
        {
            using (Matrix<Byte> mat = new Matrix<Byte>(1, 10))
            {
                mat.SetRandUniform(new MCvScalar(0.0), new MCvScalar(255.0));

                Matrix<Byte> matT = mat.Transpose();

                for (int i = 0; i < matT.Rows; i++)
                    for (int j = 0; j < matT.Cols; j++)
                        EmguAssert.AreEqual(matT[i, j], mat[j, i]);
            }
        }

        [Test]
        public void TestTransposeFloatMatrix()
        {
            using (Matrix<float> mat = new Matrix<float>(1, 3))
            {
                mat.SetRandUniform(new MCvScalar(-1000.0), new MCvScalar(1000.0));

                Matrix<float> matT = mat.Transpose();

                for (int i = 0; i < matT.Rows; i++)
                    for (int j = 0; j < matT.Cols; j++)
                        EmguAssert.AreEqual(matT[i, j], mat[j, i]);
            }
        }

        [Test]
        public void TestGetDiagColRow()
        {
            Matrix<double> m = new Matrix<double>(new double[,] { { 1, 2 }, { 3, 4 } });
            Matrix<double> diag = m.GetDiag();
            EmguAssert.IsTrue(diag[0, 0] == 1);
            EmguAssert.IsTrue(diag[1, 0] == 4);
            EmguAssert.IsTrue(diag.Sum == m.Trace.V0);

            Matrix<double> col1 = m.GetCol(1);
            EmguAssert.IsTrue(col1[0, 0] == 2);
            EmguAssert.IsTrue(col1[1, 0] == 4);
            EmguAssert.IsTrue(col1.Sum == 2 + 4);

            Matrix<double> row1 = m.GetRow(1);
            EmguAssert.IsTrue(row1[0, 0] == 3);
            EmguAssert.IsTrue(row1[0, 1] == 4);
            EmguAssert.IsTrue(row1.Sum == 3 + 4);
        }

        [Test]
        public void TestXmlSerializeAndDeserialize()
        {
            using (Matrix<Byte> mat = new Matrix<byte>(50, 60))
            {
                mat.SetRandUniform(new MCvScalar(0), new MCvScalar(255));

#if !WINDOWS_PHONE_APP
                XDocument doc = Toolbox.XmlSerialize<Matrix<Byte>>(mat);
                //Trace.WriteLine(doc.OuterXml);

                using (Matrix<Byte> mat2 = Toolbox.XmlDeserialize<Matrix<Byte>>(doc))
                    EmguAssert.IsTrue(mat.Equals(mat2));
#endif
            }
        }

#if !NETFX_CORE
        [Test]
        public void TestRuntimeSerialize1()
        {
            Matrix<Byte> mat = new Matrix<Byte>(100, 80, 2);
            mat.SetRandNormal(new MCvScalar(100, 100, 100), new MCvScalar(50, 50, 50));

            System.Runtime.Serialization.Formatters.Binary.BinaryFormatter
                formatter = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();

            Byte[] bytes;
            using (MemoryStream ms = new MemoryStream())
            {
                formatter.Serialize(ms, mat);
                bytes = ms.GetBuffer();
            }
            using (MemoryStream ms2 = new MemoryStream(bytes))
            {
                Matrix<Byte> mat2 = (Matrix<Byte>)formatter.Deserialize(ms2);
                EmguAssert.IsTrue(mat.Equals(mat2));
            }
        }

        [Test]
        public void TestRuntimeSerialize2()
        {
            Random r = new Random();
            double[,,] data = new double[100, 80, 2];
            for (int i = 0; i < data.GetLength(0); i++)
                for (int j = 0; j < data.GetLength(1); j++)
                    for (int k = 0; k < data.GetLength(2); k++)
                        data[i, j, k] = r.NextDouble();

            GCHandle handle = GCHandle.Alloc(data, GCHandleType.Pinned);

            Matrix<Double> mat = new Matrix<Double>(data.GetLength(0), data.GetLength(1), data.GetLength(2), handle.AddrOfPinnedObject(), sizeof(double) * data.GetLength(1) * data.GetLength(2));

            System.Runtime.Serialization.Formatters.Binary.BinaryFormatter
                formatter = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();

            Byte[] bytes;
            using (MemoryStream ms = new MemoryStream())
            {
                formatter.Serialize(ms, mat);
                bytes = ms.GetBuffer();
            }
            using (MemoryStream ms2 = new MemoryStream(bytes))
            {
                Matrix<Double> mat2 = (Matrix<double>)formatter.Deserialize(ms2);
                EmguAssert.IsTrue(mat.Equals(mat2));
            }
            handle.Free();
        }

        [Test]
        public void TestRuntimeSerialize3()
        {
            MCvPoint3D32f[] data = new MCvPoint3D32f[] { new MCvPoint3D32f() };

            GCHandle handle = GCHandle.Alloc(data, GCHandleType.Pinned);

            Matrix<float> mat = new Matrix<float>(data.GetLength(0), 1, 3, handle.AddrOfPinnedObject(), sizeof(float) * 3);

            System.Runtime.Serialization.Formatters.Binary.BinaryFormatter
                formatter = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();

            Byte[] bytes;
            using (MemoryStream ms = new MemoryStream())
            {
                formatter.Serialize(ms, mat);
                bytes = ms.GetBuffer();
            }
            using (MemoryStream ms2 = new MemoryStream(bytes))
            {
                Matrix<float> mat2 = (Matrix<float>)formatter.Deserialize(ms2);
                EmguAssert.IsTrue(mat.Equals(mat2));
            }
            handle.Free();
        }
#endif

#if !WINDOWS_PHONE_APP //bad garbage collector in Windows phone results in failed test?
        [Test]
        public void TestStressTestMatrixGC()
        {
            int i = 0;
            //try
            {
                for (i = 0; i < 500; i++)
                {
                    Matrix<Single> mat = new Matrix<float>(500, 500);
                    //Thread.Sleep(5);
                }
            }
            //catch (Exception)
            {
            }
            //finally
            {
                //Trace.WriteLine(i);
            }
        }
#endif

        [Test]
        public void TestSubMatrix()
        {
            Matrix<float> mat = new Matrix<float>(30, 40);
            mat.SetRandUniform(new MCvScalar(0), new MCvScalar(255));
            Matrix<float> submat = mat.GetSubRect(new Rectangle(5, 5, 15, 15));
            for (int i = 0; i < 15; i++)
                for (int j = 0; j < 15; j++)
                    EmguAssert.AreEqual(mat[i + 5, j + 5], submat[i, j]);

            Matrix<float> secondRow = mat.GetRow(1);
            for (int i = 0; i < mat.Cols; i++)
            {
                EmguAssert.AreEqual(mat[1, i], secondRow[0, i]);
            }

            Matrix<float> thirdCol = mat.GetCol(2);
            for (int i = 0; i < mat.Rows; i++)
            {
                EmguAssert.AreEqual(mat[i, 2], thirdCol[i, 0]);
            }

            Matrix<float> diagonal = mat.GetDiag();
            for (int i = 0; i < Math.Min(mat.Rows, mat.Cols); i++)
            {
                EmguAssert.AreEqual(diagonal[i, 0], mat[i, i]);
            }
        }

        [Test]
        public void TestMinMax()
        {
            Matrix<float> mat = new Matrix<float>(30, 40);
            mat.SetRandUniform(new MCvScalar(0), new MCvScalar(255));
            double min, max;
            Point minLoc, maxLoc;
            mat.MinMax(out min, out max, out minLoc, out maxLoc, null);
        }

        [Test]
        public void TestMinMax2()
        {
            Matrix<Single> matrix = new Matrix<Single>(10, 10);
            matrix.SetValue(5);
            matrix[5, 5] = 10;
            matrix[3, 3] = 0;

            double minVal = 5;
            double maxVal = 5;
            Point minLoc = new Point();
            Point maxLoc = new Point();

            matrix.MinMax(out minVal, out maxVal, out minLoc, out maxLoc);
            EmguAssert.IsTrue(minVal == 0);
            EmguAssert.IsTrue(maxVal == 10);
            EmguAssert.IsTrue(minLoc.Equals(new Point(3, 3)));
            EmguAssert.IsTrue(maxLoc.Equals(new Point(5, 5)));
        }

        [Test]
        public void TestConcate()
        {
            Matrix<float> mat = new Matrix<float>(30, 40);
            mat.SetRandUniform(new MCvScalar(0), new MCvScalar(255));

            Matrix<float> m1 = mat.GetSubRect(new Rectangle(0, 0, mat.Cols, 20));
            Matrix<float> m2 = mat.GetSubRect(new Rectangle(0, 20, mat.Cols, mat.Rows - 20));
            Matrix<float> mat2 = m1.ConcateVertical(m2);
            EmguAssert.IsTrue(mat.Equals(mat2));

            Matrix<float> m3 = mat.GetSubRect(new Rectangle(0, 0, 10, mat.Rows));
            Matrix<float> m4 = mat.GetSubRect(new Rectangle(10, 0, mat.Cols - 10, mat.Rows));
            Matrix<float> mat3 = m3.ConcateHorizontal(m4);
            EmguAssert.IsTrue(mat.Equals(mat3));

            Matrix<float> m5 = mat.GetRows(0, 5, 1);
            Matrix<float> m6 = mat.GetRows(5, 6, 1);
            Matrix<float> m7 = mat.GetRows(6, mat.Rows, 1);
            EmguAssert.IsTrue(mat.RemoveRows(5, 6).Equals(m5.ConcateVertical(m7)));
            EmguAssert.IsTrue(mat.RemoveRows(0, 1).Equals(mat.GetRows(1, mat.Rows, 1)));
            EmguAssert.IsTrue(mat.RemoveRows(mat.Rows - 1, mat.Rows).Equals(mat.GetRows(0, mat.Rows - 1, 1)));
        }

        [Test]
        public void TestMultiChannelMatrix()
        {
            Matrix<float> m = new Matrix<float>(10, 20, 2);
            m.SetRandUniform(new MCvScalar(), new MCvScalar(255, 255));
            EmguAssert.AreEqual(10, m.Rows);
            EmguAssert.AreEqual(20, m.Cols);
            EmguAssert.AreEqual(2, m.NumberOfChannels);

#if !WINDOWS_PHONE_APP
            XDocument xDoc = Toolbox.XmlSerialize<Matrix<float>>(m);
            Matrix<float> m2 = Toolbox.XmlDeserialize<Matrix<float>>(xDoc);
            EmguAssert.IsTrue(m.Equals(m2));
#endif
        }

        [Test]
        public void TestEigenVV()
        {
            int size = 3;
            Matrix<float> tmp = new Matrix<float>(size, size);
            tmp.SetRandNormal(new MCvScalar(0), new MCvScalar(1));
            Matrix<float> symMat = new Matrix<float>(tmp.Size);
            CvInvoke.MulTransposed(tmp, symMat, false, null, 1.0, CvEnum.DepthType.Cv32S);
            Matrix<float> clone = symMat.Clone();

            Matrix<float> evects = new Matrix<float>(symMat.Size);
            Matrix<float> evals = new Matrix<float>(symMat.Rows, 1);
            CvInvoke.Eigen(symMat, evals, evects);
        }

        [Test]
        public void TestSparseMatrix()
        {
            int[] dimension = new int[2];
            dimension[0] = 1000000;
            dimension[1] = 1000000;
            //without sparase matrix, a matrix of this size is almost impossible to create in memory 

            using (SparseMatrix<double> m1 = new SparseMatrix<double>(dimension))
            {
                m1[3, 10009] = 2.0;
                EmguAssert.AreEqual(2.0, m1[3, 10009]);
            }
        }

        [Test]
        public void TestMatrixSubtract()
        {
            Matrix<float> f = new Matrix<float>(600, 480);
            //set the value to 300
            f.SetValue(new MCvScalar(300));            
            f -= 10;
            using (ScalarArray sa = new ScalarArray(290))
            using (Mat absDiff = new Mat())
            {
                //calculate the different of the value in f mat with 290
                CvInvoke.AbsDiff(f, sa, absDiff);
                int nonZeroCount = CvInvoke.CountNonZero(absDiff);
                //Console.WriteLine(String.Format("number of elements that is not 290: {0}", nonZeroCount));
            }
        }

        /*
        [Test]
        public void TestDataContractSerializer()
        {
            DataContractSerializer serializer = new DataContractSerializer(typeof(Image<Bgr, float>));
            Image<Bgr, float> img1 = new Image<Bgr,float>(5, 3);

            Byte[] bytes = null;
            using (MemoryStream ms = new MemoryStream())
            {
                serializer.WriteObject(ms, img1);
                bytes = ms.ToArray();
            }
            Image<Bgr, float> img2;
            DataContractSerializer deserializer = new DataContractSerializer(typeof(Image<Bgr, float>));
            using (MemoryStream ms = new MemoryStream(bytes))
            {
                img2 = deserializer.ReadObject(ms) as Image<Bgr, float>;
            }
            Debug.Assert(img1.Equals(img2));
        }*/
    }
}
