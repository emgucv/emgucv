//----------------------------------------------------------------------------
//  Copyright (C) 2004-2012 by EMGU. All rights reserved.       
//----------------------------------------------------------------------------

using System;
using System.Runtime.InteropServices;
using Emgu.Util;

namespace Emgu.CV.Structure
{
   /// <summary>
   /// Managed Structure equivalent to CvPoint2D64f
   /// </summary>
   [StructLayout(LayoutKind.Sequential)]
   public struct MCvPoint2D64f : IEquatable<MCvPoint2D64f>, IInterpolatable<MCvPoint2D64f>
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
      /// Create a MCvPoint2D64f structure with the specific x and y coordinates
      /// </summary>
      /// <param name="x">x-coordinate</param>
      /// <param name="y">y-coordinate</param>
      public MCvPoint2D64f(double x, double y)
      {
         this.x = x; this.y = y;
      }

      #region operator overloads
      /// <summary>
      /// Compute the sum of two 3D points
      /// </summary>
      /// <param name="p1">The first point to be added</param>
      /// <param name="p2">The second point to be added</param>
      /// <returns>The sum of two points</returns>
      public static MCvPoint2D64f operator +(MCvPoint2D64f p1, MCvPoint2D64f p2)
      {
         return new MCvPoint2D64f(p1.x + p2.x, p1.y + p2.y);
      }

      /// <summary>
      /// Subtract <paramref name="p2"/> from <paramref name="p1"/>
      /// </summary>
      /// <param name="p1">The first point</param>
      /// <param name="p2">The point to be added</param>
      /// <returns>The sum of two points</returns>
      public static MCvPoint2D64f operator -(MCvPoint2D64f p1, MCvPoint2D64f p2)
      {
         return new MCvPoint2D64f(p1.x - p2.x, p1.y - p2.y);
      }

      /// <summary>
      /// Multiply the point with a scale
      /// </summary>
      /// <param name="p">The point to be multiplied</param>
      /// <param name="scale">The scale</param>
      /// <returns>The point multiplied by the scale</returns>
      public static MCvPoint2D64f operator *(MCvPoint2D64f p, double scale)
      {
         return new MCvPoint2D64f(p.x * scale, p.y * scale);
      }

      /// <summary>
      /// Multiply the point with a scale
      /// </summary>
      /// <param name="p">The point to be multiplied</param>
      /// <param name="scale">The scale</param>
      /// <returns>The point multiplied by the scale</returns>
      public static MCvPoint2D64f operator *(double scale, MCvPoint2D64f p)
      {
         return p * scale;
      }
      #endregion

      #region IEquatable<MCvPoint2D64f> Members
      /// <summary>
      /// Returns true if the two points equals.
      /// </summary>
      /// <param name="other">The other point to compare with</param>
      /// <returns>True if the two points equals</returns>
      public bool Equals(MCvPoint2D64f other)
      {
         return x == other.x && y == other.y;
      }

      #endregion

      #region IInterpolatable<MCvPoint2D64f> Members

      double IInterpolatable<MCvPoint2D64f>.InterpolationIndex
      {
         get { return x; }
      }
      /*
      void IInterpolatable<MCvPoint2D64f>.Mul(double scale)
      {
         x *= scale;
         y *= scale;
      }

      void IInterpolatable<MCvPoint2D64f>.Add(MCvPoint2D64f i)
      {
         x += i.x;
         y += i.y;
      }

      void IInterpolatable<MCvPoint2D64f>.Sub(MCvPoint2D64f i)
      {
         x -= i.x;
         y -= i.y;
      }*/

      MCvPoint2D64f IInterpolatable<MCvPoint2D64f>.LinearInterpolate(MCvPoint2D64f other, double index)
      {
         double f1 = (other.x - index) / (other.x - this.x);
         double f2 = 1.0-f1;
         return new MCvPoint2D64f(x * f1 + other.x * f2, y * f1 + other.y * f2);
      }
      #endregion
   }
}

