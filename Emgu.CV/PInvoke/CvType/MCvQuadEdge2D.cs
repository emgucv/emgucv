//----------------------------------------------------------------------------
//  Copyright (C) 2004-2012 by EMGU. All rights reserved.       
//----------------------------------------------------------------------------

using System;
using System.Runtime.InteropServices;
using System.Diagnostics;

namespace Emgu.CV.Structure
{
   /// <summary>
   /// Quad-edge of planar subdivision
   /// </summary>
   [StructLayout(LayoutKind.Sequential)]
   public struct MCvQuadEdge2D
   {
      /// <summary>
      /// flags
      /// </summary>
      public int flags;

      /// <summary>
      /// Pointers to struct CvSubdiv2DPoint
      /// </summary>
      [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
      public IntPtr[] pt;

      /// <summary>
      /// Quad-edges, for each of the edges, lower 2 bits is index (0..3) and upper bits are quad-edge pointer
      /// </summary>
      [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
      public MCvSubdiv2DEdge[] next;

      /// <summary>
      /// Get the ith subdivision point
      /// </summary>
      /// <param name="index">the index to the point, &gt;=0 and &lt;4 </param>
      /// <returns>the ith CvSubdiv2DPoint</returns>
      public MCvSubdiv2DPoint GetCvSubdiv2DPoint(int index)
      {
         Debug.Assert(0 <= index && index < 4, "index must be >= 0 and < 4");
         if (pt[index] == IntPtr.Zero) return new MCvSubdiv2DPoint();
         return (MCvSubdiv2DPoint)Marshal.PtrToStructure(pt[index], typeof(MCvSubdiv2DPoint));
      }
   }
}
