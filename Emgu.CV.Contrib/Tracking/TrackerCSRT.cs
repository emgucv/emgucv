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
    /// Discriminative Correlation Filter Tracker with Channel and Spatial Reliability
    /// </summary>
    public class TrackerCSRT : Tracker
    {
        private IntPtr _sharedPtr;

        /// <summary>
        /// Creates a CSRT tracker
        /// </summary>
        /// <param name="useHog">Use hog</param>
        /// <param name="useColorNames">Use color names</param>
        /// <param name="useGray">Use Gray</param>
        /// <param name="useRgb">Use RGB</param>
        /// <param name="useChannelWeights">Use channel weights</param>
        /// <param name="useSegmentation">Use segmentation</param>
        /// <param name="windowFunction">Windows function</param>
        /// <param name="kaiserAlpha">Kaiser alpha</param>
        /// <param name="chebAttenuation">Cheb attenuation</param>
        /// <param name="templateSize">Template size</param>
        /// <param name="gslSigma">Gsl Sigma</param>
        /// <param name="hogOrientations">Hog orientations</param>
        /// <param name="hogClip">Hog clip</param>
        /// <param name="padding">padding</param>
        /// <param name="filterLr">filter Lr</param>
        /// <param name="weightsLr">weights Lr</param>
        /// <param name="numHogChannelsUsed">Number of hog channels used</param>
        /// <param name="admmIterations">Admm iterations</param>
        /// <param name="histogramBins">Histogram bins</param>
        /// <param name="histogramLr">Histogram Lr</param>
        /// <param name="backgroundRatio">Background ratio</param>
        /// <param name="numberOfScales">Number of scales</param>
        /// <param name="scaleSigmaFactor">Scale Sigma factor</param>
        /// <param name="scaleModelMaxArea">Scale Model Max Area</param>
        /// <param name="scaleLr">Scale Lr</param>
        /// <param name="scaleStep">Scale step</param>
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
                _ptr = TrackingInvoke.cveTrackerCSRTCreate(
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
                    ref _trackerPtr,
                    ref _sharedPtr);
        }

        /// <summary>
        /// Release the unmanaged resources associated with this tracker
        /// </summary>
        protected override void DisposeObject()
        {
            if (IntPtr.Zero != _ptr)
                TrackingInvoke.cveTrackerCSRTRelease(ref _ptr, ref _sharedPtr);
            base.DisposeObject();
        }
    }

    public static partial class TrackingInvoke
    {
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