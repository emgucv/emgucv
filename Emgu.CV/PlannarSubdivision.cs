using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using System.Diagnostics;
using Emgu.Util;

namespace Emgu.CV
{
   /// <summary>
   /// Plannar Subdivision
   /// </summary>
   public class PlanarSubdivision : UnmanagedObject
   {
      private readonly MemStorage _storage;
      private readonly Rectangle<float> _roi;

      private bool _isVoronoiDirty;

      #region constructor
      /// <summary>
      /// Start the Delaunay's triangulation in the specific region of interest.
      /// </summary>
      /// <param name="roi">The region of interest of the triangulation</param>
      public PlanarSubdivision(Rectangle<float> roi)
      {
         _storage = new MemStorage();
         _ptr = CvInvoke.cvCreateSubdivDelaunay2D(roi.MCvRect, _storage);
         _roi = roi;
      }

      /// <summary>
      /// Create a planar subdivision from the given points. The ROI is computed as the minimun bounding Rectangle for the input points
      /// </summary>
      /// <param name="points">The points for this planar subdivision</param>
      public PlanarSubdivision(IEnumerable<Point2D<float>> points)
         : this(points, false)
      {
      }

      /// <summary>
      /// Create a planar subdivision from the given points. The ROI is computed as the minimun bounding Rectangle for the input points
      /// </summary>
      /// <param name="silent">If true, any exception during insert will be ignored</param>
      /// <param name="points">The points for this planar subdivision</param>
      public PlanarSubdivision(IEnumerable<Point2D<float>> points, bool silent)
      {
         #region Find the region of interest
         Rectangle<float> roi;
         using (MemStorage storage = new MemStorage())
         using (Seq<MCvPoint2D32f> seq = PointCollection.To2D32fSequence(storage, Emgu.Util.Toolbox.IEnumConvertor<Point2D<float>, Point<float>>(points, delegate(Point2D<float> p) { return (Point<float>)p; })))
         {
            MCvRect cvRect = CvInvoke.cvBoundingRect(seq.Ptr, true);
            roi = new Rectangle<float>(cvRect);
         }
         #endregion

         _storage = new MemStorage();
         _ptr = CvInvoke.cvCreateSubdivDelaunay2D(roi.MCvRect, _storage);
         _roi = roi;

         if (silent)
         {
            foreach (Point2D<float> p in points)
            {
               MCvPoint2D32f cvPoint = p.MCvPoint2D32f;
               try
               {
                  Insert(ref cvPoint);
               }
               catch (CvException)
               { }
            }
         }
         else
         {
            foreach (Point2D<float> p in points)
            {
               MCvPoint2D32f cvPoint = p.MCvPoint2D32f;
               Insert(ref cvPoint);
            }
         }
      }
      #endregion

      /// <summary>
      /// Insert a point to the triangulation. If the point is already inserted, no changes will be made.
      /// </summary>
      /// <param name="point">The point to be inserted</param>
      public void Insert(ref MCvPoint2D32f point)
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
      public CvEnum.Subdiv2DPointLocationType Locate(ref MCvPoint2D32f pt, out MCvSubdiv2DEdge? subdiv2DEdge, out MCvSubdiv2DPoint? subdiv2DPoint)
      {
         IntPtr edge, vertex = new IntPtr();
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

      private static Triangle2D<float> EdgeToTriangle(ref MCvSubdiv2DEdge e)
      {
         MCvSubdiv2DPoint v1 = e.cvSubdiv2DEdgeOrg();
         MCvSubdiv2DPoint v2 = e.cvSubdiv2DEdgeDst();

         MCvSubdiv2DEdge eLnext = e.cvSubdiv2DGetEdge(Emgu.CV.CvEnum.CV_NEXT_EDGE_TYPE.CV_NEXT_AROUND_LEFT);
         MCvSubdiv2DPoint v3 = eLnext.cvSubdiv2DEdgeDst();
         return new Triangle2D<float>(
             new Point2D<float>(v1.pt.x, v1.pt.y),
             new Point2D<float>(v3.pt.x, v3.pt.y),
             new Point2D<float>(v2.pt.x, v2.pt.y));
      }

      private static List<VoronoiFacet> EdgeToFacets(ref MCvQuadEdge2D quadEdge)
      {
         List<VoronoiFacet> facets = new List<VoronoiFacet>();

         MCvSubdiv2DEdge e1 = quadEdge.next[0].cvSubdiv2DRotateEdge(1);
         Point2D<float>[] p1 = EdgeToPoly(ref e1);
         if (p1 != null)
         {
            MCvSubdiv2DPoint pt = quadEdge.next[0].cvSubdiv2DEdgeOrg();
            facets.Add(new VoronoiFacet(new Point2D<float>(pt.pt.x, pt.pt.y), p1));
         }

         MCvSubdiv2DEdge e2 = quadEdge.next[0].cvSubdiv2DRotateEdge(3);
         Point2D<float>[] p2 = EdgeToPoly(ref e2);
         if (p2 != null)
         {
            MCvSubdiv2DPoint pt = quadEdge.next[0].cvSubdiv2DEdgeDst();
            facets.Add(new VoronoiFacet(new Point2D<float>(pt.pt.x, pt.pt.y), p2));
         }

         return facets;
      }

      private static Point2D<float>[] EdgeToPoly(ref MCvSubdiv2DEdge e)
      {
         MCvSubdiv2DPoint v0 = e.cvSubdiv2DEdgeOrg();
         if (!v0.isValid) return null;

         List<Point2D<float>> list = new List<Point2D<float>>();
         Point2D<float> startPoint = new Point2D<float>(v0.pt.x, v0.pt.y);
         list.Add(startPoint);

         for (MCvSubdiv2DEdge currentEdge = e; ; currentEdge = currentEdge.cvSubdiv2DGetEdge(Emgu.CV.CvEnum.CV_NEXT_EDGE_TYPE.CV_NEXT_AROUND_LEFT))
         {
            MCvSubdiv2DPoint v = currentEdge.cvSubdiv2DEdgeDst();
            if (!v.isValid) return null;

            Point2D<float> currentPoint = new Point2D<float>(v.pt.x, v.pt.y);
            if (currentPoint.Equals(startPoint))
               break;

            list.Add(currentPoint);
         }
         return list.ToArray();
      }

      /// <summary>
      /// Finds subdivision vertex that is the closest to the input point. It is not necessarily one of vertices of the facet containing the input point, though the facet (located using cvSubdiv2DLocate) is used as a starting point.
      /// </summary>
      /// <param name="point">Input point</param>
      /// <returns>returns the found subdivision vertex</returns>
      public MCvSubdiv2DPoint FindNearestPoint2D(ref MCvPoint2D32f point)
      {
         IntPtr ptr = CvInvoke.cvFindNearestPoint2D(Ptr, point);
         return (MCvSubdiv2DPoint)Marshal.PtrToStructure(ptr, typeof(MCvSubdiv2DPoint));
      }

      /// <summary>
      /// Returns the triangles of the Delaunay's triangulation
      /// </summary>
      /// <remarks>The vertices of the triangles all belongs to the inserted points</remarks>
      /// <returns>The result of the current triangulation</returns>
      public List<Triangle2D<float>> GetDelaunayTriangles()
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

         Dictionary<String, string> facetDict = new Dictionary<string, string>();
        
         List<VoronoiFacet> facetList = new List<VoronoiFacet>();

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

               List<VoronoiFacet> facet1 = EdgeToFacets(ref quadEdge);

               foreach (VoronoiFacet facet in facet1)
               {
                  if (facet.Point.InConvexPolygon(_roi))
                  {
                     Point2D<float> p = facet.Point;
                     if (InsertPoint2DToDictionary(p, facetDict))
                     {
                        facetList.Add(facet);
                     }
                  }
               }
            }

            CvInvoke.CV_NEXT_SEQ_ELEM(elem_size, ref reader);
         }
         return facetList;
      }

      
      /// <summary>
      /// Insert the point into the dictionary. If the point already exist, return false. Otherwise return true.
      /// </summary>
      /// <param name="pt">The point to insert</param>
      /// <param name="dic">The point dictionary</param>
      /// <returns>If the point already exist, return false. Otherwise return true.</returns>
      private static bool InsertPoint2DToDictionary<T>(Point2D<T> pt, Dictionary<string, string> dic) where T: IComparable, new() 
      {
         string key = String.Format("{0},{1}", pt.X.ToString(), pt.Y.ToString());
         if (dic.ContainsKey(key)) 
         {
            return false;
         } else
         {
            dic.Add(key, null);
            return true;
         }
      }

      /// <summary>
      /// Retruns the triangles subdivision of the current planar subdivision. 
      /// </summary>
      /// <remarks>The triangles might contains virtual points that do not belongs to the inserted points, if you do not want those points, set <param name="includeVirtualPoints"> to false</param></remarks>
      /// <returns>The triangles subdivision in the current plannar subdivision</returns>
      public List<Triangle2D<float>> GetDelaunayTriangles(bool includeVirtualPoints)
      {
         Dictionary<string, string> triangleDic = new Dictionary<string, string>();

         List<Triangle2D<float>> triangleList = new List<Triangle2D<float>>();

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

               Triangle2D<float> tri1 = EdgeToTriangle(ref quadEdge.next[0]);

               if (InsertPoint2DToDictionary(tri1.Centeroid, triangleDic))
               {
                  triangleList.Add(tri1);
               }

               Triangle2D<float> tri2 = EdgeToTriangle(ref quadEdge.next[2]);
               if (InsertPoint2DToDictionary(tri2.Centeroid, triangleDic))
               {
                  triangleList.Add(tri2);
               }
            }

            CvInvoke.CV_NEXT_SEQ_ELEM(elem_size, ref reader);
         }

         if (includeVirtualPoints)
         {
            return triangleList;
         } else
         {
            return triangleList.FindAll(
               delegate(Triangle2D<float> tri) 
               { return Util.IsConvexPolygonInConvexPolygon(tri, _roi); });
         }
      }

      /*
      /// <summary>
      /// Determine if a polygon is inside a triangle
      /// </summary>
      /// <param name="triangle">The triangle</param>
      /// <param name="polygon">The polygon</param>
      /// <returns>True if the polygon is inside the triangle; false otherwise</returns>
      private static bool IsPolygonInsideTriangle(Triangle2D<float> triangle, IConvexPolygon<float> polygon )
      {
         bool allTriangleVerticesOutside = true;
         foreach (Point2D<float> pt in triangle.Vertices)
         {
            allTriangleVerticesOutside &= (!pt.InConvexPolygon(polygon));
         }
         return allTriangleVerticesOutside;
      }*/

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
   public class VoronoiFacet : IConvexPolygon<float>
   {
      /// <summary>
      /// Create a Voronoi facet using the specific <paramref name="point"/> and <paramref name="polyline"/>
      /// </summary>
      /// <param name="point">The point this facet associate with </param>
      /// <param name="polyline">The points that defines the contour of this facet</param>
      public VoronoiFacet(Point2D<float> point, Point2D<float>[] polyline)
      {
         _point = point;
         _vertices = polyline;

         Debug.Assert(point.InConvexPolygon(this));
      }

      private Point2D<float> _point;

      /// <summary>
      /// The point this facet associate to
      /// </summary>
      public Point2D<float> Point
      {
         get { return _point; }
         set { _point = value; }
      }

      private Point2D<float>[] _vertices;

      #region IConvexPolygon<float> Members
      /// <summary>
      /// Get or set the vertices of this facet
      /// </summary>
      public Point2D<float>[] Vertices
      {
         get { return _vertices; }
         set { _vertices = value; }
      }
      #endregion
   }
}
