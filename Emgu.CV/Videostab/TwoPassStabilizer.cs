//----------------------------------------------------------------------------
//  Copyright (C) 2004-2020 by EMGU Corporation. All rights reserved.       
//----------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Emgu.CV;
using Emgu.CV.Structure;
using Emgu.Util;

namespace Emgu.CV.VideoStab
{
    /// <summary>
    /// A two pass video stabilizer
    /// </summary>
    public class TwoPassStabilizer : FrameSource
    {
        private IntPtr _stabilizerBase;

        private FrameSource _baseFrameSource;

        /// <summary>
        /// Create a two pass video stabilizer.
        /// </summary>
        /// <param name="baseFrameSource">The capture object to be stabilized. Should not be a camera stream.</param>
        public TwoPassStabilizer(FrameSource baseFrameSource)
        {
            if (baseFrameSource.CaptureSource == VideoCapture.CaptureModuleType.Camera)
            {
                throw new ArgumentException("Two pass stabilizer cannot process camera stream");
            }

            _baseFrameSource = baseFrameSource;

            _ptr = VideoStabInvoke.cveTwoPassStabilizerCreate(_baseFrameSource, ref _stabilizerBase, ref FrameSourcePtr);
        }

        /// <summary>
        /// Release the unmanaged memory
        /// </summary>
        protected override void DisposeObject()
        {
            if (_ptr != IntPtr.Zero)
            {
                VideoStabInvoke.cveTwoPassStabilizerRelease(ref _ptr);
            }

            _stabilizerBase = IntPtr.Zero;
            //_captureFrameSource.Dispose();
            base.Dispose();
        }
    }
}
