//----------------------------------------------------------------------------
//  Copyright (C) 2004-2021 by EMGU Corporation. All rights reserved.       
//----------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Text;
using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Structure;
using Emgu.CV.Util;
using Emgu.Util;
using Emgu.CV.Features2D;

namespace Emgu.CV.XFeatures2D
{
    /// <summary>
    /// Class implementing the Harris-Laplace feature detector
    /// </summary>
    public class HarrisLaplaceFeatureDetector : Feature2D
    {
        /// <summary>
        /// Create a HarrisLaplaceFeatureDetector
        /// </summary>
        /// <param name="numOctaves">the number of octaves in the scale-space pyramid</param>
        /// <param name="cornThresh">the threshold for the Harris cornerness measure</param>
        /// <param name="DOGThresh">the threshold for the Difference-of-Gaussians scale selection</param>
        /// <param name="maxCorners">the maximum number of corners to consider</param>
        /// <param name="numLayers">the number of intermediate scales per octave</param>
        public HarrisLaplaceFeatureDetector(
            int numOctaves,
            float cornThresh,
            float DOGThresh,
            int maxCorners,
            int numLayers)
        {
            _ptr = XFeatures2DInvoke.cveHarrisLaplaceFeatureDetectorCreate(
                numOctaves,
                cornThresh,
                DOGThresh,
                maxCorners,
                numLayers, 
                ref _sharedPtr);
        }

        /// <summary>
        /// Release all the unmanaged resource associated with FREAK
        /// </summary>
        protected override void DisposeObject()
        {
            if (_sharedPtr != IntPtr.Zero)
            {
                XFeatures2DInvoke.cveHarrisLaplaceFeatureDetectorRelease(ref _sharedPtr);
            }

            base.DisposeObject();
        }
    }

    public static partial class XFeatures2DInvoke
    {

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal extern static IntPtr cveHarrisLaplaceFeatureDetectorCreate(
            int numOctaves,
            float cornThresh,
            float DOGThresh,
            int maxCorners,
            int numLayers, 
            ref IntPtr sharedPtr);

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal extern static void cveHarrisLaplaceFeatureDetectorRelease(ref IntPtr sharedPtr);
    }
}

