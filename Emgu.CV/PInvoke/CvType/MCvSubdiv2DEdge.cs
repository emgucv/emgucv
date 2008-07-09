using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;

namespace Emgu.CV
{
    /// <summary>
    /// Wrapped CvSubdiv2DEdge structure
    /// </summary>
    public struct MCvSubdiv2DEdge
    {
        /// <summary>
        /// one of edges within quad-edge, lower 2 bits is index (0..3) and upper bits are quad-edge pointer 
        /// </summary>
        public IntPtr edge;

        /// <summary>
        /// similar to cvSubdiv2DEdgeOrg
        /// </summary>
        /// <returns></returns>
        public MCvSubdiv2DPoint cvSubdiv2DEdgeOrg()
        {
            IntPtr ptr = (IntPtr)(edge.ToInt64() >> 2 << 2 );
            MCvQuadEdge2D qe = (MCvQuadEdge2D)Marshal.PtrToStructure(ptr, typeof(MCvQuadEdge2D));
            MCvSubdiv2DPoint pt = (MCvSubdiv2DPoint)Marshal.PtrToStructure(qe.pt[ (edge.ToInt64() & 3) ], typeof(MCvSubdiv2DPoint));
            return pt;
        }
        
        /// <summary>
        /// similar to cvSubdiv2DEdgeDst
        /// </summary>
        /// <returns></returns>
        public MCvSubdiv2DPoint cvSubdiv2DEdgeDst()
        {
            IntPtr ptr = (IntPtr) (edge.ToInt64() >> 2 << 2);
            MCvQuadEdge2D qe = (MCvQuadEdge2D)Marshal.PtrToStructure( ptr, typeof(MCvQuadEdge2D));
            MCvSubdiv2DPoint pt = (MCvSubdiv2DPoint)Marshal.PtrToStructure(qe.pt[ (edge.ToInt64() +2) & 3], typeof(MCvSubdiv2DPoint));
            return pt;
        }

        /// <summary>
        /// similar to cvSubdiv2DGetEdge
        /// </summary>
        /// <param name="type">the next edge type</param>
        /// <returns>the next edge</returns>
        public MCvSubdiv2DEdge cvSubdiv2DGetEdge(CvEnum.CV_NEXT_EDGE_TYPE type)
        {
            IntPtr ptr = (IntPtr)(edge.ToInt64() >> 2 << 2);
            MCvQuadEdge2D qe = (MCvQuadEdge2D)Marshal.PtrToStructure(ptr, typeof(MCvQuadEdge2D));
            Int64 edgePtr = qe.next[(edge.ToInt64() + (int)type) & 3].edge.ToInt64();
            edgePtr = (edgePtr >> 2 << 2) + ((edgePtr + ((int)type >> 4)) & 3);

            MCvSubdiv2DEdge e = new MCvSubdiv2DEdge();
            e.edge = new IntPtr(edgePtr);
            return e;
        }
    }
}
