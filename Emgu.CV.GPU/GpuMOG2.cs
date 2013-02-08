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
   public class GpuMOG2<TColor> : UnmanagedObject
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

      public GpuMOG2(int nMixtures)
      {
         _ptr = GpuInvoke.gpuMog2Create(nMixtures);
      }

      public void Update(GpuImage<TColor, Byte> frame, float learningRate, Stream stream)
      {
         if (_forgroundMask == null)
         {
            _forgroundMask = new GpuImage<Gray,byte>(frame.Size);
         }
         GpuInvoke.gpuMog2Compute(_ptr, frame, learningRate, _forgroundMask, stream);
      }

      protected override void ReleaseManagedResources()
      {
         base.ReleaseManagedResources();
         if (_forgroundMask != null)
         {
            _forgroundMask.Dispose();
         }
      }

      protected override void DisposeObject()
      {
         GpuInvoke.gpuMog2Release(ref _ptr);
      }
   }

   public static partial class GpuInvoke
   {
      [DllImport(CvInvoke.EXTERN_GPU_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      internal static extern IntPtr gpuMog2Create(int nMixtures);

      [DllImport(CvInvoke.EXTERN_GPU_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      internal static extern void gpuMog2Compute(IntPtr mog, IntPtr frame, float learningRate, IntPtr fgMask, IntPtr stream);

      [DllImport(CvInvoke.EXTERN_GPU_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      internal static extern void gpuMog2Release(ref IntPtr mog);
   }
}
