using System;
using System.Collections.Generic;
using System.Text;
using Emgu.CV.Structure;

namespace Emgu.CV
{
   ///<summary> 
   ///Defines a HSV (Hue Satuation Value) color
   ///</summary>
   [ColorInfo(ConversionCodename = "HSV")]
   public class Hsv : ColorType, IEquatable<Hsv>
   {
      ///<summary> Create a HSV color using the specific values</summary>
      ///<param name="hue"> The hue value for this color ( 0 &lt; hue &lt; 180 )  </param>
      ///<param name="satuation"> The satuation value for this color </param>
      ///<param name="value"> The value for this color </param>
      public Hsv(double hue, double satuation, double value)
         : base(new MCvScalar(hue, satuation, value))
      {
      }

      ///<summary> Create a HSV color using the default values (0.0, 0.0, 0.0)</summary>
      public Hsv()
         : base()
      { }

      ///<summary> Get or set the intensity of the hue color channel ( 0 &lt; hue &lt; 180 ) </summary>
      [DisplayColor(122, 122, 122)]
      public double H { get { return _scalar.v0; } set { _scalar.v0 = value; } }

      ///<summary> Get or set the intensity of the satuation color channel </summary>
      [DisplayColor(122, 122, 122)]
      public double S { get { return _scalar.v1; } set { _scalar.v1 = value; } }

      ///<summary> Get or set the intensity of the value color channel </summary>
      [DisplayColor(122, 122, 122)]
      public double V { get { return _scalar.v2; } set { _scalar.v2 = value; } }


      #region IEquatable<Hsv> Members
      /// <summary>
      /// Return true if the two color equals
      /// </summary>
      /// <param name="other">The other color to compare with</param>
      /// <returns>true if the two color equals</returns>
      public bool Equals(Hsv other)
      {
         return MCvScalar.Equals(other.MCvScalar);
      }

      #endregion
      /// <summary>
      /// Get the dimension of this color
      /// </summary>
      public override int Dimension
      {
         get { return 3; }
      }
   }
}
