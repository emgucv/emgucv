//----------------------------------------------------------------------------
//  Copyright (C) 2004-2015 by EMGU Corporation. All rights reserved.       
//----------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using Emgu.CV;
using Emgu.CV.Structure;
using Emgu.Util;

namespace Emgu.CV.Cuda
{
   /// <summary>
   /// Base class for circles detector algorithm.
   /// </summary>
   public class CudaHoughCirclesDetector : UnmanagedObject
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
         _ptr = CudaInvoke.cudaHoughCirclesDetectorCreate(dp, minDist, cannyThreshold, votesThreshold, minRadius, maxRadius, maxCircles);
      }

      /// <summary>
      /// Finds circles in a grayscale image using the Hough transform.
      /// </summary>
      /// <param name="image">8-bit, single-channel grayscale input image.</param>
      /// <param name="circles">Output vector of found circles. Each vector is encoded as a 3-element floating-point vector.</param>
      public void Detect(IInputArray image, IOutputArray circles, Stream stream = null)
      {
         using (InputArray iaImage = image.GetInputArray())
         using (OutputArray oaCircles = circles.GetOutputArray())
            CudaInvoke.cudaHoughCirclesDetectorDetect(_ptr, iaImage, oaCircles, stream);
      }

      /// <summary>
      /// Release the unmanaged memory associated with this circle detector.
      /// </summary>
      protected override void DisposeObject()
      {
         CudaInvoke.cudaHoughCirclesDetectorRelease(ref _ptr);
      }
   }

   public static partial class CudaInvoke
   {
      [DllImport(CvInvoke.ExternCudaLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      internal static extern IntPtr cudaHoughCirclesDetectorCreate(float dp, float minDist, int cannyThreshold, int votesThreshold, int minRadius, int maxRadius, int maxCircles);

      [DllImport(CvInvoke.ExternCudaLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      internal static extern void cudaHoughCirclesDetectorDetect(IntPtr detector, IntPtr src, IntPtr circles, IntPtr stream);

      [DllImport(CvInvoke.ExternCudaLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      internal static extern void cudaHoughCirclesDetectorRelease(ref IntPtr detector);
   }
}
