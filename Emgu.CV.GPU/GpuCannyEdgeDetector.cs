//----------------------------------------------------------------------------
//  Copyright (C) 2004-2013 by EMGU. All rights reserved.       
//----------------------------------------------------------------------------

﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using Emgu.CV;
using Emgu.CV.Structure;
using Emgu.Util;

namespace Emgu.CV.GPU
{
   /// <summary>
   /// Cascade Classifier for object detection using GPU
   /// </summary>
   public class GpuCannyEdgeDetector : UnmanagedObject
   {

      /// <summary>
      /// Canny edge detector using GPU.
      /// </summary>
      /// <param name="lowThreshold">The first threshold, used for edge linking</param>
      /// <param name="highThreshold">The second threshold, used to find initial segments of strong edges</param>
      /// <param name="apertureSize">Aperture parameter for Sobel operator, use 3 for default</param>
      /// <param name="L2gradient">Use false for default</param>
      public GpuCannyEdgeDetector(double lowThreshold, double highThreshold, int apertureSize, bool L2gradient)
      {
         _ptr = GpuInvoke.gpuCreateCannyEdgeDetector(lowThreshold, highThreshold, apertureSize, L2gradient);
      }

      /// <summary>
      /// Finds the edges on the input <paramref name="src"/> and marks them in the output image edges using the Canny algorithm. 
      /// </summary>
      /// <param name="src">Input image</param>
      /// <param name="edges">Image to store the edges found by the function</param>
      public void Detect(IntPtr src, IntPtr edges)
      {
         GpuInvoke.gpuCannyEdgeDetectorDetect(_ptr, src, edges);
      }

      /// <summary>
      /// Release all the unmanaged memory associate with this Canny edge detector.
      /// </summary>
      protected override void DisposeObject()
      {
         if (_ptr != null)
            GpuInvoke.gpuCannyEdgeDetectorRelease(ref _ptr);
      }
   }

   public static partial class GpuInvoke
   {

      [DllImport(CvInvoke.EXTERN_GPU_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      internal static extern IntPtr gpuCreateCannyEdgeDetector(
         double lowThreshold, double highThreshold, int apertureSize,
         [MarshalAs(CvInvoke.BoolMarshalType)]
         bool L2gradient);

      [DllImport(CvInvoke.EXTERN_GPU_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      internal static extern void gpuCannyEdgeDetectorDetect(IntPtr detector, IntPtr src, IntPtr edges);

      [DllImport(CvInvoke.EXTERN_GPU_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      internal static extern void gpuCannyEdgeDetectorRelease(ref IntPtr detector);

   }
}
