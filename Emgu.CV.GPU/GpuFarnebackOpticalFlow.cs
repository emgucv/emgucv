//----------------------------------------------------------------------------
//  Copyright (C) 2004-2013 by EMGU. All rights reserved.       
//----------------------------------------------------------------------------

﻿using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Emgu.CV.Structure;
using Emgu.Util;

namespace Emgu.CV.GPU
{
   /// <summary>
   /// Farneback optical flow
   /// </summary>
   public class GpuFarnebackOpticalFlow : UnmanagedObject
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
      public GpuFarnebackOpticalFlow(
         int numLevels,
         double pyrScale,
         bool fastPyramids,
         int winSize,
         int numIters,
         int polyN,
         double polySigma,
         int flags)
      {
         _ptr = GpuInvoke.gpuFarnebackOpticalFlowCreate(numLevels, pyrScale, fastPyramids, winSize, numIters, polyN, polySigma, flags);
      }

      /// <summary>
      /// Compute the optical flow.
      /// </summary>
      /// <param name="frame0">Source frame</param>
      /// <param name="frame1">Frame to track (with the same size as <paramref name="frame0"/>)</param>
      /// <param name="u">Flow horizontal component (along x axis)</param>
      /// <param name="v">Flow vertical component (along y axis)</param>
      /// <param name="stream">Use a Stream to call the function asynchronously (non-blocking) or null to call the function synchronously (blocking).</param>
      public void Compute(GpuImage<Gray, float> frame0, GpuImage<Gray, float> frame1, GpuImage<Gray, Byte> u, GpuImage<Gray, Byte> v, Stream stream)
      {
         GpuInvoke.gpuFarnebackOpticalFlowCompute(_ptr, frame0, frame1, u, v, stream);
      }

      /// <summary>
      /// Release all the unmanaged memory associated with this optical flow solver.
      /// </summary>
      protected override void DisposeObject()
      {
         GpuInvoke.gpuFarnebackOpticalFlowRelease(ref _ptr);
      }
   }

   public static partial class GpuInvoke
   {
      [DllImport(CvInvoke.EXTERN_GPU_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      internal extern static IntPtr gpuFarnebackOpticalFlowCreate(
         int numLevels,
         double pyrScale,
         [MarshalAs(CvInvoke.BoolMarshalType)]
         bool fastPyramids,
         int winSize,
         int numIters,
         int polyN,
         double polySigma,
         int flags);

      [DllImport(CvInvoke.EXTERN_GPU_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      internal extern static void gpuFarnebackOpticalFlowRelease(ref IntPtr flow);

      [DllImport(CvInvoke.EXTERN_GPU_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      internal extern static void gpuFarnebackOpticalFlowCompute(IntPtr flow, IntPtr frame0, IntPtr frame1, IntPtr u, IntPtr v, IntPtr stream);
   }
}
