using System;

using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;

using System.Drawing;
using Emgu.CV;
using Emgu.CV.Structure;

namespace PlanarSubdivision
{
   [Activity(Label = "Planar Subdivision", MainLauncher = true, Icon = "@drawable/icon")]
   public class PlanarSubdivisionActivity : Activity
   {
      protected override void OnCreate(Bundle bundle)
      {
         base.OnCreate(bundle);

         // Set our view from the "main" layout resource
         SetContentView(Resource.Layout.Main);

         float maxValue = 600;

         #region create random points in the range of [0, maxValue]
         PointF[] pts = new PointF[20];
         Random r = new Random((int)(DateTime.Now.Ticks & 0x0000ffff));
         for (int i = 0; i < pts.Length; i++)
            pts[i] = new PointF((float)r.NextDouble() * maxValue, (float)r.NextDouble() * maxValue);
         #endregion

         Triangle2DF[] delaunayTriangles;
         VoronoiFacet[] voronoiFacets;
         using (Emgu.CV.PlanarSubdivision subdivision = new Emgu.CV.PlanarSubdivision(pts))
         {
            //Obtain the delaunay's triangulation from the set of points;
            delaunayTriangles = subdivision.GetDelaunayTriangles();

            //Obtain the voronoi facets from the set of points
            voronoiFacets = subdivision.GetVoronoiFacets();
         }

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
         foreach (Triangle2DF triangles in delaunayTriangles)
            img.Draw(triangles, new Bgr(Color.White), 1);

         //display the image
         ImageView imageView = FindViewById<ImageView>(Resource.Id.MyImage);
         imageView.SetImageBitmap(img.ToBitmap());   
      }
   }
}

