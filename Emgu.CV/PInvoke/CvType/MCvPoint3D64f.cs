//----------------------------------------------------------------------------
//  Copyright (C) 2004-2012 by EMGU. All rights reserved.       
//----------------------------------------------------------------------------

using System;
using System.Runtime.InteropServices;

namespace Emgu.CV.Structure
{
   /// <summary>
   /// Managed Structure equivalent to CvPoint3D64f
   /// </summary>
   [StructLayout(LayoutKind.Sequential)]
   public struct MCvPoint3D64f : IEquatable<MCvPoint3D64f>
   {
      /// <summary>
      /// x-coordinate
      /// </summary>
      public double x;

      /// <summary>
      /// y-coordinate
      /// </summary>
      public double y;

      /// <summary>
      /// z-coordinate
      /// </summary>
      public double z;

      /// <summary>
      /// Create a MCvPoint3D64f structure with the specific x and y coordinates
      /// </summary>
      /// <param name="x">x-coordinate</param>
      /// <param name="y">y-coordinate</param>
      /// <param name="z">z-coordinate</param>
      public MCvPoint3D64f(double x, double y, double z)
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
      public MCvPoint3D64f CrossProduct(MCvPoint3D64f point)
      {
         //A x B = <Ay*Bz - Az*By, Az*Bx - Ax*Bz, Ax*By - Ay*Bx>
         return new MCvPoint3D64f(
            y * point.z - z * point.y,
            z * point.x - x * point.z,
            x * point.y - y * point.x);
      }

      /// <summary>
      /// Return the dot product of two 3D point
      /// </summary>
      /// <param name="point">the other 3D point</param>
      /// <returns>The dot product of the two 3D point</returns>
      public double DotProduct(MCvPoint3D64f point)
      {
         return x * point.x + y * point.y + z * point.z;
      }

      #region operator overloads
      /// <summary>
      /// Compute the sum of two 3D points
      /// </summary>
      /// <param name="p1">The first point to be added</param>
      /// <param name="p2">The second point to be added</param>
      /// <returns>The sum of two points</returns>
      public static MCvPoint3D64f operator +(MCvPoint3D64f p1, MCvPoint3D64f p2)
      {
         return new MCvPoint3D64f(p1.x + p2.x, p1.y + p2.y, p1.z + p2.z);
      }

      /// <summary>
      /// Subtract <paramref name="p2"/> from <paramref name="p1"/>
      /// </summary>
      /// <param name="p1">The first point</param>
      /// <param name="p2">The point to be added</param>
      /// <returns>The sum of two points</returns>
      public static MCvPoint3D64f operator -(MCvPoint3D64f p1, MCvPoint3D64f p2)
      {
         return new MCvPoint3D64f(p1.x - p2.x, p1.y - p2.y, p1.z - p2.z);
      }

      /// <summary>
      /// Multiply the point with a scale
      /// </summary>
      /// <param name="p">The point to be multiplied</param>
      /// <param name="scale">The scale</param>
      /// <returns>The point multiplied by the scale</returns>
      public static MCvPoint3D64f operator *(MCvPoint3D64f p, double scale)
      {
         return new MCvPoint3D64f(p.x * scale, p.y * scale, p.z * scale);
      }

      /// <summary>
      /// Multiply the point with a scale
      /// </summary>
      /// <param name="p">The point to be multiplied</param>
      /// <param name="scale">The scale</param>
      /// <returns>The point multiplied by the scale</returns>
      public static MCvPoint3D64f operator *( double scale, MCvPoint3D64f p)
      {
         return p * scale;
      }
      #endregion

      #region IEquatable<MCvPoint3D64f> Members
      /// <summary>
      /// Check if the other point equals to this point
      /// </summary>
      /// <param name="other">The point to be compared</param>
      /// <returns>True if the two points are equal</returns>
      public bool Equals(MCvPoint3D64f other)
      {
         return x == other.x && y == other.y && z == other.z;
      }

      #endregion
   }
}
