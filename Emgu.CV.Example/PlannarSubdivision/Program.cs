using System;
using System.Collections.Generic;
using System.Windows.Forms;
using Emgu.CV;
using Emgu.CV.UI;

namespace PlannarSubdivision
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
            #region create random points in the range of [0, maxValue]
            Point2D<float>[] pts = new Point2D<float>[20];
            float maxValue = 600;
            Random r = new Random((int)(DateTime.Now.Ticks & 0x0000ffff));
            for (int i = 0; i < pts.Length; i++)
                pts[i] = new Point2D<float>((float)r.NextDouble() * maxValue, (float)r.NextDouble() * maxValue);
            #endregion

            //Obtain the delaunay's triangulation from the set of points;
            List<Triangle<float>> delaunayTriangles = PlanarSubdivision.GetDelaunayTriangles(pts);

            //Obtain the voronoi facets from the set of points
            List<VoronoiFacet> voronoiFacets = PlanarSubdivision.GetVoronoi(pts);

            //create an image for display purpose
            Image<Bgr, Byte> img = new Image<Bgr, byte>(600, 600);

            //Draw the voronoi Facets
            foreach (VoronoiFacet facet in voronoiFacets)
            {
                MCvPoint[] points = Array.ConvertAll<Point2D<float>, MCvPoint>(facet.Vertices, delegate(Point2D<float> p) { return p.MCvPoint; });

                //Draw the facet in color
                img.FillConvexPoly(
                    points,
                    new Bgr(r.NextDouble() * 120, r.NextDouble() * 120, r.NextDouble() * 120)
                    );

                //highlight the edge of the facet in black
                img.DrawPolyline(points, true, new Bgr(0.0, 0, 0), 2);

                //draw the points associated with each facet in red
                img.Draw(new Circle<float>(facet.Point, 5), new Bgr(0, 0, 255), 0);
            }

            //Draw the Delaunay triangulation
            foreach (Triangle<float> triangles in delaunayTriangles)
            {
                img.Draw(triangles, new Bgr(255.0, 255.0, 255.0), 1);
            }

            //display the image
            Application.Run(new ImageViewer(img, "Plannar Subdivision"));
        }
    }
}