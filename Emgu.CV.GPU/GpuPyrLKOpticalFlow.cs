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
      /// <param name="useInitialFlow">Weather or not use the initial flow in the input matrix. Use false for default.</param>
      public GpuPyrLKOpticalFlow(Size winSize, int maxLevel, int iters, bool useInitialFlow)
      {
         _ptr = GpuInvoke.gpuPryLKOpticalFlowCreate(winSize, maxLevel, iters, useInitialFlow);
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

      /// <summary>
      /// Calculate an optical flow for a sparse feature set.
      /// </summary>
      /// <param name="frame0">First 8-bit input image (supports both grayscale and color images).</param>
      /// <param name="frame1">Second input image of the same size and the same type as <paramref name="frame0"/></param>
      /// <param name="points0">
      /// Vector of 2D points for which the flow needs to be found. It must be one row
      /// matrix with 2 channels
      /// </param>
      /// <param name="points1">
      /// Output vector of 2D points (with single-precision two channel floating-point coordinates)
      /// containing the calculated new positions of input features in the second image.</param>
      /// <param name="status">
      /// Output status vector (CV_8UC1 type). Each element of the vector is set to 1 if the
      /// flow for the corresponding features has been found. Otherwise, it is set to 0.
      /// </param>
      /// <param name="err">
      /// Output vector (CV_32FC1 type) that contains the difference between patches around
      /// the original and moved points or min eigen value if getMinEigenVals is checked. It can be
      /// null, if not needed.
      /// </param>
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
         Size winSize, int maxLevel, int iters,
         [MarshalAs(CvInvoke.BoolMarshalType)]
         bool useInitialFlow);

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
