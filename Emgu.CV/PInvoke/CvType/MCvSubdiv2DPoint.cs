//----------------------------------------------------------------------------
//  Copyright (C) 2004-2012 by EMGU. All rights reserved.       
//----------------------------------------------------------------------------

using System;
using System.Runtime.InteropServices;
using System.Drawing;

namespace Emgu.CV.Structure
{
   /// <summary>
   /// Managed CvSubdiv2DPoint structure
   /// </summary>
   [StructLayout(LayoutKind.Sequential)]
   public struct MCvSubdiv2DPoint
   {
      /// <summary>
      /// If -1, this point is invalid;
      /// If 1&gt;&gt;30, this point is a virtual point;
      /// </summary>
      public int flags;

      /// <summary>
      /// The first edge associate with this point
      /// </summary>
      public MCvSubdiv2DEdge first;

      /// <summary>
      /// The PointF
      /// </summary>
      public PointF pt;

      /// <summary>
      /// The ID
      /// </summary>
      public int id;

      /// <summary>
      /// Return true if this is a valid point
      /// </summary>
      public bool IsValid
      {
         get
         {
            return flags != -1;
         }
      }
   }
}
