using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace Emgu.CV.Structure
{
   /// <summary>
   /// Managed structure equivalent to CvConvexityDefect
   /// </summary>
   [StructLayout(LayoutKind.Sequential)]
   public struct MCvConvexityDefect
   {
      /// <summary>
      /// Point of the contour where the defect begins
      /// </summary>
      public IntPtr start; 
      /// <summary>
      /// Point of the contour where the defect ends
      /// </summary>
      public IntPtr end; 
      /// <summary>
      /// The farthest from the convex hull point within the defect
      /// </summary>
      public IntPtr depth_point;
      /// <summary>
      /// Distance between the farthest point and the convex hull
      /// </summary>
      public float depth; 
   }
}
