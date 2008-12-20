using System;
using System.Collections.Generic;
using System.Text;
using Emgu.CV.Structure;

namespace Emgu.CV
{
   ///<summary> 
   ///Defines a Bgr (Blue Green Red) color
   ///</summary>
   [ColorInfo(ConversionCodename = "BGR")]
   public class Bgr : ColorType, IEquatable<Bgr>
   {
      ///<summary> Create a BGR color using the specific values</summary>
      ///<param name="blue"> The blue value for this color </param>
      ///<param name="green"> The green value for this color </param>
      ///<param name="red"> The red value for this color </param>
      public Bgr(double blue, double green, double red)
         : base(new MCvScalar(blue, green, red))
      {
      }

      /// <summary>
      /// Create a Bgr color using the System.Drawing.Color
      /// </summary>
      /// <param name="winColor">System.Drawing.Color</param>
      public Bgr(System.Drawing.Color winColor)
         : base(new MCvScalar(winColor.B, winColor.G, winColor.R))
      {
      }

      ///<summary> Create a BGR color using the default values (0.0, 0.0, 0.0)</summary>
      public Bgr()
         : base()
      { }

      ///<summary> Get or set the intensity of the blue color channel </summary>
      [DisplayColor(255, 0, 0)]
      public double Blue { get { return _scalar.v0; } set { _scalar.v0 = value; } }

      ///<summary> Get or set the intensity of the green color channel </summary>
      [DisplayColor(0, 255, 0)]
      public double Green { get { return _scalar.v1; } set { _scalar.v1 = value; } }

      ///<summary> Get or set the intensity of the red color channel </summary>
      [DisplayColor(0, 0, 255)]
      public double Red { get { return _scalar.v2; } set { _scalar.v2 = value; } }


      #region IEquatable<Bgr> Members
      /// <summary>
      /// Return true if the two color equals
      /// </summary>
      /// <param name="other">The other color to compare with</param>
      /// <returns>true if the two color equals</returns>
      public bool Equals(Bgr other)
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
