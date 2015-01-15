//----------------------------------------------------------------------------
//  Copyright (C) 2004-2015 by EMGU Corporation. All rights reserved.       
//----------------------------------------------------------------------------

using System;
using Emgu.CV;

namespace Emgu.CV.Structure
{
   ///<summary> 
   ///Defines a Ycc color (YCrCb JPEG)
   ///</summary>
   [ColorInfo(ConversionCodename = "YCrCb")]
   public struct Ycc : IColor, IEquatable<Ycc>
   {
      /// <summary>
      /// The MCvScalar representation of the color intensity
      /// </summary>
      private MCvScalar _scalar;

      ///<summary> Create a Ycc color using the specific values</summary>
      ///<param name="y"> The Y value for this color </param>
      ///<param name="cr"> The Cr value for this color </param>
      ///<param name="cb"> The Cb value for this color </param>
      public Ycc(double y, double cr, double cb)
      {
         _scalar = new MCvScalar(y, cr, cb);
      }

      ///<summary> Get or set the intensity of the Y color channel </summary>
      [DisplayColor(122, 122, 122)]
      public double Y { get { return _scalar.V0; } set { _scalar.V0 = value; } }

      ///<summary> Get or set the intensity of the Cr color channel </summary>
      [DisplayColor(122, 122, 122)]
      public double Cr { get { return _scalar.V1; } set { _scalar.V1 = value; } }

      ///<summary> Get or set the intensity of the Cb color channel </summary>
      [DisplayColor(122, 122, 122)]
      public double Cb { get { return _scalar.V2; } set { _scalar.V2 = value; } }

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
         return String.Format("[{0},{1},{2}]", Y, Cr, Cb);
      }
   }
}
