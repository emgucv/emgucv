//----------------------------------------------------------------------------
//  Copyright (C) 2004-2016 by EMGU Corporation. All rights reserved.       
//----------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using Emgu.CV.Structure;
using Emgu.CV.Text;
using Emgu.CV.Util;
using Emgu.Util;
using System.Diagnostics;
using System.Drawing;

namespace Emgu.CV.Tracking
{
   public class Tracker : UnmanagedObject
   {
      public Tracker(String trackerType)
      {
         using (CvString trackerTypeStr = new CvString(trackerType))
            _ptr = ContribInvoke.cveTrackerCreate(trackerTypeStr);
      }

      public bool Init(Mat image, Rectangle boundingBox)
      {
         return ContribInvoke.cveTrackerInit(_ptr, image, ref boundingBox);
      }

      public bool Update(Mat image, out Rectangle boundingBox)
      {
         boundingBox = new Rectangle();
         return ContribInvoke.cveTrackerUpdate(_ptr, image, ref boundingBox);
      }

      protected override void DisposeObject()
      {
         if (_ptr != IntPtr.Zero)
            ContribInvoke.cveTrackerRelease(ref _ptr);
      }
   }
}

namespace Emgu.CV
{
   public static partial class ContribInvoke
   {
      [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      internal static extern IntPtr cveTrackerCreate(IntPtr trackerType);
      [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      [return:MarshalAs(CvInvoke.BoolMarshalType)]
      internal static extern bool cveTrackerInit(IntPtr tracker, IntPtr image, ref Rectangle boundingBox);

      
      [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      [return: MarshalAs(CvInvoke.BoolMarshalType)]
      internal static extern bool cveTrackerUpdate(IntPtr tracker, IntPtr image, ref Rectangle boundingBox);

      [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      internal static extern void cveTrackerRelease(ref IntPtr tracker);
   }
}