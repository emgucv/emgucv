using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;

namespace Emgu.CV
{
    /// <summary>
    /// Delaunay's Triangulation
    /// </summary>
    public class DelaunayTriangulation : UnmanagedObject
    {
        private MemStorage _storage;
        private Rectangle<float> _roi;

        /// <summary>
        /// Start the Delaunay's triangulation in the specific region of interest.
        /// </summary>
        /// <param name="roi">The region of interest of the triangulation</param>
        public DelaunayTriangulation(Rectangle<double> roi)
        {
            _storage = new MemStorage();
            _ptr = CvInvoke.cvCreateSubdivDelaunay2D(roi.MCvRect, _storage);
            _roi = roi.Convert<float>();
        }

        /// <summary>
        /// Insert another point to the triangulation
        /// </summary>
        /// <param name="point">the point to be inserted</param>
        public void Insert(MCvPoint2D32f point)
        {
            CvInvoke.cvSubdivDelaunay2DInsert(_ptr, point);
        }

        /// <summary>
        /// Get the MCvsubdiv structure of this Delaunay's triangulation
        /// </summary>
        public MCvSubdiv2D MCvSubdiv2D
        {
            get { return (MCvSubdiv2D)Marshal.PtrToStructure(_ptr, typeof(MCvSubdiv2D)); }
        }

        private Triangle<float> EdgeToTriangle(ref MCvSubdiv2DEdge e)
        {
            MCvSubdiv2DPoint v1 = e.cvSubdiv2DEdgeOrg();
            MCvSubdiv2DPoint v2 = e.cvSubdiv2DEdgeDst();

            MCvSubdiv2DEdge eLnext = e.cvSubdiv2DGetEdge(Emgu.CV.CvEnum.CV_NEXT_EDGE_TYPE.CV_NEXT_AROUND_LEFT);
            MCvSubdiv2DPoint v4 = eLnext.cvSubdiv2DEdgeDst();
            return new Triangle<float>(
                new Point2D<float>(v1.pt.x, v1.pt.y),
                new Point2D<float>(v4.pt.x, v4.pt.y),
                new Point2D<float>(v2.pt.x, v2.pt.y));
        }

        /// <summary>
        /// It finds subdivision vertex that is the closest to the input point. It is not necessarily one of vertices of the facet containing the input point, though the facet (located using cvSubdiv2DLocate) is used as a starting point.
        /// </summary>
        /// <param name="pt">Input point</param>
        /// <returns>returns the found subdivision vertex</returns>
        private MCvSubdiv2DPoint FindNearestPoint2D(ref MCvPoint2D32f pt)
        {
            IntPtr ptr = CvInvoke.cvFindNearestPoint2D(Ptr, pt);
            return (MCvSubdiv2DPoint) Marshal.PtrToStructure(ptr, typeof(MCvSubdiv2DPoint));
        }

        /// <summary>
        /// Retruns the triangles in the current triangulation
        /// </summary>
        /// <returns>The triangles in the current triangulation</returns>
        public Triangle<float>[] GetCurrentTriangles()
        {
            List<Triangle<float>> triangleList = new List<Triangle<float>>();

            MCvSeqReader reader = new MCvSeqReader();
            MCvSubdiv2D subdiv = MCvSubdiv2D;
            MCvSet set = (MCvSet)Marshal.PtrToStructure(subdiv.edges, typeof(MCvSet));
            int i, total = set.total;
            int elem_size = set.elem_size;
            CvInvoke.cvStartReadSeq(subdiv.edges, ref reader, false);

            for (i = 0; i < total; i++)
            {
                IntPtr edge = reader.ptr;

                if (CvInvoke.CV_IS_SET_ELEM(edge))
                {
                    MCvQuadEdge2D qEdge = (MCvQuadEdge2D)Marshal.PtrToStructure(edge, typeof(MCvQuadEdge2D));

                    Triangle<float> tri1 = EdgeToTriangle(ref qEdge.next[0]);
                    Triangle<float> tri2 = EdgeToTriangle(ref qEdge.next[2]);
                    if ( Utils.IsConvexPolygonInConvexPolygon(tri1, _roi)
                       && Array.FindIndex<Triangle<float>>(triangleList.ToArray(), delegate(Triangle<float> existingTri) { return existingTri.Equals(tri1); }) < 0)
                        triangleList.Add(tri1);

                    if ( Utils.IsConvexPolygonInConvexPolygon(tri2, _roi)
                        && Array.FindIndex<Triangle<float>>(triangleList.ToArray(), delegate(Triangle<float> existingTri) { return existingTri.Equals(tri2); }) < 0)
                        triangleList.Add(tri2);
                }

                CvInvoke.CV_NEXT_SEQ_ELEM(elem_size, ref reader);
            }
            return triangleList.ToArray();
        }

        /// <summary>
        /// Release the storage related to this triangulation
        /// </summary>
        protected override void DisposeObject()
        {
            _storage.Dispose();
        }

        #region static methods
        /// <summary>
        /// Given an array of points, returns the delaunay's triangulation
        /// </summary>
        /// <param name="points">the points for triangulation</param>
        /// <returns>The triangles as a result of the triangulation</returns>
        public static Triangle<float>[] Triangulate(Point2D<float>[] points)
        {
            #region Find the region of interest
            Rectangle<double> roi;
            using (MemStorage storage = new MemStorage())
            using (Seq<MCvPoint2D32f> seq = PointCollection.To2D32fSequence(storage, (IEnumerable<Point<float>>)points))
            {
                MCvRect cvRect = CvInvoke.cvBoundingRect(seq.Ptr, true);
                roi = new Rectangle<double>(cvRect);
            }
            #endregion

            using (DelaunayTriangulation tri = new DelaunayTriangulation(roi))
            {
                foreach (Point2D<float> p in points)
                    tri.Insert(new MCvPoint2D32f(p.X, p.Y));

                return tri.GetCurrentTriangles();
            }
        }
        #endregion

    }
}
