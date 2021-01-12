//----------------------------------------------------------------------------
//  Copyright (C) 2004-2021 by EMGU Corporation. All rights reserved.       
//----------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using Emgu.CV;
using Emgu.CV.Structure;
using Emgu.CV.Util;
using Emgu.Util;

namespace Emgu.CV.Cuda
{
    /// <summary>
    /// Base class for line segments detector algorithm.
    /// </summary>
    public class CudaHoughSegmentDetector : SharedPtrObject
    {
        /// <summary>
        /// Create a hough segment detector
        /// </summary>
        /// <param name="rho">Distance resolution of the accumulator in pixels.</param>
        /// <param name="theta">Angle resolution of the accumulator in radians.</param>
        /// <param name="minLineLength"> Minimum line length. Line segments shorter than that are rejected.</param>
        /// <param name="maxLineGap">Maximum allowed gap between points on the same line to link them.</param>
        /// <param name="maxLines">Maximum number of output lines.</param>
        public CudaHoughSegmentDetector(float rho, float theta, int minLineLength, int maxLineGap, int maxLines = 4096)
        {
            _ptr = CudaInvoke.cudaHoughSegmentDetectorCreate(rho, theta, minLineLength, maxLineGap, maxLines, ref _sharedPtr);
        }

        /// <summary>
        /// Finds line segments in a binary image using the probabilistic Hough transform.
        /// </summary>
        /// <param name="image">8-bit, single-channel binary source image</param>
        /// <param name="lines">Output vector of lines. Each line is represented by a 4-element vector (x1, y1, x2, y2) , where (x1, y1) and (x2, y2) are the ending points of each detected line segment.</param>
        /// <param name="stream">Use a Stream to call the function asynchronously (non-blocking) or null to call the function synchronously (blocking).</param>
        public void Detect(IInputArray image, IOutputArray lines, Stream stream = null)
        {
            using (InputArray iaImage = image.GetInputArray())
            using (OutputArray oaLines = lines.GetOutputArray())
                CudaInvoke.cudaHoughSegmentDetectorDetect(_ptr, iaImage, oaLines, stream);
        }

        /// <summary>
        /// Release the unmanaged memory associated with this segment detector
        /// </summary>
        protected override void DisposeObject()
        {
            if (IntPtr.Zero != _sharedPtr)
            {
                CudaInvoke.cudaHoughSegmentDetectorRelease(ref _sharedPtr);
                _ptr = IntPtr.Zero;
            }
        }
    }

    public static partial class CudaInvoke
    {
        [DllImport(CvInvoke.ExternCudaLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern IntPtr cudaHoughSegmentDetectorCreate(float rho, float theta, int minLineLength, int maxLineGap, int maxLines, ref IntPtr sharedPtr);

        [DllImport(CvInvoke.ExternCudaLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern void cudaHoughSegmentDetectorDetect(IntPtr detector, IntPtr src, IntPtr lines, IntPtr stream);

        [DllImport(CvInvoke.ExternCudaLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern void cudaHoughSegmentDetectorRelease(ref IntPtr detector);
    }
}
