//----------------------------------------------------------------------------
//  Copyright (C) 2004-2025 by EMGU Corporation. All rights reserved.       
//----------------------------------------------------------------------------

using System;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using Emgu.CV;
using Emgu.CV.Structure;
using Emgu.CV.Util;
using Emgu.Util;
using System.Drawing;
using System.Runtime.InteropServices.ComTypes;

namespace Emgu.CV.Aruco
{
    /// <summary>
    /// Represents the parameters used for ChArUco marker detection.
    /// </summary>
    /// <remarks>
    /// This class provides configuration options for detecting ChArUco markers, 
    /// including the minimum number of markers required, whether to refine marker detection, 
    /// and whether to validate detected markers.
    /// </remarks>
    public class CharucoParameters : UnmanagedObject
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CharucoParameters"/> class with specified parameters.
        /// </summary>
        /// <param name="minMarkers">
        /// The minimum number of adjacent markers that must be detected to return a charuco corner. Default is 2.
        /// </param>
        /// <param name="tryRefineMarkers">
        /// A boolean value indicating whether to refine the detected markers for improved accuracy. Default is <c>false</c>.
        /// </param>
        /// <param name="checkMarkers">
        /// A boolean value indicating whether to validate the detected markers. Default is <c>true</c>.
        /// </param>
        public CharucoParameters(	
			int minMarkers = 2,
			bool tryRefineMarkers = false,
			bool checkMarkers = true)
        {
            _ptr = ObjdetectInvoke.cveCharucoParametersCreate(minMarkers, tryRefineMarkers, checkMarkers);
        }

    
        /// <summary>
        /// Release the unmanaged memory associated with this object
        /// </summary>
        protected override void DisposeObject()
        {
            if (_ptr != IntPtr.Zero)
            {
                ObjdetectInvoke.cveCharucoParametersRelease(ref _ptr);
            }
        }
    }


    /// <summary>
    /// This class contains functions to call into object detect module
    /// </summary>
    public static partial class ObjdetectInvoke
    {
        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern IntPtr cveCharucoParametersCreate(
			int minMarkers,
            [MarshalAs(CvInvoke.BoolMarshalType)]
			bool tryRefineMarkers,
            [MarshalAs(CvInvoke.BoolMarshalType)]
            bool checkMarkers);

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern void cveCharucoParametersRelease(ref IntPtr detector);
    }

}