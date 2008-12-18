using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;

namespace Emgu.CV
{
   /// <summary> 
   /// A line segment 
   /// </summary>
   public class LineSegment2DF : Line2DF 
   {
      /// <summary> 
      /// Create a line segment with the specific starting point and end point 
      /// </summary>
      /// <param name="p1">The first point on the line segment</param>
      /// <param name="p2">The second point on the line segment</param>
      public LineSegment2DF(PointF p1, PointF p2) : base(p1, p2) { }

      /// <summary> 
      /// Get the length of the line segment 
      /// </summary>
      public double Length
      {
         get 
         {
            float dx = P1.X - P2.X;
            float dy = P1.Y - P2.Y;
            return Math.Sqrt(dx * dx + dy * dy);  
         }
      }
   }
}
