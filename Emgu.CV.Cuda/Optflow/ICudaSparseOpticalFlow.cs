//----------------------------------------------------------------------------
//  Copyright (C) 2004-2015 by EMGU Corporation. All rights reserved.       
//----------------------------------------------------------------------------

﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Runtime.InteropServices;
﻿using Emgu.CV.Structure;
using Emgu.Util;

namespace Emgu.CV.Cuda
{
   public interface ICudaSparseOpticalFlow
   {
      IntPtr SparseOpticalFlowPtr { get; }
   }

   public static partial class CudaInvoke
   {
      public static void Calc(this ICudaSparseOpticalFlow sparseFlow, IInputArray prevImg, IInputArray nextImg, IInputArray prevPts, IInputOutputArray nextPts, IOutputArray status = null, IOutputArray err = null, Stream stream = null)
      {
         using (InputArray iaPrevImg = prevImg.GetInputArray())
         using (InputArray iaNextImg = nextImg.GetInputArray())
         using (InputArray iaPrevPts = prevPts.GetInputArray())
         using (InputOutputArray ioaNextPts = nextPts.GetInputOutputArray())
         using (OutputArray oaStatus = (status == null ? OutputArray.GetEmpty() : status.GetOutputArray()))
         using (OutputArray oaErr = (err == null ? OutputArray.GetEmpty() : err.GetOutputArray()))
            cudaSparseOpticalFlowCalc(sparseFlow.SparseOpticalFlowPtr, iaPrevImg, iaNextImg, iaPrevPts, ioaNextPts,
               oaStatus, oaErr, (stream == null) ? IntPtr.Zero : stream.Ptr);
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
