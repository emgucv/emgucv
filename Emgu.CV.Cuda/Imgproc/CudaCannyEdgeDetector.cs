//----------------------------------------------------------------------------
//  Copyright (C) 2004-2015 by EMGU Corporation. All rights reserved.       
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

namespace Emgu.CV.Cuda
{
   /// <summary>
   /// Cascade Classifier for object detection using Cuda
   /// </summary>
   public class CudaCannyEdgeDetector : UnmanagedObject
   {

      /// <summary>
      /// Canny edge detector using Cuda.
      /// </summary>
      /// <param name="lowThreshold">The first threshold, used for edge linking</param>
      /// <param name="highThreshold">The second threshold, used to find initial segments of strong edges</param>
      /// <param name="apertureSize">Aperture parameter for Sobel operator, use 3 for default</param>
      /// <param name="L2gradient">Use false for default</param>
      public CudaCannyEdgeDetector(double lowThreshold, double highThreshold, int apertureSize = 3, bool L2gradient = false)
      {
         _ptr = CudaInvoke.cudaCreateCannyEdgeDetector(lowThreshold, highThreshold, apertureSize, L2gradient);
      }

      /// <summary>
      /// Finds the edges on the input <paramref name="src"/> and marks them in the output image edges using the Canny algorithm. 
      /// </summary>
      /// <param name="src">Input image</param>
      /// <param name="edges">Image to store the edges found by the function</param>
      public void Detect(IInputArray src, IOutputArray edges, Stream stream = null)
      {
         using (InputArray iaSrc = src.GetInputArray())
         using (OutputArray oaEdges = edges.GetOutputArray())
            CudaInvoke.cudaCannyEdgeDetectorDetect(_ptr, iaSrc, oaEdges, stream);
      }

      /// <summary>
      /// Release all the unmanaged memory associate with this Canny edge detector.
      /// </summary>
      protected override void DisposeObject()
      {
         if (_ptr != IntPtr.Zero)
            CudaInvoke.cudaCannyEdgeDetectorRelease(ref _ptr);
      }
   }

   public static partial class CudaInvoke
   {

      [DllImport(CvInvoke.ExternCudaLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      internal static extern IntPtr cudaCreateCannyEdgeDetector(
         double lowThreshold, double highThreshold, int apertureSize,
         [MarshalAs(CvInvoke.BoolMarshalType)]
         bool L2gradient);

      [DllImport(CvInvoke.ExternCudaLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      internal static extern void cudaCannyEdgeDetectorDetect(IntPtr detector, IntPtr src, IntPtr edges, IntPtr stream);

      [DllImport(CvInvoke.ExternCudaLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      internal static extern void cudaCannyEdgeDetectorRelease(ref IntPtr detector);

   }
}
