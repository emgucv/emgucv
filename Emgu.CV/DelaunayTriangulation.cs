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
        /// <returns>
        /// true if the point is inserted into the triangulation;
        /// false otherwise.
        /// </returns>
        public bool Insert(ref MCvPoint2D32f point)
        {
            try
            {
                CvInvoke.cvSubdivDelaunay2DInsert(_ptr, point);
                return true;
            }
            catch (CvException)
            {
                return false;
            }
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
            MCvSubdiv2DPoint v3 = eLnext.cvSubdiv2DEdgeDst();
            return new Triangle<float>(
                new Point2D<float>(v1.pt.x, v1.pt.y),
                new Point2D<float>(v3.pt.x, v3.pt.y),
                new Point2D<float>(v2.pt.x, v2.pt.y));
        }

        /// <summary>
        /// It finds subdivision vertex that is the closest to the input point. It is not necessarily one of vertices of the facet containing the input point, though the facet (located using cvSubdiv2DLocate) is used as a starting point.
        /// </summary>
        /// <param name="point">Input point</param>
        /// <returns>returns the found subdivision vertex</returns>
        private MCvSubdiv2DPoint FindNearestPoint2D(ref MCvPoint2D32f point)
        {
            IntPtr ptr = CvInvoke.cvFindNearestPoint2D(Ptr, point);
            return (MCvSubdiv2DPoint)Marshal.PtrToStructure(ptr, typeof(MCvSubdiv2DPoint));
        }

        /// <summary>
        /// Returns the triangles of the Delaunay's triangulation
        /// </summary>
        /// <remarks>The vertices of the triangles all belongs to the inserted points</remarks>
        /// <returns>The result of the current triangulation</returns>
        public Triangle<float>[] GetDelaunayTriangles()
        {
            List<Triangle<float>> triangleList = new List<Triangle<float>>();

            Triangle<float>[] subdivisionTriangle = GetPlanarSubdivisionTriangles();
            foreach (Triangle<float> tri in subdivisionTriangle)
                if (Utils.IsConvexPolygonInConvexPolygon(tri, _roi))
                    triangleList.Add(tri);

            return triangleList.ToArray();
        }

        /// <summary>
        /// Retruns the triangles subdivision of the current triangulation. 
        /// </summary>
        /// <remarks>The triangles might contains virtual points that do not belongs to the inserted points</remarks>
        /// <returns>The triangles subdivision in the current triangulation</returns>
        public Triangle<float>[] GetPlanarSubdivisionTriangles()
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
                    MCvQuadEdge2D quadEdge = (MCvQuadEdge2D)Marshal.PtrToStructure(edge, typeof(MCvQuadEdge2D));

                    Triangle<float> tri1 = EdgeToTriangle(ref quadEdge.next[0]);
                    if (Array.FindIndex<Triangle<float>>(triangleList.ToArray(), delegate(Triangle<float> existingTri) { return existingTri.Equals(tri1); }) < 0)
                        triangleList.Add(tri1);

                    Triangle<float> tri2 = EdgeToTriangle(ref quadEdge.next[2]);
                    if (Array.FindIndex<Triangle<float>>(triangleList.ToArray(), delegate(Triangle<float> existingTri) { return existingTri.Equals(tri2); }) < 0)
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
        ///  Find the Delaunay's triangulation from the given <paramref name="points"/>
        /// </summary>
        /// <param name="points">the points for triangulation</param>
        /// <remarks>The vertices of the triangles all belongs to the inserted points</remarks>
        /// <returns>The triangles as a result of the triangulation</returns>
        public static Triangle<float>[] GetDelaunayTriangles(IEnumerable<Point2D<float>> points)
        {
            using (DelaunayTriangulation tri = GetDelaunay(points))
                return tri.GetDelaunayTriangles();
        }

        /// <summary>
        ///  Find the Delaunay's plannar subdivision triangles from the given <paramref name="points"/>
        /// </summary>
        /// <param name="points">the points for triangulation</param>
        /// <remarks>The triangles might contains virtual points that do not belongs to the inserted points</remarks>
        /// <returns>The triangles subdivision in the current triangulation</returns>
        public static Triangle<float>[] GetPlanarSubdivisionTriangles(IEnumerable<Point2D<float>> points)
        {
            using (DelaunayTriangulation tri = GetDelaunay(points))
                return tri.GetPlanarSubdivisionTriangles();
        }

        private static DelaunayTriangulation GetDelaunay(IEnumerable<Point2D<float>> points)
        {
            #region Find the region of interest
            Rectangle<double> roi;
            using (MemStorage storage = new MemStorage())
            using (Seq<MCvPoint2D32f> seq = PointCollection.To2D32fSequence(storage, Emgu.Utils.IEnumConvertor<Point2D<float>, Point<float>>(points, delegate(Point2D<float> p) { return (Point<float>) p;})))
            {
                MCvRect cvRect = CvInvoke.cvBoundingRect(seq.Ptr, true);
                roi = new Rectangle<double>(cvRect);
            }
            #endregion

            DelaunayTriangulation tri = new DelaunayTriangulation(roi);
             
            foreach (Point2D<float> p in points)
            {
                MCvPoint2D32f cvPoint = new MCvPoint2D32f(p.X, p.Y);
                tri.Insert(ref cvPoint);
            }
            return tri;
        }
        #endregion

    }
}
