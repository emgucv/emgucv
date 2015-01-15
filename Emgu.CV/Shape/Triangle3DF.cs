//----------------------------------------------------------------------------
//  Copyright (C) 2004-2015 by EMGU Corporation. All rights reserved.       
//----------------------------------------------------------------------------

using System;

namespace Emgu.CV.Structure
{
   /// <summary>
   /// A 3D triangle
   /// </summary>
   public struct Triangle3DF : IEquatable<Triangle3DF>
   {
      private MCvPoint3D32f _v0;
      private MCvPoint3D32f _v1;
      private MCvPoint3D32f _v2;

      /// <summary>
      /// One of the vertex of the triangle
      /// </summary>
      public MCvPoint3D32f V0
      {
         get { return _v0; }
         set { _v0 = value; }
      }

      /// <summary>
      /// One of the vertex of the triangle
      /// </summary>
      public MCvPoint3D32f V1
      {
         get { return _v1; }
         set { _v1 = value; }
      }

      /// <summary>
      /// One of the vertex of the triangle
      /// </summary>
      public MCvPoint3D32f V2
      {
         get { return _v2; }
         set { _v2 = value; }
      }

      /// <summary>
      /// Get the area of this triangle
      /// </summary>
      public double Area
      {
         get
         {
            #region use Heron's formula to find the area of the triangle
            double a = new LineSegment3DF(V0, V1).Length;
            double b = new LineSegment3DF(V1, V2).Length;
            double c = new LineSegment3DF(V2, V0).Length;
            double s = (a + b + c) / 2.0;
            return Math.Sqrt(s * (s - a) * (s - b) * (s - c));
            #endregion
            
         }
      }

      /// <summary>
      /// Get the normal of this triangle
      /// </summary>
      public MCvPoint3D32f Normal
      {
         get
         {
            return (V1-V0).CrossProduct(V2-V1).GetNormalizedPoint();
         }
      }

      /// <summary>
      /// Returns the centroid of this triangle
      /// </summary>
      public MCvPoint3D32f Centeroid
      {
         get
         {
            return new MCvPoint3D32f((V0.X + V1.X + V2.X) / 3.0f, (V0.Y + V1.Y + V2.Y) / 3.0f, (V0.Z + V1.Z + V2.Z)/3.0f);
         }
      }

      /// <summary>
      /// Create a triangle using the specific vertices
      /// </summary>
      /// <param name="v0">The first vertex</param>
      /// <param name="v1">The second vertex</param>
      /// <param name="v2">The third vertex</param>
      public Triangle3DF(MCvPoint3D32f v0, MCvPoint3D32f v1, MCvPoint3D32f v2)
      {
         _v0 = v0;
         _v1 = v1;
         _v2 = v2;
      }

      /// <summary>
      /// Compare two triangles and return true if equal
      /// </summary>
      /// <param name="tri">the other triangles to compare with</param>
      /// <returns>true if the two triangles equals, false otherwise</returns>
      public bool Equals(Triangle3DF tri)
      {
         return
            (V0.Equals(tri.V0) || V0.Equals(tri.V1) || V0.Equals(tri.V2)) &&
            (V1.Equals(tri.V0) || V1.Equals(tri.V1) || V1.Equals(tri.V2)) &&
            (V2.Equals(tri.V0) || V2.Equals(tri.V1) || V2.Equals(tri.V2));
      }
   }
}
