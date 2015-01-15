//----------------------------------------------------------------------------
//  Copyright (C) 2004-2015 by EMGU Corporation. All rights reserved.       
//----------------------------------------------------------------------------

using System;
using Emgu.CV;

#if UNITY_ANDROID || UNITY_IPHONE || UNITY_STANDALONE || UNITY_METRO
using UnityEngine;
#elif NETFX_CORE
using Windows.UI;
#else
using System.Drawing;
#endif

namespace Emgu.CV.Structure
{
   ///<summary> 
   ///Defines a Rgb (Red Green Blue) color
   ///</summary>
   [ColorInfo(ConversionCodename = "Rgb")]
   public struct Rgb : IColor, IEquatable<Rgb>
   {
      /// <summary>
      /// The MCvScalar representation of the color intensity
      /// </summary>
      private MCvScalar _scalar;

      ///<summary> Create a RGB color using the specific values</summary>
      ///<param name="blue"> The blue value for this color </param>
      ///<param name="green"> The green value for this color </param>
      ///<param name="red"> The red value for this color </param>
      public Rgb(double red, double green, double blue)
      {
         _scalar = new MCvScalar(red, green, blue);
      }

      /// <summary>
      /// Create a Rgb color using the System.Drawing.Color
      /// </summary>
      /// <param name="winColor">System.Drawing.Color</param>
      public Rgb(Color winColor)
#if ( UNITY_ANDROID || UNITY_IPHONE || UNITY_STANDALONE || UNITY_METRO )
         : this(winColor.r * 255.0, winColor.g * 255.0, winColor.b * 255.0)
#else
         : this(winColor.R, winColor.G, winColor.B)
#endif
      {
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

      #region IEquatable<Rgb> Members
      /// <summary>
      /// Return true if the two color equals
      /// </summary>
      /// <param name="other">The other color to compare with</param>
      /// <returns>true if the two color equals</returns>
      public bool Equals(Rgb other)
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
         return String.Format("[{0},{1},{2}]", Red, Green, Blue);
      }
   }
}
