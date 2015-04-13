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
   /// Base class for lines detector algorithm.
   /// </summary>
   public class CudaHoughLinesDetector : UnmanagedObject
   {
      /// <summary>
      /// Create a hough lines detector
      /// </summary>
      /// <param name="rho">Distance resolution of the accumulator in pixels.</param>
      /// <param name="theta">Angle resolution of the accumulator in radians.</param>
      /// <param name="threshold">Accumulator threshold parameter. Only those lines are returned that get enough votes (&gt; threshold).</param>
      /// <param name="doSort">Performs lines sort by votes.</param>
      /// <param name="maxLines">Maximum number of output lines.</param>
      public CudaHoughLinesDetector(float rho, float theta, int threshold, bool doSort = false, int maxLines = 4096)
      {
         _ptr = CudaInvoke.cudaHoughLinesDetectorCreate(rho, theta, threshold, doSort, maxLines);
      }

      /// <summary>
      /// Finds line segments in a binary image using the probabilistic Hough transform.
      /// </summary>
      /// <param name="image">8-bit, single-channel binary source image</param>
      /// <param name="lines">Output vector of lines.Output vector of lines. Each line is represented by a two-element vector. 
      /// The first element is the distance from the coordinate origin (top-left corner of the image). 
      /// The second element is the line rotation angle in radians.</param>
      public void Detect(IInputArray image, IOutputArray lines, Stream stream = null)
      {
         using (InputArray iaImage = image.GetInputArray())
         using (OutputArray oaLines = lines.GetOutputArray())
            CudaInvoke.cudaHoughLinesDetectorDetect(_ptr, iaImage, oaLines, stream);
      }

      /// <summary>
      /// Release the unmanaged memory associated to this line detector.
      /// </summary>
      protected override void DisposeObject()
      {
         CudaInvoke.cudaHoughLinesDetectorRelease(ref _ptr);
      }
   }

   public static partial class CudaInvoke
   {
      [DllImport(CvInvoke.ExternCudaLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      internal static extern IntPtr cudaHoughLinesDetectorCreate(
         float rho, float theta, int threshold, 
         [MarshalAs(CvInvoke.BoolMarshalType)]
         bool doSort, 
         int maxLines);

      [DllImport(CvInvoke.ExternCudaLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      internal static extern void cudaHoughLinesDetectorDetect(IntPtr detector, IntPtr src, IntPtr lines, IntPtr stream);

      [DllImport(CvInvoke.ExternCudaLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      internal static extern void cudaHoughLinesDetectorRelease(ref IntPtr detector);
   }
}
