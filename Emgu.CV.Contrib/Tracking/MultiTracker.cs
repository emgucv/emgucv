//----------------------------------------------------------------------------
//  Copyright (C) 2004-2018 by EMGU Corporation. All rights reserved.       
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

        public MultiTracker()
        {

            _ptr = ContribInvoke.cveMultiTrackerCreate();
        }

        /// <summary>
        /// Add a new object to be tracked. The defaultAlgorithm will be used the newly added tracker.
        /// </summary>
        /// <param name="tracker">The tracker to use for tracking the image</param>
        /// <param name="image">Input image</param>
        /// <param name="boundingBox">A rectangle represents ROI of the tracked object</param>
        /// <returns>True if successfully added</returns>
        public bool Add(Tracker tracker, Mat image, Rectangle boundingBox)
        {
            return ContribInvoke.cveMultiTrackerAdd(_ptr, tracker, image, ref boundingBox);
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
    /// <summary>
    /// Class that contains entry points for the Contrib module.
    /// </summary>
    public static partial class ContribInvoke
    {
        static ContribInvoke()
        {
            CvInvoke.CheckLibraryLoaded();
        }

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern IntPtr cveMultiTrackerCreate();

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        [return: MarshalAs(CvInvoke.BoolMarshalType)]
        internal static extern bool cveMultiTrackerAdd(IntPtr multiTracker, IntPtr tracker, IntPtr image, ref Rectangle boundingBox);

        /*
        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        [return: MarshalAs(CvInvoke.BoolMarshalType)]
        internal static extern bool cveMultiTrackerAddType(IntPtr tracker, IntPtr trackerType, IntPtr image, ref Rectangle boundingBox);
        */
        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        [return: MarshalAs(CvInvoke.BoolMarshalType)]
        internal static extern bool cveMultiTrackerUpdate(IntPtr tracker, IntPtr image, IntPtr boundingBox);

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern void cveMultiTrackerRelease(ref IntPtr tracker);
    }
}
