using System;
using System.Collections.Generic;
using System.Text;
using Emgu.Util;
using Emgu.CV.Structure;

namespace Emgu.CV
{
   /// <summary>
   /// A SURF feature
   /// </summary>
   public class SURFFeature
   {
      private MCvSURFPoint _point;
      private float[] _descriptor;

      /// <summary>
      /// The SURF point
      /// </summary>
      public MCvSURFPoint Point
      {
         get { return _point; }
         set { _point = value; }
      }

      /// <summary>
      /// The SURF descriptor as a matrix (64x1 for regular descriptor; 128x1 for extended descriptor)
      /// </summary>
      public float[] Descriptor
      {
         get { return _descriptor; }
         set { _descriptor = value; }
      }

      /// <summary>
      /// Create a SURF feature from the specific point and descriptor
      /// </summary>
      /// <param name="point">The MCvSURFPoint structure</param>
      /// <param name="descriptor">The point descriptor</param>
      public SURFFeature(ref MCvSURFPoint point, float[] descriptor)
      {
         _point = point;
         _descriptor = descriptor;
      }
   }
}
