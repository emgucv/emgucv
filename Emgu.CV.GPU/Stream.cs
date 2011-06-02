//----------------------------------------------------------------------------
//  Copyright (C) 2004-2011 by EMGU. All rights reserved.       
//----------------------------------------------------------------------------

﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Emgu.Util;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace Emgu.CV.GPU
{
   /// <summary>
   /// Encapculates Cuda Stream. Provides interface for async coping.
   /// Passed to each function that supports async kernel execution.
   /// Reference counting is enabled
   /// </summary>
   public class Stream : UnmanagedObject
   {
      #region PInvoke
      [DllImport(CvInvoke.EXTERN_GPU_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      private static extern IntPtr streamCreate();

      [DllImport(CvInvoke.EXTERN_GPU_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      private static extern void streamRelease(ref IntPtr stream);

      [DllImport(CvInvoke.EXTERN_GPU_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      private static extern void streamWaitForCompletion(IntPtr stream);

      [DllImport(CvInvoke.EXTERN_GPU_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      [return: MarshalAs(CvInvoke.BoolMarshalType)]
      private static extern bool streamQueryIfComplete(IntPtr stream);

      [DllImport(CvInvoke.EXTERN_GPU_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      private static extern void streamEnqueueCopy(IntPtr stream, IntPtr src, IntPtr dst);
      #endregion

      /// <summary>
      /// Create a new Cuda Stream
      /// </summary>
      public Stream()
      {
         _ptr = streamCreate();
      }

      /// <summary>
      /// Wait for the completion
      /// </summary>
      public void WaitForCompletion()
      {
         streamWaitForCompletion(_ptr);
      }

      /// <summary>
      /// Check if the stream is completed
      /// </summary>
      public bool Completed
      {
         get { return streamQueryIfComplete(_ptr); }
      }

      /// <summary>
      /// Copy the src GpuMat to dst GpuMat asyncronously
      /// </summary>
      /// <typeparam name="TDepth">The type of depth for the GpuMat</typeparam>
      /// <param name="src">The source matrix</param>
      /// <param name="dst">The destination matrix. Must be the same size and same number of channels</param>
      public void Copy<TDepth>(GpuMat<TDepth> src, GpuMat<TDepth> dst) where TDepth : new()
      {
         streamEnqueueCopy(_ptr, src, dst);
      }

      /// <summary>
      /// Release the stream
      /// </summary>
      protected override void DisposeObject()
      {
         streamRelease(ref _ptr);
      }
   }
}
