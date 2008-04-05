using System;
using System.Collections.Generic;
using System.Text;

namespace Emgu.CV
{
    ///<summary> 
    ///Defines a Ycc color (YCrCb JPEG)
    ///</summary>
    [ColorInfo(ConversionCodeName = "YCrCb")]
    public class Ycc : ColorType
    {
        ///<summary> Create a Ycc color using the specific values</summary>
        ///<param name="y"> The Y value for this color </param>
        ///<param name="cr"> The Cr value for this color </param>
        ///<param name="cb"> The Cb value for this color </param>
        public Ycc(double y, double cr, double cb)
            : base(3)
        {
            _coordinate[0] = y;
            _coordinate[1] = cr;
            _coordinate[2] = cb;
        }

        ///<summary> Create a Ycc color using the default values (0.0, 0.0, 0.0)</summary>
        public Ycc()
            : base(3)
        { }

        ///<summary> The intensity of the Y color channel </summary>
        public double Y { get { return _coordinate[0]; } set { _coordinate[0] = value; } }
        ///<summary> The intensity of the Cr color channel </summary>
        public double Cr { get { return _coordinate[1]; } set { _coordinate[1] = value; } }
        ///<summary> The intensity of the Cb color channel </summary>
        public double Cb { get { return _coordinate[2]; } set { _coordinate[2] = value; } }
    };
}
