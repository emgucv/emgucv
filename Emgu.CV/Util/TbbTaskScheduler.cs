//----------------------------------------------------------------------------
//  Copyright (C) 2004-2015 by EMGU Corporation. All rights reserved.       
//----------------------------------------------------------------------------

 using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Emgu.Util;

namespace Emgu.CV.Util
{
   /// <summary>
   /// This class canbe used to initiate TBB. Only usefull if it is compiled with TBB support
   /// </summary>
   public class TbbTaskScheduler : UnmanagedObject
   {
      /// <summary>
      /// Initialize the TBB task scheduler
      /// </summary>
      public TbbTaskScheduler()
      {
         _ptr = CvInvoke.tbbTaskSchedulerInit();
      }

      /// <summary>
      /// Release the TBB task scheduler
      /// </summary>
      protected override void DisposeObject()
      {
         CvInvoke.tbbTaskSchedulerRelease(ref _ptr);
      }
   }
}

namespace Emgu.CV
{
   public static partial class CvInvoke
   {
      [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      internal static extern IntPtr tbbTaskSchedulerInit();

      [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      internal static extern void tbbTaskSchedulerRelease(ref IntPtr scheduler);
   }
}
