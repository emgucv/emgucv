using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;
using System.Xml.Serialization;
using System.Runtime.Serialization;
using System.ServiceModel;

namespace Emgu.CV
{
    ///<summary> A multi dimension point</summary>
    ///<typeparam name="T"> The type of value for this point</typeparam>
    [Serializable]
    public class Point<T> : IEquatable<Point<T>> where T : IComparable, new()
    {
        ///<summary> The internal representation of this point as an array</summary>
        protected T[] _coordinate;

        ///<summary> Create a default point of 0 dimension</summary>
        public Point() 
        { 
        }

        ///<summary> Create a point of the specific dimension</summary>
        ///<param name="dimension">The dimension of this point</param>
        public Point(int dimension)
        {
            _coordinate = new T[dimension];
            _coordinate.Initialize();
        }

        ///<summary> Create a point using the specific data</summary>
        ///<param name="data"> The array of data that define this point</param>
        public Point(T[] data)
        {
            _coordinate = data;
        }

        ///<summary> The array representation of this point, <remark>warning: changing the value of this array also change the position of the point </remark></summary>
        [DataMember]
        public T[] Coordinate 
        { 
            get { return _coordinate; } 
            set { _coordinate = value; } 
        }

        ///<summary> The dimension of this point</summary>
        public int Dimension 
        { 
            get { return _coordinate.Length; } 
        }

        /// <summary>
        /// Perform a generic operation between two points and return the result
        /// </summary>
        /// <typeparam name="TOther2">The type of the second point</typeparam>
        /// <typeparam name="TOther3">The type of the resulting point</typeparam>
        /// <param name="p2">The second point to apply generic operation</param>
        /// <param name="convertor">The generic operator</param>
        /// <returns>The result of the generic operation</returns>
        public Point<TOther3> Convert<TOther2, TOther3>(Point<TOther2> p2, Emgu.Utils.Func<T, TOther2, TOther3> convertor)
            where TOther3 : IComparable, new()
            where TOther2 : IComparable, new()
        {
            int d = Dimension;
            Point<TOther3> res = new Point<TOther3>(d);
            for (int i = 0; i < d; i++)
            {
                res[i] = convertor(this[i], p2[i]);
            }
            return res;
        }

        /// <summary>
        /// Multiply the current point with the specific value
        /// </summary>
        /// <param name="value">the value to multiply</param>
        /// <returns>the multiplied point</returns>
        public Point<T> Mul(T value)
        {
            Point<double> pt = Convert<double>();
            double[] coordinate = pt.Coordinate;
            double mul = (double) System.Convert.ChangeType(value, typeof(double));
            for (int i = 0; i < coordinate.Length; i++)
            {
                coordinate[i] *= mul;
            }
            return pt.Convert<T>();
        }

        /// <summary>
        /// Multiply the point with the specific value
        /// </summary>
        /// <param name="point">The point to multiply</param>
        /// <param name="value">the value to multiply</param>
        /// <returns>the multiplication result</returns>
        public static Point<T> operator *(Point<T> point, T value)
        {
            return point.Mul(value);
        }

        /// <summary>
        /// Perform a generic operation between two points and store the result in the first point
        /// </summary>
        /// <typeparam name="TOther">The type of the second point</typeparam>
        /// <param name="p2">The second point to apply generic operation</param>
        /// <param name="convertor">The generic operator</param>
        public void _Convert<TOther>(Point<TOther> p2, Emgu.Utils.Func<T, TOther, T> convertor)
            where TOther: IComparable, new ()
        {
            for (int i = 0; i < _coordinate.Length; i++)
                _coordinate[i] = convertor(_coordinate[i], p2[i]);
        }

        ///<summary> 
        ///substract the current point with another point and returns the result
        ///</summary>
        ///<seealso cref="operator-"></seealso>
        ///<param name="other"> The other point to be added to <i>this</i></param> 
        ///<returns>The sum of the two point</returns>
        public Point<T> Sub<TOther>(Point<TOther> other)
            where TOther: IComparable, new()
        {
            return Convert<TOther, T>(
                other,
                delegate(T v1, TOther v2)
                {
                    return (T)System.Convert.ChangeType(System.Convert.ToDouble(v1) - System.Convert.ToDouble(v2), typeof(T));
                });
        }

        /// <summary>
        /// Subtract <paramref name="other"/> from the current point
        /// </summary>
        /// <typeparam name="TOther">The type of the point to be substracted</typeparam>
        /// <param name="other">The point to be substracted</param>
        public void _Sub<TOther>(Point<TOther> other)
            where TOther : IComparable, new()
        {
            _Convert<TOther>(
                other,
                delegate(T v1, TOther v2)
                {
                    return (T)System.Convert.ChangeType(System.Convert.ToDouble(v1) - System.Convert.ToDouble(v2), typeof(T));
                });
        }

        ///<summary>
        ///Subtract the 2nd point from the 1st point and returns the result
        ///</summary>
        ///<param name="p1"> The point to subtract value from </param>
        ///<param name="p2"> The value to be subtracted from p1 </param>
        public static Point<T> operator - (Point<T> p1, Point<T> p2)
        {
            return p1.Sub(p2);
        }

        ///<summary> Sum the current point with another point and returns the result</summary>
        ///<seealso cref="operator+"></seealso>
        public Point<T> Add<TOther>(Point<TOther> other)
            where TOther: IComparable, new()
        {
            return Convert<TOther, T>(
                other,
                delegate(T v1, TOther v2)
                {
                    return (T)System.Convert.ChangeType(System.Convert.ToDouble(v1) + System.Convert.ToDouble(v2), typeof(T));
                });
        }

        /// <summary>
        /// An the other point to the current point
        /// </summary>
        /// <param name="other">The point to be added to this</param>
        public void _Add<TOther>(Point<TOther> other)
            where TOther : IComparable, new()
        {
            _Convert<TOther>(
                other,
                delegate(T v1, TOther v2)
                {
                    return (T)System.Convert.ChangeType(System.Convert.ToDouble(v1) + System.Convert.ToDouble(v2), typeof(T));
                });
        }

        /// <summary>
        /// Add the 2nd point from the 1st point and returns the result
        /// </summary>
        /// <param name="p1">The point to be added</param>
        /// <param name="p2">The point to be added</param>
        /// <returns>The sum of the points</returns>
        public static Point<T> operator +(Point<T> p1, Point<T> p2)
        {
            return p1.Add(p2);
        }

        ///<summary> Convert this point to the specific type</summary>
        ///<returns> An equavailent point of the specific type</returns> 
        public Point<TOther> Convert<TOther>() where TOther : IComparable, new()
        {
            return new Point<TOther>(
                System.Array.ConvertAll<T, TOther>(
                    _coordinate, 
                    delegate(T val) { return (TOther)System.Convert.ChangeType(val, typeof(TOther)); }));
        }

        ///<summary> The norm of this point. e.g. sqrt(X^2 + Y^2 + ...) </summary>
        public double Norm
        {
            get
            {
                double sqSum = 0.0;
                foreach (T v in _coordinate)
                {
                    double val = System.Convert.ToDouble(v);
                    sqSum += val * val;
                }
                return Math.Sqrt(sqSum);
            }
        }

        ///<summary> Return a normalized point (aka. the direction) </summary>  
        public Point<double> Normalized
        {
            get
            {
                double nor = Norm;
                double[] d = Convert<double>().Coordinate;
                for (int i = 0; i < d.Length; d[i++] /= nor) ;
                return new Point<double>(d);
            }
        }

        /// <summary>
        /// Resize the current point to the specific size, 
        /// if the new size is smaller, perform a truncation,
        /// if the new size is larger, the rest of the space is filled with default value
        /// </summary>
        /// <param name="size">The new size of the point</param>
        /// <returns>The resized point</returns>
        public Point<T> Resize(int size)
        {
            Point<T> res = new Point<T>(size);
            int l = Math.Min(res.Dimension, Dimension);
            for (int i = 0; i < l; i++)
                res[i] = this[i];
            
            return res;
        }

        ///<summary> Return the specific element in this point</summary>
        public T this[int index] 
        { 
            get { return _coordinate[index]; } 
            set { _coordinate[index] = value; } 
        }

        /// <summary>
        /// Compare if the two point have equal dimension and value, if so, return true, otherwise, false
        /// </summary>
        /// <param name="other">The other point to compare with</param>
        /// <returns>true if the two points equal, false otherwise</returns>
        public bool Equals(Point<T> other)
        {
            T[] coor1 = Coordinate;
            T[] coor2 = other.Coordinate;
            if (coor1.Length != coor2.Length) return false;
            for (int i = 0; i < coor1.Length; i++)
            {
                if (coor1[i].CompareTo(coor2[i]) != 0) return false;
            }
            return true;
        }
    }
}
