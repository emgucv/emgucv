//----------------------------------------------------------------------------
//  Copyright (C) 2004-2022 by EMGU Corporation. All rights reserved.       
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
    /// A ChArUco board is a planar board where the markers are placed
    /// inside the white squares of a chessboard.The benefits of ChArUco boards is that they provide
    /// both, ArUco markers versatility and chessboard corner precision, which is important for
    /// calibration and pose estimation.
    /// </summary>
    public class CharucoBoard : SharedPtrObject, IBoard
    {
        private IntPtr _boardPtr;

        /// <summary>
        /// ChArUco board
        /// </summary>
        /// <param name="squaresX">number of chessboard squares in X direction</param>
        /// <param name="squaresY">number of chessboard squares in Y direction</param>
        /// <param name="squareLength">chessboard square side length (normally in meters)</param>
        /// <param name="markerLength">marker side length (same unit than squareLength)</param>
        /// <param name="dictionary">dictionary of markers indicating the type of markers.</param>
        public CharucoBoard(
           int squaresX, int squaresY,
           float squareLength, float markerLength,
           Dictionary dictionary)
        {
            _ptr = ArucoInvoke.cveCharucoBoardCreate(squaresX, squaresY, squareLength, markerLength, dictionary, ref _boardPtr, ref _sharedPtr);
        }

        /// <summary>
        /// Draw a ChArUco board
        /// </summary>
        /// <param name="outSize">size of the output image in pixels.</param>
        /// <param name="img">output image with the board. The size of this image will be outSize and the board will be on the center, keeping the board proportions.</param>
        /// <param name="marginSize">minimum margins (in pixels) of the board in the output image</param>
        /// <param name="borderBits">width of the marker borders.</param>
        public void Draw(Size outSize, IOutputArray img, int marginSize = 0, int borderBits = 1)
        {
            using (OutputArray oaImg = img.GetOutputArray())
                ArucoInvoke.cveCharucoBoardDraw(_ptr, ref outSize, oaImg, marginSize, borderBits);
        }

        /// <summary>
        /// Release the unmanaged resource associated with this ChArUco board
        /// </summary>
        protected override void DisposeObject()
        {
            if (_sharedPtr != IntPtr.Zero)
            {
                ArucoInvoke.cveCharucoBoardRelease(ref _sharedPtr);
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
        internal static extern IntPtr cveCharucoBoardCreate(
           int squaresX, int squaresY, float squareLength, float markerLength,
           IntPtr dictionary, ref IntPtr boardPtr, ref IntPtr sharedPtr);

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern void cveCharucoBoardDraw(IntPtr charucoBoard, ref Size outSize, IntPtr img, int marginSize, int borderBits);

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern void cveCharucoBoardRelease(ref IntPtr sharedPtr);
    }
}