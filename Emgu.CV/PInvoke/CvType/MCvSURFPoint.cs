//----------------------------------------------------------------------------
//  Copyright (C) 2004-2012 by EMGU. All rights reserved.       
//----------------------------------------------------------------------------

using System;
using System.Runtime.InteropServices;

namespace Emgu.CV.Structure
{
   /// <summary>
   /// Wrapped CvSURFPoint structure
   /// </summary>
   [StructLayout(LayoutKind.Sequential)]
   [Serializable]
   public struct MCvSURFPoint
   {
      /// <summary>
      /// Position of the feature within the image
      /// </summary>
      public System.Drawing.PointF pt;

      /// <summary>
      /// -1, 0 or +1. sign of the laplacian at the point.
      /// can be used to speedup feature comparison
      /// (normally features with laplacians of different signs can not match)
      /// </summary>
      public int laplacian;

      /// <summary>
      /// Size of the feature
      /// </summary>
      public int size;

      /// <summary>
      /// Orientation of the feature: 0..360 degrees
      /// </summary>
      public float dir;

      /// <summary>
      /// Value of the hessian (can be used to approximately estimate the feature strengths;
      /// see also params.hessianThreshold.
      /// </summary>
      public float hessian;
   }
}
