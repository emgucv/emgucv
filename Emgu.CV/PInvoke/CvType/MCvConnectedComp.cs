//----------------------------------------------------------------------------
//  Copyright (C) 2004-2013 by EMGU. All rights reserved.       
//----------------------------------------------------------------------------

using System;
using System.Runtime.InteropServices;

namespace Emgu.CV.Structure
{
   /// <summary>
   /// Managed structure equivalent to CvConnectedComp
   /// </summary>
   [StructLayout(LayoutKind.Sequential)]
   public struct MCvConnectedComp
   {
      /// <summary>
      /// area of the segmented component
      /// </summary>
      public double Area;

      /// <summary>
      /// scalar value
      /// </summary>
      public MCvScalar Value;

      /// <summary>
      /// ROI of the segmented component
      /// </summary>
      public System.Drawing.Rectangle Rect;

      /// <summary>
      /// Pointer to the CvSeq
      /// </summary>
      public IntPtr Contour;
   }
}
