using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;
using System.Text;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Serialization;
using Emgu.CV;
using Emgu.CV.Features2D;
using Emgu.CV.Structure;
using Emgu.CV.UI;
using Emgu.CV.VideoSurveillance;
using Emgu.UI;
using Emgu.Util;
using Emgu.CV.Tiff;
using NUnit.Framework;

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

            Assert.IsTrue(
               Math.Abs(x2 - x1) < epsilon &&
               Math.Abs(y2 - y1) < epsilon &&
               Math.Abs(z2 - z1) < epsilon);

            q.RotatePoints(point, pt1);

            Matrix<double> rMat = new Matrix<double>(3, 3);
            q.GetRotationMatrix(rMat);
            CvInvoke.cvGEMM(rMat, point, 1.0, IntPtr.Zero, 0.0, pt2, Emgu.CV.CvEnum.GEMM_TYPE.CV_GEMM_DEFAULT);

            CvInvoke.cvAbsDiff(pt1, pt2, pt3);

            Assert.IsTrue(
               pt3[0, 0] < epsilon &&
               pt3[1, 0] < epsilon &&
               pt3[2, 0] < epsilon);

         }

         double rotationAngle = 0.2;
         q.SetEuler(rotationAngle, 0.0, 0.0);
         Assert.IsTrue(Math.Abs(q.RotationAngle - rotationAngle) < epsilon);
         q.SetEuler(0.0, rotationAngle, 0.0);
         Assert.IsTrue(Math.Abs(q.RotationAngle - rotationAngle) < epsilon);
         q.SetEuler(0.0, 0.0, rotationAngle);
         Assert.IsTrue(Math.Abs(q.RotationAngle - rotationAngle) < epsilon);

         q = q * q;
         Assert.IsTrue(Math.Abs(q.RotationAngle / 2.0 - rotationAngle) < epsilon);

         q.SetEuler(0.2, 0.1, 0.05);
         double t = q.RotationAngle;
         q = q * q;
         Assert.IsTrue(Math.Abs(q.RotationAngle / 2.0 - t) < epsilon);

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
         Trace.WriteLine(String.Format("Time used: {0} milliseconds", watch.ElapsedMilliseconds));

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
         Assert.AreEqual(roll1, roll2, epsilon);
         Assert.AreEqual(pitch1, pitch2, epsilon);
         Assert.AreEqual(yaw1, yaw2, epsilon);

         Quaternions q2 = new Quaternions();
         q2.SetEuler(r.NextDouble(), r.NextDouble(), r.NextDouble());

         MCvPoint3D64f p = new MCvPoint3D64f(r.NextDouble() * 10, r.NextDouble() * 10, r.NextDouble() * 10);

         MCvPoint3D64f delta = (q1 * q2).RotatePoint(p) - q1.RotatePoint(q2.RotatePoint(p));

         Assert.Less(delta.x, epsilon);
         Assert.Less(delta.y, epsilon);
         Assert.Less(delta.z, epsilon);

      }

      [Test]
      public void TestQuaternion3()
      {
         Random r = new Random();
         Quaternions q1 = new Quaternions();
         q1.AxisAngle = new MCvPoint3D64f(r.NextDouble(), r.NextDouble(), r.NextDouble());

         Quaternions q2 = new Quaternions();
         q2.AxisAngle = q1.AxisAngle;

         double epsilon = 1.0e-12;
         Assert.Less(Math.Abs(q1.W - q2.W), epsilon);
         Assert.Less(Math.Abs(q1.X - q2.X), epsilon);
         Assert.Less(Math.Abs(q1.Y - q2.Y), epsilon);
         Assert.Less(Math.Abs(q1.Z - q2.Z), epsilon);

         RotationVector3D rVec = new RotationVector3D(new double[] { q1.AxisAngle.x, q1.AxisAngle.y, q1.AxisAngle.z });
         Matrix<double> m1 = rVec.RotationMatrix;
         Matrix<double> m2 = new Matrix<double>(3, 3);
         q1.GetRotationMatrix(m2);
         Matrix<double> diff = new Matrix<double>(3, 3);
         CvInvoke.cvAbsDiff(m1, m2, diff);
         double norm = CvInvoke.cvNorm(diff, IntPtr.Zero, Emgu.CV.CvEnum.NORM_TYPE.CV_C, IntPtr.Zero);
         Assert.Less(norm, epsilon);

         Quaternions q4 = q1 * Quaternions.Empty;
         Assert.AreEqual(q4, q1);
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
         Assert.AreNotEqual(double.NaN, angle.x, "Invalid value x");
         Assert.AreNotEqual(double.NaN, angle.y, "Invalid value y");
         Assert.AreNotEqual(double.NaN, angle.z, "Invalid value z");
      }

      [Test]
      public void TestQuaternionsSize()
      {
         Assert.AreEqual(4 * Marshal.SizeOf(typeof(double)), Marshal.SizeOf(typeof(Quaternions)));
      }
   }
}