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
   public class CudaBroxOpticalFlow : UnmanagedObject, ICudaDenseOpticalFlow
   {
      private IntPtr _denseFlow;

      /// <summary>
      /// Create the Brox optical flow solver
      /// </summary>
      /// <param name="alpha">Flow smoothness</param>
      /// <param name="gamma">Gradient constancy importance</param>
      /// <param name="scaleFactor">Pyramid scale factor</param>
      /// <param name="innerIterations">Number of lagged non-linearity iterations (inner loop)</param>
      /// <param name="outerIterations">Number of warping iterations (number of pyramid levels)</param>
      /// <param name="solverIterations">Number of linear system solver iterations</param>
      public CudaBroxOpticalFlow(double alpha = 0.197, double gamma = 50, double scaleFactor = 0.8, int innerIterations = 5, int outerIterations = 150, int solverIterations = 10)
      {
         _ptr = CudaInvoke.cudaBroxOpticalFlowCreate(alpha, gamma, scaleFactor, innerIterations, outerIterations, solverIterations, ref _denseFlow);
      }

      /// <summary>
      /// Release all the unmanaged memory associated with this optical flow solver.
      /// </summary>
      protected override void DisposeObject()
      {
         if (_ptr != IntPtr.Zero)
         {
            CudaInvoke.cudaBroxOpticalFlowRelease(ref _ptr);
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
      internal extern static IntPtr cudaBroxOpticalFlowCreate(double alpha, double gamma, double scaleFactor, int innerIterations, int outerIterations, int solverIterations, ref IntPtr denseFlow);

      [DllImport(CvInvoke.ExternCudaLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      internal extern static void cudaBroxOpticalFlowRelease(ref IntPtr flow);
   }
}
