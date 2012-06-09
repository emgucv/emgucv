//----------------------------------------------------------------------------
//  Copyright (C) 2004-2012 by EMGU. All rights reserved.       
//----------------------------------------------------------------------------

﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Text;
using Emgu.CV.Structure;
using Emgu.CV.Util;
using Emgu.Util;

namespace Emgu.CV.GPU
{
   /// <summary>
   /// Gpu match template buffer, used by the gpu version of MatchTemplate function.
   /// </summary>
   public class GpuMatchTemplateBuf : UnmanagedObject
   {
      /// <summary>
      /// Create a GpuMatchTemplateBuf
      /// </summary>
      public GpuMatchTemplateBuf()
      {
         _ptr = GpuInvoke.gpuMatchTemplateBufCreate();
      }

      /// <summary>
      /// Release the buffer
      /// </summary>
      protected override void DisposeObject()
      {
         GpuInvoke.gpuMatchTemplateBufRelease(ref _ptr);
      }
   }

   public static partial class GpuInvoke
   {
      [DllImport(CvInvoke.EXTERN_GPU_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      internal static extern IntPtr gpuMatchTemplateBufCreate();

      [DllImport(CvInvoke.EXTERN_GPU_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      internal static extern void gpuMatchTemplateBufRelease(ref IntPtr buf);

   }
}
