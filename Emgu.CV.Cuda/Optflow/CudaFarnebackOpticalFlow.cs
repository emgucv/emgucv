//----------------------------------------------------------------------------
//  Copyright (C) 2004-2015 by EMGU Corporation. All rights reserved.       
//----------------------------------------------------------------------------

﻿using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Emgu.CV.Structure;
using Emgu.Util;

namespace Emgu.CV.Cuda
{
   /// <summary>
   /// Farneback optical flow
   /// </summary>
   public class CudaFarnebackOpticalFlow : UnmanagedObject
   {
      /// <summary>
      /// 
      /// </summary>
      /// <param name="numLevels"></param>
      /// <param name="pyrScale"></param>
      /// <param name="fastPyramids"></param>
      /// <param name="winSize"></param>
      /// <param name="numIters"></param>
      /// <param name="polyN"></param>
      /// <param name="polySigma"></param>
      /// <param name="flags"></param>
      public CudaFarnebackOpticalFlow(
         int numLevels = 5,
         double pyrScale = 0.5,
         bool fastPyramids = false,
         int winSize = 13,
         int numIters = 10,
         int polyN = 5,
         double polySigma = 1.1,
         int flags = 0)
      {
         _ptr = CudaInvoke.cudaFarnebackOpticalFlowCreate(numLevels, pyrScale, fastPyramids, winSize, numIters, polyN, polySigma, flags);
      }

      /// <summary>
      /// Compute the optical flow.
      /// </summary>
      /// <param name="frame0">Source frame</param>
      /// <param name="frame1">Frame to track (with the same size as <paramref name="frame0"/>)</param>
      /// <param name="u">Flow horizontal component (along x axis)</param>
      /// <param name="v">Flow vertical component (along y axis)</param>
      /// <param name="stream">Use a Stream to call the function asynchronously (non-blocking) or null to call the function synchronously (blocking).</param>
      public void Compute(GpuMat frame0, GpuMat frame1, GpuMat u, GpuMat v, Stream stream = null)
      {
         CudaInvoke.cudaFarnebackOpticalFlowCompute(_ptr, frame0, frame1, u, v, stream);
      }

      /// <summary>
      /// Release all the unmanaged memory associated with this optical flow solver.
      /// </summary>
      protected override void DisposeObject()
      {
         CudaInvoke.cudaFarnebackOpticalFlowRelease(ref _ptr);
      }
   }

   public static partial class CudaInvoke
   {
      [DllImport(CvInvoke.ExternCudaLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      internal extern static IntPtr cudaFarnebackOpticalFlowCreate(
         int numLevels,
         double pyrScale,
         [MarshalAs(CvInvoke.BoolMarshalType)]
         bool fastPyramids,
         int winSize,
         int numIters,
         int polyN,
         double polySigma,
         int flags);

      [DllImport(CvInvoke.ExternCudaLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      internal extern static void cudaFarnebackOpticalFlowRelease(ref IntPtr flow);

      [DllImport(CvInvoke.ExternCudaLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      internal extern static void cudaFarnebackOpticalFlowCompute(IntPtr flow, IntPtr frame0, IntPtr frame1, IntPtr u, IntPtr v, IntPtr stream);
   }
}
