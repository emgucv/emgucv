//----------------------------------------------------------------------------
//  Copyright (C) 2004-2021 by EMGU Corporation. All rights reserved.       
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

namespace Emgu.CV.Legacy
{
    /// <summary>
    /// TLD is a novel tracking framework that explicitly decomposes the long-term tracking task into tracking, learning and detection.
    /// </summary>
    /// <remarks>The tracker follows the object from frame to frame. The detector localizes all appearances that have been observed so far and corrects the tracker if necessary. The learning estimates detector's errors and updates it to avoid these errors in the future.</remarks>
    public class TrackerTLD : Tracker
    {
        private IntPtr _sharedPtr;

        /// <summary>
        /// Creates a TLD tracker
        /// </summary>
        public TrackerTLD()
        {
            _ptr = TrackingInvoke.cveTrackerTLDCreate(ref _trackerPtr, ref _sharedPtr);
        }

        /// <summary>
        /// Release the unmanaged resources associated with this tracker
        /// </summary>
        protected override void DisposeObject()
        {
            if (IntPtr.Zero != _ptr)
                TrackingInvoke.cveTrackerTLDRelease(ref _ptr, ref _sharedPtr);
            base.DisposeObject();
        }
    }
}

namespace Emgu.CV
{
    public static partial class TrackingInvoke
    {

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern IntPtr cveTrackerTLDCreate(ref IntPtr tracker, ref IntPtr sharedPtr);
        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern void cveTrackerTLDRelease(ref IntPtr tracker, ref IntPtr sharedPtr);

    }
}
