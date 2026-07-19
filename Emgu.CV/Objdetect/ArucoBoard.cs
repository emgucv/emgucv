//----------------------------------------------------------------------------
//  Copyright (C) 2004-2026 by EMGU Corporation. All rights reserved.
//----------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using Emgu.CV.Structure;
using Emgu.CV.Util;
using Emgu.Util;
using System.Diagnostics;
using System.Drawing;

namespace Emgu.CV.Aruco
{
    /// <summary>
    /// Board of ArUco markers with a custom layout. Any set of markers can be placed at arbitrary
    /// 3D positions by providing the marker corner coordinates in the board reference system.
    /// </summary>
    public class ArucoBoard : SharedPtrObject, IBoard
    {
        /// <summary>
        /// Create a board of ArUco markers with a custom layout.
        /// </summary>
        /// <param name="objPoints">Array of object points of all the marker corners in the board. Each marker include its 4 corners in this order: top left, top right, bottom right, bottom left.</param>
        /// <param name="dictionary">The dictionary of markers employed for this board.</param>
        /// <param name="ids">Vector of the identifiers of the markers in the board.</param>
        public ArucoBoard(
            IInputArrayOfArrays objPoints,
            Dictionary dictionary,
            IInputArray ids)
        {
            using (InputArray iaObjPoints = objPoints.GetInputArray())
            using (InputArray iaIds = ids.GetInputArray())
            {
                _ptr = ArucoInvoke.cveArucoBoardCreate(iaObjPoints, dictionary, iaIds, ref _sharedPtr);
            }
        }

        /// <summary>
        /// Release the unmanaged resource associated with this ArucoBoard
        /// </summary>
        protected override void DisposeObject()
        {
            if (_sharedPtr != IntPtr.Zero)
            {
                ArucoInvoke.cveArucoBoardRelease(ref _sharedPtr);
            }
            _ptr = IntPtr.Zero;
        }

        /// <summary>
        /// Pointer to native IBoard
        /// </summary>
        public IntPtr BoardPtr { get { return _ptr; } }
    }

    public static partial class ArucoInvoke
    {
        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern IntPtr cveArucoBoardCreate(
           IntPtr objPoints, IntPtr dictionary, IntPtr ids, ref IntPtr sharedPtr);

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern void cveArucoBoardRelease(ref IntPtr sharedPtr);
    }
}
