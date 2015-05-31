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
   public interface ICudaDenseOpticalFlow
   {
      IntPtr DenseOpticalFlowPtr { get; }
   }

   public static partial class CudaInvoke
   {
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
