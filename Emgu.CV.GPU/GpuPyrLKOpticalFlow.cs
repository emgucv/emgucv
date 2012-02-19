//----------------------------------------------------------------------------
//  Copyright (C) 2004-2012 by EMGU. All rights reserved.       
//----------------------------------------------------------------------------

﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Runtime.InteropServices;
using Emgu.CV.Structure;
using Emgu.Util;

namespace Emgu.CV.GPU
{
   /// <summary>
   /// PyrLK optical flow
   /// </summary>
   public class GpuPyrLKOpticalFlow : UnmanagedObject
   {
      /// <summary>
      /// Create the PyrLK optical flow solver
      /// </summary>
      /// <param name="winSize">Windows size. Use 21x21 for default</param>
      /// <param name="maxLevel">The maximum number of pyramid leveles. Use 3 for default</param>
      /// <param name="iters">The number of iterations. Use 30 for default.</param>
      /// <param name="derivLambda">Use 0.5 for default.</param>
      /// <param name="useInitialFlow">Weather or not use the initial flow in the input matrix. Use false for default.</param>
      /// <param name="minEigThreshold">Threshold for the minimum eigen values. Use 1e-4f for default.</param>
      public GpuPyrLKOpticalFlow(Size winSize, int maxLevel, int iters, double derivLambda, bool useInitialFlow, float minEigThreshold)
      {
         _ptr = GpuInvoke.gpuPryLKOpticalFlowCreate(winSize, maxLevel, iters, derivLambda, useInitialFlow, minEigThreshold);
      }

      /// <summary>
      /// Compute the dense optical flow.
      /// </summary>
      /// <param name="frame0">Source frame</param>
      /// <param name="frame1">Frame to track (with the same size as <paramref name="frame0"/>)</param>
      /// <param name="u">Flow horizontal component (along x axis)</param>
      /// <param name="v">Flow vertical component (along y axis)</param>
      public void Dense(GpuImage<Gray, byte> frame0, GpuImage<Gray, byte> frame1, GpuImage<Gray, float> u, GpuImage<Gray, float> v)
      {
         GpuInvoke.gpuPryLKOpticalFlowDense(_ptr, frame0, frame1, u, v, IntPtr.Zero);
      }

      public void Sparse(GpuImage<Gray, byte> frame0, GpuImage<Gray, byte> frame1, GpuMat<float> points0, out GpuMat<float> points1, out GpuMat<Byte> status, out GpuMat<float> err)
      {
         points1 = new GpuMat<float>();
         status = new GpuMat<byte>();
         err = new GpuMat<float>();
         GpuInvoke.gpuPryLKOpticalFlowSparse(_ptr, frame0, frame1, points0, points1, status, err);
      }

      /// <summary>
      /// Release all the unmanaged memory associated with this optical flow solver.
      /// </summary>
      protected override void DisposeObject()
      {
         GpuInvoke.gpuPryLKOpticalFlowRelease(ref _ptr);
      }
   }

   public static partial class GpuInvoke
   {
      [DllImport(CvInvoke.EXTERN_GPU_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      internal extern static IntPtr gpuPryLKOpticalFlowCreate(
         Size winSize, int maxLevel, int iters, double derivLambda,
         [MarshalAs(CvInvoke.BoolMarshalType)]
         bool useInitialFlow,
         float minEigThreshold);

      [DllImport(CvInvoke.EXTERN_GPU_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      internal extern static void gpuPryLKOpticalFlowDense(IntPtr flow, IntPtr prevImg, IntPtr nextImg, IntPtr u, IntPtr v, IntPtr err);

      [DllImport(CvInvoke.EXTERN_GPU_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      internal extern static void gpuPryLKOpticalFlowSparse(
         IntPtr flow, 
         IntPtr prevImg, 
         IntPtr nextImg, 
         IntPtr prevPts, 
         IntPtr nextPts,
         IntPtr status, 
         IntPtr err);

      [DllImport(CvInvoke.EXTERN_GPU_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      internal extern static void gpuPryLKOpticalFlowRelease(ref IntPtr flow);
   }
}
