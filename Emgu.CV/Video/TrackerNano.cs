//----------------------------------------------------------------------------
//  Copyright (C) 2004-2026 by EMGU Corporation. All rights reserved.       
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
using Emgu.CV.Dnn;

namespace Emgu.CV
{
    /// <summary>
    /// The Nano tracker is a super lightweight dnn-based general object tracking.
    /// Nano tracker is much faster and extremely lightweight due to special model structure, the whole model size is about 1.9 MB. Nano tracker needs two models: one for feature extraction (backbone) and the another for localization (neckhead).
    /// </summary>
    public partial class TrackerNano : Tracker
    {
        private IntPtr _sharedPtr;

        /// <summary>
        /// Create a Nano tracker
        /// </summary>
        /// <param name="backbone">Path to the model for feature extraction. Model download link: https://github.com/HonglinChu/SiamTrackers/tree/master/NanoTrack/models/nanotrackv2</param>
        /// <param name="neckhead">Path to the model for localization. Model download link: https://github.com/HonglinChu/SiamTrackers/tree/master/NanoTrack/models/nanotrackv2</param>
        /// <param name="backend">The preferred DNN backend</param>
        /// <param name="target">The preferred DNN target</param>
        public TrackerNano(
            String backbone,
            String neckhead,
            Dnn.Backend backend = Dnn.Backend.Default,
            Dnn.Target target = Target.Cpu)
        {
            using (CvString csBackbone = new CvString(backbone))
            using (CvString csNeckhead = new CvString(neckhead))
                _ptr = CvInvoke.cveTrackerNanoCreate(
                    csBackbone,
                    csNeckhead,
                    backend,
                    target,
                    ref _trackerPtr,
                    ref _sharedPtr);
        }

        /// <summary>
        /// Release the unmanaged resources associated with this tracker
        /// </summary>
        protected override void DisposeObject()
        {
            if (IntPtr.Zero != _ptr)
            {
                CvInvoke.cveTrackerNanoRelease(ref _sharedPtr);
                _ptr = IntPtr.Zero;
            }

            base.DisposeObject();
        }
    }

    public static partial class CvInvoke
    {
        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern IntPtr cveTrackerNanoCreate(
            IntPtr backbone,
            IntPtr neckhead,
            Dnn.Backend backend,
            Dnn.Target target,
            ref IntPtr tracker,
            ref IntPtr sharedPtr);

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern void cveTrackerNanoRelease(ref IntPtr sharedPtr);
    }
}