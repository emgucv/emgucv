using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;
using System.Runtime.Serialization;
using System.Diagnostics;

namespace Emgu.CV
{
   ///<summary> A two dimensional point </summary>
   ///<typeparam name="T"> The type of value for this 2D point</typeparam>
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
         Debug.Assert(data.Length == 2, "The array 'data' should have lentgth of 2");
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
      /// The MCvPoint representation of this 2D point
      /// </summary>
      [XmlIgnore]
      public MCvPoint MCvPoint
      {
         get
         {
            Point2D<int> p = Convert<int>();
            return new MCvPoint(p.X, p.Y);
         }
         set
         {
            _coordinate[0] = (T)System.Convert.ChangeType(value.x, typeof(T));
            _coordinate[1] = (T)System.Convert.ChangeType(value.y, typeof(T));
         }
      }

      /// <summary>
      /// The MCvPoint2D32f representation of this 2D point
      /// </summary>
      [XmlIgnore]
      public MCvPoint2D32f MCvPoint2D32f
      {
         get
         {
            Point2D<float> p = Convert<float>();
            return new MCvPoint2D32f(p.X, p.Y);
         }
         set
         {
            _coordinate[0] = (T)System.Convert.ChangeType(value.x, typeof(T));
            _coordinate[1] = (T)System.Convert.ChangeType(value.y, typeof(T));
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
         return InConvexPolygon(polygon.Vertices);
      }

      /// <summary>
      /// Determine if the point is in a convex polygon
      /// </summary>
      /// <param name="polygon">the convex polygon</param>
      /// <returns>true if the point is in/on the convex polygon; false otherwise </returns>
      public bool InConvexPolygon(Point2D<T>[] polygon)
      {
         LineSegment2D<T>[] edges = PointCollection.PolyLine<T>(polygon, true);
         int side = edges[0].Side(this);

         for (int i = 1; i < edges.Length; i++)
         {
            int currentSide = edges[i].Side(this);
            if (side == 0) side = currentSide;

            if (!(side == currentSide || currentSide == 0))
               return false;
         }
         return true;
      }

      #region operator overload
      /// <summary>
      /// Add the 2nd point from the 1st point and returns the result
      /// </summary>
      /// <param name="p1">The point to be added</param>
      /// <param name="p2">The point to be added</param>
      /// <returns>The sum of the points</returns>
      public static Point2D<T> operator +(Point2D<T> p1, Point2D<T> p2)
      {
         return new Point2D<T>(p1.Add(p2).Coordinate);
      }

      /// <summary>
      /// Multiply the point with the specific value
      /// </summary>
      /// <param name="point">The point to multiply</param>
      /// <param name="value">the value to multiply</param>
      /// <returns>the multiplication result</returns>
      public static Point2D<T> operator *(Point2D<T> point, T value)
      {
         return new Point2D<T>( point.Mul(value).Coordinate );
      }

      ///<summary>
      ///Subtract the 2nd point from the 1st point and returns the result
      ///</summary>
      ///<param name="p1"> The point to subtract value from </param>
      ///<param name="p2"> The value to be subtracted from p1 </param>
      public static Point2D<T> operator -(Point2D<T> p1, Point2D<T> p2)
      {
         return new Point2D<T>(p1.Sub(p2).Coordinate);
      }
      #endregion 
   }
}
