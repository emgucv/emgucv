using System;
using System.Collections.Generic;
using System.Text;
using Emgu.CV.Structure;

namespace Emgu.CV
{
   ///<summary> 
   ///Defines a Ycc color (YCrCb JPEG)
   ///</summary>
   [ColorInfo(ConversionCodename = "YCrCb")]
   public class Ycc : ColorType, IEquatable<Ycc>
   {
      ///<summary> Create a Ycc color using the specific values</summary>
      ///<param name="y"> The Y value for this color </param>
      ///<param name="cr"> The Cr value for this color </param>
      ///<param name="cb"> The Cb value for this color </param>
      public Ycc(double y, double cr, double cb)
         : base(3, new MCvScalar(y, cr, cb))
      {
      }

      ///<summary> Create a Ycc color using the default values (0.0, 0.0, 0.0)</summary>
      public Ycc()
         : base(3)
      { }

      ///<summary> Get or set the intensity of the Y color channel </summary>
      [DisplayColor(122, 122, 122)]
      public double Y { get { return _scalar.v0; } set { _scalar.v0 = value; } }

      ///<summary> Get or set the intensity of the Cr color channel </summary>
      [DisplayColor(122, 122, 122)]
      public double Cr { get { return _scalar.v1; } set { _scalar.v1 = value; } }

      ///<summary> Get or set the intensity of the Cb color channel </summary>
      [DisplayColor(122, 122, 122)]
      public double Cb { get { return _scalar.v2; } set { _scalar.v2 = value; } }

      #region IEquatable<Ycc> Members
      /// <summary>
      /// Return true if the two color equals
      /// </summary>
      /// <param name="other">The other color to compare with</param>
      /// <returns>true if the two color equals</returns>
      public bool Equals(Ycc other)
      {
         return MCvScalar.Equals(other.MCvScalar);
      }

      #endregion
   }
}
