using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;

namespace Emgu.CV
{
    public class DelaunayTriangulation : UnmanagedObject
    {
        private MemStorage _storage;

        public DelaunayTriangulation(Rectangle<double> roi)
        {
            _storage = new MemStorage();
            _ptr = CvInvoke.cvCreateSubdivDelaunay2D(roi.MCvRect, _storage);
        }

        public void Insert(MCvPoint2D32f point)
        {
            CvInvoke.cvSubdivDelaunay2DInsert(_ptr, point);
        }

        public MCvSubdiv2D MCvSubdiv2D
        {
            get { return (MCvSubdiv2D) Marshal.PtrToStructure(_ptr, typeof(MCvSubdiv2D)); }
        }
           

        public void QuadEdges()
        {
            MCvSeqReader reader = new MCvSeqReader();
            MCvSubdiv2D subdiv = MCvSubdiv2D;
            MCvSet set = (MCvSet) Marshal.PtrToStructure( subdiv.edges, typeof(MCvSet));
            int i, total = set.total; 
            int elem_size = set.elem_size;
            CvInvoke.cvStartReadSeq(subdiv.edges, ref reader, false);

            for (i = 0; i < total; i++)
            {
                IntPtr edge = reader.ptr;
                
                if (CvInvoke.CV_IS_SET_ELEM(edge))
                {
                    MCvSubdiv2DEdge e = (MCvSubdiv2DEdge) Marshal.PtrToStructure(edge, typeof(MCvSubdiv2DEdge));
                    MCvSubdiv2DPoint p1 = e.cvSubdiv2DEdgeDst();
                    //draw_subdiv_edge(img, (CvSubdiv2DEdge)edge + 1, voronoi_color);
                    //draw_subdiv_edge(img, (CvSubdiv2DEdge)edge, delaunay_color);
                }

                CvInvoke.CV_NEXT_SEQ_ELEM(elem_size, ref reader);
            }
        }

        protected override void DisposeObject()
        {         
        }
    }
}
