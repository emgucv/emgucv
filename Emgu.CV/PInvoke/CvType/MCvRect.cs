using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;

namespace Emgu.CV
{
   /// <summary>
   /// Managed structure equivalent to CvRect
   /// </summary>
   [StructLayout(LayoutKind.Sequential)]
   public struct MCvRect
   {
      /// <summary>
      /// x-coordinate of the left-most rectangle corner[s]
      /// </summary>
      public int x;
      /// <summary>
      /// y-coordinate of the bottom-most rectangle corner[s]
      /// </summary>
      public int y;
      /// <summary>
      /// width of the rectangle
      /// </summary>
      public int width;
      /// <summary>
      /// height of the rectangle 
      /// </summary>
      public int height;

      /// <summary>
      /// Create a CvRect with the specific information
      /// </summary>
      /// <param name="x">x-coordinate of the left-most rectangle corner[s]</param>
      /// <param name="y">y-coordinate of the bottom-most rectangle corner[s]</param>
      /// <param name="width">width of the rectangle</param>
      /// <param name="height">height of the rectangle </param>
      public MCvRect(int x, int y, int width, int height)
      {
         this.x = x; this.y = y;
         this.width = width;
         this.height = height;
      }

      /// <summary>
      /// Center of the MCvRect
      /// </summary>
      public Point2D<double> Center
      {
         get { return new Point2D<double>((x + width / 2.0), (y + height / 2.0)); }
      }

      /// <summary>
      /// Size of the MCvRect
      /// </summary>
      public Point2D<int> Size
      {
         get { return new Point2D<int>(width, height); }
      }

      /// <summary>
      /// Area of the MCvRect
      /// </summary>
      public int Area
      {
         get { return width * height; }
      }

      /// <summary>
      /// Implicit operator for Rectangle
      /// </summary>
      /// <param name="rect">The MCvRect structure</param>
      /// <returns>An equivalent representation of the System.Drawing.Rectangle</returns>
      public static implicit operator System.Drawing.Rectangle(MCvRect rect)
      {
         return new System.Drawing.Rectangle(rect.x, rect.y, rect.width, rect.height);
      }
   }
}
