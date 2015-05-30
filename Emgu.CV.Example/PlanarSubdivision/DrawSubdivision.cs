//----------------------------------------------------------------------------
//  Copyright (C) 2004-2015 by EMGU Corporation. All rights reserved.       
//----------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Runtime.InteropServices;
using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Structure;
using Emgu.CV.Util;

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

         using (Subdiv2D subdivision = new Subdiv2D(pts))
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
      public static Mat Draw(float maxValue, int pointCount)
      {
         Triangle2DF[] delaunayTriangles;
         VoronoiFacet[] voronoiFacets;
         Random r = new Random((int)(DateTime.Now.Ticks & 0x0000ffff));

         CreateSubdivision(maxValue, pointCount, out delaunayTriangles, out voronoiFacets);

         //create an image for display purpose
         Mat img = new Mat((int)maxValue, (int)maxValue, DepthType.Cv8U, 3);

         //Draw the voronoi Facets
         foreach (VoronoiFacet facet in voronoiFacets)
         {
#if NETFX_CORE
            Point[] polyline = Extensions.ConvertAll<PointF, Point>(facet.Vertices, Point.Round);
#else
            Point[] polyline = Array.ConvertAll<PointF, Point>(facet.Vertices, Point.Round);
#endif
            using (VectorOfPoint vp = new VectorOfPoint(polyline))
            using (VectorOfVectorOfPoint vvp = new VectorOfVectorOfPoint(vp))
            {
               //Draw the facet in color
               CvInvoke.FillPoly(
                  img, vvp, 
                  new Bgr(r.NextDouble()*120, r.NextDouble()*120, r.NextDouble()*120).MCvScalar);

               //highlight the edge of the facet in black
               CvInvoke.Polylines(img, vp, true, new Bgr(0, 0, 0).MCvScalar, 2);
            }
            //draw the points associated with each facet in red
            CvInvoke.Circle(img, Point.Round( facet.Point ), 5, new Bgr(0, 0, 255).MCvScalar, -1);
         }

         //Draw the Delaunay triangulation
         foreach (Triangle2DF triangle in delaunayTriangles)
         {
#if NETFX_CORE
            Point[] vertices = Extensions.ConvertAll<PointF, Point>(triangle.GetVertices(), Point.Round);
#else
            Point[] vertices = Array.ConvertAll<PointF, Point>(triangle.GetVertices(), Point.Round);
#endif
            using (VectorOfPoint vp = new VectorOfPoint(vertices))
            {
               CvInvoke.Polylines(img, vp, true, new Bgr(255, 255, 255).MCvScalar);
            }
         }

         return img;
      }
   }
}
