using System;
using System.Collections.Generic;
using System.Text;

namespace Emgu.CV
{
   ///<summary> A 2D line </summary>
   public class Line2D<T> where T : struct, IComparable
   {
      ///<summary> A point on the line </summary>
      protected Point2D<T> _p1;
      ///<value> An other point on the line </value>
      protected Point2D<T> _p2;

      ///<summary>
      ///Create a default line
      ///</summary>
      public Line2D()
      {
         _p1 = new Point2D<T>();
         _p2 = new Point2D<T>();
      }

      ///<summary> 
      ///Create a line by specifing two points on the line
      ///</summary>
      ///<param name="p1"> A point on the line </param>
      ///<param name="p2"> An other point on the line </param>
      public Line2D(Point2D<T> p1, Point2D<T> p2)
      {
         _p1 = p1;
         _p2 = p2;
      }

      ///<summary> A point on the line </summary>
      public Point2D<T> P1 { get { return _p1; } set { _p1 = value; } }

      ///<summary> An other point on the line </summary>
      public Point2D<T> P2 { get { return _p2; } set { _p2 = value; } }

      ///<summary> The direction of the line, the norm of which is 1 </summary>
      public virtual Point2D<double> Direction
      {
         get
         {
            return new Point2D<double>((_p2 - _p1).Normalized.Coordinate);
         }
      }

      ///<summary> Obtain the Y value from the X value</summary>
      ///<param name="x">The X value</param>
      ///<returns>The Y value</returns>
      public T YByX(T x)
      {
         Point2D<double> p1 = _p1.Convert<double>();
         Point2D<double> dir = Direction;
         return (T)System.Convert.ChangeType((System.Convert.ToDouble(x) - p1.X) / dir.X * dir.Y + p1.Y, typeof(T));
      }

      /// <summary>
      /// Determin which side of the line the 2D point is at
      /// </summary>
      /// <param name="point">the point</param>
      /// <returns>
      /// 1 if on the right hand side;
      /// 0 if on the line;
      /// -1 if on the left hand side;
      /// </returns>
      public int Side(Point2D<T> point)
      {
         Point2D<double> p1 = P1.Convert<double>();
         Point2D<double> p2 = P2.Convert<double>();
         Point2D<double> p = point.Convert<double>();
         double res = (p2.X - p1.X) * (p.Y - p1.Y) - (p.X - p1.X) * (p2.Y - p1.Y);
         return res > 0.0 ? 1 :
            res < 0.0 ? -1 : 0;
      }

      /// <summary>
      /// Get the exterior angle between this line and <paramref name="otherLine"/>
      /// </summary>
      /// <param name="otherLine">The other line</param>
      /// <returns>The exterior angle between this line and <paramref name="otherLine"/></returns>
      public double GetExteriorAngleDegree(Line2D<T> otherLine)
      {
         double angle1 = Direction.PointDegreeAngle;
         double angle2 = otherLine.Direction.PointDegreeAngle;
         double angle = angle2 - angle1;
         return
             angle <= -180.0 ? angle + 360 :
             angle > 180.0 ? angle - 360 :
             angle;
      }
   }
}
