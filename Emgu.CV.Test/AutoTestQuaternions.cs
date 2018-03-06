//----------------------------------------------------------------------------
//  Copyright (C) 2004-2018 by EMGU Corporation. All rights reserved.       
//----------------------------------------------------------------------------
﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using Emgu.CV;
using Emgu.CV.Features2D;
using Emgu.CV.Structure;
using Emgu.Util;
#if !__IOS__
using Emgu.CV.Tiff;
#endif

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
   public class AutoTestQuaternions
   {
      [Test]
      public void TestQuaternions1()
      {
         Quaternions q = new Quaternions();
         double epsilon = 1.0e-10;

         Matrix<double> point = new Matrix<double>(3, 1);
         point.SetRandNormal(new MCvScalar(), new MCvScalar(20));
         using (Matrix<double> pt1 = new Matrix<double>(3, 1))
         using (Matrix<double> pt2 = new Matrix<double>(3, 1))
         using (Matrix<double> pt3 = new Matrix<double>(3, 1))
         {
            double x1 = 1.0, y1 = 0.2, z1 = 0.1;
            double x2 = 0.0, y2 = 0.0, z2 = 0.0;

            q.SetEuler(x1, y1, z1);
            q.GetEuler(ref x2, ref y2, ref z2);

            EmguAssert.IsTrue(
               Math.Abs(x2 - x1) < epsilon &&
               Math.Abs(y2 - y1) < epsilon &&
               Math.Abs(z2 - z1) < epsilon);

            q.RotatePoints(point, pt1);

            Matrix<double> rMat = new Matrix<double>(3, 3);
            q.GetRotationMatrix(rMat);
            CvInvoke.Gemm(rMat, point, 1.0, null, 0.0, pt2, Emgu.CV.CvEnum.GemmType.Default);

            CvInvoke.AbsDiff(pt1, pt2, pt3);

            EmguAssert.IsTrue(
               pt3[0, 0] < epsilon &&
               pt3[1, 0] < epsilon &&
               pt3[2, 0] < epsilon);

         }

         double rotationAngle = 0.2;
         q.SetEuler(rotationAngle, 0.0, 0.0);
         EmguAssert.IsTrue(Math.Abs(q.RotationAngle - rotationAngle) < epsilon);
         q.SetEuler(0.0, rotationAngle, 0.0);
         EmguAssert.IsTrue(Math.Abs(q.RotationAngle - rotationAngle) < epsilon);
         q.SetEuler(0.0, 0.0, rotationAngle);
         EmguAssert.IsTrue(Math.Abs(q.RotationAngle - rotationAngle) < epsilon);

         q = q * q;
         EmguAssert.IsTrue(Math.Abs(q.RotationAngle / 2.0 - rotationAngle) < epsilon);

         q.SetEuler(0.2, 0.1, 0.05);
         double t = q.RotationAngle;
         q = q * q;
         EmguAssert.IsTrue(Math.Abs(q.RotationAngle / 2.0 - t) < epsilon);

      }

      public void TestQuaternionsMultiplicationPerformance()
      {
         Quaternions q = new Quaternions();
         Random r = new Random();
         q.SetEuler(r.NextDouble(), r.NextDouble(), r.NextDouble());

         Stopwatch watch = Stopwatch.StartNew();
         Quaternions sum = Quaternions.Empty;
         for (int i = 0; i < 1000000; i++)
         {
            sum *= q;
         }
         watch.Stop();
         EmguAssert.WriteLine(String.Format("Time used: {0} milliseconds", watch.ElapsedMilliseconds));

      }

      [Test]
      public void TestQuaternionEulerAngleAndRotate()
      {
         double epsilon = 1.0e-12;
         Random r = new Random();
         Quaternions q1 = new Quaternions();
         double roll1 = r.NextDouble(), pitch1 = r.NextDouble(), yaw1 = r.NextDouble();
         double roll2 = 0, pitch2 = 0, yaw2 = 0;
         q1.SetEuler(roll1, pitch1, yaw1);
         q1.GetEuler(ref roll2, ref pitch2, ref yaw2);
         EmguAssert.IsTrue(Math.Abs(roll1 - roll2) < epsilon);
         EmguAssert.IsTrue(Math.Abs(pitch1 - pitch2) < epsilon);
         EmguAssert.IsTrue(Math.Abs(yaw1 - yaw2) < epsilon);

         Quaternions q2 = new Quaternions();
         q2.SetEuler(r.NextDouble(), r.NextDouble(), r.NextDouble());

         MCvPoint3D64f p = new MCvPoint3D64f(r.NextDouble() * 10, r.NextDouble() * 10, r.NextDouble() * 10);

         MCvPoint3D64f delta = (q1 * q2).RotatePoint(p) - q1.RotatePoint(q2.RotatePoint(p));

         EmguAssert.IsTrue(delta.X < epsilon);
         EmguAssert.IsTrue(delta.Y < epsilon);
         EmguAssert.IsTrue(delta.Z < epsilon);

      }

      [Test]
      public void TestQuaternion3()
      {
         Random r = new Random();
         Quaternions q1 = new Quaternions();
         q1.AxisAngle = new MCvPoint3D64f(r.NextDouble(), r.NextDouble(), r.NextDouble());

         Quaternions q2 = new Quaternions();
         q2.AxisAngle = q1.AxisAngle;

         double epsilon = 1.0e-8;
         EmguAssert.IsTrue(Math.Abs(q1.W - q2.W) < epsilon);
         EmguAssert.IsTrue(Math.Abs(q1.X - q2.X) < epsilon);
         EmguAssert.IsTrue(Math.Abs(q1.Y - q2.Y) < epsilon);
         EmguAssert.IsTrue(Math.Abs(q1.Z - q2.Z) < epsilon);

         RotationVector3D rVec = new RotationVector3D(new double[] { q1.AxisAngle.X, q1.AxisAngle.Y, q1.AxisAngle.Z });
         Mat m1 = rVec.RotationMatrix;
         Matrix<double> m2 = new Matrix<double>(3, 3);
         q1.GetRotationMatrix(m2);
         Matrix<double> diff = new Matrix<double>(3, 3);
         CvInvoke.AbsDiff(m1, m2, diff);
         double norm = CvInvoke.Norm(diff, Emgu.CV.CvEnum.NormType.C);
         EmguAssert.IsTrue(norm < epsilon);

         Quaternions q4 = q1 * Quaternions.Empty;
         //EmguAssert.IsTrue(q4.Equals(q1));
      }

      [Test]
      public void TestAxisAngleCompose()
      {
         MCvPoint3D64f angle1 = new MCvPoint3D64f(4.1652539565753417e-022, -9.4229054916424228e-022, 5.1619136559035708e-008);
         MCvPoint3D64f angle2 = new MCvPoint3D64f(4.3209729769679014e-023, 3.2042397847543764e-023, -6.4083339340765912e-008);
         Quaternions q1 = new Quaternions();
         q1.AxisAngle = angle1;
         Quaternions q2 = new Quaternions();
         q2.AxisAngle = angle2;
         Quaternions q = q1 * q2;

         MCvPoint3D64f angle = q.AxisAngle;
         EmguAssert.AreNotEqual(double.NaN, angle.X, "Invalid value x");
         EmguAssert.AreNotEqual(double.NaN, angle.Y, "Invalid value y");
         EmguAssert.AreNotEqual(double.NaN, angle.Z, "Invalid value z");
      }

      [Test]
      public void TestQuaternionsSize()
      {
#if NETFX_CORE
         EmguAssert.AreEqual(4 * Marshal.SizeOf<double>(), Marshal.SizeOf<Quaternions>());
#else
         EmguAssert.AreEqual(4 * Marshal.SizeOf(typeof(double)), Marshal.SizeOf(typeof(Quaternions)));
#endif
      }

      [Test]
      public void TestQuaternionsSlerp1()
      {
         Random r = new Random();
         Quaternions q1 = new Quaternions();
         q1.AxisAngle = new MCvPoint3D64f(r.NextDouble(), r.NextDouble(), r.NextDouble());
         Quaternions q2 = new Quaternions();
         q2.AxisAngle = new MCvPoint3D64f(r.NextDouble(), r.NextDouble(), r.NextDouble());

         double epsilon = 1.0e-12;

         Quaternions q = q1.Slerp(q2, 0.0);
         EmguAssert.IsTrue(Math.Abs(q1.W - q.W) < epsilon);
         EmguAssert.IsTrue(Math.Abs(q1.X - q.X) < epsilon);
         EmguAssert.IsTrue(Math.Abs(q1.Y - q.Y) < epsilon);
         EmguAssert.IsTrue(Math.Abs(q1.Z - q.Z) < epsilon);

         q = q1.Slerp(q2, 1.0);
         EmguAssert.IsTrue(Math.Abs(q2.W - q.W) < epsilon);
         EmguAssert.IsTrue(Math.Abs(q2.X - q.X) < epsilon);
         EmguAssert.IsTrue(Math.Abs(q2.Y - q.Y) < epsilon);
         EmguAssert.IsTrue(Math.Abs(q2.Z - q.Z) < epsilon);

      }

      [Test]
      public void TestQuaternionsSlerp2()
      {
         Random r = new Random();
         Quaternions q1 = new Quaternions();
         q1.AxisAngle = new MCvPoint3D64f(30.0 / 180 * Math.PI, 0.0, 0.0);
         Quaternions q2 = new Quaternions();
         q2.AxisAngle = new MCvPoint3D64f(40.0 / 180 * Math.PI, 0.0, 0.0);

         double epsilon = 1.0e-12;
         double x = 0, y = 0, z = 0;

         Quaternions q = q1.Slerp(q2, 0.5);
         q.GetEuler(ref x, ref y, ref z);
         double deltaDegree = Math.Abs(x / Math.PI * 180.0 - 35.0);
         EmguAssert.IsTrue(deltaDegree <= epsilon);

         q = q1.Slerp(q2, 0.8);
         q.GetEuler(ref x, ref y, ref z);
         deltaDegree = Math.Abs(x / Math.PI * 180.0 - 38.0);
         EmguAssert.IsTrue(deltaDegree <= epsilon);

         q = q1.Slerp(q2, 0.15);
         q.GetEuler(ref x, ref y, ref z);
         deltaDegree = Math.Abs(x / Math.PI * 180.0 - 31.5);
         EmguAssert.IsTrue(deltaDegree <= epsilon);
      }

      [Test]
      public void TestQuaternionsSlerp3()
      {
         Random r = new Random();
         Quaternions q1 = new Quaternions();
         q1.AxisAngle = new MCvPoint3D64f(0.0, 30.0 / 180 * Math.PI, 0.0);
         Quaternions q2 = new Quaternions();
         q2.AxisAngle = new MCvPoint3D64f(0.0, 40.0 / 180 * Math.PI, 0.0);

         double epsilon = 1.0e-12;
         double x = 0, y = 0, z = 0;

         Quaternions q = q1.Slerp(q2, 0.5);
         q.GetEuler(ref x, ref y, ref z);
         double deltaDegree = Math.Abs(y / Math.PI * 180.0 - 35.0);
         EmguAssert.IsTrue(deltaDegree <= epsilon);

         q = q1.Slerp(q2, 0.8);
         q.GetEuler(ref x, ref y, ref z);
         deltaDegree = Math.Abs(y / Math.PI * 180.0 - 38.0);
         EmguAssert.IsTrue(deltaDegree <= epsilon);

         q = q1.Slerp(q2, 0.15);
         q.GetEuler(ref x, ref y, ref z);
         deltaDegree = Math.Abs(y / Math.PI * 180.0 - 31.5);
         EmguAssert.IsTrue(deltaDegree <= epsilon);
      }

      [Test]
      public void TestQuaternionsSlerp4()
      {
         Random r = new Random();
         Quaternions q1 = new Quaternions();
         q1.AxisAngle = new MCvPoint3D64f(0.0, 175.0 / 180 * Math.PI, 0.0);
         Quaternions q2 = new Quaternions();
         q2.AxisAngle = new MCvPoint3D64f(0.0, 5.0 / 180 * Math.PI, 0.0);

         double epsilon = 1.0e-12;
         double x = 0, y = 0, z = 0;

         Quaternions q = q1.Slerp(q2, 0.5);
         q.GetEuler(ref x, ref y, ref z);
         EmguAssert.IsFalse(double.IsNaN(x));
         EmguAssert.IsFalse(double.IsNaN(y));
         EmguAssert.IsFalse(double.IsNaN(z));
         double deltaDegree = Math.Abs(y / Math.PI * 180.0 - 90.0);
         EmguAssert.IsTrue(deltaDegree <= epsilon);
      }

      [Test]
      public void TestQuaternionsSlerp5()
      {
         Random r = new Random();
         Quaternions q1 = new Quaternions();
         q1.AxisAngle = new MCvPoint3D64f(0.0, 355.0 / 180 * Math.PI, 0.0);
         Quaternions q2 = new Quaternions();
         q2.AxisAngle = new MCvPoint3D64f(0.0, 5.0 / 180 * Math.PI, 0.0);

         double epsilon = 1.0e-12;
         double x = 0, y = 0, z = 0;

         Quaternions q = q1.Slerp(q2, 0.5);
         q.GetEuler(ref x, ref y, ref z);
         EmguAssert.IsFalse(double.IsNaN(x));
         EmguAssert.IsFalse(double.IsNaN(y));
         EmguAssert.IsFalse(double.IsNaN(z));
         double deltaDegree = Math.Abs(y / Math.PI * 180.0 - 0.0);
         EmguAssert.IsTrue(deltaDegree <= epsilon);

         q = q2.Slerp(q1, 0.5);
         q.GetEuler(ref x, ref y, ref z);
         EmguAssert.IsFalse(double.IsNaN(x));
         EmguAssert.IsFalse(double.IsNaN(y));
         EmguAssert.IsFalse(double.IsNaN(z));
         deltaDegree = Math.Abs(y / Math.PI * 180.0 - 0.0);
         EmguAssert.IsTrue(deltaDegree <= epsilon);
      }
   }
}