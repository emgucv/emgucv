//----------------------------------------------------------------------------
//  Copyright (C) 2004-2013 by EMGU. All rights reserved.       
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

namespace Emgu.CV.OpenCL
{
   /// <summary>
   /// Ocl match template buffer, used by the OpenCL version of MatchTemplate function.
   /// </summary>
   public class OclMatchTemplateBuf : UnmanagedObject
   {
      /// <summary>
      /// Create a OclMatchTemplateBuf
      /// </summary>
      public OclMatchTemplateBuf()
      {
         _ptr = OclInvoke.oclMatchTemplateBufCreate();
      }

      /// <summary>
      /// Release the buffer
      /// </summary>
      protected override void DisposeObject()
      {
         OclInvoke.oclMatchTemplateBufRelease(ref _ptr);
      }
   }

   public static partial class OclInvoke
   {
      [DllImport(CvInvoke.EXTERN_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      internal static extern IntPtr oclMatchTemplateBufCreate();

      [DllImport(CvInvoke.EXTERN_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      internal static extern void oclMatchTemplateBufRelease(ref IntPtr buf);

   }
}
