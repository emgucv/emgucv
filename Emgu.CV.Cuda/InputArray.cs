//----------------------------------------------------------------------------
//  Copyright (C) 2004-2021 by EMGU Corporation. All rights reserved.       
//----------------------------------------------------------------------------

#if !(UNITY_IPHONE || UNITY_ANDROID || UNITY_STANDALONE_OSX || UNITY_EDITOR_OSX || UNITY_STANDALONE_WIN || UNITY_EDITOR_WIN || NETFX_CORE)
using System;
using System.Drawing;
using System.Runtime.InteropServices;
using Emgu.CV.Cuda;
using Emgu.CV.CvEnum;
using Emgu.Util;

namespace Emgu.CV
{
   /// <summary>
   /// This is the proxy class for passing read-only input arrays into OpenCV functions.
   /// </summary>
   public partial class InputArray : UnmanagedObject
   {
      /// <summary>
      /// Get the GpuMat from the input array
      /// </summary>
      /// <returns>The GpuMat</returns>
      public Cuda.GpuMat GetGpuMat()
      {
         Cuda.GpuMat m = new Cuda.GpuMat();
         CvInvoke.cveInputArrayGetGpuMat(Ptr, m);
         return m;
      }
   }

   public partial class CvInvoke
   {
      [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      internal static extern void cveInputArrayGetGpuMat(IntPtr ia, IntPtr gpumat);
   }
}
#endif