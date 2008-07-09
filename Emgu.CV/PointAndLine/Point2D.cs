using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;
using System.Runtime.Serialization;

namespace Emgu.CV
{
    ///<summary> A two dimensional point </summary>
    ///<typeparam name="T"> The type of value for this 2D point</typeparam>
    [DataContract]
    [Serializable]
    public class Point2D<T> : Point<T> where T : IComparable, new() 
    {
        ///<summary> Create a 2D point located in the origin</summary>
        public Point2D() : base(2) { }

        ///<summary> Create a 2D point of the specific location</summary>
        ///<param name="x"> The x value of this point</param>
        ///<param name="y"> The y value of this point</param>
        public Point2D(T x, T y)
        {
            _coordinate = new T[2] { x, y };
        }

        ///<summary> Create a 2D point from a array of size 2</summary>
        ///<param name="data"> An array of size 2</param>
        public Point2D(T[] data)
        {
            _coordinate = data;
        }

        ///<summary> The x value of this point</summary>
        [XmlIgnore]
        public T X { get { return _coordinate[0]; } set { _coordinate[0] = value; } }

        ///<summary> The y value of this point</summary>
        [XmlIgnore]
        public T Y { get { return _coordinate[1]; } set { _coordinate[1] = value; } }

        ///<summary> The angle between the direction of this point and the x-axis, in radian</summary>
        public double PointRadianAngle
        {
            get
            {
                double[] d = Convert<double>().Coordinate;
                return System.Math.Atan2(d[1], d[0]);
            }
        }

        ///<summary> The angle between the direction of this point and the x-axis, in degree</summary>
        public double PointDegreeAngle
        {
            get { return PointRadianAngle * 180.0 / System.Math.PI; }
        }

        ///<summary> Convert this 2D point to the specific format</summary>
        ///<returns> An equavailent 2D point of the specific format</returns> 
        public new Point2D<T2> Convert<T2>() where T2 : IComparable, new()
        {
            return new Point2D<T2>(
            (T2)System.Convert.ChangeType(X, typeof(T2)),
            (T2)System.Convert.ChangeType(Y, typeof(T2)));
        }

        /// <summary>
        /// The CvPoint representation of th is 2D point
        /// </summary>
        [Obsolete("Will be removed in the next version, use MCvPoint instead")]
        public MCvPoint CvPoint
        {
            get
            {
                return MCvPoint;
            }
        }

        /// <summary>
        /// The CvPoint representation of th is 2D point
        /// </summary>
        public MCvPoint MCvPoint
        {
            get
            {
                Point2D<int> p = Convert<int>();
                return new MCvPoint(p.X, p.Y);
            }
            set
            {
                _coordinate[0] = (T) System.Convert.ChangeType(value.x, typeof(T));
                _coordinate[1] = (T) System.Convert.ChangeType(value.y, typeof(T));
            }
        }

        /// <summary>
        /// implicit operator to MCvPoint
        /// </summary>
        /// <param name="point">The 2D point</param>
        /// <returns>MCvPoint</returns>
        public static implicit operator MCvPoint(Point2D<T> point)
        {
            return point.MCvPoint;
        }

        /// <summary>
        /// Determine if the point is in a convex polygon
        /// </summary>
        /// <param name="polygon">the convex polygon</param>
        /// <returns>true if the point is in the convex polygon; false otherwise </returns>
        public bool InConvexPolygon(IConvexPolygon<T> polygon)
        {
            LineSegment2D<T>[] edges = PointCollection.PolyLine<T>(polygon.Vertices, true);
            int side = edges[0].Side(this);
            bool inside = true;
            for (int i = 1; i < edges.Length; i++)
                inside = (inside && (side == edges[i].Side(this)));
            return inside;
        }
    };

}
