//----------------------------------------------------------------------------
//  Copyright (C) 2004-2012 by EMGU. All rights reserved.       
//----------------------------------------------------------------------------

using System;
using System.Runtime.InteropServices;
using System.Drawing;

namespace Emgu.CV.Structure
{
   /// <summary>
   /// Managed structure equivalent to CvConvexityDefect
   /// </summary>
   [StructLayout(LayoutKind.Sequential)]
   public struct MCvConvexityDefect
   {
      /// <summary>
      /// Pointer to the point of the contour where the defect begins
      /// </summary>
      public IntPtr StartPointPointer;

      /// <summary>
      /// Pointer to the point of the contour where the defect ends
      /// </summary>
      public IntPtr EndPointPointer;

      /// <summary>
      /// Pointer to the farthest point from the convex hull within the defect
      /// </summary>
      public IntPtr DepthPointPointer;

      /// <summary>
      /// Distance between the farthest point and the convex hull
      /// </summary>
      public float Depth; 

      /// <summary>
      /// Point of the contour where the defect begins
      /// </summary>
      public Point StartPoint
      {
         get
         {
            return (Point)Marshal.PtrToStructure(StartPointPointer, typeof(Point));
         }
      }

      /// <summary>
      /// Point of the contour where the defect ends
      /// </summary>
      public Point EndPoint
      {
         get
         {
            return (Point)Marshal.PtrToStructure(EndPointPointer, typeof(Point));
         }
      }

      /// <summary>
      /// The farthest from the convex hull point within the defect
      /// </summary>
      public Point DepthPoint
      {
         get
         {
            return (Point)Marshal.PtrToStructure(DepthPointPointer, typeof(Point));
         }
      }
   }
}
