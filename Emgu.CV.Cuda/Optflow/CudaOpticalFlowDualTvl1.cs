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
   public class CudaOpticalFlowDualTvl1 : UnmanagedObject
   {
      /// <summary>
      /// 
      /// </summary>
      public CudaOpticalFlowDualTvl1()
      {
         _ptr = CudaInvoke.cudaOpticalFlowDualTvl1Create();
      }

      /// <summary>
      /// Compute the optical flow.
      /// </summary>
      /// <param name="frame0">Source frame</param>
      /// <param name="frame1">Frame to track (with the same size as <paramref name="frame0"/>)</param>
      /// <param name="u">Flow horizontal component (along x axis)</param>
      /// <param name="v">Flow vertical component (along y axis)</param>
      /// <param name="stream">Use a Stream to call the function asynchronously (non-blocking) or null to call the function synchronously (blocking).</param>
      public void Compute(GpuMat frame0, GpuMat frame1, GpuMat u, GpuMat v)
      {
         CudaInvoke.cudaOpticalFlowDualTvl1Compute(_ptr, frame0, frame1, u, v);
      }

      /// <summary>
      /// Release all the unmanaged memory associated with this optical flow solver.
      /// </summary>
      protected override void DisposeObject()
      {
         CudaInvoke.cudaOpticalFlowDualTvl1Release(ref _ptr);
      }
   }

   public static partial class CudaInvoke
   {
      [DllImport(CvInvoke.ExternCudaLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      internal static extern IntPtr cudaOpticalFlowDualTvl1Create();

      [DllImport(CvInvoke.ExternCudaLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      internal extern static void cudaOpticalFlowDualTvl1Release(ref IntPtr flow);

      [DllImport(CvInvoke.ExternCudaLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      internal extern static void cudaOpticalFlowDualTvl1Compute(IntPtr flow, IntPtr frame0, IntPtr frame1, IntPtr u, IntPtr v);
   }
}
