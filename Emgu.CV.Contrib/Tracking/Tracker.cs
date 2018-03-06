//----------------------------------------------------------------------------
//  Copyright (C) 2004-2017 by EMGU Corporation. All rights reserved.       
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
            if (IntPtr.Zero == _ptr)
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

    public class TrackerTLD : Tracker
    {
        public TrackerTLD()
        {
            _ptr = ContribInvoke.cveTrackerTLDCreate(ref _trackerPtr);
        }

        /// <summary>
        /// Release the unmanaged resources associated with this tracker
        /// </summary>
        protected override void DisposeObject()
        {
            if (IntPtr.Zero == _ptr)
                ContribInvoke.cveTrackerTLDRelease(ref _ptr);
            base.DisposeObject();
        }
    }

    /// <summary>
    /// KCF is a novel tracking framework that utilizes properties of circulant matrix to enhance the processing speed.
    /// This tracking method is an implementation of @cite KCF_ECCV which is extended to KFC with color-names features(@cite KCF_CN).
    /// The original paper of KCF is available at http://home.isr.uc.pt/~henriques/circulant/index.html
    /// as well as the matlab implementation.For more information about KCF with color-names features, please refer to
    /// http://www.cvl.isy.liu.se/research/objrec/visualtracking/colvistrack/index.html.
    /// </summary>
    public class TrackerKCF : Tracker
    {
        /// <summary>
        /// Feature type to be used in the tracking grayscale, colornames, compressed color-names
        /// The modes available now:
        /// -   "GRAY" -- Use grayscale values as the feature
        /// -   "CN" -- Color-names feature
        /// </summary>
        public enum Mode
        {
            /// <summary>
            /// Grayscale
            /// </summary>
            GRAY = 1,
            /// <summary>
            /// Color
            /// </summary>
            CN = 2,
            /// <summary>
            /// Custom
            /// </summary>
            CUSTOM = 4
        }

        /// <summary>
        /// Creates a KCF Tracker
        /// </summary>
        /// <param name="detect_thresh">detection confidence threshold</param>
        /// <param name="sigma">gaussian kernel bandwidth</param>
        /// <param name="lambda">regularization</param>
        /// <param name="interp_factor">linear interpolation factor for adaptation</param>
        /// <param name="output_sigma_factor">spatial bandwidth (proportional to target)</param>
        /// <param name="pca_learning_rate">compression learning rate</param>
        /// <param name="resize">activate the resize feature to improve the processing speed</param>
        /// <param name="split_coeff">split the training coefficients into two matrices</param>
        /// <param name="wrap_kernel">wrap around the kernel values</param>
        /// <param name="compress_feature">activate the pca method to compress the features</param>
        /// <param name="max_patch_size">threshold for the ROI size</param>
        /// <param name="compressed_size">feature size after compression</param>
        /// <param name="desc_pca">compressed descriptors of TrackerKCF::MODE</param>
        /// <param name="desc_npca">non-compressed descriptors of TrackerKCF::MODE</param>
        public TrackerKCF(
            double detect_thresh = 0.5,
            double sigma = 0.2,
            double lambda = 0.01,
            double interp_factor = 0.075,
            double output_sigma_factor = 1.0/16.0,
            double pca_learning_rate = 0.15,
            bool resize = true,
            bool split_coeff = true,
            bool wrap_kernel = false,
            bool compress_feature = true,
            int max_patch_size = 80*80,
            int compressed_size = 2,
            Mode desc_pca = Mode.CN,
            Mode desc_npca = Mode.GRAY)
        {
            _ptr = ContribInvoke.cveTrackerKCFCreate(
                detect_thresh,
                sigma,
                lambda,
                interp_factor,
                output_sigma_factor,
                pca_learning_rate,
                resize,
                split_coeff,
                wrap_kernel,
                compress_feature,
                max_patch_size,
                compressed_size,
                (int)desc_pca,
                (int)desc_npca,
                ref _trackerPtr);
        }

        /// <summary>
        /// Release the unmanaged resources associated with this tracker
        /// </summary>
        protected override void DisposeObject()
        {
            if (IntPtr.Zero == _ptr)
                ContribInvoke.cveTrackerKCFRelease(ref _ptr);
            base.DisposeObject();
        }
    }

    public class TrackerGOTURN : Tracker
    {
        public TrackerGOTURN()
        {
            _ptr = ContribInvoke.cveTrackerGOTURNCreate(ref _trackerPtr);
        }

        /// <summary>
        /// Release the unmanaged resources associated with this tracker
        /// </summary>
        protected override void DisposeObject()
        {
            if (IntPtr.Zero == _ptr)
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
        internal static extern IntPtr cveTrackerKCFCreate(
            double detect_thresh,
            double sigma,
            double lambda,
            double interp_factor,
            double output_sigma_factor,
            double pca_learning_rate,
            bool resize,
            bool split_coeff,
            bool wrap_kernel,
            bool compress_feature,
            int max_patch_size,
            int compressed_size,
            int desc_pca,
            int desc_npca,
            ref IntPtr tracker);
        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern void cveTrackerKCFRelease(ref IntPtr tracker);

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern IntPtr cveTrackerGOTURNCreate(ref IntPtr tracker);
        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern void cveTrackerGOTURNRelease(ref IntPtr tracker);
    }
}
