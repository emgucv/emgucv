//----------------------------------------------------------------------------
//  Copyright (C) 2004-2015 by EMGU Corporation. All rights reserved.       
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
   public struct RotatedRect : IConvexPolygonF, IEquatable<RotatedRect>
   {
      /// <summary>
      /// The center of the box
      /// </summary>
      public PointF Center;
      /// <summary>
      /// The size of the box
      /// </summary>
      public SizeF Size;
      /// <summary>
      /// The angle between the horizontal axis and the first side (i.e. width) in degrees
      /// </summary>
      /// <remarks>Possitive value means counter-clock wise rotation</remarks>
      public float Angle;

      /// <summary>
      /// Create a RotatedRect structure with the specific parameters
      /// </summary>
      /// <param name="center">The center of the box</param>
      /// <param name="size">The size of the box</param>
      /// <param name="angle">The angle of the box in degrees. Possitive value means counter-clock wise rotation</param>
      public RotatedRect(PointF center, SizeF size, float angle)
      {
         this.Center = center;
         this.Size = size;
         this.Angle = angle;
      }

      /// <summary>
      /// Shift the box by the specific amount
      /// </summary>
      /// <param name="x">The x value to be offseted</param>
      /// <param name="y">The y value to be offseted</param>
      public void Offset(int x, int y)
      {
         Center.X += x;
         Center.Y += y;
      }

      /// <summary>
      /// Represent an uninitialized RotatedRect
      /// </summary>
      public static RotatedRect Empty
      {
         get
         {
            return new RotatedRect();
         }
      }

      #region IConvexPolygonF Members
      /// <summary>
      /// Get the 4 verticies of this Box.
      /// </summary>
      /// <returns>The vertives of this RotatedRect</returns>
      public System.Drawing.PointF[] GetVertices()
      {
         return CvInvoke.BoxPoints(this);
      }

      #endregion

      /// <summary>
      /// Get the minimum enclosing rectangle for this Box
      /// </summary>
      /// <returns>The minimum enclosing rectangle for this Box</returns>
      public System.Drawing.Rectangle MinAreaRect()
      {
         PointF[] data = CvInvoke.BoxPoints(this);
         int minX = (int)Math.Round(Math.Min(Math.Min(data[0].X, data[1].X), Math.Min(data[2].X, data[3].X)));
         int maxX = (int)Math.Round(Math.Max(Math.Max(data[0].X, data[1].X), Math.Max(data[2].X, data[3].X)));
         int minY = (int)Math.Round(Math.Min(Math.Min(data[0].Y, data[1].Y), Math.Min(data[2].Y, data[3].Y)));
         int maxY = (int)Math.Round(Math.Max(Math.Max(data[0].Y, data[1].Y), Math.Max(data[2].Y, data[3].Y)));
         return new Rectangle(minX, minY, maxX - minX, maxY - minY);
      }

      #region IEquatable<RotatedRect> Members
      /// <summary>
      /// Returns true if the two box are equal
      /// </summary>
      /// <param name="other">The other box to compare with</param>
      /// <returns>True if two boxes are equal</returns>
      public bool Equals(RotatedRect other)
      {
         return Center.Equals(other.Center)
            && Size.Equals(other.Size)
            && Angle.Equals( other.Angle );
      }

      /// <summary>
      /// Convert a RectangleF to RotatedRect
      /// </summary>
      /// <param name="rectangle">The rectangle</param>
      /// <returns>The equivalent RotatedRect</returns>
      public static implicit operator RotatedRect(System.Drawing.RectangleF rectangle)
      {
         return new RotatedRect(
            new PointF(
               rectangle.Location.X + (rectangle.Width * 0.5f), 
               rectangle.Location.Y + (rectangle.Height* 0.5f) ), 
            rectangle.Size, 
            0);
      }
      #endregion
   }
}
