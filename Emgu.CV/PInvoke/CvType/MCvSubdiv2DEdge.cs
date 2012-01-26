//----------------------------------------------------------------------------
//  Copyright (C) 2004-2012 by EMGU. All rights reserved.       
//----------------------------------------------------------------------------

using System;
using System.Runtime.InteropServices;

namespace Emgu.CV.Structure
{
   /// <summary>
   /// Wrapped CvSubdiv2DEdge structure
   /// </summary>
   [StructLayout(LayoutKind.Sequential)]
   public struct MCvSubdiv2DEdge
   {
      /// <summary>
      /// one of edges within quad-edge, lower 2 bits is index (0..3) and upper bits are quad-edge pointer 
      /// </summary>
      public IntPtr edge;

      /// <summary>
      /// Create a MCvSubdiv2DEdge from the specific edge
      /// </summary>
      /// <param name="e">the edge</param>
      public MCvSubdiv2DEdge(IntPtr e)
      {
         edge = e;
      }

      /// <summary>
      /// similar to cvSubdiv2DEdgeOrg
      /// </summary>
      /// <returns></returns>
      public MCvSubdiv2DPoint cvSubdiv2DEdgeOrg()
      {
         Int64 edgeInt64 = edge.ToInt64();

         IntPtr ptr = (IntPtr)(edgeInt64 & -4); //the last 2 bits are set to zero
         MCvQuadEdge2D qe = (MCvQuadEdge2D)Marshal.PtrToStructure(ptr, typeof(MCvQuadEdge2D));
         IntPtr pointPtr = qe.pt[edgeInt64 & 3]; //use only the last 2 bit as index
         if (pointPtr == IntPtr.Zero)
         {
            MCvSubdiv2DPoint pt = new MCvSubdiv2DPoint();
            pt.flags = -1;
            return pt; // return an invalid point
         }
         else
         {
            return (MCvSubdiv2DPoint)Marshal.PtrToStructure(pointPtr, typeof(MCvSubdiv2DPoint));
         }
      }

      /// <summary>
      /// similar to cvSubdiv2DEdgeDst
      /// </summary>
      /// <returns></returns>
      public MCvSubdiv2DPoint cvSubdiv2DEdgeDst()
      {
         Int64 edgeInt64 = edge.ToInt64();

         IntPtr ptr = (IntPtr)(edgeInt64 & -4);
         MCvQuadEdge2D qe = (MCvQuadEdge2D)Marshal.PtrToStructure(ptr, typeof(MCvQuadEdge2D));
         IntPtr pointPtr = qe.pt[(edgeInt64 + 2) & 3];
         if (pointPtr == IntPtr.Zero)
         {
            MCvSubdiv2DPoint pt = new MCvSubdiv2DPoint();
            pt.flags = -1;
            return pt; // return an invalid point
         }
         else
         {
            return (MCvSubdiv2DPoint)Marshal.PtrToStructure(pointPtr, typeof(MCvSubdiv2DPoint));
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
         Int64 edgeInt64 = edge.ToInt64();
         return new MCvSubdiv2DEdge(new IntPtr((edgeInt64 & -4) | ((edgeInt64 + rotate) & 3)));
      }

      /// <summary>
      /// Similar to cvSubdiv2DGetEdge
      /// </summary>
      /// <param name="type">The next edge type</param>
      /// <returns>The next edge</returns>
      public MCvSubdiv2DEdge cvSubdiv2DGetEdge(CvEnum.CV_NEXT_EDGE_TYPE type)
      {
         Int64 edgeInt64 = edge.ToInt64();
         IntPtr ptr = (IntPtr)(edgeInt64 & -4);
         MCvQuadEdge2D qe = (MCvQuadEdge2D)Marshal.PtrToStructure(ptr, typeof(MCvQuadEdge2D));
         Int64 edgePtr = qe.next[(edgeInt64 + (int)type) & 3].edge.ToInt64();
         edgePtr = (edgePtr & -4) + ((edgePtr + ((int)type >> 4)) & 3);

         return new MCvSubdiv2DEdge( new IntPtr(edgePtr));
      }
   }
}
