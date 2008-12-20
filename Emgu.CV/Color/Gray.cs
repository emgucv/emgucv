using System;
using System.Collections.Generic;
using System.Text;
using Emgu.CV.Structure;

namespace Emgu.CV
{
   ///<summary> Defines a Gray color </summary>
   [ColorInfo(ConversionCodename = "GRAY")]
   public class Gray : ColorType, IComparable, IEquatable<Gray>
   {
      ///<summary> Create a Gray color with the given intensity</summary>
      ///<param name="intensity"> The intensity for this color </param>
      public Gray(double intensity)
         : base(new MCvScalar(intensity))
      {
      }

      ///<summary> Create a Gray color with o intensity </summary>
      public Gray()
         : base()
      {
      }

      ///<summary> The intensity of the gray color </summary>
      ///<value> The intensity of the gray color</value>
      [DisplayColor(122, 122, 122)]
      public double Intensity { get { return _scalar.v0; } set { _scalar.v0 = value; } }

      /// <summary>
      /// Check if two gray color are equal
      /// </summary>
      /// <param name="obj">the object to compare with</param>
      /// <returns>true if <paramref name="obj"/> is Gray color with the same intensity</returns>
      public override bool Equals(object obj)
      {
         Gray other = obj as Gray;
         if (other == null) return false;
         return Intensity.Equals(other.Intensity);
      }

      /// <summary>
      /// Returns the hash code for this color
      /// </summary>
      /// <returns>the hash code</returns>
      public override int GetHashCode()
      {
         return Intensity.GetHashCode();
      }

      /// <summary>
      /// Compare method that implement IComparable interface
      /// </summary>
      /// <param name="obj">The other object to compare to</param>
      /// <returns>a positive value if greater, 0 if equal, negative value if smaller</returns>
      public virtual int CompareTo(System.Object obj)
      {
         Gray gray = obj as Gray;
         if (gray != null)
         {
            return Intensity.CompareTo(gray.Intensity);
         }
         throw new ArgumentException("object is not Gray");
      }

      #region IEquatable<Gray> Members
      /// <summary>
      /// Return true if the two color equals
      /// </summary>
      /// <param name="other">The other color to compare with</param>
      /// <returns>true if the two color equals</returns>
      public bool Equals(Gray other)
      {
         return MCvScalar.Equals(other);
      }

      #endregion
      /// <summary>
      /// Get the dimension of this color
      /// </summary>
      public override int Dimension
      {
         get { return 1; }
      }
   }
}
