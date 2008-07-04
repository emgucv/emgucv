using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;

namespace Emgu.CV
{
    public struct MCvSubdiv2DEdge
    {
        public long edge;

        public MCvSubdiv2DPoint cvSubdiv2DEdgeOrg()
        {
            IntPtr ptr = unchecked( (IntPtr)(edge & ~3) );
            MCvQuadEdge2D qe = (MCvQuadEdge2D)Marshal.PtrToStructure(ptr, typeof(MCvQuadEdge2D));
            MCvSubdiv2DPoint pt = (MCvSubdiv2DPoint)Marshal.PtrToStructure(qe.pt[edge & 3], typeof(MCvSubdiv2DPoint));
            return pt;
        }

        public MCvSubdiv2DPoint cvSubdiv2DEdgeDst()
        {
            IntPtr ptr = (IntPtr) (edge & 0xffff);
            MCvQuadEdge2D qe = (MCvQuadEdge2D)Marshal.PtrToStructure( ptr, typeof(MCvQuadEdge2D));
            MCvSubdiv2DPoint pt = (MCvSubdiv2DPoint)Marshal.PtrToStructure(qe.pt[(edge +2) & 3], typeof(MCvSubdiv2DPoint));
            return pt;
        }
    }
}
