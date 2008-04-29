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
            byte[][] d2 = m.Data;
            foreach (byte[] row in d2)
                foreach (byte v in row)
                    Assert.AreEqual(254.0, v);
        }
        
        /*
        [Test]
        public void Test_Data()
        {
            Random r = new Random();
            Byte[,] data = new Byte[20,30];
            for (int i = 0; i < data.Length; i++)
            {
                data[i] = new Byte[30];
                r.NextBytes(data);
            }
           

            Matrix<Byte> m = new Matrix<byte>(data);
            Byte[][] data2 = m.Data;

            for (int i = 0; i < data.Length; i++)
                for (int j = 0; j < data[i].Length; j++)
                    Assert.AreEqual(data[i][j], data2[i][j]);

        }

        [Test]
        public void Test_ByteConstructor()
        {
            Byte[][] data = new Byte[1][] { new Byte[5] { 0x1, 0x2, 0x3, 0x4, 0x5 } };
            data = new Byte[3][] { new Byte[data[0].Length], data[0], new Byte[data[0].Length] };

            using (Matrix<Byte> mat = new Matrix<Byte>(data))
            {
                Byte[][] data2 = mat.Transpose().Data;

                for (int i = 0; i < data2.Length; i++)
                {
                    for (int j = 0; j < data2[i].Length; j++)
                        if (j == 1) Assert.AreEqual(i + 1, data2[i][j]);
                        else Assert.AreEqual(0, data2[i][j]);
                }
            }
        }

        [Test]
        public void Test_FloatConstructor()
        {
            float[][] data = new float[1][] { new float[5] { 1.0F, 2.0F, 3.0F, 4.0F, 5.0F } };
            data = new float[3][] { new float[data[0].Length], data[0], new float[data[0].Length] };

            using (Matrix<float> mat = new Matrix<float>(data))
            {
                float[][] data2 = mat.Transpose().Data;

                for (int i = 0; i < data2.Length; i++)
                {
                    for (int j = 0; j < data2[i].Length; j++)
                        if (j == 1) Assert.AreEqual(i + 1, data2[i][j]);
                        else Assert.AreEqual(0.0, data2[i][j]);
                }
            }
        }*/
    }
}
