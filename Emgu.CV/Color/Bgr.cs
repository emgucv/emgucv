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
      ///<summary> Create a BGR color using the specific values</summary>
      ///<param name="blue"> The blue value for this color </param>
      ///<param name="green"> The green value for this color </param>
      ///<param name="red"> The red value for this color </param>
      public Bgr(double blue, double green, double red)
         : this()
      {
         _coordinate[0] = blue;
         _coordinate[1] = green;
         _coordinate[2] = red;
      }

      /// <summary>
      /// Create a Bgr color using the System.Drawing.Color
      /// </summary>
      /// <param name="winColor">System.Drawing.Color</param>
      public Bgr(System.Drawing.Color winColor)
         : this()
      {
         _coordinate[0] = winColor.B;
         _coordinate[1] = winColor.G;
         _coordinate[2] = winColor.R;
      }

      ///<summary> Create a BGR color using the default values (0.0, 0.0, 0.0)</summary>
      public Bgr()
         : base(3)
      { }

      ///<summary> Get or set the intensity of the blue color channel </summary>
      [DisplayColor(255, 0, 0)]
      public double Blue { get { return _coordinate[0]; } set { _coordinate[0] = value; } }

      ///<summary> Get or set the intensity of the green color channel </summary>
      [DisplayColor(0, 255, 0)]
      public double Green { get { return _coordinate[1]; } set { _coordinate[1] = value; } }

      ///<summary> Get or set the intensity of the reg color channel </summary>
      [DisplayColor(0, 0, 255)]
      public double Red { get { return _coordinate[2]; } set { _coordinate[2] = value; } }

   }
}
