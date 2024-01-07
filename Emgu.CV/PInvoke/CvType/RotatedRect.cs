//----------------------------------------------------------------------------
//  Copyright (C) 2004-2024 by EMGU Corporation. All rights reserved.       
//----------------------------------------------------------------------------

using System;
using System.Runtime.InteropServices;
using System.Drawing;

namespace Emgu.CV.Structure
{
    /// <summary>
    /// The class represents rotated (i.e. not up-right) rectangles on a plane.
    /// </summary>
    [Serializable]
    [StructLayout(LayoutKind.Sequential)]
    public struct RotatedRect : IConvexPolygonF, IEquatable<RotatedRect>
    {
        /// <summary>
        /// The rectangle mass center
        /// </summary>
        public PointF Center;
        /// <summary>
        /// Width and height of the rectangle
        /// </summary>
        public SizeF Size;
        /// <summary>
        /// The rotation angle. When the angle is 0, 90, 180, 270 etc., the rectangle becomes an up-right rectangle.
        /// </summary>
        public float Angle;

        /// <summary>
        /// Create a RotatedRect structure with the specific parameters
        /// </summary>
        /// <param name="center">The rectangle mass center</param>
        /// <param name="size">Width and height of the rectangle</param>
        /// <param name="angle">The rotation angle. When the angle is 0, 90, 180, 270 etc., the rectangle becomes an up-right rectangle.</param>
        public RotatedRect(PointF center, SizeF size, float angle)
        {
            this.Center = center;
            this.Size = size;
            this.Angle = angle;
        }

        /// <summary>
        /// Shift the box by the specific amount
        /// </summary>
        /// <param name="x">The x value to be offset</param>
        /// <param name="y">The y value to be offset</param>
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
        /// Get the 4 vertices of this Box.
        /// </summary>
        /// <returns>The vertices of this RotatedRect</returns>
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
               && Angle.Equals(other.Angle);
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
                  rectangle.Location.Y + (rectangle.Height * 0.5f)),
               rectangle.Size,
               0);
        }
        #endregion
    }
}
