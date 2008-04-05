using System;
using System.Collections.Generic;
using System.Text;

namespace Emgu.CV
{
    ///<summary> 
    ///Defines a Bgr (Blue Green Red) color
    ///</summary>
    [ColorInfo(ConversionCodeName = "BGR")]
    public class Bgr : ColorType
    {
        ///<summary> Create a RGB color using the specific values</summary>
        ///<param name="blue"> The blue value for this color </param>
        ///<param name="green"> The green value for this color </param>
        ///<param name="red"> The red value for this color </param>
        public Bgr(double blue, double green, double red)
            : base(3)
        {
            _coordinate[0] = blue;
            _coordinate[1] = green;
            _coordinate[2] = red;
        }

        ///<summary> Create a RGB color using the default values (0.0, 0.0, 0.0)</summary>
        public Bgr()
            : base(3)
        { }

        ///<summary> The intensity of the z color channel </summary>
        public double B { get { return _coordinate[0]; } set { _coordinate[0] = value; } }
        ///<summary> The intensity of the y color channel </summary>
        public double G { get { return _coordinate[1]; } set { _coordinate[1] = value; } }
        ///<summary> The intensity of the x color channel </summary>
        public double R { get { return _coordinate[2]; } set { _coordinate[2] = value; } }
    };
}
