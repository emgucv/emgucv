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
   /// Background/Foreground Segmentation Algorithm.
   /// </summary>
   public class CudaBackgroundSubtractorGMG : UnmanagedObject
   {
      /// <summary>
      /// Create a Background/Foreground Segmentation model
      /// </summary>
      public CudaBackgroundSubtractorGMG(int initializationFrames = 120, double decisionThreshold = 0.8)
      {
         _ptr = CudaInvoke.cudaBackgroundSubtractorGMGCreate(initializationFrames, decisionThreshold);
      }

      /// <summary>
      /// Updates the background model
      /// </summary>
      /// <param name="frame">Next video frame.</param>
      /// <param name="learningRate">The learning rate, use -1.0f for default value.</param>
      /// <param name="stream">Use a Stream to call the function asynchronously (non-blocking) or null to call the function synchronously (blocking).</param>
      public void Apply(IInputArray frame, IOutputArray forgroundMask, double learningRate = -1, Stream stream = null)
      {
         using (InputArray iaFrame = frame.GetInputArray())
         using (OutputArray oaForgroundMask = forgroundMask.GetOutputArray())
            CudaInvoke.cudaBackgroundSubtractorGMGApply(_ptr, iaFrame, oaForgroundMask, learningRate, stream);
      }

      /// <summary>
      /// Release all the unmanaged resource associated with this object
      /// </summary>
      protected override void DisposeObject()
      {
         CudaInvoke.cudaBackgroundSubtractorGMGRelease(ref _ptr);
      }
   }

   public static partial class CudaInvoke
   {
      [DllImport(CvInvoke.ExternCudaLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      internal static extern IntPtr cudaBackgroundSubtractorGMGCreate(int initializationFrames, double decisionThreshold);

      [DllImport(CvInvoke.ExternCudaLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      internal static extern void cudaBackgroundSubtractorGMGApply(IntPtr gmg, IntPtr frame, IntPtr fgMask, double learningRate, IntPtr stream);

      [DllImport(CvInvoke.ExternCudaLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      internal static extern void cudaBackgroundSubtractorGMGRelease(ref IntPtr gmg);
   }
}
