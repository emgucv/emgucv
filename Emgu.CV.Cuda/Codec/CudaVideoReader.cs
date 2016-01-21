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
   public class CudaVideoReader : UnmanagedObject
   {
      public CudaVideoReader(String fileName)
      {
         using (CvString s = new CvString(fileName))
            _ptr = CudaInvoke.cudaVideoReaderCreate(s);
      }

      public bool NextFrame(IOutputArray frame)
      {
         using (OutputArray oaFrame = frame.GetOutputArray())
         {
            return CudaInvoke.cudaVideoReaderNextFrame(_ptr, oaFrame);
         }
      }

      protected override void DisposeObject()
      {
         if (_ptr != IntPtr.Zero)
            CudaInvoke.cudaVideoReaderRelease(ref _ptr);
      }
   }

   public static partial class CudaInvoke
   {
      [DllImport(CvInvoke.ExternCudaLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      internal static extern IntPtr cudaVideoReaderCreate(IntPtr fileName);

      [DllImport(CvInvoke.ExternCudaLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      internal static extern void cudaVideoReaderRelease(ref IntPtr reader);


      [DllImport(CvInvoke.ExternCudaLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      [return: MarshalAs(CvInvoke.BoolMarshalType)]
      internal static extern bool cudaVideoReaderNextFrame(IntPtr reader, IntPtr frame);
   }
}
