using System;
using Emgu.CV;

namespace Emgu.CV.Structure
{
   ///<summary> 
   ///Defines a Bgr (Blue Green Red) color
   ///</summary>
   [ColorInfo(ConversionCodename = "BGR")]
   public struct Bgr: IColor, IEquatable<Bgr>
   {
      /// <summary>
      /// The MCvScalar representation of the color intensity
      /// </summary>
      private MCvScalar _scalar;

      ///<summary> Create a BGR color using the specific values</summary>
      ///<param name="blue"> The blue value for this color </param>
      ///<param name="green"> The green value for this color </param>
      ///<param name="red"> The red value for this color </param>
      public Bgr(double blue, double green, double red)
      {
         _scalar = new MCvScalar(blue, green, red);
      }

      /// <summary>
      /// Create a Bgr color using the System.Drawing.Color
      /// </summary>
      /// <param name="winColor">System.Drawing.Color</param>
      public Bgr(System.Drawing.Color winColor)
      {
         _scalar = new MCvScalar(winColor.B, winColor.G, winColor.R);
      }

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
         return String.Format("[{0},{1},{2}]", Blue, Green, Red);
      }

      #region Operators; Contribution from yoavhacohen
      /// <summary>
      /// Return the sum of two color
      /// </summary>
      /// <param name="a">The first color</param>
      /// <param name="b">The second color</param>
      /// <returns>The sum of two color</returns>
      public static Bgr operator +(Bgr a, Bgr b)
      {
         return new Bgr(
             a.Blue + b.Blue,
             a.Green + b.Green,
             a.Red + b.Red);
      }

      /// <summary>
      /// Return the different of two color
      /// </summary>
      /// <param name="a">The first color</param>
      /// <param name="b">The second color</param>
      /// <returns>The different of two color</returns>
      public static Bgr operator -(Bgr a, Bgr b)
      {
         return new Bgr(
             a.Blue - b.Blue,
             a.Green - b.Green,
             a.Red - b.Red);
      }

      /// <summary>
      /// Elementwise divide the first color by the second color
      /// </summary>
      /// <param name="a">The first color</param>
      /// <param name="b">The second color</param>
      /// <returns>The result of elementwise dividing the first color by the second color</returns>
      public static Bgr operator /(Bgr a, Bgr b)
      {
         return new Bgr(
             a.Blue / b.Blue,
             a.Green / b.Green,
             a.Red / b.Red);
      }

      /// <summary>
      /// Elementwise multiply the first color by the second color
      /// </summary>
      /// <param name="a">The first color</param>
      /// <param name="b">The second color</param>
      /// <returns>The result of elementwise multiplying the first color by the second color</returns>
      public static Bgr operator *(Bgr a, Bgr b)
      {
         return new Bgr(
             a.Blue * b.Blue,
             a.Green * b.Green,
             a.Red * b.Red);
      }

      /// <summary>
      /// Multiply the color by a scalar
      /// </summary>
      /// <param name="a">The color</param>
      /// <param name="scalar">The scalar</param>
      /// <returns>The result of multiplying the color by a scalar</returns>
      public static Bgr operator *(Bgr a, double scalar)
      {
         return new Bgr(
             a.Blue * scalar,
             a.Green * scalar,
             a.Red * scalar);
      }

      /// <summary>
      /// Divide the color by a scalar
      /// </summary>
      /// <param name="a">The color</param>
      /// <param name="scalar">The scalar</param>
      /// <returns>The result of dividing the color by a scalar</returns>
      public static Bgr operator /(Bgr a, double scalar)
      {
         return new Bgr(
             a.Blue / scalar,
             a.Green / scalar,
             a.Red / scalar);
      }

      /// <summary>
      /// Convert a scalar to a Bgr color
      /// </summary>
      /// <param name="scalar">The scalar</param>
      /// <returns>The Bgr color</returns>
      public static implicit operator Bgr(double scalar)
      {
         return new Bgr(scalar, scalar, scalar);
      }
      #endregion
   }
}
