using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;
using Emgu.CV.Structure;

namespace Emgu.CV
{
   /// <summary>
   /// A unit quaternions that defines rotation in 3D
   /// </summary>
   [Serializable]
   //TODO: Check if mono has System.Windows.Media.Media3D.Quaternion implemented and if it comes as a package on Ubuntu and fedora
   public struct Quaternions : IEquatable<Quaternions>
   {
      private double _w;
      private double _x;
      private double _y;
      private double _z;

      /// <summary>
      /// The W component of the quaternion.
      /// </summary>
      public double W
      {
         get { return _w; }
         set { _w = value; }
      }

      /// <summary>
      /// The X component of the quaternion.
      /// </summary>
      public double X
      {
         get { return _x; }
         set { _x = value; }
      }

      /// <summary>
      /// The Y component of the quaternion.
      /// </summary>
      public double Y
      {
         get { return _y; }
         set { _y = value; }
      }

      /// <summary>
      /// The Z component of the quaternion.
      /// </summary>
      public double Z
      {
         get { return _z; }
         set { _z = value; }
      }

      [DllImport(CvInvoke.EXTERN_LIBRARY)]
      private extern static void eulerToQuaternions(double x, double y, double z, ref Quaternions q);

      [DllImport(CvInvoke.EXTERN_LIBRARY)]
      private extern static void quaternionsToEuler(ref Quaternions q, ref double x, ref double y, ref double z);

      [DllImport(CvInvoke.EXTERN_LIBRARY)]
      private extern static void axisAngleToQuaternions(ref MCvPoint3D64f axisAngle, ref Quaternions q);

      [DllImport(CvInvoke.EXTERN_LIBRARY)]
      private extern static void quaternionsToAxisAngle(ref Quaternions q, ref MCvPoint3D64f axisAngle);

      [DllImport(CvInvoke.EXTERN_LIBRARY)]
      private extern static void quaternionsToRotationMatrix(ref Quaternions quaternions, IntPtr rotation);

      [DllImport(CvInvoke.EXTERN_LIBRARY)]
      private extern static void quaternionsRotatePoint(ref Quaternions quaternions, ref MCvPoint3D64f point, ref MCvPoint3D64f pointDst);

      [DllImport(CvInvoke.EXTERN_LIBRARY)]
      private extern static void quaternionsRotatePoints(ref Quaternions quaternions, IntPtr pointSrc, IntPtr pointDst);

      [DllImport(CvInvoke.EXTERN_LIBRARY)]
      private extern static void quaternionsMultiply(ref Quaternions quaternions1, ref Quaternions quaternions2, ref Quaternions quaternionsDst);

      /// <summary>
      /// Set the value of the quaternions using euler angle
      /// </summary>
      /// <param name="x">Rotation around x-axis (roll) in radian</param>
      /// <param name="y">Rotation around y-axis (pitch) in radian</param>
      /// <param name="z">rotation around z-axis (yaw) in radian</param>
      public void SetEuler(double x, double y, double z)
      {
         eulerToQuaternions(x, y, z, ref this);
      }

      /// <summary>
      /// Get the equaivalent euler angle
      /// </summary>
      /// <param name="x">Rotation around x-axis (roll) in radian</param>
      /// <param name="y">Rotation around y-axis (pitch) in radian</param>
      /// <param name="z">rotation around z-axis (yaw) in radian</param>
      public void GetEuler(ref double x, ref double y, ref double z)
      {
         quaternionsToEuler(ref this, ref x, ref y, ref z);
      }

      /// <summary>
      /// Get or Set the equaivalent axis angle representation. (x,y,z) is the rotatation axis and |(x,y,z)| is the rotation angle in radians
      /// </summary>
      public MCvPoint3D64f AxisAngle
      {
         get
         {
            MCvPoint3D64f axisAngle = new MCvPoint3D64f();
            quaternionsToAxisAngle(ref this, ref axisAngle);
            return axisAngle;
         }
         set
         {
            axisAngleToQuaternions(ref value, ref this);
         }
      }

      /// <summary>
      /// Fill the (3x3) rotation matrix with the value such that it represent the quaternions
      /// </summary>
      /// <param name="rotation">The (3x3) rotation matrix which values will be set to represent this quaternions</param>
      public void GetRotationMatrix(Matrix<double> rotation)
      {
         quaternionsToRotationMatrix(ref this, rotation);
      }

      /// <summary>
      /// Rotate the points in <paramref name="pointsSrc"/> and save the result in <paramref name="pointsDst"/>. Inplace operation is supported (<paramref name="pointsSrc"/> == <paramref name="pointsDst"/>).
      /// </summary>
      /// <param name="pointsSrc">The points to be rotated</param>
      /// <param name="pointsDst">The result of the rotation, should be the same size as <paramref name="pointsSrc"/>, can be <paramref name="pointSrc"/> as well for inplace rotation</param>
      public void RotatePoints(Matrix<double> pointsSrc, Matrix<double> pointsDst)
      {
         quaternionsRotatePoints(ref this, pointsSrc, pointsDst);
      }

      /// <summary>
      /// Rotate the specific point and return the result
      /// </summary>
      /// <param name="point">The point to be rotated</param>
      /// <returns>The rotated point</returns>
      public MCvPoint3D64f RotatePoint(MCvPoint3D64f point)
      {
         MCvPoint3D64f result = new MCvPoint3D64f();
         quaternionsRotatePoint(ref this, ref point, ref result);
         return result;
      }

      /// <summary>
      /// Get or Set the unit rotation axis of the quaternion
      /// </summary>
      public MCvPoint3D64f RotationAxis
      {
         get
         {
            return new MCvPoint3D64f(X, Y, Z);
         }
         set
         {
            X = value.x;
            Y = value.y;
            Z = value.z;
         }
      }

      /// <summary>
      /// Get or Set the rotation angle in radian
      /// </summary>
      public double RotationAngle
      {
         get
         {
            return 2.0* Math.Acos(W);
         }
         set
         {
            W = Math.Cos(value / 2.0);
         }
      }

      /// <summary>
      /// Multiply the current Quaternions with <paramref name="quaternionsOther"/> 
      /// </summary>
      /// <param name="quaternionsOther">The other rotation</param>
      /// <return>A composition of the two rotations</return>
      public Quaternions Multiply(Quaternions quaternionsOther)
      {
         Quaternions result = new Quaternions();
         quaternionsMultiply(ref this, ref quaternionsOther, ref result);
         return result;
      }

      /// <summary>
      /// Computes the multiplication of two quaternions
      /// </summary>
      /// <param name="q1">The quaternions to be multiplied</param>
      /// <param name="q2">The quaternions to be multiplied</param>
      /// <returns>The multiplication of two quaternions</returns>
      public static Quaternions operator *(Quaternions q1, Quaternions q2)
      {
         return q1.Multiply(q2);
      }

      #region IEquatable<Quaternions> Members
      /// <summary>
      /// Check if this quaternions equals to <paramref name="other"/>
      /// </summary>
      /// <param name="other">The quaternions to be compared</param>
      /// <returns>True if two quaternions equals, false otherwise</returns>
      public bool Equals(Quaternions other)
      {
         return W == other.W && X == other.X && Y == other.Y && Z == other.Z;
      }

      #endregion
   }
}
