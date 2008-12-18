using System;
using System.Collections.Generic;
using System.Text;
using Emgu.CV.Structure;

namespace Emgu.CV
{
   ///<summary> 
   ///Defines a CIE Luv color 
   ///</summary>
   [ColorInfo(ConversionCodename = "Luv")]
   public class Luv : ColorType, IEquatable<Luv>
   {
      ///<summary> Create a CIE Lab color using the specific values</summary>
      ///<param name="z"> The z value for this color </param>
      ///<param name="y"> The y value for this color </param>
      ///<param name="x"> The x value for this color </param>
      public Luv(double x, double y, double z)
         : base(3, new MCvScalar(x, y, z))
      {
      }

      ///<summary> Create a CIE Luv color using the default values (0.0, 0.0, 0.0)</summary>
      public Luv()
         : base(3)
      { }

      ///<summary> The intensity of the x color channel </summary>
      [DisplayColor(122, 122, 122)]
      public double X { get { return _scalar.v0; } set { _scalar.v0 = value; } }

      ///<summary> The intensity of the y color channel </summary>
      [DisplayColor(122, 122, 122)]
      public double Y { get { return _scalar.v1; } set { _scalar.v1 = value; } }

      ///<summary> The intensity of the z color channel </summary>
      [DisplayColor(122, 122, 122)]
      public double Z { get { return _scalar.v2; } set { _scalar.v2 = value; } }

      #region IEquatable<Luv> Members
      /// <summary>
      /// Return true if the two color equals
      /// </summary>
      /// <param name="other">The other color to compare with</param>
      /// <returns>true if the two color equals</returns>
      public bool Equals(Luv other)
      {
         return MCvScalar.Equals(other.MCvScalar);
      }

      #endregion
   }
}
