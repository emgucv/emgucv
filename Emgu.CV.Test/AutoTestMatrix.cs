using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using Emgu.CV;
using Emgu.UI;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;
using System.Xml;
using System.IO;
using System.Runtime.Serialization;
using System.Threading;

namespace Emgu.CV.Test
{
    [TestFixture]
    public class AutoTestMatrix
    {
        [Test]
        public void Test_Not()
        {
            Matrix<byte> m = new Matrix<byte>(10, 8);
            m.SetValue(1.0);
            m._Not();
            byte[,] d2 = m.Data;

            foreach (byte v in d2)
                Assert.AreEqual(254.0, v);
        }
        
        /// <summary>
        /// Test the matrix constructor that accepts a two dimensional array as input
        /// </summary>
        [Test]
        public void Test_Data()
        {
            Byte[,] data = new Byte[20, 30];
            Random r = new Random();
            Byte[] bytes = new Byte[data.Length];
            r.NextBytes(bytes);
            for (int i = 0; i < data.GetLength(0); i++)
                for (int j = 0; j < data.GetLength(1); j++)
                    data[i, j] = bytes[i * data.GetLength(1)+ j];

            Matrix<Byte> m = new Matrix<byte>(data);
            Byte[,] data2 = m.Data;

            for (int i = 0; i < data.GetLength(0); i++)
                for (int j = 0; j < data.GetLength(1); j++)
                {
                    Assert.AreEqual(data[i, j], data2[i, j]);
                    Assert.AreEqual(data[i, j], m[i, j]);
                }
        }

        /// <summary>
        /// Test the matrix transpose function for matrix of Byte
        /// </summary>
        [Test]
        public void Test_TransposeByteMatrix()
        {
            using (Matrix<Byte> mat = new Matrix<Byte>(1, 10))
            {
                mat._RandUniform((ulong)DateTime.Now.Ticks, new MCvScalar(0.0), new MCvScalar(255.0));

                Matrix<Byte> matT = mat.Transpose();

                for (int i = 0; i < matT.Rows; i++)
                    for (int j = 0; j < matT.Cols; j++)
                        Assert.AreEqual(matT[i, j], mat[j, i]);
            }
        }

        [Test]
        public void Test_TransposeFloatMatrix()
        {
            using (Matrix<float> mat = new Matrix<float>(1, 3))
            {
                mat._RandUniform((ulong)DateTime.Now.Ticks, new MCvScalar(-1000.0), new MCvScalar(1000.0));
                
                Matrix<float> matT = mat.Transpose();

                for (int i = 0; i < matT.Rows; i++)
                    for (int j = 0; j < matT.Cols; j++)
                        Assert.AreEqual(matT[i, j], mat[j, i]);
            }
        }
        
        
       [Test]
        public void Test_XmlSerializeAndDeserialize()
        {
            using ( Matrix<Byte> mat = new Matrix<byte>(50, 60))
            {
                mat._RandUniform((ulong) DateTime.Now.Ticks, new MCvScalar(0), new MCvScalar(255));
                XmlDocument doc = Emgu.Utils.XmlSerialize<Matrix<Byte>>(mat);
                //Trace.WriteLine(doc.OuterXml);
                
                using (Matrix<Byte> mat2 = Emgu.Utils.XmlDeserialize<Matrix<Byte>>(doc))
                    Assert.IsTrue(mat.Equals(mat2));
                
            }
        }

        [Test]
        public void Test_StressTestMatrixGC()
        {
            int i = 0;
            //try
            {
                for (i = 0; i < 2000; i++)
                {
                    Matrix<Single> mat = new Matrix<float>(1000, 1000);
                    Thread.Sleep(1);
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
    }
}
