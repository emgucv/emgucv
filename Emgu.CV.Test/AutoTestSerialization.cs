//----------------------------------------------------------------------------
//  Copyright (C) 2004-2026 by EMGU Corporation. All rights reserved.       
//----------------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;
using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Structure;
using Emgu.CV.Util;
using Emgu.Util;
using System.Linq;

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
    public class AutoTestSerialization
    {
        [Test]
        public void TestXmlSerialization()
        {
            MCvPoint2D64f pt2d = new MCvPoint2D64f(12.0, 5.5);

            XDocument xdoc = Toolbox.XmlSerialize<MCvPoint2D64f>(pt2d);
            //Trace.WriteLine(xdoc.OuterXml);
            pt2d = Toolbox.XmlDeserialize<MCvPoint2D64f>(xdoc);

            CircleF cir = new CircleF(new PointF(0.0f, 1.0f), 2.8f);
            xdoc = Toolbox.XmlSerialize<CircleF>(cir);
            //Trace.WriteLine(xdoc.OuterXml);
            cir = Toolbox.XmlDeserialize<CircleF>(xdoc);
            
            Mat img1 = EmguAssert.LoadMat("stuff.jpg");
            xdoc = Toolbox.XmlSerialize(img1);
            //Trace.WriteLine(xdoc.OuterXml);
            Mat img2 = Toolbox.XmlDeserialize<Mat>(xdoc);

            Byte[] a1 = img1.GetData(false) as byte[];
            Byte[] a2 = img2.GetData(false) as byte[];
            EmguAssert.AreEqual(a1.Length, a2.Length);
            for (int i = 0; i < a1.Length; i++)
            {
                EmguAssert.AreEqual(a1[i], a2[i]);
            }

            img1.Dispose();
            img2.Dispose();
        }

        [Test]
        public void TestXmlSerialize()
        {
            PointF p = new PointF(0.0f, 0.0f);
            XDocument xDoc = Toolbox.XmlSerialize<PointF>(p, new Type[] { typeof(Point) });
            PointF p2 = Toolbox.XmlDeserialize<PointF>(xDoc, new Type[] { typeof(Point) });
            EmguAssert.IsTrue(p.Equals(p2));


            Rectangle rect = new Rectangle(3, 4, 5, 3);
            XDocument xDoc2 = Toolbox.XmlSerialize<Rectangle>(rect);
            Rectangle rect2 = Toolbox.XmlDeserialize<Rectangle>(xDoc2);
            EmguAssert.IsTrue(rect.Equals(rect2));

        }

        [Test]
        public void TestFileStorage1()
        {
            FileStorage fs = new FileStorage(EmguAssert.GetFile("trained_classifierNM1.xml"), FileStorage.Mode.Read);
        }

        [Test]
        public void TestFileStorage2()
        {
            Mat m = new Mat(40, 30, DepthType.Cv8U, 3);

            using (ScalarArray lower = new ScalarArray(new MCvScalar(0, 0, 0)))
            using (ScalarArray higher = new ScalarArray(new MCvScalar(255, 255, 255)))
                CvInvoke.Randu(m, lower, higher);

            int intValue = 10;
            float floatValue = 213.993f;
            double doubleValue = 32.314;

            using (FileStorage fs = new FileStorage(".xml", FileStorage.Mode.Write | FileStorage.Mode.Memory))
            {
                fs.Write(m, "m");
                fs.Write(intValue, "int");
                fs.Write(floatValue, "float");
                fs.Write(doubleValue, "double");

                string s = fs.ReleaseAndGetString();

                using (FileStorage fs2 = new FileStorage(s, FileStorage.Mode.Read | FileStorage.Mode.Memory))
                {
                    using (FileNode root = fs2.GetRoot())
                        foreach (FileNode n in root)
                        {
                            //String[] keys = n.Keys;
                            String name = n.Name;
                            n.Dispose();
                        }

                    using (FileNode node = fs2.GetFirstTopLevelNode())
                    {
                        Mat m2 = new Mat();
                        node.ReadMat(m2);
                        EmguAssert.IsTrue(m.Equals(m2));
                    }

                    using (FileNode node = fs2.GetNode("m"))
                    {
                        Mat m2 = new Mat();
                        node.ReadMat(m2);
                        EmguAssert.IsTrue(m.Equals(m2));
                    }

                    using (FileNode node = fs2.GetNode("int"))
                    {
                        EmguAssert.IsTrue(intValue.Equals(node.ReadInt()));
                    }

                    using (FileNode node = fs2.GetNode("float"))
                    {
                        EmguAssert.IsTrue(floatValue.Equals(node.ReadFloat()));
                    }

                    using (FileNode node = fs2.GetNode("double"))
                    {
                        EmguAssert.IsTrue(doubleValue.Equals(node.ReadDouble()));
                    }
                }
            }
        }

#if !NETFX_CORE
        [Test]
        public void TestBinaryStorage()
        {
            //generate some randome points
            PointF[] pts = new PointF[120];
            GCHandle handle = GCHandle.Alloc(pts, GCHandleType.Pinned);
            using(Mat ptsMat = new Mat(pts.Length, 2, DepthType.Cv32F, 1, handle.AddrOfPinnedObject(), Marshal.SizeOf(typeof(float)) * 2))
            //using (Matrix<float> ptsMat = new Matrix<float>(pts.Length, 2, handle.AddrOfPinnedObject(), Marshal.SizeOf(typeof(float)) * 2))
            {
                CvInvoke.Randu(ptsMat, new MCvScalar(), new MCvScalar(100));
                //ptsMat.SetRandNormal(new MCvScalar(), new MCvScalar(100));
            }
            handle.Free();

            String fileName = Path.Combine(Path.GetTempPath(), "tmp.dat");
            Stopwatch watch = Stopwatch.StartNew();
            BinaryFileStorage<PointF> stor = new BinaryFileStorage<PointF>(fileName, pts);
            //BinaryFileStorage<PointF> stor = new BinaryFileStorage<PointF>("abc.data", pts);
            watch.Stop();
            EmguAssert.WriteLine(String.Format("Time for writing {0} points: {1} milliseconds", pts.Length, watch.ElapsedMilliseconds));
            int estimatedSize = stor.EstimateSize();

            //EmguAssert.IsTrue(pts.Length == estimatedSize);

            watch.Reset();
            watch.Start();
            PointF[] pts2 = stor.ToArray();
            watch.Stop();
            EmguAssert.WriteLine(String.Format("Time for reading {0} points: {1} milliseconds", pts.Length, watch.ElapsedMilliseconds));

            if (File.Exists(fileName))
                File.Delete(fileName);

            //EmguAssert.IsTrue(pts.Length == pts2.Length);

            //Check for equality
            for (int i = 0; i < pts.Length; i++)
            {
                //EmguAssert.IsTrue(pts[i] == pts2[i]);
            }
        }
#endif
    }
}
