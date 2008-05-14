using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace Emgu.CV
{
    ///<summary> A circle </summary>
    public class Circle<T> : Point2D<T> where T : IComparable, new()
    {
        // The radius of the circle
        private T _radius;

        ///<summary> Create a circle with default values</summary>
        public Circle()
        : base()
        { 
            _radius = new T(); 
        }

        ///<summary> Create a circle with the specific center and radius </summary>
        ///<param name="center"> The center of this circle </param>
        ///<param name="radius"> The radius of this circle </param>
        public Circle(Point2D<T> center, T radius) 
            : base(center.Coordinate)
        {  
            _radius = radius; 
        }

        ///<summary> The center of the circle </summary>
        [XmlIgnore]
        public Point2D<T> Center 
        { 
            get { return (Point2D<T>) this; } 
            set { Coordinate = value.Coordinate; } 
        }

        ///<summary> The radius of the circle </summary>
        [XmlAttribute("Radius")]
        public T Radius { get { return _radius; } set { _radius = value; } }

        ///<summary> The area of the circle </summary>
        public double Area
        {
            get
            {
                double a = 0.0;
                try
                {
                    a = System.Convert.ToDouble(_radius);
                }
                catch (Exception)
                {
                    throw new Emgu.PrioritizedException(Emgu.ExceptionLevel.Critical, "Unable to compute area");
                }
                return a * a * Math.PI;
            }
        }

        /// <summary>
        /// Compare this circle with <paramref name="circle2"/>
        /// </summary>
        /// <param name="circle2">The other box to be compared</param>
        /// <returns>true if the two boxes equals</returns>
        public bool Equals(Circle<T> circle2)
        {
            return base.Equals(circle2) && Radius.CompareTo(circle2.Radius)==0;
        }
    };
}
