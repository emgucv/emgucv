using System;
using System.Collections.Generic;
using System.Text;

namespace Emgu.CV
{
   /// <summary>
   /// An interface for the convex polygon
   /// </summary>
   /// <typeparam name="T">The depth of the convex polygon</typeparam>
   public interface IConvexPolygon<T> where T : struct, IComparable
   {
      /// <summary>
      /// Get the vertices of this convex polygon
      /// </summary>
      Point2D<T>[] Vertices
      {
         get;
      }
   }
}
