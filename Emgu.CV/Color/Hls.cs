using System;
using System.Collections.Generic;
using System.Text;

namespace Emgu.CV
{
    ///<summary> 
    ///Defines a Hls (Hue Lightness Satuation) color
    ///</summary>
    [ColorInfo(ConversionCodeName = "HLS")]
    public class Hls : ColorType
    {
        ///<summary> Create a Hls color using the specific values</summary>
        ///<param name="hue"> The hue value for this color ( 0 &lt; hue &lt; 180 )  </param>
        ///<param name="satuation"> The satuation for this color </param>
        ///<param name="lightness"> The lightness for this color </param>
        public Hls(double hue, double lightness, double satuation)
            : base(3)
        {
            _coordinate[0] = hue;
            _coordinate[1] = lightness;
            _coordinate[2] = satuation;
        }

        ///<summary> Create a Hls color using the default values (0.0, 0.0, 0.0)</summary>
        public Hls()
            : base(3)
        { }

        ///<summary> The intensity of the hue color channel ( 0 &lt; hue &lt; 180 ) </summary>
        public double H { get { return _coordinate[0]; } set { _coordinate[0] = value; } }

        ///<summary> The intensity of the lightness color channel </summary>
        public double L { get { return _coordinate[1]; } set { _coordinate[1] = value; } }

        ///<summary> The intensity of the satuation color channel </summary>
        public double S { get { return _coordinate[2]; } set { _coordinate[2] = value; } }
    }
}
