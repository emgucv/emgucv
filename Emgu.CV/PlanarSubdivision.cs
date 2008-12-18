using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using System.Diagnostics;
using Emgu.Util;
using Emgu.CV.Structure;

namespace Emgu.CV
{
   /// <summary>
   /// Plannar Subdivision
   /// </summary>
   public class PlanarSubdivision : UnmanagedObject
   {
      private readonly MemStorage _storage;
      private readonly System.Drawing.Rectangle _roi;

      private bool _isVoronoiDirty;

      #region constructor
      /// <summary>
      /// Start the Delaunay's triangulation in the specific region of interest.
      /// </summary>
      /// <param name="roi">The region of interest of the triangulation</param>
      public PlanarSubdivision(ref System.Drawing.Rectangle roi)
      {
         _storage = new MemStorage();
         _ptr = CvInvoke.cvCreateSubdivDelaunay2D(roi, _storage);
         _roi = roi;
      }

      /// <summary>
      /// Create a planar subdivision from the given points. The ROI is computed as the minimun bounding Rectangle for the input points
      /// </summary>
      /// <param name="points">The points for this planar subdivision</param>
      public PlanarSubdivision(System.Drawing.PointF[] points)
         : this(points, false)
      {
      }

      /// <summary>
      /// Create a planar subdivision from the given points. The ROI is computed as the minimun bounding Rectangle for the input points
      /// </summary>
      /// <param name="silent">If true, any exception during insert will be ignored</param>
      /// <param name="points">The points for this planar subdivision</param>
      public PlanarSubdivision(System.Drawing.PointF[] points, bool silent)
      {
         #region Find the region of interest
         _roi = PointCollection.BoundingRectangle(points);
         #endregion

         _storage = new MemStorage();
         _ptr = CvInvoke.cvCreateSubdivDelaunay2D(_roi, _storage);

         if (silent)
            foreach (System.Drawing.PointF p in points)
               try
               {
                  CvInvoke.cvSubdivDelaunay2DInsert(_ptr, p);
               }
               catch { }
         else
            foreach (System.Drawing.PointF p in points)
               CvInvoke.cvSubdivDelaunay2DInsert(_ptr, p);

         _isVoronoiDirty = true;
      }
      #endregion

      /// <summary>
      /// Insert a point to the triangulation. If the point is already inserted, no changes will be made.
      /// </summary>
      /// <param name="point">The point to be inserted</param>
      public void Insert(System.Drawing.PointF point)
      {
         CvInvoke.cvSubdivDelaunay2DInsert(_ptr, point);
         _isVoronoiDirty = true;
      }

      /// <summary>
      /// Locates input point within subdivision
      /// </summary>
      /// <param name="pt">The point to locate</param>
      /// <param name="subdiv2DEdge">The output edge the point falls onto or right to</param>
      /// <param name="subdiv2DPoint">Optional output vertex double pointer the input point coincides with</param>
      /// <returns>The type of location for the point</returns>
      public CvEnum.Subdiv2DPointLocationType Locate(ref System.Drawing.PointF pt, out MCvSubdiv2DEdge? subdiv2DEdge, out MCvSubdiv2DPoint? subdiv2DPoint)
      {
         IntPtr edge;
         IntPtr vertex = new IntPtr();
         CvEnum.Subdiv2DPointLocationType res = CvInvoke.cvSubdiv2DLocate(Ptr, pt, out edge, ref vertex);

         subdiv2DEdge = (edge == IntPtr.Zero) ? null : (MCvSubdiv2DEdge?)Marshal.PtrToStructure(edge, typeof(MCvSubdiv2DEdge));
         subdiv2DPoint = (vertex == IntPtr.Zero) ? null : (MCvSubdiv2DPoint?)Marshal.PtrToStructure(vertex, typeof(MCvSubdiv2DPoint));
         return res;
      }

      /// <summary>
      /// Get the MCvSubdiv2D structure of this Delaunay's triangulation
      /// </summary>
      public MCvSubdiv2D MCvSubdiv2D
      {
         get { return (MCvSubdiv2D)Marshal.PtrToStructure(_ptr, typeof(MCvSubdiv2D)); }
      }

      private static Triangle2DF EdgeToTriangle(MCvSubdiv2DEdge e)
      {
         MCvSubdiv2DPoint v1 = e.cvSubdiv2DEdgeOrg();
         MCvSubdiv2DPoint v2 = e.cvSubdiv2DEdgeDst();

         MCvSubdiv2DEdge eLnext = e.cvSubdiv2DGetEdge(Emgu.CV.CvEnum.CV_NEXT_EDGE_TYPE.CV_NEXT_AROUND_LEFT);
         MCvSubdiv2DPoint v3 = eLnext.cvSubdiv2DEdgeDst();
         return new Triangle2DF(v1.pt, v3.pt, v2.pt);
      }

      private static IEnumerable<VoronoiFacet> EdgeToFacets(IntPtr edge, List<System.Drawing.PointF> bufferList)
      {
         MCvQuadEdge2D quadEdge = (MCvQuadEdge2D)Marshal.PtrToStructure(edge, typeof(MCvQuadEdge2D));
         MCvSubdiv2DEdge nextQuadEdge = quadEdge.next[0];

         MCvSubdiv2DEdge e1 = nextQuadEdge.cvSubdiv2DRotateEdge(1);

         System.Drawing.PointF[] p1 = EdgeToPoly(e1, bufferList);
         if (p1 != null)
         {
            MCvSubdiv2DPoint pt = nextQuadEdge.cvSubdiv2DEdgeOrg();
            yield return new VoronoiFacet(pt.pt, p1);
         }

         MCvSubdiv2DEdge e2 = nextQuadEdge.cvSubdiv2DRotateEdge(3);
         System.Drawing.PointF[] p2 = EdgeToPoly(e2, bufferList);
         if (p2 != null)
         {
            MCvSubdiv2DPoint pt = nextQuadEdge.cvSubdiv2DEdgeDst();
            yield return new VoronoiFacet(pt.pt, p2);
         }
      }

      private static System.Drawing.PointF[] EdgeToPoly(MCvSubdiv2DEdge e, List<System.Drawing.PointF> bufferList)
      {
         MCvSubdiv2DPoint v0 = e.cvSubdiv2DEdgeOrg();
         if (!v0.IsValid) return null;
         
         bufferList.Clear();
         bufferList.Add(v0.pt);

         for (MCvSubdiv2DEdge currentEdge = e; ; currentEdge = currentEdge.cvSubdiv2DGetEdge(Emgu.CV.CvEnum.CV_NEXT_EDGE_TYPE.CV_NEXT_AROUND_LEFT))
         {
            MCvSubdiv2DPoint v = currentEdge.cvSubdiv2DEdgeDst();
            if (!v.IsValid) return null;

            if ( v.pt.X == v0.pt.X && v.pt.Y == v0.pt.Y)
               break;

            bufferList.Add(v.pt);
         }
         return bufferList.Count > 2 ? bufferList.ToArray() : null;
      }

      /// <summary>
      /// Finds subdivision vertex that is the closest to the input point. It is not necessarily one of vertices of the facet containing the input point, though the facet (located using cvSubdiv2DLocate) is used as a starting point.
      /// </summary>
      /// <param name="point">Input point</param>
      /// <returns>returns the found subdivision vertex</returns>
      public MCvSubdiv2DPoint FindNearestPoint2D(ref System.Drawing.PointF point)
      {
         return (MCvSubdiv2DPoint)Marshal.PtrToStructure(
            CvInvoke.cvFindNearestPoint2D(Ptr, point), 
            typeof(MCvSubdiv2DPoint));
      }

      /// <summary>
      /// Returns the triangles of the Delaunay's triangulation
      /// </summary>
      /// <remarks>The vertices of the triangles all belongs to the inserted points</remarks>
      /// <returns>The result of the current triangulation</returns>
      public List<Triangle2DF> GetDelaunayTriangles()
      {
         return GetDelaunayTriangles(false);
      }

      /// <summary>
      /// Obtains the list of Voronoi Facets 
      /// </summary>
      /// <returns>The list of Voronoi Facets</returns>
      public List<VoronoiFacet> GetVoronoiFacets()
      {
         if (_isVoronoiDirty == true)
         {
            CvInvoke.cvCalcSubdivVoronoi2D(Ptr);
            _isVoronoiDirty = false;
         }

         Dictionary<System.Drawing.PointF, Byte> facetDict = new Dictionary<System.Drawing.PointF, Byte>();
        
         List<VoronoiFacet> facetList = new List<VoronoiFacet>();

         MCvSeqReader reader = new MCvSeqReader();
         MCvSubdiv2D subdiv = MCvSubdiv2D;
         MCvSet set = (MCvSet)Marshal.PtrToStructure(subdiv.edges, typeof(MCvSet));
         int total = set.total;
         int elem_size = set.elem_size;
         CvInvoke.cvStartReadSeq(subdiv.edges, ref reader, false);

         List<System.Drawing.PointF> bufferList = new List<System.Drawing.PointF>();

         int left = _roi.X, top = _roi.Y, right = _roi.X + _roi.Width, bottom = _roi.Y + _roi.Height;

         for (;CvInvoke.CV_IS_SET_ELEM(reader.ptr); CvInvoke.CV_NEXT_SEQ_ELEM(elem_size, ref reader))
         {
            foreach (VoronoiFacet facet in EdgeToFacets(reader.ptr, bufferList))
            {
               System.Drawing.PointF p = facet.Point;
               if (p.X >= left && p.X <= right && p.Y >= top && p.Y <= bottom
                  && InsertPoint2DToDictionary(p, facetDict))
                  facetList.Add(facet);
            }
         }
         return facetList;
      }
      
      /// <summary>
      /// Insert the point into the dictionary. If the point already exist, return false. Otherwise return true.
      /// </summary>
      /// <param name="pt">The point to insert</param>
      /// <param name="dic">The point dictionary</param>
      /// <returns>If the point already exist, return false. Otherwise return true.</returns>
      private static bool InsertPoint2DToDictionary(System.Drawing.PointF pt, Dictionary<System.Drawing.PointF, Byte> dic) 
      {
         if (dic.ContainsKey(pt))
            return false;

         dic.Add(pt, (Byte)0);
         return true;
      }

      /// <summary>
      /// Retruns the triangles subdivision of the current planar subdivision. 
      /// </summary>
      /// <remarks>The triangles might contains virtual points that do not belongs to the inserted points, if you do not want those points, set <param name="includeVirtualPoints"> to false</param></remarks>
      /// <returns>The triangles subdivision in the current plannar subdivision</returns>
      public List<Triangle2DF> GetDelaunayTriangles(bool includeVirtualPoints)
      {
         Dictionary<System.Drawing.PointF, Byte> triangleDic = new Dictionary<System.Drawing.PointF, Byte>();

         List<Triangle2DF> triangleList = new List<Triangle2DF>();

         MCvSeqReader reader = new MCvSeqReader();
         MCvSubdiv2D subdiv = MCvSubdiv2D;
         MCvSet set = (MCvSet)Marshal.PtrToStructure(subdiv.edges, typeof(MCvSet));
         int total = set.total;
         int elem_size = set.elem_size;
         CvInvoke.cvStartReadSeq(subdiv.edges, ref reader, false);

         for (;CvInvoke.CV_IS_SET_ELEM(reader.ptr); CvInvoke.CV_NEXT_SEQ_ELEM(elem_size, ref reader))
         {
            MCvQuadEdge2D quadEdge = (MCvQuadEdge2D)Marshal.PtrToStructure(reader.ptr, typeof(MCvQuadEdge2D));

            Triangle2DF tri1 = EdgeToTriangle(quadEdge.next[0]);

            if (InsertPoint2DToDictionary(tri1.Centeroid, triangleDic))
            {
               triangleList.Add(tri1);
            }

            Triangle2DF tri2 = EdgeToTriangle(quadEdge.next[2]);
            if (InsertPoint2DToDictionary(tri2.Centeroid, triangleDic))
            {
               triangleList.Add(tri2);
            }
         }

         if (!includeVirtualPoints)
         {
            int left = _roi.X, top = _roi.Y, right = _roi.X + _roi.Width, bottom = _roi.Y + _roi.Height;

            triangleList.RemoveAll(
               delegate(Triangle2DF tri)
               {
                  //Point2D<float>[] vertices = tri.Vertices;
                  return
                     !(
                     tri.V0.X >= left && tri.V0.X <= right && tri.V0.Y >= top && tri.V0.Y <= bottom &&
                     tri.V1.X >= left && tri.V1.X <= right && tri.V1.Y >= top && tri.V1.Y <= bottom &&
                     tri.V1.X >= left && tri.V1.X <= right && tri.V1.Y >= top && tri.V1.Y <= bottom);
               });
         }
         return triangleList;
      }

      /// <summary>
      /// Release the storage related to this triangulation
      /// </summary>
      protected override void DisposeObject()
      {
         _storage.Dispose();
      }
   }

   /// <summary>
   /// A Voronoi Facet
   /// </summary>
   public class VoronoiFacet //: IConvexPolygon<float>
   {
      /// <summary>
      /// Create a Voronoi facet using the specific <paramref name="point"/> and <paramref name="polyline"/>
      /// </summary>
      /// <param name="point">The point this facet associate with </param>
      /// <param name="polyline">The points that defines the contour of this facet</param>
      public VoronoiFacet(System.Drawing.PointF point, System.Drawing.PointF[] polyline)
      {
         _point = point;
         _vertices = polyline;

         //Debug.Assert(point.InConvexPolygon(this));
      }

      private System.Drawing.PointF _point;

      /// <summary>
      /// The point this facet associate to
      /// </summary>
      public System.Drawing.PointF Point
      {
         get { return _point; }
         set { _point = value; }
      }

      private System.Drawing.PointF[] _vertices;

      #region IConvexPolygon<float> Members
      /// <summary>
      /// Get or set the vertices of this facet
      /// </summary>
      public System.Drawing.PointF[] Vertices
      {
         get { return _vertices; }
         set { _vertices = value; }
      }
      #endregion
   }
}
