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
   public class MultiTracker : UnmanagedObject
   {
      public MultiTracker(String trackerType)
      {
         using (CvString trackerTypeStr = new CvString(trackerType))
            _ptr = ContribInvoke.cveMultiTrackerCreate(trackerTypeStr);
      }

      public bool Add(Mat image, Rectangle boundingBox)
      {
         return ContribInvoke.cveMultiTrackerAdd(_ptr, image, ref boundingBox);
      }

      public bool Add(String trackerType, Mat image, Rectangle boundingBox)
      {
         using (CvString trackerTypeStr = new CvString(trackerType))
            return ContribInvoke.cveMultiTrackerAddType(_ptr, trackerTypeStr, image, ref boundingBox);
      }

      public bool Update(Mat image, VectorOfRect boundingBox)
      {
         return ContribInvoke.cveMultiTrackerUpdate(_ptr, image, boundingBox);
      }

      protected override void DisposeObject()
      {
         if (_ptr != IntPtr.Zero)
            ContribInvoke.cveMultiTrackerRelease(ref _ptr);
      }
   }
}

namespace Emgu.CV
{
   public static partial class ContribInvoke
   {
      [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      internal static extern IntPtr cveMultiTrackerCreate(IntPtr trackerType);

      [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      [return:MarshalAs(CvInvoke.BoolMarshalType)]
      internal static extern bool cveMultiTrackerAdd(IntPtr tracker, IntPtr image, ref Rectangle boundingBox);

      [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      [return: MarshalAs(CvInvoke.BoolMarshalType)]
      internal static extern bool cveMultiTrackerAddType(IntPtr tracker, IntPtr trackerType, IntPtr image, ref Rectangle boundingBox);

      [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      [return: MarshalAs(CvInvoke.BoolMarshalType)]
      internal static extern bool cveMultiTrackerUpdate(IntPtr tracker, IntPtr image, IntPtr boundingBox);

      [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      internal static extern void cveMultiTrackerRelease(ref IntPtr tracker);
   }
}