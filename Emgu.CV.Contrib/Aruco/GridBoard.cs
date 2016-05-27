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
using System.Drawing;

namespace Emgu.CV.Aruco
{
   /// <summary>
   /// Board of markers
   /// </summary>
   public interface IBoard
   {
      /// <summary>
      /// Pointer to native IBoard
      /// </summary>
      IntPtr BoardPtr { get; }
   }

   /// <summary>
   /// Planar board with grid arrangement of markers More common type of board. All markers are placed in the same plane in a grid arrangment.
   /// </summary>
   public class GridBoard : UnmanagedObject, IBoard
   {
      private IntPtr _boardPtr;

      /// <summary>
      /// Create a GridBoard object.
      /// </summary>
      /// <param name="markersX">number of markers in X direction</param>
      /// <param name="markersY">number of markers in Y direction</param>
      /// <param name="markerLength">marker side length (normally in meters)</param>
      /// <param name="markerSeparation">separation between two markers (same unit than markerLenght)</param>
      /// <param name="dictionary">dictionary of markers indicating the type of markers. The first markersX*markersY markers in the dictionary are used.</param>
      public GridBoard(int markersX, int markersY, float markerLength, float markerSeparation,
         Dictionary dictionary)
      {
         _ptr = ArucoInvoke.cveArucoGridBoardCreate(markersX, markersY, markerLength, markerSeparation, dictionary, ref _boardPtr);
      }

      /// <summary>
      /// Draw a GridBoard.
      /// </summary>
      /// <param name="outSize">size of the output image in pixels.</param>
      /// <param name="img">output image with the board. The size of this image will be outSize and the board will be on the center, keeping the board proportions.</param>
      /// <param name="margindSize">minimum margins (in pixels) of the board in the output image</param>
      /// <param name="borderBits">width of the marker borders.</param>
      public void Draw(Size outSize, IOutputArray img, int margindSize = 0, int borderBits = 1)
      {
         using (OutputArray oaImg = img.GetOutputArray())
            ArucoInvoke.cveArucoGridBoardDraw(_ptr, ref outSize, oaImg, margindSize, borderBits);
      }

      /// <summary>
      /// Release the unmanage resource associated with this GridBoard
      /// </summary>
      protected override void DisposeObject()
      {
         if (_ptr != IntPtr.Zero)
            ArucoInvoke.cveArucoGridBoardRelease(ref _ptr);

         _boardPtr = IntPtr.Zero;
      }

      /// <summary>
      /// Pointer to native IBoard
      /// </summary>
      public IntPtr BoardPtr { get { return _boardPtr;} }
   }

   
   public static partial class ArucoInvoke
   {
      [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      internal static extern IntPtr cveArucoGridBoardCreate(
         int markersX, int markersY, float markerLength, float markerSeparation,
         IntPtr dictionary, ref IntPtr boardPtr);

      [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      internal static extern void cveArucoGridBoardRelease(ref IntPtr gridBoard);

      [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      internal static extern void cveArucoGridBoardDraw(IntPtr gridBoard, ref Size outSize, IntPtr img, int marginSize, int borderBits);
   }
}