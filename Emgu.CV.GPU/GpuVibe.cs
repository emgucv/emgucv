//----------------------------------------------------------------------------
//  Copyright (C) 2004-2013 by EMGU. All rights reserved.       
//----------------------------------------------------------------------------

/*
﻿using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;
using Emgu.CV.Structure;
using Emgu.CV.Util;
using Emgu.Util;

namespace Emgu.CV.GPU
{
   public class GpuVibe<TColor> : UnmanagedObject
            where TColor : struct, IColor
   {

      private GpuImage<Gray, Byte> _forgroundMask;

      public GpuImage<Gray, Byte> ForgroundMask
      {
         get
         {
            return _forgroundMask;
         }
      }

      public GpuVibe(ulong rngSeed, GpuImage<TColor, Byte> firstFrame, Stream stream)
      {
         _ptr = GpuInvoke.gpuVibeCreate(rngSeed, firstFrame, stream);
         _forgroundMask = new GpuImage<Gray, byte>(firstFrame.Size);
      }

      public void Update(GpuImage<TColor, Byte> frame, Stream stream)
      {
         GpuInvoke.gpuVibeCompute(_ptr, frame, _forgroundMask, stream);
      }

      protected override void ReleaseManagedResources()
      {
         base.ReleaseManagedResources();
         _forgroundMask.Dispose();
      }

      protected override void DisposeObject()
      {
         GpuInvoke.gpuVibeRelease(ref _ptr);
      }
   }

   public static partial class GpuInvoke
   {
      [DllImport(CvInvoke.EXTERN_GPU_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      internal static extern IntPtr gpuVibeCreate(ulong rngSeed, IntPtr firstFrame, IntPtr stream);

      [DllImport(CvInvoke.EXTERN_GPU_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      internal static extern void gpuVibeCompute(IntPtr vibe, IntPtr frame, IntPtr fgMask, IntPtr stream);

      [DllImport(CvInvoke.EXTERN_GPU_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      internal static extern void gpuVibeRelease(ref IntPtr vibe);
   }
}
*/