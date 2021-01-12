//----------------------------------------------------------------------------
//  Copyright (C) 2004-2021 by EMGU Corporation. All rights reserved.       
//----------------------------------------------------------------------------

#if UNITY_WSA
using System;
using System.Runtime.InteropServices;

namespace System.Drawing
{
   /// <summary>
   /// Managed structure equivalent to CvRect
   /// </summary>
   [Serializable]
   [StructLayout(LayoutKind.Sequential)]
   public struct Rectangle
   {
      /// <summary>
      /// x-coordinate of the left-most rectangle corner[s]
      /// </summary>
      public int X;
      /// <summary>
      /// y-coordinate of the bottom-most rectangle corner[s]
      /// </summary>
      public int Y;
      /// <summary>
      /// width of the rectangle
      /// </summary>
      public int Width;
      /// <summary>
      /// height of the rectangle 
      /// </summary>
      public int Height;

      public int Left
      {
         get
         {
            return X;
         }
      }

      public int Right
      {
         get
         {
            return X + Width;
         }
      }

      public int Top
      {
         get
         {
            return Y;
         }
      }

      public int Bottom
      {
         get
         {
            return Y + Height;
         }
      }

      /// <summary>
      /// Create a CvRect with the specific information
      /// </summary>
      /// <param name="x">x-coordinate of the left-most rectangle corner[s]</param>
      /// <param name="y">y-coordinate of the bottom-most rectangle corner[s]</param>
      /// <param name="width">width of the rectangle</param>
      /// <param name="height">height of the rectangle </param>
      public Rectangle(int x, int y, int width, int height)
      {
         X = x; Y = y;
         Width = width;
         Height = height;
      }

      public void Offset(int x, int y)
      {
         X += x;
         Y += y;
      }

      public Rectangle(Point p, Size s)
      {
         X = p.X;
         Y = p.Y;
         Width = s.Width;
         Height = s.Height;
      }

      /// <summary>
      /// Area of the MCvRect
      /// </summary>
      public int Area
      {
         get { return Width * Height; }
      }

      public static Rectangle Empty
      {
         get
         {
            return new Rectangle(0, 0, 0, 0);
         }
      }

      public bool IsEmpty
      {
         get
         {
            return X == 0 && Y == 0 && Width == 0 && Height == 0;
         }
      }

      public Point Location
      {
         get
         {
            return new Point(X, Y);
         }
      }

      public Size Size
      {
         get
         {
            return new Size(Width, Height);
         }
      }

      public static Rectangle Round(RectangleF rectf)
      {
         return new Rectangle((int)Math.Round(rectf.X), (int)Math.Round(rectf.Y), (int)Math.Round(rectf.Width), (int)Math.Round(rectf.Height));
      }

      public void Intersect(Rectangle other)
      {
         Rectangle intersect = GetIntersection(other);
         X = intersect.X;
         Y = intersect.Y;
         Width = intersect.Width;
         Height = intersect.Height;
      }

      public bool Contains(Point point)
      {
         return point.X >= X && point.Y >= Y && point.X < (X + Width) &&
                point.Y < (Y + Height);
      }

      public bool Contains(Rectangle other)
      {
         return other.Equals(GetIntersection(other));
      }

      public bool IntersectsWith(Rectangle other)
      {
         return !GetIntersection(other).Equals(Rectangle.Empty);
      }

      private Rectangle GetIntersection(Rectangle other)
      {
         int left = Math.Max(Left, other.Left);
         int right = Math.Min(Right, other.Right);
         if (right < left)
         {
            //X = 0; Y = 0; Width = 0; Height = 0;
            return Rectangle.Empty;
         }

         int top = Math.Max(Top, other.Top);
         int bottom = Math.Min(Bottom, other.Bottom);

         if (bottom < top)
         {
            //X = 0; Y = 0; Width = 0; Height = 0;
            return Rectangle.Empty;
         }

         return new Rectangle(left, top, right - left, bottom - top);
      }
   }
}

#endif