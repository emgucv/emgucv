using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace Emgu.CV.Structure
{
   /// <summary>
   /// Managed Structure equivalent to CvPoint3D32f
   /// </summary>
   [StructLayout(LayoutKind.Sequential)]
   public struct MCvPoint3D32f : IEquatable<MCvPoint3D32f>
   {
      /// <summary>
      /// x-coordinate
      /// </summary>
      public float x;

      /// <summary>
      /// y-coordinate
      /// </summary>
      public float y;

      /// <summary>
      /// z-coordinate
      /// </summary>
      public float z;

      /// <summary>
      /// Create a MCvPoint3D32f structure with the specific x and y coordinates
      /// </summary>
      /// <param name="x">x-coordinate</param>
      /// <param name="y">y-coordinate</param>
      /// <param name="z">z-coordinate</param>
      public MCvPoint3D32f(float x, float y, float z)
      {
         this.x = x;
         this.y = y;
         this.z = z;
      }

      /// <summary>
      /// Return the cross product of two 3D point
      /// </summary>
      /// <param name="point">the other 3D point</param>
      /// <returns>The cross product of the two 3D point</returns>
      public MCvPoint3D32f CrossProduct(MCvPoint3D32f point)
      {
         //A x B = <Ay*Bz - Az*By, Az*Bx - Ax*Bz, Ax*By - Ay*Bx>
         return  new MCvPoint3D32f(
            y * point.z - z * point.y,
            z * point.x - x * point.z,
            x * point.y - y * point.x);
      }

      /// <summary>
      /// return the norm of this 3D point
      /// </summary>
      public double Norm
      {
         get
         {
            return Math.Sqrt(x * x + y * y + z * z);
         }
      }

      /// <summary>
      /// Get the normalized point
      /// </summary>
      /// <returns></returns>
      public MCvPoint3D32f GetNormalizePoint()
      {
         float norm = (float) Norm;
         return new MCvPoint3D32f(x / norm, y / norm, z / norm);
      }

      #region IEquatable<MCvPoint3D32f> Members
      /// <summary>
      /// Return true if the location of the two points are equal
      /// </summary>
      /// <param name="other">The other point to compare with</param>
      /// <returns>True if the location of the two points are equal</returns>
      public bool Equals(MCvPoint3D32f other)
      {
         return (x == other.x) && (y == other.y) && (z == other.z);
      }
      #endregion
   }
}
