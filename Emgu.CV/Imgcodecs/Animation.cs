//----------------------------------------------------------------------------
//  Copyright (C) 2004-2025 by EMGU Corporation. All rights reserved.       
//----------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Runtime.InteropServices;
using Emgu.CV.Structure;
using Emgu.CV.Util;
using Emgu.Util;

namespace Emgu.CV
{
    /// <summary>
    /// The `Animation` struct is designed to store and manage data for animated sequences such as those from animated formats (e.g., GIF, AVIF, APNG, WebP).
    /// It provides support for looping, background color settings, frame timing, and frame storage.
    /// </summary>
    public class Animation : UnmanagedObject
    {
        /// <summary>
        /// Constructs an Animation object with optional loop count and background color.
        /// </summary>
        /// <param name="loopCount">
        /// An integer representing the number of times the animation should loop:
        /// `0` (default) indicates infinite looping, meaning the animation will replay continuously;
        /// Positive values denote finite repeat counts, allowing the animation to play a limited number of times;
        /// If a negative value or a value beyond the maximum of `0xffff` (65535) is provided, it is reset to `0`
        /// (infinite looping) to maintain valid bounds.
        /// </param>
        /// <param name="bgColor">
        /// A `Scalar` object representing the background color in BGRA format:
        /// Defaults to `Scalar()`, indicating an empty color(usually transparent if supported).
        /// This background color provides a solid fill behind frames that have transparency, ensuring a consistent display appearance.
        /// </param>
        public Animation(int loopCount, MCvScalar bgColor)
		{
			_ptr = CvInvoke.cveAnimationCreate(loopCount, ref bgColor);
		}

        /// <summary>
        /// Duration for each frame in milliseconds.
        /// </summary>
        public VectorOfInt Durations
        {
            get
            {
                return new VectorOfInt(CvInvoke.cveAnimationGetDurations(_ptr), false);
            }
        }

        /// <summary>
        /// Vector of frames, where each Mat represents a single frame.
        /// </summary>
        public VectorOfMat Frames
        {
            get
            {
                return new VectorOfMat(CvInvoke.cveAnimationGetFrames(_ptr), false);
            }
        }

        /// <summary>
        /// Release unmanaged resources
        /// </summary>
        protected override void DisposeObject()
        {
            if (IntPtr.Zero != _ptr)
                CvInvoke.cveAnimationRelease(ref _ptr);
        }
    }

    public static partial class CvInvoke
    {
        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern IntPtr cveAnimationCreate(int loopCount, ref MCvScalar bgColor);

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern void cveAnimationRelease(ref IntPtr animation);

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern IntPtr cveAnimationGetDurations(IntPtr animation);

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern IntPtr cveAnimationGetFrames(IntPtr animation);

    }
}
