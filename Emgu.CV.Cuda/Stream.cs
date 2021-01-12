//----------------------------------------------------------------------------
//  Copyright (C) 2004-2021 by EMGU Corporation. All rights reserved.       
//----------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Text;
using Emgu.Util;

namespace Emgu.CV.Cuda
{
   /// <summary>
   /// Encapculates Cuda Stream. Provides interface for async coping.
   /// Passed to each function that supports async kernel execution.
   /// Reference counting is enabled
   /// </summary>
   public class Stream : UnmanagedObject
   {
      /// <summary>
      /// Create a new Cuda Stream
      /// </summary>
      public Stream()
      {
         _ptr = CudaInvoke.streamCreate();
      }

      /// <summary>
      /// Wait for the completion
      /// </summary>
      public void WaitForCompletion()
      {
         CudaInvoke.streamWaitForCompletion(_ptr);
      }

      /// <summary>
      /// Check if the stream is completed
      /// </summary>
      public bool Completed
      {
         get { return CudaInvoke.streamQueryIfComplete(_ptr); }
      }

      /*
      /// <summary>
      /// Copy the src GpuMat to dst GpuMat asyncronously
      /// </summary>
      /// <typeparam name="TDepth">The type of depth for the GpuMat</typeparam>
      /// <param name="src">The source matrix</param>
      /// <param name="dst">The destination matrix. Must be the same size and same number of channels</param>
      public void Copy<TDepth>(GpuMat<TDepth> src, GpuMat<TDepth> dst) where TDepth : new()
      {
         GpuInvoke.streamEnqueueCopy(_ptr, src, dst);
      }*/

      /// <summary>
      /// Release the stream
      /// </summary>
      protected override void DisposeObject()
      {
         CudaInvoke.streamRelease(ref _ptr);
      }
   }

   public static partial class CudaInvoke
   {
      [DllImport(CvInvoke.ExternCudaLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      internal static extern IntPtr streamCreate();

      [DllImport(CvInvoke.ExternCudaLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      internal static extern void streamRelease(ref IntPtr stream);

      [DllImport(CvInvoke.ExternCudaLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      internal static extern void streamWaitForCompletion(IntPtr stream);

      [DllImport(CvInvoke.ExternCudaLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      [return: MarshalAs(CvInvoke.BoolMarshalType)]
      internal static extern bool streamQueryIfComplete(IntPtr stream);
      /*
      [DllImport(CvInvoke.EXTERN_GPU_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      internal static extern void streamEnqueueCopy(IntPtr stream, IntPtr src, IntPtr dst);
       */
   }
}
