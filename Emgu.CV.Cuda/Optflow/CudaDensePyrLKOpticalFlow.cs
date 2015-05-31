//----------------------------------------------------------------------------
//  Copyright (C) 2004-2015 by EMGU Corporation. All rights reserved.       
//----------------------------------------------------------------------------

﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Runtime.InteropServices;
using Emgu.CV.Structure;
using Emgu.Util;

namespace Emgu.CV.Cuda
{
   /// <summary>
   /// PyrLK optical flow
   /// </summary>
   public class CudaDensePyrLKOpticalFlow : UnmanagedObject, ICudaDenseOpticalFlow
   {
      private IntPtr _denseFlow;

      /// <summary>
      /// Create the PyrLK optical flow solver
      /// </summary>
      /// <param name="winSize">Windows size. Use 21x21 for default</param>
      /// <param name="maxLevel">The maximum number of pyramid levels.</param>
      /// <param name="iters">The number of iterations.</param>
      /// <param name="useInitialFlow">Weather or not use the initial flow in the input matrix.</param>
      public CudaDensePyrLKOpticalFlow(Size winSize, int maxLevel = 3, int iters = 30, bool useInitialFlow = false)
      {
         _ptr = CudaInvoke.cudaDensePyrLKOpticalFlowCreate(ref winSize, maxLevel, iters, useInitialFlow, ref _denseFlow);
      }

      /// <summary>
      /// Release all the unmanaged memory associated with this optical flow solver.
      /// </summary>
      protected override void DisposeObject()
      {
         if (_ptr != IntPtr.Zero)
         {
            CudaInvoke.cudaDensePyrLKOpticalFlowRelease(ref _ptr);
            _denseFlow = IntPtr.Zero;
         }
      }

      IntPtr ICudaDenseOpticalFlow.DenseOpticalFlowPtr
      {
         get { return _denseFlow; }
      }
   }

   public static partial class CudaInvoke
   {
      [DllImport(CvInvoke.ExternCudaLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      internal extern static IntPtr cudaDensePyrLKOpticalFlowCreate(
         ref Size winSize, int maxLevel, int iters,
         [MarshalAs(CvInvoke.BoolMarshalType)]
         bool useInitialFlow,
         ref IntPtr denseFlow);

      [DllImport(CvInvoke.ExternCudaLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      internal extern static void cudaDensePyrLKOpticalFlowRelease(ref IntPtr flow);
   }
}
