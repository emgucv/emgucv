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
    public class CharucoParameters : UnmanagedObject
    {        
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