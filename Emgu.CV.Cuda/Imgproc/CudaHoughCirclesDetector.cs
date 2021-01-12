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
    /// Base class for circles detector algorithm.
    /// </summary>
    public class CudaHoughCirclesDetector : SharedPtrObject
    {
        /// <summary>
        /// Create hough circles detector
        /// </summary>
        /// <param name="dp">Inverse ratio of the accumulator resolution to the image resolution. For example, if dp=1 , the accumulator has the same resolution as the input image. If dp=2 , the accumulator has half as big width and height.</param>
        /// <param name="minDist">Minimum distance between the centers of the detected circles. If the parameter is too small, multiple neighbor circles may be falsely detected in addition to a true one. If it is too large, some circles may be missed.</param>
        /// <param name="cannyThreshold">The higher threshold of the two passed to Canny edge detector (the lower one is twice smaller).</param>
        /// <param name="votesThreshold">The accumulator threshold for the circle centers at the detection stage. The smaller it is, the more false circles may be detected.</param>
        /// <param name="minRadius">Minimum circle radius.</param>
        /// <param name="maxRadius">Maximum circle radius.</param>
        /// <param name="maxCircles">Maximum number of output circles.</param>
        public CudaHoughCirclesDetector(float dp, float minDist, int cannyThreshold, int votesThreshold, int minRadius, int maxRadius, int maxCircles = 4096)
        {
            _ptr = CudaInvoke.cudaHoughCirclesDetectorCreate(dp, minDist, cannyThreshold, votesThreshold, minRadius, maxRadius, maxCircles, ref _sharedPtr);
        }

        /// <summary>
        /// Finds circles in a grayscale image using the Hough transform.
        /// </summary>
        /// <param name="image">8-bit, single-channel grayscale input image.</param>
        /// <param name="circles">Output vector of found circles. Each vector is encoded as a 3-element floating-point vector.</param>
        /// <param name="stream">Use a Stream to call the function asynchronously (non-blocking) or null to call the function synchronously (blocking).</param>
        public void Detect(IInputArray image, IOutputArray circles, Stream stream = null)
        {
            using (InputArray iaImage = image.GetInputArray())
            using (OutputArray oaCircles = circles.GetOutputArray())
                CudaInvoke.cudaHoughCirclesDetectorDetect(_ptr, iaImage, oaCircles, stream);
        }

        /// <summary>
        /// Finds circles in a grayscale image using the Hough transform.
        /// </summary>
        /// <param name="image">8-bit, single-channel grayscale input image.</param>
        /// <returns>Circles detected</returns>
        public CircleF[] Detect(IInputArray image)
        {
            using (GpuMat circlesGpu = new GpuMat())
            using (Mat circlesMat = new Mat())
            {
                Detect(image, circlesGpu);
                circlesGpu.Download(circlesMat);
                CircleF[] circles = new CircleF[circlesMat.Cols];
                GCHandle circlesHandle = GCHandle.Alloc(circles, GCHandleType.Pinned);
                Emgu.CV.Util.CvToolbox.Memcpy(circlesHandle.AddrOfPinnedObject(), circlesMat.DataPointer, Toolbox.SizeOf<CircleF>() * circles.Length);
                circlesHandle.Free();
                return circles;
            }
        }

        /// <summary>
        /// Release the unmanaged memory associated with this circle detector.
        /// </summary>
        protected override void DisposeObject()
        {
            if (_sharedPtr != IntPtr.Zero)
            {
                CudaInvoke.cudaHoughCirclesDetectorRelease(ref _sharedPtr);
                _ptr = IntPtr.Zero;
            }
        }
    }

    public static partial class CudaInvoke
    {
        [DllImport(CvInvoke.ExternCudaLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern IntPtr cudaHoughCirclesDetectorCreate(float dp, float minDist, int cannyThreshold, int votesThreshold, int minRadius, int maxRadius, int maxCircles, ref IntPtr sharedPtr);

        [DllImport(CvInvoke.ExternCudaLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern void cudaHoughCirclesDetectorDetect(IntPtr detector, IntPtr src, IntPtr circles, IntPtr stream);

        [DllImport(CvInvoke.ExternCudaLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern void cudaHoughCirclesDetectorRelease(ref IntPtr detector);
    }
}
