//----------------------------------------------------------------------------
//  Copyright (C) 2004-2021 by EMGU Corporation. All rights reserved.       
//----------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;
using Emgu.CV;
using Emgu.CV.Structure;
using Emgu.Util;

namespace Emgu.CV.VideoStab
{
    /// <summary>
    /// Use the Capture class as a FrameSource
    /// </summary>
    public class CaptureFrameSource : FrameSource
    {
        /// <summary>
        /// Create a Capture frame source
        /// </summary>
        /// <param name="capture">The capture object that will be converted to a FrameSource</param>
        public CaptureFrameSource(VideoCapture capture)
        {
            _ptr = VideoStabInvoke.cveVideostabCaptureFrameSourceCreate(capture, ref FrameSourcePtr);
            CaptureSource = capture.CaptureSource;
        }

        /// <summary>
        /// Release the unmanaged memory associated with this CaptureFrameSource
        /// </summary>
        protected override void DisposeObject()
        {
            if (_ptr != IntPtr.Zero)
            {
                VideoStabInvoke.cveVideostabCaptureFrameSourceRelease(ref _ptr);
            }

            FrameSourcePtr = IntPtr.Zero;
            base.DisposeObject();
        }
    }
}
