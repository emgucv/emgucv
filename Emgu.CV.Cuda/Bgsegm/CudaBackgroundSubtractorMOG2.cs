//----------------------------------------------------------------------------
//  Copyright (C) 2004-2015 by EMGU Corporation. All rights reserved.       
//----------------------------------------------------------------------------

﻿using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;
using Emgu.CV.Structure;
using Emgu.CV.Util;
using Emgu.Util;

namespace Emgu.CV.Cuda
{
   /// <summary>
   /// Gaussian Mixture-based Background/Foreground Segmentation Algorithm.
   /// </summary>
   public class CudaBackgroundSubtractorMOG2 : UnmanagedObject
   {

      /// <summary>
      /// Create a Gaussian Mixture-based Background/Foreground Segmentation model
      /// </summary>
      public CudaBackgroundSubtractorMOG2(int history = 500, double varThreshold = 16, bool detectShadows = true)
      {
         _ptr = CudaInvoke.cudaBackgroundSubtractorMOG2Create(history, varThreshold, detectShadows);
      }

      /// <summary>
      /// Updates the background model
      /// </summary>
      /// <param name="frame">Next video frame.</param>
      /// <param name="learningRate">The learning rate, use -1.0f for default value.</param>
      /// <param name="stream">Use a Stream to call the function asynchronously (non-blocking) or null to call the function synchronously (blocking).</param>
      public void Update(IInputArray frame, double learningRate, IOutputArray forgroundMask, Stream stream = null)
      {
         using (InputArray iaFrame = frame.GetInputArray())
         using (OutputArray oaForgroundMask = forgroundMask.GetOutputArray())
            CudaInvoke.cudaBackgroundSubtractorMOG2Apply(_ptr, iaFrame, oaForgroundMask, learningRate, stream);
      }

      /// <summary>
      /// Release all the unmanaged resource associated with this object
      /// </summary>
      protected override void DisposeObject()
      {
         CudaInvoke.cudaBackgroundSubtractorMOG2Release(ref _ptr);
      }
   }

   public static partial class CudaInvoke
   {
      [DllImport(CvInvoke.ExternCudaLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      internal static extern IntPtr cudaBackgroundSubtractorMOG2Create(
         int history,
         double varThreshold,
         [MarshalAs(CvInvoke.BoolMarshalType)]
         bool detectShadows);

      [DllImport(CvInvoke.ExternCudaLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      internal static extern void cudaBackgroundSubtractorMOG2Apply(IntPtr mog, IntPtr frame, IntPtr fgMask, double learningRate, IntPtr stream);

      [DllImport(CvInvoke.ExternCudaLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      internal static extern void cudaBackgroundSubtractorMOG2Release(ref IntPtr mog);
   }
}
