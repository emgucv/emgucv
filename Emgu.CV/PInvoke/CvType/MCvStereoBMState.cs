//----------------------------------------------------------------------------
//  Copyright (C) 2004-2015 by EMGU Corporation. All rights reserved.       
//----------------------------------------------------------------------------

using System;
using System.Runtime.InteropServices;
using System.Drawing;

namespace Emgu.CV.Structure
{
   /// <summary>
   /// Wrapped CvStereoBMState structure
   /// </summary>
   [StructLayout(LayoutKind.Sequential)]
   public struct MCvStereoBMState
   {
      #region pre filters (normalize input images):
      /// <summary>
      /// 0 for now
      /// </summary>
      public int preFilterType;
      /// <summary>
      /// ~5x5..21x21
      /// </summary>
      public int preFilterSize;
      /// <summary>
      /// up to ~31
      /// </summary>
      public int preFilterCap;
      #endregion

      #region correspondence using Sum of Absolute Difference (SAD)
      /// <summary>
      /// Could be 5x5..21x21. Correspondence using Sum of Absolute Difference (SAD):
      /// </summary>
      public int SADWindowSize;
      /// <summary>
      /// minimum disparity (=0)
      /// </summary>
      public int minDisparity;
      /// <summary>
      /// maximum disparity - minimum disparity
      /// </summary>
      public int numberOfDisparities;
      #endregion

      #region post filters (knock out bad matches)
      /// <summary>
      /// areas with no texture are ignored
      /// </summary>
      public int textureThreshold;

      /// <summary>
      /// Filter out pixels if there are other close matches
      /// </summary>
      public int uniquenessRatio;
      #endregion

      // with different disparity
      /// <summary>
      /// Disparity variation window (not used)
      /// </summary>
      public int speckleWindowSize;
      /// <summary>
      /// Acceptable range of variation in window (not used)
      /// </summary>
      public int speckleRange;

      /// <summary>
      /// If 1, the results may be more accurate at the expense of slower processing.
      /// </summary>
      public int trySmallerWindows;

      /// <summary>
      /// 
      /// </summary>
      public Rectangle roi1;
      /// <summary>
      /// 
      /// </summary>
      public Rectangle roi2;
      /// <summary>
      /// 
      /// </summary>
      public int disp12MaxDiff; 

      /// <summary>
      /// internal buffers, do not modify (!)
      /// </summary>
      public IntPtr preFilteredImg0;
      /// <summary>
      /// internal buffers, do not modify (!)
      /// </summary>
      public IntPtr preFilteredImg1;
      /// <summary>
      /// internal buffers, do not modify (!)
      /// </summary>
      public IntPtr slidingSumBuf;
      /// <summary>
      /// internal buffers, do not modify (!)
      /// </summary>
      public IntPtr cost;
      /// <summary>
      /// internal buffers, do not modify (!)
      /// </summary>
      public IntPtr disp;
   }
}
