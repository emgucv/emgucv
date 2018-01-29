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
    /// This is a real-time object tracking based on a novel on-line version of the AdaBoost algorithm. 
    /// The classifier uses the surrounding background as negative examples in update step to avoid the drifting problem.
    /// </summary>
    public class TrackerBoosting : Tracker
    {
        /// <summary>
        /// Create a Boosting Tracker
        /// </summary>
        /// <param name="numClassifiers">The number of classifiers to use in a OnlineBoosting algorithm</param>
        /// <param name="samplerOverlap">Search region parameters to use in a OnlineBoosting algorithm</param>
        /// <param name="samplerSearchFactor">search region parameters to use in a OnlineBoosting algorithm</param>
        /// <param name="iterationInit">The initial iterations</param>
        /// <param name="featureSetNumFeatures">Number of features, a good value would be 10*numClassifiers + iterationInit</param>
        public TrackerBoosting(
            int numClassifiers = 100, 
            float samplerOverlap = 0.99f, 
            float samplerSearchFactor = 1.8f, 
            int iterationInit = 50, 
            int featureSetNumFeatures = 100*10+50)
        {
            ContribInvoke.cveTrackerBoostingCreate(numClassifiers, samplerOverlap, samplerSearchFactor, iterationInit, featureSetNumFeatures, ref _trackerPtr);
        }

        /// <summary>
        /// Release all the unmanaged memory associated with this Boosting Tracker
        /// </summary>
        protected override void DisposeObject()
        {
            if (IntPtr.Zero != _ptr)
                ContribInvoke.cveTrackerBoostingRelease(ref _ptr);
            base.DisposeObject();
            
        }
    }

    /// <summary>
    /// Median Flow tracker implementation.
    /// The tracker is suitable for very smooth and predictable movements when object is visible throughout
    /// the whole sequence.It's quite and accurate for this type of problems (in particular, it was shown
    /// by authors to outperform MIL). During the implementation period the code at
    /// http://www.aonsquared.co.uk/node/5, the courtesy of the author Arthur Amarra, was used for the
    /// reference purpose.
    /// </summary>
    public class TrackerMedianFlow : Tracker
    {
        /// <summary>Create a median flow tracker</summary>
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

        /// <summary>
        /// Release the unmanaged resources associated with this tracker
        /// </summary>
        protected override void DisposeObject()
        {
            if (IntPtr.Zero != _ptr)
                ContribInvoke.cveTrackerMedianFlowRelease(ref _ptr);
            base.DisposeObject();
        }
    }

    /// <summary>
    /// The MIL algorithm trains a classifier in an online manner to separate the object from the background.
    /// Multiple Instance Learning avoids the drift problem for a robust tracking.
    /// Original code can be found here http://vision.ucsd.edu/~bbabenko/project_miltrack.shtml
    /// </summary>
    public class TrackerMIL : Tracker
    {
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

        /// <summary>
        /// Release all the unmanaged memory associated with this tracker
        /// </summary>
        protected override void DisposeObject()
        {
            if (IntPtr.Zero != _ptr)
                ContribInvoke.cveTrackerMILRelease(ref _ptr);
            base.DisposeObject();
        }
    }

    /// <summary>
    /// TLD is a novel tracking framework that explicitly decomposes the long-term tracking task into tracking, learning and detection.
    /// </summary>
    /// <remarks>The tracker follows the object from frame to frame. The detector localizes all appearances that have been observed so far and corrects the tracker if necessary. The learning estimates detector's errors and updates it to avoid these errors in the future.</remarks>
    public class TrackerTLD : Tracker
    {
        /// <summary>
        /// Creates a TLD tracker
        /// </summary>
        public TrackerTLD()
        {
            _ptr = ContribInvoke.cveTrackerTLDCreate(ref _trackerPtr);
        }

        /// <summary>
        /// Release the unmanaged resources associated with this tracker
        /// </summary>
        protected override void DisposeObject()
        {
            if (IntPtr.Zero != _ptr)
                ContribInvoke.cveTrackerTLDRelease(ref _ptr);
            base.DisposeObject();
        }
    }

    /// <summary>
    /// KCF is a novel tracking framework that utilizes properties of circulant matrix to enhance the processing speed.
    /// </summary>
    /// <remarks>
    /// The original paper of KCF is available at http://www.robots.ox.ac.uk/~joao/publications/henriques_tpami2015.pdf as well as the matlab implementation.
    /// For more information about KCF with color-names features, please refer to http://www.cvl.isy.liu.se/research/objrec/visualtracking/colvistrack/index.html </remarks>
    public class TrackerKCF : Tracker
    {
        /// <summary>
        /// Create a KCF Tracker
        /// </summary>
        public TrackerKCF()
        {
            _ptr = ContribInvoke.cveTrackerKCFCreate(ref _trackerPtr);
        }

        /// <summary>
        /// Release the unmanaged resources associated with this tracker
        /// </summary>
        protected override void DisposeObject()
        {
            if (IntPtr.Zero != _ptr)
                ContribInvoke.cveTrackerKCFRelease(ref _ptr);
            base.DisposeObject();
        }
    }

    /// <summary>
    /// GOTURN is kind of trackers based on Convolutional Neural Networks (CNN). While taking all advantages of CNN trackers, GOTURN is much faster due to offline training without online fine-tuning nature. GOTURN tracker addresses the problem of single target tracking: given a bounding box label of an object in the first frame of the video, we track that object through the rest of the video. NOTE: Current method of GOTURN does not handle occlusions; however, it is fairly robust to viewpoint changes, lighting changes, and deformations. Inputs of GOTURN are two RGB patches representing Target and Search patches resized to 227x227. Outputs of GOTURN are predicted bounding box coordinates, relative to Search patch coordinate system, in format X1,Y1,X2,Y2.
    /// </summary>
    /// <remarks>Original paper is here: http://davheld.github.io/GOTURN/GOTURN.pdf As long as original authors implementation: https://github.com/davheld/GOTURN#train-the-tracker Implementation of training algorithm is placed in separately here due to 3d-party dependencies: https://github.com/Auron-X/GOTURN_Training_Toolkit GOTURN architecture goturn.prototxt and trained model goturn.caffemodel are accessible on opencv_extra GitHub repository.</remarks>
    public class TrackerGOTURN : Tracker
    {
        /// <summary>
        /// Create a GOTURN tracker
        /// </summary>
        public TrackerGOTURN()
        {
            _ptr = ContribInvoke.cveTrackerGOTURNCreate(ref _trackerPtr);
        }

        /// <summary>
        /// Release the unmanaged resources associated with this tracker
        /// </summary>
        protected override void DisposeObject()
        {
            if (IntPtr.Zero != _ptr)
                ContribInvoke.cveTrackerGOTURNRelease(ref _ptr);
            base.DisposeObject();
        }
    }

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

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern IntPtr cveTrackerTLDCreate(ref IntPtr tracker);
        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern void cveTrackerTLDRelease(ref IntPtr tracker);

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern IntPtr cveTrackerKCFCreate(ref IntPtr tracker);
        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern void cveTrackerKCFRelease(ref IntPtr tracker);

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern IntPtr cveTrackerGOTURNCreate(ref IntPtr tracker);
        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern void cveTrackerGOTURNRelease(ref IntPtr tracker);
    }
}
