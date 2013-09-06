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

namespace Emgu.CV.GPU
{
   /// <summary>
   /// Gaussian Mixture-based Background/Foreground Segmentation Algorithm.
   /// </summary>
   /// <typeparam name="TColor">The color type of the GpuImage to be processed</typeparam>
   public class GpuMOG2<TColor> : UnmanagedObject
       where TColor : struct, IColor
   {
      private GpuImage<Gray, Byte> _forgroundMask;

      /// <summary>
      /// The forground mask
      /// </summary>
      public GpuImage<Gray, Byte> ForgroundMask
      {
         get
         {
            return _forgroundMask;
         }
      }

      /// <summary>
      /// Create a Gaussian Mixture-based Background/Foreground Segmentation model
      /// </summary>
      public GpuMOG2(int history, double varThreshold, bool detectShadows)
      {
         _ptr = GpuInvoke.gpuMog2Create(history, varThreshold,detectShadows);
      }

      /// <summary>
      /// Updates the background model
      /// </summary>
      /// <param name="frame">Next video frame.</param>
      /// <param name="learningRate">The learning rate, use -1.0f for default value.</param>
      /// <param name="stream">Use a Stream to call the function asynchronously (non-blocking) or null to call the function synchronously (blocking).</param>
      public void Update(GpuImage<TColor, Byte> frame, float learningRate, Stream stream)
      {
         if (_forgroundMask == null)
         {
            _forgroundMask = new GpuImage<Gray, byte>(frame.Size);
         }
         GpuInvoke.gpuMog2Compute(_ptr, frame, learningRate, _forgroundMask, stream);
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
         GpuInvoke.gpuMog2Release(ref _ptr);
      }
   }

   public static partial class GpuInvoke
   {
      [DllImport(CvInvoke.EXTERN_GPU_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      internal static extern IntPtr gpuMog2Create(int history, double varThreshold, bool detectShadows);

      [DllImport(CvInvoke.EXTERN_GPU_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      internal static extern void gpuMog2Compute(IntPtr mog, IntPtr frame, float learningRate, IntPtr fgMask, IntPtr stream);

      [DllImport(CvInvoke.EXTERN_GPU_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      internal static extern void gpuMog2Release(ref IntPtr mog);
   }
}
