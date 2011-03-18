//----------------------------------------------------------------------------
//  Copyright (C) 2004-2011 by EMGU. All rights reserved.       
//----------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using Emgu.CV;
using Emgu.CV.Structure;
using Emgu.CV.UI;

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
         if (!IsPlaformCompatable()) return;
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

      /// <summary>
      /// Check if both the managed and unmanaged code are compiled for the same architecture
      /// </summary>
      /// <returns>Returns true if both the managed and unmanaged code are compiled for the same architecture</returns>
      static bool IsPlaformCompatable()
      {
         int clrBitness = Marshal.SizeOf(typeof(IntPtr)) * 8;
         if (clrBitness != CvInvoke.UnmanagedCodeBitness)
         {
            MessageBox.Show(String.Format("Platform mismatched: CLR is {0} bit, C++ code is {1} bit."
               + " Please consider recompiling the executable with the same platform target as C++ code.",
               clrBitness, CvInvoke.UnmanagedCodeBitness));
            return false;
         }
         return true;
      }
   }
}
