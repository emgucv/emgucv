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
        /// <param name="detectThresh">detection confidence threshold</param>
        /// <param name="sigma">gaussian kernel bandwidth</param>
        /// <param name="lambda">regularization</param>
        /// <param name="interpFactor">linear interpolation factor for adaptation</param>
        /// <param name="outputSigmaFactor">spatial bandwidth (proportional to target)</param>
        /// <param name="pcaLearningRate">compression learning rate</param>
        /// <param name="resize">activate the resize feature to improve the processing speed</param>
        /// <param name="splitCoeff">split the training coefficients into two matrices</param>
        /// <param name="wrapKernel">wrap around the kernel values</param>
        /// <param name="compressFeature">activate the pca method to compress the features</param>
        /// <param name="maxPatchSize">threshold for the ROI size</param>
        /// <param name="compressedSize">feature size after compression</param>
        /// <param name="descPca">compressed descriptors of TrackerKCF::MODE</param>
        /// <param name="descNpca">non-compressed descriptors of TrackerKCF::MODE</param>
        public TrackerKCF(
            float detectThresh = 0.5f,
            float sigma = 0.2f,
            float lambda = 0.01f,
            float interpFactor = 0.075f,
            float outputSigmaFactor = 1.0f/16.0f,
            float pcaLearningRate = 0.15f,
            bool resize = true,
            bool splitCoeff = true,
            bool wrapKernel = false,
            bool compressFeature = true,
            int maxPatchSize = 80*80,
            int compressedSize = 2,
            Mode descPca = Mode.CN,
            Mode descNpca = Mode.GRAY)
        {
            _ptr = ContribInvoke.cveTrackerKCFCreate(
                detectThresh,
                sigma,
                lambda,
                interpFactor,
                outputSigmaFactor,
                pcaLearningRate,
                resize,
                splitCoeff,
                wrapKernel,
                compressFeature,
                maxPatchSize,
                compressedSize,
                descPca,
                descNpca,
                ref _trackerPtr);
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
    /// MOSSE Visual Object Tracking using Adaptive Correlation Filters
    /// </summary>
    /// <remarks>note, that this tracker works with grayscale images, if passed bgr ones, they will get converted internally.</remarks>
    public class TrackerMOSSE : Tracker
    {
        /// <summary>
        /// Create a MOSSE tracker
        /// </summary>
        public TrackerMOSSE()
        {
            _ptr = ContribInvoke.cveTrackerMOSSECreate(ref _trackerPtr);
        }

        /// <summary>
        /// Release the unmanaged resources associated with this tracker
        /// </summary>
        protected override void DisposeObject()
        {
            if (IntPtr.Zero != _ptr)
                ContribInvoke.cveTrackerMOSSERelease(ref _ptr);
            base.DisposeObject();
        }
    }




    /// <summary>
    /// Discriminative Correlation Filter Tracker with Channel and Spatial Reliability
    /// </summary>
    public class TrackerCSRT : Tracker
    {
        /// <summary>
        /// Creates a CSRT tracker
        /// </summary>
        public TrackerCSRT(             
            bool useHog = true,
            bool useColorNames = true,
            bool useGray = true,
            bool useRgb = false,
            bool useChannelWeights = true,
            bool useSegmentation = true,
            String windowFunction = null,
            float kaiserAlpha = 3.75f,
            float chebAttenuation = 45,
            float templateSize = 200,
            float gslSigma = 1.0f,
            float hogOrientations = 9,
            float hogClip = 0.2f,
            float padding = 3.0f,
            float filterLr = 0.02f,
            float weightsLr = 0.02f,
            int numHogChannelsUsed = 18,
            int admmIterations = 4,
            int histogramBins = 16,
            float histogramLr = 0.04f,
            int backgroundRatio = 2,
            int numberOfScales = 33,
            float scaleSigmaFactor = 0.250f,
            float scaleModelMaxArea = 512.0f,
            float scaleLr = 0.025f,
            float scaleStep = 1.020f
            )
        {
            using (CvString csWindowFunction = new CvString(windowFunction))
                _ptr = ContribInvoke.cveTrackerCSRTCreate(
                    useHog,
                    useColorNames,
                    useGray,
                    useRgb,
                    useChannelWeights,
                    useSegmentation,
                    csWindowFunction,
                    kaiserAlpha,
                    chebAttenuation,
                    templateSize,
                    gslSigma,
                    hogOrientations,
                    hogClip,
                    padding,
                    filterLr,
                    weightsLr,
                    numHogChannelsUsed,
                    admmIterations,
                    histogramBins,
                    histogramLr,
                    backgroundRatio,
                    numberOfScales,
                    scaleSigmaFactor,
                    scaleModelMaxArea,
                    scaleLr,
                    scaleStep,
                    ref _trackerPtr);
        }

        /// <summary>
        /// Release the unmanaged resources associated with this tracker
        /// </summary>
        protected override void DisposeObject()
        {
            if (IntPtr.Zero != _ptr)
                ContribInvoke.cveTrackerCSRTRelease(ref _ptr);
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
            float detect_thresh,
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
            ref IntPtr tracker);
        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern void cveTrackerKCFRelease(ref IntPtr tracker);

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern IntPtr cveTrackerGOTURNCreate(ref IntPtr tracker);
        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern void cveTrackerGOTURNRelease(ref IntPtr tracker);

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern IntPtr cveTrackerMOSSECreate(ref IntPtr tracker);
        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern void cveTrackerMOSSERelease(ref IntPtr tracker);

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
            ref IntPtr tracker);
        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern void cveTrackerCSRTRelease(ref IntPtr tracker);
    }
}
