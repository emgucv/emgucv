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
        /// <param name="markersX">Number of markers in X direction</param>
        /// <param name="markersY">Number of markers in Y direction</param>
        /// <param name="markerLength">Marker side length (normally in meters)</param>
        /// <param name="markerSeparation">Separation between two markers (same unit than markerLenght)</param>
        /// <param name="dictionary">Dictionary of markers indicating the type of markers. The first markersX*markersY markers in the dictionary are used.</param>
        /// <param name="ids">set of marker ids in dictionary to use on board.</param>
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
        /// Release the unmanaged resource associated with this GridBoard
        /// </summary>
        protected override void DisposeObject()
        {
            if (_sharedPtr != IntPtr.Zero)
            {
                ArucoInvoke.cveArucoGridBoardRelease(ref _sharedPtr);
            }
            _ptr = IntPtr.Zero;
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

    }
}