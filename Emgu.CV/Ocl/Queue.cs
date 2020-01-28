//----------------------------------------------------------------------------
//  Copyright (C) 2004-2020 by EMGU Corporation. All rights reserved.       
//----------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Text;
using Emgu.Util;
using Emgu.CV;
using System.Runtime.InteropServices;

namespace Emgu.CV.Ocl
{
   /// <summary>
   /// An OpenCL Queue
   /// </summary>
   public class Queue : UnmanagedObject
   {
      /// <summary>
      /// OpenCL queue
      /// </summary>
      public Queue()
      {
         _ptr = OclInvoke.oclQueueCreate();
      }

      /// <summary>
      /// Wait for the queue to finish
      /// </summary>
      public void Finish()
      {
         OclInvoke.oclQueueFinish(_ptr);
      }

      /// <summary>
      /// Release the unmanaged memory associated with this object.
      /// </summary>
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
