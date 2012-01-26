//----------------------------------------------------------------------------
//  Copyright (C) 2004-2012 by EMGU. All rights reserved.       
//----------------------------------------------------------------------------

using System;
using Emgu.CV.Structure;
using System.Runtime.Serialization;

namespace Emgu.CV.Features2D
{
   /// <summary>
   /// A SURF feature
   /// </summary>
   [Serializable]
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
      /// The feature descriptor as an array
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
      /// <param name="descriptor">The feature descriptor</param>
      public SURFFeature(ref MCvSURFPoint point, float[] descriptor)
      {
         _point = point;
         _descriptor = descriptor;
      }
   }
}
