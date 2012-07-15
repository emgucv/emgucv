//----------------------------------------------------------------------------
//  Copyright (C) 2004-2012 by EMGU. All rights reserved.       
//----------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Runtime.InteropServices;
using Emgu.CV;
using Emgu.CV.Structure;

namespace PlanarSubdivisionExample
{
   public static class DrawSubdivision
   {
      /// <summary>
      /// Create planar subdivision for random points
      /// </summary>
      /// <param name="maxValue">The points contains values between [0, maxValue)</param>
      /// <param name="pointCount">The total number of points to create</param>
      public static void CreateSubdivision(float maxValue, int pointCount, out Triangle2DF[] delaunayTriangles, out VoronoiFacet[] voronoiFacets)
      {
         #region create random points in the range of [0, maxValue]
         PointF[] pts = new PointF[pointCount];
         Random r = new Random((int)(DateTime.Now.Ticks & 0x0000ffff));
         for (int i = 0; i < pts.Length; i++)
            pts[i] = new PointF((float)r.NextDouble() * maxValue, (float)r.NextDouble() * maxValue);
         #endregion

         using (PlanarSubdivision subdivision = new PlanarSubdivision(pts))
         {
            //Obtain the delaunay's triangulation from the set of points;
            delaunayTriangles = subdivision.GetDelaunayTriangles();

            //Obtain the voronoi facets from the set of points
            voronoiFacets = subdivision.GetVoronoiFacets();
         }
      }

      /// <summary>
      /// Draw the planar subdivision
      /// </summary>
      /// <param name="maxValue">The points contains values between [0, maxValue)</param>
      /// <param name="pointCount">The total number of points</param>
      /// <returns>An image representing the planar subvidision of the points</returns>
      public static Image<Bgr, Byte> Draw(float maxValue, int pointCount)
      {
         Triangle2DF[] delaunayTriangles;
         VoronoiFacet[] voronoiFacets;
         Random r = new Random((int)(DateTime.Now.Ticks & 0x0000ffff));

         CreateSubdivision(maxValue, pointCount, out delaunayTriangles, out voronoiFacets);

         //create an image for display purpose
         Image<Bgr, Byte> img = new Image<Bgr, byte>((int)maxValue, (int)maxValue);

         //Draw the voronoi Facets
         foreach (VoronoiFacet facet in voronoiFacets)
         {
            Point[] polyline = Array.ConvertAll<PointF, Point>(facet.Vertices, Point.Round);

            //Draw the facet in color
            img.FillConvexPoly(
                polyline,
                new Bgr(r.NextDouble() * 120, r.NextDouble() * 120, r.NextDouble() * 120)
                );

            //highlight the edge of the facet in black
            img.DrawPolyline(polyline, true, new Bgr(Color.Black), 2);

            //draw the points associated with each facet in red
            img.Draw(new CircleF(facet.Point, 5.0f), new Bgr(Color.Red), 0);
         }

         //Draw the Delaunay triangulation
         foreach (Triangle2DF triangle in delaunayTriangles)
            img.Draw(triangle, new Bgr(Color.White), 1);

         return img;
      }
   }
}
