using System;
using System.Collections.Generic;
using System.Text;

namespace Emgu.CV
{
   /// <summary>
   /// An interface for the convex polygon
   /// </summary>
   public interface IConvexPolygon
   {
      /// <summary>
      /// Get the vertices of this convex polygon
      /// </summary>
      System.Drawing.Point[] GetVertices();
   }

   /// <summary>
   /// An interface for the convex polygon
   /// </summary>
   public interface IConvexPolygonF
   {
      /// <summary>
      /// Get the vertices of this convex polygon
      /// </summary>
      System.Drawing.PointF[] GetVertices();
   }
}
