using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;
using System.Runtime.Serialization;

namespace Emgu.CV
{
    ///<summary> A rectangle </summary>
    [DataContract]
    [Serializable]
    public class Rectangle<T> : Point2D<T> where T : IComparable, new()
    {
        private Point2D<T> _size;

        ///<summary> Create a rectangle with default values </summary>
        public Rectangle()
            : base()
        {
            _size = new Point2D<T>();
        }

        ///<summary> Create a rectangle with the specific center, with and height</summary>
        ///<param name="center"> The center of the rectangle</param>
        ///<param name="size"> The size of the rectangle </param>
        public Rectangle(Point2D<T> center, Point2D<T> size)
        {
            _coordinate = center.Coordinate;
            _size = size;
        }

        ///<summary> Create a rectangle with the specific left, right, top bottom corrdinates</summary>
        public Rectangle(T left, T right, T top, T bottom)
        {
            _coordinate = new T[2] { 
                (T) System.Convert.ChangeType( (System.Convert.ToDouble(right) + System.Convert.ToDouble(left)) / 2.0, typeof(T)),
                (T) System.Convert.ChangeType( (System.Convert.ToDouble(top) + System.Convert.ToDouble(bottom)) / 2.0, typeof(T))
                };
            _size = new Point2D<T>(
                (T)System.Convert.ChangeType(System.Convert.ToDouble(right) - System.Convert.ToDouble(left), typeof(T)),
                (T)System.Convert.ChangeType(System.Convert.ToDouble(top) - System.Convert.ToDouble(bottom), typeof(T))
                );
        }

        ///<summary> Create a rectangle from a CvRect structure </summary>
        public Rectangle(MCvRect rect)
            : this()
        {
            MCvRect = rect;
        }

        /// <summary>
        /// The left most coordinate
        /// </summary>
        public T Left { get { return (T)System.Convert.ChangeType((System.Convert.ToDouble(X) - System.Convert.ToDouble(Width) / 2.0), typeof(T)); } }
        /// <summary>
        /// The right most coordinate
        /// </summary>
        public T Right { get { return (T)System.Convert.ChangeType((System.Convert.ToDouble(X) + System.Convert.ToDouble(Width) / 2.0), typeof(T)); } }
        /// <summary>
        /// The top most coordinate
        /// </summary>
        public T Top { get { return (T)System.Convert.ChangeType((System.Convert.ToDouble(Y) + System.Convert.ToDouble(Height) / 2.0), typeof(T)); } }
        /// <summary>
        /// The bottom most coordinate
        /// </summary>
        public T Bottom { get { return (T)System.Convert.ChangeType((System.Convert.ToDouble(Y) - System.Convert.ToDouble(Height) / 2.0), typeof(T)); } }

        /// <summary>
        /// The Size (width and height) of this rectangle
        /// </summary>
        [DataMember]
        public Point2D<T> Size
        {
            get { return _size; }
            set { _size = value; }
        }

        ///<summary> The center of the rectangle</summary>
        [XmlIgnore]
        public Point2D<T> Center
        {
            get { return (Point2D<T>) this; }
            set { _coordinate = value.Coordinate; }
        }

        ///<summary> The width of the rectangle </summary>
        [XmlIgnore]
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
        
        ///<summary> The height of the rectangle </summary> 
        [XmlIgnore]
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

        ///<summary> The top left corner of the rectangle</summary>
        public Point2D<T> TopLeft { get { return new Point2D<T>(Left, Top); } }
        ///<summary> The top right corner of the rectangle</summary>
        public Point2D<T> TopRight { get { return new Point2D<T>(Right, Top); } }
        ///<summary> The bottom left corner of the rectangle</summary>
        public Point2D<T> BottomLeft { get { return new Point2D<T>(Left, Bottom); } }
        ///<summary> The bottom right corner of the rectangle</summary>
        public Point2D<T> BottomRight { get { return new Point2D<T>(Right, Bottom); } }

        ///<summary> The area of the rectangle </summary>
        public double Area { get { return Math.Abs(System.Convert.ToDouble(Width) * System.Convert.ToDouble(Height)); } }

        ///<summary> The CvRect representation of this rectangle </summary>
        [XmlIgnore]
        public MCvRect MCvRect
        {
            get
            {
                return new MCvRect(
                    System.Convert.ToInt32(Left),
                    System.Convert.ToInt32(Bottom),
                    System.Convert.ToInt32(Width),
                    System.Convert.ToInt32(Height));
            }
            set
            {
                _coordinate = value.Center.Convert<T>().Coordinate;
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
            return base.Equals(rec) && Size.Equals(rec.Size);
        }
    };
}
