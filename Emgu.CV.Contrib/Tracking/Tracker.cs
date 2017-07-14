//----------------------------------------------------------------------------
//  Copyright (C) 2004-2017 by EMGU Corporation. All rights reserved.       
//----------------------------------------------------------------------------

#if !(__IOS__ || UNITY_IPHONE || NETFX_CORE)

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
    public class TrackerBoosting : Tracker
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="numClassifiers">The number of classifiers to use in a OnlineBoosting algorithm</param>
        /// <param name="samplerOverlap">Search region parameters to use in a OnlineBoosting algorithm</param>
        /// <param name="samplerSearchFactor">search region parameters to use in a OnlineBoosting algorithm</param>
        /// <param name="iterationInit">The initial iterations</param>
        /// <param name="featureSetNumFeatures">Number of features, a good value would be 10*numClassifiers + iterationInit</param>
        public TrackerBoosting(int numClassifiers = 100, float samplerOverlap = 0.99f, float samplerSearchFactor = 1.8f, int iterationInit = 50, int featureSetNumFeatures = 100*10+50)
        {
            ContribInvoke.cveTrackerBoostingCreate(numClassifiers, samplerOverlap, samplerSearchFactor, iterationInit, featureSetNumFeatures, ref _trackerPtr);
        }

        protected override void DisposeObject()
        {
            base.DisposeObject();
            ContribInvoke.cveTrackerBoostingRelease(ref _ptr);
        }
    }

    public class TrackerMedianFlow : Tracker
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="pointsInGrid">Points in grid, use 10 for default.</param>
        /// <param name="winSize">Win size, use (3, 3) for default</param>
        /// <param name="maxLevel">Max level, use 5 for default.</param>
        /// <param name="termCriteria">Termination criteria, use count = 20 and eps = 0.3 for default</param>
        /// <param name="winSizeNCC">win size NCC, use (30, 30) for default</param>
        /// <param name="maxMedianLengthOfDisplacementDifference">Max median length of displacement difference</param>
        public TrackerMedianFlow(int pointsInGrid, Size winSize, int maxLevel, MCvTermCriteria termCriteria, Size winSizeNCC, double maxMedianLengthOfDisplacementDifference = 10)
        {
            ContribInvoke.cveTrackerMedianFlowCreate(pointsInGrid, ref winSize, maxLevel, ref termCriteria, ref winSizeNCC, maxMedianLengthOfDisplacementDifference, ref _trackerPtr);
        }

        protected override void DisposeObject()
        {
            base.DisposeObject();
            ContribInvoke.cveTrackerMedianFlowRelease(ref _ptr);
        }
    }

    public class TrackerMIL : Tracker
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="samplerInitInRadius">radius for gathering positive instances during init</param>
        /// <param name="samplerInitMaxNegNum">negative samples to use during init</param>
        /// <param name="samplerSearchWinSize">size of search window</param>
        /// <param name="samplerTrackInRadius">radius for gathering positive instances during tracking</param>
        /// <param name="samplerTrackMaxPosNum">positive samples to use during tracking</param>
        /// <param name="samplerTrackMaxNegNum">negative samples to use during tracking</param>
        /// <param name="featureSetNumFeatures">features</param>
        public TrackerMIL(
            float samplerInitInRadius,
        int samplerInitMaxNegNum,  
        float samplerSearchWinSize,  
        float samplerTrackInRadius,  
        int samplerTrackMaxPosNum,  
        int samplerTrackMaxNegNum,  
        int featureSetNumFeatures)  
        {
            ContribInvoke.cveTrackerMILCreate(
                samplerInitInRadius,
            samplerInitMaxNegNum,
            samplerSearchWinSize,
            samplerTrackInRadius,
            samplerTrackMaxPosNum,
            samplerTrackMaxNegNum,
            featureSetNumFeatures, ref _trackerPtr);
        }

        protected override void DisposeObject()
        {
            base.DisposeObject();
            ContribInvoke.cveTrackerMILRelease(ref _ptr);
        }
    }

    /// <summary>
    /// Long-term tracker
    /// </summary>
    public abstract class Tracker : UnmanagedObject
    {

        protected IntPtr _trackerPtr;

        /// <summary>
        /// Initialize the tracker with a know bounding box that surrounding the target.
        /// </summary>
        /// <param name="image">The initial frame</param>
        /// <param name="boundingBox">The initial bounding box</param>
        /// <returns></returns>
        public bool Init(Mat image, Rectangle boundingBox)
        {
            return ContribInvoke.cveTrackerInit(_trackerPtr, image, ref boundingBox);
        }

        /// <summary>
        /// Update the tracker, find the new most likely bounding box for the target.
        /// </summary>
        /// <param name="image">The current frame</param>
        /// <param name="boundingBox">The bounding box that represent the new target location, if true was returned, not modified otherwise</param>
        /// <returns>True means that target was located and false means that tracker cannot locate target in current frame. Note, that latter does not imply that tracker has failed, maybe target is indeed missing from the frame (say, out of sight)</returns>
        public bool Update(Mat image, out Rectangle boundingBox)
        {
            boundingBox = new Rectangle();
            return ContribInvoke.cveTrackerUpdate(_trackerPtr, image, ref boundingBox);
        }


        /// <summary>
        /// Release the unmanaged memory associated with this tracker
        /// </summary>
        protected override void DisposeObject()
        {
            _trackerPtr = IntPtr.Zero;
        }
    }

}

namespace Emgu.CV
{
    public static partial class ContribInvoke
    {
        //[DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        //internal static extern IntPtr cveTrackerCreate(IntPtr trackerType);

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        [return: MarshalAs(CvInvoke.BoolMarshalType)]
        internal static extern bool cveTrackerInit(IntPtr tracker, IntPtr image, ref Rectangle boundingBox);


        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        [return: MarshalAs(CvInvoke.BoolMarshalType)]
        internal static extern bool cveTrackerUpdate(IntPtr tracker, IntPtr image, ref Rectangle boundingBox);

        //[DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        //internal static extern void cveTrackerRelease(ref IntPtr tracker);

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern IntPtr cveTrackerMedianFlowCreate(int pointsInGrid, ref Size winSize, int maxLevel, ref MCvTermCriteria termCriteria, ref Size winSizeNCC, double maxMedianLengthOfDisplacementDifference, ref IntPtr tracker);

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern void cveTrackerMedianFlowRelease(ref IntPtr tracker);

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern IntPtr cveTrackerBoostingCreate(int numClassifiers, float samplerOverlap, float samplerSearchFactor, int iterationInit, int featureSetNumFeatures, ref IntPtr tracker);

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern void cveTrackerBoostingRelease(ref IntPtr tracker);

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern IntPtr cveTrackerMILCreate(
            float samplerInitInRadius,
            int samplerInitMaxNegNum,
            float samplerSearchWinSize,
            float samplerTrackInRadius,
            int samplerTrackMaxPosNum,
            int samplerTrackMaxNegNum,
            int featureSetNumFeatures,
            ref IntPtr tracker);
        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern void cveTrackerMILRelease(ref IntPtr tracker);
    }
}

#endif