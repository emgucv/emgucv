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
   /// Brox optical flow
   /// </summary>
   public class CudaBroxOpticalFlow : UnmanagedObject
   {
      /// <summary>
      /// Create the Brox optical flow solver
      /// </summary>
      /// <param name="alpha">Flow smoothness</param>
      /// <param name="gamma">Gradient constancy importance</param>
      /// <param name="scaleFactor">Pyramid scale factor</param>
      /// <param name="innerIterations">Number of lagged non-linearity iterations (inner loop)</param>
      /// <param name="outerIterations">Number of warping iterations (number of pyramid levels)</param>
      /// <param name="solverIterations">Number of linear system solver iterations</param>
      public CudaBroxOpticalFlow(float alpha, float gamma, float scaleFactor, int innerIterations, int outerIterations, int solverIterations)
      {
         _ptr = CudaInvoke.cudaBroxOpticalFlowCreate(alpha, gamma, scaleFactor, innerIterations, outerIterations, solverIterations);
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
         CudaInvoke.cudaBroxOpticalFlowCompute(_ptr, frame0, frame1, u, v, stream);
      }

      /// <summary>
      /// Release all the unmanaged memory associated with this optical flow solver.
      /// </summary>
      protected override void DisposeObject()
      {
         CudaInvoke.cudaBroxOpticalFlowRelease(ref _ptr);
      }
   }

   public static partial class CudaInvoke
   {
      [DllImport(CvInvoke.ExternCudaLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      internal extern static IntPtr cudaBroxOpticalFlowCreate(float alpha, float gamma, float scaleFactor, int innerIterations, int outerIterations, int solverIterations);

      [DllImport(CvInvoke.ExternCudaLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      internal extern static void cudaBroxOpticalFlowRelease(ref IntPtr flow);

      [DllImport(CvInvoke.ExternCudaLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      internal extern static void cudaBroxOpticalFlowCompute(IntPtr flow, IntPtr frame0, IntPtr frame1, IntPtr u, IntPtr v, IntPtr stream);
   }
}
