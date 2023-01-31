//----------------------------------------------------------------------------
//  Copyright (C) 2004-2023 by EMGU Corporation. All rights reserved.       
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
    /// Planar board with grid arrangement of markers More common type of board. All markers are placed in the same plane in a grid arrangement.
    /// </summary>
    public class GridBoard : SharedPtrObject, IBoard
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
        /// <param name="firstMarker">	id of first marker in dictionary to use on board.</param>
        public GridBoard(
            int markersX, 
            int markersY, 
            float markerLength, 
            float markerSeparation,
            Dictionary dictionary, 
            IInputArray ids = null)
        {
            using (InputArray iaIds = (ids == null) ? InputArray.GetEmpty() : ids.GetInputArray())
            {
                _ptr = ArucoInvoke.cveArucoGridBoardCreate(
                    markersX, markersY, 
                    markerLength, markerSeparation,
                    dictionary, iaIds, 
                    ref _boardPtr, ref _sharedPtr);
            }
        }

        /// <summary>
        /// Draw a GridBoard.
        /// </summary>
        /// <param name="outSize">size of the output image in pixels.</param>
        /// <param name="img">output image with the board. The size of this image will be outSize and the board will be on the center, keeping the board proportions.</param>
        /// <param name="marginSize">minimum margins (in pixels) of the board in the output image</param>
        /// <param name="borderBits">width of the marker borders.</param>
        public void Draw(Size outSize, IOutputArray img, int marginSize = 0, int borderBits = 1)
        {
            using (OutputArray oaImg = img.GetOutputArray())
                ArucoInvoke.cveArucoGridBoardDraw(_ptr, ref outSize, oaImg, marginSize, borderBits);
        }

        /// <summary>
        /// Release the unmanaged resource associated with this GridBoard
        /// </summary>
        protected override void DisposeObject()
        {
            if (_sharedPtr != IntPtr.Zero)
            {
                ArucoInvoke.cveArucoGridBoardRelease(ref _sharedPtr);
                _ptr = IntPtr.Zero;
            }

            _boardPtr = IntPtr.Zero;
        }

        /// <summary>
        /// Pointer to native IBoard
        /// </summary>
        public IntPtr BoardPtr { get { return _boardPtr; } }
    }

    public static partial class ArucoInvoke
    {
        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern IntPtr cveArucoGridBoardCreate(
           int markersX, int markersY, float markerLength, float markerSeparation,
           IntPtr dictionary, IntPtr ids, ref IntPtr boardPtr, ref IntPtr sharedPtr);

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern void cveArucoGridBoardRelease(ref IntPtr sharedPtr);

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern void cveArucoGridBoardDraw(IntPtr gridBoard, ref Size outSize, IntPtr img, int marginSize, int borderBits);

    }
}