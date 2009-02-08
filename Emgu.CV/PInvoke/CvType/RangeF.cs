using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;

namespace Emgu.CV.Structure
{
   /// <summary>
   /// The range use to setup the histogram
   /// </summary>
   [StructLayout(LayoutKind.Sequential)]
   public struct RangeF
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
   }
}
