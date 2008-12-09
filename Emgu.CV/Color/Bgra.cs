using System;
using System.Collections.Generic;
using System.Text;

namespace Emgu.CV
{
   ///<summary> 
   ///Defines a Bgra (Blue Green Red Alpha) color
   ///</summary>
   [ColorInfo(ConversionCodename = "BGRA")]
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

      ///<summary> Get or set the intensity of the blue color channel </summary>
      [DisplayColor(255, 0, 0)]
      public double Blue { get { return _coordinate[0]; } set { _coordinate[0] = value; } }

      ///<summary> Get or set the intensity of the green color channel </summary>
      [DisplayColor(0, 255, 0)]
      public double Green { get { return _coordinate[1]; } set { _coordinate[1] = value; } }

      ///<summary> Get or set the intensity of the red color channel </summary>
      [DisplayColor(0, 0, 255)]
      public double Red { get { return _coordinate[2]; } set { _coordinate[2] = value; } }

      ///<summary> Get or set the intensity of the alpha color channel </summary>
      [DisplayColor(122, 122, 122)]
      public double Alpha { get { return _coordinate[3]; } set { _coordinate[3] = value; } }

   }
}
