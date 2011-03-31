//----------------------------------------------------------------------------
//  Copyright (C) 2004-2011 by EMGU. All rights reserved.       
//----------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Text;
using Emgu.CV.Structure;
using Emgu.Util;

namespace Emgu.CV
{
   /// <summary>
   /// Plannar Subdivision, can be use to compute Delaunnay's triangulation or Voroni diagram.
   /// </summary>
   public class PlanarSubdivision : UnmanagedObject, IEnumerable<MCvQuadEdge2D>
   {
      private readonly MemStorage _storage;
      private readonly Rectangle _roi;
      private bool _isVoronoiDirty;

      #region PInvoke
      [DllImport(CvInvoke.EXTERN_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      private static extern void PlanarSubdivisionGetTriangles(IntPtr subdiv, IntPtr triangles, ref int triangleCount, int includeVirtualPoints);

      [DllImport(CvInvoke.EXTERN_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      private static extern void PlanarSubdivisionInsertPoints(IntPtr subdiv, IntPtr points, int count);

      [DllImport(CvInvoke.EXTERN_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      private static extern int PlanarSubdivisionGetSubdiv2DPoints(IntPtr subdiv, IntPtr points, IntPtr edges, ref int count);

      [DllImport(CvInvoke.EXTERN_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      private static extern void PlanarSubdivisionEdgeToPoly(MCvSubdiv2DEdge edge, IntPtr buffer);
      #endregion

      #region constructor
      /// <summary>
      /// Start the Delaunay's triangulation in the specific region of interest.
      /// </summary>
      /// <param name="roi">The region of interest of the triangulation</param>
      public PlanarSubdivision(ref Rectangle roi)
      {
         _storage = new MemStorage();
         _ptr = CvInvoke.cvCreateSubdivDelaunay2D(roi, _storage);
         _roi = roi;
      }

      /// <summary>
      /// Create a planar subdivision from the given points. The ROI is computed as the minimun bounding Rectangle for the input points
      /// </summary>
      /// <param name="points">The points for this planar subdivision</param>
      public PlanarSubdivision(PointF[] points)
         : this(points, false)
      {
      }

      /// <summary>
      /// Create a planar subdivision from the given points. The ROI is computed as the minimun bounding Rectangle for the input points
      /// </summary>
      /// <param name="silent">If true, any exception during insert will be ignored</param>
      /// <param name="points">The points to be inserted to this planar subdivision</param>
      public PlanarSubdivision(PointF[] points, bool silent)
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
      public void Insert(PointF point)
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
      public CvEnum.Subdiv2DPointLocationType Locate(ref PointF pt, out MCvSubdiv2DEdge? subdiv2DEdge, out MCvSubdiv2DPoint? subdiv2DPoint)
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

      /// <summary>
      /// Finds subdivision vertex that is the closest to the input point. It is not necessarily one of vertices of the facet containing the input point, though the facet (located using cvSubdiv2DLocate) is used as a starting point.
      /// </summary>
      /// <param name="point">Input point</param>
      /// <returns>The nearest subdivision vertex</returns>
      public MCvSubdiv2DPoint FindNearestPoint2D(ref PointF point)
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
         List<VoronoiFacet> res = new List<VoronoiFacet>(GetVoronoiFacetsHelper());
         return res.ToArray();
      }

      /// <summary>
      /// Obtains the list of Voronoi Facets 
      /// </summary>
      /// <returns>The list of Voronoi Facets</returns>
      private IEnumerable<VoronoiFacet> GetVoronoiFacetsHelper()
      {
         if (_isVoronoiDirty)
         {
            CvInvoke.cvCalcSubdivVoronoi2D(Ptr);
            _isVoronoiDirty = false;
         }

         int size = MCvSubdiv2D.total;
         PointF[] points = new PointF[size];
         MCvSubdiv2DEdge[] edges = new MCvSubdiv2DEdge[size];
         GCHandle pointHandle = GCHandle.Alloc(points, GCHandleType.Pinned);
         GCHandle edgeHandle = GCHandle.Alloc(edges, GCHandleType.Pinned);
         PlanarSubdivisionGetSubdiv2DPoints(_ptr, pointHandle.AddrOfPinnedObject(), edgeHandle.AddrOfPinnedObject(), ref size);
         pointHandle.Free();
         edgeHandle.Free();
         using (MemStorage stor = new MemStorage())
         {
            Seq<PointF> ptSeq = new Seq<PointF>(stor);
            for (int i = 0; i < size; i++)
            {
               PlanarSubdivisionEdgeToPoly(edges[i], ptSeq);
               PointF[] polygon = ptSeq.ToArray();
               if (polygon.Length > 0)
               {
                  yield return new VoronoiFacet(points[i], polygon);
               }
            }
         }
      }

      /// <summary>
      /// Retruns the triangles subdivision of the current planar subdivision. 
      /// </summary>
      /// <param name="includeVirtualPoints">Indicates if virtual points should be included or not</param>
      /// <remarks>The triangles might contains virtual points that do not belongs to the inserted points, if you do not want those points, set <paramref name="includeVirtualPoints"> to false</paramref></remarks>
      /// <returns>The triangles subdivision in the current plannar subdivision</returns>
      public Triangle2DF[] GetDelaunayTriangles(bool includeVirtualPoints)
      {
         int size = ((MCvSet)Marshal.PtrToStructure(MCvSubdiv2D.edges, typeof(MCvSet))).total * 2;
         Triangle2DF[] triangles = new Triangle2DF[size];
         GCHandle handle = GCHandle.Alloc(triangles, GCHandleType.Pinned);
         PlanarSubdivisionGetTriangles(_ptr, handle.AddrOfPinnedObject(), ref size, includeVirtualPoints ? 1 : 0);
         handle.Free();
         Array.Resize(ref triangles, size);
         return triangles;
      }

      /// <summary>
      /// Release unmanaged resources
      /// </summary>
      protected override void DisposeObject()
      {
      }

      /// <summary>
      /// Release the storage related to this triangulation
      /// </summary>
      protected override void ReleaseManagedResources()
      {
         _storage.Dispose();
      }

      #region IEnumerable<MCvQuadEdge2D> Members
      /// <summary>
      /// Get an enumerator of the QuadEdges in this plannar subdivision
      /// </summary>
      /// <returns>An enumerator of all MCvQuadEdge2D</returns>
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
      private PointF _point;
      private PointF[] _vertices;

      /// <summary>
      /// Create a Voronoi facet using the specific <paramref name="point"/> and <paramref name="polyline"/>
      /// </summary>
      /// <param name="point">The point this facet associate with </param>
      /// <param name="polyline">The points that defines the contour of this facet</param>
      public VoronoiFacet(PointF point, PointF[] polyline)
      {
         _point = point;
         _vertices = polyline;

         //Debug.Assert(point.InConvexPolygon(this));
      }

      /// <summary>
      /// The point this facet associates to
      /// </summary>
      public PointF Point
      {
         get { return _point; }
         set { _point = value; }
      }

      /// <summary>
      /// Get or set the vertices of this facet
      /// </summary>
      public PointF[] Vertices
      {
         get { return _vertices; }
         set { _vertices = value; }
      }
   }
}
