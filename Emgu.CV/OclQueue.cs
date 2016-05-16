//----------------------------------------------------------------------------
//  Copyright (C) 2004-2016 by EMGU Corporation. All rights reserved.       
//----------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Text;
using Emgu.Util;
using System.Runtime.InteropServices;

namespace Emgu.CV
{
   public class OclQueue : UnmanagedObject
   {
      public OclQueue()
      {
         _ptr = OclInvoke.oclQueueCreate();
      }

      public void Finish()
      {
         OclInvoke.oclQueueFinish(_ptr);
      }

      protected override void DisposeObject()
      {
         if (_ptr != IntPtr.Zero)
            OclInvoke.oclQueueRelease(ref _ptr);
      }
   }

   /// <summary>
   /// Class that contains ocl functions.
   /// </summary>
   public static partial class OclInvoke
   {
      [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      internal static extern IntPtr oclQueueCreate();

      [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      internal static extern IntPtr oclQueueFinish(IntPtr queue);

      [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      internal static extern void oclQueueRelease(ref IntPtr queue);
   }
}
