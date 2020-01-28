//----------------------------------------------------------------------------
//  Copyright (C) 2004-2020 by EMGU Corporation. All rights reserved.       
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

namespace Emgu.CV.Tracking
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
            _ptr = ContribInvoke.cveTrackerMOSSECreate(ref _trackerPtr, ref _sharedPtr);
        }

        /// <summary>
        /// Release the unmanaged resources associated with this tracker
        /// </summary>
        protected override void DisposeObject()
        {
            if (IntPtr.Zero != _ptr)
                ContribInvoke.cveTrackerMOSSERelease(ref _ptr, ref _sharedPtr);
            base.DisposeObject();
        }
    }
}
