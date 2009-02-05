using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using System.Diagnostics;
using Emgu.Util;
using Emgu.CV.Structure;
using System.Drawing;

namespace Emgu.CV
{
   /// <summary>
   /// Plannar Subdivision
   /// </summary>
   public class PlanarSubdivision : UnmanagedObject, IEnumerable<MCvQuadEdge2D>
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
      /// <param name="points">The points to be inserted to this planar subdivision</param>
      public PlanarSubdivision(System.Drawing.PointF[] points, bool silent)
      {
         #region Find the region of interest
         _roi = PointCollection.BoundingRectangle(points);
         #endregion

         _storage = new MemStorage();
         _ptr = CvInvoke.cvCreateSubdivDelaunay2D(_roi, _storage);

         Insert(points, silent);
      }
      #endregion

      /// <summary>
      /// Insert a collection of points to this planar subdivision
      /// </summary>
      /// <param name="points">The points to be inserted to this planar subdivision</param>
      /// <param name="silent">If true, any exception during insert will be ignored</param>
      public void Insert(PointF[] points, bool silent)
      {
         GCHandle handle = GCHandle.Alloc(points, GCHandleType.Pinned);

         if (silent)
         {
            //ignore all errors
            IntPtr oldErrorCallback = CvInvoke.cvRedirectError(CvInvoke.CvErrorHandlerIgnoreError, IntPtr.Zero, IntPtr.Zero);

            PlanarSubdivisionInsertPoints(_ptr, handle.AddrOfPinnedObject(), points.Length);

            //reset the error handler 
            CvInvoke.cvRedirectError(oldErrorCallback, IntPtr.Zero, IntPtr.Zero);
         }
         else
            PlanarSubdivisionInsertPoints(_ptr, handle.AddrOfPinnedObject(), points.Length);

         handle.Free();

         _isVoronoiDirty = true;
      }

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

      [DllImport(CvInvoke.EXTERN_LIBRARY)]
      private static extern void PlanarSubdivisionGetTriangles(IntPtr subdiv, IntPtr triangles, ref int triangleCount, bool includeVirtualPoints);

      [DllImport(CvInvoke.EXTERN_LIBRARY)]
      private static extern void PlanarSubdivisionInsertPoints(IntPtr subdiv, IntPtr points, int count);

      [DllImport(CvInvoke.EXTERN_LIBRARY)]
      private static extern int PlanarSubdivisionGetSubdiv2DPoints(IntPtr subdiv, IntPtr points, IntPtr edges, ref int count);

      private static System.Drawing.PointF[] EdgeToPoly(MCvSubdiv2DEdge currentEdge, List<System.Drawing.PointF> bufferList)
      {
         MCvSubdiv2DPoint v0 = currentEdge.cvSubdiv2DEdgeOrg();
         if (!v0.IsValid) return null;

         bufferList.Clear();

         for (; ; currentEdge = currentEdge.cvSubdiv2DGetEdge(Emgu.CV.CvEnum.CV_NEXT_EDGE_TYPE.CV_NEXT_AROUND_LEFT))
         {
            MCvSubdiv2DPoint v = currentEdge.cvSubdiv2DEdgeDst();
            if (!v.IsValid) return null;

            bufferList.Add(v.pt);

            if (v.pt.Equals(v0.pt)) //reach the starting point
               break;
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
      public Triangle2DF[] GetDelaunayTriangles()
      {
         return GetDelaunayTriangles(false);
      }

      /// <summary>
      /// Obtains the list of Voronoi Facets 
      /// </summary>
      /// <returns>The list of Voronoi Facets</returns>
      public VoronoiFacet[] GetVoronoiFacets()
      {
         if (_isVoronoiDirty)
         {
            CvInvoke.cvCalcSubdivVoronoi2D(Ptr);
            _isVoronoiDirty = false;
         }

         {  //slower C# implementation, works correctly in both DEBUG and RELEASE
            int left = _roi.X, top = _roi.Y, right = _roi.X + _roi.Width, bottom = _roi.Y + _roi.Height;
            Dictionary<System.Drawing.PointF, Byte> facetDict = new Dictionary<System.Drawing.PointF, Byte>();
            List<VoronoiFacet> facetList = new List<VoronoiFacet>();
            List<System.Drawing.PointF> bufferList = new List<System.Drawing.PointF>();

            foreach (MCvQuadEdge2D quadEdge in this)
            {
               MCvSubdiv2DEdge nextQuadEdge = quadEdge.next[0];

               System.Drawing.PointF pt1 = nextQuadEdge.cvSubdiv2DEdgeOrg().pt;
               if (pt1.X >= left && pt1.X <= right && pt1.Y >= top && pt1.Y <= bottom
                  && InsertPoint2DToDictionary(pt1, facetDict))
               {
                  MCvSubdiv2DEdge e1 = nextQuadEdge.cvSubdiv2DRotateEdge(1);
                  PointF[] p1 = EdgeToPoly(e1, bufferList);
                  if (p1 != null)
                  {
                     facetList.Add(new VoronoiFacet(pt1, p1));
                  }
               }

               System.Drawing.PointF pt2 = nextQuadEdge.cvSubdiv2DEdgeDst().pt;
               if (pt2.X >= left && pt2.X <= right && pt2.Y >= top && pt2.Y <= bottom
                  && InsertPoint2DToDictionary(pt2, facetDict))
               {
                  MCvSubdiv2DEdge e2 = nextQuadEdge.cvSubdiv2DRotateEdge(3);
                  PointF[] p2 = EdgeToPoly(e2, bufferList);
                  if (p2 != null)
                  {
                     facetList.Add(new VoronoiFacet(pt2, p2));
                  }
               }
            }
            return facetList.ToArray();
         }

         /*
         {  //alternative high-performance method, works correctly in DEBUG but not RELEASE
            int size = MCvSubdiv2D.total; 
            PointF[] points = new PointF[size];
            MCvSubdiv2DEdge[] edges = new MCvSubdiv2DEdge[size];
            GCHandle pointHandle = GCHandle.Alloc(points, GCHandleType.Pinned);
            GCHandle edgeHandle = GCHandle.Alloc(edges, GCHandleType.Pinned);
            
            PlanarSubdivisionGetSubdiv2DPoints(_ptr, pointHandle.AddrOfPinnedObject() , edgeHandle.AddrOfPinnedObject(), ref size);
            pointHandle.Free();
            edgeHandle.Free();

            List<System.Drawing.PointF> buffer = new List<System.Drawing.PointF>();
            VoronoiFacet[] facets = new VoronoiFacet[size];
            
            for (int i = 0; i < size; i++)
            {
               
               System.Drawing.PointF[] polygon = EdgeToPoly(edges[i], buffer);
               
               if (polygon != null)
                  facets[i] = new VoronoiFacet(points[i], polygon);
            }
            return facets;
         }*/
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
      public Triangle2DF[] GetDelaunayTriangles(bool includeVirtualPoints)
      {
         int size = ((MCvSet)Marshal.PtrToStructure(MCvSubdiv2D.edges, typeof(MCvSet))).total * 2;
         Triangle2DF[] triangles = new Triangle2DF[size];
         GCHandle handle = GCHandle.Alloc(triangles, GCHandleType.Pinned);
         PlanarSubdivisionGetTriangles(_ptr, handle.AddrOfPinnedObject(), ref size, includeVirtualPoints);
         handle.Free();
         Array.Resize(ref triangles, size);
         return triangles;
      }

      /// <summary>
      /// Release the storage related to this triangulation
      /// </summary>
      protected override void DisposeObject()
      {
         _storage.Dispose();
      }

      #region IEnumerable<MCvQuadEdge2D> Members
      /// <summary>
      /// Get an enumerator of the QuadEdges in this plannar subdivision
      /// </summary>
      /// <returns></returns>
      public IEnumerator<MCvQuadEdge2D> GetEnumerator()
      {
         IntPtr subdivEdges = MCvSubdiv2D.edges;
         int elemSize = ((MCvSet)Marshal.PtrToStructure(subdivEdges, typeof(MCvSet))).elem_size;
         MCvSeqReader reader = new MCvSeqReader();
         CvInvoke.cvStartReadSeq(subdivEdges, ref reader, false);

         for (; CvInvoke.CV_IS_SET_ELEM(reader.ptr); CvInvoke.CV_NEXT_SEQ_ELEM(elemSize, ref reader))
         {
            yield return (MCvQuadEdge2D)Marshal.PtrToStructure(reader.ptr, typeof(MCvQuadEdge2D));
         }
      }

      #endregion

      #region IEnumerable Members

      System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
      {
         return GetEnumerator();
      }

      #endregion
   }

   /// <summary>
   /// A Voronoi Facet
   /// </summary>
   public class VoronoiFacet
   {
      private System.Drawing.PointF _point;
      private System.Drawing.PointF[] _vertices;

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

      /// <summary>
      /// The point this facet associates to
      /// </summary>
      public System.Drawing.PointF Point
      {
         get { return _point; }
         set { _point = value; }
      }

      /// <summary>
      /// Get or set the vertices of this facet
      /// </summary>
      public System.Drawing.PointF[] Vertices
      {
         get { return _vertices; }
         set { _vertices = value; }
      }
   }
}
