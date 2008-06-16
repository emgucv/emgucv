using System;
using System.Collections.Generic;
using System.Text;

namespace Emgu.CV
{
    ///<summary> 
    ///Defines a Bgra (Blue Green Red Alpha) color
    ///</summary>
    [ColorInfo(ConversionCodeName = "BGRA")]
    public class Bgra : ColorType
    {
        /// <summary>
        /// Create a BGRA color using the specific values
        /// </summary>
        public Bgra()
            : base(4)
        {
        }

        ///<summary> Create a BGRA color using the specific values</summary>
        ///<param name="blue"> The blue value for this color </param>
        ///<param name="green"> The green value for this color </param>
        ///<param name="red"> The red value for this color </param>
        ///<param name="alpha"> The alpha value for this color</param>
        public Bgra(double blue, double green, double red, double alpha)
            : this()
        {
            _coordinate[0] = blue;
            _coordinate[1] = green;
            _coordinate[2] = red;
            _coordinate[3] = alpha;
        }

        ///<summary> The intensity of the B color channel </summary>
        public double B { get { return _coordinate[0]; } set { _coordinate[0] = value; } }
        ///<summary> The intensity of the G color channel </summary>
        public double G { get { return _coordinate[1]; } set { _coordinate[1] = value; } }
        ///<summary> The intensity of the R color channel </summary>
        public double R { get { return _coordinate[2]; } set { _coordinate[2] = value; } }
        ///<summary> The intensity of the A color channel </summary>
        public double A { get { return _coordinate[3]; } set { _coordinate[3] = value; } }

    }
}
