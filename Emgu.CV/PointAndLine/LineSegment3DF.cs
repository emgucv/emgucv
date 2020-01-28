//----------------------------------------------------------------------------
//  Copyright (C) 2004-2020 by EMGU Corporation. All rights reserved.       
//----------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Text;

namespace Emgu.CV.Structure
{
   /// <summary>
   /// A 3D line segment
   /// </summary>
   public struct LineSegment3DF
   {
      /// <summary> A point on the line </summary>
      private MCvPoint3D32f _p1;
      /// <summary> An other point on the line </summary>
      private MCvPoint3D32f _p2;

      /// <summary> A point on the line </summary>
      public MCvPoint3D32f P1 { get { return _p1; } set { _p1 = value; } }

      /// <summary> An other point on the line </summary>
      public MCvPoint3D32f P2 { get { return _p2; } set { _p2 = value; } }

      /// <summary> 
      /// Create a line segment with the specific start point and end point 
      /// </summary>
      /// <param name="p1">The first point on the line segment</param>
      /// <param name="p2">The second point on the line segment</param>
      public LineSegment3DF(MCvPoint3D32f p1, MCvPoint3D32f p2)
      {
         _p1 = p1;
         _p2 = p2;
      }

      /// <summary> 
      /// Get the length of the line segment 
      /// </summary>
      public double Length
      {
         get
         {
            float dx = P1.X - P2.X;
            float dy = P1.Y - P2.Y;
            float dz = P1.Z - P2.Z;
            return Math.Sqrt(dx * dx + dy * dy + dz * dz);
         }
      }
   }
}
