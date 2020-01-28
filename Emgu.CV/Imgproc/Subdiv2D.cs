//----------------------------------------------------------------------------
//  Copyright (C) 2004-2020 by EMGU Corporation. All rights reserved.       
//----------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Runtime.InteropServices;
using Emgu.CV.Structure;
using Emgu.CV.Util;
using Emgu.Util;

namespace Emgu.CV
{
    /// <summary>
    /// Planar Subdivision, can be use to compute Delaunnay's triangulation or Voroni diagram.
    /// </summary>
    public class Subdiv2D : UnmanagedObject
    {
        private readonly Rectangle _roi;

        #region constructor
        /// <summary>
        /// Start the Delaunay's triangulation in the specific region of interest.
        /// </summary>
        /// <param name="roi">The region of interest of the triangulation</param>
        public Subdiv2D(Rectangle roi)
        {
            _ptr = CvInvoke.cveSubdiv2DCreate(ref roi);
            _roi = roi;
        }

        /// <summary>
        /// Create a planar subdivision from the given points. The ROI is computed as the minimum bounding Rectangle for the input points
        /// </summary>
        /// <param name="silent">If true, any exception during insert will be ignored</param>
        /// <param name="points">The points to be inserted to this planar subdivision</param>
        public Subdiv2D(PointF[] points, bool silent = false)
        {
            #region Find the region of interest
            _roi = PointCollection.BoundingRectangle(points);
            #endregion

            _ptr = CvInvoke.cveSubdiv2DCreate(ref _roi);

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
            using (VectorOfPointF vpf = new VectorOfPointF(points))
                if (silent)
                {
#if !UNITY_IOS
                    //ignore all errors
                    IntPtr oldErrorCallback = CvInvoke.RedirectError(CvInvoke.CvErrorHandlerIgnoreError, IntPtr.Zero, IntPtr.Zero);
#endif
                    CvInvoke.cveSubdiv2DInsertMulti(_ptr, vpf);

#if !UNITY_IOS
                    //reset the error handler 
                    CvInvoke.RedirectError(oldErrorCallback, IntPtr.Zero, IntPtr.Zero);
#endif
                }
                else
                    CvInvoke.cveSubdiv2DInsertMulti(_ptr, vpf);
        }

        /// <summary>
        /// Insert a point to the triangulation. 
        /// </summary>
        /// <param name="point">The point to be inserted</param>
        public void Insert(PointF point)
        {
            CvInvoke.cveSubdiv2DInsertSingle(_ptr, ref point);
        }

        /// <summary>
        /// Locates input point within subdivision
        /// </summary>
        /// <param name="pt">The point to locate</param>
        /// <param name="subdiv2DEdge">The output edge the point falls onto or right to</param>
        /// <param name="subdiv2DPoint">Optional output vertex double pointer the input point coincides with</param>
        /// <returns>The type of location for the point</returns>
        public CvEnum.Subdiv2DPointLocationType Locate(PointF pt, out int subdiv2DEdge, out int subdiv2DPoint)
        {
            subdiv2DEdge = 0;
            subdiv2DPoint = 0;
            return CvInvoke.cveSubdiv2DLocate(_ptr, ref pt, ref subdiv2DEdge, ref subdiv2DPoint);
        }

        /// <summary>
        /// Finds subdivision vertex that is the closest to the input point. It is not necessarily one of vertices of the facet containing the input point, though the facet (located using cvSubdiv2DLocate) is used as a starting point.
        /// </summary>
        /// <param name="point">Input point</param>
        /// <param name="nearestPoint">The nearest subdivision vertex</param>
        /// <returns>The location type of the point</returns>
        public CvEnum.Subdiv2DPointLocationType FindNearest(PointF point, out PointF nearestPoint)
        {
            nearestPoint = new PointF();
            return CvInvoke.cveSubdiv2DFindNearest(_ptr, ref point, ref nearestPoint);
        }

        /// <summary>
        /// Obtains the list of Voronoi Facets 
        /// </summary>
        /// <param name="idx">Vector of vertices IDs to consider. For all vertices you can pass empty vector.</param>
        /// <returns>The list of Voronoi Facets</returns>
        public VoronoiFacet[] GetVoronoiFacets(int[] idx = null)
        {
            using (VectorOfInt vi = new VectorOfInt())
            using (VectorOfVectorOfPointF facetVec = new VectorOfVectorOfPointF())
            using (VectorOfPointF centerVec = new VectorOfPointF())
            {
                if (idx != null)
                    vi.Push(idx);

                CvInvoke.cveSubdiv2DGetVoronoiFacetList(_ptr, vi, facetVec, centerVec);
                PointF[][] vertices = facetVec.ToArrayOfArray();
                PointF[] centers = centerVec.ToArray();

                VoronoiFacet[] facets = new VoronoiFacet[centers.Length];
                for (int i = 0; i < facets.Length; i++)
                {
                    facets[i] = new VoronoiFacet(centers[i], vertices[i]);
                }
                return facets;
            }

        }

        /// <summary>
        /// Returns the triangles subdivision of the current planar subdivision. 
        /// </summary>
        /// <param name="includeVirtualPoints">If true, will include the virtual points in the resulting triangles</param>
        /// <remarks>The triangles might contains virtual points that do not belongs to the inserted points, if you do not want those points, set <paramref name="includeVirtualPoints"> to false</paramref></remarks>
        /// <returns>The triangles subdivision in the current planar subdivision</returns>
        public Triangle2DF[] GetDelaunayTriangles(bool includeVirtualPoints = false)
        {
            using (VectorOfTriangle2DF triangleVec = new VectorOfTriangle2DF())
            {
                CvInvoke.cveSubdiv2DGetTriangleList(_ptr, triangleVec);
                Triangle2DF[] result = triangleVec.ToArray();
                if (includeVirtualPoints)
                    return result;
                Rectangle r = new Rectangle(_roi.Location, new Size(_roi.Width + 1, _roi.Height + 1));
                return
                   Array.FindAll(result, (Triangle2DF t) =>
                   {
                       return
                      r.Contains(Point.Round(t.V0))
                      && r.Contains(Point.Round(t.V1))
                      && r.Contains(Point.Round(t.V2));
                   }
                );
            }
        }

        /// <summary>
        /// Release unmanaged resources
        /// </summary>
        protected override void DisposeObject()
        {
            if (IntPtr.Zero != _ptr)
                CvInvoke.cveSubdiv2DRelease(ref _ptr);
        }
    }

    public static partial class CvInvoke
    {
        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern IntPtr cveSubdiv2DCreate(ref Rectangle rect);

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern void cveSubdiv2DRelease(ref IntPtr subdiv);

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern void cveSubdiv2DInsertMulti(IntPtr subdiv, IntPtr points);

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern int cveSubdiv2DInsertSingle(IntPtr subdiv, ref PointF pt);

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern void cveSubdiv2DGetTriangleList(IntPtr subdiv, IntPtr triangleList);

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern void cveSubdiv2DGetVoronoiFacetList(IntPtr subdiv, IntPtr idx, IntPtr facetList, IntPtr facetCenters);

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern CvEnum.Subdiv2DPointLocationType cveSubdiv2DFindNearest(IntPtr subdiv, ref PointF pt, ref PointF nearestPt);

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern CvEnum.Subdiv2DPointLocationType cveSubdiv2DLocate(IntPtr subdiv, ref PointF pt, ref int edge, ref int vertex);

    }
}
