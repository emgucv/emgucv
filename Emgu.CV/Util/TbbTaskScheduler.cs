//----------------------------------------------------------------------------
//  Copyright (C) 2004-2011 by EMGU. All rights reserved.       
//----------------------------------------------------------------------------

﻿using System;
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
      #region PInvoke
      [DllImport(CvInvoke.EXTERN_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      private static extern IntPtr tbbTaskSchedulerInit();

      [DllImport(CvInvoke.EXTERN_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      private static extern void tbbTaskSchedulerRelease(ref IntPtr scheduler);
      #endregion

      /// <summary>
      /// Initialize the TBB task scheduler
      /// </summary>
      public TbbTaskScheduler()
      {
         _ptr = tbbTaskSchedulerInit();
      }

      /// <summary>
      /// Release the TBB task scheduler
      /// </summary>
      protected override void DisposeObject()
      {
         tbbTaskSchedulerRelease(ref _ptr);
      }
   }
}
