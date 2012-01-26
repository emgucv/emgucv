//----------------------------------------------------------------------------
//  Copyright (C) 2004-2012 by EMGU. All rights reserved.       
//----------------------------------------------------------------------------

using System;
using System.Runtime.InteropServices;
using System.Drawing;

namespace Emgu.CV.Structure
{
   /// <summary>
   /// Managed structure equivalent to CvBox2D
   /// </summary>
   [StructLayout(LayoutKind.Sequential)]
   public struct MCvBox2D : IConvexPolygonF, IEquatable<MCvBox2D>
   {
      /// <summary>
      /// The center of the box
      /// </summary>
      public PointF center;
      /// <summary>
      /// The size of the box
      /// </summary>
      public SizeF size;
      /// <summary>
      /// The angle between the horizontal axis and the first side (i.e. width) in degrees
      /// </summary>
      /// <remarks>Possitive value means counter-clock wise rotation</remarks>
      public float angle;

      /// <summary>
      /// Create a MCvBox2D structure with the specific parameters
      /// </summary>
      /// <param name="center">The center of the box</param>
      /// <param name="size">The size of the box</param>
      /// <param name="angle">The angle of the box in degrees. Possitive value means counter-clock wise rotation</param>
      public MCvBox2D(PointF center, SizeF size, float angle)
      {
         this.center = center;
         this.size = size;
         this.angle = angle;
      }

      /// <summary>
      /// Shift the box by the specific amount
      /// </summary>
      /// <param name="x">The x value to be offseted</param>
      /// <param name="y">The y value to be offseted</param>
      public void Offset(int x, int y)
      {
         center.X += x;
         center.Y += y;
      }

      /// <summary>
      /// Represent an uninitialized MCvBox2D
      /// </summary>
      public static MCvBox2D Empty
      {
         get
         {
            return new MCvBox2D();
         }
      }

      #region IConvexPolygonF Members
      /// <summary>
      /// Get the 4 verticies of this Box.
      /// </summary>
      /// <returns>The vertives of this MCvBox2D</returns>
      public System.Drawing.PointF[] GetVertices()
      {
         PointF[] coordinates = new PointF[4];
         CvInvoke.cvBoxPoints(this, coordinates);
         return coordinates;
      }

      #endregion

      /// <summary>
      /// Get the minimum enclosing rectangle for this Box
      /// </summary>
      /// <returns>The minimum enclosing rectangle for this Box</returns>
      public System.Drawing.Rectangle MinAreaRect()
      {
         float[] data = new float[8];
         CvInvoke.cvBoxPoints(this, data);
         int minX = (int)Math.Round(Math.Min(Math.Min(data[0], data[2]), Math.Min(data[4], data[6])));
         int maxX = (int)Math.Round(Math.Max(Math.Max(data[0], data[2]), Math.Max(data[4], data[6])));
         int minY = (int)Math.Round(Math.Min(Math.Min(data[1], data[3]), Math.Min(data[5], data[7])));
         int maxY = (int)Math.Round(Math.Max(Math.Max(data[1], data[3]), Math.Max(data[5], data[7])));
         return new Rectangle(minX, minY, maxX - minX, maxY - minY);
      }

      #region IEquatable<MCvBox2D> Members
      /// <summary>
      /// Returns true if the two box are equal
      /// </summary>
      /// <param name="other">The other box to compare with</param>
      /// <returns>True if two boxes are equal</returns>
      public bool Equals(MCvBox2D other)
      {
         return center.Equals(other.center)
            && size.Equals(other.size)
            && angle == other.angle;
      }

      /// <summary>
      /// Convert a RectangleF to MCvBox2D
      /// </summary>
      /// <param name="rectangle">The rectangle</param>
      /// <returns>The equivalent MCvBox2D</returns>
      public static implicit operator MCvBox2D(System.Drawing.RectangleF rectangle)
      {
         return new MCvBox2D(
            new PointF(
               rectangle.Location.X + (rectangle.Width * 0.5f), 
               rectangle.Location.Y + (rectangle.Height* 0.5f) ), 
            rectangle.Size, 
            0);
      }
      #endregion
   }
}
