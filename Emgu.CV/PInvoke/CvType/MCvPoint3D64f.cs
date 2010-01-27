using System;
using System.Runtime.InteropServices;

namespace Emgu.CV.Structure
{
   /// <summary>
   /// Managed Structure equivalent to CvPoint3D64f
   /// </summary>
   [StructLayout(LayoutKind.Sequential)]
   public struct MCvPoint3D64f
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
   }
}
