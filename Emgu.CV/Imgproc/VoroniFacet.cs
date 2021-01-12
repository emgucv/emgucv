//----------------------------------------------------------------------------
//  Copyright (C) 2004-2021 by EMGU Corporation. All rights reserved.       
//----------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Runtime.InteropServices;
using Emgu.CV.Structure;
using Emgu.CV.Util;
using Emgu.Util;

namespace Emgu.CV
{
   /// <summary>
   /// A Voronoi Facet
   /// </summary>
   public class VoronoiFacet
   {
      private PointF _point;
      private PointF[] _vertices;

      /// <summary>
      /// Create a Voronoi facet using the specific <paramref name="point"/> and <paramref name="polyline"/>
      /// </summary>
      /// <param name="point">The point this facet associate with </param>
      /// <param name="polyline">The points that defines the contour of this facet</param>
      public VoronoiFacet(PointF point, PointF[] polyline)
      {
         _point = point;
         _vertices = polyline;

         //Debug.Assert(point.InConvexPolygon(this));
      }

      /// <summary>
      /// The point this facet associates to
      /// </summary>
      public PointF Point
      {
         get { return _point; }
         set { _point = value; }
      }

      /// <summary>
      /// Get or set the vertices of this facet
      /// </summary>
      public PointF[] Vertices
      {
         get { return _vertices; }
         set { _vertices = value; }
      }
   }
}
