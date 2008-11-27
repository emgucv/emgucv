using System;
using System.Collections.Generic;
using System.Text;

namespace Emgu.CV
{
   ///<summary> 
   ///Defines a CIE Lab color 
   ///</summary>
   [ColorInfo(ConversionCodename = "Lab")]
   public class Lab : ColorType
   {
      ///<summary> Create a CIE Lab color using the specific values</summary>
      ///<param name="z"> The z value for this color </param>
      ///<param name="y"> The y value for this color </param>
      ///<param name="x"> The x value for this color </param>
      public Lab(double x, double y, double z)
         : this()
      {
         _coordinate[0] = x;
         _coordinate[1] = y;
         _coordinate[2] = z;
      }

      ///<summary> Create a CIE Lab color using the default values (0.0, 0.0, 0.0)</summary>
      public Lab()
         : base(3)
      { }

      ///<summary> Get or set the intensity of the x color channel </summary>
      [DisplayColor(122, 122, 122)]
      public double X { get { return _coordinate[0]; } set { _coordinate[0] = value; } }

      ///<summary> Get or set the intensity of the y color channel </summary>
      [DisplayColor(122, 122, 122)]
      public double Y { get { return _coordinate[1]; } set { _coordinate[1] = value; } }

      ///<summary> Get or set the intensity of the z color channel </summary>
      [DisplayColor(122, 122, 122)]
      public double Z { get { return _coordinate[2]; } set { _coordinate[2] = value; } }
   }
}
