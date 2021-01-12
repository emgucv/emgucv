//----------------------------------------------------------------------------
//  Copyright (C) 2004-2021 by EMGU Corporation. All rights reserved.       
//----------------------------------------------------------------------------

﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Runtime.InteropServices;
﻿using Emgu.CV.Structure;
using Emgu.Util;

namespace Emgu.CV.Cuda
{
   /// <summary>
   /// Cuda Dense Optical flow
   /// </summary>
   public interface ICudaDenseOpticalFlow : IAlgorithm
   {
      /// <summary>
      /// Pointer to cv::cuda::denseOpticalFlow
      /// </summary>
      IntPtr DenseOpticalFlowPtr { get; }
   }

   public static partial class CudaInvoke
   {
      /// <summary>
      /// Calculates a dense optical flow.
      /// </summary>
      /// <param name="denseFlow">The dense optical flow object</param>
      /// <param name="i0">first input image.</param>
      /// <param name="i1">second input image of the same size and the same type as <paramref name="i0"/>.</param>
      /// <param name="flow">computed flow image that has the same size as I0 and type CV_32FC2.</param>
      /// <param name="stream">Use a Stream to call the function asynchronously (non-blocking) or null to call the function synchronously (blocking).</param>
      public static void Calc(this ICudaDenseOpticalFlow denseFlow, IInputArray i0, IInputArray i1, IInputOutputArray flow, Stream stream = null)
      {
         using (InputArray iaI0 = i0.GetInputArray())
         using (InputArray iaI1 = i1.GetInputArray())
         using (InputOutputArray ioaFlow = flow.GetInputOutputArray())
            cudaDenseOpticalFlowCalc(denseFlow.DenseOpticalFlowPtr, iaI0, iaI1, ioaFlow, (stream == null) ?  IntPtr.Zero : stream.Ptr);
      }

      [DllImport(CvInvoke.ExternCudaLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      private static extern void cudaDenseOpticalFlowCalc(
         IntPtr opticalFlow, 
         IntPtr i0, 
         IntPtr i1, 
         IntPtr flow, 
         IntPtr stream);
   }
}
