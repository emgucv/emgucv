//----------------------------------------------------------------------------
//  Copyright (C) 2004-2013 by EMGU. All rights reserved.       
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
         int numLevels,
         double pyrScale,
         bool fastPyramids,
         int winSize,
         int numIters,
         int polyN,
         double polySigma,
         int flags)
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
      public void Compute(CudaImage<Gray, float> frame0, CudaImage<Gray, float> frame1, CudaImage<Gray, Byte> u, CudaImage<Gray, Byte> v, Stream stream)
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
