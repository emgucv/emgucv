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
   /// This class is used to track multiple objects using the specified tracker algorithm. The MultiTracker is naive implementation of multiple object tracking. It process the tracked objects independently without any optimization accross the tracked objects.
   /// </summary>
   public class MultiTracker : UnmanagedObject
   {
      /// <summary>
      /// Constructor. In the case of trackerType is given, it will be set as the default algorithm for all trackers.
      /// </summary>
      /// <param name="trackerType">The name of the tracker algorithm to be used</param>
      public MultiTracker(String trackerType)
      {
         using (CvString trackerTypeStr = new CvString(trackerType))
            _ptr = ContribInvoke.cveMultiTrackerCreate(trackerTypeStr);
      }

      /// <summary>
      /// Add a new object to be tracked. The defaultAlgorithm will be used the newly added tracker.
      /// </summary>
      /// <param name="image">Tnput image</param>
      /// <param name="boundingBox">A rectangle represents ROI of the tracked object</param>
      /// <returns>True if sucessfully added</returns>
      public bool Add(Mat image, Rectangle boundingBox)
      {
         return ContribInvoke.cveMultiTrackerAdd(_ptr, image, ref boundingBox);
      }

      /// <summary>
      /// Add a new object to be tracked
      /// </summary>
      /// <param name="trackerType">The name of the tracker algorithm to be used</param>
      /// <param name="image">Input image</param>
      /// <param name="boundingBox">S rectangle represents ROI of the tracked object</param>
      /// <returns>True if sucessfully added</returns>
      public bool Add(String trackerType, Mat image, Rectangle boundingBox)
      {
         using (CvString trackerTypeStr = new CvString(trackerType))
            return ContribInvoke.cveMultiTrackerAddType(_ptr, trackerTypeStr, image, ref boundingBox);
      }

      /// <summary>
      /// Update the current tracking status. The result will be saved in the internal storage.
      /// </summary>
      /// <param name="image">Input image</param>
      /// <param name="boundingBox">the tracking result, represent a list of ROIs of the tracked objects.</param>
      /// <returns>True id update success</returns>
      public bool Update(Mat image, VectorOfRect boundingBox)
      {
         return ContribInvoke.cveMultiTrackerUpdate(_ptr, image, boundingBox);
      }

      /// <summary>
      /// Release the unmanaged memory associated with this multi-tracker.
      /// </summary>
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