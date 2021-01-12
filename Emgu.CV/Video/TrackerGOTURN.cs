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
    /// GOTURN is kind of trackers based on Convolutional Neural Networks (CNN). While taking all advantages of CNN trackers, GOTURN is much faster due to offline training without online fine-tuning nature. GOTURN tracker addresses the problem of single target tracking: given a bounding box label of an object in the first frame of the video, we track that object through the rest of the video. NOTE: Current method of GOTURN does not handle occlusions; however, it is fairly robust to viewpoint changes, lighting changes, and deformations. Inputs of GOTURN are two RGB patches representing Target and Search patches resized to 227x227. Outputs of GOTURN are predicted bounding box coordinates, relative to Search patch coordinate system, in format X1,Y1,X2,Y2.
    /// </summary>
    /// <remarks>Original paper is here: http://davheld.github.io/GOTURN/GOTURN.pdf As long as original authors implementation: https://github.com/davheld/GOTURN#train-the-tracker Implementation of training algorithm is placed in separately here due to 3d-party dependencies: https://github.com/Auron-X/GOTURN_Training_Toolkit GOTURN architecture goturn.prototxt and trained model goturn.caffemodel are accessible on opencv_extra GitHub repository.</remarks>
    public class TrackerGOTURN : Tracker
    {
        private IntPtr _sharedPtr;

        /// <summary>
        /// Create a GOTURN tracker
        /// </summary>
        public TrackerGOTURN()
        {
            _ptr = CvInvoke.cveTrackerGOTURNCreate(ref _trackerPtr, ref _sharedPtr);
        }

        /// <summary>
        /// Release the unmanaged resources associated with this tracker
        /// </summary>
        protected override void DisposeObject()
        {
            if (IntPtr.Zero != _ptr)
                CvInvoke.cveTrackerGOTURNRelease(ref _ptr, ref _sharedPtr);
            base.DisposeObject();
        }
    }
}

namespace Emgu.CV
{
    public static partial class CvInvoke
    {

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern IntPtr cveTrackerGOTURNCreate(ref IntPtr tracker, ref IntPtr sharedPtr);

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern void cveTrackerGOTURNRelease(ref IntPtr tracker, ref IntPtr sharedPtr);

    }
}
