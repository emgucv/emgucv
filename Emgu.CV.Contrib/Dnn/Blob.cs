//----------------------------------------------------------------------------
//  Copyright (C) 2004-2016 by EMGU Corporation. All rights reserved.       
//----------------------------------------------------------------------------

#if !(__IOS__ || UNITY_IPHONE || NETFX_CORE)
using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using Emgu.CV.Structure;
using Emgu.CV.Text;
using Emgu.CV.Util;
using Emgu.Util;
using System.Diagnostics;

namespace Emgu.CV.Dnn
{
   public class Blob : UnmanagedObject
   {
      public Blob(IInputArray image, int dstCn = -1)
      {
         using (InputArray iaImage = image.GetInputArray())
            _ptr = ContribInvoke.cveDnnBlobCreateFromInputArray(iaImage, dstCn);
      }

      public Mat MatRef()
      {
         Mat m = new Mat();
         ContribInvoke.cveDnnBlobMatRef(_ptr, m);
         return m;
      }

      protected override void DisposeObject()
      {
         if (_ptr != IntPtr.Zero)
         {
            ContribInvoke.cveDnnBlobRelease(ref _ptr);
         }
      }
   }
}

namespace Emgu.CV
{
   public static partial class ContribInvoke
   {
      [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      internal static extern IntPtr cveDnnBlobCreateFromInputArray(IntPtr image, int dstCn);
      [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      internal static extern void cveDnnBlobMatRef(IntPtr blob, IntPtr outMat);
      [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      internal static extern void cveDnnBlobRelease(ref IntPtr blob);
   }
}

#endif