using System;
using System.Collections.Generic;
using System.Text;

namespace Emgu.CV
{
    ///<summary>
    /// A rotated rectangle box
    ///</summary>
    public class Box2D<T> : Rectangle<T> where T : IComparable, new()
    {
        /// <summary>
        /// The rotation angle of the rectangle in radians
        /// </summary>
        private double _angle;

        ///<summary> 
        ///Create an rotated rectangle with default parameters
        ///</summary>
        public Box2D()
            : base()
        {
            _angle = 0.0;
        }

        /// <summary>
        /// Compare this box with <paramref name="box2"/>
        /// </summary>
        /// <param name="box2">The other box to be compared</param>
        /// <returns>true if the two boxes equals</returns>
        public bool Equals(Box2D<T> box2)
        {
            return base.Equals(box2) && RadianAngle == box2.RadianAngle;
        }

        ///<summary>
        ///Create an rotated rectangle with specific parameters
        ///</summary>
        ///<param name="center"> The center of the rectangle</param>
        ///<param name="size"> The size of the rectangle</param>
        ///<param name="angle"> The rotation angle in radian for the rectangle</param>
        public Box2D(Point2D<T> center, Point2D<T> size, double angle)
            : base(center, size)
        {
            _angle = angle;
        }

        ///<summary> The rotation angle for this box in radian</summary>
        public double RadianAngle { get { return _angle; } }

        ///<summary> The rotation angle for this box in radian</summary>
        public double DegreeAngle { get { return _angle * 180.0 / Math.PI; } }

        ///<summary> The CvBox2D representation of this rectangle </summary>
        public MCvBox2D MCvBox2D
        {
            get
            {
                MCvBox2D box;
                box.center = new MCvPoint2D32f(System.Convert.ToSingle(Center.X), System.Convert.ToSingle(Center.Y));
                box.size = new MCvSize2D32f(System.Convert.ToSingle(Width), System.Convert.ToSingle(Height));
                box.angle = System.Convert.ToSingle(_angle);
                return box;
            }

            set
            {
                X = (T)System.Convert.ChangeType(value.center.x, typeof(T));
                Y = (T)System.Convert.ChangeType(value.center.y, typeof(T));
                Size = new Point2D<T>((T)System.Convert.ChangeType(value.size.width, typeof(T)), (T)System.Convert.ChangeType(value.size.height, typeof(T)));
                _angle = System.Convert.ToDouble(value.angle);
            }
        }
    };
}
