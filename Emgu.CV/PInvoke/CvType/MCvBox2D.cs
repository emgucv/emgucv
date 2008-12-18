using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;

namespace Emgu.CV.Structure
{
   /// <summary>
   /// Managed structure equivalent to CvBox2D
   /// </summary>
   [StructLayout(LayoutKind.Sequential)]
   public struct MCvBox2D : IConvexPolygonF
   {
      /// <summary>
      /// The center of the box
      /// </summary>
      public System.Drawing.PointF center;
      /// <summary>
      /// The size of the box
      /// </summary>
      public System.Drawing.SizeF size;
      /// <summary>
      /// The angle of the box
      /// </summary>
      public float angle;

      /// <summary>
      /// Create a MCvBox2D structure with the specific parameters
      /// </summary>
      /// <param name="center">The center of the box</param>
      /// <param name="size">The size of the box</param>
      /// <param name="angle">The angle of the box</param>
      public MCvBox2D(System.Drawing.PointF center, System.Drawing.SizeF size, float angle)
      {
         this.center = center;
         this.size = size;
         this.angle = angle;
      }

      #region IConvexPolygonF Members
      /// <summary>
      /// Get the 4 verticies of this Box.
      /// </summary>
      /// <returns></returns>
      public System.Drawing.PointF[] GetVertices()
      {
         System.Drawing.PointF[] coordinates = new System.Drawing.PointF[4];
         CvInvoke.cvBoxPoints(this, coordinates);
         return coordinates;
      }

      #endregion
   }
}
