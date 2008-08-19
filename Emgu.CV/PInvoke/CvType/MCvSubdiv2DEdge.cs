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
         IntPtr ptr = (IntPtr)(edge.ToInt64() >> 2 << 2);
         MCvQuadEdge2D qe = (MCvQuadEdge2D)Marshal.PtrToStructure(ptr, typeof(MCvQuadEdge2D));
         IntPtr pointPtr = qe.pt[(edge.ToInt64() & 3)];
         if (pointPtr == IntPtr.Zero)
         {
            MCvSubdiv2DPoint pt = new MCvSubdiv2DPoint();
            pt.flags = -1;
            return pt; // return an invalid point
         }
         else
         {
            MCvSubdiv2DPoint pt = (MCvSubdiv2DPoint)Marshal.PtrToStructure(pointPtr, typeof(MCvSubdiv2DPoint));
            return pt;
         }
      }

      /// <summary>
      /// similar to cvSubdiv2DEdgeDst
      /// </summary>
      /// <returns></returns>
      public MCvSubdiv2DPoint cvSubdiv2DEdgeDst()
      {
         IntPtr ptr = (IntPtr)(edge.ToInt64() >> 2 << 2);
         MCvQuadEdge2D qe = (MCvQuadEdge2D)Marshal.PtrToStructure(ptr, typeof(MCvQuadEdge2D));
         IntPtr pointPtr = qe.pt[(edge.ToInt64() + 2) & 3];
         if (pointPtr == IntPtr.Zero)
         {
            MCvSubdiv2DPoint pt = new MCvSubdiv2DPoint();
            pt.flags = -1;
            return pt; // return an invalid point
         }
         else
         {
            MCvSubdiv2DPoint pt = (MCvSubdiv2DPoint)Marshal.PtrToStructure(pointPtr, typeof(MCvSubdiv2DPoint));
            return pt;
         }
      }

      /// <summary>
      /// Similar to cvSubdiv2DRotateEdge
      /// </summary>
      /// <param name="rotate">
      /// Specifies, which of edges of the same quad-edge as the input one to return, one of:
      /// 0 - the input edge (e if e is the input edge) 
      /// 1 - the rotated edge (eRot) 
      /// 2 - the reversed edge (reversed e (in green)) 
      /// 3 - the reversed rotated edge (reversed eRot (in green)) 
      ///</param>
      /// <returns>The rotated edge</returns>
      public MCvSubdiv2DEdge cvSubdiv2DRotateEdge(int rotate)
      {
         MCvSubdiv2DEdge e = new MCvSubdiv2DEdge();
         e.edge = new IntPtr((edge.ToInt64() & ~3) + ((edge.ToInt64() + rotate) & 3));
         return e;
      }

      /// <summary>
      /// Similar to cvSubdiv2DGetEdge
      /// </summary>
      /// <param name="type">The next edge type</param>
      /// <returns>The next edge</returns>
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
