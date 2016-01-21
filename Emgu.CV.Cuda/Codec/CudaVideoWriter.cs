//----------------------------------------------------------------------------
//  Copyright (C) 2004-2016 by EMGU Corporation. All rights reserved.       
//----------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Runtime.InteropServices;
//using System.Runtime.Remoting.Messaging;
using System.Text;
using Emgu.CV.Features2D;
using Emgu.CV.Structure;
using Emgu.CV.Util;
using Emgu.Util;

namespace Emgu.CV.Cuda
{
   /// <summary>
   /// Works only under Windows, Supports olny H264 video codec and AVI files.
   /// </summary>
   public class CudaVideoWriter : UnmanagedObject
   {
      /// <summary>
      /// Surface format
      /// </summary>
      public enum SurfaceFormat
      {
         UYVY = 0,
         YUY2,
         YV12,
         NV12,
         IYUV,
         BGR,
         GRAY = BGR
      }

      public CudaVideoWriter(String fileName, Size frameSize, double fps, SurfaceFormat format = SurfaceFormat.BGR)
      {
         using (CvString s = new CvString(fileName))
         {
            _ptr = CudaInvoke.cudaVideoWriterCreate(s, ref frameSize, fps, format);
         }
      }

      protected override void DisposeObject()
      {
         CudaInvoke.cudaVideoWriterRelease(ref _ptr);
      }


      public void Write(IInputArray frame, bool lastFrame = false)
      {
         using (InputArray iaFrame = frame.GetInputArray())
            CudaInvoke.cudaVideoWriterWrite(_ptr, iaFrame, lastFrame);
      }
   }

   public static partial class CudaInvoke
   {
      [DllImport(CvInvoke.ExternCudaLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      internal static extern IntPtr cudaVideoWriterCreate(IntPtr fileName, ref Size frameSize, double fps, CudaVideoWriter.SurfaceFormat format);

      [DllImport(CvInvoke.ExternCudaLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      internal static extern void cudaVideoWriterRelease(ref IntPtr writer);

      [DllImport(CvInvoke.ExternCudaLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      internal static extern void cudaVideoWriterWrite(IntPtr writer, IntPtr frame, 
         [MarshalAs(CvInvoke.BoolMarshalType)]
         bool lastFrame);
   }
}
