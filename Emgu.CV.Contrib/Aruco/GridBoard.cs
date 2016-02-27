//----------------------------------------------------------------------------
//  Copyright (C) 2004-2016 by EMGU Corporation. All rights reserved.       
//----------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using Emgu.CV.Structure;
using Emgu.CV.Text;
using Emgu.CV.Util;
using Emgu.Util;
using System.Diagnostics;

namespace Emgu.CV.Aruco
{
   public class GridBoard : UnmanagedObject
   {
      public GridBoard(int markersX, int markersY, float markerLength, float markerSeparation,
         Dictionary dictionary)
      {
         _ptr = ArucoInvoke.cveArucoGridBoardCreate(markersX, markersY, markerLength, markerSeparation, dictionary);
      }

      protected override void DisposeObject()
      {
         if (_ptr != IntPtr.Zero)
            ArucoInvoke.cveArucoGridBoardRelease(ref _ptr);

      }
   }



   public static partial class ArucoInvoke
   {
      [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      internal static extern IntPtr cveArucoGridBoardCreate(
         int markersX, int markersY, float markerLength, float markerSeparation,
         IntPtr dictionary);

      [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      internal static extern void cveArucoGridBoardRelease(ref IntPtr gridBoard);
   }
}