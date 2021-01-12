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
   /// Interface to provide access to the cuda::SparseOpticalFlow class.
   /// </summary>
   public interface ICudaSparseOpticalFlow : IAlgorithm
   {
      /// <summary>
      /// Pointer the the native cuda::sparseOpticalFlow object.
      /// </summary>
      IntPtr SparseOpticalFlowPtr { get; }
   }

   public static partial class CudaInvoke
   {
      /// <summary>
      /// Calculates a sparse optical flow.
      /// </summary>
      /// <param name="sparseFlow">The sparse optical flow</param>
      /// <param name="prevImg">First input image.</param>
      /// <param name="nextImg">Second input image of the same size and the same type as <paramref name="prevImg"/>.</param>
      /// <param name="prevPts">Vector of 2D points for which the flow needs to be found.</param>
      /// <param name="nextPts">Output vector of 2D points containing the calculated new positions of input features in the second image.</param>
      /// <param name="status">Output status vector. Each element of the vector is set to 1 if the flow for the corresponding features has been found. Otherwise, it is set to 0.</param>
      /// <param name="err">Optional output vector that contains error response for each point (inverse confidence).</param>
      /// <param name="stream">Use a Stream to call the function asynchronously (non-blocking) or null to call the function synchronously (blocking).</param>
      public static void Calc(this ICudaSparseOpticalFlow sparseFlow, IInputArray prevImg, IInputArray nextImg, IInputArray prevPts, IInputOutputArray nextPts, IOutputArray status, IOutputArray err = null, Stream stream = null)
      {
         using (InputArray iaPrevImg = prevImg.GetInputArray())
         using (InputArray iaNextImg = nextImg.GetInputArray())
         using (InputArray iaPrevPts = prevPts.GetInputArray())
         using (InputOutputArray ioaNextPts = nextPts.GetInputOutputArray())
         using (OutputArray oaStatus = status.GetOutputArray())
         using (OutputArray oaErr = (err == null ? OutputArray.GetEmpty() : err.GetOutputArray()))
            cudaSparseOpticalFlowCalc(
                sparseFlow.SparseOpticalFlowPtr, 
                iaPrevImg, 
                iaNextImg, 
                iaPrevPts, 
                ioaNextPts,
                oaStatus, 
                oaErr, 
                (stream == null) ? IntPtr.Zero : stream.Ptr);
      }

      [DllImport(CvInvoke.ExternCudaLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      private static extern void cudaSparseOpticalFlowCalc(
         IntPtr opticalFlow,
         IntPtr prevImg, IntPtr nextImg,
         IntPtr prevPts, IntPtr nextPts,
         IntPtr status,
         IntPtr err,
         IntPtr stream);
   }
}
