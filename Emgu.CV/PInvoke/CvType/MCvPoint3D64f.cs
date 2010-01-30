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
