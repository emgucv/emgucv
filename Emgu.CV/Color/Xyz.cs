//----------------------------------------------------------------------------
//  Copyright (C) 2004-2015 by EMGU Corporation. All rights reserved.       
//----------------------------------------------------------------------------

using System;
using Emgu.CV;

namespace Emgu.CV.Structure
{
   ///<summary> 
   ///Defines a Xyz color (CIE XYZ.Rec 709 with D65 white point)
   ///</summary>
   [ColorInfo(ConversionCodename = "Xyz")]
   public struct Xyz : IColor, IEquatable<Xyz>
   {
      /// <summary>
      /// The MCvScalar representation of the color intensity
      /// </summary>
      private MCvScalar _scalar;

      ///<summary> Create a Xyz color using the specific values</summary>
      ///<param name="z"> The z value for this color </param>
      ///<param name="y"> The y value for this color </param>
      ///<param name="x"> The x value for this color </param>
      public Xyz(double x, double y, double z)
      {
         _scalar = new MCvScalar(x, y, z);
      }

      ///<summary> Get or set the intensity of the z color channel </summary>
      [DisplayColor(122, 122, 122)]
      public double X { get { return _scalar.V0; } set { _scalar.V0 = value; } }

      ///<summary> Get or set the intensity of the y color channel </summary>
      [DisplayColor(122, 122, 122)]
      public double Y { get { return _scalar.V1; } set { _scalar.V1 = value; } }

      ///<summary> Get or set the intensity of the x color channel </summary>
      [DisplayColor(122, 122, 122)]
      public double Z { get { return _scalar.V2; } set { _scalar.V2 = value; } }

      #region IEquatable<Xyz> Members
      /// <summary>
      /// Return true if the two color equals
      /// </summary>
      /// <param name="other">The other color to compare with</param>
      /// <returns>true if the two color equals</returns>
      public bool Equals(Xyz other)
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
         return String.Format("[{0},{1},{2}]", X, Y, Z);
      }
   }
}
