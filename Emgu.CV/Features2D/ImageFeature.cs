//----------------------------------------------------------------------------
//  Copyright (C) 2004-2011 by EMGU. All rights reserved.       
//----------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Text;
using Emgu.CV.Structure;
using System.Runtime.Serialization;

namespace Emgu.CV.Features2D
{
   /// <summary>
   /// An ImageFeature contains a keypoint and its descriptor
   /// </summary>
   [Serializable]
   public struct ImageFeature
   {
      private MKeyPoint _keyPoint;
      private float[] _descriptor;

      /// <summary>
      /// The descriptor to the keypoint
      /// </summary>
      public float[] Descriptor
      {
         get { return _descriptor; }
         set { _descriptor = value; }
      }

      /// <summary>
      /// The keypoint
      /// </summary>
      public MKeyPoint KeyPoint
      {
         get { return _keyPoint; }
         set { _keyPoint = value; }
      }
   }
}
