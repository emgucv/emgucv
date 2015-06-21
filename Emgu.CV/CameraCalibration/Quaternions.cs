//----------------------------------------------------------------------------
//  Copyright (C) 2004-2015 by EMGU Corporation. All rights reserved.       
//----------------------------------------------------------------------------

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
#if !NETFX_CORE
   [Serializable]
#endif
   public struct Quaternions : IEquatable<Quaternions>
   {
      /// <summary>
      /// Create a quaternion with the specific values
      /// </summary>
      /// <param name="w">The W component of the quaternion: the value for cos(rotation angle / 2)</param>
      /// <param name="x">The X component of the vector: rotation axis * sin(rotation angle / 2)</param>
      /// <param name="y">The Y component of the vector: rotation axis * sin(rotation angle / 2)</param>
      /// <param name="z">The Z component of the vector: rotation axis * sin(rotation angle / 2)</param>
      public Quaternions(double w, double x, double y, double z)
      {
         _w = w;
         _x = x;
         _y = y;
         _z = z;
      }

      private double _w;
      private double _x;
      private double _y;
      private double _z;

      /// <summary>
      /// The W component of the quaternion: the value for cos(rotation angle / 2)
      /// </summary>
      public double W
      {
         get { return _w; }
         set { _w = value; }
      }

      /// <summary>
      /// The X component of the vector: rotation axis * sin(rotation angle / 2)
      /// </summary>
      public double X
      {
         get { return _x; }
         set { _x = value; }
      }

      /// <summary>
      /// The Y component of the vector: rotation axis * sin(rotation angle / 2)
      /// </summary>
      public double Y
      {
         get { return _y; }
         set { _y = value; }
      }

      /// <summary>
      /// The Z component of the vector: rotation axis * sin(rotation angle / 2)
      /// </summary>
      public double Z
      {
         get { return _z; }
         set { _z = value; }
      }


      /// <summary>
      /// Set the value of the quaternions using euler angle
      /// </summary>
      /// <param name="x">Rotation around x-axis (roll) in radian</param>
      /// <param name="y">Rotation around y-axis (pitch) in radian</param>
      /// <param name="z">rotation around z-axis (yaw) in radian</param>
      public void SetEuler(double x, double y, double z)
      {
         CvInvoke.eulerToQuaternions(x, y, z, ref this);
      }

      /// <summary>
      /// Get the equivalent euler angle
      /// </summary>
      /// <param name="x">Rotation around x-axis (roll) in radian</param>
      /// <param name="y">Rotation around y-axis (pitch) in radian</param>
      /// <param name="z">rotation around z-axis (yaw) in radian</param>
      public void GetEuler(ref double x, ref double y, ref double z)
      {
         CvInvoke.quaternionsToEuler(ref this, ref x, ref y, ref z);
      }

      /// <summary>
      /// Get or set the equivalent axis angle representation. (x,y,z) is the rotation axis and |(x,y,z)| is the rotation angle in radians
      /// </summary>
      public MCvPoint3D64f AxisAngle
      {
         get
         {
            MCvPoint3D64f axisAngle = new MCvPoint3D64f();
            CvInvoke.quaternionsToAxisAngle(ref this, ref axisAngle);
            return axisAngle;
         }
         set
         {
            CvInvoke.axisAngleToQuaternions(ref value, ref this);
         }
      }

      /// <summary>
      /// Fill the (3x3) rotation matrix with the value such that it represent the quaternions
      /// </summary>
      /// <param name="rotation">The (3x3) rotation matrix which values will be set to represent this quaternions</param>
      public void GetRotationMatrix(Matrix<double> rotation)
      {
         CvInvoke.quaternionsToRotationMatrix(ref this, rotation);
      }

      /// <summary>
      /// Rotate the points in <paramref name="pointsSrc"/> and save the result in <paramref name="pointsDst"/>. In-place operation is supported (<paramref name="pointsSrc"/> == <paramref name="pointsDst"/>).
      /// </summary>
      /// <param name="pointsSrc">The points to be rotated</param>
      /// <param name="pointsDst">The result of the rotation, should be the same size as <paramref name="pointsSrc"/>, can be <paramref name="pointsSrc"/> as well for inplace rotation</param>
      public void RotatePoints(Matrix<double> pointsSrc, Matrix<double> pointsDst)
      {
         CvInvoke.quaternionsRotatePoints(ref this, pointsSrc, pointsDst);
      }

      /// <summary>
      /// Rotate the specific point and return the result
      /// </summary>
      /// <param name="point">The point to be rotated</param>
      /// <returns>The rotated point</returns>
      public MCvPoint3D64f RotatePoint(MCvPoint3D64f point)
      {
         MCvPoint3D64f result = new MCvPoint3D64f();
         CvInvoke.quaternionsRotatePoint(ref this, ref point, ref result);
         return result;
      }

      /// <summary>
      /// Get the rotation axis of the quaternion
      /// </summary>
      public MCvPoint3D64f RotationAxis
      {
         get
         {
            if (this.Equals(Empty))
               return new MCvPoint3D64f(0, 0, 1); //For empty quaternion, return a random axis
            else
            {
               double norm = Math.Sqrt(X * X + Y * Y + Z * Z);
               return new MCvPoint3D64f(X / norm, Y / norm, Z / norm);
            }
         }
         /*
         set
         {
            X = value.x;
            Y = value.y;
            Z = value.z;
         }*/
      }

      /// <summary>
      /// Get the rotation angle in radian
      /// </summary>
      public double RotationAngle
      {
         get
         {
            return 2.0 * Math.Acos(W);
         }
         /*
         set
         {
            W = Math.Cos(value / 2.0);
         }*/
      }

      /// <summary>
      /// Multiply the current Quaternions with <paramref name="quaternionsOther"/> 
      /// </summary>
      /// <param name="quaternionsOther">The other rotation</param>
      /// <return>A composition of the two rotations</return>
      public Quaternions Multiply(Quaternions quaternionsOther)
      {
         Quaternions result = new Quaternions();
         CvInvoke.quaternionsMultiply(ref this, ref quaternionsOther, ref result);
         return result;
      }

      /// <summary>
      /// Perform quaternions linear interpolation
      /// </summary>
      /// <param name="quaternionsOther">The other quaternions to interpolate with</param>
      /// <param name="weightForOther">If 0.0, the result is the same as this quaternions. If 1.0 the result is the same as <paramref name="quaternionsOther"/></param>
      /// <returns>The linear interpolated quaternions</returns>
      public Quaternions Slerp(Quaternions quaternionsOther, double weightForOther)
      {
         Quaternions result = new Quaternions();
         CvInvoke.quaternionsSlerp(ref this, ref quaternionsOther, weightForOther, ref result);
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

      /// <summary>
      /// Get the quaternions that represent a rotation of 0 degrees.
      /// </summary>
      public static readonly Quaternions Empty = new Quaternions(1.0, 0.0, 0.0, 0.0);

      /// <summary>
      /// Compute the conjugate of the quaternions
      /// </summary>
      public void Conjugate()
      {
         X = -X; Y = -Y; Z = -Z;
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

      /// <summary>
      /// Get the string representation of the Quaternions
      /// </summary>
      /// <returns>The string representation</returns>
      public override string ToString()
      {
         return String.Format("[{0}, {1}, {2}, {3}]", W, X, Y, Z);
      }
   }

   public static partial class CvInvoke
   {
      [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      internal extern static void eulerToQuaternions(double x, double y, double z, ref Quaternions q);

      [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      internal extern static void quaternionsToEuler(ref Quaternions q, ref double x, ref double y, ref double z);

      [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      internal extern static void axisAngleToQuaternions(ref MCvPoint3D64f axisAngle, ref Quaternions q);

      [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      internal extern static void quaternionsToAxisAngle(ref Quaternions q, ref MCvPoint3D64f axisAngle);

      [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      internal extern static void quaternionsToRotationMatrix(ref Quaternions quaternions, IntPtr rotation);

      [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      internal extern static void quaternionsRotatePoint(ref Quaternions quaternions, ref MCvPoint3D64f point, ref MCvPoint3D64f pointDst);

      [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      internal extern static void quaternionsRotatePoints(ref Quaternions quaternions, IntPtr pointSrc, IntPtr pointDst);

      [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      internal extern static void quaternionsMultiply(ref Quaternions quaternions1, ref Quaternions quaternions2, ref Quaternions quaternionsDst);

      [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      internal extern static void quaternionsSlerp(ref Quaternions qa, ref Quaternions qb, double t, ref Quaternions qm);
   }
}
