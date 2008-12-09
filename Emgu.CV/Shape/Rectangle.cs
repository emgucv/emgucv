using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;
using System.Runtime.Serialization;

namespace Emgu.CV
{
   ///<summary> A rectangle </summary>
   ///<typeparam name="T">The type of elements in this Rectangle</typeparam>
   [Serializable]
   public class Rectangle<T> : IConvexPolygon<T> where T : IComparable, new()
   {
      private Point2D<T> _center;

      /// <summary>
      /// The size: width &amp; height
      /// </summary>
      protected Point2D<T> _size;

      ///<summary> Create a rectangle with default values </summary>
      public Rectangle()
         : this(new Point2D<T>(), new Point2D<T>())
      {
      }

      ///<summary> Create a rectangle with the specific center, with and height</summary>
      ///<param name="center"> The center of the rectangle</param>
      ///<param name="size"> The size of the rectangle </param>
      public Rectangle(Point2D<T> center, Point2D<T> size)
      {
         _center = center;
         _size = size;
      }

      ///<summary> Create a rectangle with the specific center, with and height</summary>
      ///<param name="center"> The center of the rectangle</param>
      ///<param name="width"> The width of the rectangle </param>
      ///<param name="height"> The height of the rectangle </param>
      public Rectangle(Point2D<T> center, T width, T height)
      {
         _center = center;
         _size = new Point2D<T>(width, height);
      }

      ///<summary> 
      /// Create a rectangle from a CvRect structure 
      ///</summary>
      public Rectangle(MCvRect rect)
         : this()
      {
         MCvRect = rect;
      }

      /// <summary>
      /// The left most coordinate (a smaller x value)
      /// </summary>
      public T Left { get { return (T)System.Convert.ChangeType((System.Convert.ToDouble(_center.X) - System.Convert.ToDouble(Width) / 2.0), typeof(T)); } }
      /// <summary>
      /// The right most coordinate (a larger x value)
      /// </summary>
      public T Right { get { return (T)System.Convert.ChangeType((System.Convert.ToDouble(_center.X) + System.Convert.ToDouble(Width) / 2.0), typeof(T)); } }
      /// <summary>
      /// The top most coordinate ( a smaller y value )
      /// </summary>
      public T Bottom { get { return (T)System.Convert.ChangeType((System.Convert.ToDouble(_center.Y) + System.Convert.ToDouble(Height) / 2.0), typeof(T)); } }
      /// <summary>
      /// The bottom most coordinate ( a larger y value )
      /// </summary>
      public T Top { get { return (T)System.Convert.ChangeType((System.Convert.ToDouble(_center.Y) - System.Convert.ToDouble(Height) / 2.0), typeof(T)); } }

      /// <summary>
      /// The Size (width and height) of this rectangle
      /// </summary>
      [XmlIgnore]
      public Point2D<T> Size
      {
         get { return _size; }
         set { _size = value; }
      }

      ///<summary> 
      ///Get or Set the center of the rectangle
      ///</summary>
      public Point2D<T> Center
      {
         get { return _center; }
         set { _center = value; }
      }

      ///<summary> Get or Set the width of the rectangle </summary>
      [XmlAttribute("Width")]
      public T Width
      {
         get
         {
            return _size[0];
         }
         set
         {
            _size[0] = value;
         }
      }

      ///<summary> Get or Set the height of the rectangle </summary> 
      [XmlAttribute("Height")]
      public T Height
      {
         get
         {
            return _size[1];
         }
         set
         {
            _size[1] = value;
         }
      }

      ///<summary> 
      ///Get the top left corner of the rectangle
      ///</summary>
      public Point2D<T> TopLeft { get { return new Point2D<T>(Left, Top); } }
      ///<summary> 
      ///Get the top right corner of the rectangle
      ///</summary>
      public Point2D<T> TopRight { get { return new Point2D<T>(Right, Top); } }
      ///<summary> 
      ///Get the bottom left corner of the rectangle
      ///</summary>
      public Point2D<T> BottomLeft { get { return new Point2D<T>(Left, Bottom); } }
      ///<summary> 
      ///Get the bottom right corner of the rectangle
      ///</summary>
      public Point2D<T> BottomRight { get { return new Point2D<T>(Right, Bottom); } }

      ///<summary> 
      /// Get the area the rectangle occupied
      ///</summary>
      public double Area
      {
         get
         {
            return Math.Abs(System.Convert.ToDouble(Width) * System.Convert.ToDouble(Height));
         }
      }

      ///<summary> The CvRect representation of this rectangle </summary>
      [XmlIgnore]
      public MCvRect MCvRect
      {
         get
         {
            return new MCvRect(
                System.Convert.ToInt32(Left),
                System.Convert.ToInt32(Top),
                System.Convert.ToInt32(Width),
                System.Convert.ToInt32(Height));
         }
         set
         {
            _center = value.Center.Convert<T>();
            _size = value.Size.Convert<T>();
         }
      }

      /// <summary>
      /// Compare two rectangle, if equal, return true, otherwise return false
      /// </summary>
      /// <param name="rec">the other rectangle to compare with</param>
      /// <returns>true if the two rectangle equals, false otherwise</returns>
      public bool Equals(Rectangle<T> rec)
      {
         return Center.Equals(rec.Center) && Width.Equals(rec.Width) && Height.Equals(rec.Height);
      }

      /// <summary>
      /// Convert the current rectangle to different depth
      /// </summary>
      /// <typeparam name="TOther">the depth type to convert to</typeparam>
      /// <returns>The current rectangle in different depth</returns>
      public Rectangle<TOther> Convert<TOther>() where TOther : IComparable, new()
      {
         return new Rectangle<TOther>(Center.Convert<TOther>(), Size.Convert<TOther>());
      }

      #region explicit operators
      /// <summary>
      /// Explicit operator for RectangleF
      /// </summary>
      /// <param name="rect">A generic rectangle</param>
      /// <returns>The corresponding System.Drawing.Rectangle</returns>
      public static explicit operator System.Drawing.Rectangle(Rectangle<T> rect)
      {
         return rect.MCvRect;         
      }
      #endregion

      #region IConvexPolygon<T> Members
      /// <summary>
      /// Get the vertices of this triangle
      /// </summary>
      public virtual Point2D<T>[] Vertices
      {
         get
         {
            return new Point2D<T>[] { TopLeft, TopRight, BottomRight, BottomLeft };
         }
      }
      #endregion
   }
}
