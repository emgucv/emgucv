using System;
using System.Collections.Generic;
using System.Text;

namespace Emgu.CV
{
    ///<summary> 
    ///Defines a CIE Luv color 
    ///</summary>
    [ColorInfo(ConversionCodeName = "Luv")]
    public class Luv : ColorType
    {
        ///<summary> Create a CIE Lab color using the specific values</summary>
        ///<param name="z"> The z value for this color </param>
        ///<param name="y"> The y value for this color </param>
        ///<param name="x"> The x value for this color </param>
        public Luv(double x, double y, double z)
            : base(3)
        {
            _coordinate[0] = x;
            _coordinate[1] = y;
            _coordinate[2] = z;
        }

        ///<summary> Create a CIE Luv color using the default values (0.0, 0.0, 0.0)</summary>
        public Luv()
            : base(3)
        { }

        ///<summary> The intensity of the z color channel </summary>
        public double X { get { return _coordinate[0]; } set { _coordinate[0] = value; } }
        ///<summary> The intensity of the y color channel </summary>
        public double Y { get { return _coordinate[1]; } set { _coordinate[1] = value; } }
        ///<summary> The intensity of the x color channel </summary>
        public double Z { get { return _coordinate[2]; } set { _coordinate[2] = value; } }
    };
}
