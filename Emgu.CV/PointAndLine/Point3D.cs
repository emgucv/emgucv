using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Serialization;
using System.Xml.Serialization;

namespace Emgu.CV
{
    ///<summary> A 3D point </summary>
    ///<typeparam name="T"> The type of value for this 3D point</typeparam>
    [DataContract]
    public class Point3D<T> : Point2D<T> where T : IComparable, new()
    {

        ///<summary> Create a 3D point located in the origin</summary>
        public Point3D()
        {
            _coordinate = new T[3];
            //initialize the data
            _coordinate.Initialize();
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

    };
}
