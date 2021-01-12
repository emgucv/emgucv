//----------------------------------------------------------------------------
//  Copyright (C) 2004-2021 by EMGU Corporation. All rights reserved.       
//----------------------------------------------------------------------------

using System;
using Emgu.CV;

namespace Emgu.CV.Structure
{
   /// <summary> Defines a Gray color </summary>
   [ColorInfo(ConversionCodename = "Gray")]
   public struct Gray : IColor, IEquatable<Gray>
   {
      /// <summary>
      /// The MCvScalar representation of the color intensity
      /// </summary>
      private MCvScalar _scalar;

      /// <summary> Create a Gray color with the given intensity</summary>
      /// <param name="intensity"> The intensity for this color </param>
      public Gray(double intensity)
      {
         _scalar = new MCvScalar(intensity);
      }

      /// <summary> The intensity of the gray color </summary>
      /// <value> The intensity of the gray color</value>
      [DisplayColor(122, 122, 122)]
      public double Intensity { get { return _scalar.V0; } set { _scalar.V0 = value; } }

      /// <summary>
      /// Returns the hash code for this color
      /// </summary>
      /// <returns>the hash code</returns>
      public override int GetHashCode()
      {
         return Intensity.GetHashCode();
      }

      #region IEquatable<Gray> Members
      /// <summary>
      /// Return true if the two color equals
      /// </summary>
      /// <param name="other">The other color to compare with</param>
      /// <returns>true if the two color equals</returns>
      public bool Equals(Gray other)
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
         get { return 1; }
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
         return String.Format("[{0}]", Intensity);
      }
   }
}
