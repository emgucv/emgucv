//----------------------------------------------------------------------------
//  Copyright (C) 2004-2021 by EMGU Corporation. All rights reserved.       
//----------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using Emgu.CV.Structure;
using Emgu.CV.Text;
using Emgu.CV.Util;
using System.Diagnostics;

namespace Emgu.CV.Text
{
    /// <summary>
    /// Base class for 1st and 2nd stages of Neumann and Matas scene text detection algorithm
    /// </summary>
    public abstract class ERFilter : SharedPtrObject
    {
        /*
        /// <summary>
        /// The native pointer to the shared object.
        /// </summary>
        protected IntPtr _sharedPtr;

        static ERFilter()
        {
            CvInvoke.CheckLibraryLoaded();
        }*/

        /// <summary>
        /// Release all the unmanaged memory associate with this ERFilter
        /// </summary>
        protected override void DisposeObject()
        {
            if (_sharedPtr != IntPtr.Zero)
            {
                TextInvoke.cveERFilterRelease(ref _sharedPtr);
                _ptr = IntPtr.Zero;
            }
        }

        /// <summary>
        /// Takes image on input and returns the selected regions in a vector of ERStat only distinctive ERs which correspond to characters are selected by a sequential classifier
        /// </summary>
        /// <param name="image">Single channel image CV_8UC1</param>
        /// <param name="regions">Output for the 1st stage and Input/Output for the 2nd. The selected Extremal Regions are stored here.</param>
        public void Run(IInputArray image, VectorOfERStat regions)
        {
            using (InputArray iaImage = image.GetInputArray())
                TextInvoke.cveERFilterRun(_ptr, iaImage, regions);
        }

        /// <summary>
        /// The grouping method
        /// </summary>
        public enum GroupingMethod
        {
            /// <summary>
            /// Only perform grouping horizontally.
            /// </summary>
            OrientationHoriz,
            /// <summary>
            /// Perform grouping in any orientation.
            /// </summary>
            OrientationAny
        }

        /// <summary>
        /// Find groups of Extremal Regions that are organized as text blocks.
        /// </summary>
        /// <param name="image">The image where ER grouping is to be perform on</param>
        /// <param name="channels">Array of single channel images from which the regions were extracted</param>
        /// <param name="erstats">Vector of ER’s retrieved from the ERFilter algorithm from each channel</param>
        /// <param name="groupingTrainedFileName">The XML or YAML file with the classifier model (e.g. trained_classifier_erGrouping.xml)</param>
        /// <param name="minProbability">The minimum probability for accepting a group.</param>
        /// <param name="groupMethods">The grouping methods</param>
        /// <returns>The output of the algorithm that indicates the text regions</returns>
        public static System.Drawing.Rectangle[] ERGrouping(IInputArray image, IInputArrayOfArrays channels, VectorOfERStat[] erstats, GroupingMethod groupMethods = GroupingMethod.OrientationHoriz, String groupingTrainedFileName = null, float minProbability = 0.5f)
        {
            IntPtr[] erstatPtrs = new IntPtr[erstats.Length];

            for (int i = 0; i < erstatPtrs.Length; i++)
            {
                erstatPtrs[i] = erstats[i].Ptr;
            }

            using (VectorOfVectorOfPoint regionGroups = new VectorOfVectorOfPoint())
            using (VectorOfRect groupsBoxes = new VectorOfRect())
            using (InputArray iaImage = image.GetInputArray())
            using (InputArray iaChannels = channels.GetInputArray())
            using (CvString s = (groupingTrainedFileName == null ? new CvString() : new CvString(groupingTrainedFileName)))
            {
                GCHandle erstatsHandle = GCHandle.Alloc(erstatPtrs, GCHandleType.Pinned);
                TextInvoke.cveERGrouping(
                   iaImage, iaChannels,
                   erstatsHandle.AddrOfPinnedObject(), erstatPtrs.Length,
                   regionGroups, groupsBoxes,
                   groupMethods,
                   s, minProbability);

                erstatsHandle.Free();
                return groupsBoxes.ToArray();
            }
        }
    }

    /// <summary>
    /// Extremal Region Filter for the 1st stage classifier of N&amp;M algorithm
    /// </summary>
    public class ERFilterNM1 : ERFilter
    {
        /// <summary>
        /// Create an Extremal Region Filter for the 1st stage classifier of N&amp;M algorithm
        /// </summary>
        /// <param name="classifierFileName">The file name of the classifier</param>
        /// <param name="thresholdDelta">Threshold step in subsequent thresholds when extracting the component tree.</param>
        /// <param name="minArea">The minimum area (% of image size) allowed for retreived ER’s.</param>
        /// <param name="maxArea">The maximum area (% of image size) allowed for retreived ER’s.</param>
        /// <param name="minProbability">The minimum probability P(er|character) allowed for retreived ER’s.</param>
        /// <param name="nonMaxSuppression">Whenever non-maximum suppression is done over the branch probabilities.</param>
        /// <param name="minProbabilityDiff">The minimum probability difference between local maxima and local minima ERs.</param>
        public ERFilterNM1(
           String classifierFileName,
           int thresholdDelta = 1,
           float minArea = 0.00025f,
           float maxArea = 0.13f,
           float minProbability = 0.4f,
           bool nonMaxSuppression = true,
           float minProbabilityDiff = 0.1f)
        {
            using (CvString s = new CvString(classifierFileName))
                _ptr = TextInvoke.cveERFilterNM1Create(s, thresholdDelta, minArea, maxArea, minProbability, nonMaxSuppression, minProbabilityDiff, ref _sharedPtr);
        }

    }

    /// <summary>
    /// Extremal Region Filter for the 2nd stage classifier of N&amp;M algorithm
    /// </summary>
    public class ERFilterNM2 : ERFilter
    {
        /// <summary>
        /// Create an Extremal Region Filter for the 2nd stage classifier of N&amp;M algorithm
        /// </summary>
        /// <param name="classifierFileName">The file name of the classifier</param>
        /// <param name="minProbability">The minimum probability P(er|character) allowed for retreived ER’s.</param>
        public ERFilterNM2(String classifierFileName, float minProbability = 0.3f)
        {
            using (CvString s = new CvString(classifierFileName))
                _ptr = TextInvoke.cveERFilterNM2Create(s, minProbability, ref _sharedPtr);
        }

    }

    /// <summary>
    /// computeNMChannels operation modes
    /// </summary>
    public enum ERFilterNMMode
    {
        /// <summary>
        /// A combination of red (R), green (G), blue (B), lightness (L), and gradient
        /// magnitude (Grad).
        /// </summary>
        RGBLGrad,
        /// <summary>
        /// In N&amp;M algorithm, the combination of intensity (I), hue (H), saturation (S), and gradient magnitude
        /// channels (Grad) are used in order to obtain high localization recall. 
        /// </summary>
        IHSGrad
    }

    /// <summary>
    /// This class wraps the functional calls to the OpenCV Text modules
    /// </summary>
    public static partial class TextInvoke
    {
        static TextInvoke()
        {
            CvInvoke.Init();
        }

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern void cveERFilterRelease(ref IntPtr sharedPtr);

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern void cveERFilterRun(IntPtr filter, IntPtr image, IntPtr regions);

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern void cveERGrouping(
            IntPtr image, IntPtr channels,
            IntPtr regions, int count,
            IntPtr groups, IntPtr groupRects,
            ERFilter.GroupingMethod method, 
            IntPtr fileName, 
            float minProbability);

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern IntPtr cveERFilterNM1Create(
            IntPtr classifier,
            int thresholdDelta,
            float minArea,
            float maxArea,
            float minProbability,
            [MarshalAs(CvInvoke.BoolMarshalType)]
            bool nonMaxSuppression,
            float minProbabilityDiff,
            ref IntPtr sharedPtr);

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern IntPtr cveERFilterNM2Create(
           IntPtr classifier,
           float minProbability,
           ref IntPtr sharedPtr);

        /// <summary>
        /// Converts MSER contours (vector of point) to ERStat regions.
        /// </summary>
        /// <param name="image">Source image CV_8UC1 from which the MSERs where extracted.</param>
        /// <param name="contours">Input vector with all the contours (vector of Point).</param>
        /// <param name="regions">Output where the ERStat regions are stored.</param>
        public static void MSERsToERStats(
            IInputArray image,
            VectorOfVectorOfPoint contours,
            VectorOfVectorOfERStat regions)
        {
            using (InputArray iaImage = image.GetInputArray())
            {
                cveMSERsToERStats(iaImage, contours, regions);
            }
        }

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern void cveMSERsToERStats(
            IntPtr image,
            IntPtr contours,
            IntPtr regions);

        /// <summary>
        /// Compute the different channels to be processed independently in the N&amp;M algorithm.
        /// </summary>
        /// <param name="src">Source image. Must be RGB CV_8UC3.</param>
        /// <param name="channels">Output vector of Mat where computed channels are stored.</param>
        /// <param name="mode">Mode of operation</param>
        public static void ComputeNMChannels(IInputArray src, IOutputArrayOfArrays channels, ERFilterNMMode mode = ERFilterNMMode.RGBLGrad)
        {
            using (InputArray iaSrc = src.GetInputArray())
            using (OutputArray oaChannels = channels.GetOutputArray())
            {
                cveComputeNMChannels(iaSrc, oaChannels, mode);
            }
        }

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern void cveComputeNMChannels(IntPtr src, IntPtr channels, ERFilterNMMode mode);
    }

}

