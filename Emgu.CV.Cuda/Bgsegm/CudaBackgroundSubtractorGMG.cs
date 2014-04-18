//----------------------------------------------------------------------------
//  Copyright (C) 2004-2013 by EMGU. All rights reserved.       
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
   /// <typeparam name="TColor">The color type of the CudaImage to be processed</typeparam>
   public class CudaBackgroundSubtractorGMG<TColor> : UnmanagedObject
       where TColor : struct, IColor
   {
      private CudaImage<Gray, Byte> _forgroundMask;

      /// <summary>
      /// The forground mask
      /// </summary>
      public CudaImage<Gray, Byte> ForgroundMask
      {
         get
         {
            return _forgroundMask;
         }
      }

      /// <summary>
      /// Create a Background/Foreground Segmentation model
      /// </summary>
      public CudaBackgroundSubtractorGMG(int initializationFrames, double decisionThreshold)
      {
         _ptr = CudaInvoke.cudaBackgroundSubtractorGMGCreate(initializationFrames, decisionThreshold);
      }

      /// <summary>
      /// Updates the background model
      /// </summary>
      /// <param name="frame">Next video frame.</param>
      /// <param name="learningRate">The learning rate, use -1.0f for default value.</param>
      /// <param name="stream">Use a Stream to call the function asynchronously (non-blocking) or null to call the function synchronously (blocking).</param>
      public void Update(CudaImage<TColor, Byte> frame, double learningRate, Stream stream)
      {
         if (_forgroundMask == null)
         {
            _forgroundMask = new CudaImage<Gray, byte>(frame.Size);
         }
         CudaInvoke.cudaBackgroundSubtractorGMGApply(_ptr, frame, learningRate, _forgroundMask, stream);
      }

      /// <summary>
      /// Release all the managed resource associated with this object
      /// </summary>
      protected override void ReleaseManagedResources()
      {
         base.ReleaseManagedResources();
         if (_forgroundMask != null)
         {
            _forgroundMask.Dispose();
         }
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
      internal static extern void cudaBackgroundSubtractorGMGApply(IntPtr gmg, IntPtr frame, double learningRate, IntPtr fgMask, IntPtr stream);

      [DllImport(CvInvoke.ExternCudaLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      internal static extern void cudaBackgroundSubtractorGMGRelease(ref IntPtr gmg);
   }
}
