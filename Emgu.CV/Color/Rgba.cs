//----------------------------------------------------------------------------
//  Copyright (C) 2004-2015 by EMGU Corporation. All rights reserved.       
//----------------------------------------------------------------------------

using System;
using Emgu.CV;

namespace Emgu.CV.Structure
{
   ///<summary> 
   ///Defines a Rgba (Red Green Blue Alpha) color
   ///</summary>
   [ColorInfo(ConversionCodename = "Rgba")]
   public struct Rgba : IColor, IEquatable<Rgba>
   {
      /// <summary>
      /// The MCvScalar representation of the color intensity
      /// </summary>
      private MCvScalar _scalar;

      ///<summary> Create a RGBA color using the specific values</summary>
      ///<param name="blue"> The blue value for this color </param>
      ///<param name="green"> The green value for this color </param>
      ///<param name="red"> The red value for this color </param>
      ///<param name="alpha"> The alpha value for this color</param>
      public Rgba(double red, double green, double blue, double alpha)
      {
         _scalar = new MCvScalar(red, green, blue, alpha);
      }

      ///<summary> Get or set the intensity of the red color channel </summary>
      [DisplayColor(0, 0, 255)]
      public double Red { get { return _scalar.V0; } set { _scalar.V0 = value; } }

      ///<summary> Get or set the intensity of the green color channel </summary>
      [DisplayColor(0, 255, 0)]
      public double Green { get { return _scalar.V1; } set { _scalar.V1 = value; } }

      ///<summary> Get or set the intensity of the blue color channel </summary>
      [DisplayColor(255, 0, 0)]
      public double Blue { get { return _scalar.V2; } set { _scalar.V2 = value; } }

      ///<summary> Get or set the intensity of the alpha color channel </summary>
      [DisplayColor(122, 122, 122)]
      public double Alpha { get { return _scalar.V3; } set { _scalar.V3 = value; } }

      #region IEquatable<Bgra> Members

      /// <summary>
      /// Return true if the two color equals
      /// </summary>
      /// <param name="other">The other color to compare with</param>
      /// <returns>true if the two color equals</returns>
      public bool Equals(Rgba other)
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
         get { return 4; }
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
         return String.Format("[{0},{1},{2},{3}]", Red, Green, Blue, Alpha);
      }
   }
}
