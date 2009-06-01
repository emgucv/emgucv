using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Drawing;
using Emgu.CV;
using Emgu.CV.UI;
using Emgu.CV.Structure;

namespace PlanarSubdivisionExample
{
   static class Program
   {
      /// <summary>
      /// The main entry point for the application.
      /// </summary>
      [STAThread]
      static void Main()
      {
         Application.EnableVisualStyles();
         Application.SetCompatibleTextRenderingDefault(false);
         Run();
      }

      static void Run()
      {
         float maxValue = 600;

         #region create random points in the range of [0, maxValue]
         PointF[] pts = new PointF[20];
         Random r = new Random((int)(DateTime.Now.Ticks & 0x0000ffff));
         for (int i = 0; i < pts.Length; i++)
            pts[i] = new PointF((float)r.NextDouble() * maxValue, (float)r.NextDouble() * maxValue);
         #endregion

         Triangle2DF[] delaunayTriangles;
         VoronoiFacet[] voronoiFacets;
         using (PlanarSubdivision subdivision = new PlanarSubdivision(pts))
         {
            //Obtain the delaunay's triangulation from the set of points;
            delaunayTriangles = subdivision.GetDelaunayTriangles();

            //Obtain the voronoi facets from the set of points
            voronoiFacets = subdivision.GetVoronoiFacets();
         }

         //create an image for display purpose
         Image<Bgr, Byte> img = new Image<Bgr, byte>((int)maxValue, (int) maxValue);

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
         foreach (Triangle2DF triangles in delaunayTriangles)
            img.Draw(triangles, new Bgr(Color.White), 1);

         //display the image
         ImageViewer.Show(img, "Plannar Subdivision");
      }
   }
}
