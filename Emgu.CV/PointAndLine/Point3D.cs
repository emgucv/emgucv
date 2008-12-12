using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Serialization;
using System.Xml.Serialization;
using System.Diagnostics;

namespace Emgu.CV
{
   ///<summary> A 3D point </summary>
   ///<typeparam name="T"> The type of value for this 3D point</typeparam>
   [Serializable]
   public class Point3D<T> : Point2D<T> where T : struct, IComparable
   {
      /// <summary>
      /// Create a 3D point using the specific coordinate
      /// </summary>
      /// <param name="coordinate">The coordinate for this point</param>
      public Point3D(T[] coordinate)
      {
         Debug.Assert(coordinate.Length == 3, "The coordinate must have a length of 3");
         _coordinate = coordinate;
      }

      ///<summary> Create a 3D point located in the origin</summary>
      public Point3D()
      {
         _coordinate = new T[3];
      }

      ///<summary> Create a 3D point of the specific location</summary>
      ///<param name="x"> The x value of this point</param>
      ///<param name="y"> The y value of this point</param>
      ///<param name="z"> The z value of this point</param>
      public Point3D(T x, T y, T z)
      {
         _coordinate = new T[3] { x, y, z };
      }

      ///<summary> The z value of this point</summary>
      [XmlIgnore]
      public T Z { get { return _coordinate[2]; } set { _coordinate[2] = value; } }

      /// <summary>
      /// Return the cross product of this point with <paramref name="point2"/>
      /// </summary>
      /// <param name="point2">The other point to apply cross product with</param>
      /// <returns>The cross product of the two point</returns>
      public Point3D<T> CrossProduct(Point3D<T> point2)
      {
         using (Matrix<T> m1 = new Matrix<T>(this.Coordinate))
         using (Matrix<T> m2 = new Matrix<T>(point2.Coordinate))
         using (Matrix<T> m3 = new Matrix<T>(3, 1))
         {
            CvInvoke.cvCrossProduct(m1.Ptr, m2.Ptr, m3.Ptr);
            return new Point3D<T>(m3[0, 0], m3[1, 0], m3[2, 0]);
         }
      }

      /// <summary> 
      /// Return a normalized point (aka. the direction) 
      /// </summary>  
      public new Point3D<double> Normalized
      {
         get
         {
            return new Point3D<double>(base.Normalized.Coordinate);
         }
      }
   }
}
