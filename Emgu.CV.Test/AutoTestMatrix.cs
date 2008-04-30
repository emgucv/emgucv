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
        
        
        [Test]
        public void Test_Data()
        {
            Random r = new Random();
            Byte[,] data = new Byte[20, 30];
            Byte[] bytes = new Byte[data.Length];
            r.NextBytes(bytes);
            for (int i = 0; i < data.GetLength(0); i++)
                for (int j = 0; j < data.GetLength(1); j++)
                    data[i, j] = bytes[i * data.GetLength(1)];

            Matrix<Byte> m = new Matrix<byte>(data);
            Byte[,] data2 = m.Data;

            for (int i = 0; i < data.GetLength(0); i++)
                for (int j = 0; j < data.GetLength(1); j++)
                {
                    Assert.AreEqual(data[i, j], data2[i, j]);
                    Assert.AreEqual(data[i, j], m[i, j]);
                }
        }

        
        [Test]
        public void Test_TransposeByteMatrix()
        {
            Byte[,] data =  { { 0x1, 0x2, 0x3, 0x4, 0x5 } };
            
            using (Matrix<Byte> mat = new Matrix<Byte>(data))
            {
                Byte[,] data2 = mat.Transpose().Data;

                for (int i = 0; i < data2.GetLength(0); i++)
                    for (int j = 0; j < data2.GetLength(1); j++)
                        Assert.AreEqual(data2[i,j], data[j,i]);
            }
        }

        
        [Test]
        public void Test_TransposeFloatMatrix()
        {
            float[,] data = { { 1.0F, 2.0F, 3.0F, 4.0F, 5.0F } };

            using (Matrix<float> mat = new Matrix<float>(data))
            {
                float[,] data2 = mat.Transpose().Data;

                for (int i = 0; i < data2.GetLength(0); i++)
                    for (int j = 0; j < data2.GetLength(1); j++)
                        Assert.AreEqual(data2[i, j], data[j, i]);
            }
        }
        
        /*
        [Test]
        public void Test_XmlSerialize()
        {
            using ( Matrix<Byte> mat = new Matrix<byte>(50, 60))
            {
                XmlDocument doc = Emgu.Utils.XmlSerialize<Matrix<Byte>>(mat);

                using (Matrix<Byte> mat2 = Emgu.Utils.XmlDeserialize<Matrix<Byte>>(doc))
                    Assert.IsTrue(mat.Equals(mat2));
            }
        }*/
    }
}
