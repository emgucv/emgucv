using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Emgu.Util;

namespace Emgu.CV.Util
{
   public class TbbTaskScheduler : UnmanagedObject
   {
      #region PInvoke
      [DllImport(CvInvoke.EXTERN_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      private static extern IntPtr tbbTaskSchedulerInit();

      [DllImport(CvInvoke.EXTERN_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      private static extern void tbbTaskSchedulerRelease(ref IntPtr scheduler);
      #endregion

      public TbbTaskScheduler()
      {
         _ptr = tbbTaskSchedulerInit();
      }

      protected override void DisposeObject()
      {
         tbbTaskSchedulerRelease(ref _ptr);
      }
   }
}
