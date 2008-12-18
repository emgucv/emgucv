using System;
using System.Collections.Generic;
using System.Text;
using Emgu.CV.Structure;

namespace Emgu.CV
{
   ///<summary> 
   ///Defines a Hls (Hue Lightness Satuation) color
   ///</summary>
   [ColorInfo(ConversionCodename = "HLS")]
   public class Hls : ColorType, IEquatable<Hls>
   {
      ///<summary> Create a Hls color using the specific values</summary>
      ///<param name="hue"> The hue value for this color ( 0 &lt; hue &lt; 180 )  </param>
      ///<param name="satuation"> The satuation for this color </param>
      ///<param name="lightness"> The lightness for this color </param>
      public Hls(double hue, double lightness, double satuation)
         : base(3, new MCvScalar(hue, lightness, satuation))
      {
      }

      ///<summary> Create a Hls color using the default values (0.0, 0.0, 0.0)</summary>
      public Hls()
         : base(3)
      { }

      ///<summary> Get or set the intensity of the hue color channel ( 0 &lt; hue &lt; 180 ) </summary>
      [DisplayColor(255, 0, 0)]
      public double Hue { get { return _scalar.v0 ; } set { _scalar.v0 = value; } }

      ///<summary> Get or set the intensity of the lightness color channel </summary>
      [DisplayColor(122, 122, 122)]
      public double Lightness { get { return _scalar.v1; } set { _scalar.v1 = value; } }

      ///<summary> Get or set the intensity of the satuation color channel </summary>
      [DisplayColor(122, 122, 122)]
      public double Satuation { get { return _scalar.v2; } set { _scalar.v2 = value; } }

      #region IEquatable<Hls> Members
      /// <summary>
      /// Return true if the two color equals
      /// </summary>
      /// <param name="other">The other color to compare with</param>
      /// <returns>true if the two color equals</returns>
      public bool Equals(Hls other)
      {
         return MCvScalar.Equals(other.MCvScalar);
      }

      #endregion
   }
}
