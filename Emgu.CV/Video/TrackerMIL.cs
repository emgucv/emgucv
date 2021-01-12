//----------------------------------------------------------------------------
//  Copyright (C) 2004-2021 by EMGU Corporation. All rights reserved.       
//----------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using Emgu.CV.Structure;
using Emgu.CV.Util;
using Emgu.Util;
using System.Diagnostics;
using System.Drawing;

namespace Emgu.CV
{
    /// <summary>
    /// The MIL algorithm trains a classifier in an online manner to separate the object from the background.
    /// Multiple Instance Learning avoids the drift problem for a robust tracking.
    /// Original code can be found here http://vision.ucsd.edu/~bbabenko/project_miltrack.shtml
    /// </summary>
    public class TrackerMIL : Tracker
    {
        private IntPtr _sharedPtr;

        /// <summary>
        /// Creates a MIL Tracker
        /// </summary>
        /// <param name="samplerInitInRadius">radius for gathering positive instances during init</param>
        /// <param name="samplerInitMaxNegNum">negative samples to use during init</param>
        /// <param name="samplerSearchWinSize">size of search window</param>
        /// <param name="samplerTrackInRadius">radius for gathering positive instances during tracking</param>
        /// <param name="samplerTrackMaxPosNum">positive samples to use during tracking</param>
        /// <param name="samplerTrackMaxNegNum">negative samples to use during tracking</param>
        /// <param name="featureSetNumFeatures">features</param>
        public TrackerMIL(
            float samplerInitInRadius = 3.0f,
            int samplerInitMaxNegNum = 65,
            float samplerSearchWinSize = 25.0f,
            float samplerTrackInRadius = 4.0f,
            int samplerTrackMaxPosNum = 100000,
            int samplerTrackMaxNegNum = 65,
            int featureSetNumFeatures = 250)
        {
            CvInvoke.cveTrackerMILCreate(
                samplerInitInRadius,
                samplerInitMaxNegNum,
                samplerSearchWinSize,
                samplerTrackInRadius,
                samplerTrackMaxPosNum,
                samplerTrackMaxNegNum,
                featureSetNumFeatures,
                ref _trackerPtr,
                ref _sharedPtr);
        }

        /// <summary>
        /// Release all the unmanaged memory associated with this tracker
        /// </summary>
        protected override void DisposeObject()
        {
            if (IntPtr.Zero != _ptr)
                CvInvoke.cveTrackerMILRelease(ref _ptr, ref _sharedPtr);
            base.DisposeObject();
        }
    }

}


namespace Emgu.CV
{
    public static partial class CvInvoke
    {

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern IntPtr cveTrackerMILCreate(
            float samplerInitInRadius,
            int samplerInitMaxNegNum,
            float samplerSearchWinSize,
            float samplerTrackInRadius,
            int samplerTrackMaxPosNum,
            int samplerTrackMaxNegNum,
            int featureSetNumFeatures,
            ref IntPtr tracker,
            ref IntPtr sharedPtr);
        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern void cveTrackerMILRelease(ref IntPtr tracker, ref IntPtr sharedPtr);

    }
}
