//----------------------------------------------------------------------------
//  Copyright (C) 2004-2020 by EMGU Corporation. All rights reserved.       
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
    public abstract class Tracker : UnmanagedObject
    {
        /// <summary>
        /// The native pointer to the tracker
        /// </summary>
        protected IntPtr _trackerPtr;

        /// <summary>
        /// Initialize the tracker with a know bounding box that surrounding the target.
        /// </summary>
        /// <param name="image">The initial frame</param>
        /// <param name="boundingBox">The initial bounding box</param>
        /// <returns>True if successful.</returns>
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
        internal static extern IntPtr cveTrackerMedianFlowCreate(int pointsInGrid, ref Size winSize, int maxLevel, ref MCvTermCriteria termCriteria, ref Size winSizeNCC, double maxMedianLengthOfDisplacementDifference, ref IntPtr tracker, ref IntPtr sharedPtr);

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern void cveTrackerMedianFlowRelease(ref IntPtr tracker, ref IntPtr sharedPtr);

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern IntPtr cveTrackerBoostingCreate(int numClassifiers, float samplerOverlap, float samplerSearchFactor, int iterationInit, int featureSetNumFeatures, ref IntPtr tracker, ref IntPtr sharedPtr);

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern void cveTrackerBoostingRelease(ref IntPtr tracker, ref IntPtr sharedPtr);

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

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern IntPtr cveTrackerTLDCreate(ref IntPtr tracker, ref IntPtr sharedPtr);
        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern void cveTrackerTLDRelease(ref IntPtr tracker, ref IntPtr sharedPtr);

        //[DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        //internal static extern IntPtr cveTrackerKCFCreate(ref IntPtr tracker);
        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern IntPtr cveTrackerKCFCreate(
            float detectThresh,
            float sigma,
            float lambda,
            float interpFactor,
            float outputSigmaFactor,
            float pcaLearningRate,
            [MarshalAs(CvInvoke.BoolMarshalType)]
            bool resize,
            [MarshalAs(CvInvoke.BoolMarshalType)]
            bool splitCoeff,
            [MarshalAs(CvInvoke.BoolMarshalType)]
            bool wrapKernel,
            [MarshalAs(CvInvoke.BoolMarshalType)]
            bool compressFeature,
            int maxPatchSize,
            int compressedSize,
            Tracking.TrackerKCF.Mode descPca,
            Tracking.TrackerKCF.Mode descNpca,
            ref IntPtr tracker,
            ref IntPtr sharedPtr);
        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern void cveTrackerKCFRelease(ref IntPtr tracker, ref IntPtr sharedPtr);

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern IntPtr cveTrackerGOTURNCreate(ref IntPtr tracker, ref IntPtr sharedPtr);
        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern void cveTrackerGOTURNRelease(ref IntPtr tracker, ref IntPtr sharedPtr);

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern IntPtr cveTrackerMOSSECreate(ref IntPtr tracker, ref IntPtr sharedPtr);
        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern void cveTrackerMOSSERelease(ref IntPtr tracker, ref IntPtr sharedPTr);

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern IntPtr cveTrackerCSRTCreate(
            [MarshalAs(CvInvoke.BoolMarshalType)]
            bool useHog,
            [MarshalAs(CvInvoke.BoolMarshalType)]
            bool useColorNames,
            [MarshalAs(CvInvoke.BoolMarshalType)]
            bool useGray,
            [MarshalAs(CvInvoke.BoolMarshalType)]
            bool useRgb,
            [MarshalAs(CvInvoke.BoolMarshalType)]
            bool useChannelWeights,
            [MarshalAs(CvInvoke.BoolMarshalType)]
            bool useSegmentation,
            IntPtr windowFunction,
            float kaiserAlpha,
            float chebAttenuation,
            float templateSize,
            float gslSigma,
            float hogOrientations,
            float hogClip,
            float padding,
            float filterLr,
            float weightsLr,
            int numHogChannelsUsed,
            int admmIterations,
            int histogramBins,
            float histogramLr,
            int backgroundRatio,
            int numberOfScales,
            float scaleSigmaFactor,
            float scaleModelMaxArea,
            float scaleLr,
            float scaleStep,
            ref IntPtr tracker,
            ref IntPtr sharedPtr);

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern void cveTrackerCSRTRelease(ref IntPtr tracker, ref IntPtr sharedPtr);
    }
}
