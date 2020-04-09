//----------------------------------------------------------------------------
//  Copyright (C) 2004-2020 by EMGU Corporation. All rights reserved.       
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
    /// A FrameSource that can be used by the Video Stabilizer
    /// </summary>
    public abstract class FrameSource : UnmanagedObject
    {
        private VideoCapture.CaptureModuleType _captureSource;

        /// <summary>
        /// Get or Set the capture type
        /// </summary>
        public VideoCapture.CaptureModuleType CaptureSource
        {
            get
            {
                return _captureSource;
            }
            set
            {
                _captureSource = value;
            }
        }

        /// <summary>
        /// The unmanaged pointer the frameSource
        /// </summary>
        public IntPtr FrameSourcePtr;

        /// <summary>
        /// Retrieve the next frame from the FrameSource
        /// </summary>
        /// <returns>The next frame. If no more frames, null will be returned.</returns>
        public Mat NextFrame()
        {
            Mat frame = new Mat();
            if (VideoStabInvoke.cveVideostabFrameSourceGetNextFrame(FrameSourcePtr, frame))
            {
                return frame;
            }
            else
            {
                frame.Dispose();
                return null;
            }
        }

        /// <summary>
        /// Retrieve the next frame from the FrameSource
        /// </summary>
        /// <param name="frame">The next frame</param>
        /// <returns>True if there are more frames</returns>
        public bool NextFrame(Mat frame)
        {
            return VideoStabInvoke.cveVideostabFrameSourceGetNextFrame(FrameSourcePtr, frame);
        }

        /// <summary>
        /// Release the unmanaged memory associated with this FrameSource
        /// </summary>
        protected override void DisposeObject()
        {
            FrameSourcePtr = IntPtr.Zero;
        }
    }
}