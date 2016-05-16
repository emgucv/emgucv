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
   /// <summary>
   /// Long-term tracker
   /// </summary>
   public class Tracker : UnmanagedObject
   {
      /// <summary>
      /// Creates a tracker by its name.
      /// </summary>
      /// <param name="trackerType">Tracker type, The following detector types are supported: "MIL" – TrackerMIL; "BOOSTING" – TrackerBoosting</param>
      public Tracker(String trackerType)
      {
         using (CvString trackerTypeStr = new CvString(trackerType))
            _ptr = ContribInvoke.cveTrackerCreate(trackerTypeStr);
      }

      /// <summary>
      /// Initialize the tracker with a know bounding box that surrounding the target.
      /// </summary>
      /// <param name="image">The initial frame</param>
      /// <param name="boundingBox">The initial boundig box</param>
      /// <returns></returns>
      public bool Init(Mat image, Rectangle boundingBox)
      {
         return ContribInvoke.cveTrackerInit(_ptr, image, ref boundingBox);
      }

      /// <summary>
      /// Update the tracker, find the new most likely bounding box for the target.
      /// </summary>
      /// <param name="image">The current frame</param>
      /// <param name="boundingBox">The boundig box that represent the new target location, if true was returned, not modified otherwise</param>
      /// <returns>True means that target was located and false means that tracker cannot locate target in current frame. Note, that latter does not imply that tracker has failed, maybe target is indeed missing from the frame (say, out of sight)</returns>
      public bool Update(Mat image, out Rectangle boundingBox)
      {
         boundingBox = new Rectangle();
         return ContribInvoke.cveTrackerUpdate(_ptr, image, ref boundingBox);
      }

      /// <summary>
      /// Release the unmanaged memory associated with this tracker
      /// </summary>
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