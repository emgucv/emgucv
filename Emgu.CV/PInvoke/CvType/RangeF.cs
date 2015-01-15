//----------------------------------------------------------------------------
//  Copyright (C) 2004-2015 by EMGU Corporation. All rights reserved.       
//----------------------------------------------------------------------------

using System;
using System.Runtime.InteropServices;

namespace Emgu.CV.Structure
{
   /// <summary>
   /// The range use to setup the histogram
   /// </summary>
#if !NETFX_CORE
   [Serializable]
#endif
   [StructLayout(LayoutKind.Sequential)]
   public struct RangeF : IEquatable<RangeF>
   {
      private float _min;
      private float _max;

      /// <summary>
      /// Create a range of the specific min/max value
      /// </summary>
      /// <param name="min">The min value of this range</param>
      /// <param name="max">The max value of this range</param>
      public RangeF(float min, float max)
      {
         _min = min;
         _max = max;
      }

      /// <summary>
      /// The minimum value of this range
      /// </summary>
      public float Min
      {
         get { return _min; }
         set { _min = value; }
      }

      /// <summary>
      /// The Maximum value of this range
      /// </summary>
      public float Max
      {
         get { return _max; }
         set { _max = value; }
      }

      #region IEquatable<RangeF> Members
      /// <summary>
      /// Return true if the two RangeF equals
      /// </summary>
      /// <param name="other">The other RangeF to compare with</param>
      /// <returns>True if the two RangeF equals</returns>
      public bool Equals(RangeF other)
      {
         return Min == other.Min && Max == other.Max;
      }
      #endregion
   }
}
