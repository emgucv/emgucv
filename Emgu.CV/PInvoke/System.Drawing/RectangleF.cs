//----------------------------------------------------------------------------
//  Copyright (C) 2004-2020 by EMGU Corporation. All rights reserved.       
//----------------------------------------------------------------------------

#if (UNITY_WSA || UNITY_STANDALONE_WIN)
using System;
using System.Runtime.InteropServices;

namespace System.Drawing
{
   /// <summary>
   /// Managed structure equivalent to CvRect
   /// </summary>
   [Serializable]
   [StructLayout(LayoutKind.Sequential)]
   public struct RectangleF :IEquatable<RectangleF>
   {
      /// <summary>
      /// x-coordinate of the left-most rectangle corner[s]
      /// </summary>
      public float X;
      /// <summary>
      /// y-coordinate of the bottom-most rectangle corner[s]
      /// </summary>
      public float Y;
      /// <summary>
      /// width of the rectangle
      /// </summary>
      public float Width;
      /// <summary>
      /// height of the rectangle 
      /// </summary>
      public float Height;

      public float Left
      {
         get
         {
            return X;
         }
      }

      public float Right
      {
         get
         {
            return X + Width;
         }
      }

      public float Top
      {
         get
         {
            return Y;
         }
      }

      public float Bottom
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
      public RectangleF(float x, float y, float width, float height)
      {
         X = x; Y = y;
         Width = width;
         Height = height;
      }

      public RectangleF(PointF p, SizeF s)
      {
         X = p.X;
         Y = p.Y;
         Width = s.Width;
         Height = s.Height;
      }

      /// <summary>
      /// Area of the MCvRect
      /// </summary>
      public float Area
      {
         get { return Width * Height; }
      }

      public static RectangleF Empty
      {
         get
         {
            return new RectangleF(0, 0, 0, 0);
         }
      }

      public PointF Location
      {
         get
         {
            return new PointF(X, Y);
         }
         set
         {
            X = value.X;
            Y = value.Y;
         }
      }

      public SizeF Size
      {
         get
         {
            return new SizeF(Width, Height);
         }
         set
         {
            Width = value.Width;
            Height = value.Height;
         }
      }

      public void Intersect(RectangleF other)
      {
         RectangleF intersect = GetIntersection(other);
         X = intersect.X;
         Y = intersect.Y;
         Width = intersect.Width;
         Height = intersect.Height;
      }

      public bool Contains(RectangleF other)
      {
         return other.Equals(GetIntersection(other));
      }

      public bool IntersectsWith(RectangleF other)
      {
         return !GetIntersection(other).Equals(RectangleF.Empty);
      }

      private RectangleF GetIntersection(RectangleF other)
      {
         float left = Math.Max(Left, other.Left);
         float right = Math.Min(Right, other.Right);
         if (right < left)
         {
            //X = 0; Y = 0; Width = 0; Height = 0;
            return RectangleF.Empty;
         }

         float top = Math.Max(Top, other.Top);
         float bottom = Math.Min(Bottom, other.Bottom);

         if (bottom < top)
         {
            //X = 0; Y = 0; Width = 0; Height = 0;
            return RectangleF.Empty;
         }

         return new RectangleF(left, top, right - left, bottom - top);
      }

      public bool Equals(RectangleF other)
      {
         return X == other.X && Y == other.Y && Width == other.Width && Height == other.Height;
      }
   }
}

#endif