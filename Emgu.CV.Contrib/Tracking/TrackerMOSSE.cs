//----------------------------------------------------------------------------
//  Copyright (C) 2004-2021 by EMGU Corporation. All rights reserved.       
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

namespace Emgu.CV.Legacy
{
    /// <summary>
    /// MOSSE Visual Object Tracking using Adaptive Correlation Filters
    /// </summary>
    /// <remarks>note, that this tracker works with grayscale images, if passed bgr ones, they will get converted internally.</remarks>
    public class TrackerMOSSE : Tracker
    {
        private IntPtr _sharedPtr;

        /// <summary>
        /// Create a MOSSE tracker
        /// </summary>
        public TrackerMOSSE()
        {
            _ptr = TrackingInvoke.cveTrackerMOSSECreate(ref _trackerPtr, ref _sharedPtr);
        }

        /// <summary>
        /// Release the unmanaged resources associated with this tracker
        /// </summary>
        protected override void DisposeObject()
        {
            if (IntPtr.Zero != _ptr)
                TrackingInvoke.cveTrackerMOSSERelease(ref _ptr, ref _sharedPtr);
            base.DisposeObject();
        }
    }
}

namespace Emgu.CV
{
    public static partial class TrackingInvoke
    {

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern IntPtr cveTrackerMOSSECreate(ref IntPtr tracker, ref IntPtr sharedPtr);
        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern void cveTrackerMOSSERelease(ref IntPtr tracker, ref IntPtr sharedPTr);

    }
}