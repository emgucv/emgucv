//----------------------------------------------------------------------------
//  Copyright (C) 2004-2021 by EMGU Corporation. All rights reserved.       
//----------------------------------------------------------------------------

using System;
using Emgu.CV;

namespace Emgu.CV.Structure
{
   /// <summary> 
   /// Defines a Hls (Hue Lightness Satuation) color
   /// </summary>
   [ColorInfo(ConversionCodename = "Hls")]
   public struct Hls : IColor, IEquatable<Hls>
   {
      /// <summary>
      /// The MCvScalar representation of the color intensity
      /// </summary>
      private MCvScalar _scalar;

      /// <summary> Create a Hls color using the specific values</summary>
      /// <param name="hue"> The hue value for this color ( 0 &lt; hue &lt; 180 )  </param>
      /// <param name="satuation"> The satuation for this color </param>
      /// <param name="lightness"> The lightness for this color </param>
      public Hls(double hue, double lightness, double satuation)
      {
         _scalar = new MCvScalar(hue, lightness, satuation);
      }


      /// <summary> Get or set the intensity of the hue color channel ( 0 &lt; hue &lt; 180 ) </summary>
      [DisplayColor(255, 0, 0)]
      public double Hue { get { return _scalar.V0 ; } set { _scalar.V0 = value; } }

      /// <summary> Get or set the intensity of the lightness color channel </summary>
      [DisplayColor(122, 122, 122)]
      public double Lightness { get { return _scalar.V1; } set { _scalar.V1 = value; } }

      /// <summary> Get or set the intensity of the satuation color channel </summary>
      [DisplayColor(122, 122, 122)]
      public double Satuation { get { return _scalar.V2; } set { _scalar.V2 = value; } }

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


      #region IColor Members
      /// <summary>
      /// Get the dimension of this color
      /// </summary>
      public int Dimension
      {
         get { return 3; }
      }

      /// <summary>
      /// Get or Set the equivalent MCvScalar value
      /// </summary>
      public MCvScalar MCvScalar
      {
         get
         {
            return _scalar;
         }
         set
         {
            _scalar = value;
         }
      }
      #endregion

      /// <summary>
      /// Represent this color as a String
      /// </summary>
      /// <returns>The string representation of this color</returns>
      public override string ToString()
      {
         return String.Format("[{0},{1},{2}]", Hue, Lightness, Satuation);
      }
   }
}
